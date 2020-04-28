using System;
using System.Collections.Generic;

namespace Recetario.BaseDatos
{
    public partial class Actor
    {
        public Actor()
        {
            Correo = new HashSet<Correo>();
            Receta = new HashSet<Receta>();
            Visualizacion = new HashSet<Visualizacion>();
        }

        public int IdActor { get; set; }
        public string NombreActor { get; set; }
        public DateTime FechaNac { get; set; }
        public bool Tipo { get; set; }

        public virtual ICollection<Correo> Correo { get; set; }
        public virtual ICollection<Receta> Receta { get; set; }
        public virtual ICollection<Visualizacion> Visualizacion { get; set; }
    }
}
