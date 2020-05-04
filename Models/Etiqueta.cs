using System;
using System.Collections.Generic;

namespace Recetario.Models
{
    public partial class Etiqueta
    {
        public Etiqueta()
        {
            Usa = new HashSet<Usa>();
        }

        public int IdEtiqueta { get; set; }
        public string Etiqueta1 { get; set; }

        public virtual ICollection<Usa> Usa { get; set; }
    }
}
