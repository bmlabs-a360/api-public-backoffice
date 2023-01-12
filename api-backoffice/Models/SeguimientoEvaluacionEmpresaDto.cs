using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class SeguimientoEvaluacionEmpresaDto
    {
        public Guid EvaluacionEmpresaId { get; set; }
        /* INICIO atributos solo para el seguimiento LB*/
        public string EmpresaRazonSocial { get; set; }
        public string EvaluacionNombre { get; set; }
        public string Madurez { get; set; }
        public string Respuestas { get; set; }
        public string RespuestaFechaMax { get; set; }
        public string PlanMejoras { get; set; }
        public string PlanMejoraFechaMax { get; set; }

        public bool Estado { get; set; }
        /* FIN atributos solo para el seguimiento LB*/
    }
}
