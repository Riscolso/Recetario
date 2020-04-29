using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recetario.BaseDatos
{
    public partial class Correo
    {
        public int IdEmail { get; set; }
        [Display(Name = "Email")]
        public string Correo1 { get; set; }
        public int ActorIdActor { get; set; }

        public virtual Actor ActorIdActorNavigation { get; set; }
    }
}
