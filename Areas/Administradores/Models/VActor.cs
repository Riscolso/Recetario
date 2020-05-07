using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recetario.Areas.Administradores.Models
{
    //El nombre es V de Vista
    //Para diferenciar la clase de vista con el modelo Actor
    public partial class VActor
    {
        public int IdActor { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        public string NombreActor { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaNac { get; set; }
        public int Tipo { get; set; }
        [Required]
        public string Usuario { get; set; }
        // TODO: Usar ER para exigir que la contrasena sea segura
        [Required]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
