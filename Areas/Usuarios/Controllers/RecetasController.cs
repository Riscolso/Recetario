using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recetario.Areas.Administradores.Servicios;
using Recetario.Models;
using Recetario.BaseDatos;
using Recetario.Servicios;

namespace Recetario.Areas.Usuarios
{
    [Area("Usuarios")]
    public class RecetasController : Controller
    {
        private readonly ContextoBD _context;
        private readonly IReceta _servicioreceta;
        private readonly IEmail _serviciosemail;
        private UserManager<Actor> _userManager { get; }
        public RecetasController(ContextoBD context,
            IReceta serviciosreceta,
            UserManager<Actor> userManager,
            IEmail serviciosemail)
        {
            _context = context;
            _servicioreceta = serviciosreceta;
            _userManager = userManager;
            _serviciosemail = serviciosemail;
        }

        // GET: Usuarios/Recetas
        public IActionResult Index(int id)
        {
            // TODO: Agregar links a las etiquetas/ingredientes para reedireccionar a búsqueda con dichas 
            //Caraterísticas
            return View(_servicioreceta.Obtener(id));
        }


        public IActionResult Crear()
        {
            return View();
        }

        //TODO: Agregar DragAndDrop a los pasos de las recetas para cambiarlas de posición
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(RecetaDTO receta)
        {
            if (ModelState.IsValid)
            {
                //Agregar el usuario que esta creando la receta a recetaUDTO
                receta.usuario = new UsuarioDTO
                {
                    IdUsuario = Convert.ToInt32(_userManager.GetUserId(User))
                };
                int id = _servicioreceta.Agregar(receta);
                return RedirectToAction("Index", new { id = id });
            }
            return View(receta);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Checar que este usuario sea el creador (Dios, obviamente)
            //Obtener el actor logeado
            var actor = await _userManager.GetUserAsync(User);
            var receta = _servicioreceta.Obtener((int)id);
            //En caso de ser el mismo que la creó, dejar hacer lo que quiera con esta
            if (actor.Id == receta.usuario.IdUsuario)
            {
                if (receta == null) return NotFound();
                return View(receta);
            }
            else return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(RecetaDTO receta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Checar que sea el mismo usuario quien creó la receta
                    if (receta.usuario.IdUsuario == _userManager.GetUserAsync(User).Result.Id) _servicioreceta.Editar(receta);
                    else return NotFound();
                }
                //En caso de que no exista la receta
                catch (NotImplementedException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index), new {id = receta.IdReceta });
            }
            //ViewData["ActorIdActor"] = new SelectList(_context.Actor, "Id", "NombreActor", receta.ActorIdActor);
            return View(receta);
        }

        // GET: Usuarios/Recetas/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = _servicioreceta.Obtener((int)id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        // POST: Usuarios/Recetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            //TODO: Permisos para modificar receta (Si es el usuario que la creó)
            _servicioreceta.Eliminar(id);
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public IActionResult ListaReceta(string cadenaBusqueda, int? noPagina, String filtroActual)
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
                recetas = _servicioreceta.ObtenerXUsuario(_userManager.GetUserAsync(User).Result.Id);
            }
            //Cantidad de Elementos a mostrar por página
            int pageSize = 4;
            return View(Paginacion<RecetaDTO>.Create(recetas, noPagina ?? 1, pageSize));
        }

        public IActionResult EnviarIngredientes(int IdReceta)
        {
            try
            {
                _serviciosemail.EnviarEmailIngredientes(_userManager.GetUserAsync(User).Result.Email, IdReceta);
            }
            catch
            {
                return NotFound();
            }
            return RedirectToAction("Index", new { id = IdReceta });
        }
    }
}
