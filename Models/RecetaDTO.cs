using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Recetario.Areas.Administradores.Models;

namespace Recetario.Models
{
    public partial class RecetaDTO
    {
        public int IdReceta { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Display(Name = "Imagen de receta")]
        public IFormFile Imagen { get; set; }
        public string NombreImagen { get; set; }
        public string Descripcion { get; set; }
        [Display(Name = "Porcentaje Promedio")]
        public int ProcentajePromedio { get; set; }
        [Display(Name = "Tiempo de preparación (hor:min)")]
        [Required]
        [DataType(DataType.Time)]
        public string TiempoPrep { get; set; }
        [Display(Name = "Etiquetas (Separados por puntos)")]
        public String Etiquetas { get; set; }
        //Esta lista es para cuando se normalice el texto
        public List<string> IngredientesNormalizados { get; set; }
        [Display(Name = "Ingredientes (Separados por puntos)")]
        public String Ingredientes { get; set; }
        public List<PasoDTO> Pasos { get; set; }
        //Información del usuario que creó la receta
        public UsuarioDTO usuario { get; set; }
    }
    //Esta clases es solo para la trnaferencia del información 
    //Del usuario que creo una receta
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
    }
}
