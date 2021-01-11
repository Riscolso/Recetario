using System;
using System.Collections.Generic;

namespace Recetario.BaseDatos
{
    public partial class Lleva
    {
        public int IdLleva { get; set; }
        public int RecetaIdReceta { get; set; }
        public int RecetaActorIdActor { get; set; }
        public int IngredienteIdIngrediente { get; set; }
        public string IngredienteCrudo { get; set; }

        public virtual Ingrediente IngredienteIdIngredienteNavigation { get; set; }
        public virtual Receta Receta { get; set; }
    }
}
