using System;
using System.Collections.Generic;

namespace Recetario.BaseDatos
{
    public partial class Receta
    {
        public Receta()
        {
            Lleva = new HashSet<Lleva>();
            Paso = new HashSet<Paso>();
            Usa = new HashSet<Usa>();
            Visualizacion = new HashSet<Visualizacion>();
        }

        public int IdReceta { get; set; }
        public string Nombre { get; set; }
        public int ProcentajePromedio { get; set; }
        public string Descripcion { get; set; } 
        public string TiempoPrep { get; set; }
        public int ActorIdActor { get; set; }

        public virtual Actor ActorIdActorNavigation { get; set; }
        public virtual ICollection<Lleva> Lleva { get; set; }
        public virtual ICollection<Paso> Paso { get; set; }
        public virtual ICollection<Usa> Usa { get; set; }
        public virtual ICollection<Visualizacion> Visualizacion { get; set; }
    }
}
