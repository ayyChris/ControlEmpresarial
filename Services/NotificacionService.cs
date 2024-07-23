using ControlEmpresarial.Controlador;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;  // Cambiar a MySql.Data.MySqlClient
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
                string query = "SELECT * FROM Notificaciones WHERE IdRecibidor = @IdRecibidor";

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
                        Fecha = reader.GetDateTime("Fecha")
                    };
                    notificaciones.Add(notificacion);
                }
            }

            return notificaciones;
        }
    }
}
