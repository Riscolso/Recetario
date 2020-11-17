using System;
using System.Collections.Generic;

namespace Recetario.BaseDatos
{
    public partial class Etiqueta
    {
        public Etiqueta()
        {
            Usa = new HashSet<Usa>();
        }

        public int IdEtiqueta { get; set; }
        //TODO: Etiqueta1 ????? xD
        public string Etiqueta1 { get; set; }

        public virtual ICollection<Usa> Usa { get; set; }
    }
}
