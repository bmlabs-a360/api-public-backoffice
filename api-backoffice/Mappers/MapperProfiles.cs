using api_public_backOffice.Models;
using AutoMapper;
using neva.entities;

namespace api_public_backOffice.Mappers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {

            CreateMap<AlternativaModel, Alternativa>().ReverseMap();
            CreateMap<BitacoraModel, Bitacora>().ReverseMap();
            CreateMap<ControlTokenModel, ControlToken>().ReverseMap();
            CreateMap<EmpresaModel, Empresa>().ReverseMap();
            CreateMap<EvaluacionModel, Evaluacion>().ReverseMap();
            CreateMap<EvaluacionEmpresaModel, EvaluacionEmpresa>().ReverseMap();
            CreateMap<ImportanciaEstrategicaModel, ImportanciaEstrategica>().ReverseMap();
            CreateMap<ImportanciaRelativaModel, ImportanciaRelativa>().ReverseMap();
            CreateMap<PerfilModel, Perfil>().ReverseMap();
            CreateMap<PerfilPermisoModel, PerfilPermiso>().ReverseMap();
            CreateMap<PlanMejoraModel, PlanMejora>().ReverseMap();
            CreateMap<PreguntaModel, Pregunta>().ReverseMap();
            CreateMap<ReporteModel, Reporte>().ReverseMap();
            CreateMap<ReporteAreaModel, ReporteArea>().ReverseMap();
            CreateMap<ReporteItemModel, ReporteItem>().ReverseMap();
            CreateMap<RespuestaModel, Respuesta>().ReverseMap();
            CreateMap<SegmentacionAreaModel, SegmentacionArea>().ReverseMap();
            CreateMap<SegmentacionSubAreaModel, SegmentacionSubArea>().ReverseMap();
            CreateMap<SeguimientoModel, Seguimiento>().ReverseMap();
            CreateMap<TipoCantidadEmpleadoModel, TipoCantidadEmpleado>().ReverseMap();
            CreateMap<TipoDiferenciaRelacionadaModel, TipoDiferenciaRelacionada>().ReverseMap();
            CreateMap<TipoImportanciaModel, TipoImportancia>().ReverseMap();
            CreateMap<TipoItemReporteModel, TipoItemReporte>().ReverseMap();
            CreateMap<TipoNivelVentaModel, TipoNivelVenta>().ReverseMap();
            CreateMap<TipoRubroModel, TipoRubro>().ReverseMap();
            CreateMap<TipoSubRubroModel, TipoSubRubro>().ReverseMap();
            CreateMap<TipoTamanoEmpresaModel, TipoTamanoEmpresa>().ReverseMap();
            CreateMap<UsuarioModel, Usuario>().ReverseMap();
            CreateMap<UsuarioAreaModel, UsuarioArea>().ReverseMap();
            CreateMap<UsuarioEmpresaModel, UsuarioEmpresa>().ReverseMap();
            CreateMap<UsuarioEvaluacionModel, UsuarioEvaluacion>().ReverseMap();
            CreateMap<UsuarioSuscripcionModel, UsuarioSuscripcion>().ReverseMap();
            

        }
    }
}