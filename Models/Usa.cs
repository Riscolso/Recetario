using System;
using System.Collections.Generic;

namespace Recetario.BaseDatos
{
    public partial class Usa
    {
        public int RecetaIdReceta { get; set; }
        public int RecetaActorIdActor { get; set; }
        public int EtiquetaIdEtiqueta { get; set; }

        public virtual Etiqueta EtiquetaIdEtiquetaNavigation { get; set; }
        public virtual Receta Receta { get; set; }
    }
}
