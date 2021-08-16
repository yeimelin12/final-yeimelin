using System;
using System.Collections.Generic;

#nullable disable

namespace MediPlus.Models
{
    public partial class Secretarium
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Oficina { get; set; }
        public string Telefono { get; set; }
        public int IdUsuario { get; set; }
        public int IdDoctor { get; set; }

        public virtual Medico IdDoctorNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
