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
    public interface IAlternativaService
    {
        Task<AlternativaModel> GetAlternativaById(AlternativaModel AlternativaModel);
        Task<AlternativaModel> InsertOrUpdate(AlternativaModel alternativaModel);
        Task<List<AlternativaModel>> GetAlternativaByPreguntaId(PreguntaModel preguntaModel);

        Task<List<AlternativaModel>> GetAlternativaByEvaluacionId(AlternativaModel alternativaMode);
        Task<int> DeleteAlternativa(AlternativaModel alternativaModel);

       void Dispose();
    }
    public class AlternativaService : IAlternativaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IAlternativaRepository _alternativaRepository;
        private ISecurityHelper _securityHelper;

        public AlternativaService(IMapper mapper, IMemoryCache memoryCache, AlternativaRepository alternativaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _alternativaRepository = alternativaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<AlternativaModel> GetAlternativaById(AlternativaModel alternativaModel)
        {
            if (string.IsNullOrEmpty(alternativaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miAlternativa = await _alternativaRepository.GetAlternativaById(_mapper.Map<Alternativa>( alternativaModel));
            return _mapper.Map<AlternativaModel>(miAlternativa);
        }
        public async Task<List<AlternativaModel>> GetAlternativaByPreguntaId(PreguntaModel preguntaModel)
        {
            if (string.IsNullOrEmpty(preguntaModel.Id.ToString())) throw new ArgumentNullException("PreguntaId");
            var miAlternativa = await _alternativaRepository.GetAlternativaByPreguntaId(_mapper.Map<Pregunta>(preguntaModel));
            return _mapper.Map<List<AlternativaModel>>(miAlternativa);
        }
        public async Task<List<AlternativaModel>> GetAlternativaByEvaluacionId(AlternativaModel alternativaMode)
        {
            if (alternativaMode == null) throw new ArgumentNullException("alternativaMode");

            var alternativasList = await _alternativaRepository.GetAlternativaByEvaluacionId(_mapper.Map<Alternativa>(alternativaMode));
            return _mapper.Map<List<AlternativaModel>>(alternativasList);
        }
        public async Task<AlternativaModel> InsertOrUpdate(AlternativaModel alternativaModel)
        {
            if (string.IsNullOrEmpty(alternativaModel.PreguntaId.ToString())) throw new ArgumentNullException("PreguntaId");
            if (string.IsNullOrEmpty(alternativaModel.EvaluacionId.ToString())) throw new ArgumentNullException("EvaluacionId");
            if (string.IsNullOrEmpty(alternativaModel.Retroalimentacion)) throw new ArgumentNullException("Retroalimentacion");
            if (string.IsNullOrEmpty(alternativaModel.Valor.ToString())) throw new ArgumentNullException("Valor");
            if (string.IsNullOrEmpty(alternativaModel.Orden.ToString())) throw new ArgumentNullException("Orden");

            if (string.IsNullOrEmpty(alternativaModel.Detalle.ToString())) throw new ArgumentNullException("Detalle");

            var retorno = await _alternativaRepository.InsertOrUpdate(_mapper.Map<Alternativa>(alternativaModel));
            return _mapper.Map<AlternativaModel>(retorno);
        }
        public async Task<int> DeleteAlternativa(AlternativaModel alternativaModel)
        {


            return await _alternativaRepository.DeleteAlternativa(_mapper.Map<Alternativa>(alternativaModel));

        }

        public void Dispose() 
        { 
            if (_alternativaRepository != null)
            {
                _alternativaRepository.Dispose();
                _alternativaRepository = null;
            }
        }

    }

}
