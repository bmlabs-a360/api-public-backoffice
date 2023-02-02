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
    public interface IReporteRecomendacionAreaService
    {
        Task<ReporteRecomendacionAreaModel> GetReporteRecomendacionAreaById(ReporteRecomendacionAreaModel ReporteRecomendacionAreaModel);
        Task<List<ReporteRecomendacionAreaModel>> GetReporteRecomendacionAreas();
        Task<List<ReporteRecomendacionAreaModel>> GetReporteRecomendacionAreasByReporteId(ReporteModel reporteModel);
        Task<List<ReporteRecomendacionAreaModel>> GetReporteRecomendacionAreasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel);
        Task<ReporteRecomendacionAreaModel> InsertOrUpdate(ReporteRecomendacionAreaModel ReporteRecomendacionAreaModel);
        void Dispose();
    }
    public class ReporteRecomendacionAreaService : IReporteRecomendacionAreaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IReporteRecomendacionAreaRepository _ReporteRecomendacionAreaRepository;
        private ISecurityHelper _securityHelper;
        public ReporteRecomendacionAreaService(IMapper mapper, IMemoryCache memoryCache, ReporteRecomendacionAreaRepository ReporteRecomendacionAreaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _ReporteRecomendacionAreaRepository = ReporteRecomendacionAreaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<ReporteRecomendacionAreaModel> GetReporteRecomendacionAreaById(ReporteRecomendacionAreaModel ReporteRecomendacionAreaModel)
        {
            if (string.IsNullOrEmpty(ReporteRecomendacionAreaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miReporteRecomendacionArea = await _ReporteRecomendacionAreaRepository.GetReporteRecomendacionAreaById(_mapper.Map<ReporteRecomendacionArea>(ReporteRecomendacionAreaModel));
            return _mapper.Map<ReporteRecomendacionAreaModel>(miReporteRecomendacionArea);
        }
        public async Task<List<ReporteRecomendacionAreaModel>> GetReporteRecomendacionAreas()
        {
            var ReporteRecomendacionAreasList = await _ReporteRecomendacionAreaRepository.GetReporteRecomendacionAreas();
            return _mapper.Map<List<ReporteRecomendacionAreaModel>>(ReporteRecomendacionAreasList);
        }
        public async Task<List<ReporteRecomendacionAreaModel>> GetReporteRecomendacionAreasByReporteId(ReporteModel reporteModel)
        {
            if (string.IsNullOrEmpty(reporteModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miReporteRecomendacionArea = await _ReporteRecomendacionAreaRepository.GetReporteRecomendacionAreasByReporteId(_mapper.Map<Reporte>(reporteModel));
            return _mapper.Map<List<ReporteRecomendacionAreaModel>>(miReporteRecomendacionArea);
        }
        public async Task<List<ReporteRecomendacionAreaModel>> GetReporteRecomendacionAreasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel)
        {
            if (string.IsNullOrEmpty(segmentacionAreaModel.Id.ToString())) throw new ArgumentNullException("Id");

            var ReporteRecomendacionAreasList = await _ReporteRecomendacionAreaRepository.GetReporteRecomendacionAreasBySegmentacionAreaId(_mapper.Map<SegmentacionArea>(segmentacionAreaModel));
            return _mapper.Map<List<ReporteRecomendacionAreaModel>>(ReporteRecomendacionAreasList);
        }
        public async Task<ReporteRecomendacionAreaModel> InsertOrUpdate(ReporteRecomendacionAreaModel ReporteRecomendacionAreaModel)
        {
            if (string.IsNullOrEmpty(ReporteRecomendacionAreaModel.ReporteId.ToString())) throw new ArgumentNullException("ReporteId");
            if (string.IsNullOrEmpty(ReporteRecomendacionAreaModel.SegmentacionAreaId.ToString())) throw new ArgumentNullException("SegmentacionAreaId");
            if (string.IsNullOrEmpty(ReporteRecomendacionAreaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            if (!ReporteRecomendacionAreaModel.Id.Equals(Guid.Empty))
            {
                var miReporteReporteRecomendacionArea = await _ReporteRecomendacionAreaRepository.GetReporteRecomendacionAreaById(_mapper.Map<ReporteRecomendacionArea>(ReporteRecomendacionAreaModel));
                ReporteRecomendacionAreaModel.FechaCreacion = miReporteReporteRecomendacionArea.FechaCreacion;
            }

            var retorno = await _ReporteRecomendacionAreaRepository.InsertOrUpdate(_mapper.Map<ReporteRecomendacionArea>(ReporteRecomendacionAreaModel));
            return _mapper.Map<ReporteRecomendacionAreaModel>(retorno);
        }
        public void Dispose()
        {
            if (_ReporteRecomendacionAreaRepository != null)
            {
                _ReporteRecomendacionAreaRepository.Dispose();
                _ReporteRecomendacionAreaRepository = null;
            }
        }

    }

}
