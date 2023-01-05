using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class TipoItemReporteModel
    {
        /*public TipoItemReporteModel()
        {
            ReporteItems = new HashSet<ReporteItem>();
        }*/

        public Guid Id { get; set; }
        public int Orden { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }

        //public virtual ICollection<ReporteItem> ReporteItems { get; set; }
    }
}
