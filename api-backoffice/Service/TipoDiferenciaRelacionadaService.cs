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
    public interface ITipoDiferenciaRelacionadaService
    {
        Task<TipoDiferenciaRelacionadaModel> GetTipoDiferenciaRelacionadaById(TipoDiferenciaRelacionadaModel TipoDiferenciaRelacionadaModel);
        Task<List<TipoDiferenciaRelacionadaModel>> GetTipoDiferenciaRelacionadas();

        Task<TipoDiferenciaRelacionadaModel> InsertOrUpdate(TipoDiferenciaRelacionadaModel TipoDiferenciaRelacionadaModel);
        void Dispose();
    }
    public class TipoDiferenciaRelacionadaService : ITipoDiferenciaRelacionadaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private ITipoDiferenciaRelacionadaRepository _TipoDiferenciaRelacionadaRepository;
        private ISecurityHelper _securityHelper;
        public TipoDiferenciaRelacionadaService(IMapper mapper, IMemoryCache memoryCache, TipoDiferenciaRelacionadaRepository TipoDiferenciaRelacionadaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _TipoDiferenciaRelacionadaRepository = TipoDiferenciaRelacionadaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<TipoDiferenciaRelacionadaModel> GetTipoDiferenciaRelacionadaById(TipoDiferenciaRelacionadaModel TipoDiferenciaRelacionadaModel)
        {
            if (string.IsNullOrEmpty(TipoDiferenciaRelacionadaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miTipoDiferenciaRelacionada = await _TipoDiferenciaRelacionadaRepository.GetTipoDiferenciaRelacionadaById(_mapper.Map<TipoDiferenciaRelacionada>( TipoDiferenciaRelacionadaModel));
            return _mapper.Map<TipoDiferenciaRelacionadaModel>(miTipoDiferenciaRelacionada);
        }
        public async Task<List<TipoDiferenciaRelacionadaModel>> GetTipoDiferenciaRelacionadas()
        {
            var TipoDiferenciaRelacionadasList = await _TipoDiferenciaRelacionadaRepository.GetTipoDiferenciaRelacionadas();
            return _mapper.Map<List<TipoDiferenciaRelacionadaModel>>(TipoDiferenciaRelacionadasList);
        }
        public async Task<TipoDiferenciaRelacionadaModel> InsertOrUpdate(TipoDiferenciaRelacionadaModel TipoDiferenciaRelacionadaModel)
        {
            if (string.IsNullOrEmpty(TipoDiferenciaRelacionadaModel.Detalle.ToString())) throw new ArgumentNullException("Detalle");
            if (string.IsNullOrEmpty(TipoDiferenciaRelacionadaModel.Nombre.ToString())) throw new ArgumentNullException("Nombre");
            if (string.IsNullOrEmpty(TipoDiferenciaRelacionadaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _TipoDiferenciaRelacionadaRepository.InsertOrUpdate(_mapper.Map<TipoDiferenciaRelacionada>(TipoDiferenciaRelacionadaModel));
            return _mapper.Map<TipoDiferenciaRelacionadaModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_TipoDiferenciaRelacionadaRepository != null)
            {
                _TipoDiferenciaRelacionadaRepository.Dispose();
                _TipoDiferenciaRelacionadaRepository = null;
            }
        }

    }

}
