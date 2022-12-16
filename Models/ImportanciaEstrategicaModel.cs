using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class ImportanciaEstrategicaModel
    {
        public Guid Id { get; set; }
        public Guid ImportanciaRelativaId { get; set; }
        public Guid SegmentacionSubAreaId { get; set; }
        public int Valor { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

       /* public virtual ImportanciaRelativa ImportanciaRelativa { get; set; }
        public virtual SegmentacionSubArea SegmentacionSubArea { get; set; }*/

    }
}
