using System;
//using System.Collections.Generic;

namespace api_public_backOffice.Models
{
    public class BitacoraModel
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }

        //public virtual UsuarioModel Usuario { get; set; }

    }
}
