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
    public interface IUsuarioSuscripcionService
    {
        Task<UsuarioSuscripcionModel> GetUsuarioSuscripcionById(UsuarioSuscripcionModel UsuarioSuscripcionModel);
        Task<UsuarioSuscripcionModel> InsertOrUpdate(UsuarioSuscripcionModel UsuarioSuscripcionModel);
        Task<List<UsuarioSuscripcionModel>> GetUsuarioSuscripcions();
        Task<UsuarioSuscripcionModel> GetUsuarioSuscripcionsByUsuarioId(UsuarioModel usuarioModel);


        void Dispose();
    }
    public class UsuarioSuscripcionService : IUsuarioSuscripcionService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IUsuarioSuscripcionRepository _UsuarioSuscripcionRepository;
        private ISecurityHelper _securityHelper;

        public UsuarioSuscripcionService(IMapper mapper, IMemoryCache memoryCache, UsuarioSuscripcionRepository UsuarioSuscripcionRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _UsuarioSuscripcionRepository = UsuarioSuscripcionRepository;
            _securityHelper = securityHelper;
        }
        public async Task<UsuarioSuscripcionModel> GetUsuarioSuscripcionById(UsuarioSuscripcionModel UsuarioSuscripcionModel)
        {
            if (string.IsNullOrEmpty(UsuarioSuscripcionModel.Id.ToString())) throw new ArgumentNullException("Debe indicar UsuarioSuscripcion ID");
            var miUsuarioSuscripcion = await _UsuarioSuscripcionRepository.GetUsuarioSuscripcionById(_mapper.Map<UsuarioSuscripcion>(UsuarioSuscripcionModel));
            return _mapper.Map<UsuarioSuscripcionModel>(miUsuarioSuscripcion);
        }
        public async Task<List<UsuarioSuscripcionModel>> GetUsuarioSuscripcions()
        {
            var UsuarioSuscripcionsList = await _UsuarioSuscripcionRepository.GetUsuarioSuscripcions();
            return _mapper.Map<List<UsuarioSuscripcionModel>>(UsuarioSuscripcionsList);
        }
        public async Task<UsuarioSuscripcionModel> InsertOrUpdate(UsuarioSuscripcionModel UsuarioSuscripcionModel)
        {
            if (string.IsNullOrEmpty(UsuarioSuscripcionModel.UsuarioId.ToString())) throw new ArgumentNullException("Debe indicar UsuarioId");
            if (string.IsNullOrEmpty(UsuarioSuscripcionModel.FechaExpiracion.ToString())) throw new ArgumentNullException("Debe indicar FechaExpiracion");
            if (string.IsNullOrEmpty(UsuarioSuscripcionModel.TiempoSuscripcion.ToString())) throw new ArgumentNullException("Debe indicar TiempoSuscripcion");
            if (string.IsNullOrEmpty(UsuarioSuscripcionModel.Activo.ToString())) throw new ArgumentNullException("Debe indicar Activo");

            var retorno = await _UsuarioSuscripcionRepository.InsertOrUpdate(_mapper.Map<UsuarioSuscripcion>(UsuarioSuscripcionModel));
            return _mapper.Map<UsuarioSuscripcionModel>(retorno);
        }
        public async Task<UsuarioSuscripcionModel> GetUsuarioSuscripcionsByUsuarioId(UsuarioModel usuarioModel)
        {

            if (string.IsNullOrEmpty(usuarioModel.Id.ToString())) throw new ArgumentNullException("Debe indicar Id de usuario");
            var UsuarioSuscripcionsList = await _UsuarioSuscripcionRepository.GetUsuarioSuscripcionsByUsuarioId(_mapper.Map<Usuario>(usuarioModel));
            return _mapper.Map<UsuarioSuscripcionModel>(UsuarioSuscripcionsList);
        }





        public void Dispose() 
        { 
            if (_UsuarioSuscripcionRepository != null)
            {
                _UsuarioSuscripcionRepository.Dispose();
                _UsuarioSuscripcionRepository = null;
            }
        }

    }

}
