﻿using Recetario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Recetario.BaseDatos;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Recetario.Areas.Administradores.Servicios
{
    public class ServiciosReceta : IReceta
    {
        private readonly ContextoBD _contextoBD;
        public ServiciosReceta(ContextoBD contextoBD)
        {
            _contextoBD = contextoBD;
        }
        /// <inheritdoc/>
        public ICollection<RecetaDTO> BuscarFiltro(string Filtro)
        {
            int idCreador = 0;
            //Hacer la búsqueda insensible a mayúscular o minúsculas
            Filtro = Filtro.ToLower();

            //Buscar posible usuario creador en el filtro
            var creador = _contextoBD.Actor.FirstOrDefault(a =>
            a.NombreActor.ToLower().Contains(Filtro));
            //Obtener Ids del usuario creador
            if (creador != null)
                idCreador = creador.Id;

            //Buscar en la base de datos las recetas que conincidan con el filtro
            var recetas = _contextoBD.Receta
                .Include(r => r.ActorIdActorNavigation)
                .Where(r =>
            r.Nombre.ToLower().Contains(Filtro) ||
            r.ActorIdActor == idCreador);
            //Se convierte el IQueryable en lista de recetas
            List<Receta> listaRecetas = recetas.ToList();
            //Una lista para guardar las vistas que se van a regresar
            List<RecetaDTO> vrecetas = new List<RecetaDTO>();
            //Convertir el modelo de datos a modelo de vista
            foreach (Receta receta in listaRecetas)
            {
                vrecetas.Add(CasteoVReceta(receta));
            }
            return vrecetas;
        }

        public ICollection<RecetaDTO> BuscarFiltro(string Filtro, int IdUsuario)
        {
            //Hacer la búsqueda insensible a mayúscular o minúsculas
            Filtro = Filtro.ToLower();

            //Buscar en la base de datos las recetas que conincidan con el filtro
            var recetas = _contextoBD.Receta
                .Include(r => r.ActorIdActorNavigation)
                .Where(r =>
            r.Nombre.ToLower().Contains(Filtro) &&
            r.ActorIdActor == IdUsuario);

            //Se convierte el IQueryable en lista de recetas
            List<Receta> listaRecetas = recetas.ToList();
            //Una lista para guardar las vistas que se van a regresar
            List<RecetaDTO> vrecetas = new List<RecetaDTO>();
            //Convertir el modelo de datos a modelo de vista
            foreach (Receta receta in listaRecetas)
            {
                vrecetas.Add(CasteoVReceta(receta));
            }
            return vrecetas;
        }

        /// <inheritdoc/>
        public void Eliminar(int Id)
        {
            //Traer a la receta y sus tablas dependientes de la BD
            var receta = _contextoBD.Receta
                .Include(r => r.Usa)
                .Include(r => r.Lleva)
                .Include(r => r.Paso)
                .FirstOrDefault(r => r.IdReceta == Id)
                ;
            //Sacarla del contexto
            _contextoBD.Receta.Remove(receta);
            //Aplicar los cambios a la BD
            _contextoBD.SaveChanges();
        }

        /// <inheritdoc/>
        public RecetaDTO Obtener(int Id)
        {
            var aux = _contextoBD.Receta
                .Include(r => r.ActorIdActorNavigation)
                .Include(r => r.Paso)
                .Include(r => r.Usa)
                    .ThenInclude(u => u.EtiquetaIdEtiquetaNavigation)
                .Select(r => new RecetaDTO
                {
                    //LLenar la información del usuario que creó la receta
                    //No se llenan todos los datos, por que no se usan
                    usuario = new UsuarioDTO
                    {
                        IdUsuario = r.ActorIdActor,
                        Usuario = r.ActorIdActorNavigation.UserName
                    },
                    IdReceta = r.IdReceta,
                    Nombre = r.Nombre,
                    TiempoPrep = r.TiempoPrep,
                    //Traer todas las etiquetas de la BD e irlas pegando con un espacio
                    Etiquetas = string.Join(", ", r.Usa.Select(u => u.EtiquetaIdEtiquetaNavigation.Etiqueta1)),
                    //Lo mimsmo para los ingredientes
                    Ingredientes = string.Join(", ", r.Lleva.Select(l => l.IngredienteCrudo)),
                    Pasos = r.Paso.Select(p => new PasoDTO
                    {
                        NoPaso = p.NoPaso,
                        Texto = p.Texto,
                        TiempoTemporizador = DeMinutosAString(p.TiempoTemporizador)
                    }).ToList()
                }).FirstOrDefault(r => r.IdReceta == Id);
            return aux;
        }

        /// <inheritdoc/>
        public ICollection<RecetaDTO> Obtener()
        {
            var recetas = _contextoBD.Receta
                .Include(r => r.ActorIdActorNavigation).ToList();
            List<RecetaDTO> vrecetas = new List<RecetaDTO>();
            foreach (Receta receta in recetas) vrecetas.Add(CasteoVReceta(receta));
            return vrecetas;
        }

        public ICollection<RecetaDTO> ObtenerXUsuario(int IdUsuario)
        {
            var recetas = _contextoBD.Receta
                .Include(r => r.ActorIdActorNavigation)
                .Where(r => r.ActorIdActor == IdUsuario)
                .ToList();
            List<RecetaDTO> vrecetas = new List<RecetaDTO>();
            foreach (Receta receta in recetas) vrecetas.Add(CasteoVReceta(receta));
            return vrecetas;
        }

        public RecetaDTO CasteoVReceta(Receta receta)
        {
            return new RecetaDTO
            {
                IdReceta = receta.IdReceta,
                Nombre = receta.Nombre,
                ProcentajePromedio = receta.ProcentajePromedio,
                TiempoPrep = receta.TiempoPrep,
                usuario = new UsuarioDTO { 
                    IdUsuario = receta.ActorIdActor,
                    Usuario = receta.ActorIdActorNavigation.NombreActor
                },
                //ActorIdActor = receta.ActorIdActor,
                //ActorNombreActor = _contextoBD.Actor.Find(receta.ActorIdActor).NombreActor
            };
        }

        public int Agregar(RecetaDTO recetadto)
        {
            /*------Código para Ingredientes y Etiquetas------*/
            //Checar si las etiquetas coinciden con una existente
            //TODO: Agregar NLP
            //TODO: Minusculas, normalizar
            //Separar las etiquetas
            var etiquetas = recetadto.Etiquetas.Split(',');
            //Separar los ingredientes
            var ingredientes = recetadto.Ingredientes.Split(',');
            var tagsID = new List<int>();
            var ingID = new List<int>();
            //Lista de las entidades de la BD para tags
            var eT = new List<EntityEntry<Etiqueta>>();
            //Lista de las entidades de la BD para ingredientes
            var eI = new List<EntityEntry<Ingrediente>>();
            foreach (string etiqueta in etiquetas)
            {
                var aux = _contextoBD.Etiqueta
                    .FirstOrDefault(e => e.Etiqueta1 == etiqueta);
                //Checar si existe con una de la BD, si no para agregarla
                if (aux == null)
                {
                    //Si no existe se agrega a la BD y a una lista de tipo entidad de EF
                    eT.Add(_contextoBD.Etiqueta.Add(new Etiqueta
                    {
                        Etiqueta1 = etiqueta
                    }));
                }
                else
                {
                    //Si existe solo agregar a una lista de etiquetas
                    tagsID.Add(aux.IdEtiqueta);
                }
            }
            // TODO: Aquí va la etapa de NLP
            //Lo mismo, pero para ingredientes xD
            foreach (string ingrediente in ingredientes)
            {
                var aux = _contextoBD.Ingrediente
                    .FirstOrDefault(ingr => ingr.Nombre == ingrediente);
                //Checar si existe con una de la BD, si no para agregarla
                if (aux == null)
                {
                    //Si no existe se agrega a la BD y a una lista de tipo entidad de EF
                    eI.Add(_contextoBD.Ingrediente.Add(new Ingrediente
                    {
                        Nombre = ingrediente
                    }));
                }
                else
                {
                    //Si existe solo agregar a una lista de etiquetas
                    ingID.Add(aux.IdIngrediente);
                }
            }

            /*La mágia ocurre aquí, EF funciona de tal manera, que al momento de salvar los
             cambios, automáticamente las entidades que guardamos en 'e' son rastreadas por EF
            Así que ahora esas entidades tienen sus valores tal cual como en la BD, pero lo que nos interesa
            de esto, es su ID*/
            _contextoBD.SaveChanges();
            //Agregar las etiquetas se se sumaron a la BD a nuestra lista de
            //etiquetas para esta receta
            foreach (EntityEntry<Etiqueta> tag in eT)
            {
                tagsID.Add(tag.Entity.IdEtiqueta);
            }
            //Se agregan también los ingredientes crudos
            foreach (EntityEntry<Ingrediente> ingr in eI)
            {
                ingID.Add(ingr.Entity.IdIngrediente);
            }
            //Preparar la lista de Usa
            List<Usa> u = new List<Usa>();
            tagsID.ForEach(t => u.Add(new Usa
            {
                EtiquetaIdEtiqueta = t
            }));

            List<Lleva> l = new List<Lleva>();
            for (int i = 0; i < eI.Count; i++)
            {
                l.Add(new Lleva
                {
                    //Se ligan los ingredientes con la receta
                    IngredienteIdIngrediente = ingID[i],
                    //Se agregan los ingredientes tal cual el usuario los escribió
                    //Estos son los que se muestran en la página
                    IngredienteCrudo = ingredientes[i] + "crudo"
                });
            }

            /*------Código para Pasos------*/
            //Prepara el objeto
            var pasos = new List<Paso>();
            recetadto.Pasos.ForEach(p => pasos.Add(new Paso
            {
                NoPaso = p.NoPaso,
                Texto = p.Texto,
                TiempoTemporizador = DeStringAMinutos(p.TiempoTemporizador)
            }));

            //Agregar el objeto de receta con las etiquetas e ingredientes
            var receta = new Receta
            {
                ActorIdActor = recetadto.usuario.IdUsuario,
                Nombre = recetadto.Nombre,
                TiempoPrep = recetadto.TiempoPrep,
                Usa = u,
                Lleva = l,
                Paso = pasos
            };
            //Agregar en el contexto
            _contextoBD.Receta.Add(receta);
            //Reflejar cambios en la BD
            _contextoBD.SaveChanges();
            //Regresa el id de la receta
            return receta.IdReceta;
        }

        //TODO: Aceptar segundos y convertirlo todo a segundos
        /// <summary>
        /// Convierte un tipo de dato cadena con formato "hh:mm" a minutos en int
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static int? DeStringAMinutos(string cadena)
        {
            if (cadena != null)
            {
                var aux = cadena.Split(':');
                return (Convert.ToInt32(aux[0]) * (60)) + Convert.ToInt32(aux[1]);
            }
            else return null;
        }
        //Se usa static para evitar un desbordamiento de memoria
        public static string DeMinutosAString(int? minutos)
        {
            if (minutos != null)
            {
                var hor = ((int) minutos / 60).ToString();
                var min = (minutos % 60).ToString();
                return Formatohora(hor) + ":" + Formatohora(min);
            }
            else return null;
        }
        public static string Formatohora(string v)
        {
            if (v.Length == 1) return "0" + v;
            else return v;
        }

        public int Editar(RecetaDTO recetadto)
        {
            //Buscar el obejto en la BD
            var receta = _contextoBD.Receta
                //Traer también las tablas relacionadas
                .Include(r => r.Paso)
                .Include(r => r.Lleva)
                .Include(r => r.Usa)
                .SingleOrDefault(r => r.IdReceta == recetadto.IdReceta);
            //Verificar que exista
            if (receta == null) throw new NotImplementedException("No existe el objeto en la BD");
            //Realizar los cambios
            //TODO: Agregar lo de editar etiquetas, hacer lo mismo que en crear
            receta.Nombre = recetadto.Nombre;
            receta.ProcentajePromedio = recetadto.ProcentajePromedio;
            receta.TiempoPrep = recetadto.TiempoPrep;
            receta.Paso = recetadto.Pasos.Select(p => new Paso
            {
                NoPaso = p.NoPaso,
                Texto = p.Texto,
                TiempoTemporizador = DeStringAMinutos(p.TiempoTemporizador)
            }
            ).ToList();
            //Actualizar los cambios
            _contextoBD.Update(receta);
            _contextoBD.SaveChanges();
            return receta.IdReceta;
        }

        public void Calificar(int IdReceta, bool Gustar, int Idusuario)
        {
            //Obtener quién creó la receta
            int? IdCreador = _contextoBD.Receta
                .FirstOrDefault(r => r.IdReceta == IdReceta)
                .ActorIdActor;
            
            //Si es IdCreador es null, significa que no existe
            if (IdCreador == null) throw new NotImplementedException("La Receta o Usuario no existen =(");
            //Checar si ya hay una calificación de esta receta por este usuario
            var visu = _contextoBD.Visualizacion
                .FirstOrDefault(v => v.ActorIdActor == Idusuario && v.RecetaIdReceta == IdReceta && v.RecetaActorIdActor == IdCreador);
            //Si ya se ha calificado antes 
            if (visu != null)
            {
                visu.Calificacion = Gustar;
                _contextoBD.Update(visu);
                
            }
            //Nueva calificación
            else
            {
                visu = new Visualizacion
                {
                    ActorIdActor = Idusuario,
                    RecetaIdReceta = IdReceta,
                    RecetaActorIdActor = (int)IdCreador,
                    Calificacion = Gustar
                };
                _contextoBD.Add(visu);
            }
            _contextoBD.SaveChanges();
        }
    }
}
