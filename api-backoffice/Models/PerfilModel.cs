using System;
using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class PerfilModel
    {
        public PerfilModel()
        {
            PerfilPermisos = new HashSet<PerfilPermisoModel>();
            //Usuarios = new HashSet<Usuario>();
        }

        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

        public virtual ICollection<PerfilPermisoModel> PerfilPermisos { get; set; }
        //public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
