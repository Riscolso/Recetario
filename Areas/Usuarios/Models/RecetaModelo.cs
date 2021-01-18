using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recetario.Models;
using Recetario.BaseDatos;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Recetario.Areas.Usuarios.Models
{
    public class RecetaModelo
    {
        //Clase contenedora de todo lo que tiene que ver con la receta
        public RecetaDTO Receta;
        
        public Visualizacion Visualizar { get; set; }
    }
}
