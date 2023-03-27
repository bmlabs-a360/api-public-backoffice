using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using api_public_backOffice.Models;
using api_public_backOffice.Repository;
using api_public_backOffice.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using neva.entities;

namespace api_public_backOffice.Service
{
    public interface IPerfilService
    {
        Task<PerfilModel> GetPerfilById(PerfilModel PerfilModel);
        Task<PerfilModel> InsertOrUpdate(PerfilModel PerfilModel);
        Task<List<PerfilModel>> GetPerfils();
        Task<List<PerfilModel>> GetPerfilsConsultor();
        Task<PerfilModel> GetPerfilsUsuarioPro();


        void Dispose();
    }
    public class PerfilService : IPerfilService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IPerfilRepository _PerfilRepository;
        private ISecurityHelper _securityHelper;

        public PerfilService(IMapper mapper, IMemoryCache memoryCache, PerfilRepository PerfilRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _PerfilRepository = PerfilRepository;
            _securityHelper = securityHelper;
        }
        public async Task<PerfilModel> GetPerfilById(PerfilModel PerfilModel)
        {
            if (string.IsNullOrEmpty(PerfilModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miPerfil = await _PerfilRepository.GetPerfilById(_mapper.Map<Perfil>(PerfilModel));
            return _mapper.Map<PerfilModel>(miPerfil);
        }

        public async Task<List<PerfilModel>> GetPerfils()
        {
            var PerfilsList = await _PerfilRepository.GetPerfils();
            return _mapper.Map<List<PerfilModel>>(PerfilsList);
        }
        public async Task<PerfilModel> InsertOrUpdate(PerfilModel PerfilModel)
        {
            if (string.IsNullOrEmpty(PerfilModel.Nombre)) throw new ArgumentNullException("Debe indicar Nombre");
            if (string.IsNullOrEmpty(PerfilModel.Detalle)) throw new ArgumentNullException("Debe indicar Detalle");
            if (string.IsNullOrEmpty(PerfilModel.Activo.ToString())) throw new ArgumentNullException("Debe indicar Activo");

            var retorno = await _PerfilRepository.InsertOrUpdate(_mapper.Map<Perfil>(PerfilModel));
            return _mapper.Map<PerfilModel>(retorno);
        }
        public async Task<List<PerfilModel>> GetPerfilsConsultor()
        {
            var PerfilsList = await _PerfilRepository.GetPerfilsConsultor();
            return _mapper.Map<List<PerfilModel>>(PerfilsList);
        }
        public async Task<PerfilModel> GetPerfilsUsuarioPro()
        {
            var miPerfil = await _PerfilRepository.GetPerfilsUsuarioPro();
            return _mapper.Map<PerfilModel>(miPerfil);
        }
        public void Dispose() 
        { 
            if (_PerfilRepository != null)
            {
                _PerfilRepository.Dispose();
                _PerfilRepository = null;
            }
        }

    }

}
