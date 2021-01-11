﻿using Recetario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Recetario.BaseDatos;
using Microsoft.EntityFrameworkCore;
using Recetario.Clases;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Recetario.Areas.Usuarios.Models;

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
            var recetas = _contextoBD.Receta.AsNoTracking()
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
            var recetas = _contextoBD.Receta.AsNoTracking()
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

        public (string Ingrediente, int Id)[] AnalizarIngredientes(string Listaingredientes)
        {
            //Separar los ingredientes
            var ingredientes = Listaingredientes.Split('.');
            var entidadesIngre = new List<EntityEntry<Ingrediente>>();
            //Una lista auxiliar para guardar los ingredientes y sus ID
            (string Ingrediente, int Id)[] ingredientesID = new (string Ingre, int valor)[ingredientes.Length];
            for (int i = 0; i < ingredientes.Length; i++)
            {
                //Obtener el ingrediente de una línea
                string ingrediente = NLP.ObtenerIngrediente(ingredientes[i]);
                //En caso de ser plural, pasar a singular
                ingrediente = NLP.Singular(ingrediente);
                var aux = _contextoBD.Ingrediente.AsNoTracking().FirstOrDefault(ing => ing.Nombre == ingrediente);
                var aux2 = entidadesIngre.FirstOrDefault(a => a.Entity.Nombre == ingrediente);
                //Agregar el ingrediente a la lista local
                // El Id = -1 namás es temporal
                ingredientesID[i] = (ingrediente, -1);
                //Checar si existe el ingrediente en la BD
                //En caso no estar en la BD y que tampoco exista un ingrediente igual en la receta
                if (aux == null && aux2 == null)
                {
                    //Si no existe, se agrega y se guardan los cambios
                    entidadesIngre.Add(_contextoBD.Ingrediente.Add(new Ingrediente { Nombre = ingrediente }));
                }
            }
            _contextoBD.SaveChanges();
            //Obtener los IDs de los ingredientes respecto a la BD
            for (int i = 0; i < ingredientesID.Length; i++)
            {
                //En caso de que el ingrediente, ya exista en la BD
                if (ingredientesID[i].Id == -1)
                {
                    ingredientesID[i].Id = _contextoBD.Ingrediente
                    .AsNoTracking().FirstOrDefault(ing => ing.Nombre == ingredientesID[i].Ingrediente).IdIngrediente;
                }
                else
                    ingredientesID[i].Id = entidadesIngre[i].Entity.IdIngrediente;
            }
            return ingredientesID;
        }

        public int Agregar(RecetaDTO recetadto, (string Ingrediente, int Id)[] ingredientesID)
        {
            List<Lleva> l = new List<Lleva>();
            var ingredientes = recetadto.Ingredientes.Split('.');
            for (int i = 0; i < ingredientesID.Length; i++)
            {
                l.Add(new Lleva
                {
                    //Se ligan los ingredientes con la receta
                    IngredienteIdIngrediente = ingredientesID[i].Id,
                    //Se agregan los ingredientes tal cual el usuario los escribió
                    //Estos son los que se muestran en la página
                    IngredienteCrudo = ingredientes[i].Trim(),
                    RecetaActorIdActor = recetadto.usuario.IdUsuario,
                });
            }
            /*------Código para Pasos------*/
            //Prepara el objeto
            var pasos = new List<Paso>();
            /*recetadto.Pasos.ForEach(p => pasos.Add(new Paso
            {
                NoPaso = p.NoPaso,
                Texto = p.Texto,
                TiempoTemporizador = DeStringAMinutos(p.TiempoTemporizador)
            }));*/

            //Agregar el objeto de receta con las etiquetas e ingredientes
            var receta = new Receta
            {
                ActorIdActor = recetadto.usuario.IdUsuario,
                Nombre = recetadto.Nombre.Trim(),
                TiempoPrep = recetadto.TiempoPrep,
                Usa = new List<Usa>(),
                Lleva = l,
                Paso = pasos
            };
            //Agregar en el contexto
            _contextoBD.Receta.Add(receta);
            //Reflejar cambios en la BD
            _contextoBD.SaveChanges();
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
        public void ListarCocinarDespues(int IdReceta, bool Estado, int IdUsuario)
        {
            //Obtener quién creó la receta
            int? IdCreador = _contextoBD.Receta
                .FirstOrDefault(r => r.IdReceta == IdReceta)
                .ActorIdActor;

            //Si es IdCreador es null, significa que no existe
            if (IdCreador == null) throw new NotImplementedException("La Receta o Usuario no existen =(");
            //Checar si ya hay una calificación de esta receta por este usuario
            var visu = _contextoBD.Visualizacion
                .FirstOrDefault(v => v.ActorIdActor == IdUsuario && v.RecetaIdReceta == IdReceta && v.RecetaActorIdActor == IdCreador);
            //Si ya se ha calificado antes 
            if (visu != null)
            {
                visu.PorCocinar = Estado;
                _contextoBD.Update(visu);

            }
            //Nueva calificación
            else
            {
                visu = new Visualizacion
                {
                    ActorIdActor = IdUsuario,
                    RecetaIdReceta = IdReceta,
                    RecetaActorIdActor = (int)IdCreador,
                    PorCocinar = Estado
                };
                _contextoBD.Add(visu);
            }
            _contextoBD.SaveChanges();
        }

        public RecetaModelo Obtener(int IdReceta, int IdUsuario)
        {
            //Obtener receta 
            var receta = Obtener(IdReceta);
            if (receta != null)
            {
                Visualizacion visualiza = null;
                //Si el usuario es 0, significa que es uno anónimo
                if (IdUsuario != 0)
                {
                    //Obtener visualización
                    visualiza = _contextoBD.Visualizacion
                    .FirstOrDefault(v => v.ActorIdActor == IdUsuario && v.RecetaIdReceta == IdReceta && v.RecetaActorIdActor == receta.usuario.IdUsuario);
                }
                //Si no existe solo se crea una
                if (visualiza == null)
                {
                    visualiza = new Visualizacion()
                    {
                        ActorIdActor = (int)IdUsuario,
                        RecetaIdReceta = receta.IdReceta,
                        RecetaActorIdActor = receta.usuario.IdUsuario,
                        PorCocinar = false
                    };
                }
                return new RecetaModelo
                {
                    Receta = receta,
                    Visualizar = visualiza
                };
            }
            else throw new NotImplementedException("La receta no se encuentra en la BD");
        }

        public ICollection<RecetaDTO> ObtenerPendientes(int IdUsuario)
        {
            //No se regresan todos los valores de las recetas
            //Solo los que se van a mostrar en la lista de pentientes
            return _contextoBD.Visualizacion
                .Where(v => v.PorCocinar && v.ActorIdActor == IdUsuario)
                .Include(v => v.Receta)
                .Select(v => new RecetaDTO
                {
                    IdReceta = v.Receta.IdReceta,
                    Nombre = v.Receta.Nombre,
                    TiempoPrep = v.Receta.TiempoPrep,
                }
                ).ToList();
            throw new NotImplementedException();
        }
        public ICollection<RecetaDTO> ObtenerPendientes(int IdUsuario, string Filtro)
        {
            //No se regresan todos los valores de las recetas
            //Solo los que se van a mostrar en la lista de pentientes
            return _contextoBD.Visualizacion
                .Include(v => v.Receta)
                .Where(v => v.PorCocinar
                    && v.ActorIdActor == IdUsuario
                    && v.Receta.Nombre.ToLower().Contains(Filtro.ToLower()))
                .Select(v => new RecetaDTO
                {
                    IdReceta = v.Receta.IdReceta,
                    Nombre = v.Receta.Nombre,
                    TiempoPrep = v.Receta.TiempoPrep,
                }
                ).ToList();
            throw new NotImplementedException();
        }
    }
}
