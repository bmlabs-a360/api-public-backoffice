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
    public interface ITipoItemReporteService
    {
        Task<TipoItemReporteModel> GetTipoItemReporteById(TipoItemReporteModel TipoItemReporteModel);
        Task<List<TipoItemReporteModel>> GetTipoItemReportes();

        Task<TipoItemReporteModel> InsertOrUpdate(TipoItemReporteModel TipoItemReporteModel);
        void Dispose();
    }
    public class TipoItemReporteService : ITipoItemReporteService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private ITipoItemReporteRepository _TipoItemReporteRepository;
        private ISecurityHelper _securityHelper;
        public TipoItemReporteService(IMapper mapper, IMemoryCache memoryCache, TipoItemReporteRepository TipoItemReporteRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _TipoItemReporteRepository = TipoItemReporteRepository;
            _securityHelper = securityHelper;
        }
        public async Task<TipoItemReporteModel> GetTipoItemReporteById(TipoItemReporteModel TipoItemReporteModel)
        {
            if (string.IsNullOrEmpty(TipoItemReporteModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miTipoItemReporte = await _TipoItemReporteRepository.GetTipoItemReporteById(_mapper.Map<TipoItemReporte>( TipoItemReporteModel));
            return _mapper.Map<TipoItemReporteModel>(miTipoItemReporte);
        }
        public async Task<List<TipoItemReporteModel>> GetTipoItemReportes()
        {
            var TipoItemReportesList = await _TipoItemReporteRepository.GetTipoItemReportes();
            return _mapper.Map<List<TipoItemReporteModel>>(TipoItemReportesList);
        }
        public async Task<TipoItemReporteModel> InsertOrUpdate(TipoItemReporteModel TipoItemReporteModel)
        {
            if (string.IsNullOrEmpty(TipoItemReporteModel.Detalle.ToString())) throw new ArgumentNullException("Detalle");
            if (string.IsNullOrEmpty(TipoItemReporteModel.Nombre.ToString())) throw new ArgumentNullException("Nombre");
            if (string.IsNullOrEmpty(TipoItemReporteModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _TipoItemReporteRepository.InsertOrUpdate(_mapper.Map<TipoItemReporte>(TipoItemReporteModel));
            return _mapper.Map<TipoItemReporteModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_TipoItemReporteRepository != null)
            {
                _TipoItemReporteRepository.Dispose();
                _TipoItemReporteRepository = null;
            }
        }

    }

}
