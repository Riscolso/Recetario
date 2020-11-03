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
        private readonly IActor _serviciosActor;
        //Inyección de dependencia para logar una programación iperativa (manual) en cuanto 
        //a las autorizaciones
        private IAuthorizationService _authService;

        public ActorsController(UserManager<Actor> userManager,
            IActor serviciosActor,
            IAuthorizationService authService)
        {
            _userManager = userManager;
            _serviciosActor = serviciosActor;
            _authService = authService;
        }
        // GET: Administradores/Actors
        public async Task<IActionResult> Index(string cadenaBusqueda, int? noPagina, String filtroActual, String rol)
        {
            //Guardar qué roles se están trabajando
            ViewData["Rol"] = rol;
            //Verificar manualmente que se cumpla con la política 
            //Ya que Administrador y SA comparten los mismo views/Controladores
            if (rol.Equals("Administrador"))
            {
                var authResult = await _authService.AuthorizeAsync(User, "RequireSuperAdministradorRole");
                if (!authResult.Succeeded)
                {
                    return new ForbidResult();
                }
            }
            
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

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                // TODO: En sí qué hace NotFound? 
                return NotFound();
            }
            //Regresa una tupla, el actorDTO y su rol
            var actor = _serviciosActor.Obtener(id);
            //Si el actor que se encontró es un admin
            if (actor.rol.Equals("Administrador") || actor.rol.Equals("Administrador"))
            {
                //Se debe checar que el usuario actual tenga permisos
                var authResult = await _authService.AuthorizeAsync(User, "RequireSuperAdministradorRole");
                if (!authResult.Succeeded)
                {
                    //Si no lo mandamos a la ñonga
                    return new ForbidResult();
                }
            }
            if (actor.actor == null)
            {
                return NotFound();
            }

            return View(actor.actor);
        }

        [Authorize(Roles = "SuperAdministrador")]
        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdministrador")]
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
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Administrador"));
                    return RedirectToAction(nameof(Index), new
                    {
                        rol = "Administrador"
                    });
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(actor);
            }
            return View(actor);
        }


        public async Task<IActionResult> Editar(int? id)
        {
            //Comprobar que en la URL se pase un ID
            if (id == null)
            {
                return NotFound();
            }
            var actor = _serviciosActor.Obtener(id);
            //En caso de que el Actor no exista en la BD

            //TODO: Arreglar la onda de que si no hay usuario, regresar null aquí y en los demás métodos
            if (actor.actor == null)
            {
                return NotFound();
            }
            if (actor.rol.Equals("Administrador") || actor.rol.Equals("Administrador"))
            {
                //Se debe checar que el usuario actual tenga permisos
                var authResult = await _authService.AuthorizeAsync(User, "RequireSuperAdministradorRole");
                if (!authResult.Succeeded)
                {
                    //Si no lo mandamos a la ñonga
                    return new ForbidResult();
                }
            }
            return View(actor.actor);
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
                        return RedirectToAction(nameof(Index), new
                        {
                            rol = _userManager
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

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var actor = _serviciosActor.Obtener(id);
            // TODO: Confrimar que se puede regresar el null
            if (actor.actor == null)
            {
                return NotFound();
            }
            if (actor.rol.Equals("Administrador") || actor.rol.Equals("Administrador"))
            {
                //Se debe checar que el usuario actual tenga permisos
                var authResult = await _authService.AuthorizeAsync(User, "RequireSuperAdministradorRole");
                if (!authResult.Succeeded)
                {
                    //Si no lo mandamos a la ñonga
                    return new ForbidResult();
                }
            }

            return View(actor.actor);
        }

        // Se debe especificar el nombre de la acción por que el nombre la firma del anterior método es el mismo
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            Actor toElim = await _userManager.FindByIdAsync(id.ToString());
            //Como se va a eliminar el usuario, guardamos su rol en un auxiliar
            var aux = _userManager
                        .GetClaimsAsync(toElim).Result
                        .FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            await _userManager.DeleteAsync(toElim);
            return RedirectToAction(nameof(Index), new
            {
                rol = aux
            });
        }
    }
}
