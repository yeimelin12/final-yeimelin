using System;
using System.Collections.Generic;

#nullable disable

namespace MediPlus.Models
{
    public partial class Codigosvalidacion
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public int UsuarioId { get; set; }
        public int Enviado { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
