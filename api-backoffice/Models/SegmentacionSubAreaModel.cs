using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class SegmentacionSubAreaModel
    { 
       /*public SegmentacionSubAreaModel()
        {
            ImportanciaEstrategicas = new HashSet<ImportanciaEstrategica>();
            PlanMejoras = new HashSet<PlanMejora>();
            Pregunta = new HashSet<Pregunta>();
        }*/

        public Guid Id { get; set; }
        public Guid SegmentacionAreaId { get; set; }
        public string NombreSubArea { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

        /*public virtual SegmentacionArea SegmentacionArea { get; set; }
        public virtual ICollection<ImportanciaEstrategica> ImportanciaEstrategicas { get; set; }
        public virtual ICollection<PlanMejora> PlanMejoras { get; set; }
        public virtual ICollection<Pregunta> Pregunta { get; set; }*/
    }
}
