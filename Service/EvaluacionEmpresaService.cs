﻿using AutoMapper;
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
    public interface IEvaluacionEmpresaService
    {
        Task<EvaluacionEmpresaModel> GetEvaluacionEmpresaById(EvaluacionEmpresaModel EvaluacionEmpresaModel);
        Task<List<EvaluacionEmpresaModel>> GetEvaluacionEmpresas();
        Task<EvaluacionEmpresaModel> GetEvaluacionEmpresasByEvaluacionId(EvaluacionModel EvaluacionModel);
        Task<List<EvaluacionEmpresaModel>> GetEvaluacionEmpresasByEmpresaId(EmpresaModel EmpresaMode);
        Task<EvaluacionEmpresaModel> InsertOrUpdate(EvaluacionEmpresaModel EvaluacionEmpresaModel);
        void Dispose();
    }
    public class EvaluacionEmpresaService : IEvaluacionEmpresaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IEvaluacionEmpresaRepository _EvaluacionEmpresaRepository;
        private ISecurityHelper _securityHelper;
        public EvaluacionEmpresaService(IMapper mapper, IMemoryCache memoryCache, EvaluacionEmpresaRepository EvaluacionEmpresaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _EvaluacionEmpresaRepository = EvaluacionEmpresaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<EvaluacionEmpresaModel> GetEvaluacionEmpresaById(EvaluacionEmpresaModel EvaluacionEmpresaModel)
        {
            if (string.IsNullOrEmpty(EvaluacionEmpresaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miEvaluacionEmpresa = await _EvaluacionEmpresaRepository.GetEvaluacionEmpresaById(_mapper.Map<EvaluacionEmpresa>( EvaluacionEmpresaModel));
            return _mapper.Map<EvaluacionEmpresaModel>(miEvaluacionEmpresa);
        }
        public async Task<List<EvaluacionEmpresaModel>> GetEvaluacionEmpresas()
        {
            var EvaluacionEmpresasList = await _EvaluacionEmpresaRepository.GetEvaluacionEmpresas();
            return _mapper.Map<List<EvaluacionEmpresaModel>>(EvaluacionEmpresasList);
        }
        public async Task<EvaluacionEmpresaModel> GetEvaluacionEmpresasByEvaluacionId(EvaluacionModel EvaluacionModel)
        {
            if (string.IsNullOrEmpty(EvaluacionModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miEvaluacionEmpresa = await _EvaluacionEmpresaRepository.GetEvaluacionEmpresasByEvaluacionId(_mapper.Map<Evaluacion>(EvaluacionModel));
            return _mapper.Map<EvaluacionEmpresaModel>(miEvaluacionEmpresa);
        }
        public async Task<List<EvaluacionEmpresaModel>> GetEvaluacionEmpresasByEmpresaId(EmpresaModel EmpresaMode)
        {
            if (string.IsNullOrEmpty(EmpresaMode.Id.ToString())) throw new ArgumentNullException("Id");

            var EvaluacionEmpresasList = await _EvaluacionEmpresaRepository.GetEvaluacionEmpresasByEmpresaId(_mapper.Map<Empresa>(EmpresaMode));
            return _mapper.Map<List<EvaluacionEmpresaModel>>(EvaluacionEmpresasList);
        }
        public async Task<EvaluacionEmpresaModel> InsertOrUpdate(EvaluacionEmpresaModel EvaluacionEmpresaModel)
        {
            if (string.IsNullOrEmpty(EvaluacionEmpresaModel.EmpresaId.ToString())) throw new ArgumentNullException("EmpresaId");
            if (string.IsNullOrEmpty(EvaluacionEmpresaModel.EvaluacionId.ToString())) throw new ArgumentNullException("EvaluacionId");
            if (string.IsNullOrEmpty(EvaluacionEmpresaModel.FechaInicioTiempoLimite.ToString())) throw new ArgumentNullException("FechaInicioTiempoLimite");
            if (string.IsNullOrEmpty(EvaluacionEmpresaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _EvaluacionEmpresaRepository.InsertOrUpdate(_mapper.Map<EvaluacionEmpresa>(EvaluacionEmpresaModel));
            return _mapper.Map<EvaluacionEmpresaModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_EvaluacionEmpresaRepository != null)
            {
                _EvaluacionEmpresaRepository.Dispose();
                _EvaluacionEmpresaRepository = null;
            }
        }

    }

}
