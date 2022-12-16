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
    public interface ITipoImportanciaService
    {
        Task<TipoImportanciaModel> GetTipoImportanciaById(TipoImportanciaModel TipoImportanciaModel);
        Task<List<TipoImportanciaModel>> GetTipoImportancias();

        Task<TipoImportanciaModel> InsertOrUpdate(TipoImportanciaModel TipoImportanciaModel);
        void Dispose();
    }
    public class TipoImportanciaService : ITipoImportanciaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private ITipoImportanciaRepository _TipoImportanciaRepository;
        private ISecurityHelper _securityHelper;
        public TipoImportanciaService(IMapper mapper, IMemoryCache memoryCache, TipoImportanciaRepository TipoImportanciaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _TipoImportanciaRepository = TipoImportanciaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<TipoImportanciaModel> GetTipoImportanciaById(TipoImportanciaModel TipoImportanciaModel)
        {
            if (string.IsNullOrEmpty(TipoImportanciaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miTipoImportancia = await _TipoImportanciaRepository.GetTipoImportanciaById(_mapper.Map<TipoImportancia>( TipoImportanciaModel));
            return _mapper.Map<TipoImportanciaModel>(miTipoImportancia);
        }
        public async Task<List<TipoImportanciaModel>> GetTipoImportancias()
        {
            var TipoImportanciasList = await _TipoImportanciaRepository.GetTipoImportancias();
            return _mapper.Map<List<TipoImportanciaModel>>(TipoImportanciasList);
        }
        public async Task<TipoImportanciaModel> InsertOrUpdate(TipoImportanciaModel TipoImportanciaModel)
        {
            if (string.IsNullOrEmpty(TipoImportanciaModel.Detalle.ToString())) throw new ArgumentNullException("Detalle");
            if (string.IsNullOrEmpty(TipoImportanciaModel.Nombre.ToString())) throw new ArgumentNullException("Nombre");
            if (string.IsNullOrEmpty(TipoImportanciaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _TipoImportanciaRepository.InsertOrUpdate(_mapper.Map<TipoImportancia>(TipoImportanciaModel));
            return _mapper.Map<TipoImportanciaModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_TipoImportanciaRepository != null)
            {
                _TipoImportanciaRepository.Dispose();
                _TipoImportanciaRepository = null;
            }
        }

    }

}
