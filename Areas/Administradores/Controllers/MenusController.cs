using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//TODO: Deshacerse de este controlador, las autorizaciones pueden ir en un razor index
namespace Recetario.Areas.Administradores.Controllers
{
    [Area("Administradores")]
    public class MenusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //Autorización para Admin
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        public IActionResult MenuA()
        {
            return View();
        }
        //Autorización para Usuario
        [Authorize(Roles = "Usuario, Administrador,SuperAdministrador")]
        public IActionResult MenuU()
        {
            return View();
        }
        //Autorización para SuperAdmin
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public IActionResult MenuSA()
        {
            return View();
        }
    }
}