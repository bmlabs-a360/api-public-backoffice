using neva.entities;
using System;
using System.Collections.Generic;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class ReporteModel
    {
        /*public Reporte()
        {
            ReporteAreas = new HashSet<ReporteArea>();
            ReporteItems = new HashSet<ReporteItem>();
        }*/

        public Guid Id { get; set; }
        public Guid EvaluacionId { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

        public virtual ICollection<ReporteItemNivelBasicoModel> ReporteItemNivelBasicos { get; set; }
        public virtual ICollection<ReporteAreaModel> ReporteAreas { get; set; }
        public virtual ICollection<ReporteItemModel> ReporteItems { get; set; }
        /*public virtual Evaluacion Evaluacion { get; set; }
        public virtual ICollection<ReporteArea> ReporteAreas { get; set; }
        public virtual ICollection<ReporteItem> ReporteItems { get; set; }*/
    }
}
