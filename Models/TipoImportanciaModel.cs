﻿using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class TipoImportanciaModel
    {
        /*public TipoImportanciaModel()
        {
            PlanMejoras = new HashSet<PlanMejora>();
            Respuesta = new HashSet<Respuesta>();
        }*/

        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }

        /*public virtual ICollection<PlanMejora> PlanMejoras { get; set; }
        public virtual ICollection<Respuesta> Respuesta { get; set; }*/
    }
}
