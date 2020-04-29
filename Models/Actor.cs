using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Nombre")]
        public string NombreActor { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNac { get; set; }
        public bool Tipo { get; set; }
        public string Usuario { get; set; }
        [Display(Name = "Contraseña")]
        public byte[] Contrasena { get; set; }

        public virtual ICollection<Correo> Correo { get; set; }
        public virtual ICollection<Receta> Receta { get; set; }
        public virtual ICollection<Visualizacion> Visualizacion { get; set; }
    }
}
