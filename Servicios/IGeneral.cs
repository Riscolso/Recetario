using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recetario.Servicios
{
    public interface IGeneral
    {
        public ICollection<string> IngredientesFavoritos(int IdUsuario);
    }
}
