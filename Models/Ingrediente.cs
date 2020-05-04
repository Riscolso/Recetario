using System;
using System.Collections.Generic;

namespace Recetario.Models
{
    public partial class Ingrediente
    {
        public Ingrediente()
        {
            Lleva = new HashSet<Lleva>();
        }

        public int IdIngrediente { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Lleva> Lleva { get; set; }
    }
}
