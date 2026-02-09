using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimuladorConduccion.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string NombreUsuario { get; set; }

        [Required]
        public string Contrasena { get; set; }

        public string Error { get; set; }
    }
}