using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recetario.Models;

namespace Recetario.Areas.Usuarios.Models
{
    public class RecetaModelo
    {
        //Clase contenedora de todo lo que tiene que ver con la receta
        public RecetaDTO Receta;
        //Otros Items que se necesitan para mostrar el Index de una receta
        public String ManoArriba
        {
            get
            {
                return "~/img/arriba.png";
            }

        }
        public String ManoAbajo
        {
            get
            {
                return "~/img/abajo.png";
            }

        }
    }
}
