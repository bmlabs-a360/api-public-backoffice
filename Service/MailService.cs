using api_public_backOffice.Clients;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using api_public_backOffice.Helpers;

namespace api_public_backOffice.Service
{
    public interface IMailService
    {
        Task<OutputMail> SendMailAsync(MailDTO mailDTO);
    }

    public class MailService : IMailService, IDisposable
    {
        private readonly MsMailClient _msMailClient;
        private readonly IEmailHelper _emailHelper;

        public MailService(MsMailClient msMailClient, EmailHelper emailHelper)
        {
            _msMailClient = msMailClient;
            _emailHelper = emailHelper;
        }

        public async Task<OutputMail> SendMailAsync(MailDTO mailDTO)
        {
            
            if (string.IsNullOrEmpty(mailDTO.From)) throw new ArgumentNullException("From");
            if (!_emailHelper.IsValidEmail(mailDTO.From)) throw new FormatException("From");

            if (string.IsNullOrEmpty(mailDTO.FromAlias)) throw new ArgumentNullException("FromAlias");
            
            if (!mailDTO.To.Any()) throw new ArgumentNullException("To");
            if (mailDTO.To.Any(x => !_emailHelper.IsValidEmail(x))) throw new FormatException("To");

            if (!mailDTO.ToAlias.Any()) throw new ArgumentNullException("ToAlias");

            if (string.IsNullOrEmpty(mailDTO.Subject)) throw new ArgumentNullException("Subject");
            if (string.IsNullOrEmpty(mailDTO.Body)) throw new ArgumentNullException("Body");
            
            var retorno = await _msMailClient.SendAsync(mailDTO);

            return retorno;
        }

        public void Dispose() { }
    }
}
