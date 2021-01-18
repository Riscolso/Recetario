using System;
using System.Collections.Generic;

namespace Recetario.BaseDatos
{
    public partial class Paso
    {
        public int NoPaso { get; set; }
        public string Texto { get; set; }
        public string Imagen { get; set; }
        public int? TiempoTemporizador { get; set; }
        public int RecetaIdReceta { get; set; }
        public int RecetaActorIdActor { get; set; }

        public virtual Receta Receta { get; set; }
    }
}
