using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class TipoNivelVentaModel
    {
        /*public TipoNivelVentaModel()
        {
            Empresas = new HashSet<Empresa>();
        }*/

        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

        //public virtual ICollection<Empresa> Empresas { get; set; }
    }
}
