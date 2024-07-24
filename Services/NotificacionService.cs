using ControlEmpresarial.Controlador;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Web;

namespace ControlEmpresarial.Services
{
    public class NotificacionService
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        public List<Notificacion> ObtenerNotificaciones(int idRecibidor)
        {
            List<Notificacion> notificaciones = new List<Notificacion>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"
                    SELECT n.IdNotificacion, n.IdEnviador, n.IdRecibidor, n.Titulo, n.Motivo, n.Fecha,
                           e.Nombre AS EnviadorNombre, e.Apellidos AS EnviadorApellidos
                    FROM Notificaciones n
                    JOIN Empleado e ON n.IdEnviador = e.idEmpleado
                    WHERE n.IdRecibidor = @IdRecibidor";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdRecibidor", idRecibidor);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Notificacion notificacion = new Notificacion
                    {
                        IdNotificacion = reader.GetInt32("IdNotificacion"),
                        IdEnviador = reader.GetInt32("IdEnviador"),
                        IdRecibidor = reader.GetInt32("IdRecibidor"),
                        Titulo = reader.GetString("Titulo"),
                        Motivo = reader.GetString("Motivo"),
                        Fecha = reader.GetDateTime("Fecha"),
                        EnviadorNombre = reader.GetString("EnviadorNombre"),
                        EnviadorApellidos = reader.GetString("EnviadorApellidos")
                    };
                    notificaciones.Add(notificacion);
                }
            }

            return notificaciones;
        }

        public void InsertarNotificacion(int idEnviador, int idRecibidor, string titulo, string motivo, DateTime fecha)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO Notificaciones (IdEnviador, IdRecibidor, Titulo, Motivo, Fecha)
                    VALUES (@IdEnviador, @IdRecibidor, @Titulo, @Motivo, @Fecha)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdEnviador", idEnviador);
                cmd.Parameters.AddWithValue("@IdRecibidor", idRecibidor);
                cmd.Parameters.AddWithValue("@Titulo", titulo);
                cmd.Parameters.AddWithValue("@Motivo", motivo);
                cmd.Parameters.AddWithValue("@Fecha", fecha);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
