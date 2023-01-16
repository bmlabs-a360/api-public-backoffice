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
    public interface IReporteItemNivelBasicoService
    {
        Task<ReporteItemNivelBasicoModel> GetReporteItemNivelBasicoById(ReporteItemNivelBasicoModel ReporteItemNivelBasicoModel);
        Task<List<ReporteItemNivelBasicoModel>> GetReporteItemNivelBasicos();
        Task<List<ReporteItemNivelBasicoModel>> GetReporteItemNivelBasicosByReporteId(ReporteModel reporteModel);
        Task<ReporteItemNivelBasicoModel> InsertOrUpdate(ReporteItemNivelBasicoModel ReporteItemNivelBasicoModel);
        void Dispose();
    }
    public class ReporteItemNivelBasicoService : IReporteItemNivelBasicoService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IReporteItemNivelBasicoRepository _ReporteItemNivelBasicoRepository;
        private ISecurityHelper _securityHelper;
        public ReporteItemNivelBasicoService(IMapper mapper, IMemoryCache memoryCache, ReporteItemNivelBasicoRepository ReporteItemNivelBasicoRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _ReporteItemNivelBasicoRepository = ReporteItemNivelBasicoRepository;
            _securityHelper = securityHelper;
        }
        public async Task<ReporteItemNivelBasicoModel> GetReporteItemNivelBasicoById(ReporteItemNivelBasicoModel ReporteItemNivelBasicoModel)
        {
            if (string.IsNullOrEmpty(ReporteItemNivelBasicoModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miReporteItemNivelBasico = await _ReporteItemNivelBasicoRepository.GetReporteItemNivelBasicoById(_mapper.Map<ReporteItemNivelBasico>(ReporteItemNivelBasicoModel));
            return _mapper.Map<ReporteItemNivelBasicoModel>(miReporteItemNivelBasico);
        }
        public async Task<List<ReporteItemNivelBasicoModel>> GetReporteItemNivelBasicos()
        {
            var ReporteItemNivelBasicosList = await _ReporteItemNivelBasicoRepository.GetReporteItemNivelBasicos();
            return _mapper.Map<List<ReporteItemNivelBasicoModel>>(ReporteItemNivelBasicosList);
        }
        public async Task<List<ReporteItemNivelBasicoModel>> GetReporteItemNivelBasicosByReporteId(ReporteModel reporteModel)
        {
            if (string.IsNullOrEmpty(reporteModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miReporteItemNivelBasico = await _ReporteItemNivelBasicoRepository.GetReporteItemNivelBasicosByReporteId(_mapper.Map<Reporte>(reporteModel));
            return _mapper.Map<List<ReporteItemNivelBasicoModel>>(miReporteItemNivelBasico);
        }
        public async Task<ReporteItemNivelBasicoModel> InsertOrUpdate(ReporteItemNivelBasicoModel ReporteItemNivelBasicoModel)
        {
            if (string.IsNullOrEmpty(ReporteItemNivelBasicoModel.ReporteId.ToString())) throw new ArgumentNullException("ReporteId");
            if (string.IsNullOrEmpty(ReporteItemNivelBasicoModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _ReporteItemNivelBasicoRepository.InsertOrUpdate(_mapper.Map<ReporteItemNivelBasico>(ReporteItemNivelBasicoModel));
            return _mapper.Map<ReporteItemNivelBasicoModel>(retorno);
        }
        public void Dispose()
        {
            if (_ReporteItemNivelBasicoRepository != null)
            {
                _ReporteItemNivelBasicoRepository.Dispose();
                _ReporteItemNivelBasicoRepository = null;
            }
        }

    }

}
