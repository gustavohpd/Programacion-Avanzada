using SimuladorConduccion.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorConduccion.Core
{
    public class LoginService
    {
        public Usuarios ValidarUsuario(string nombreUsuario, string contrasena)
        {
            using (var db = new SimuladorConduccionDBEntities1())
            {
                return db.Usuarios.FirstOrDefault(u =>
                    u.NombreUsuario == nombreUsuario &&
                    u.ContrasenaHash == contrasena &&
                    u.Activo
                );
            }
        }
    }
}
