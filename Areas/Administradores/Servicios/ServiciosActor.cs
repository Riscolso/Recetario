using Recetario.Areas.Administradores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Recetario.BaseDatos;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

// TODO: Agregar más TryCatch en caso de que muera la wea
// TODO: Crear y usar funciones para convertir las clases Actor -> Vactor y viceversa

namespace Recetario.Areas.Administradores.Servicios
{
    /// <summary>
    /// Clase que hereda de la clase IActor, maneja los métodos abstractos con un contexto de Base de Datos de MySQL.
    /// </summary>
    public class ServiciosActor : IActor
    {
        private readonly ContextoBD _contextoBD;
        public ServiciosActor(ContextoBD contextoBD)
        {
            _contextoBD = contextoBD;
        }

        /// <inheritdoc/>
        public int Actualizar(VActor vactor) 
        {
            var actor = CasteoActor(vactor);
            try
            {
                _contextoBD.Update(actor);
                _contextoBD.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return actor.IdActor;
        }

        /// <inheritdoc/>
        public ICollection<VActor> BuscarFiltro(string Filtro)
        {
            //Hacer la búsqueda insensible a mayúscular o minúsculas
            Filtro = Filtro.ToLower();
            //Buscar en la base de datos los actores que conincidan con el filtro
            var actores = _contextoBD.Actor.Where(a =>
            a.NombreActor.ToLower().Contains(Filtro) ||
            a.Usuario.ToLower().Contains(Filtro) ||
            a.Email.ToLower().Contains(Filtro));
            //Una lista para guardar las vista que se van a regresar
            List<VActor> vactores = new List<VActor>();
            //Convertir el modelo de datos a modelo de vista
            foreach (Actor actor in actores)
            {
                vactores.Add(CasteoVActor(actor));
            }
            return vactores;
        }
        /// <inheritdoc/>
        public ICollection<VActor> BuscarFiltroUsuarios(string Filtro)
        {
            //Hacer la búsqueda insensible a mayúscular o minúsculas
            Filtro = Filtro.ToLower();
            //Buscar en la base de datos los actores que conincidan con el filtro
            var actores = _contextoBD.Actor.Where(a =>
            a.Tipo == 3 &&
            (a.NombreActor.ToLower().Contains(Filtro) ||
            a.Usuario.ToLower().Contains(Filtro) ||
            a.Email.ToLower().Contains(Filtro)));
            //Una lista para guardar las vista que se van a regresar
            List<VActor> vactores = new List<VActor>();
            //Convertir el modelo de datos a modelo de vista
            foreach (Actor actor in actores)
            {
                vactores.Add(CasteoVActor(actor));
            }
            return vactores;
        }

        /// <inheritdoc/>
        public void Eliminar(int Id)
        {
            //Traer a el actor de la BD
            var actor = _contextoBD.Actor.Find(Id);
            //Sacarlo del contexto
            _contextoBD.Actor.Remove(actor);
            //Aplicar los cambios a la BD
            _contextoBD.SaveChanges();
        }

        /// <inheritdoc/>
        public VActor Obtener(int? Id)
        {
            return CasteoVActor(_contextoBD.Actor.Find(Id));
        }

        /// <inheritdoc/>
        public ICollection<VActor> Obtener()
        {
            var actores =  _contextoBD.Actor.ToList();
            List<VActor> vactores = new List<VActor>();
            foreach(Actor actor in actores) vactores.Add(CasteoVActor(actor));
            return vactores;
        }
        /// <inheritdoc/>
        public ICollection<VActor> ObtenerUsuarios()
        {
            var actores = _contextoBD.Actor.ToList();
            List<VActor> vactores = new List<VActor>();
            foreach (Actor actor in actores)
                if(actor.Tipo == 3)
                    vactores.Add(CasteoVActor(actor));
            return vactores;
        }

        /// <inheritdoc/>
        public int Registrar(VActor vactor)
        {
            Actor actor = CasteoActor(vactor);
            _contextoBD.Add(actor);
            _contextoBD.SaveChanges();
            return actor.IdActor;
        }

        /// <summary>
        /// Con base a una case de tipo Actor del modelo de BD
        /// Se crea una clase de tipo VActor de las vistas
        /// </summary>
        /// <param name="actor">Clase Actor de la cual se obtendrán los valores</param>
        /// <returns>Una clase VActor para usarse como vista</returns>
        VActor CasteoVActor(Actor actor)
        {
            return new VActor
            {
                IdActor = actor.IdActor,
                NombreActor = actor.NombreActor,
                FechaNac = actor.FechaNac,
                Tipo = actor.Tipo,
                Usuario = actor.Usuario,
                //Contrasena = Convert.ToString(actor.Contrasena),
                Email = actor.Email
            };
        }

        Actor CasteoActor(VActor vactor)
        {
            return new Actor
            {
                IdActor = vactor.IdActor,
                NombreActor = vactor.NombreActor,
                FechaNac = vactor.FechaNac,
                Tipo = vactor.Tipo,
                Usuario = vactor.Usuario,
                // TODO : Agregar Encriptación por AES
                Contrasena = Encoding.ASCII.GetBytes(vactor.Contrasena),
                Email = vactor.Email
            };
        }
        public VActor AppUserToVActor(AppUser user)
        {
            return CasteoVActor(_contextoBD.Actor.Find(user.Id));
        }

        public VActor FindVActor(string user)
        {
            var actor = _contextoBD.Actor.FirstOrDefault(a =>
            a.Usuario.Contains(user)); ;
            return CasteoVActor(actor) ;
        }
    }
}
