using System;

namespace api_public_backOffice.Models
{
    public class UsuarioEmpresaModel
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid EmpresaId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

        //public virtual Empresa Empresa { get; set; }
        //public virtual Usuario Usuario { get; set; }
    }
}
