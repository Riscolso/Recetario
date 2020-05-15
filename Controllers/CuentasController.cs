using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recetario.BaseDatos;

namespace Recetario.Controllers
{
    public class CuentasController : Controller
    {
        private UserManager<AppUser> UserMgr { get; }
        private SignInManager<AppUser> SignInMgr { get; } 

        public CuentasController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
                UserMgr = userManager;
                SignInMgr = signInManager;
        }

        // POST: Administradores/Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(Actor actor) //string userName, string email, string password)
        {
            try
            {
                AppUser user = await UserMgr.FindByNameAsync(actor.Usuario);// userName);
                if(user == null)
                {
                    user = new AppUser();
                    user.UserName = actor.Usuario;// userName;
                    user.Email = actor.Email;// email;

                    IdentityResult result = await UserMgr.CreateAsync(user, Convert.ToString(actor.Contrasena));// password);
                    return View(user);
                }
                else
                {
                    return View("Home/Index.cshtml");
                }
            }
            catch(Exception) { return View("Home/Index.cshtml"); }
        }

        public async Task<IActionResult> IniciarSesion(Actor actor)
        {
            var result = await SignInMgr.PasswordSignInAsync(actor.Usuario, Convert.ToString(actor.Contrasena), false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("MenuSA", "Administradores/Menus");
            }
            else
            {
                return RedirectToAction("InicioSesion", "Home");
            }
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await SignInMgr.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}