using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recetario.Areas.Administradores.Models;
using Recetario.BaseDatos;

//TODO: Agregar imagen de pefil junto con su respectiva edición

namespace Recetario.Areas.Identity.Pages.Account
{
    public class EditarUsuarioModel : PageModel
    {
        //Dependencia del usuario
        public UserManager<Actor> _userManager;
        public EditarUsuarioModel(UserManager<Actor> usrManager)
        {
            _userManager = usrManager;
            //TODO: Tratar de hacer que funcione sin instanciar primero
            Input = new Edicion();
        }
        
        //TODO: Buscar para qué sirve BindProperty
        //Se usa un modelo propio de esta clase
        [BindProperty]
        public Edicion Input { get; set; }

        /*Crear un modelo de datos para edición
         * Ya que los datos que no todos los datos se pueden modificar
         * no se puede usar Actor DTO, ya que tiene anotaciones para registro
        */
        public class Edicion
        {
            //Datos que se pueden editar
            [Required]
            [Display(Name = "Nombre")]
            public string Nombre { get; set; }
            [Display(Name = "Fecha de Nacimiento")]
            [Required]
            [DataType(DataType.Date)]
            public DateTime FechaNac { get; set; }

            //Datos que se muestran, pero no son editables (Por lo menos no aquí)
            public string Usuario { get; set; }
            public string Email { get; set; }

        }


        /*Al momento de cargarse por el método get, carga el usuario asociado al id que le llega
         Y lo llena en los campos*/
        public async Task OnGetAsync()
        {
            //TODO: Averiguar de donde chuchas sale User xD
            //Obtener el usuario que esta logeado de la BD
            var usuario = await _userManager.FindByIdAsync(_userManager.GetUserId(User).ToString());
            //System.Diagnostics.Debug.WriteLine(usuario.NombreActor);
            //Si no se encuentra mandar a la página de error
            if (usuario == null) NotFound();
            //Mostrar los datos en la vista
            Input.Nombre = usuario.NombreActor; 
            Input.FechaNac = usuario.FechaNac;
            Input.Email = usuario.Email;
            Input.Usuario = usuario.UserName;
        }
        /*Modificar en la BD (Por medio de identity)
         los datos del usuario*/
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //Traer el objeto usuario de la BD
                var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User).ToString());
                //Modificar sus valores
                user.NombreActor = Input.Nombre;
                user.FechaNac = Input.FechaNac;
                //Actualizar los valores
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToPage("EditarUsuario");
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return Page();
        }
    }
}
