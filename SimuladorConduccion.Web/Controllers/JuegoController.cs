using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Web.Mvc;

public class JuegoController : Controller
{
    private string connectionString = ConfigurationManager
        .ConnectionStrings["MyConnection"]
        .ConnectionString;

    [HttpPost]
    public ActionResult Iniciar(int vehiculo, int pista)
    {
        try
        {
            if (Session["IdUsuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string queryJuego = "INSERT INTO Juegos (IdUsuario) VALUES (@IdUsuario); SELECT LAST_INSERT_ID();";
                MySqlCommand cmdJuego = new MySqlCommand(queryJuego, conn);
                cmdJuego.Parameters.AddWithValue("@IdUsuario", idUsuario);

                int idJuego = Convert.ToInt32(cmdJuego.ExecuteScalar());

                string queryTurno = @"INSERT INTO Turnos (IdJuego, IdVehiculo, IdPista, Tiempo, Puntaje)
                                     VALUES (@IdJuego, @Vehiculo, @Pista, 0, 0)";

                MySqlCommand cmdTurno = new MySqlCommand(queryTurno, conn);
                cmdTurno.Parameters.AddWithValue("@IdJuego", idJuego);
                cmdTurno.Parameters.AddWithValue("@Vehiculo", vehiculo);
                cmdTurno.Parameters.AddWithValue("@Pista", pista);

                cmdTurno.ExecuteNonQuery();
            }

            
            TempData["Mensaje"] = "Carrera iniciada 🚗🔥";

            return RedirectToAction("Gameplay", "Home");
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = "Error: " + ex.Message;
            return RedirectToAction("Gameplay", "Home");
        }
    }

    // =========================
    // GAMEPLAY SIMPLE
    // =========================
    public ActionResult Gameplay()
    {
        return View();
    }

    public ActionResult Index()
    {
        return View();
    }
}