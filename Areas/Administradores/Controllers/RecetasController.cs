using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recetario.Areas.Administradores.Servicios;
using Recetario.Areas.Administradores.Models;
using Recetario.Models;
using Microsoft.AspNetCore.Authorization;

namespace Recetario.Areas.Administradores.Controllers
{
    [Area("Administradores")]
    public class RecetasController : Controller
    {
        private readonly IReceta _serviciosReceta;

        public RecetasController(IReceta serviciosReceta)
        {
            _serviciosReceta = serviciosReceta;
        }
        //Autorización para Admin
        [Authorize(Roles = "Administrador,SuperAdministrador")]
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
            ICollection<VReceta> recetas;
            //Si hay cadena de búsqueda
            //En caso de que no haber ninguna búsqueda, muestro todo, TODO
            if (!String.IsNullOrEmpty(cadenaBusqueda))
            {
                recetas = _serviciosReceta.BuscarFiltro(cadenaBusqueda);
            }
            else
            {
                recetas = _serviciosReceta.Obtener();
            }
            //Cantidad de Elementos a mostrar por página
            int pageSize = 4;
            return View(Paginacion<VReceta>.Create(recetas, noPagina ?? 1, pageSize));
        }
    }
}