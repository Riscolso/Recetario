using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Recetario.BaseDatos;
using Recetario.Areas.Administradores.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Recetario.Models;

namespace Recetario
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Agregar todos los servicios relacionados con MVC
            services.AddMvc(options =>
            {
                //Aplica un Filtro general para requerir un usuario autenticado
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }
                );
            //registra los servicios de Identity para el login (Identificación)
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                //Eliminar restricciones de contraseña
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                //options.Password.RequireLowercase = false;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ContextoBD>().AddErrorDescriber<CustomIdentityErrorDescriber>();

            //Agregar la Conexión con la BD
            //para hacer Scaffolding de la BD
            //Scaffold-DbContext "server=localhost;user id=root;password=root;database=recetario;persistsecurityinfo=True" Pomelo.EntityFrameworkCore.MySql -OutputDir BaseDatos -ContextDir BaseDatos -Context ContextoBD -Force
            services.AddDbContext<ContextoBD>(options =>
            options.UseMySql(Configuration.GetConnectionString("ConexionAzure"), x => x.ServerVersion("5.7.19-mysql")));

            //Ligar la clase ServiciosActor a la dependecia
            services.AddScoped<IActor, ServiciosActor>();
            services.AddScoped<IReceta, ServiciosReceta>();

            //Servicios para autorización con políticas
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireSuperAdministradorRole",
                     policy => policy.RequireRole("SuperAdministrador"));
                options.AddPolicy("RequireAdministradorRole",
                     policy => policy.RequireRole("Administrador", "SuperAdministrador"));
                options.AddPolicy("RequireUsuarioRole",
                     policy => policy.RequireRole("Usuario", "Administrador", "SuperAdministrador"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //Agregar autenticación
            app.UseAuthentication();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //Agregar Endpoint para la parte de los administradores
                endpoints.MapAreaControllerRoute(
                    name: "Administradores",
                    areaName: "Administradores",
                    pattern: "Administradores/{controller=Home}/{action=Index}/{id?}"
                    );

                //Agregar Endpoint para la parte de los usuarios
                endpoints.MapAreaControllerRoute(
                    name: "Usuarios",
                    areaName: "Usuarios",
                    pattern: "Usuarios/{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
