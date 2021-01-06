using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Recetario.BaseDatos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Recetario.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<Actor> _userManager;
        private readonly SignInManager<Actor> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguration _configuration;

        public LoginModel(SignInManager<Actor> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<Actor> userManager, 
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            //TODO: Login con Correo o con usuario
            [Required]
            //[EmailAddress]
            public string Usuario { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Recuerdame")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Usuario, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    /*------------------CÓDIGO PARA TOKEN DE JWT----------------*/
                    /*
                    var user = await _userManager.FindByNameAsync(Input.Usuario);
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email)
                    };
                    // Leer el secret_key desde appseting
                    var secretKey = _configuration.GetValue<string>("SecretKey");
                    var key = Encoding.ASCII.GetBytes(secretKey);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        // El token va a durar un día
                        Expires = DateTime.UtcNow.AddDays(1),
                        // Credenciales para generar el token usando nuestro secretykey y el algoritmo hash 256
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var createdToken = tokenHandler.CreateToken(tokenDescriptor);


                    var token = tokenHandler.WriteToken(createdToken);
                    */
                    _logger.LogInformation("User logged in.");

                    /*Ya que en esta misma request (POST) se logeó el usuario no es posible obtener su
                     * claim a partir de User, ya que este trabaja en base a cookies, y como aquí apenas
                     * se generó, por no hay, hasta la siguiente request, por eso mismo se manda
                     * a otra acción de otro controlador*/
                    return RedirectToAction("LoginRoute", new { returnUrl = returnUrl });
                    
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
