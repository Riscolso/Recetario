using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recetario.Areas.Administradores.Models
{
    /// <summary>
    /// Clase que define el modelo de las vistas para la tabla Actor
    /// </summary>
    /// <remarks>El nombre es V de Vista
    /// <para>Para diferenciar la clase de vista con el modelo Receta</para></remarks>

    public partial class RecetaDTO
    {
        [Required]
        public int IdReceta { get; set; }
        [Display(Name = "Receta")]
        [Required]
        public string Nombre { get; set; }
        [Display(Name = "Porcentaje Promedio")]
        [Required]
        public int ProcentajePromedio { get; set; }
        [Display(Name = "Tiempo de preparación")]
        [Required]
        public string TiempoPrep { get; set; }
        [Display(Name = "Id de actor")]
        [Required]
        public int ActorIdActor { get; set; }
        /// <remarks>
        /// Se agrega a la vista el nombre del actor que creó la receta
        /// </remarks>
        [Display(Name = "Usuario creador")]
        public string ActorNombreActor { get; set; }
    }
}
