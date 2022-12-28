using System;

namespace api_public_backOffice.Models
{
    public class UsuarioAreaModel
    {
        public Guid Id { get; set; }
        public Guid UsuarioEvaluacionId { get; set; }
        public Guid SegmentacionAreaId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

        //public virtual SegmentacionArea SegmentacionArea { get; set; }
        //public virtual UsuarioEvaluacion UsuarioEvaluacion { get; set; }
    }
}
