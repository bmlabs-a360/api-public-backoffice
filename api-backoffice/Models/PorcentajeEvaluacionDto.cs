using System;

namespace api_public_backOffice.Models
{
    public class PorcentajeEvaluacionDto
    {
        public string Nombre { get; set; }
        public string NombreArea { get; set; }
        public Guid SegmentacionAreaId { get; set; }
        public string RespuestaPorcentaje { get; set; }
    }
}
