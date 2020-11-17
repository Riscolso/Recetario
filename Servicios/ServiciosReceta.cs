using Recetario.Areas.Administradores.Models;
using Recetario.Areas.Usuarios.Models;
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
            if(creador != null)
                idCreador = creador.Id;

            //Buscar en la base de datos las recetas que conincidan con el filtro
            var recetas = _contextoBD.Receta.Where(r =>
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

        /// <inheritdoc/>
        public void Eliminar(int Id)
        {
            //Traer a la receta y sus tablas dependientes de la BD
            var receta = _contextoBD.Receta
                .Include(r => r.Usa)
                .Include(r => r.Lleva)
                .Include(r=> r.Paso)
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
            return CasteoVReceta(_contextoBD.Receta.FirstOrDefault(r => r.IdReceta == Id));
        }

        /// <inheritdoc/>
        public ICollection<RecetaDTO> Obtener()
        {
            var recetas = _contextoBD.Receta.ToList();
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
                ActorIdActor = receta.ActorIdActor,
                ActorNombreActor = _contextoBD.Actor.Find(receta.ActorIdActor).NombreActor
            };
        }

        public int Agregar(RecetaUDTO recetadto)
        {
            //Checar si las etiquetas coinciden con una existente
            //TODO: Agregar NLP
            //TODO: Minusculas, normalizar
            //Separar las etiquetas
            var etiquetas = recetadto.Etiquetas.Split(' ');
            var tagsID = new List<int>();
            //Lista de las entidades de la BD
            List<EntityEntry<Etiqueta>> e = new List<EntityEntry<Etiqueta>>();
            foreach (string etiqueta in etiquetas)
            {
                var aux = _contextoBD.Etiqueta
                    .FirstOrDefault(e => e.Etiqueta1 == etiqueta);
                //Checar si existe con una de la BD, si no para agregarla
                if (aux == null)
                {
                    //Si no existe se agrega a la BD y a una lista de tipo entidad de EF
                    e.Add(_contextoBD.Etiqueta.Add(new Etiqueta
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
            /*La mágia ocurre aquí, EF funciona de tal manera, que al momento de salvar los
             cambios, automáticamente las entidades que guardamos en 'e' son rastreadas por EF
            Así que ahora esas entidades tienen sus valores tal cual como en la BD, pero lo que nos interesa
            de esto, es su ID*/
            _contextoBD.SaveChanges();
            //Agregar las etiquetas se se sumaron a la BD a nuestra lista de
            //etiquetas para esta receta
            foreach(EntityEntry<Etiqueta> tag in e)
            {
                tagsID.Add(tag.Entity.IdEtiqueta);
            }
            //Preparar la lista de Usa
            List<Usa> u = new List<Usa>();
            tagsID.ForEach(t => u.Add(new Usa
            {
                EtiquetaIdEtiqueta = t
            }));
            //Agregar el objeto de receta con las etiquetas
            _contextoBD.Receta.Add(
                new Receta
                {
                    ActorIdActor = recetadto.idUsuario,
                    Nombre = recetadto.Nombre,
                    TiempoPrep = recetadto.TiempoPrep,
                    Usa = u
                }
                );
            //Regresa el número de objetos que se modificaron en el save
            return _contextoBD.SaveChanges();
        }
    }
}
