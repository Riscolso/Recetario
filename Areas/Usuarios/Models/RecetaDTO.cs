using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recetario.Areas.Usuarios.Models
{
    public class RecetaDTO
    {
        public int IdReceta { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Display(Name = "Tiempo de preparación")]
        [Required]
        [DataType(DataType.Duration)]
        public DateTime TiempoPrep { get; set; }
        public String Etiquetas { get; set; }
        public String Ingredientes { get; set; }
        public List<(int, string, bool, int)> Pasos { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Imagen { get; set; }
    }
}
