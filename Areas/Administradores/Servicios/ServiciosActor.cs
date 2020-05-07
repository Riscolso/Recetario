using Recetario.Areas.Administradores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Recetario.BaseDatos;
namespace Recetario.Areas.Administradores.Servicios
{
    public class ServiciosActor : IActor
    {
        private readonly ContextoBD _contextoBD;
        public ServiciosActor(ContextoBD contextoBD)
        {
            _contextoBD = contextoBD;
        }
        public int RegistrarActor(VActor vactor)
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
