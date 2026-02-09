using SimuladorConduccion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimuladorConduccion.Models;


namespace SimuladorConduccion.Data
{
    public class UserRepository
    {
        public User GetUser(string username, string passwordHash)
        {
            using (var db = new SimuladorConduccionDb())
            {
                return db.Users.FirstOrDefault(u =>
                    u.Username == username &&
                    u.PasswordHash == passwordHash &&
                    u.IsActive);
            }
        }
    }
}
