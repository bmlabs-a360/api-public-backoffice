using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class SeguimientoPlanMejoraModelDto
    {
        public Guid EvaluacionEmpresaId { get; set; }
        public Guid EvaluacionId { get; set; }
        public Guid EmpresaId { get; set; }
        public string FechaInicioTiempoLimite { get; set; }
        public Guid PreguntaId { get; set; }
        public Guid SegmentacionAreaId { get; set; }
        public Guid SegmentacionSubAreaId { get; set; }
        public string PreguntaDetalle { get; set; }
        public string PreguntaOrden { get; set; }
        public string PreguntaCapacidad { get; set; }
        public Guid AlternativaId { get; set; }
        public string AlternativaDetalle { get; set; }
        public string AlternativaValor { get; set; }
        public Guid TipoImportanciaId { get; set; }
        public Guid TipoDiferenciaRelacionadaId { get; set; }
        public string RespuestaValor { get; set; }
        public string RespuestaRealimentacion { get; set; }
        public Guid PlanMejoraId { get; set; }
        public string Mejora { get; set; }
        public string NombreArea { get; set; }
        public string NombreSubArea { get; set; }
        public bool Accion { get; set; }
        public bool Estado { get; set; }
        public string TipoImportanciaNombre { get; set; }
        
	  public string TipoDiferenciaRelacionadaNombre { get; set; }
        


    }
}
