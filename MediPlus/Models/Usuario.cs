using System;
using System.Collections.Generic;

#nullable disable

namespace MediPlus.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Codigosvalidacions = new HashSet<Codigosvalidacion>();
            Medicos = new HashSet<Medico>();
            Pacientes = new HashSet<Paciente>();
            Secretaria = new HashSet<Secretarium>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public int? RolId { get; set; }
        public int? Confirmado { get; set; }

        public virtual Role Rol { get; set; }
        public virtual ICollection<Codigosvalidacion> Codigosvalidacions { get; set; }
        public virtual ICollection<Medico> Medicos { get; set; }
        public virtual ICollection<Paciente> Pacientes { get; set; }
        public virtual ICollection<Secretarium> Secretaria { get; set; }
    }
}
