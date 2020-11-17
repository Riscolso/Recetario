using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recetario.Areas.Usuarios.Models
{
    //TODO: Agregar requireds
    public class RecetaUDTO
    {
        public int idUsuario { get; set; }
        public int IdReceta { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Display(Name = "Tiempo de preparación (minutos:segundos)")]
        [Required]
        [DataType(DataType.Time)]
        public string TiempoPrep { get; set; }
        [Display(Name = "Etiquetas (Separados por espacios)")]
        public String Etiquetas { get; set; }
        public String Ingredientes { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Imagen { get; set; }
        public List<PasoDTO> Pasos { get; set; }
    }
}
