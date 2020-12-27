using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recetario.Areas.Administradores.Servicios;
using Recetario.BaseDatos;
using Recetario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recetario.Areas.Usuarios.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly IReceta _servicioreceta;
        private UserManager<Actor> _userManager { get; }
        public PrincipalController(IReceta servicioreceta,
            UserManager<Actor> usermng)
        {
            _servicioreceta = servicioreceta;
            _userManager = usermng;
        }
        public IActionResult Index(string cadenaBusqueda, int? noPagina, String filtroActual)
        {
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
                recetas = _servicioreceta.BuscarFiltro(cadenaBusqueda);
            }
            else
            {
                recetas = _servicioreceta.Obtener();
            }
            //Cantidad de Elementos a mostrar por página
            int pageSize = 4;
            return View(Paginacion<RecetaDTO>.Create(recetas, noPagina ?? 1, pageSize));
        }
    }
}
