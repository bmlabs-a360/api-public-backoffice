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
    public interface IUsuarioAreaService
    {
        Task<UsuarioAreaModel> GetUsuarioAreaById(UsuarioAreaModel UsuarioAreaModel);
        Task<List<UsuarioAreaModel>> GetUsuarioAreas();
        Task<List<UsuarioAreaModel>> GetUsuarioAreasByUsuarioEvaluacionId(UsuarioEvaluacionModel usuarioEvaluacionModel);
        Task<List<UsuarioAreaModel>> GetUsuarioAreasByUsuarioSegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel);
        Task<UsuarioAreaModel> InsertOrUpdate(UsuarioAreaModel UsuarioAreaModel);
        void Dispose();
    }
    public class UsuarioAreaService : IUsuarioAreaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IUsuarioAreaRepository _UsuarioAreaRepository;
        private ISecurityHelper _securityHelper;
        public UsuarioAreaService(IMapper mapper, IMemoryCache memoryCache, UsuarioAreaRepository UsuarioAreaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _UsuarioAreaRepository = UsuarioAreaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<UsuarioAreaModel> GetUsuarioAreaById(UsuarioAreaModel UsuarioAreaModel)
        {
            if (string.IsNullOrEmpty(UsuarioAreaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miUsuarioArea = await _UsuarioAreaRepository.GetUsuarioAreaById(_mapper.Map<UsuarioArea>( UsuarioAreaModel));
            return _mapper.Map<UsuarioAreaModel>(miUsuarioArea);
        }
        public async Task<List<UsuarioAreaModel>> GetUsuarioAreas()
        {
            var UsuarioAreasList = await _UsuarioAreaRepository.GetUsuarioAreas();
            return _mapper.Map<List<UsuarioAreaModel>>(UsuarioAreasList);
        }

        public async Task<List<UsuarioAreaModel>> GetUsuarioAreasByUsuarioEvaluacionId(UsuarioEvaluacionModel usuarioEvaluacionModel)
        {
            if (string.IsNullOrEmpty(usuarioEvaluacionModel.Id.ToString())) throw new ArgumentNullException("id");
            var UsuarioAreasList = await _UsuarioAreaRepository.GetUsuarioAreasByUsuarioEvaluacionId(_mapper.Map <UsuarioEvaluacion>(usuarioEvaluacionModel));
            return _mapper.Map<List<UsuarioAreaModel>>(UsuarioAreasList);
        }
        public async Task<List<UsuarioAreaModel>> GetUsuarioAreasByUsuarioSegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel)
        {
            if (string.IsNullOrEmpty(segmentacionAreaModel.Id.ToString())) throw new ArgumentNullException("id");
            var UsuarioAreasList = await _UsuarioAreaRepository.GetUsuarioAreasByUsuarioSegmentacionAreaId(_mapper.Map<SegmentacionArea>(segmentacionAreaModel));
            return _mapper.Map<List<UsuarioAreaModel>>(UsuarioAreasList);
        }
        public async Task<UsuarioAreaModel> InsertOrUpdate(UsuarioAreaModel UsuarioAreaModel)
        {
            if (string.IsNullOrEmpty(UsuarioAreaModel.SegmentacionAreaId.ToString())) throw new ArgumentNullException("SegmentacionAreaId");
            if (string.IsNullOrEmpty(UsuarioAreaModel.UsuarioEvaluacionId.ToString())) throw new ArgumentNullException("UsuarioEvaluacionId");
            if (string.IsNullOrEmpty(UsuarioAreaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _UsuarioAreaRepository.InsertOrUpdate(_mapper.Map<UsuarioArea>(UsuarioAreaModel));
            return _mapper.Map<UsuarioAreaModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_UsuarioAreaRepository != null)
            {
                _UsuarioAreaRepository.Dispose();
                _UsuarioAreaRepository = null;
            }
        }

    }

}
