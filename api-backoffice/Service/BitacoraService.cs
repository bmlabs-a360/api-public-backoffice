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
    public interface IBitacoraService
    {
        Task<BitacoraModel> InsertOrUpdate(BitacoraModel bitacoraModel);
        Task<List<BitacoraModel>> GetBitacorasByUsuarioId(BitacoraModel UsuarioId);
        void Dispose();
    }
    public class BitacoraService : IBitacoraService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IBitacoraRepository _bitacoraRepository;
        private ISecurityHelper _securityHelper;
        public BitacoraService(IMapper mapper, IMemoryCache memoryCache, BitacoraRepository BitacoraRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _bitacoraRepository = BitacoraRepository;
            _securityHelper = securityHelper;
        }
        public async Task<BitacoraModel> InsertOrUpdate(BitacoraModel bitacoraModel)
        {
            if (string.IsNullOrEmpty(bitacoraModel.Descripcion)) throw new ArgumentNullException("Descripcion");
            var retorno = await _bitacoraRepository.InsertOrUpdate(_mapper.Map<Bitacora>(bitacoraModel));
            return _mapper.Map<BitacoraModel>(retorno);
        }
        public async Task<List<BitacoraModel>> GetBitacorasByUsuarioId(BitacoraModel bitacoraModel)
        {
            if (bitacoraModel.UsuarioId.ToString() == null) throw new ArgumentNullException("UsuarioId");

            var bitacorasList = await _bitacoraRepository.GetBitacorasByUsuarioId(bitacoraModel);
            return _mapper.Map<List<BitacoraModel>>(bitacorasList);
        }
        public async Task<BitacoraModel> GetBitacoraById(BitacoraModel bitacoraModel)
        {
            if (string.IsNullOrEmpty(bitacoraModel.Id.ToString())) throw new ArgumentNullException("BitacoraId");
            var miBitacora = await _bitacoraRepository.GetBitacoraById(_mapper.Map<Bitacora>(bitacoraModel));
            return _mapper.Map<BitacoraModel>(miBitacora);
        }
        public void Dispose() 
        { 
            if (_bitacoraRepository != null)
            {
                _bitacoraRepository.Dispose();
                _bitacoraRepository = null;
            }
        }
    }

}
