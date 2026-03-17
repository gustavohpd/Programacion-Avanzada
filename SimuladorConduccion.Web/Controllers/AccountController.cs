using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web.Mvc;
using SimuladorConduccion.Web.ViewModels;

public class AccountController : Controller
{
    private string connectionString = ConfigurationManager
        .ConnectionStrings["MyConnection"]
        .ConnectionString;

    // =========================
    // LOGIN GET
    // =========================
    public ActionResult Login()
    {
        return View();
    }

    // =========================
    // LOGIN POST
    // =========================
    [HttpPost]
    public ActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = @"SELECT * FROM Usuarios 
                             WHERE NombreUsuario = @user 
                             AND ContrasenaHash = @pass
                             AND Activo = TRUE";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@user", model.NombreUsuario);
            cmd.Parameters.AddWithValue("@pass", model.Contrasena);

            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                Session["Usuario"] = reader["NombreUsuario"].ToString();
                Session["IdUsuario"] = reader["IdUsuario"].ToString();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                model.Error = "❌ Usuario o contraseña incorrectos";
                return View(model);
            }
        }
    }

    // =========================
    // REGISTRO
    // =========================
    [HttpPost]
    public ActionResult Registrar(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Login", model);

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = @"INSERT INTO Usuarios 
                            (NombreUsuario, ContrasenaHash, Correo, IdRol, Activo)
                            VALUES (@user, @pass, @correo, 2, TRUE)";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@user", model.NombreUsuario);
            cmd.Parameters.AddWithValue("@pass", model.Contrasena);
            cmd.Parameters.AddWithValue("@correo", model.Correo ?? "test@test.com");

            cmd.ExecuteNonQuery();
        }

        return RedirectToAction("Login");
    }
}