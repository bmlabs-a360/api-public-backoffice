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
    public interface IIndicadoresService
    {

        Task<int> CantidadEmpresas();
        int CantidadEmpresasSql();

        int CantidadEmpresasSuscripcion();
        int CantidadGranEmpresa();
        int CantidaEmpresasEvaluacionProceso();
        int CantidadEmpresasEvaluacionFinalizada();

        List<PromedioIMTamanoEmpresaDto> PromedioIMTamanoEmpresa();
        List<PromedioIMRubroDto> PromedioIMRubro();

        void Dispose();
    }
    public class IndicadoresService : IIndicadoresService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IIndicadoresRepository _IndicadoresRepository;
        private ISecurityHelper _securityHelper;

        public IndicadoresService(IMapper mapper, IMemoryCache memoryCache, IndicadoresRepository IndicadoresRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _IndicadoresRepository = IndicadoresRepository;
            _securityHelper = securityHelper;
        }
        public async Task<int> CantidadEmpresas()
        {
            return await _IndicadoresRepository.CantidadEmpresas();
        }
        public  int CantidadEmpresasSql() { 
            return  _IndicadoresRepository.CantidadEmpresasSql();
        }

        public  int CantidadEmpresasSuscripcion()
        {
            return  _IndicadoresRepository.CantidadEmpresasSuscripcion();
        }
        public  int CantidadGranEmpresa()
        {
            return  _IndicadoresRepository.CantidadGranEmpresa();
        }
        public  int CantidaEmpresasEvaluacionProceso()
        {
            return  _IndicadoresRepository.CantidaEmpresasEvaluacionProceso();
        }
        public  int CantidadEmpresasEvaluacionFinalizada()
        {
            return  _IndicadoresRepository.CantidadEmpresasEvaluacionFinalizada();
        }
        public List<PromedioIMTamanoEmpresaDto> PromedioIMTamanoEmpresa()
        {
            return _IndicadoresRepository.PromedioIMTamanoEmpresa();
        }
        public List<PromedioIMRubroDto> PromedioIMRubro()
        {
            return _IndicadoresRepository.PromedioIMRubro();
        }
        public void Dispose() 
        { 
            if (_IndicadoresRepository != null)
            {
                _IndicadoresRepository.Dispose();
                _IndicadoresRepository = null;
            }
        }
    }

}
