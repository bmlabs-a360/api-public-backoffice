using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace api_public_backOffice.Helpers
{
    public interface IEmailHelper
    {
        bool IsValidEmail(string email);
        string GetBodyEmailSolicitudCreacionUsuario(string emisorAlias, string destinatarioAlias,
            string linkIrFormulario);
        string GetBodyEmailConfirmacion(string linkVerificar);
        string GetBodyEmailRecuperacion(string linkIrFormulario);
        string GetBodyEmailCompletaEvaluacionUsuarioBasico(string porcentajeRespuesta, string nombre);
        string GetBodyEmailCompletaEvaluacionUsuarioPro(string porcentajeRespuesta, string nombre);
        
    }

    public class EmailHelper : IEmailHelper, IDisposable
    {
        private readonly IWebHostEnvironment _env;

        public EmailHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        public bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public string GetBodyEmailSolicitudCreacionUsuario(
            string emisorAlias, string destinatarioAlias, string linkIrFormulario)
        {
            string templateBody = "";
            string urlTemplate = "wwwroot/htmlEmail/BodySolicitudCrearUsuario.html";

            try
            {
                templateBody = File.ReadAllText(urlTemplate);

                templateBody = templateBody.Replace("[EMISOR]", emisorAlias);
                templateBody = templateBody.Replace("[DESTINATARIO]", destinatarioAlias);
                templateBody = templateBody.Replace("[LINK_IRFORMULARIO]", linkIrFormulario);

                return templateBody;

            }
            catch (Exception)
            {
                return templateBody;
            }

        }

        public string GetBodyEmailConfirmacion(string linkVerificar)
        {
            string templateBody = "";
            string urlTemplate = "wwwroot/htmlEmail/BodyVerificarEmail.html";

            try
            {
                templateBody = File.ReadAllText(urlTemplate);

                templateBody = templateBody.Replace("[LINK_VERIFICAR]", linkVerificar);

                return templateBody;
            }
            catch (Exception)
            {
                return templateBody;
            }
        }

        public string GetBodyEmailRecuperacion(string linkIrFormulario)
        {
            string templateBody = "";
            string urlTemplate = "wwwroot/htmlEmail/BodyRecuperacionEmail.html";

            try
            {
                templateBody = File.ReadAllText(urlTemplate);

                templateBody = templateBody.Replace("[LINK_IRFORMULARIO]", linkIrFormulario);

                return templateBody;

            }
            catch (Exception)
            {
                return templateBody;
            }
        }

        public string GetBodyEmailCompletaEvaluacionUsuarioBasico(string porcentajeRespuesta, string nombre)
        {
            string templateBody = "";
            string urlTemplate = "wwwroot/htmlEmail/BodyCompletaEvaluacionUsuarioBasico.html";

            try
            {
                templateBody = File.ReadAllText(urlTemplate);

                templateBody = templateBody.Replace("[PORCENTAJE]", porcentajeRespuesta).Replace("[NOMBRE_USUARIO]", nombre);

                return templateBody;

            }
            catch (Exception)
            {
                return templateBody;
            }
        }

        public string GetBodyEmailCompletaEvaluacionUsuarioPro(string porcentajeRespuesta, string nombre)
        {
            string templateBody = "";
            string urlTemplate = "wwwroot/htmlEmail/BodyCompletaEvaluacionUsuarioPro.html";

            try
            {
                templateBody = File.ReadAllText(urlTemplate);

                templateBody = templateBody.Replace("[PORCENTAJE]", porcentajeRespuesta).Replace("[NOMBRE_USUARIO]", nombre);

                return templateBody;

            }
            catch (Exception)
            {
                return templateBody;
            }
        }

        public void Dispose() {}
    }

}
