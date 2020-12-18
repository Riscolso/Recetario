using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recetario.Servicios
{
    public interface IEmail
    {
        void EnviarEmailIngredientes(string correo, int IdReceta);
    }
}
