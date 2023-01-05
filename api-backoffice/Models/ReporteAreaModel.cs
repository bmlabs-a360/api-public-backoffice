using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class ReporteAreaModel
    {
        public Guid Id { get; set; }
        public Guid ReporteId { get; set; }
        public Guid SegmentacionAreaId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

        /*public virtual Reporte Reporte { get; set; }
        public virtual SegmentacionArea SegmentacionArea { get; set; }*/
    }
}
