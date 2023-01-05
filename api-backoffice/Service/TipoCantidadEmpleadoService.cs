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
    public interface ITipoCantidadEmpleadoService
    {
        Task<TipoCantidadEmpleadoModel> GetTipoCantidadEmpleadoById(TipoCantidadEmpleadoModel TipoCantidadEmpleadoModel);
        Task<List<TipoCantidadEmpleadoModel>> GetTipoCantidadEmpleados();
        Task<TipoCantidadEmpleadoModel> InsertOrUpdate(TipoCantidadEmpleadoModel TipoCantidadEmpleadoModel);
        void Dispose();
    }
    public class TipoCantidadEmpleadoService : ITipoCantidadEmpleadoService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private ITipoCantidadEmpleadoRepository _TipoCantidadEmpleadoRepository;
        private ISecurityHelper _securityHelper;
        public TipoCantidadEmpleadoService(IMapper mapper, IMemoryCache memoryCache, TipoCantidadEmpleadoRepository TipoCantidadEmpleadoRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _TipoCantidadEmpleadoRepository = TipoCantidadEmpleadoRepository;
            _securityHelper = securityHelper;
        }
        public async Task<TipoCantidadEmpleadoModel> GetTipoCantidadEmpleadoById(TipoCantidadEmpleadoModel TipoCantidadEmpleadoModel)
        {
            if (string.IsNullOrEmpty(TipoCantidadEmpleadoModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miTipoCantidadEmpleado = await _TipoCantidadEmpleadoRepository.GetTipoCantidadEmpleadoById(_mapper.Map<TipoCantidadEmpleado>( TipoCantidadEmpleadoModel));
            return _mapper.Map<TipoCantidadEmpleadoModel>(miTipoCantidadEmpleado);
        }
        public async Task<List<TipoCantidadEmpleadoModel>> GetTipoCantidadEmpleados()
        {
            var TipoCantidadEmpleadosList = await _TipoCantidadEmpleadoRepository.GetTipoCantidadEmpleados();
            return _mapper.Map<List<TipoCantidadEmpleadoModel>>(TipoCantidadEmpleadosList);
        }
        public async Task<TipoCantidadEmpleadoModel> InsertOrUpdate(TipoCantidadEmpleadoModel TipoCantidadEmpleadoModel)
        {
            if (string.IsNullOrEmpty(TipoCantidadEmpleadoModel.Detalle.ToString())) throw new ArgumentNullException("Detalle");
            if (string.IsNullOrEmpty(TipoCantidadEmpleadoModel.Nombre.ToString())) throw new ArgumentNullException("Nombre");
            if (string.IsNullOrEmpty(TipoCantidadEmpleadoModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _TipoCantidadEmpleadoRepository.InsertOrUpdate(_mapper.Map<TipoCantidadEmpleado>(TipoCantidadEmpleadoModel));
            return _mapper.Map<TipoCantidadEmpleadoModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_TipoCantidadEmpleadoRepository != null)
            {
                _TipoCantidadEmpleadoRepository.Dispose();
                _TipoCantidadEmpleadoRepository = null;
            }
        }

    }

}
