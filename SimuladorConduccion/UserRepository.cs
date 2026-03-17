using System;
using MySql.Data.MySqlClient;
using System.Configuration;
using SimuladorConduccion.Models;

namespace SimuladorConduccion.Data
{
    public class UserRepository
    {
        public User ObtenerUser(string nombreUsuario, string contrasena)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Usuarios WHERE NombreUsuario=@usuario AND ContrasenaHash=@password AND Activo=1";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
                cmd.Parameters.AddWithValue("@password", contrasena);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        UserId = reader.GetInt32("IdUsuario"),
                        Username = reader.GetString("NombreUsuario"),
                        PasswordHash = reader.GetString("ContrasenaHash"),
                        IsActive = reader.GetBoolean("Activo")
                    };
                }
            }

            return null;
        }
    }
}