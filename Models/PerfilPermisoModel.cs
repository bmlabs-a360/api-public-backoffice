using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class PerfilPermisoModel
    {
        public Guid Id { get; set; }
        public Guid PerfilId { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

        //public virtual Perfil Perfil { get; set; }
    }
}
