﻿using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Controlador
{
    public partial class SolicitarVacacion : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Obtener la cookie
                HttpCookie cookie = Request.Cookies["UserInfo"];
                if (cookie != null)
                {
                    // Extraer el valor de idEmpleado
                    string idEmpleadoValue = ConseguirCookie(cookie.Value, "idEmpleado");

                    // Asegúrate de que idEmpleadoValue no sea null y conviértelo a entero
                    if (idEmpleadoValue != null && int.TryParse(idEmpleadoValue, out int idEmpleado))
                    {
                        System.Diagnostics.Debug.WriteLine("idEmpleado extraído de la cookie: " + idEmpleado);
                        CargarDiasDisponibles(idEmpleado);
                    }
                    else
                    {
                        vacacionesCountLabel.Text = "Error: ID de empleado no válido.";
                    }
                }
                else
                {
                    vacacionesCountLabel.Text = "0";
                }
            }
        }

        private string ConseguirCookie(string cookieString, string key)
        {
            // Divide la cadena de cookie en pares clave-valor
            var pairs = cookieString.Split('&');
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2 && keyValue[0] == key)
                {
                    return keyValue[1];
                }
            }
            return null;
        }
        private void CargarDiasDisponibles(int idEmpleado)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT DiasDeVacaciones FROM Empleado WHERE idEmpleado = @idEmpleado";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        int diasDeVacaciones = Convert.ToInt32(result);
                        vacacionesCountLabel.Text = diasDeVacaciones.ToString();
                    }
                    else
                    {
                        vacacionesCountLabel.Text = "0"; // Valor predeterminado si no se encuentra el registro
                        System.Diagnostics.Debug.WriteLine("No se encontró el registro para idEmpleado: " + idEmpleado);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error al encontrar el label: {ex.Message}')", true);
                }
            }
        }



        protected void submit_Click(object sender, EventArgs e)
        {
            DateTime fechaInicioTexto;
            DateTime fechaFinalTexto;

            // Inicializar las fechas con valores predeterminados
            fechaInicioTexto = DateTime.MinValue;
            fechaFinalTexto = DateTime.MinValue;

            // Obtener el texto de los TextBox y convertir a DateTime
            bool fechaInicioValida = DateTime.TryParse(fechaInicio.Text, out fechaInicioTexto);
            bool fechaFinalValida = DateTime.TryParse(fechaFinal.Text, out fechaFinalTexto);

            // Verificar si las fechas son válidas
            if (!fechaInicioValida || !fechaFinalValida)
            {
                System.Diagnostics.Debug.WriteLine("Error: Formato de fecha no válido.");
                return;
            }

            if (fechaFinalTexto < fechaInicioTexto)
            {
                System.Diagnostics.Debug.WriteLine("Error: La fecha final debe ser posterior a la fecha de inicio.");
                return;
            }

            if (HayFechasNoDisponibles(fechaInicioTexto, fechaFinalTexto))
            {
                System.Diagnostics.Debug.WriteLine("Error: Algunas de las fechas solicitadas no están disponibles.");
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Algunas de las fechas solicitadas no están disponibles.');", true);
                return;
            }
            int diasSolicitados = (fechaFinalTexto - fechaInicioTexto).Days + 1; // +1 para incluir el último día
            System.Diagnostics.Debug.WriteLine("Días solicitados: " + diasSolicitados);

            int diasDisponibles;
            if (!int.TryParse(vacacionesCountLabel.Text, out diasDisponibles))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Días disponibles no válidos para la cantidad de dias solicitados.');", true);
                return;
            }

            // Verificar si hay suficientes días disponibles
            if (diasSolicitados > diasDisponibles)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('No hay suficientes días disponibles.');", true);
                return;
            }

            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: Cookie no encontrada.");
                return;
            }

            string idEmpleadoValue = ConseguirCookie(cookie.Value, "idEmpleado");
            if (idEmpleadoValue == null || !int.TryParse(idEmpleadoValue, out int idEmpleado))
            {
                System.Diagnostics.Debug.WriteLine("Error: ID de empleado no válido.");
                return;
            }

            bool insercionExitosa = InsertarSolicitudes(fechaInicioTexto, fechaFinalTexto, idEmpleado);

            if (insercionExitosa == true)
            {
                ActualizarDiasDeVacaciones(idEmpleado, diasSolicitados);
                vacacionesCountLabel.Text = (diasDisponibles - diasSolicitados).ToString();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Error: La inserción de solicitudes no fue exitosa.");
            }

        }

        private bool InsertarSolicitudes(DateTime fechaInicio, DateTime fechaFinal, int idEmpleado)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            bool insercionExitosa = false;
            DateTime fechaActual = DateTime.Now;
            string fechaPublicada = fechaActual.ToString("yyyy-MM-dd");
            string Estado = "Pendiente";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO SolicitudVacaciones (FechaPublicada,FechaVacacion, DiasDisfrutados, Estado, idEmpleado) VALUES (@FechaPublicada,@FechaVacacion,@DiasDisfrutados,@Estado, @idEmpleado)";
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();

                    for (DateTime fecha = fechaInicio; fecha <= fechaFinal; fecha = fecha.AddDays(1))
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@FechaPublicada", fechaPublicada);
                        command.Parameters.AddWithValue("@FechaVacacion", fecha);
                        command.Parameters.AddWithValue("@DiasDisfrutados", 1); // Cada día cuenta como 1 día disfrutado
                        command.Parameters.AddWithValue("@Estado", Estado);
                        command.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                        command.ExecuteNonQuery();
                    }
                    insercionExitosa = true;
                    System.Diagnostics.Debug.WriteLine("Solicitudes de vacaciones insertadas con éxito.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
            return insercionExitosa;

        }

        private void ActualizarDiasDeVacaciones(int idEmpleado, int diasSolicitados)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "UPDATE Empleado SET DiasDeVacaciones = DiasDeVacaciones - @DiasSolicitados WHERE idEmpleado = @idEmpleado";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@DiasSolicitados", diasSolicitados);
                command.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Días de vacaciones actualizados con éxito.");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Error: No se actualizó ningún registro.");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        private bool HayFechasNoDisponibles(DateTime fechaInicio, DateTime fechaFinal)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            bool hayFechasNoDisponibles = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM DiasNoDisponibles WHERE Fecha BETWEEN @FechaInicio AND @FechaFinal";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                command.Parameters.AddWithValue("@FechaFinal", fechaFinal);

                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                    {
                        hayFechasNoDisponibles = true;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }

            return hayFechasNoDisponibles;
        }



    }

}