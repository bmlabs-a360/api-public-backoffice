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
    public interface IPreguntaService
    {
        Task<PreguntaModel> GetPreguntaById(PreguntaModel PreguntaModel);
        Task<List<PreguntaModel>> GetPreguntas();
        Task<List<PreguntaModel>> GetPreguntasByEvaluacionId(EvaluacionModel evaluacionModel);
        Task<List<PreguntaModel>> GetPreguntasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel);
        Task<List<PreguntaModel>> GetPreguntasBySegmentacionSubAreaId(SegmentacionSubAreaModel segmentacionSubAreaModel);
        Task<PreguntaModel> InsertOrUpdate(PreguntaModel PreguntaModel);
        Task<int> DeletePregunta(PreguntaModel preguntaModel);
        Task<int> GetMaxOrdenPregunta(EvaluacionModel evaluacion);
        void Dispose();
    }
    public class PreguntaService : IPreguntaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IPreguntaRepository _PreguntaRepository;
        private ISecurityHelper _securityHelper;
        public PreguntaService(IMapper mapper, IMemoryCache memoryCache, PreguntaRepository PreguntaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _PreguntaRepository = PreguntaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<PreguntaModel> GetPreguntaById(PreguntaModel PreguntaModel)
        {
            if (string.IsNullOrEmpty(PreguntaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miPregunta = await _PreguntaRepository.GetPreguntaById(_mapper.Map<Pregunta>( PreguntaModel));
            return _mapper.Map<PreguntaModel>(miPregunta);
        }
        public async Task<List<PreguntaModel>> GetPreguntas()
        {
            var PreguntasList = await _PreguntaRepository.GetPreguntas();
            return _mapper.Map<List<PreguntaModel>>(PreguntasList);
        }
        public async Task<List<PreguntaModel>> GetPreguntasByEvaluacionId(EvaluacionModel evaluacionModel)
        {
            if (string.IsNullOrEmpty(evaluacionModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miPregunta = await _PreguntaRepository.GetPreguntasByEvaluacionId(_mapper.Map<Evaluacion>(evaluacionModel));
            return _mapper.Map< List<PreguntaModel>>(miPregunta);
        }
        public async Task<List<PreguntaModel>> GetPreguntasBySegmentacionAreaId(SegmentacionAreaModel segmentacionAreaModel)
        {
            if (string.IsNullOrEmpty(segmentacionAreaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miPregunta = await _PreguntaRepository.GetPreguntasBySegmentacionAreaId(_mapper.Map<SegmentacionArea>(segmentacionAreaModel));
            return _mapper.Map<List<PreguntaModel>>(miPregunta);
        }
        public async Task<List<PreguntaModel>> GetPreguntasBySegmentacionSubAreaId(SegmentacionSubAreaModel segmentacionSubAreaModel)
        {
            if (string.IsNullOrEmpty(segmentacionSubAreaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miPregunta = await _PreguntaRepository.GetPreguntasBySegmentacionSubAreaId(_mapper.Map<SegmentacionSubArea>(segmentacionSubAreaModel));
            return _mapper.Map<List<PreguntaModel>>(miPregunta);
        }
        public async Task<PreguntaModel> InsertOrUpdate(PreguntaModel PreguntaModel)
        {
            if (string.IsNullOrEmpty(PreguntaModel.Capacidad.ToString())) throw new ArgumentNullException("AlternativaId");
            if (string.IsNullOrEmpty(PreguntaModel.Detalle.ToString())) throw new ArgumentNullException("Mejora");
            if (string.IsNullOrEmpty(PreguntaModel.EvaluacionId.ToString())) throw new ArgumentNullException("PreguntaId");
            if (string.IsNullOrEmpty(PreguntaModel.SegmentacionAreaId.ToString())) throw new ArgumentNullException("SegmentacionAreaId");
            if (string.IsNullOrEmpty(PreguntaModel.SegmentacionSubAreaId.ToString())) throw new ArgumentNullException("SegmentacionSubAreaId");
            if (string.IsNullOrEmpty(PreguntaModel.Orden.ToString())) throw new ArgumentNullException("TipoDiferenciaRelacionadaId");
            if (string.IsNullOrEmpty(PreguntaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _PreguntaRepository.InsertOrUpdate(_mapper.Map<Pregunta>(PreguntaModel));
            return _mapper.Map<PreguntaModel>(retorno);
        }

        public async Task<int> DeletePregunta(PreguntaModel preguntaModel)
        {
            return await _PreguntaRepository.DeletePregunta(_mapper.Map<Pregunta>(preguntaModel));
        }
        public async Task<int> GetMaxOrdenPregunta(EvaluacionModel evaluacion)
        {
            return await _PreguntaRepository.GetMaxOrdenPregunta(_mapper.Map<Evaluacion>(evaluacion));
        }
        public void Dispose() 
        { 
            if (_PreguntaRepository != null)
            {
                _PreguntaRepository.Dispose();
                _PreguntaRepository = null;
            }
        }

    }

}
