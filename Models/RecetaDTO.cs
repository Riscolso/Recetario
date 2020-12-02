using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Recetario.Areas.Administradores.Models;

namespace Recetario.Models
{
    /// <summary>
    /// Clase que define el modelo de las vistas para la tabla Actor
    /// </summary>
    /// <remarks>El nombre es V de Vista
    /// <para>Para diferenciar la clase de vista con el modelo Receta</para></remarks>

    // TODO: Combinar RecetarioDTO y RecetarioUDTO, usar inputs no visibles

    public partial class RecetaDTO
    {
        public int IdReceta { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Display(Name = "Porcentaje Promedio")]
        public int ProcentajePromedio { get; set; }
        [Display(Name = "Tiempo de preparación (hor:min)")]
        [Required]
        [DataType(DataType.Time)]
        public string TiempoPrep { get; set; }
        [Display(Name = "Etiquetas (Separados por comas)")]
        public String Etiquetas { get; set; }
        //Esta lista es para cuando se normalice el texto
        public List<string> IngredientesNormalizados { get; set; }
        [Display(Name = "Ingredientes (Separados por comas)")]
        public String Ingredientes { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Imagen { get; set; }
        public List<PasoDTO> Pasos { get; set; }
        //Información del usuario que creó la receta
        public ActorDTO usuario { get; set; }
    }
}
