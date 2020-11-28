using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recetario.Models
{
    //TODO: Agregar requireds
    public class PasoDTO
    {
        public int IdPaso { get; set; }
        public int NoPaso { get; set; }
        public string Texto { get; set; }
        //Después de va a convertir el tiempo en minutos al registrar en la BD
        [Display(Name = "Tiempo (horas: minutos")]
        [DataType(DataType.Time)]
        public string TiempoTemporizador { get; set; }
    }
}
