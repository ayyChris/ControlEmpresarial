using ControlEmpresarial.Controlador;
using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Horas_Extra
{
    public partial class EvidenciaHorasExtra : System.Web.UI.Page
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos();
                CargarNotificaciones();
                CargarNombreUsuario();
            }
        }

        private void CargarNombreUsuario()
        {
            // Obtener el nombre de las cookies
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null)
            {
                string nombre = cookie["Nombre"];
                string apellidos = cookie["Apellidos"];
                lblNombre.Text = nombre + " " + apellidos;
                lblNombre.Visible = true;
            }
            else
            {
                lblNombre.Text = "Error";
                lblNombre.Visible = true;
            }
        }

        private void CargarNotificaciones()
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null)
            {
                // Intentar extraer el idEmpleado de la cookie
                if (int.TryParse(cookie["idEmpleado"], out int idEmpleado))
                {
                    // Obtener las notificaciones usando el idEmpleado extraído
                    NotificacionService service = new NotificacionService();
                    List<Notificacion> notificaciones = service.ObtenerNotificaciones(idEmpleado);

                    // Enlazar los datos al repeater
                    repeaterNotificaciones.DataSource = notificaciones;
                    repeaterNotificaciones.DataBind();
                }
                else
                {
                 
                }
            }
            else
            {
               
            }
        }


        protected void submit_Click(object sender, EventArgs e)
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            if (userCookie != null)
            {
                int idEmpleado = int.Parse(userCookie["idEmpleado"]);
                string idSolicitud = colaborador.SelectedValue;
                string evidencia = Evidencia.Text.Trim();

                if (!string.IsNullOrEmpty(idSolicitud) && !string.IsNullOrEmpty(evidencia))
                {
                    bool datosInsertados = InsertarDatos(idEmpleado, idSolicitud, evidencia);

                    if (datosInsertados)
                    {
                        // Llamar al método para actualizar la columna Actividad
                        bool actividadActualizada = ActualizarActividad(idEmpleado);

                        if (actividadActualizada)
                        {
                            // Llamar al método para obtener el ID del empleado que asignó la hora extra
                            int idEnviador = ObtenerIdEnviador(idSolicitud);

                            if (idEnviador > 0)
                            {
                                // Enviar notificación al empleado que asignó la hora extra
                                NotificacionService notificacionService = new NotificacionService();
                                notificacionService.InsertarNotificacion(idEnviador, idEmpleado, "Evidencia de Hora Extra Enviada",
                                    $"Se ha enviado evidencia de la hora extra para la solicitud {idSolicitud}.", DateTime.Now);
                            }

                            lblMensaje.Text = "Se hizo la evidencia.";
                            lblMensaje.CssClass = "mensaje-exito";
                            lblMensaje.Visible = true;
                        }
                        else
                        {
                            lblMensaje.Text = "Datos insertados correctamente, pero hubo un error al actualizar la actividad.";
                            lblMensaje.CssClass = "mensaje-error";
                            lblMensaje.Visible = true;
                        }
                    }
                    else
                    {
                        // El mensaje de error ya se establece en el método InsertarDatos
                        // No es necesario establecerlo aquí
                    }
                }
                else
                {
                    lblMensaje.Text = "Por favor, complete todos los campos.";
                    lblMensaje.CssClass = "mensaje-error"; // Estiliza el mensaje de error
                    lblMensaje.Visible = true;
                }
            }
            else
            {
                lblMensaje.Text = "No se encontró información del usuario. Por favor, inicie sesión nuevamente.";
                lblMensaje.CssClass = "mensaje-error"; // Estiliza el mensaje de error
                lblMensaje.Visible = true;
            }
        }

        private int ObtenerIdEnviador(string idSolicitud)
        {
            int idEnviador = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idEnviador FROM solicitudhorasextras WHERE idSolicitud = @idSolicitud";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idSolicitud", idSolicitud);

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    idEnviador = Convert.ToInt32(result);
                }
            }
            return idEnviador;
        }

        private bool ActualizarActividad(int idEmpleado)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "UPDATE respuestahorasextras SET Actividad = 'Inactivo' " +
                               "WHERE idEmpleado = @idEmpleado AND Actividad = 'Activo'";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                    try
                    {
                        conn.Open();
                        int rowsUpdated = cmd.ExecuteNonQuery();
                        return rowsUpdated > 0;
                    }
                    catch (Exception ex)
                    {
                        lblMensaje.Text = ("Error: " + ex.Message);
                        lblMensaje.CssClass = "mensaje-error"; // Estiliza el mensaje de error
                        lblMensaje.Visible = true;
                        return false;
                    }
                }
            }
        }

        private bool InsertarDatos(int idEmpleado, string idSolicitud, string evidencia)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO evidenciahorasextras (idSolicitud, idEmpleado, EnlaceEvidencia, FechaEvidencia, Estado) " +
                               "VALUES (@idSolicitud, @idEmpleado, @enlaceEvidencia, @fechaEvidencia, @estado)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idSolicitud", idSolicitud);
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@enlaceEvidencia", evidencia);
                    cmd.Parameters.AddWithValue("@fechaEvidencia", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@estado", "Evidenciada");

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        lblMensaje.Text = "Error al insertar los datos: " + ex.Message;
                        lblMensaje.CssClass = "mensaje-error"; // Estiliza el mensaje de error
                        lblMensaje.Visible = true;

                        return false;
                    }
                }
            }
        }

        private void CargarDatos()
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            if (userCookie != null)
            {
                int idEmpleado = int.Parse(userCookie["idEmpleado"]);
                DataTable dt = ObtenerDatos(idEmpleado);

                colaborador.Items.Clear();

                colaborador.Items.Add(new ListItem("Seleccione", ""));

                foreach (DataRow row in dt.Rows)
                {
                    colaborador.Items.Add(new ListItem(row["idSolicitud"].ToString(), row["idSolicitud"].ToString()));
                }

                lblMensaje.Visible = false;
            }
            else
            {
                lblMensaje.Text = "No se encontró información del usuario. Por favor, inicie sesión nuevamente.";
                lblMensaje.Visible = true;
            }
        }

        private DataTable ObtenerDatos(int idEmpleado)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idSolicitud " +
                               "FROM respuestahorasextras " +
                               "WHERE idEmpleado = @idEmpleado and Actividad = 'Activo' ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
