using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recetario.Areas.Administradores.Servicios;
using Recetario.Areas.Administradores.Models;
using Recetario.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Recetario.BaseDatos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

// TODO: Agregar input para confirma contraseña
// TODO: Agregar agrupación de resultados mostrados en Index
/*
 1/2   Pantalla principal de Super Administrador
 */

namespace Recetario.Areas.Administradores.Controllers
{
    [Area("Administradores")]
    public class ActorsController : Controller
    {
        private UserManager<Actor> UserMgr { get; }
        private SignInManager<Actor> SignInMgr { get; }
        private readonly IActor _serviciosActor;
        private static int TIPO = 1;

        public ActorsController(UserManager<Actor> userManager,
            SignInManager<Actor> signInManager, IActor serviciosActor)
        {
            UserMgr = userManager;
            SignInMgr = signInManager;
            _serviciosActor = serviciosActor;
        }
        //Autorización para Admin
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        // GET: Administradores/Actors
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
            ICollection<ActorDTO> actores;
            //Si hay cadena de búsqueda
            //En caso de que no haber ninguna búsqueda, muestro todo, TODO
            if (!String.IsNullOrEmpty(cadenaBusqueda))
            {
                 actores = _serviciosActor.BuscarFiltro(cadenaBusqueda, 1);
            }
            else
            {
                actores = _serviciosActor.ObtenerLista(TIPO);
            }
            //Cantidad de Elementos a mostrar por página
            int pageSize = 4;
            return View(Paginacion<ActorDTO>.Create(actores, noPagina ?? 1, pageSize));
        }

        //Autorización para Admin
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        public IActionResult Detalles(int? id)
        {
            if (id == null)
            {
                // TODO: En sí qué hace NotFound? 
                return NotFound();
            }

            var actor = _serviciosActor.Obtener(id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        //Autorización para Admin
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        // GET: Administradores/Actors/Create
        public IActionResult Agregar()
        {
            return View();
        }

        // POST: Administradores/Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(ActorDTO actor)
        {
            if (ModelState.IsValid)
            {
                
                var user = new Actor()
                {
                    UserName = actor.Usuario,
                    Email = actor.Email
                };
                //Se agrega el usuario a la tabla aspnetusers (AppUser) usada para el login
                IdentityResult result = await UserMgr.CreateAsync(user, actor.Contrasena);
                if (result.Succeeded)
                {
                    //Establecer que es un administrador
                    //0.-root, 1.-Administrador, 2.-Usuario
                    actor.Tipo = 1;
                    //Se registra en la tabla actor
                    //TODO: Checar esto, ya que Ahora Actor esta fucionado con AppUser
                    //_serviciosActor.Registrar(actor);

                    await UserMgr.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Administrador"));
                    return RedirectToAction(nameof(Index));
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View(actor);

            }

            return View(actor);
        }

        public IActionResult Editar(int? id)
        {
            //Comprobar que en la URL se pase un ID
            if (id == null)
            {
                return NotFound();
            }
            var actor = _serviciosActor.Obtener(id);
            //En caso de que el Actor no exista en la BD
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Administradores/Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //Autorización para Admin
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, ActorDTO actor)
        {
            if (id != actor.IdActor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _serviciosActor.Actualizar(actor);
                }
                catch (Exception)
                {
                    return NotFound();
                }
                // TODO: Arreglar lo de las excepciones xD
                /*
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.IdActor))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                */
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        //Autorización para Admin
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        public IActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ActorDTO actor = _serviciosActor.Obtener(id);
            // TODO: Confrimar que se puede regresar el null
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        //Autorización para Admin
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        // Se debe especificar el nombre de la acción por que el nombre la firma del anterior método es el mismo
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            Actor toElim = await UserMgr.FindByIdAsync(id.ToString());
            await UserMgr.DeleteAsync(toElim);
            //TODO: Checar esto, ya que Ahora Actor esta fucionado con AppUser
            //_serviciosActor.Eliminar(id);
            return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }

        /*
        private bool ActorExists(int id)
        {
            return _context.Actor.Any(e => e.IdActor == id);
        }
        */
    }
}
