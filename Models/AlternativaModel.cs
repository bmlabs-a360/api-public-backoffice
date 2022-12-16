using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class AlternativaModel
    {
        /*public Alternativa()
        {
            PlanMejoras = new HashSet<PlanMejora>();
            Respuesta = new HashSet<Respuesta>();
        }*/

        public Guid Id { get; set; }
        public Guid EvaluacionId { get; set; }
        public Guid PreguntaId { get; set; }
        public string Detalle { get; set; }
        public int Orden { get; set; }
        public int Valor { get; set; }
        public string Retroalimentacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

      /*  public virtual Evaluacion Evaluacion { get; set; }
        public virtual Pregunta Pregunta { get; set; }
        public virtual ICollection<PlanMejora> PlanMejoras { get; set; }
        public virtual ICollection<Respuesta> Respuesta { get; set; }
      */

    }
}
