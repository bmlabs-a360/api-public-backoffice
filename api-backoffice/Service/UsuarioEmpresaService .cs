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
    public interface IUsuarioEmpresaService
    {
        Task<UsuarioEmpresaModel> GetUsuarioEmpresaById(UsuarioEmpresaModel usuarioEmpresaModel);
        Task<UsuarioEmpresaModel> InsertOrUpdate(UsuarioEmpresaModel usuarioEmpresaModel);
        Task<List<UsuarioEmpresaModel>> GetUsuarioEmpresas();
        Task<List<UsuarioEmpresaModel>> GetUsuarioEmpresasByUsuarioId(UsuarioModel usuarioModel);
        Task<List<UsuarioEmpresaModel>> GetUsuarioEmpresasByUsuario(UsuarioModel usuarioModel);
        Task<List<UsuarioEmpresaModel>> GetUsuarioEmpresasByEmpresaId(Guid empresaId);
        

        void Dispose();
    }
    public class UsuarioEmpresaService : IUsuarioEmpresaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IUsuarioEmpresaRepository _UsuarioEmpresaRepository;
        private ISecurityHelper _securityHelper;

        public UsuarioEmpresaService(IMapper mapper, IMemoryCache memoryCache, UsuarioEmpresaRepository UsuarioEmpresaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _UsuarioEmpresaRepository = UsuarioEmpresaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<UsuarioEmpresaModel> GetUsuarioEmpresaById(UsuarioEmpresaModel usuarioEmpresaModel)
        {
            if (string.IsNullOrEmpty(usuarioEmpresaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miUsuarioEmpresa = await _UsuarioEmpresaRepository.GetUsuarioEmpresaById(_mapper.Map<UsuarioEmpresa>(usuarioEmpresaModel));
            return _mapper.Map<UsuarioEmpresaModel>(miUsuarioEmpresa);
        }

        public async Task<List<UsuarioEmpresaModel>> GetUsuarioEmpresas()
        {
            var UsuarioEmpresasList = await _UsuarioEmpresaRepository.GetUsuarioEmpresas();
            return _mapper.Map<List<UsuarioEmpresaModel>>(UsuarioEmpresasList);
        }
        public async Task<UsuarioEmpresaModel> InsertOrUpdate(UsuarioEmpresaModel usuarioEmpresaModel)
        {

            if (string.IsNullOrEmpty(usuarioEmpresaModel.UsuarioId.ToString())) throw new ArgumentNullException("Debe indicar UsuarioId");
            if (string.IsNullOrEmpty(usuarioEmpresaModel.EmpresaId.ToString())) throw new ArgumentNullException("Debe indicar EmpresaId");
            if (string.IsNullOrEmpty(usuarioEmpresaModel.Activo.ToString())) throw new ArgumentNullException("Debe indicar Activo");

            var retorno = await _UsuarioEmpresaRepository.InsertOrUpdate(_mapper.Map<UsuarioEmpresa>(usuarioEmpresaModel));
            return _mapper.Map<UsuarioEmpresaModel>(retorno);
        }


        public async Task<List<UsuarioEmpresaModel>> GetUsuarioEmpresasByUsuarioId(UsuarioModel usuarioModel)
        {

            if (string.IsNullOrEmpty(usuarioModel.Id.ToString())) throw new ArgumentNullException("Debe indicar Id de usuario");
            var UsuarioEmpresasList = await _UsuarioEmpresaRepository.GetUsuarioEmpresasByUsuarioId(_mapper.Map<Usuario>(usuarioModel));
            return _mapper.Map<List<UsuarioEmpresaModel>>(UsuarioEmpresasList);
        }

        public async Task<List<UsuarioEmpresaModel>> GetUsuarioEmpresasByUsuario(UsuarioModel usuarioModel)
        {

            if (string.IsNullOrEmpty(usuarioModel.Id.ToString())) throw new ArgumentNullException("Debe indicar Id de usuario");
            var UsuarioEmpresasList = await _UsuarioEmpresaRepository.GetUsuarioEmpresasByUsuario(_mapper.Map<Usuario>(usuarioModel));
            return _mapper.Map<List<UsuarioEmpresaModel>>(UsuarioEmpresasList);
        }
        public async Task<List<UsuarioEmpresaModel>> GetUsuarioEmpresasByEmpresaId(Guid empresaId)
        {

            if (string.IsNullOrEmpty(empresaId.ToString())) throw new ArgumentNullException("Debe indicar Id de empresa");
            var UsuarioEmpresasList = await _UsuarioEmpresaRepository.GetUsuarioEmpresasByEmpresaId(empresaId);
            return _mapper.Map<List<UsuarioEmpresaModel>>(UsuarioEmpresasList);
        }
        



        public void Dispose() 
        { 
            if (_UsuarioEmpresaRepository != null)
            {
                _UsuarioEmpresaRepository.Dispose();
                _UsuarioEmpresaRepository = null;
            }
        }

    }

}
