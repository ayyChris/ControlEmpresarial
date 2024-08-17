using System;
using System.Data;
using System.Net.Mail;
using System.Net;
using MySql.Data.MySqlClient;

namespace ControlEmpresarial.Vistas.Inconsistencias
{
    public partial class AceptacionJustificacionInconsistenciaJefe : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["id"], out int idJustificacion))
                {
                    CargarDatos(idJustificacion);
                }
                else
                {
                    Label1.Text = "ID de justificación no válido.";
                    Label1.Visible = true;
                }
            }
        }


        private void CargarDatos(int idJustificacion)
        {
            string query = @"
                SELECT 
                    e.Nombre AS NombreEmpleado,
                    ji.Justificacion AS Descripcion,
                    ti.Nombre AS TipoInconsistencia,
                    ji.FechaJustificacion AS Fecha
                FROM 
                    justificacioninconsistencia ji
                JOIN 
                    inconsistencias i ON ji.idInconsistencia = i.idInconsistencia
                JOIN 
                    empleado e ON i.idEmpleado = e.idEmpleado
                JOIN 
                    tiposinconsistencia ti ON i.idTipoInconsistencia = ti.idTipoInconsistencia
                WHERE 
                    ji.idJustificacion = @idJustificacion";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idJustificacion", idJustificacion);
                    try
                    {
                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblNombreEmpleado.Text = reader["NombreEmpleado"].ToString();

                                lblTipoInconsistencia.Text = $"<strong>{reader["TipoInconsistencia"].ToString()}</strong>";

                                lblFechaJustificacion.Text = $"Fecha de Justificación: {Convert.ToDateTime(reader["Fecha"]).ToString("yyyy-MM-dd")}";
                                lblDescripcion.Text = $"<strong>Justificación:</strong> {reader["Descripcion"].ToString()}";
                            }
                            else
                            {
                                Label1.Text = "No se encontraron datos para esta justificación.";
                                Label1.Visible = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Label1.Text = "Error al cargar los datos: " + ex.Message;
                        Label1.Visible = true;
                    }
                }
            }
        }

        private void MostrarMensaje(bool exito, string mensaje)
        {
            if (exito)
            {
                lblSuccessMessage.Text = mensaje;
                lblSuccessMessage.Visible = true;
                lblErrorMessage.Visible = false;
            }
            else
            {
                lblErrorMessage.Text = mensaje;
                lblErrorMessage.Visible = true;
                lblSuccessMessage.Visible = false;
            }
        }

        protected void AceptarButton_Click(object sender, EventArgs e)
        {
            int idJustificacion = Convert.ToInt32(Request.QueryString["id"]);
            ActualizarEstadoJustificacion(idJustificacion, "Aceptado");

            string correoEmpleado = ObtenerCorreoEmpleado(idJustificacion);
            if (!string.IsNullOrEmpty(correoEmpleado))
            {
                EnviarCorreoJustificacion(true, correoEmpleado, lblNombreEmpleado.Text, lblTipoInconsistencia.Text, Convert.ToDateTime(lblFechaJustificacion.Text.Replace("Fecha de Justificación: ", "")), lblDescripcion.Text);
                MostrarMensaje(true, "Justificación aceptada y correo enviado correctamente.");
            }
            else
            {
                MostrarMensaje(false, "Error al obtener el correo del empleado.");
            }
        }

        protected void DenegarButton_Click(object sender, EventArgs e)
        {
            int idJustificacion = Convert.ToInt32(Request.QueryString["id"]);
            ActualizarEstadoJustificacion(idJustificacion, "Denegado");

            string correoEmpleado = ObtenerCorreoEmpleado(idJustificacion);
            if (!string.IsNullOrEmpty(correoEmpleado))
            {
                EnviarCorreoJustificacion(false, correoEmpleado, lblNombreEmpleado.Text, lblTipoInconsistencia.Text, Convert.ToDateTime(lblFechaJustificacion.Text.Replace("Fecha de Justificación: ", "")), lblDescripcion.Text);
                MostrarMensaje(true, "Justificación denegada y correo enviado correctamente.");
            }
            else
            {
                MostrarMensaje(false, "Error al obtener el correo del empleado.");
            }
        }

        private void ActualizarEstadoJustificacion(int idJustificacion, string nuevoEstado)
        {
            string query = "UPDATE justificacioninconsistencia SET Estado = @nuevoEstado WHERE idJustificacion = @idJustificacion";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@idJustificacion", idJustificacion);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MostrarMensaje(true, "Estado actualizado a " + nuevoEstado);
                        // Redirigir o actualizar la página si es necesario
                    }
                    catch (Exception ex)
                    {
                        MostrarMensaje(false, "Error al actualizar el estado: " + ex.Message);
                    }
                }
            }
        }


        private string ObtenerCorreoEmpleado(int idJustificacion)
        {
            string correoEmpleado = string.Empty;
            string query = @"
        SELECT e.Correo
        FROM justificacioninconsistencia ji
        JOIN inconsistencias i ON ji.idInconsistencia = i.idInconsistencia
        JOIN empleado e ON i.idEmpleado = e.idEmpleado
        WHERE ji.idJustificacion = @idJustificacion";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idJustificacion", idJustificacion);
                    try
                    {
                        conn.Open();
                        correoEmpleado = cmd.ExecuteScalar()?.ToString();
                    }
                    catch (Exception ex)
                    {
                        Label1.Text = "Error al obtener el correo del empleado: " + ex.Message;
                        Label1.Visible = true;
                    }
                }
            }
            return correoEmpleado;
        }

        private void EnviarCorreoJustificacion(bool justificacionAceptada, string destinatarioEmail, string nombreEmpleado, string tipoInconsistencia, DateTime fechaJustificacion, string descripcion)
        {
            string fromEmail = "apsw.activity.sync@gmail.com";
            string fromPassword = "hpehyzvcvdfcatgn";
            string subject = justificacionAceptada ? "Justificación Aceptada" : "Justificación Denegada";
            string body = $@"
    <html>
    <body>
        <p>Estimado/a {nombreEmpleado},</p>
        <p>La justificación con los siguientes detalles ha sido {(justificacionAceptada ? "aceptada" : "denegada")}:</p>
        <p><strong>Tipo de Inconsistencia:</strong> {tipoInconsistencia}</p>
        <p><strong>Fecha de Justificación:</strong> {fechaJustificacion:yyyy-MM-dd}</p>
        <p><strong>Justificación:</strong> {descripcion}</p>
        <p>Atentamente,<br />El Equipo de Actividades</p>
    </body>
    </html>";

            try
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(fromEmail, "Equipo de Actividades"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(destinatarioEmail);

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    EnableSsl = true
                };

                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                // Manejo de errores (opcional)
                // Puedes registrar el error en un archivo o base de datos
                Label1.Text = "Error al enviar el correo: " + ex.Message;
                Label1.Visible = true;
            }
        }

        
    }
}