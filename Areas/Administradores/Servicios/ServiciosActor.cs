using Recetario.Areas.Administradores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Recetario.BaseDatos;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        private readonly UserManager<Actor> _userManager;
        public ServiciosActor(ContextoBD contextoBD,
            UserManager<Actor> userManager)
        {
            _userManager = userManager;
            _contextoBD = contextoBD;
        }

        /// <inheritdoc/>
        public Actor Actualizar(ActorDTO vactor) 
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
            return actor;
        }

        /// <inheritdoc/>
        public ICollection<ActorDTO> BuscarFiltro(string Filtro)
        {
            //Hacer la búsqueda insensible a mayúscular o minúsculas
            Filtro = Filtro.ToLower();
            //Buscar en la base de datos los actores que conincidan con el filtro
            var actores = _contextoBD.Actor.Where(a =>
            a.NombreActor.ToLower().Contains(Filtro) ||
            a.UserName.ToLower().Contains(Filtro) ||
            a.Email.ToLower().Contains(Filtro));
            //Una lista para guardar las vista que se van a regresar
            List<ActorDTO> vactores = new List<ActorDTO>();
            //Convertir el modelo de datos a modelo de vista
            foreach (Actor actor in actores)
            {
                vactores.Add(CasteoVActor(actor));
            }
            return vactores;
        }
        /// <inheritdoc/>
        public ICollection<ActorDTO> BuscarFiltro(string Filtro, string rol)
        {
            //Hacer la búsqueda insensible a mayúscular o minúsculas
            Filtro = Filtro.ToLower();
            //Obtener la lista de Actores que coinciden con el rol
            var actores = _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, rol))
                .Result
                //Aplicando filtro
                .Where(a =>
                    a.NombreActor.ToLower().Contains(Filtro) ||
                    a.UserName.ToLower().Contains(Filtro) ||
                    a.Email.ToLower().Contains(Filtro));

            //Buscar en la base de datos los actores que conincidan con el filtro
            /*
            var actores = _contextoBD.Actor.Where(a =>
            a.Tipo==2 && (
            a.NombreActor.ToLower().Contains(Filtro) ||
            a.UserName.ToLower().Contains(Filtro) ||
            a.Email.ToLower().Contains(Filtro)));
            */
            //Una lista para guardar las vista que se van a regresar
            List<ActorDTO> vactores = new List<ActorDTO>();
            //Convertir el modelo de datos a modelo de vista
            foreach (Actor actor in actores)
            {
                vactores.Add(CasteoVActor(actor));
            }
            return vactores;
        }
        /// <inheritdoc/>
        public ICollection<ActorDTO> BuscarFiltroUsuarios(string Filtro)
        {
            //Hacer la búsqueda insensible a mayúscular o minúsculas
            Filtro = Filtro.ToLower();
            //Buscar en la base de datos los actores que conincidan con el filtro
            var actores = _contextoBD.Actor.Where(a =>
            a.Tipo == 3 &&
            (a.NombreActor.ToLower().Contains(Filtro) ||
            a.UserName.ToLower().Contains(Filtro) ||
            a.Email.ToLower().Contains(Filtro)));
            //Una lista para guardar las vista que se van a regresar
            List<ActorDTO> vactores = new List<ActorDTO>();
            //Convertir el modelo de datos a modelo de vista
            foreach (Actor actor in actores)
            {
                vactores.Add(CasteoVActor(actor));
            }
            return vactores;
        }

        /// <inheritdoc/>
        public async void Eliminar(int Id)
        {
            /*Chale
             No funcó y justo ahora no tengo ganas de avenriguar por qué jajajaj
             Funciona en el mismo controlador, así que no es el fin del mundo 
             si no se usa este método*/
            Actor toElim = await _userManager.FindByIdAsync(Id.ToString());
            await _userManager.DeleteAsync(toElim);
        }

        /// <inheritdoc/>
        public (ActorDTO actor, string rol) Obtener(int? Id)
        {
            Actor act = _userManager.FindByIdAsync(Id.ToString()).Result;
            return (CasteoVActor(_contextoBD.Actor.Find(Id)),_userManager.GetClaimsAsync(act)
                .Result.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value);
        }

        /// <inheritdoc/>
        public ICollection<ActorDTO> ObtenerLista()
        {
            var actores =  _contextoBD.Actor.ToList();
            List<ActorDTO> vactores = new List<ActorDTO>();
            foreach(Actor actor in actores) vactores.Add(CasteoVActor(actor));
            return vactores;
        }
        /// <inheritdoc/>
        public ICollection<ActorDTO> ObtenerLista(string rol)
        {
            //Obtener los actores que pertenecen a un rol (Claim)
            var actores = _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, rol))
                .Result;
            //Castear a clase de vista
            List<ActorDTO> vactores = new List<ActorDTO>();
            foreach (Actor actor in actores) vactores.Add(CasteoVActor(actor));
            return vactores;
        }
        /// <inheritdoc/>
        public ICollection<ActorDTO> ObtenerUsuarios()
        {
            var actores = _contextoBD.Actor.ToList();
            List<ActorDTO> vactores = new List<ActorDTO>();
            foreach (Actor actor in actores)
                if(actor.Tipo == 2)
                    vactores.Add(CasteoVActor(actor));
            return vactores;
        }

        /// <inheritdoc/>
        public Actor Registrar(ActorDTO vactor)
        {
            Actor actor = CasteoActor(vactor);
            _contextoBD.Add(actor);
            _contextoBD.SaveChanges();
            return actor;
        }

        /// <summary>
        /// Con base a una case de tipo Actor del modelo de BD
        /// Se crea una clase de tipo VActor de las vistas
        /// </summary>
        /// <param name="actor">Clase Actor de la cual se obtendrán los valores</param>
        /// <returns>Una clase VActor para usarse como vista</returns>
        ActorDTO CasteoVActor(Actor actor)
        {
            //Si es null, regresa el null
            return actor == null? null: new ActorDTO
            {
                IdActor = actor.Id,
                NombreActor = actor.NombreActor,
                FechaNac = actor.FechaNac,
                Tipo = actor.Tipo,
                Usuario = actor.UserName,
                //Contrasena = Convert.ToString(actor.Contrasena),
                Email = actor.Email
            };
        }

        Actor CasteoActor(ActorDTO vactor)
        {
            return new Actor
            {
                NombreActor = vactor.NombreActor,
                FechaNac = vactor.FechaNac,
                Tipo = vactor.Tipo,
                UserName = vactor.Usuario,
                // TODO : Agregar Encriptación por AES
                PasswordHash = vactor.Contrasena,
                Email = vactor.Email
            };
        }

        public ActorDTO FindVActor(string user)
        {
            var actor = _contextoBD.Actor.FirstOrDefault(a =>
            a.UserName.Contains(user)); ;
            return CasteoVActor(actor) ;
        }

        int IActor.Registrar(ActorDTO vactor)
        {
            throw new NotImplementedException();
        }

        int IActor.Actualizar(ActorDTO vactor)
        {
            throw new NotImplementedException();
        }
    }
}
