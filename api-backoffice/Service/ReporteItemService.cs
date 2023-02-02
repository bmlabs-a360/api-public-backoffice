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
    public interface IReporteItemService
    {
        Task<ReporteItemModel> GetReporteItemById(ReporteItemModel ReporteItemModel);
        Task<List<ReporteItemModel>> GetReporteItems();
        Task<List<ReporteItemModel>> GetReporteItemsByReporteId(ReporteModel reporteModel);
        Task<ReporteItemModel> InsertOrUpdate(ReporteItemModel ReporteItemModel);
        void Dispose();
    }
    public class ReporteItemService : IReporteItemService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IReporteItemRepository _ReporteItemRepository;
        private ISecurityHelper _securityHelper;
        public ReporteItemService(IMapper mapper, IMemoryCache memoryCache, ReporteItemRepository ReporteItemRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _ReporteItemRepository = ReporteItemRepository;
            _securityHelper = securityHelper;
        }
        public async Task<ReporteItemModel> GetReporteItemById(ReporteItemModel ReporteItemModel)
        {
            if (string.IsNullOrEmpty(ReporteItemModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miReporteItem = await _ReporteItemRepository.GetReporteItemById(_mapper.Map<ReporteItem>( ReporteItemModel));
            return _mapper.Map<ReporteItemModel>(miReporteItem);
        }
        public async Task<List<ReporteItemModel>> GetReporteItems()
        {
            var ReporteItemsList = await _ReporteItemRepository.GetReporteItems();
            return _mapper.Map<List<ReporteItemModel>>(ReporteItemsList);
        }
        public async Task<List<ReporteItemModel>> GetReporteItemsByReporteId(ReporteModel reporteModel)
        {
            if (string.IsNullOrEmpty(reporteModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miReporteItem = await _ReporteItemRepository.GetReporteItemsByReporteId(_mapper.Map<Reporte>(reporteModel));
            return _mapper.Map<List<ReporteItemModel>>(miReporteItem);
        }
        public async Task<ReporteItemModel> InsertOrUpdate(ReporteItemModel ReporteItemModel)
        {
            if (string.IsNullOrEmpty(ReporteItemModel.ReporteId.ToString())) throw new ArgumentNullException("ReporteId");
            if (string.IsNullOrEmpty(ReporteItemModel.TipoItemReporteId.ToString())) throw new ArgumentNullException("TipoItemReporteId");
            if (string.IsNullOrEmpty(ReporteItemModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            if (!ReporteItemModel.Id.Equals(Guid.Empty)) { 
                var miReporteItem = await _ReporteItemRepository.GetReporteItemById(_mapper.Map<ReporteItem>(ReporteItemModel));
                ReporteItemModel.FechaCreacion = miReporteItem.FechaCreacion;
            }

            var retorno = await _ReporteItemRepository.InsertOrUpdate(_mapper.Map<ReporteItem>(ReporteItemModel));
            return _mapper.Map<ReporteItemModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_ReporteItemRepository != null)
            {
                _ReporteItemRepository.Dispose();
                _ReporteItemRepository = null;
            }
        }

    }

}
