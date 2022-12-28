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
    public interface IReporteService
    {
        Task<ReporteModel> GetReporteById(ReporteModel ReporteModel);
        Task<List<ReporteModel>> GetReportes();
        Task<List<ReporteModel>> GetReportesByEvaluacionId(EvaluacionModel evaluacionModel);
        Task<ReporteModel> InsertOrUpdate(ReporteModel ReporteModel);
        void Dispose();
    }
    public class ReporteService : IReporteService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IReporteRepository _ReporteRepository;
        private ISecurityHelper _securityHelper;
        public ReporteService(IMapper mapper, IMemoryCache memoryCache, ReporteRepository ReporteRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _ReporteRepository = ReporteRepository;
            _securityHelper = securityHelper;
        }
        public async Task<ReporteModel> GetReporteById(ReporteModel ReporteModel)
        {
            if (string.IsNullOrEmpty(ReporteModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miReporte = await _ReporteRepository.GetReporteById(_mapper.Map<Reporte>( ReporteModel));
            return _mapper.Map<ReporteModel>(miReporte);
        }
        public async Task<List<ReporteModel>> GetReportes()
        {
            var ReportesList = await _ReporteRepository.GetReportes();
            return _mapper.Map<List<ReporteModel>>(ReportesList);
        }
        public async Task<List<ReporteModel>> GetReportesByEvaluacionId(EvaluacionModel evaluacionModel)
        {
            if (string.IsNullOrEmpty(evaluacionModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miReporte = await _ReporteRepository.GetReportesByEvaluacionId(_mapper.Map<Evaluacion>(evaluacionModel));
            return _mapper.Map<List<ReporteModel>>(miReporte);
        }
        public async Task<ReporteModel> InsertOrUpdate(ReporteModel ReporteModel)
        {
            if (string.IsNullOrEmpty(ReporteModel.EvaluacionId.ToString())) throw new ArgumentNullException("ReporteId");
            if (string.IsNullOrEmpty(ReporteModel.Nombre.ToString())) throw new ArgumentNullException("TipoItemReporteId");
            if (string.IsNullOrEmpty(ReporteModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _ReporteRepository.InsertOrUpdate(_mapper.Map<Reporte>(ReporteModel));
            return _mapper.Map<ReporteModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_ReporteRepository != null)
            {
                _ReporteRepository.Dispose();
                _ReporteRepository = null;
            }
        }

    }

}
