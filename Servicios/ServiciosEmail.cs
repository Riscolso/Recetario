using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using Recetario.BaseDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recetario.Servicios
{
    public class ServiciosEmail : IEmail
    {
        //TODO: Agregar seguridad a las credenciales jajajaja
        //Lo siento la ciberseguiridad no es mi pasión =( 
        private readonly string Usuario = "recetarioazulTT";
        private readonly string Contrasena = "RTzVw7vG5T9tb2E";
        private readonly ContextoBD _contextoBD;

        public ServiciosEmail(ContextoBD contextoBD)
        {
            _contextoBD = contextoBD;
        }


        public void EnviarEmailIngredientes(string destino, int IdReceta)
        {
            //Traer la información que se va a enviar en el correo
            var receta = _contextoBD.Receta
                .Include(r => r.Lleva)
                .FirstOrDefault(r => r.IdReceta == IdReceta);

            //Verificar que exista la receta
            if (receta != null)
            {
                //Dar formato a los ingredientes
                //Cadena donde se almacenan los ingredientes
                string aux = "";
                foreach (Lleva ingrediente in receta.Lleva)
                {
                    //Agregar el ingrediente y un salto de línea
                    aux += "<li>" + ingrediente.IngredienteCrudo + "</li>";
                }
                // Crear el mensaje
                var email = new MimeMessage();
                email.From.Add( new MailboxAddress("Recetario Azul", "recetarioazulTT@gmail.com"));
                email.To.Add(MailboxAddress.Parse(destino));
                email.Subject = "Lista de ingrediente de "+receta.Nombre;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = "<h1>Ingredientes de " + receta.Nombre + "</h1><br/>" +
                        "Hola, esta es la lista de ingredientes que necesitas para tu receta"+
                        "<ul>" +
                        aux +
                        "</ul>" +
                        "<br/><br/><br/>Tripa vacía, corazón sin alegría"
                };

                // Enviar email 
                using SmtpClient smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(Usuario, Contrasena);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            else throw new Exception("La receta no existe en la Base de datos");
        }
    }
}
