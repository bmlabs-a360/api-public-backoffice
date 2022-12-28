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
    public interface ITipoSubRubroService
    {
        Task<List<TipoSubRubroModel>> GetTipoSubRubroByIdRubro(Guid TipoRubroId);


        Task<TipoSubRubroModel> GetTipoSubRubroById(TipoSubRubroModel TipoSubRubroModel);
        Task<List<TipoSubRubroModel>> GetTipoSubRubros();

        Task<TipoSubRubroModel> InsertOrUpdate(TipoSubRubroModel TipoSubRubroModel);
        void Dispose();
    }
    public class TipoSubRubroService : ITipoSubRubroService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private ITipoSubRubroRepository _TipoSubRubroRepository;
        private ISecurityHelper _securityHelper;
        public TipoSubRubroService(IMapper mapper, IMemoryCache memoryCache, TipoSubRubroRepository TipoSubRubroRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _TipoSubRubroRepository = TipoSubRubroRepository;
            _securityHelper = securityHelper;
        }
        public async Task<List<TipoSubRubroModel>> GetTipoSubRubroByIdRubro(Guid TipoRubroId)
        {
            if (string.IsNullOrEmpty(TipoRubroId.ToString())) throw new ArgumentNullException("TipoRubroId");
            var miTipoSubRubro = await _TipoSubRubroRepository.GetTipoSubRubroByIdRubro(TipoRubroId);
            return _mapper.Map<List<TipoSubRubroModel>>(miTipoSubRubro);
        }

        public async Task<TipoSubRubroModel> GetTipoSubRubroById(TipoSubRubroModel TipoSubRubroModel)
        {
            if (string.IsNullOrEmpty(TipoSubRubroModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miTipoSubRubro = await _TipoSubRubroRepository.GetTipoSubRubroById(_mapper.Map<TipoSubRubro>(TipoSubRubroModel));
            return _mapper.Map<TipoSubRubroModel>(miTipoSubRubro);
        }
        public async Task<List<TipoSubRubroModel>> GetTipoSubRubros()
        {
            var TipoSubRubrosList = await _TipoSubRubroRepository.GetTipoSubRubros();
            return _mapper.Map<List<TipoSubRubroModel>>(TipoSubRubrosList);
        }
        public async Task<TipoSubRubroModel> InsertOrUpdate(TipoSubRubroModel TipoSubRubroModel)
        {
            if (string.IsNullOrEmpty(TipoSubRubroModel.Detalle.ToString())) throw new ArgumentNullException("Detalle");
            if (string.IsNullOrEmpty(TipoSubRubroModel.Nombre.ToString())) throw new ArgumentNullException("Nombre");
            if (string.IsNullOrEmpty(TipoSubRubroModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _TipoSubRubroRepository.InsertOrUpdate(_mapper.Map<TipoSubRubro>(TipoSubRubroModel));
            return _mapper.Map<TipoSubRubroModel>(retorno);
        }
        public void Dispose()
        {
            if (_TipoSubRubroRepository != null)
            {
                _TipoSubRubroRepository.Dispose();
                _TipoSubRubroRepository = null;
            }
        }

    }

}
