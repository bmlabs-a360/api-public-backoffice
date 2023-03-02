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
    public interface IPlanMejoraService
    {
        Task<PlanMejoraModel> GetPlanMejoraById(PlanMejoraModel PlanMejoraModel);
        Task<List<PlanMejoraModel>> GetPlanMejoras();
        Task<List<PlanMejoraModel>> GetPlanMejorasByPreguntaId(PreguntaModel preguntaModel);
        Task<PlanMejoraModel> InsertOrUpdate(PlanMejoraModel PlanMejoraModel);
        Task PlanMejoraInsertOrUpdate(PlanMejoraModel planMejoraModel);
        void Dispose();
    }
    public class PlanMejoraService : IPlanMejoraService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IPlanMejoraRepository _PlanMejoraRepository;
        private ISecurityHelper _securityHelper;
        public PlanMejoraService(IMapper mapper, IMemoryCache memoryCache, PlanMejoraRepository PlanMejoraRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _PlanMejoraRepository = PlanMejoraRepository;
            _securityHelper = securityHelper;
        }
        public async Task<PlanMejoraModel> GetPlanMejoraById(PlanMejoraModel PlanMejoraModel)
        {
            if (string.IsNullOrEmpty(PlanMejoraModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miPlanMejora = await _PlanMejoraRepository.GetPlanMejoraById(_mapper.Map<PlanMejora>( PlanMejoraModel));
            return _mapper.Map<PlanMejoraModel>(miPlanMejora);
        }
        public async Task<List<PlanMejoraModel>> GetPlanMejoras()
        {
            var PlanMejorasList = await _PlanMejoraRepository.GetPlanMejoras();
            return _mapper.Map<List<PlanMejoraModel>>(PlanMejorasList);
        }
        public async Task<List<PlanMejoraModel>> GetPlanMejorasByPreguntaId(PreguntaModel preguntaModel)
        {
            if (string.IsNullOrEmpty(preguntaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miPlanMejora = await _PlanMejoraRepository.GetPlanMejorasByPreguntaId(_mapper.Map<Pregunta>(preguntaModel));
            return _mapper.Map< List<PlanMejoraModel>>(miPlanMejora);
        }
        public async Task<PlanMejoraModel> InsertOrUpdate(PlanMejoraModel PlanMejoraModel)
        {
            if (string.IsNullOrEmpty(PlanMejoraModel.AlternativaId.ToString())) throw new ArgumentNullException("AlternativaId");
            if (string.IsNullOrEmpty(PlanMejoraModel.Mejora.ToString())) throw new ArgumentNullException("Mejora");
            if (string.IsNullOrEmpty(PlanMejoraModel.PreguntaId.ToString())) throw new ArgumentNullException("PreguntaId");
            if (string.IsNullOrEmpty(PlanMejoraModel.SegmentacionAreaId.ToString())) throw new ArgumentNullException("SegmentacionAreaId");
            if (string.IsNullOrEmpty(PlanMejoraModel.SegmentacionSubAreaId.ToString())) throw new ArgumentNullException("SegmentacionSubAreaId");
            //if (string.IsNullOrEmpty(PlanMejoraModel.TipoDiferenciaRelacionadaId.ToString())) throw new ArgumentNullException("TipoDiferenciaRelacionadaId");
            if (string.IsNullOrEmpty(PlanMejoraModel.TipoImportanciaId.ToString())) throw new ArgumentNullException("TipoImportanciaId");
            if (string.IsNullOrEmpty(PlanMejoraModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _PlanMejoraRepository.InsertOrUpdate(_mapper.Map<PlanMejora>(PlanMejoraModel));
            return _mapper.Map<PlanMejoraModel>(retorno);
        }

        public async Task PlanMejoraInsertOrUpdate(PlanMejoraModel planMejoraModel)
        {
            await _PlanMejoraRepository.PlanMejoraInsertOrUpdate(_mapper.Map<PlanMejora>(planMejoraModel));
        }
            public void Dispose() 
        { 
            if (_PlanMejoraRepository != null)
            {
                _PlanMejoraRepository.Dispose();
                _PlanMejoraRepository = null;
            }
        }

    }

}
