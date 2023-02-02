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
    public interface IReporteAreaService
    {
        Task<ReporteAreaModel> GetReporteAreaById(ReporteAreaModel ReporteAreaModel);
        Task<List<ReporteAreaModel>> GetReporteAreas();
        Task<List<ReporteAreaModel>> GetReporteAreasByReporteId(ReporteModel reporteModel);
        Task<List<ReporteAreaModel>> GetReporteAreasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel);
        Task<ReporteAreaModel> InsertOrUpdate(ReporteAreaModel ReporteAreaModel);
        void Dispose();
    }
    public class ReporteAreaService : IReporteAreaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IReporteAreaRepository _ReporteAreaRepository;
        private ISecurityHelper _securityHelper;
        public ReporteAreaService(IMapper mapper, IMemoryCache memoryCache, ReporteAreaRepository ReporteAreaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _ReporteAreaRepository = ReporteAreaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<ReporteAreaModel> GetReporteAreaById(ReporteAreaModel ReporteAreaModel)
        {
            if (string.IsNullOrEmpty(ReporteAreaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miReporteArea = await _ReporteAreaRepository.GetReporteAreaById(_mapper.Map<ReporteArea>( ReporteAreaModel));
            return _mapper.Map<ReporteAreaModel>(miReporteArea);
        }
        public async Task<List<ReporteAreaModel>> GetReporteAreas()
        {
            var ReporteAreasList = await _ReporteAreaRepository.GetReporteAreas();
            return _mapper.Map<List<ReporteAreaModel>>(ReporteAreasList);
        }
        public async Task<List<ReporteAreaModel>> GetReporteAreasByReporteId(ReporteModel reporteModel)
        {
            if (string.IsNullOrEmpty(reporteModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miReporteArea = await _ReporteAreaRepository.GetReporteAreasByReporteId(_mapper.Map<Reporte>(reporteModel));
            return _mapper.Map<List<ReporteAreaModel>>(miReporteArea);
        }
        public async Task<List<ReporteAreaModel>> GetReporteAreasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel)
        {
            if (string.IsNullOrEmpty(segmentacionAreaModel.Id.ToString())) throw new ArgumentNullException("Id");

            var ReporteAreasList = await _ReporteAreaRepository.GetReporteAreasBySegmentacionAreaId(_mapper.Map<SegmentacionArea>(segmentacionAreaModel));
            return _mapper.Map<List<ReporteAreaModel>>(ReporteAreasList);
        }
        public async Task<ReporteAreaModel> InsertOrUpdate(ReporteAreaModel ReporteAreaModel)
        {
            if (string.IsNullOrEmpty(ReporteAreaModel.ReporteId.ToString())) throw new ArgumentNullException("ReporteId");
            if (string.IsNullOrEmpty(ReporteAreaModel.SegmentacionAreaId.ToString())) throw new ArgumentNullException("SegmentacionAreaId");
            if (string.IsNullOrEmpty(ReporteAreaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            if (!ReporteAreaModel.Id.Equals(Guid.Empty))
            {
                var miReporteReporteArea = await _ReporteAreaRepository.GetReporteAreaById(_mapper.Map<ReporteArea>(ReporteAreaModel));
                ReporteAreaModel.FechaCreacion = miReporteReporteArea.FechaCreacion;
            }

            var retorno = await _ReporteAreaRepository.InsertOrUpdate(_mapper.Map<ReporteArea>(ReporteAreaModel));
            return _mapper.Map<ReporteAreaModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_ReporteAreaRepository != null)
            {
                _ReporteAreaRepository.Dispose();
                _ReporteAreaRepository = null;
            }
        }

    }

}
