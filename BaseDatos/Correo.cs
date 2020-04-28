using System;
using System.Collections.Generic;

namespace Recetario.BaseDatos
{
    public partial class Correo
    {
        public int IdEmail { get; set; }
        public string Correo1 { get; set; }
        public int ActorIdActor { get; set; }

        public virtual Actor ActorIdActorNavigation { get; set; }
    }
}
