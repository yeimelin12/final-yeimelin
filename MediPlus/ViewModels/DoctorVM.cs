using MediPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediPlus.ViewModels
{
    public class DoctorVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int? Especialidad { get; set; }
        public string Oficina { get; set; }
        public string Telefono { get; set; }
        public int IdUsuario { get; set; }

        public virtual Especialidade EspecialidadNavigation { get; set; }
    }
}
