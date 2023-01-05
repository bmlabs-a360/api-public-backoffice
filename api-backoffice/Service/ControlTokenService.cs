using api_public_backOffice.Models;
using api_public_backOffice.Repository;
using AutoMapper;
using neva.entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace api_public_backOffice.Service
{
    public interface IControlTokenService
    {
        Task<ControlTokenModel> SaveOrUpdateControlToken(ControlTokenModel controltokenModel);
        Task<bool> IsValidToken(string token, DateTime fechaCreacion, bool usarToken = false);
        void Dispose();
    }

    public class ControlTokenService : IControlTokenService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IControlTokenRepository _controlTokenRepository;

        public ControlTokenService(
            IMapper mapper,
            IMemoryCache memoryCache,
            ControlTokenRepository controlTokenRepository)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _controlTokenRepository = controlTokenRepository;
        }

        public async Task<ControlTokenModel> SaveOrUpdateControlToken(ControlTokenModel controltokenModel)
        {
            var retorno = await _controlTokenRepository.InsertOrUpdate(_mapper.Map<ControlToken>(controltokenModel));
            return _mapper.Map<ControlTokenModel>(retorno);
        }

        public async Task<bool> IsValidToken(string token, DateTime fechaCreacion, bool usarToken = false)
        {
            bool isValid = await _controlTokenRepository.IsValidToken(token, fechaCreacion, usarToken);
            return isValid;
        }

        public void Dispose()
        {
            if (_controlTokenRepository != null)
            {
                _controlTokenRepository.Dispose();
                _controlTokenRepository = null;
            }
        }
    }
}
