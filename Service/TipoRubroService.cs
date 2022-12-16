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
    public interface ITipoRubroService
    {
        Task<TipoRubroModel> GetTipoRubroById(TipoRubroModel TipoRubroModel);
        Task<List<TipoRubroModel>> GetTipoRubros();

        Task<TipoRubroModel> InsertOrUpdate(TipoRubroModel TipoRubroModel);
        void Dispose();
    }
    public class TipoRubroService : ITipoRubroService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private ITipoRubroRepository _TipoRubroRepository;
        private ISecurityHelper _securityHelper;
        public TipoRubroService(IMapper mapper, IMemoryCache memoryCache, TipoRubroRepository TipoRubroRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _TipoRubroRepository = TipoRubroRepository;
            _securityHelper = securityHelper;
        }
        public async Task<TipoRubroModel> GetTipoRubroById(TipoRubroModel TipoRubroModel)
        {
            if (string.IsNullOrEmpty(TipoRubroModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miTipoRubro = await _TipoRubroRepository.GetTipoRubroById(_mapper.Map<TipoRubro>( TipoRubroModel));
            return _mapper.Map<TipoRubroModel>(miTipoRubro);
        }
        public async Task<List<TipoRubroModel>> GetTipoRubros()
        {
            var TipoRubrosList = await _TipoRubroRepository.GetTipoRubros();
            return _mapper.Map<List<TipoRubroModel>>(TipoRubrosList);
        }
        public async Task<TipoRubroModel> InsertOrUpdate(TipoRubroModel TipoRubroModel)
        {
            if (string.IsNullOrEmpty(TipoRubroModel.Detalle.ToString())) throw new ArgumentNullException("Detalle");
            if (string.IsNullOrEmpty(TipoRubroModel.Nombre.ToString())) throw new ArgumentNullException("Nombre");
            if (string.IsNullOrEmpty(TipoRubroModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _TipoRubroRepository.InsertOrUpdate(_mapper.Map<TipoRubro>(TipoRubroModel));
            return _mapper.Map<TipoRubroModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_TipoRubroRepository != null)
            {
                _TipoRubroRepository.Dispose();
                _TipoRubroRepository = null;
            }
        }

    }

}
