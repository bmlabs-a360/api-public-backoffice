using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class RespuestaModel
    {
        public Guid Id { get; set; }
        public Guid AlternativaId { get; set; }
        public Guid PreguntaId { get; set; }
        public Guid EvaluacionEmpresaId { get; set; }
        public Guid TipoImportanciaId { get; set; }
        public Guid TipoDiferenciaRelacionadaId { get; set; }
        public int Valor { get; set; }
        public string Realimentacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

        /*public virtual Alternativa Alternativa { get; set; }
        public virtual EvaluacionEmpresa EvaluacionEmpresa { get; set; }
        public virtual Pregunta Pregunta { get; set; }
        public virtual TipoDiferenciaRelacionada TipoDiferenciaRelacionada { get; set; }
        public virtual TipoImportancia TipoImportancia { get; set; }*/
    }
}
