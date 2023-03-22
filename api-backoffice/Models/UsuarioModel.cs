using System;
using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class UsuarioModel
    {
        public UsuarioModel()
        {
            //Bitacoras = new HashSet<Bitacora>();
            UsuarioEmpresas = new HashSet<UsuarioEmpresaModel>();
            UsuarioEvaluacions = new HashSet<UsuarioEvaluacionModel>();
            UsuarioSuscripcions = new HashSet<UsuarioSuscripcionModel>();
        }

        public Guid Id { get; set; }
        public Guid PerfilId { get; set; }
        public Guid EmpresaId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nombres { get; set; }
        public string Telefono { get; set; }
        public DateTime? FechaUltimoAcceso { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

       public virtual EmpresaModel Empresa { get; set; }
       public virtual PerfilModel Perfil { get; set; }
       //public virtual ICollection<Bitacora> Bitacoras { get; set; }
       public virtual ICollection<UsuarioEmpresaModel> UsuarioEmpresas { get; set; }
       public virtual ICollection<UsuarioEvaluacionModel> UsuarioEvaluacions { get; set; }
       public virtual ICollection<UsuarioSuscripcionModel> UsuarioSuscripcions { get; set; }

    }
}
