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
    public interface ITipoTamanoEmpresaService
    {
        Task<TipoTamanoEmpresaModel> GetTipoTamanoEmpresaById(TipoTamanoEmpresaModel TipoTamanoEmpresaModel);
        Task<List<TipoTamanoEmpresaModel>> GetTipoTamanoEmpresas();

        Task<TipoTamanoEmpresaModel> InsertOrUpdate(TipoTamanoEmpresaModel TipoTamanoEmpresaModel);
        void Dispose();
    }
    public class TipoTamanoEmpresaService : ITipoTamanoEmpresaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private ITipoTamanoEmpresaRepository _TipoTamanoEmpresaRepository;
        private ISecurityHelper _securityHelper;
        public TipoTamanoEmpresaService(IMapper mapper, IMemoryCache memoryCache, TipoTamanoEmpresaRepository TipoTamanoEmpresaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _TipoTamanoEmpresaRepository = TipoTamanoEmpresaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<TipoTamanoEmpresaModel> GetTipoTamanoEmpresaById(TipoTamanoEmpresaModel TipoTamanoEmpresaModel)
        {
            if (string.IsNullOrEmpty(TipoTamanoEmpresaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miTipoTamanoEmpresa = await _TipoTamanoEmpresaRepository.GetTipoTamanoEmpresaById(_mapper.Map<TipoTamanoEmpresa>( TipoTamanoEmpresaModel));
            return _mapper.Map<TipoTamanoEmpresaModel>(miTipoTamanoEmpresa);
        }
        public async Task<List<TipoTamanoEmpresaModel>> GetTipoTamanoEmpresas()
        {
            var TipoTamanoEmpresasList = await _TipoTamanoEmpresaRepository.GetTipoTamanoEmpresas();
            return _mapper.Map<List<TipoTamanoEmpresaModel>>(TipoTamanoEmpresasList);
        }
        public async Task<TipoTamanoEmpresaModel> InsertOrUpdate(TipoTamanoEmpresaModel TipoTamanoEmpresaModel)
        {
            if (string.IsNullOrEmpty(TipoTamanoEmpresaModel.Detalle.ToString())) throw new ArgumentNullException("Detalle");
            if (string.IsNullOrEmpty(TipoTamanoEmpresaModel.Nombre.ToString())) throw new ArgumentNullException("Nombre");
            if (string.IsNullOrEmpty(TipoTamanoEmpresaModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _TipoTamanoEmpresaRepository.InsertOrUpdate(_mapper.Map<TipoTamanoEmpresa>(TipoTamanoEmpresaModel));
            return _mapper.Map<TipoTamanoEmpresaModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_TipoTamanoEmpresaRepository != null)
            {
                _TipoTamanoEmpresaRepository.Dispose();
                _TipoTamanoEmpresaRepository = null;
            }
        }

    }

}
