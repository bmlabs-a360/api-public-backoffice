using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;

namespace api_public_backOffice.Helpers
{
    public interface IEmailHelper
    {
        bool IsValidEmail(string email);
        string GetBodyEmailSolicitudCreacionUsuario(string emisorAlias, string destinatarioAlias,
            string linkIrFormulario);
        string GetBodyEmailConfirmacion(string linkVerificar);
        string GetBodyEmailRecuperacion(string linkIrFormulario);
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

        public void Dispose() {}
    }

}
