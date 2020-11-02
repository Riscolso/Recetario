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
using System.Linq;

// TODO: Agregar input para confirma contraseña
// TODO: Agregar agrupación de resultados mostrados en Index
// TODO: Editar, agregar, eliminar y todo eso, pasar código a _Servicios actor
/*
 1/2   Pantalla principal de Super Administrador
 */

namespace Recetario.Areas.Administradores.Controllers
{
    [Area("Administradores")]
    //Autorización para Admin
    [Authorize(Roles = "Administrador,SuperAdministrador")]
    public class ActorsController : Controller
    {
        private UserManager<Actor> _userManager { get; }
        private SignInManager<Actor> SignInMgr { get; }
        private readonly IActor _serviciosActor;

        public ActorsController(UserManager<Actor> userManager,
            SignInManager<Actor> signInManager, IActor serviciosActor)
        {
            _userManager = userManager;
            SignInMgr = signInManager;
            _serviciosActor = serviciosActor;
        }
        // GET: Administradores/Actors
        public IActionResult Index(string cadenaBusqueda, int? noPagina, String filtroActual, String rol)
        {
            //Guardar qué roles se están trabajando
            ViewData["Rol"] = rol;

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
                 actores = _serviciosActor.BuscarFiltro(cadenaBusqueda, rol);
            }
            else
            {
                actores = _serviciosActor.ObtenerLista(rol);
            }
            //Cantidad de Elementos a mostrar por página
            int pageSize = 4;
            return View(Paginacion<ActorDTO>.Create(actores, noPagina ?? 1, pageSize));
        }

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
                var user = new Actor
                {
                    UserName = actor.Usuario,
                    Email = actor.Email,
                    NombreActor = actor.NombreActor,
                    FechaNac = actor.FechaNac
                };
                //Se agrega el usuario a la tabla aspnetusers (AppUser) usada para el login
                var result = await _userManager.CreateAsync(user, actor.Contrasena);
                if (result.Succeeded)
                {
                    //Establecer que es un administrador
                    //0.-root, 1.-Administrador, 2.-Usuario
                    //actor.Tipo = 1;
                    //Se registra en la tabla actor
                    //TODO: Checar esto, ya que Ahora Actor esta fucionado con AppUser
                    //_serviciosActor.Registrar(actor);
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Administrador"));
                    return RedirectToAction(nameof(Index), new
                    {
                        rol = "Administrador"
                    });
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, ActorDTO actorDTO)
        {
            if (id != actorDTO.IdActor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Obtener el actor
                    var actor = await _userManager.FindByIdAsync(id.ToString());
                    //Actualizar sus datos
                    actor.UserName = actorDTO.Usuario;
                    actor.Email = actorDTO.Email;
                    actor.NombreActor = actorDTO.NombreActor;
                    actor.FechaNac = actorDTO.FechaNac;
                    //Reflejar los cambios en la BD
                    var result = await _userManager.UpdateAsync(actor);
                    //TODO: Poner todo esto a los demás métodos
                    if (result.Succeeded)
                    {
                        /*Redirecciona al index, el truco está en que regresa como parámetro el
                        rol del usuario que se está editando*/
                        return RedirectToAction(nameof(Index), new {rol = _userManager
                            .GetClaimsAsync(actor).Result
                            .FirstOrDefault(c => c.Type == ClaimTypes.Role).Value
                        });
                    }
                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return View(actorDTO);
        }

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

        // Se debe especificar el nombre de la acción por que el nombre la firma del anterior método es el mismo
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            Actor toElim = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.DeleteAsync(toElim);
            return RedirectToAction(nameof(Index), new
            {
                rol = _userManager
                        .GetClaimsAsync(toElim).Result
                        .FirstOrDefault(c => c.Type == ClaimTypes.Role).Value
            });
        }

        /*
        private bool ActorExists(int id)
        {
            return _context.Actor.Any(e => e.IdActor == id);
        }
        */
    }
}
