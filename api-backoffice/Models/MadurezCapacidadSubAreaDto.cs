using neva.entities;
using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class MadurezCapacidadSubAreaDto
    {
        public Guid EvaluacionId { get; set; }
        public Guid EmpresaId { get; set; }
        public Guid PreguntaId { get; set; }
        public Guid RespuestaId { get; set; }
        public Guid TipoImportanciaId { get; set; }
        public Guid TipoDiferenciaRelacionadaId { get; set; }
        public Guid ImportanciaRelativaId { get; set; }
        public Guid EvaluacionEmpresaId { get; set; }
        public Guid ImportanciaEstrategicaId { get; set; }
        public Guid SegmentacionAreaId { get; set; }
        public Guid SegmentacionSubAreaId { get; set; }
        public string RazonSocial { get; set; }
        public string NombreEvaluacion { get; set; }
        public string NombreArea { get; set; }
        public string NombreSubArea { get; set; }
        public string  Capacidad  { get; set; }
        public string Pregunta  { get; set; }
        public string Respuesta  { get; set; }
        public decimal PesoRelativoAreaPorc  { get; set; }
        public decimal  PesoRelativoRubareaPorc  { get; set; }
        public decimal PesoRelativoCapacidadValor  { get; set; }
        public decimal PesoRelativoCapacidadPorc { get; set; }
        public decimal RespuestaValor { get; set; }

    }
}
