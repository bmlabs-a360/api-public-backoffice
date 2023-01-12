using neva.entities;
using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class EvaluacionEmpresaModel
    {
       /* public EvaluacionEmpresa()
        {
            ImportanciaRelativas = new HashSet<ImportanciaRelativa>();
            Respuesta = new HashSet<Respuesta>();
        }*/

        public Guid Id { get; set; }
        public Guid EvaluacionId { get; set; }
        public Guid EmpresaId { get; set; }
        public DateTime FechaInicioTiempoLimite { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }
        public virtual Evaluacion Evaluacion { get; set; }


        /*public virtual Empresa Empresa { get; set; }
        
        public virtual ICollection<ImportanciaRelativa> ImportanciaRelativas { get; set; }
        public virtual ICollection<Respuesta> Respuesta { get; set; }*/

    }
}
