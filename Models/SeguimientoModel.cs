using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class SeguimientoModel
    {
        public Guid Id { get; set; }
        public Guid EmpresaId { get; set; }
        public Guid EvaluacionId { get; set; }
        public Guid PlanMejoraId { get; set; }
        public int Madurez { get; set; }
        public int PorcentajeRespuestas { get; set; }
        public int PorcentajePlaMejora { get; set; }
        public DateTime FechaUltimoAcceso { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

        /*public virtual Empresa Empresa { get; set; }
        public virtual Evaluacion Evaluacion { get; set; }
        public virtual PlanMejora PlanMejora { get; set; }*/
    }
}
