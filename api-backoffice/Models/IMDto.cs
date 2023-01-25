using neva.entities;
using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class IMDto
    {
        public Guid EvaluacionId { get; set; }
        public Guid EmpresaId { get; set; }
        public Guid EvaluacionEmpresaId { get; set; }
        public string RazonSocial { get; set; }
        public string NombreEvaluacion { get; set; }
        public decimal IMValor { get; set; }

    }
}
