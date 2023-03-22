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
    public interface IEmpresaService
    {
        Task<EmpresaModel> GetEmpresaById(EmpresaModel empresaModel);
        Task<EmpresaModel> InsertOrUpdate(EmpresaModel empresaModel);
        Task<List<EmpresaModel>> GetEmpresas();
        Task<List<EmpresaModel>> GetEmpresasByUsuarioId(UsuarioModel usuarioModel);
        Task<List<EmpresaModel>> GetEmpresasByEvaluacionId(Guid evaluacionId);

       void Dispose();
    }
    public class EmpresaService : IEmpresaService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IEmpresaRepository _EmpresaRepository;
        private ISecurityHelper _securityHelper;

        public EmpresaService(IMapper mapper, IMemoryCache memoryCache, EmpresaRepository EmpresaRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _EmpresaRepository = EmpresaRepository;
            _securityHelper = securityHelper;
        }
        public async Task<EmpresaModel> GetEmpresaById(EmpresaModel empresaModel)
        {
            if (string.IsNullOrEmpty(empresaModel.Id.ToString())) throw new ArgumentNullException("Id");
            var miEmpresa = await _EmpresaRepository.GetEmpresaById(_mapper.Map<Empresa>(empresaModel));
            return _mapper.Map<EmpresaModel>(miEmpresa);
        }

        public async Task<List<EmpresaModel>> GetEmpresas()
        {
            var empresasList = await _EmpresaRepository.GetEmpresas();
            return _mapper.Map<List<EmpresaModel>>(empresasList);
        }
        public async Task<EmpresaModel> InsertOrUpdate(EmpresaModel empresaModel)
        {
            //if (string.IsNullOrEmpty(empresaModel.Id.ToString())) throw new ArgumentNullException("Debe indicar Password");
            if (string.IsNullOrEmpty(empresaModel.RazonSocial)) throw new ArgumentNullException("Debe indicar RazonSocial");
            if (string.IsNullOrEmpty(empresaModel.RutEmpresa)) throw new ArgumentNullException("Debe indicar RutEmpresa");
            if (string.IsNullOrEmpty(empresaModel.Comuna)) throw new ArgumentNullException("Debe indicar Comuna");
            //if (string.IsNullOrEmpty(empresaModel.TipoRubroId.ToString())) throw new ArgumentNullException("Debe indicar TipoRubroId");
            //if (string.IsNullOrEmpty(empresaModel.TipoSubRubroId.ToString())) throw new ArgumentNullException("Debe indicar TipoSubRubroId");
            //if (string.IsNullOrEmpty(empresaModel.TipoTamanoEmpresaId.ToString())) throw new ArgumentNullException("Debe indicar TipoTamanoEmpresaId");
            //if (string.IsNullOrEmpty(empresaModel.TipoNivelVentaId.ToString())) throw new ArgumentNullException("Debe indicar TipoNivelVentaId");
            //if (string.IsNullOrEmpty(empresaModel.TipoCantidadEmpleadoId.ToString())) throw new ArgumentNullException("Debe indicar TipoCantidadEmpleadoId");
            //if (string.IsNullOrEmpty(empresaModel.FechaCreacion.ToString())) throw new ArgumentNullException("Debe indicar FechaCreacion");
            if (string.IsNullOrEmpty(empresaModel.Activo.ToString())) throw new ArgumentNullException("Debe indicar Activo");

            if (empresaModel.RutEmpresa == null)
            {
                var empresa = await _EmpresaRepository.GetEmpresaByRutEmpresa(empresaModel.RutEmpresa);
                empresaModel.Id = empresa.Id;
            }

            var retorno = await _EmpresaRepository.InsertOrUpdate(_mapper.Map<Empresa>(empresaModel));
            return _mapper.Map<EmpresaModel>(retorno);
        }


        public async Task<List<EmpresaModel>> GetEmpresasByUsuarioId(UsuarioModel usuarioModel)
        {

            if (string.IsNullOrEmpty(usuarioModel.Id.ToString())) throw new ArgumentNullException("Debe indicar Id de usuario");
            var empresasList = await _EmpresaRepository.GetEmpresasByUsuarioId(_mapper.Map<Usuario>(usuarioModel));
            return _mapper.Map<List<EmpresaModel>>(empresasList);
        }
        public async Task<List<EmpresaModel>> GetEmpresasByEvaluacionId(Guid evaluacionId)
        {
            if (string.IsNullOrEmpty(evaluacionId.ToString())) throw new ArgumentNullException("Debe indicar Id de evaluacion");
            var empresasList = await _EmpresaRepository.GetEmpresasByEvaluacionId(evaluacionId);
            return _mapper.Map<List<EmpresaModel>>(empresasList);
        }



        public void Dispose() 
        { 
            if (_EmpresaRepository != null)
            {
                _EmpresaRepository.Dispose();
                _EmpresaRepository = null;
            }
        }

    }

}
