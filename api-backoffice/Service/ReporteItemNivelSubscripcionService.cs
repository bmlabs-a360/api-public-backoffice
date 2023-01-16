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
    public interface IReporteItemNivelSubscripcionService
    {
        Task<ReporteItemNivelSubscripcionModel> GetReporteItemNivelSubscripcionById(ReporteItemNivelSubscripcionModel ReporteItemNivelSubscripcionModel);
        Task<List<ReporteItemNivelSubscripcionModel>> GetReporteItemNivelSubscripcions();
        Task<List<ReporteItemNivelSubscripcionModel>> GetReporteItemNivelSubscripcionsByReporteId(ReporteModel reporteModel);
        Task<ReporteItemNivelSubscripcionModel> InsertOrUpdate(ReporteItemNivelSubscripcionModel ReporteItemNivelSubscripcionModel);
        void Dispose();
    }
    public class ReporteItemNivelSubscripcionService : IReporteItemNivelSubscripcionService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IReporteItemNivelSubscripcionRepository _ReporteItemNivelSubscripcionRepository;
        private ISecurityHelper _securityHelper;
        public ReporteItemNivelSubscripcionService(IMapper mapper, IMemoryCache memoryCache, ReporteItemNivelSubscripcionRepository ReporteItemNivelSubscripcionRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _ReporteItemNivelSubscripcionRepository = ReporteItemNivelSubscripcionRepository;
            _securityHelper = securityHelper;
        }
        public async Task<ReporteItemNivelSubscripcionModel> GetReporteItemNivelSubscripcionById(ReporteItemNivelSubscripcionModel ReporteItemNivelSubscripcionModel)
        {
            if (string.IsNullOrEmpty(ReporteItemNivelSubscripcionModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miReporteItemNivelSubscripcion = await _ReporteItemNivelSubscripcionRepository.GetReporteItemNivelSubscripcionById(_mapper.Map<ReporteItemNivelSubscripcion>(ReporteItemNivelSubscripcionModel));
            return _mapper.Map<ReporteItemNivelSubscripcionModel>(miReporteItemNivelSubscripcion);
        }
        public async Task<List<ReporteItemNivelSubscripcionModel>> GetReporteItemNivelSubscripcions()
        {
            var ReporteItemNivelSubscripcionsList = await _ReporteItemNivelSubscripcionRepository.GetReporteItemNivelSubscripcions();
            return _mapper.Map<List<ReporteItemNivelSubscripcionModel>>(ReporteItemNivelSubscripcionsList);
        }
        public async Task<List<ReporteItemNivelSubscripcionModel>> GetReporteItemNivelSubscripcionsByReporteId(ReporteModel reporteModel)
        {
            if (string.IsNullOrEmpty(reporteModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miReporteItemNivelSubscripcion = await _ReporteItemNivelSubscripcionRepository.GetReporteItemNivelSubscripcionsByReporteId(_mapper.Map<Reporte>(reporteModel));
            return _mapper.Map<List<ReporteItemNivelSubscripcionModel>>(miReporteItemNivelSubscripcion);
        }
        public async Task<ReporteItemNivelSubscripcionModel> InsertOrUpdate(ReporteItemNivelSubscripcionModel ReporteItemNivelSubscripcionModel)
        {
            if (string.IsNullOrEmpty(ReporteItemNivelSubscripcionModel.ReporteId.ToString())) throw new ArgumentNullException("ReporteId");
            if (string.IsNullOrEmpty(ReporteItemNivelSubscripcionModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _ReporteItemNivelSubscripcionRepository.InsertOrUpdate(_mapper.Map<ReporteItemNivelSubscripcion>(ReporteItemNivelSubscripcionModel));
            return _mapper.Map<ReporteItemNivelSubscripcionModel>(retorno);
        }
        public void Dispose()
        {
            if (_ReporteItemNivelSubscripcionRepository != null)
            {
                _ReporteItemNivelSubscripcionRepository.Dispose();
                _ReporteItemNivelSubscripcionRepository = null;
            }
        }

    }

}
