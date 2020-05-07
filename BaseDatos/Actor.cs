using Recetario.Areas.Administradores.Models;
using System;
using System.Collections.Generic;

namespace Recetario.BaseDatos
{
    public partial class Actor
    {
        public Actor()
        {
            Receta = new HashSet<Receta>();
            Visualizacion = new HashSet<Visualizacion>();
        }

        public int IdActor { get; set; }
        public string NombreActor { get; set; }
        public DateTime FechaNac { get; set; }
        public int Tipo { get; set; }
        public string Usuario { get; set; }
        public byte[] Contrasena { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Receta> Receta { get; set; }
        public virtual ICollection<Visualizacion> Visualizacion { get; set; }
    }
}
