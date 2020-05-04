using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recetario.Models
{
    public partial class Correo
    {
        public int IdEmail { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        public string Correo1 { get; set; }
        public int ActorIdActor { get; set; }

        public virtual Actor ActorIdActorNavigation { get; set; }
    }
}
