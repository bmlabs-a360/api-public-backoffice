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
    public interface ISegmentacionSubAreaService
    {
        Task<SegmentacionSubAreaModel> GetSegmentacionSubAreaById(SegmentacionSubAreaModel SegmentacionSubAreaModel);
        Task<List<SegmentacionSubAreaModel>> GetSegmentacionSubAreas();
        Task<List<SegmentacionSubAreaModel>> GetSegmentacionSubAreasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel);
        Task<SegmentacionSubAreaModel> InsertOrUpdate(SegmentacionSubAreaModel SegmentacionSubAreaModel);
        void Dispose();
    }
    public class SegmentacionSubAreaService : ISegmentacionSubAreaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private ISegmentacionSubAreaRepository _SegmentacionSubAreaRepository;
        private ISecurityHelper _securityHelper;
        public SegmentacionSubAreaService(IMapper mapper, IMemoryCache memoryCache, SegmentacionSubAreaRepository SegmentacionSubAreaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _SegmentacionSubAreaRepository = SegmentacionSubAreaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<SegmentacionSubAreaModel> GetSegmentacionSubAreaById(SegmentacionSubAreaModel SegmentacionSubAreaModel)
        {
            if (string.IsNullOrEmpty(SegmentacionSubAreaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miSegmentacionSubArea = await _SegmentacionSubAreaRepository.GetSegmentacionSubAreaById(_mapper.Map<SegmentacionSubArea>( SegmentacionSubAreaModel));
            return _mapper.Map<SegmentacionSubAreaModel>(miSegmentacionSubArea);
        }
        public async Task<List<SegmentacionSubAreaModel>> GetSegmentacionSubAreas()
        {
            var SegmentacionSubAreasList = await _SegmentacionSubAreaRepository.GetSegmentacionSubAreas();
            return _mapper.Map<List<SegmentacionSubAreaModel>>(SegmentacionSubAreasList);
        }
        public async Task<List<SegmentacionSubAreaModel>> GetSegmentacionSubAreasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel)
        {
            if (string.IsNullOrEmpty(segmentacionAreaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miSegmentacionSubArea = await _SegmentacionSubAreaRepository.GetSegmentacionSubAreasBySegmentacionAreaId(_mapper.Map<SegmentacionArea>(segmentacionAreaModel));
            return _mapper.Map<List<SegmentacionSubAreaModel>>(miSegmentacionSubArea);
        }
        public async Task<SegmentacionSubAreaModel> InsertOrUpdate(SegmentacionSubAreaModel SegmentacionSubAreaModel)
        {
            if (string.IsNullOrEmpty(SegmentacionSubAreaModel.SegmentacionAreaId.ToString())) throw new ArgumentNullException("SegmentacionAreaId");
            if (string.IsNullOrEmpty(SegmentacionSubAreaModel.NombreSubArea.ToString())) throw new ArgumentNullException("NombreSubArea");
            if (string.IsNullOrEmpty(SegmentacionSubAreaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _SegmentacionSubAreaRepository.InsertOrUpdate(_mapper.Map<SegmentacionSubArea>(SegmentacionSubAreaModel));
            return _mapper.Map<SegmentacionSubAreaModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_SegmentacionSubAreaRepository != null)
            {
                _SegmentacionSubAreaRepository.Dispose();
                _SegmentacionSubAreaRepository = null;
            }
        }

    }

}
