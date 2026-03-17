using MySql.Data.MySqlClient;

namespace SimuladorConduccion.Core
{
    public class LoginService
    {
        private string connectionString =
            "server=localhost;database=SimuladorConduccion;uid=root;pwd=;";

        public UsuarioLogin ValidarUsuario(string nombreUsuario, string contrasena)
        {
            UsuarioLogin usuario = null;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT IdUsuario, NombreUsuario 
                                 FROM Usuarios 
                                 WHERE NombreUsuario=@user 
                                 AND ContrasenaHash=@pass 
                                 AND Activo=1";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", nombreUsuario);
                cmd.Parameters.AddWithValue("@pass", contrasena);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    usuario = new UsuarioLogin
                    {
                        IdUsuario = (int)reader["IdUsuario"],
                        NombreUsuario = reader["NombreUsuario"].ToString()
                    };
                }
            }

            return usuario;
        }
    }
}