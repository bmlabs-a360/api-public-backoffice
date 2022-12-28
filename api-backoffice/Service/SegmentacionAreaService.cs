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
    public interface ISegmentacionAreaService
    {
        Task<SegmentacionAreaModel> GetSegmentacionAreaById(SegmentacionAreaModel SegmentacionAreaModel);
        Task<List<SegmentacionAreaModel>> GetSegmentacionAreas();
        Task<List<SegmentacionAreaModel>> GetSegmentacionAreasByEvaluacionId(EvaluacionModel evaluacionModel);
        Task<SegmentacionAreaModel> InsertOrUpdate(SegmentacionAreaModel SegmentacionAreaModel);
        void Dispose();
    }
    public class SegmentacionAreaService : ISegmentacionAreaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private ISegmentacionAreaRepository _SegmentacionAreaRepository;
        private ISecurityHelper _securityHelper;
        public SegmentacionAreaService(IMapper mapper, IMemoryCache memoryCache, SegmentacionAreaRepository SegmentacionAreaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _SegmentacionAreaRepository = SegmentacionAreaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<SegmentacionAreaModel> GetSegmentacionAreaById(SegmentacionAreaModel SegmentacionAreaModel)
        {
            if (string.IsNullOrEmpty(SegmentacionAreaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miSegmentacionArea = await _SegmentacionAreaRepository.GetSegmentacionAreaById(_mapper.Map<SegmentacionArea>( SegmentacionAreaModel));
            return _mapper.Map<SegmentacionAreaModel>(miSegmentacionArea);
        }
        public async Task<List<SegmentacionAreaModel>> GetSegmentacionAreas()
        {
            var SegmentacionAreasList = await _SegmentacionAreaRepository.GetSegmentacionAreas();
            return _mapper.Map<List<SegmentacionAreaModel>>(SegmentacionAreasList);
        }
        public async Task<List<SegmentacionAreaModel>> GetSegmentacionAreasByEvaluacionId(EvaluacionModel evaluacionModel)
        {
            if (string.IsNullOrEmpty(evaluacionModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miSegmentacionArea = await _SegmentacionAreaRepository.GetSegmentacionAreasByEvaluacionId(_mapper.Map<Evaluacion>(evaluacionModel));
            return _mapper.Map<List<SegmentacionAreaModel>>(miSegmentacionArea);
        }
        public async Task<SegmentacionAreaModel> InsertOrUpdate(SegmentacionAreaModel SegmentacionAreaModel)
        {
            if (string.IsNullOrEmpty(SegmentacionAreaModel.EvaluacionId.ToString())) throw new ArgumentNullException("SegmentacionAreaId");
            if (string.IsNullOrEmpty(SegmentacionAreaModel.NombreArea.ToString())) throw new ArgumentNullException("TipoItemSegmentacionAreaId");
            if (string.IsNullOrEmpty(SegmentacionAreaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _SegmentacionAreaRepository.InsertOrUpdate(_mapper.Map<SegmentacionArea>(SegmentacionAreaModel));
            return _mapper.Map<SegmentacionAreaModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_SegmentacionAreaRepository != null)
            {
                _SegmentacionAreaRepository.Dispose();
                _SegmentacionAreaRepository = null;
            }
        }

    }

}
