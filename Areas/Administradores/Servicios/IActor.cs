using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recetario.Areas.Administradores.Models;

namespace Recetario.Areas.Administradores.Servicios
{
    public interface IActor
    {
        //Regresa el ID del actor
        int RegistrarActor(VActor actor);
    }
}
