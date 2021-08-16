using System;
using System.Collections.Generic;

#nullable disable

namespace MediPlus.Models
{
    public partial class Paciente
    {
        public Paciente()
        {
            Cita = new HashSet<Cita>();
        }

        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Cedula { get; set; }
        public int? Edad { get; set; }
        public string LugarDeNacimiento { get; set; }
        public string Sexo { get; set; }
        public int IdUsuario { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Cita> Cita { get; set; }
    }
}
