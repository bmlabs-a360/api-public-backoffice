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
    public interface IImportanciaEstrategicaService
    {
        Task<ImportanciaEstrategicaModel> GetImportanciaEstrategicaById(ImportanciaEstrategicaModel ImportanciaEstrategicaModel);
        Task<List<ImportanciaEstrategicaModel>> GetImportanciaEstrategicas();
        Task<List<ImportanciaEstrategicaModel>> GetImportanciaEstrategicasByImportanciaRelativaId(ImportanciaRelativaModel importanciaRelativaModel);
        Task<List<ImportanciaEstrategicaModel>> GetImportanciaEstrategicasBySegmentacionSubAreaId(SegmentacionSubAreaModel segmentacionSubAreaModel);
        Task<ImportanciaEstrategicaModel> InsertOrUpdate(ImportanciaEstrategicaModel ImportanciaEstrategicaModel);
        void Dispose();
    }
    public class ImportanciaEstrategicaService : IImportanciaEstrategicaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IImportanciaEstrategicaRepository _ImportanciaEstrategicaRepository;
        private ISecurityHelper _securityHelper;
        public ImportanciaEstrategicaService(IMapper mapper, IMemoryCache memoryCache, ImportanciaEstrategicaRepository ImportanciaEstrategicaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _ImportanciaEstrategicaRepository = ImportanciaEstrategicaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<ImportanciaEstrategicaModel> GetImportanciaEstrategicaById(ImportanciaEstrategicaModel ImportanciaEstrategicaModel)
        {
            if (string.IsNullOrEmpty(ImportanciaEstrategicaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miImportanciaEstrategica = await _ImportanciaEstrategicaRepository.GetImportanciaEstrategicaById(_mapper.Map<ImportanciaEstrategica>( ImportanciaEstrategicaModel));
            return _mapper.Map<ImportanciaEstrategicaModel>(miImportanciaEstrategica);
        }
        public async Task<List<ImportanciaEstrategicaModel>> GetImportanciaEstrategicas()
        {
            var ImportanciaEstrategicasList = await _ImportanciaEstrategicaRepository.GetImportanciaEstrategicas();
            return _mapper.Map<List<ImportanciaEstrategicaModel>>(ImportanciaEstrategicasList);
        }
        public async Task<List<ImportanciaEstrategicaModel>> GetImportanciaEstrategicasByImportanciaRelativaId(ImportanciaRelativaModel importanciaRelativaModel)
        {
            if (string.IsNullOrEmpty(importanciaRelativaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miImportanciaEstrategica = await _ImportanciaEstrategicaRepository.GetImportanciaEstrategicasByImportanciaRelativaId(_mapper.Map<ImportanciaRelativa>(importanciaRelativaModel));
            return _mapper.Map<List<ImportanciaEstrategicaModel>>(miImportanciaEstrategica);
        }
        public async Task<List<ImportanciaEstrategicaModel>> GetImportanciaEstrategicasBySegmentacionSubAreaId(SegmentacionSubAreaModel segmentacionSubAreaModel)
        {
            if (string.IsNullOrEmpty(segmentacionSubAreaModel.Id.ToString())) throw new ArgumentNullException("Id");

            var ImportanciaEstrategicasList = await _ImportanciaEstrategicaRepository.GetImportanciaEstrategicasBySegmentacionSubAreaId(_mapper.Map<SegmentacionSubArea>(segmentacionSubAreaModel));
            return _mapper.Map<List<ImportanciaEstrategicaModel>>(ImportanciaEstrategicasList);
        }
        public async Task<ImportanciaEstrategicaModel> InsertOrUpdate(ImportanciaEstrategicaModel ImportanciaEstrategicaModel)
        {
            if (string.IsNullOrEmpty(ImportanciaEstrategicaModel.ImportanciaRelativaId.ToString())) throw new ArgumentNullException("ImportanciaRelativaId");
            if (string.IsNullOrEmpty(ImportanciaEstrategicaModel.SegmentacionSubAreaId.ToString())) throw new ArgumentNullException("SegmentacionSubAreaId");
            if (string.IsNullOrEmpty(ImportanciaEstrategicaModel.Valor.ToString())) throw new ArgumentNullException("Valor");
            if (string.IsNullOrEmpty(ImportanciaEstrategicaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _ImportanciaEstrategicaRepository.InsertOrUpdate(_mapper.Map<ImportanciaEstrategica>(ImportanciaEstrategicaModel));
            return _mapper.Map<ImportanciaEstrategicaModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_ImportanciaEstrategicaRepository != null)
            {
                _ImportanciaEstrategicaRepository.Dispose();
                _ImportanciaEstrategicaRepository = null;
            }
        }
    }
}
