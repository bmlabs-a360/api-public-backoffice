using neva.entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api_public_backOffice.Models
{
    public class EvaluacionModel
    {
       public EvaluacionModel()
        {
            //Alternativas = new HashSet<Alternativa>();
           // EvaluacionEmpresas = new HashSet<EvaluacionEmpresaModel>();
            //Pregunta = new HashSet<Pregunta>();
            //Reportes = new HashSet<Reporte>();
            SegmentacionAreas = new HashSet<SegmentacionAreaModel>();
            //Seguimientos = new HashSet<Seguimiento>();
            //UsuarioEvaluacions = new HashSet<UsuarioEvaluacion>();
        }

        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string TiempoLimite { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int CantidadEmpresas { get; set; }
        public int CantidadAreas{ get; set; }
        public int CantidadSubAreas{ get; set; }
        public int CantidadPreguntas { get; set; }
        public int CantidadAlternativas { get; set; }
        public bool? Activo { get; set; }
        public bool? Default { get; set; }

        //public virtual ICollection<Alternativa> Alternativas { get; set; }
        //public virtual ICollection<EvaluacionEmpresaModel> EvaluacionEmpresas { get; set; }
        //public virtual ICollection<Pregunta> Pregunta { get; set; }
        //public virtual ICollection<Reporte> Reportes { get; set; }
        public virtual ICollection<SegmentacionAreaModel> SegmentacionAreas { get; set; }
        //public virtual ICollection<Seguimiento> Seguimientos { get; set; }
        //public virtual ICollection<UsuarioEvaluacion> UsuarioEvaluacions { get; set; }

    }
}
