using System;
using System.Collections.Generic;

namespace Recetario.BaseDatos
{
    public partial class Visualizacion
    {
        public int ActorIdActor { get; set; }
        public int RecetaIdReceta { get; set; }
        public int RecetaActorIdActor { get; set; }
        public int? ProcentajeCompl { get; set; }
        public bool? Calificacion { get; set; }

        public virtual Actor ActorIdActorNavigation { get; set; }
        public virtual Receta Receta { get; set; }
    }
}
