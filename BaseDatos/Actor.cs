using Microsoft.AspNetCore.Identity;
using Recetario.Areas.Administradores.Models;
using System;
using System.Collections.Generic;

namespace Recetario.BaseDatos
{
    //El int del final es por que LA clave principal del Actor es un entero
    public partial class Actor : IdentityUser<int>
    {
        public Actor()
        {
            Receta = new HashSet<Receta>();
            Visualizacion = new HashSet<Visualizacion>();
        }

        public string NombreActor { get; set; }
        public DateTime FechaNac { get; set; }
        public int Tipo { get; set; }

        public virtual ICollection<Receta> Receta { get; set; }
        public virtual ICollection<Visualizacion> Visualizacion { get; set; }
    }
}
