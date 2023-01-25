using neva.entities;
using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class IMSADto
    {
        public Guid EvaluacionId { get; set; }
        public Guid EmpresaId { get; set; }
        public Guid ImportanciaRelativaId { get; set; }
        public Guid EvaluacionEmpresaId { get; set; }
        public Guid ImportanciaEstrategicaId { get; set; }
        public Guid SegmentacionAreaId { get; set; }
        public Guid SegmentacionSubAreaId { get; set; }
        public string RazonSocial { get; set; }
        public string NombreEvaluacion { get; set; }
        public string NombreArea { get; set; }
        public string NombreSubArea { get; set; }
        public decimal PesoRelativoAreaPorc  { get; set; }
        public decimal  PesoRelativoRubareaPorc  { get; set; }
        public decimal IMSAValor { get; set; }

    }
}
