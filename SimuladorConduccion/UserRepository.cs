using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SimuladorConduccion.Data
{
    public class UserRepository
    {
        public Usuarios ObtenerUsuario(string nombreUsuario, string contrasenaHash)
        {
            using (var db = new SimuladorConduccionDBEntities1())
            {
                return db.Usuarios.FirstOrDefault(u =>
                    u.NombreUsuario == nombreUsuario &&
                    u.ContrasenaHash == contrasenaHash &&
                    u.Activo
                );
            }
        }
    }
}
