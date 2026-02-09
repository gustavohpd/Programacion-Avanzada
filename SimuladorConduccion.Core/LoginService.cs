using SimuladorConduccion.Data;
using SimuladorConduccion.Models;
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
        private readonly UserRepository _repo = new UserRepository();

        public User Validate(string username, string password)
        {
            string hash = HashPassword(password);
            return _repo.GetUser(username, hash);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
