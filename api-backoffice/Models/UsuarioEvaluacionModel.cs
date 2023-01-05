using System;
using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class UsuarioEvaluacionModel
    {
        public UsuarioEvaluacionModel()
        {
            UsuarioAreas = new HashSet<UsuarioAreaModel>();
        }

        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid EmpresaId { get; set; }
        public Guid EvaluacionId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

        //public virtual Empresa Empresa { get; set; }
        public virtual EvaluacionModel Evaluacion { get; set; }
        //public virtual Usuario Usuario { get; set; }
        public virtual ICollection<UsuarioAreaModel> UsuarioAreas { get; set; }
    }
}
