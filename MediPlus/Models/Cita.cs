using System;
using System.Collections.Generic;

#nullable disable

namespace MediPlus.Models
{
    public partial class Cita
    {
        public int Id { get; set; }
        public int? IdPacientes { get; set; }
        public int? IdMedico { get; set; }
        public string MotivoDeConsulta { get; set; }
        public DateTime FechaCita { get; set; }
        public string Comentario { get; set; }
        public string Estado { get; set; }

        public virtual Medico IdMedicoNavigation { get; set; }
        public virtual Paciente IdPacientesNavigation { get; set; }
    }
}
