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
    public interface IEvaluacionService
    {
        Task<EvaluacionModel> GetEvaluacionById(EvaluacionModel EvaluacionModel);
        Task<List<EvaluacionModel>> GetEvaluacions();
        Task<EvaluacionModel> GetEvaluacionsByUsuarioId(UsuarioModel UsuarioModel);
        Task<List<EvaluacionModel>> GetEvaluacionsByEmpresaId(EmpresaModel EmpresaMode);
        Task<EvaluacionModel> InsertOrUpdate(EvaluacionModel EvaluacionModel);
        void Dispose();
    }
    public class EvaluacionService : IEvaluacionService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IEvaluacionRepository _EvaluacionRepository;
        private ISecurityHelper _securityHelper;
        public EvaluacionService(IMapper mapper, IMemoryCache memoryCache, EvaluacionRepository EvaluacionRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _EvaluacionRepository = EvaluacionRepository;
            _securityHelper = securityHelper;
        }
        public async Task<EvaluacionModel> GetEvaluacionById(EvaluacionModel EvaluacionModel)
        {
            if (string.IsNullOrEmpty(EvaluacionModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miEvaluacion = await _EvaluacionRepository.GetEvaluacionById(_mapper.Map<Evaluacion>( EvaluacionModel));
            return _mapper.Map<EvaluacionModel>(miEvaluacion);
        }
        public async Task<List<EvaluacionModel>> GetEvaluacions()
        {
            var EvaluacionsList = await _EvaluacionRepository.GetEvaluacions();
            return _mapper.Map<List<EvaluacionModel>>(EvaluacionsList);
        }
        public async Task<EvaluacionModel> GetEvaluacionsByUsuarioId(UsuarioModel UsuarioModel)
        {
            if (string.IsNullOrEmpty(UsuarioModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miEvaluacion = await _EvaluacionRepository.GetEvaluacionsByUsuarioId(_mapper.Map<Usuario>(UsuarioModel));
            return _mapper.Map<EvaluacionModel>(miEvaluacion);
        }
        public async Task<List<EvaluacionModel>> GetEvaluacionsByEmpresaId(EmpresaModel EmpresaMode)
        {
            if (string.IsNullOrEmpty(EmpresaMode.Id.ToString())) throw new ArgumentNullException("Id");

            var EvaluacionsList = await _EvaluacionRepository.GetEvaluacionsByEmpresaId(_mapper.Map<Empresa>(EmpresaMode));
            return _mapper.Map<List<EvaluacionModel>>(EvaluacionsList);
        }
        public async Task<EvaluacionModel> InsertOrUpdate(EvaluacionModel EvaluacionModel)
        {
            if (string.IsNullOrEmpty(EvaluacionModel.Nombre.ToString())) throw new ArgumentNullException("Nombre");
            if (string.IsNullOrEmpty(EvaluacionModel.TiempoLimite.ToString())) throw new ArgumentNullException("TiempoLimite");
            if (string.IsNullOrEmpty(EvaluacionModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _EvaluacionRepository.InsertOrUpdate(_mapper.Map<Evaluacion>(EvaluacionModel));
            return _mapper.Map<EvaluacionModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_EvaluacionRepository != null)
            {
                _EvaluacionRepository.Dispose();
                _EvaluacionRepository = null;
            }
        }

    }

}
