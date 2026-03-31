using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Web.Mvc;

public class HomeController : Controller
{
    private string connectionString = ConfigurationManager
        .ConnectionStrings["MyConnection"]
        .ConnectionString;

    public ActionResult Index()
    {
        if (Session["Usuario"] == null)
            return RedirectToAction("Login", "Account");

        List<dynamic> vehiculos = new List<dynamic>();
        List<dynamic> pistas = new List<dynamic>();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            //  VEHICULOS
            string queryVehiculos = "SELECT IdVehiculo, NombreVehiculo, VelocidadMaxima FROM Vehiculos LIMIT 3";
            MySqlCommand cmd1 = new MySqlCommand(queryVehiculos, conn);
            var reader1 = cmd1.ExecuteReader();

            while (reader1.Read())
            {
                dynamic v = new ExpandoObject();
                v.IdVehiculo = Convert.ToInt32(reader1["IdVehiculo"]);
                v.Nombre = reader1["NombreVehiculo"].ToString();
                v.Velocidad = Convert.ToInt32(reader1["VelocidadMaxima"]);

                vehiculos.Add(v);
            }

            reader1.Close(); // 

            //  PISTAS
            string queryPistas = "SELECT IdPista, NombrePista, Dificultad FROM Pistas LIMIT 3";
            MySqlCommand cmd2 = new MySqlCommand(queryPistas, conn);
            var reader2 = cmd2.ExecuteReader();

            while (reader2.Read())
            {
                dynamic p = new ExpandoObject();
                p.IdPista = Convert.ToInt32(reader2["IdPista"]);
                p.Nombre = reader2["NombrePista"].ToString();
                p.Dificultad = reader2["Dificultad"].ToString();

                pistas.Add(p);
            }

            reader2.Close(); // 
        }

        ViewBag.Vehiculos = vehiculos;
        ViewBag.Pistas = pistas;

        return View();
    }


    [HttpPost]
    public ActionResult SeleccionarVehiculo(int idVehiculo, int velocidad)
    {
        Session["Vehiculo"] = idVehiculo;
        Session["Velocidad"] = velocidad;

        return RedirectToAction("Gameplay");
    }

    [HttpPost]
    public ActionResult SeleccionarTodo(int idVehiculo, int velocidad, int idPista, int dificultad)
    {
        Session["Vehiculo"] = idVehiculo;
        Session["Velocidad"] = velocidad;
        Session["Pista"] = idPista;
        Session["Dificultad"] = dificultad;

        return RedirectToAction("Gameplay");
    }
    public ActionResult Gameplay()
    {
        // opcional: proteger acceso
        if (Session["Usuario"] == null)
            return RedirectToAction("Login", "Account");

        return View();
    }

    [HttpPost]
    public JsonResult GuardarPuntaje(int puntaje)
    {
        int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            //  1. Guardar en TURNOS (último juego)
            string updateTurno = @"
            UPDATE Turnos 
            SET Puntaje = @Puntaje 
            WHERE IdTurno = (
                SELECT IdTurno FROM Turnos 
                ORDER BY IdTurno DESC LIMIT 1
            )";

            MySqlCommand cmd1 = new MySqlCommand(updateTurno, conn);
            cmd1.Parameters.AddWithValue("@Puntaje", puntaje);
            cmd1.ExecuteNonQuery();

            //  2. Actualizar ranking
            string checkRanking = "SELECT COUNT(*) FROM Rankings WHERE IdUsuario = @IdUsuario";
            MySqlCommand checkCmd = new MySqlCommand(checkRanking, conn);
            checkCmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

            int existe = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (existe > 0)
            {
                string updateRanking = @"
                UPDATE Rankings 
                SET PuntajeTotal = PuntajeTotal + @Puntaje 
                WHERE IdUsuario = @IdUsuario";

                MySqlCommand cmd2 = new MySqlCommand(updateRanking, conn);
                cmd2.Parameters.AddWithValue("@Puntaje", puntaje);
                cmd2.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd2.ExecuteNonQuery();
            }
            else
            {
                string insertRanking = @"
                INSERT INTO Rankings (IdUsuario, PuntajeTotal)
                VALUES (@IdUsuario, @Puntaje)";

                MySqlCommand cmd3 = new MySqlCommand(insertRanking, conn);
                cmd3.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd3.Parameters.AddWithValue("@Puntaje", puntaje);
                cmd3.ExecuteNonQuery();
            }
        }



        return Json(true);
    }

    public ActionResult Ranking()
    {
        List<dynamic> ranking = new List<dynamic>();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = @"
        SELECT U.NombreUsuario, R.PuntajeTotal, U.Monedas
        FROM Rankings R
        JOIN Usuarios U ON U.IdUsuario = R.IdUsuario
        ORDER BY R.PuntajeTotal DESC
        LIMIT 10";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                dynamic r = new ExpandoObject();
                r.NombreUsuario = reader["NombreUsuario"].ToString();
                r.PuntajeTotal = Convert.ToInt32(reader["PuntajeTotal"]);
                r.Monedas = Convert.ToInt32(reader["Monedas"]);

                ranking.Add(r);
            }
        }

        ViewBag.Ranking = ranking;
        return View();
    }

    public ActionResult SeleccionarVehiculo()
    {
        List<dynamic> vehiculos = new List<dynamic>();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = "SELECT IdVehiculo, Nombre, Velocidad FROM Vehiculos LIMIT 3";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                dynamic v = new ExpandoObject();
                v.IdVehiculo = Convert.ToInt32(reader["IdVehiculo"]);
                v.Nombre = reader["Nombre"].ToString();
                v.Velocidad = Convert.ToInt32(reader["Velocidad"]);

                vehiculos.Add(v);
            }
        }

        ViewBag.Vehiculos = vehiculos;
        return View();
    }

    [HttpPost]
    public ActionResult ElegirVehiculo(int idVehiculo, int velocidad)
    {
        Session["Vehiculo"] = idVehiculo;
        Session["Velocidad"] = velocidad;

        return RedirectToAction("Gameplay");
    }

    public ActionResult GameOver()
    {
        return View();
    }


}