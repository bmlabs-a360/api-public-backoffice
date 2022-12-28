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
    public interface ISeguimientoService
    {
        Task<SeguimientoModel> GetSeguimientoById(SeguimientoModel SeguimientoModel);
        Task<List<SeguimientoModel>> GetSeguimientos();
        Task<List<SeguimientoModel>> GetSeguimientosByEmpresaId(EmpresaModel empresaModel);
        Task<List<SeguimientoModel>> GetSeguimientosByEvaluacionId(EvaluacionModel evaluacionModel);
        Task<List<SeguimientoModel>> GetSeguimientosByPlanMejoraId(PlanMejoraModel planMejoraModel);
        Task<SeguimientoModel> InsertOrUpdate(SeguimientoModel SeguimientoModel);
        void Dispose();
    }
    public class SeguimientoService : ISeguimientoService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private ISeguimientoRepository _SeguimientoRepository;
        private ISecurityHelper _securityHelper;
        public SeguimientoService(IMapper mapper, IMemoryCache memoryCache, SeguimientoRepository SeguimientoRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _SeguimientoRepository = SeguimientoRepository;
            _securityHelper = securityHelper;
        }
        public async Task<SeguimientoModel> GetSeguimientoById(SeguimientoModel SeguimientoModel)
        {
            if (string.IsNullOrEmpty(SeguimientoModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miSeguimiento = await _SeguimientoRepository.GetSeguimientoById(_mapper.Map<Seguimiento>( SeguimientoModel));
            return _mapper.Map<SeguimientoModel>(miSeguimiento);
        }
        public async Task<List<SeguimientoModel>> GetSeguimientos()
        {
            var SeguimientosList = await _SeguimientoRepository.GetSeguimientos();
            return _mapper.Map<List<SeguimientoModel>>(SeguimientosList);
        }
        public async Task<List<SeguimientoModel>> GetSeguimientosByEmpresaId(EmpresaModel empresaModel)
        {
            if (string.IsNullOrEmpty(empresaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miSeguimiento = await _SeguimientoRepository.GetSeguimientosByEmpresaId(_mapper.Map<Empresa>(empresaModel));
            return _mapper.Map<List<SeguimientoModel>>(miSeguimiento);
        }
        public async Task<List<SeguimientoModel>> GetSeguimientosByEvaluacionId(EvaluacionModel evaluacionModel)
        {
            if (string.IsNullOrEmpty(evaluacionModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miSeguimiento = await _SeguimientoRepository.GetSeguimientosByEvaluacionId(_mapper.Map<Evaluacion>(evaluacionModel));
            return _mapper.Map<List<SeguimientoModel>>(miSeguimiento);
        }
        public async Task<List<SeguimientoModel>> GetSeguimientosByPlanMejoraId(PlanMejoraModel planMejoraModel)
        {
            if (string.IsNullOrEmpty(planMejoraModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miSeguimiento = await _SeguimientoRepository.GetSeguimientosByPlanMejoraId(_mapper.Map<PlanMejora>(planMejoraModel));
            return _mapper.Map<List<SeguimientoModel>>(miSeguimiento);
        }
        public async Task<SeguimientoModel> InsertOrUpdate(SeguimientoModel SeguimientoModel)
        {
            if (string.IsNullOrEmpty(SeguimientoModel.EmpresaId.ToString())) throw new ArgumentNullException("SegmentacionAreaId");
            if (string.IsNullOrEmpty(SeguimientoModel.EvaluacionId.ToString())) throw new ArgumentNullException("NombreSubArea");
            if (string.IsNullOrEmpty(SeguimientoModel.FechaUltimoAcceso.ToString())) throw new ArgumentNullException("NombreSubArea");
            if (string.IsNullOrEmpty(SeguimientoModel.Madurez.ToString())) throw new ArgumentNullException("NombreSubArea");
            if (string.IsNullOrEmpty(SeguimientoModel.PlanMejoraId.ToString())) throw new ArgumentNullException("NombreSubArea");
            if (string.IsNullOrEmpty(SeguimientoModel.PorcentajePlaMejora.ToString())) throw new ArgumentNullException("NombreSubArea");
            if (string.IsNullOrEmpty(SeguimientoModel.PorcentajeRespuestas.ToString())) throw new ArgumentNullException("NombreSubArea");
            if (string.IsNullOrEmpty(SeguimientoModel.Activo.ToString())) throw new ArgumentNullException("Activo");

            var retorno = await _SeguimientoRepository.InsertOrUpdate(_mapper.Map<Seguimiento>(SeguimientoModel));
            return _mapper.Map<SeguimientoModel>(retorno);
        }
        public void Dispose() 
        { 
            if (_SeguimientoRepository != null)
            {
                _SeguimientoRepository.Dispose();
                _SeguimientoRepository = null;
            }
        }

    }

}
