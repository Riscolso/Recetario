using Recetario.Areas.Administradores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Recetario.BaseDatos;
using Microsoft.EntityFrameworkCore;

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
            var actor = new Actor
            {
                NombreActor = vactor.NombreActor,
                FechaNac = vactor.FechaNac,
                Tipo = vactor.Tipo,
                Usuario = vactor.Usuario,
                // TODO : Agregar Encriptación por AES
                Contrasena = Encoding.ASCII.GetBytes(vactor.Contrasena),
                Email = vactor.Email
            };
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
        public VActor Obtener(int? Id)
        {
            var actor = _contextoBD.Actor.Find(Id);
            return new VActor
            {
                IdActor = actor.IdActor,
                NombreActor = actor.NombreActor,
                FechaNac = actor.FechaNac,
                Tipo = actor.Tipo,
                Usuario = actor.Usuario,
                Contrasena = Convert.ToString(actor.Contrasena),
                Email = actor.Email

            };
        }

        /// <inheritdoc/>
        public ICollection<VActor> Obtener()
        {
            var actores =  _contextoBD.Actor.ToList();
            List<VActor> vactores = new List<VActor>();
            foreach(Actor actor in actores){
                vactores.Add(new VActor
                {
                    IdActor = actor.IdActor,
                    NombreActor = actor.NombreActor,
                    FechaNac = actor.FechaNac,
                    Tipo = actor.Tipo,
                    Usuario = actor.Usuario,
                    Contrasena = Convert.ToString(actor.Contrasena),
                    Email = actor.Email
                });
            }
            return vactores;
        }
        /// <inheritdoc/>
        public int Registrar(VActor vactor)
        {
            var actor = new Actor
            {
                NombreActor = vactor.NombreActor,
                FechaNac = vactor.FechaNac,
                Tipo = vactor.Tipo,
                Usuario = vactor.Usuario,
                // TODO : Agregar Encriptación por AES
                Contrasena = Encoding.ASCII.GetBytes(vactor.Contrasena),
                Email = vactor.Email
            };
            _contextoBD.Add(actor);
            _contextoBD.SaveChanges();
            return actor.IdActor;
        }
    }
}
