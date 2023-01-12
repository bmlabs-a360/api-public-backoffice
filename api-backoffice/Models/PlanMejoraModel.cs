using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class PlanMejoraModel
    {
        /*public PlanMejoraModel()
        {
            Seguimientos = new HashSet<Seguimiento>();
        }*/

        public Guid Id { get; set; }
        public Guid PreguntaId { get; set; }
        public Guid SegmentacionAreaId { get; set; }
        public Guid SegmentacionSubAreaId { get; set; }
        public Guid AlternativaId { get; set; }
        public Guid TipoImportanciaId { get; set; }
        public Guid TipoDiferenciaRelacionadaId { get; set; }
        public string Mejora { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }
        public Guid EvaluacionId { get; set; }

        /*public virtual Alternativa Alternativa { get; set; }
        public virtual SegmentacionArea SegmentacionArea { get; set; }
        public virtual SegmentacionSubArea SegmentacionSubArea { get; set; }
        public virtual TipoDiferenciaRelacionada TipoDiferenciaRelacionada { get; set; }
        public virtual TipoImportancia TipoImportancia { get; set; }
        public virtual ICollection<Seguimiento> Seguimientos { get; set; }*/
    }
}
