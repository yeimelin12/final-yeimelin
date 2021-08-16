using MediPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediPlus.ViewModels
{
    public class UsuarioVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public RolVM Rol { get; set; }

        public DoctorVM Doctor { get; set; }




    }
}
