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
    public interface ITipoNivelVentaService
    {
        Task<TipoNivelVentaModel> GetTipoNivelVentaById(TipoNivelVentaModel TipoNivelVentaModel);
        Task<List<TipoNivelVentaModel>> GetTipoNivelVentas();

        Task<TipoNivelVentaModel> InsertOrUpdate(TipoNivelVentaModel TipoNivelVentaModel);
        void Dispose();
    }
    public class TipoNivelVentaService : ITipoNivelVentaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private ITipoNivelVentaRepository _TipoNivelVentaRepository;
        private ISecurityHelper _securityHelper;
        public TipoNivelVentaService(IMapper mapper, IMemoryCache memoryCache, TipoNivelVentaRepository TipoNivelVentaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _TipoNivelVentaRepository = TipoNivelVentaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<TipoNivelVentaModel> GetTipoNivelVentaById(TipoNivelVentaModel TipoNivelVentaModel)
        {
            if (string.IsNullOrEmpty(TipoNivelVentaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miTipoNivelVenta = await _TipoNivelVentaRepository.GetTipoNivelVentaById(_mapper.Map<TipoNivelVenta>( TipoNivelVentaModel));
            return _mapper.Map<TipoNivelVentaModel>(miTipoNivelVenta);
        }
        public async Task<List<TipoNivelVentaModel>> GetTipoNivelVentas()
        {
            var TipoNivelVentasList = await _TipoNivelVentaRepository.GetTipoNivelVentas();
            return _mapper.Map<List<TipoNivelVentaModel>>(TipoNivelVentasList);
        }
        public async Task<TipoNivelVentaModel> InsertOrUpdate(TipoNivelVentaModel TipoNivelVentaModel)
        {
            if (string.IsNullOrEmpty(TipoNivelVentaModel.Detalle.ToString())) throw new ArgumentNullException("Detalle");
            if (string.IsNullOrEmpty(TipoNivelVentaModel.Nombre.ToString())) throw new ArgumentNullException("Nombre");
            if (string.IsNullOrEmpty(TipoNivelVentaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _TipoNivelVentaRepository.InsertOrUpdate(_mapper.Map<TipoNivelVenta>(TipoNivelVentaModel));
            return _mapper.Map<TipoNivelVentaModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_TipoNivelVentaRepository != null)
            {
                _TipoNivelVentaRepository.Dispose();
                _TipoNivelVentaRepository = null;
            }
        }

    }

}
