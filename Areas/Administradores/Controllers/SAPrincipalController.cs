using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Recetario.Areas.Administradores.Controllers
{
    public class SAPrincipalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}