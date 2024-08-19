using ControlEmpresarial.Controlador;
using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Net.Mail;
using Newtonsoft.Json;
using Mysqlx.Cursor;

namespace ControlEmpresarial.Vistas
{
    public partial class MenuColaborador : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            if (userCookie != null)
            {
                int idEmpleado = int.Parse(userCookie["idEmpleado"]);
                int idDepartamento = int.Parse(userCookie["idDepartamento"]);
                DateTime fechaActual = DateTime.Today;
                VerificarInconsistencia(idEmpleado, fechaActual);
                VerificarInconsistenciasActivas(idEmpleado, fechaActual);
                VerificarFalta(idEmpleado, idDepartamento, fechaActual);
                VerificarSalidaTemprana(idEmpleado, fechaActual);
            }
            else
            {
                // Manejar el caso en el que la cookie no existe
                lblError.Text = "No se ha encontrado la cookie de usuario.";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void VerificarSalidaTemprana(int empleadoId, DateTime fecha)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            int idTipoInconsistencia = 6; // Suponiendo que 6 es el id para salida temprana
            string Detalle = "Salida Temprana";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
            SELECT COUNT(*) AS Inconsistencias
            FROM Entradas e
            JOIN Empleado emp ON e.idEmpleado = emp.idEmpleado
            JOIN horario h ON emp.idHorario = h.idHorario
            WHERE e.idEmpleado = @EmpleadoId
              AND e.DiaMarcado = @Fecha
              AND e.HoraSalida < h.horaSalida
              AND FIND_IN_SET(
                  CASE 
                      WHEN DAYNAME(@Fecha) = 'Monday' THEN 'Lunes'
                      WHEN DAYNAME(@Fecha) = 'Tuesday' THEN 'Martes'
                      WHEN DAYNAME(@Fecha) = 'Wednesday' THEN 'Miércoles'
                      WHEN DAYNAME(@Fecha) = 'Thursday' THEN 'Jueves'
                      WHEN DAYNAME(@Fecha) = 'Friday' THEN 'Viernes'
                      WHEN DAYNAME(@Fecha) = 'Saturday' THEN 'Sábado'
                      WHEN DAYNAME(@Fecha) = 'Sunday' THEN 'Domingo'
                  END, 
                  h.diaSemana
              ) > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmpleadoId", empleadoId);
                    command.Parameters.AddWithValue("@Fecha", fecha.ToString("yyyy-MM-dd")); // Formato de fecha para MySQL

                    try
                    {
                        connection.Open();
                        int inconsistencias = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();

                        // Actualizar la etiqueta en la página
                        lblInconsistencias.Text = $"Inconsistencias: {inconsistencias}";

                        if (inconsistencias > 0)
                        {
                            // Verificar si ya existe una inconsistencia del mismo tipo en la fecha
                            if (!ExisteInconsistencia(idTipoInconsistencia, fecha, empleadoId))
                            {
                                // Insertar una nueva inconsistencia
                                InsertarInconsistencia(idTipoInconsistencia, fecha, empleadoId, Detalle);
                            }
                            else
                            {
                                lblInconsistencias.Text += " - Ya existe una inconsistencia de este tipo para el día de hoy.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Mostrar el error en la etiqueta
                        lblError.Text = $"Error al verificar inconsistencias: {ex.Message}";
                    }
                }
            }
        }

        private void VerificarFalta(int idEmpleado, int idDepartamento, DateTime fecha)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            string Detalle = "Falta";
            int idTipoInconsistencia = 5;

            // Verificar si es un día festivo, laboral, o si el empleado tiene permiso o está en vacaciones colectivas
            if (!EsDiaFestivo(idDepartamento) && EsDiaLaboral(idEmpleado) && !HayPermiso(idEmpleado) && !EsVacacionColectiva())
            {
                using (MySqlConnection conexion = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conexion.Open();

                        // Verificar si hay un registro de entrada para el empleado en el día de hoy
                        string query = @"
                    SELECT COUNT(*)
                    FROM entradas
                    WHERE idEmpleado = @idEmpleado
                    AND DiaMarcado = CURDATE()";

                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                            int count = Convert.ToInt32(cmd.ExecuteScalar());

                            // Si no hay registro de entrada, verificar la existencia de la inconsistencia
                            if (count == 0)
                            {
                                // Verificar si ya existe una inconsistencia del mismo tipo en la fecha
                                if (!ExisteInconsistencia(idTipoInconsistencia, fecha, idEmpleado))
                                {
                                    // Insertar una nueva inconsistencia y enviar el correo
                                    InsertarInconsistencia(idTipoInconsistencia, fecha, idEmpleado, Detalle);
                                    EnviarCorreoInconsistencia(Detalle);
                                }
                                else
                                {
                                    // La inconsistencia ya existe, puedes notificar esto en la etiqueta
                                    lblInconsistencias.Text = "Ya existe una inconsistencia de tipo 'Falta' para el día de hoy.";
                                }
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error en la base de datos: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error inesperado: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    }
                }
            }
        }


        private void VerificarInconsistencia(int empleadoId, DateTime fecha)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            int idTipoInconsistencia = 4;
            string Detalle = "Llegada Tardía";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
            SELECT COUNT(*) AS Inconsistencias
            FROM Entradas e
            JOIN Empleado emp ON e.idEmpleado = emp.idEmpleado
            JOIN horario h ON emp.idHorario = h.idHorario
            WHERE e.idEmpleado = @EmpleadoId
              AND e.DiaMarcado = @Fecha
              AND e.HoraEntrada > h.horaEntrada
              AND FIND_IN_SET(
                  CASE 
                      WHEN DAYNAME(@Fecha) = 'Monday' THEN 'Lunes'
                      WHEN DAYNAME(@Fecha) = 'Tuesday' THEN 'Martes'
                      WHEN DAYNAME(@Fecha) = 'Wednesday' THEN 'Miércoles'
                      WHEN DAYNAME(@Fecha) = 'Thursday' THEN 'Jueves'
                      WHEN DAYNAME(@Fecha) = 'Friday' THEN 'Viernes'
                      WHEN DAYNAME(@Fecha) = 'Saturday' THEN 'Sábado'
                      WHEN DAYNAME(@Fecha) = 'Sunday' THEN 'Domingo'
                  END, 
                  h.diaSemana
              ) > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmpleadoId", empleadoId);
                    command.Parameters.AddWithValue("@Fecha", fecha.ToString("yyyy-MM-dd")); // Formato de fecha para MySQL

                    try
                    {
                        connection.Open();
                        int inconsistencias = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();

                        // Actualizar la etiqueta en la página
                        lblInconsistencias.Text = $"Inconsistencias: {inconsistencias}";

                        if (inconsistencias > 0)
                        {
                            // Verificar si ya existe una inconsistencia del mismo tipo en la fecha
                            if (!ExisteInconsistencia(idTipoInconsistencia, fecha, empleadoId))
                            {
                                // Insertar una nueva inconsistencia
                                InsertarInconsistencia(idTipoInconsistencia, fecha, empleadoId, Detalle);
                            }
                            else
                            {
                                lblInconsistencias.Text += " - Ya existe una inconsistencia de este tipo para el día de hoy.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Mostrar el error en la etiqueta
                        lblError.Text = $"Error al verificar inconsistencias: {ex.Message}";
                    }
                }
            }
        }

        private bool ExisteInconsistencia(int idTipoInconsistencia, DateTime fecha, int empleadoId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
            SELECT COUNT(*) 
            FROM inconsistencias
            WHERE idTipoInconsistencia = @IdTipoInconsistencia
              AND Fecha = @Fecha
              AND idEmpleado = @IdEmpleado";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdTipoInconsistencia", idTipoInconsistencia);
                    command.Parameters.AddWithValue("@Fecha", fecha.ToString("yyyy-MM-dd")); // Formato de fecha para MySQL
                    command.Parameters.AddWithValue("@IdEmpleado", empleadoId);

                    try
                    {
                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();

                        return count > 0;
                    }
                    catch (Exception ex)
                    {
                        // Mostrar el error en la etiqueta
                        lblError.Text = $"Error al verificar la existencia de la inconsistencia: {ex.Message}";
                        return false;
                    }
                }
            }
        }

        private void InsertarInconsistencia(int idTipoInconsistencia, DateTime fecha, int empleadoId,string Detalle)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
            INSERT INTO inconsistencias (idTipoInconsistencia, Fecha, Estado, idEmpleado)
            VALUES (@IdTipoInconsistencia, @Fecha, @Estado, @IdEmpleado)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdTipoInconsistencia", idTipoInconsistencia);
                    command.Parameters.AddWithValue("@Fecha", fecha.ToString("yyyy-MM-dd")); // Formato de fecha para MySQL
                    command.Parameters.AddWithValue("@Estado", "Activo");
                    command.Parameters.AddWithValue("@IdEmpleado", empleadoId);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                        // Actualizar la etiqueta en la página
                        lblInconsistencias.Text += " - Inconsistencia registrada correctamente.";

                        // Enviar correo
                        EnviarCorreoInconsistencia(Detalle);
                    }
                    catch (Exception ex)
                    {
                        // Mostrar el error en la etiqueta
                        lblError.Text += $" - Error al registrar la inconsistencia: {ex.Message}";
                    }
                }
            }
        }

        private void EnviarCorreoInconsistencia(string Detalle)
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            if (userCookie != null)
            {
                string correo = userCookie["Correo"];

                if (!string.IsNullOrEmpty(correo))
                {
                    string fromEmail = "apsw.activity.sync@gmail.com";
                    string fromPassword = "hpehyzvcvdfcatgn";
                    string subject = "Inconsistencia registrada";
                    string body = $"Estimado Usuario,\n\nSe ha registrado una inconsistencia en el sistema el día {DateTime.Today.ToString("dd/MM/yyyy")}.\n\nDetalles: {Detalle}.\n\nSaludos,\nEl equipo de ActivitySync";

                    try
                    {
                        var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
                        {
                            Credentials = new System.Net.NetworkCredential(fromEmail, fromPassword),
                            EnableSsl = true
                        };

                        smtpClient.Send(fromEmail, correo, subject, body);
                        lblInconsistencias.Text += " - Correo de notificación enviado.";
                    }
                    catch (Exception ex)
                    {
                        lblError.Text += $" - Error al enviar el correo: {ex.Message}";
                    }
                }
                else
                {
                    lblError.Text += " - Correo del usuario no encontrado en la cookie.";
                }
            }
            else
            {
                lblError.Text += " - Cookie de usuario no encontrada.";
            }
        }
        private void VerificarInconsistenciasActivas(int empleadoId, DateTime fecha)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
        SELECT idInconsistencia
        FROM inconsistencias
        WHERE idEmpleado = @EmpleadoId
          AND Estado = 'Activo'
          AND DATEDIFF(CURDATE(), Fecha) > 3";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmpleadoId", empleadoId);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int idInconsistencia = reader.GetInt32("idInconsistencia");
                                if (!ExisteRebajo(idInconsistencia, empleadoId))
                                {
                                    AplicarRebajo(empleadoId, fecha, idInconsistencia);
                                }
                                else
                                {
                                    lblInconsistencias.Text += $" - Ya existe un rebajo para la inconsistencia ID {idInconsistencia}.";
                                }
                            }
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = $"Error al verificar las inconsistencias activas: {ex.Message}";
                    }
                }
            }
        }



        private bool ExisteRebajo(int idInconsistencia, int empleadoId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
        SELECT COUNT(*)
        FROM rebajo
        WHERE idEmpleado = @EmpleadoId
          AND idInconsistencia = @IdInconsistencia";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmpleadoId", empleadoId);
                    command.Parameters.AddWithValue("@IdInconsistencia", idInconsistencia);

                    try
                    {
                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();

                        return count > 0;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = $"Error al verificar la existencia de rebajo: {ex.Message}";
                        return false;
                    }
                }
            }
        }



        private void AplicarRebajo(int empleadoId, DateTime fecha, int idInconsistencia)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
                INSERT INTO rebajo (idEmpleado, TipoRebajoID, Fecha, Estado, Motivo, Monto, idInconsistencia)
                VALUES (@IdEmpleado, @TipoRebajoID, @Fecha, @Estado, @Motivo, @Monto, @idInconsistencia)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    int idTipoRebajo = 1; // Tipo de rebajo
                    double monto = 5.0; // Porcentaje o cantidad del rebajo

                    command.Parameters.AddWithValue("@IdEmpleado", empleadoId);
                    command.Parameters.AddWithValue("@TipoRebajoID", idTipoRebajo);
                    command.Parameters.AddWithValue("@Fecha", fecha.ToString("yyyy-MM-dd")); // Formato de fecha para MySQL
                    command.Parameters.AddWithValue("@Estado", "Pendiente");
                    command.Parameters.AddWithValue("@Motivo", $"Inconsistencia con ID {idInconsistencia} no justificada en más de 3 días.");
                    command.Parameters.AddWithValue("@Monto", monto);
                    command.Parameters.AddWithValue("@idInconsistencia", idInconsistencia);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                        lblInconsistencias.Text += $" - Rebajo aplicado por inconsistencia ID {idInconsistencia}.";

                        // Enviar correo
                        EnviarCorreoRebajo();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text += $" - Error al aplicar el rebajo: {ex.Message}";
                    }
                }
            }
        }
        private bool EsDiaFestivo(int idDepartamento)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT COUNT(*) FROM diasfestivos WHERE FechaVacacion = @FechaHoy AND idDepartamento = @idDepartamento";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@FechaHoy", DateTime.Today);
                        cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (MySqlException ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error en la base de datos: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error inesperado: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
            }
        }

        private bool EsDiaLaboral(int idEmpleado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            var diasSemana = new Dictionary<DayOfWeek, string>
    {
        { DayOfWeek.Monday, "Lunes" },
        { DayOfWeek.Tuesday, "Martes" },
        { DayOfWeek.Wednesday, "Miercoles" },
        { DayOfWeek.Thursday, "Jueves" },
        { DayOfWeek.Friday, "Viernes" },
        { DayOfWeek.Saturday, "Sabado" },
        { DayOfWeek.Sunday, "Domingo" }
    };

            string diaActual = diasSemana[DateTime.Today.DayOfWeek];

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();
                    string query = @"
            SELECT COUNT(*) 
            FROM Horario H
            INNER JOIN Empleado E ON E.idHorario = H.idHorario
            WHERE E.idEmpleado = @idEmpleado
            AND FIND_IN_SET(@diaActual, H.diaSemana) > 0";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                        cmd.Parameters.AddWithValue("@diaActual", diaActual);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error al verificar días laborales: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
            }
        }

        private bool HayPermiso(int idEmpleado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();
                    string query = @"
            SELECT COUNT(*)
            FROM solicitudpermiso
            WHERE idEmpleado = @idEmpleado
            AND @FechaHoy BETWEEN FechaDeseadaInicial AND FechaDeseadaFinal
            AND Estado = 'Aceptada'";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                        cmd.Parameters.AddWithValue("@FechaHoy", DateTime.Today);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (MySqlException ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error en la base de datos: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error inesperado: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
            }
        }
        private bool EsVacacionColectiva()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT COUNT(*) FROM vacacionescolectivas WHERE FechaVacacion = @FechaHoy";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@FechaHoy", DateTime.Today);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (MySqlException ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error en la base de datos: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error inesperado: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
            }
        }

        private void EnviarCorreoRebajo()
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            if (userCookie != null)
            {
                string correo = userCookie["Correo"];

                if (!string.IsNullOrEmpty(correo))
                {
                    string fromEmail = "apsw.activity.sync@gmail.com";
                    string fromPassword = "hpehyzvcvdfcatgn";
                    string subject = "Rebajo Aplicado";
                    string body = $"Estimado Usuario,\n\nSe ha aplicado un rebajo en el sistema el día {DateTime.Today.ToString("dd/MM/yyyy")} debido a inconsistencias no justificadas.\n\nSaludos,\nEl equipo de ActivitySync";

                    try
                    {
                        var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
                        {
                            Credentials = new System.Net.NetworkCredential(fromEmail, fromPassword),
                            EnableSsl = true
                        };

                        smtpClient.Send(fromEmail, correo, subject, body);
                        lblInconsistencias.Text += " - Correo de notificación del rebajo enviado.";
                    }
                    catch (Exception ex)
                    {
                        lblError.Text += $" - Error al enviar el correo del rebajo: {ex.Message}";
                    }
                }
                else
                {
                    lblError.Text += " - Correo del usuario no encontrado en la cookie.";
                }
            }
            else
            {
                lblError.Text += " - Cookie de usuario no encontrada.";
            }
        }
    }
}



