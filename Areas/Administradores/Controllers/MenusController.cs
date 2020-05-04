using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Recetario.Areas.Administradores.Controllers
{
    [Area("Administradores")]
    public class MenusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //No sé como se impplemente, pero solo lo pongo aquí para usarlo temporalmente
        //Lo puedes modificar como quieras xD
        public IActionResult IniciarSesion()
        {
            return View("MenuSA");
        }
    }
}