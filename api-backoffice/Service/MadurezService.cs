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
    public interface IMadurezService
    {
        List<MadurezCapacidadSubAreaDto> GetCapacidadSubAreas(MadurezCapacidadSubAreaDto madurezCapacidadSubAreaDto);
        List<IMSADto> GetIMSA(MadurezCapacidadSubAreaDto madurezCapacidadSubAreasDto);
        List<IMADto> GetIMA(MadurezCapacidadSubAreaDto madurezCapacidadSubAreasDto);
        List<IMDto> GetIM(MadurezCapacidadSubAreaDto madurezCapacidadSubAreasDto);
        List<MadurezCapacidadSubAreaDto> GetAllCapacidadSubAreas();
        List<IMSADto> GetAllIMSA();
        List<IMADto> GetAllIMA();
        List<IMDto> GetAllIM();
        Task<List<IMADto>> GetIMAReporteSubscripcionOBasico(UsuarioModel usuario, Guid evaluacionId, Guid empresaId);
        
        void Dispose();
    }
    public class MadurezService : IMadurezService, IDisposable
    {
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        private IMadurezRepository _madurezRepository;
        private IUsuarioRepository _usuarioRepository;
        private IPerfilRepository _PerfilRepository;
        private IUsuarioSuscripcionRepository _UsuarioSuscripcionRepository;
        private IReporteRepository _ReporteRepository;
        private ISecurityHelper _securityHelper;
        public MadurezService(IMapper mapper, IMemoryCache memoryCache, ReporteRepository ReporteRepository, UsuarioSuscripcionRepository UsuarioSuscripcionRepository, UsuarioRepository usuarioRepository, PerfilRepository PerfilRepository, MadurezRepository madurezRepository, SecurityHelper securityHelper)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _madurezRepository = madurezRepository;
            _usuarioRepository = usuarioRepository;
            _PerfilRepository = PerfilRepository;
            _UsuarioSuscripcionRepository = UsuarioSuscripcionRepository;
            _ReporteRepository = ReporteRepository;
            _securityHelper = securityHelper;
        }
        

        public List<MadurezCapacidadSubAreaDto> GetCapacidadSubAreas(MadurezCapacidadSubAreaDto madurezCapacidadSubAreaDto)
        {
            return _madurezRepository.GetCapacidadSubAreas(madurezCapacidadSubAreaDto);
        }

        public List<IMSADto> GetIMSA(MadurezCapacidadSubAreaDto madurezCapacidadSubAreasDto)
        {
            return _madurezRepository.GetIMSA(madurezCapacidadSubAreasDto);
        }
        public List<IMADto> GetIMA(MadurezCapacidadSubAreaDto madurezCapacidadSubAreasDto)
        {
            return _madurezRepository.GetIMA(madurezCapacidadSubAreasDto);
        }
        public async Task<List<IMADto>> GetIMAReporteSubscripcionOBasico(UsuarioModel usuario, Guid evaluacionId, Guid empresaId)
        {
            List<IMADto> retorno = null;
            PerfilModel perfil = new PerfilModel
            {
                Id = usuario.PerfilId
            };

            EvaluacionModel evaluacion = new EvaluacionModel
            {
                Id = evaluacionId
            };
            var reporteRetorno = await _ReporteRepository.GetReportesByEvaluacionId(_mapper.Map<Evaluacion>(evaluacion));

            List<Guid> areas = new();
            foreach (var rr in reporteRetorno)
            {
                foreach (var ra in rr.ReporteAreas)
                {
                    //if (ra.Activo == true)
                    areas.Add(ra.SegmentacionAreaId);
                }
            }

            var miPerfil = await _PerfilRepository.GetPerfilById(_mapper.Map<Perfil>(perfil));


            if (miPerfil.Nombre != "Usuario pro (empresa)")
            {
                var miPerfilPro = await _PerfilRepository.GetPerfilsUsuarioPro();
                var perfilId = miPerfilPro.Id;
                var usuarioPro = await _usuarioRepository.GetUsuarioByPerfilIdEmpresaId(perfilId, empresaId);
                if (usuarioPro != null)
                {
                    var usuarioSubscripcion = await _UsuarioSuscripcionRepository.GetUsuarioSuscripcionsByUsuarioId(usuarioPro);
                    if (usuarioSubscripcion == null || usuarioSubscripcion.Activo == false)
                    {
                        retorno = _madurezRepository.GetIMAByAreasUsuarioBasico(evaluacionId, empresaId, areas, false);
                    }
                    else
                    {
                        retorno = _madurezRepository.GetIMAByAreasUsuarioBasico(evaluacionId, empresaId, areas, true);
                    }
                }
                else 
                {
                    retorno = _madurezRepository.GetIMAByAreasUsuarioBasico(evaluacionId, empresaId, areas, false);
                }
                
            }
            else 
            {
                var usuarioSubscripcion = await _UsuarioSuscripcionRepository.GetUsuarioSuscripcionsByUsuarioId(_mapper.Map<Usuario>(usuario));
                if (usuarioSubscripcion == null || usuarioSubscripcion.Activo == false)
                {
                    retorno = _madurezRepository.GetIMAByAreasUsuarioBasico(evaluacionId, empresaId, areas, false);
                }
                else 
                {
                    retorno = _madurezRepository.GetIMAByAreasUsuarioBasico(evaluacionId, empresaId, areas, true);
                }
            }

            return retorno;
            //UsuarioSuscripcionModel usuarioRetorno = await _usuarioSubscripcionService.GetUsuarioSuscripcionsByUsuarioId(usuario);
            //return _madurezRepository.GetIMAByAreasUsuarioBasico(evaluacionId, empresaId, areas);
        }
        public List<IMDto> GetIM(MadurezCapacidadSubAreaDto madurezCapacidadSubAreasDto)
        {
            return _madurezRepository.GetIM(madurezCapacidadSubAreasDto);
        }

        public List<MadurezCapacidadSubAreaDto> GetAllCapacidadSubAreas( )
        {
            return _madurezRepository.GetAllCapacidadSubAreas();
        }

        public List<IMSADto> GetAllIMSA( )
        {
            return _madurezRepository.GetAllIMSA();
        }
        public List<IMADto> GetAllIMA( )
        {
            return _madurezRepository.GetAllIMA();
        }
        public List<IMDto> GetAllIM( )
        {
            return _madurezRepository.GetAllIM();
        }
        public void Dispose()
        {
            if (_madurezRepository != null)
            {
                _madurezRepository.Dispose();
                _madurezRepository = null;
            }
        }

    }

}
