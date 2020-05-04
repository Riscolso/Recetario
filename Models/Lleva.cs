using System;
using System.Collections.Generic;

namespace Recetario.Models
{
    public partial class Lleva
    {
        public int RecetaIdReceta { get; set; }
        public int RecetaActorIdActor { get; set; }
        public int IngredienteIdIngrediente { get; set; }

        public virtual Ingrediente IngredienteIdIngredienteNavigation { get; set; }
        public virtual Receta Receta { get; set; }
    }
}
