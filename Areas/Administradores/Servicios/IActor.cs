﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recetario.Models;

namespace Recetario.Areas.Administradores.Servicios
{
    public interface IActor
    {
        Boolean RegistrarActor(Actor actor);
    }
}