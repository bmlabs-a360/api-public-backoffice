using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class ControlTokenModel
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}