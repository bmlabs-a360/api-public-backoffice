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
    public interface IImportanciaRelativaService
    {
        Task<ImportanciaRelativaModel> GetImportanciaRelativaById(ImportanciaRelativaModel ImportanciaRelativaModel);
        Task<List<ImportanciaRelativaModel>> GetImportanciaRelativas();
        Task<List<ImportanciaRelativaModel>> GetImportanciaRelativasByEvaluacionEmpresaId(EvaluacionEmpresaModel evaluacionEmpresaModel);
        Task<List<ImportanciaRelativaModel>> GetImportanciaRelativasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel);
        Task<ImportanciaRelativaModel> InsertOrUpdate(ImportanciaRelativaModel ImportanciaRelativaModel);
        void Dispose();
    }
    public class ImportanciaRelativaService : IImportanciaRelativaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IImportanciaRelativaRepository _ImportanciaRelativaRepository;
        private ISecurityHelper _securityHelper;
        public ImportanciaRelativaService(IMapper mapper, IMemoryCache memoryCache, ImportanciaRelativaRepository ImportanciaRelativaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _ImportanciaRelativaRepository = ImportanciaRelativaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<ImportanciaRelativaModel> GetImportanciaRelativaById(ImportanciaRelativaModel ImportanciaRelativaModel)
        {
            if (string.IsNullOrEmpty(ImportanciaRelativaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miImportanciaRelativa = await _ImportanciaRelativaRepository.GetImportanciaRelativaById(_mapper.Map<ImportanciaRelativa>( ImportanciaRelativaModel));
            return _mapper.Map<ImportanciaRelativaModel>(miImportanciaRelativa);
        }
        public async Task<List<ImportanciaRelativaModel>> GetImportanciaRelativas()
        {
            var ImportanciaRelativasList = await _ImportanciaRelativaRepository.GetImportanciaRelativas();
            return _mapper.Map<List<ImportanciaRelativaModel>>(ImportanciaRelativasList);
        }
        public async Task<List<ImportanciaRelativaModel>> GetImportanciaRelativasByEvaluacionEmpresaId(EvaluacionEmpresaModel evaluacionEmpresaModel)
        {
            if (string.IsNullOrEmpty(evaluacionEmpresaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miImportanciaRelativa = await _ImportanciaRelativaRepository.GetImportanciaRelativasByEvaluacionEmpresaId(_mapper.Map<EvaluacionEmpresa>(evaluacionEmpresaModel));
            return _mapper.Map<List<ImportanciaRelativaModel>>(miImportanciaRelativa);
        }
        public async Task<List<ImportanciaRelativaModel>> GetImportanciaRelativasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel)
        {
            if (string.IsNullOrEmpty(segmentacionAreaModel.Id.ToString())) throw new ArgumentNullException("Id");

            var ImportanciaRelativasList = await _ImportanciaRelativaRepository.GetImportanciaRelativasBySegmentacionAreaId(_mapper.Map<SegmentacionArea>(segmentacionAreaModel));
            return _mapper.Map<List<ImportanciaRelativaModel>>(ImportanciaRelativasList);
        }
        public async Task<ImportanciaRelativaModel> InsertOrUpdate(ImportanciaRelativaModel ImportanciaRelativaModel)
        {
            if (string.IsNullOrEmpty(ImportanciaRelativaModel.EvaluacionEmpresaId.ToString())) throw new ArgumentNullException("EvaluacionEmpresaId");
            if (string.IsNullOrEmpty(ImportanciaRelativaModel.SegmentacionAreaId.ToString())) throw new ArgumentNullException("SegmentacionAreaId");
            if (string.IsNullOrEmpty(ImportanciaRelativaModel.Valor.ToString())) throw new ArgumentNullException("Valor");
            if (string.IsNullOrEmpty(ImportanciaRelativaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _ImportanciaRelativaRepository.InsertOrUpdate(_mapper.Map<ImportanciaRelativa>(ImportanciaRelativaModel));
            return _mapper.Map<ImportanciaRelativaModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_ImportanciaRelativaRepository != null)
            {
                _ImportanciaRelativaRepository.Dispose();
                _ImportanciaRelativaRepository = null;
            }
        }
    }
}
