using System;
using System.Collections.Generic;

#nullable disable

namespace MediPlus.Models
{
    public partial class Medico
    {
        public Medico()
        {
            Cita = new HashSet<Cita>();
            Secretaria = new HashSet<Secretarium>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int? Especialidad { get; set; }
        public string Oficina { get; set; }
        public string Telefono { get; set; }
        public int IdUsuario { get; set; }
        public int CapacidadPacientes { get; set; }

        public string CedulaIdentidad { get; set; }

        public virtual Especialidade EspecialidadNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Cita> Cita { get; set; }
        public virtual ICollection<Secretarium> Secretaria { get; set; }
    }
}
