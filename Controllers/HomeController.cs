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
using Recetario.Areas.Administradores.Servicios;
using Recetario.BaseDatos;
using Recetario.Models;

namespace Recetario.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Actor> _userManager;
        private readonly IReceta _serviciosreceta;

        public HomeController(ILogger<HomeController> logger,
                            UserManager<Actor> userManager,
                            IReceta serviciosRece)
        {
            _logger = logger;
            _userManager = userManager;
            _serviciosreceta = serviciosRece;
        }
        public IActionResult Index(string cadenaBusqueda, int? noPagina, String filtroActual)
        {

            var aux = _serviciosreceta.IngredientesFavoritos(Convert.ToInt32(_userManager.GetUserId(User)));


            //Se mete el filtro a ViewData para que permanezca aunque se cambie de páginas
            ViewData["FiltroActual"] = cadenaBusqueda;

            if (cadenaBusqueda != null)
            {
                noPagina = 1;
            }
            else
            {
                cadenaBusqueda = filtroActual;
            }
            ICollection<RecetaDTO> recetas;
            //Si hay cadena de búsqueda
            //En caso de que no haber ninguna búsqueda, muestro todo, TODO
            if (!String.IsNullOrEmpty(cadenaBusqueda))
            {
                recetas = _serviciosreceta.BuscarFiltro(cadenaBusqueda);
            }
            else
            {
                recetas = _serviciosreceta.Obtener();
            }
            //Cantidad de Elementos a mostrar por página
            int pageSize = 4;
            return View(Paginacion<RecetaDTO>.Create(recetas, noPagina ?? 1, pageSize));
        }
        public IActionResult InicioSesion()
        {
            return View();
        }
        public IActionResult Registro()
        {
            return View();
        }

        //Acción para reedireccionar deacuerdo al tipo de usuario que esta logeado
        public ActionResult LoginRoute(string returnUrl)  //this method is new
        {
            //TODO: Usar returnURL para mandar a la página desde la cual se inició el login
            var rol = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role).Value;
            //Bueno, aquí se ve un poco obvio que pasa y así 
            if (rol.Equals("SuperAdministrador"))
            {
                return  RedirectToAction("MenuSA", "Menus", new { area = "Administradores" });
            }
            if (rol.Equals("Administrador"))
            {
                return RedirectToAction("MenuSA", "Menus", new { area = "Administradores" });
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
