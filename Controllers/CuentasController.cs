using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recetario.Areas.Administradores.Models;
using Recetario.Areas.Administradores.Servicios;
using Recetario.BaseDatos;

namespace Recetario.Controllers
{
    public class CuentasController : Controller
    {
        private UserManager<AppUser> _userMgr { get; }
        private SignInManager<AppUser> _signInMgr { get; }
        private readonly IActor _serviciosActor;

        public CuentasController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IActor serviciosActor)
        {
                _userMgr = userManager;
                _signInMgr = signInManager;
                _serviciosActor = serviciosActor;
        }

        // POST: Administradores/Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar(VActor actor)
        {
            try
            {
                //Se especifica como tipo usuario
                actor.Tipo = 2;
                //Se verifica que no se repita el nombre de usuario
                AppUser user = await _userMgr.FindByNameAsync(actor.Usuario);
                if (user == null)
                {
                    user = new AppUser();
                    user.UserName = actor.Usuario;// userName;
                    user.Email = actor.Email;// email;
                    //Se registra en la tabla aspnetusers (AppUser)
                    IdentityResult result = await _userMgr.CreateAsync(user, actor.Contrasena);
                    //Si se registró correctamente, iniciar sesión
                    if (result.Succeeded)
                    {
                        //Se registra como en la tabla actor (Actor)
                        _serviciosActor.Registrar(actor);
                        //Se agrega el claim usuario para la autorización
                        await _userMgr.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Usuario"));
                        return await IniciarSesion(actor);
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(actor);
                    }
                }
                else
                {
                    this.ModelState.AddModelError("Usuario", "El nombre de usuario ya existe");
                    return View(actor);
                }
            }
            catch (Exception) { return RedirectToAction("Home", "Index"); } //View("Home/Index.cshtml"); }
        }
        [AllowAnonymous]
        public async Task<IActionResult> IniciarSesion(VActor actor)
        {
            var result = await _signInMgr.PasswordSignInAsync(actor.Usuario, actor.Contrasena, false, false);
            if (result.Succeeded)
            {
                actor = _serviciosActor.FindVActor(actor.Usuario);
                if(actor.Tipo == 0)
                {
                    return RedirectToAction("MenuSA", "Menus", new { area = "Administradores" });
                }
                if(actor.Tipo == 1)
                {
                    return RedirectToAction("MenuA", "Menus", new { area = "Administradores" });
                }
                else
                {
                    return RedirectToAction("MenuU", "Menus", new { area = "Administradores" });
                }
                
            }
            else
            {
                return RedirectToAction("InicioSesion", "Home");
            }
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await _signInMgr.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}