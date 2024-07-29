using MySql.Data.MySqlClient;
using ControlEmpresarial.Services;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using ControlEmpresarial.Controlador;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Horas_Extra
{
    public partial class RespuestaHorasExtra : Page
    {
        private NotificacionService notificacionService = new NotificacionService();
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        private string idSolicitud;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarNotificaciones();
                CargarNombreUsuario();
                idSolicitud = Request.QueryString["idSolicitud"];
                if (!string.IsNullOrEmpty(idSolicitud))
                {
                    lblSolicitudId.Text = "ID de Solicitud: " + idSolicitud;
                    Session["idSolicitud"] = idSolicitud; // Almacenar en sesión
                    CargarDatos();
                }
                else
                {
                    lblSolicitudId.Text = "Error: idSolicitud no proporcionado";
                }
            }
            else
            {
                idSolicitud = Session["idSolicitud"] as string; // Recuperar de sesión
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
                    // Manejar caso en el que idEmpleado no es válido
                    Label1.Text = "Error al extraer ID de empleado";
                    Label1.Visible = true;
                }
            }
            else
            {
                Label1.Text = "Cookie no encontrada";
                Label1.Visible = true;
            }
        }

        protected void AceptarButton_Click(object sender, EventArgs e)
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            if (userCookie != null)
            {
                int idEmpleado;
                if (int.TryParse(userCookie["idEmpleado"], out idEmpleado))
                {
                    DateTime fechaRespuesta = DateTime.Now;
                    bool respuesta = true;

                    try
                    {
                        idSolicitud = Session["idSolicitud"] as string; // Obtener de sesión

                        // Verificar si idSolicitud no es null antes de pasarla al método
                        if (!string.IsNullOrEmpty(idSolicitud))
                        {
                            string actividad = "Activo";
                            InsertarRespuesta(idSolicitud, idEmpleado, fechaRespuesta, respuesta, actividad);
                            ActualizarEstadoSolicitud(idSolicitud, "Inactivo");
                            AceptarButton.Text = "Aceptada";

                            // Obtener el id del empleado que hizo la solicitud
                            int idSolicitante = ObtenerIdEmpleadoSolicitud(idSolicitud);

                            // Enviar notificación al empleado que hizo la solicitud
                            NotificacionService notificacionService = new NotificacionService();
                            string titulo = "Solicitud de Hora Extra Aceptada";
                            string motivo = $"Tu solicitud de hora extra ha sido aceptada.";
                            EnviarNotificacion(idSolicitante, idEmpleado, titulo, motivo);

                        }
                        else
                        {
                            AceptarButton.Text = "Error: idSolicitud es null";
                        }
                    }
                    catch (Exception ex)
                    {
                        AceptarButton.Text = "Error: " + ex.Message; // Depuración de errores
                    }
                }
                else
                {
                    AceptarButton.Text = "Error: idEmpleado inválido"; // Depuración de errores
                }
            }
            else
            {
                AceptarButton.Text = "Error: No user cookie"; // Depuración de errores
            }
        }

        protected void DenegarButton_Click(object sender, EventArgs e)
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            if (userCookie != null)
            {
                int idEmpleado;
                if (int.TryParse(userCookie["idEmpleado"], out idEmpleado))
                {
                    DateTime fechaRespuesta = DateTime.Now;
                    bool respuesta = false;

                    try
                    {
                        idSolicitud = Session["idSolicitud"] as string; // Obtener de sesión

                        // Verificar si idSolicitud no es null antes de pasarla al método
                        if (!string.IsNullOrEmpty(idSolicitud))
                        {
                            string actividad = "Inactivo";
                            InsertarRespuesta(idSolicitud, idEmpleado, fechaRespuesta, respuesta, actividad);
                            ActualizarEstadoSolicitud(idSolicitud, "Inactivo");
                            DenegarButton.Text = "Denegada";

                            // Obtener el id del empleado que hizo la solicitud
                            int idSolicitante = ObtenerIdEmpleadoSolicitud(idSolicitud);

                            // Enviar notificación al empleado que hizo la solicitud
                            NotificacionService notificacionService = new NotificacionService();
                            string titulo = "Solicitud de Hora Extra Denegada";
                            string motivo = $"Tu solicitud de hora extra ha sido denegada.";
                            EnviarNotificacion(idSolicitante, idEmpleado, titulo, motivo);

                        }
                        else
                        {
                            DenegarButton.Text = "Error: idSolicitud es null";
                        }
                    }
                    catch (Exception ex)
                    {
                        DenegarButton.Text = "Error: " + ex.Message; // Depuración de errores
                    }
                }
                else
                {
                    DenegarButton.Text = "Error: idEmpleado inválido"; // Depuración de errores
                }
            }
            else
            {
                DenegarButton.Text = "Error: No user cookie"; // Depuración de errores
            }
        }

        private void InsertarRespuesta(string idSolicitud, int idEmpleado, DateTime fechaRespuesta, bool respuesta, string actividad)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "INSERT INTO respuestahorasextras (idSolicitud, idEmpleado, FechaRespuesta, Respuesta, Actividad) " +
                                   "VALUES (@idSolicitud, @idEmpleado, @FechaRespuesta, @Respuesta, @Actividad)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idSolicitud", idSolicitud);
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@FechaRespuesta", fechaRespuesta);
                    cmd.Parameters.AddWithValue("@Respuesta", respuesta);
                    cmd.Parameters.AddWithValue("@Actividad", actividad);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al insertar respuesta: " + ex.Message);
            }
        }

        private void ActualizarEstadoSolicitud(string idSolicitud, string estado)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "UPDATE solicitudhorasextras SET Estado = @Estado WHERE idSolicitud = @idSolicitud";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    cmd.Parameters.AddWithValue("@idSolicitud", idSolicitud);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al actualizar estado de solicitud: " + ex.Message);
            }
        }

        private void CargarDatos()
        {
            DataTable dt = ObtenerDatosBD();
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                lblHorasSolicitadas.Text = row["HorasSolicitadas"].ToString();
                lblMotivo.Text =  row["Motivo"].ToString();
                lblHoraInicialExtra.Text = row["HoraInicialExtra"].ToString();
                lblHoraFinalExtra.Text = row["HoraFinalExtra"].ToString();
            }
        }

        private DataTable ObtenerDatosBD()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT HorasSolicitadas, Motivo, HoraInicialExtra, HoraFinalExtra " +
                               "FROM solicitudhorasextras " +
                               "WHERE idSolicitud = @idSolicitud";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idSolicitud", idSolicitud);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        private int ObtenerIdEmpleadoSolicitud(string idSolicitud)
        {
            int idEmpleado = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idEnviador FROM solicitudhorasextras WHERE idSolicitud = @idSolicitud";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idSolicitud", idSolicitud);
                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out idEmpleado))
                {
                    return idEmpleado;
                }
            }
            return idEmpleado;
        }

        private void EnviarNotificacion(int idRecibidor, int idEnviador, string titulo, string motivo)
        {
            NotificacionService notificacionService = new NotificacionService();
            notificacionService.InsertarNotificacion(idEnviador, idRecibidor, titulo, motivo, DateTime.Now);
        }
    }
}
