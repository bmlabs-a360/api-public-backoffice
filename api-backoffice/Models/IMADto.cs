using neva.entities;
using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class IMADto
    {
        public Guid EvaluacionId { get; set; }
        public Guid EmpresaId { get; set; }
        public Guid EvaluacionEmpresaId { get; set; }
        public Guid SegmentacionAreaId { get; set; }
        public string RazonSocial { get; set; }
        public string NombreEvaluacion { get; set; }
        public string NombreArea { get; set; }
        public decimal PesoRelativoAreaPorc  { get; set; }
        public decimal IMAValor { get; set; }

    }
}
