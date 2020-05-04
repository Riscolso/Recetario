using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recetario.Models
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
        [Required]
        public string NombreActor { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaNac { get; set; }
        public bool Tipo { get; set; }
        [Required]
        public string Usuario { get; set; }
        [Display(Name = "Contraseña")]
        [Required]
        public byte[] Contrasena { get; set; }

        public virtual ICollection<Correo> Correo { get; set; }
        public virtual ICollection<Receta> Receta { get; set; }
        public virtual ICollection<Visualizacion> Visualizacion { get; set; }
    }
}
