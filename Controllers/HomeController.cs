using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recetario.BaseDatos;
using Recetario.Models;

namespace Recetario.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Actor> _userManager;

        public HomeController(ILogger<HomeController> logger,
                            UserManager<Actor> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        //Permite que pueda ser llamado por
        //un usuario sin autenticar
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult InicioSesion()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        //Acción para reedireccionar deacuerdo al tipo de usuario que esta logeado
        public ActionResult LoginRoute(string returnUrl)  //this method is new
        {
            var rol = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role).Value;
            //Bueno, aquí se ve un poco obvio que pasa y así 
            if (rol.Equals("SuperAdministrador"))
            {
                return RedirectToAction("MenuSA", "Menus", new { area = "Administradores" });
            }
            if (rol.Equals("Administrador"))
            {
                return RedirectToAction("MenuA", "Menus", new { area = "Administradores" });
            }
            //Si es un usuario lo mandamos a dónde estaba
            else
            {
                return LocalRedirect(returnUrl);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
