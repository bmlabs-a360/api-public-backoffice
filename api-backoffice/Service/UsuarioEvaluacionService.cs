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
    public interface IUsuarioEvaluacionService
    {
        Task<UsuarioEvaluacionModel> GetUsuarioEvaluacionById(UsuarioEvaluacionModel UsuarioEvaluacionModel);
        Task<UsuarioEvaluacionModel> InsertOrUpdate(UsuarioEvaluacionModel UsuarioEvaluacionModel);
        Task<List<UsuarioEvaluacionModel>> GetUsuarioEvaluacions();
        Task<List<UsuarioEvaluacionModel>> GetUsuarioEvaluacionsByUsuarioId(UsuarioModel usuarioModel);
        Task<List<UsuarioEvaluacionModel>> GetUsuarioEvaluacionsByEmpresaId(EmpresaModel empresaModel);
        Task<List<UsuarioEvaluacionModel>> GetUsuarioEvaluacionsByEvaluacionId(EvaluacionModel evaluacionModel);
        void Dispose();
    }
    public class UsuarioEvaluacionService : IUsuarioEvaluacionService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IUsuarioEvaluacionRepository _UsuarioEvaluacionRepository;
        private ISecurityHelper _securityHelper;

        public UsuarioEvaluacionService(IMapper mapper, IMemoryCache memoryCache, UsuarioEvaluacionRepository UsuarioEvaluacionRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _UsuarioEvaluacionRepository = UsuarioEvaluacionRepository;
            _securityHelper = securityHelper;
        }
        public async Task<UsuarioEvaluacionModel> GetUsuarioEvaluacionById(UsuarioEvaluacionModel UsuarioEvaluacionModel)
        {
            if (string.IsNullOrEmpty(UsuarioEvaluacionModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miUsuarioEvaluacion = await _UsuarioEvaluacionRepository.GetUsuarioEvaluacionById(_mapper.Map<UsuarioEvaluacion>(UsuarioEvaluacionModel));
            return _mapper.Map<UsuarioEvaluacionModel>(miUsuarioEvaluacion);
        }
        public async Task<List<UsuarioEvaluacionModel>> GetUsuarioEvaluacions()
        {
            var UsuarioEvaluacionsList = await _UsuarioEvaluacionRepository.GetUsuarioEvaluacions();
            return _mapper.Map<List<UsuarioEvaluacionModel>>(UsuarioEvaluacionsList);
        }
        public async Task<UsuarioEvaluacionModel> InsertOrUpdate(UsuarioEvaluacionModel UsuarioEvaluacionModel)
        {

            if (string.IsNullOrEmpty(UsuarioEvaluacionModel.UsuarioId.ToString())) throw new ArgumentNullException("Debe indicar UsuarioId");
            if (string.IsNullOrEmpty(UsuarioEvaluacionModel.EmpresaId.ToString())) throw new ArgumentNullException("Debe indicar EmpresaId");
            if (string.IsNullOrEmpty(UsuarioEvaluacionModel.Activo.ToString())) throw new ArgumentNullException("Debe indicar Activo");

            var retorno = await _UsuarioEvaluacionRepository.InsertOrUpdate(_mapper.Map<UsuarioEvaluacion>(UsuarioEvaluacionModel));
            return _mapper.Map<UsuarioEvaluacionModel>(retorno);
        }
        public async Task<List<UsuarioEvaluacionModel>> GetUsuarioEvaluacionsByUsuarioId(UsuarioModel usuarioModel)
        {

            if (string.IsNullOrEmpty(usuarioModel.Id.ToString())) throw new ArgumentNullException("Debe indicar Id de usuario");
            var UsuarioEvaluacionsList = await _UsuarioEvaluacionRepository.GetUsuarioEvaluacionsByUsuarioId(_mapper.Map<Usuario>(usuarioModel));
            return _mapper.Map<List<UsuarioEvaluacionModel>>(UsuarioEvaluacionsList);
        }
        public async Task<List<UsuarioEvaluacionModel>> GetUsuarioEvaluacionsByEmpresaId(EmpresaModel empresaModel)
        {

            if (string.IsNullOrEmpty(empresaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var UsuarioEvaluacionsList = await _UsuarioEvaluacionRepository.GetUsuarioEvaluacionsByEmpresaId(_mapper.Map<Empresa>(empresaModel));
            return _mapper.Map<List<UsuarioEvaluacionModel>>(UsuarioEvaluacionsList);
        }
        public async Task<List<UsuarioEvaluacionModel>> GetUsuarioEvaluacionsByEvaluacionId(EvaluacionModel evaluacionModel)
        {

            if (string.IsNullOrEmpty(evaluacionModel.Id.ToString())) throw new ArgumentNullException("Id");
            var UsuarioEvaluacionsList = await _UsuarioEvaluacionRepository.GetUsuarioEvaluacionsByEvaluacionId(_mapper.Map<Evaluacion>(evaluacionModel));
            return _mapper.Map<List<UsuarioEvaluacionModel>>(UsuarioEvaluacionsList);
        }
        public void Dispose() 
        { 
            if (_UsuarioEvaluacionRepository != null)
            {
                _UsuarioEvaluacionRepository.Dispose();
                _UsuarioEvaluacionRepository = null;
            }
        }

    }

}
