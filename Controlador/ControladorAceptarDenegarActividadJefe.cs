using ControlEmpresarial.Controlador;
using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Control_de_Actividades
{
    public partial class NegacionAceptacionActividadJefe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarNombreUsuario();
                CargarNotificaciones();
                CargarActividad(); // Asegúrate de que también llames a este método
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

        private void CargarActividad()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            // Obtener el idActividad de la query string
            string idActividadString = Request.QueryString["evidencia"];
            if (int.TryParse(idActividadString, out int idActividad))
            {
                string query = @"
                    SELECT a.idActividad AS Evidencia, ar.Titulo, a.Descripcion, a.FechaInicio, a.FechaFin, e.Nombre
                    FROM actividades a
                    INNER JOIN empleado e ON a.idEmpleado = e.idEmpleado
                    INNER JOIN actividadesregistradas ar ON a.idActividadRegistrada = ar.id
                    WHERE a.idActividad = @idActividad";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idActividad", idActividad);

                        try
                        {
                            connection.Open();
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Asignar los datos a los controles
                                    lblNombreEmpleado.Text = reader["Nombre"].ToString();
                                    LblTitulo.Text = reader["Titulo"].ToString();
                                    lblFechaInicio.Text = Convert.ToDateTime(reader["FechaInicio"]).ToString("dd/MM/yyyy");
                                    lblFechaFin.Text = Convert.ToDateTime(reader["FechaFin"]).ToString("dd/MM/yyyy");
                                    lblDescripcion.Text = reader["Descripcion"].ToString();
                                }
                                else
                                {
                                    // Manejar el caso en el que no se encuentra la actividad
                                    Label1.Text = "Actividad no encontrada";
                                    Label1.Visible = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Manejo de errores
                            Label1.Text = "Error al cargar los datos: " + ex.Message;
                            Label1.Visible = true;
                        }
                    }
                }
            }
            else
            {
                // Manejar el caso en el que el idActividad no es válido
                Label1.Text = "ID de actividad no válido";
                Label1.Visible = true;
            }
        }

        protected void AceptarButton_Click(object sender, EventArgs e)
        {
            string idActividadString = Request.QueryString["evidencia"];
            int idJefe = ObtenerIdJefe();

            if (!int.TryParse(idActividadString, out int idActividad))
            {
                Label1.Text = "ID de actividad no válido.";
                Label1.Visible = true;
                return;
            }

            int idEmpleado = ObtenerIdEmpleadoPorActividad(idActividad);
            if (idEmpleado == -1)
            {
                Label1.Text = "No se encontró el idEmpleado para la actividad.";
                Label1.Visible = true;
                return;
            }

            DateTime fecha = DateTime.Now;
            string estado = "Aceptado";

            // Insertar el registro de actividad
            InsertarRegistroActividad(idJefe, idEmpleado, fecha, estado, idActividad);

            // Actualizar el estado de la actividad
            ActualizarEstadoActividad(idActividad);

            // Obtener el título de la actividad para la notificación
            string tituloActividad = ObtenerTituloActividad(idActividad);

            // Crear la notificación
            string tituloNotificacion = $"Actividad ({tituloActividad})";
            string motivo = "Actividad aceptada";
            CrearNotificacion(idJefe, idEmpleado, tituloNotificacion, motivo, fecha);

            Label1.Text = "<i class='fas fa-thumbs-up'></i> Actividad Aceptada";
            Label1.CssClass = "like-icon"; 
            Label1.Visible = true;

            // Opcionalmente ocultar el botón después de hacer clic
            AceptarButton.Visible = false;
        }



        protected void DenegarButton_Click(object sender, EventArgs e)
        {
            string idActividadString = Request.QueryString["evidencia"];
            int idJefe = ObtenerIdJefe();

            if (!int.TryParse(idActividadString, out int idActividad))
            {
                Label1.Text = "ID de actividad no válido.";
                Label1.Visible = true;
                return;
            }

            int idEmpleado = ObtenerIdEmpleadoPorActividad(idActividad);
            if (idEmpleado == -1)
            {
                Label1.Text = "No se encontró el idEmpleado para la actividad.";
                Label1.Visible = true;
                return;
            }

            DateTime fecha = DateTime.Now;
            string estado = "Denegado";

            // Insertar el registro de actividad
            InsertarRegistroActividad(idJefe, idEmpleado, fecha, estado, idActividad);

            // Actualizar el estado de la actividad
            ActualizarEstadoActividad(idActividad);

            // Obtener el título de la actividad para la notificación
            string tituloActividad = ObtenerTituloActividad(idActividad);

            // Crear la notificación
            string tituloNotificacion = $"Actividad ({tituloActividad})";
            string motivo = "Actividad rechazada";
            CrearNotificacion(idJefe, idEmpleado, tituloNotificacion, motivo, fecha);

            Label1.Text = "<i class='fas fa-thumbs-down'></i> Actividad Rechazada";
            Label1.CssClass = "like-icon"; // Añade la clase para el estilo
            Label1.Visible = true;
        }


        private string ObtenerTituloActividad(int idActividad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            string titulo = string.Empty;

            string query = "SELECT Titulo FROM actividadesregistradas WHERE id = @idActividad";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idActividad", idActividad);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        titulo = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Label1.Text = "Error al obtener el título de la actividad: " + ex.Message;
                    Label1.Visible = true;
                }
            }

            return titulo;
        }

        private void CrearNotificacion(int idEnviador, int idRecibidor, string titulo, string motivo, DateTime fecha)
        {
            try
            {
                // Crear una instancia del servicio de notificaciones
                NotificacionService notificacionService = new NotificacionService();
                notificacionService.InsertarNotificacion(idEnviador, idRecibidor, titulo, motivo, fecha);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Label1.Text = "Error al crear la notificación: " + ex.Message;
                Label1.Visible = true;
            }
        }


        private void ActualizarEstadoActividad(int idActividad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            string query = "UPDATE actividades SET Estado = @estado WHERE idActividad = @idActividad";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@estado", "Revisada");
                    command.Parameters.AddWithValue("@idActividad", idActividad);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            // Manejo de caso en el que no se encontró ninguna fila afectada
                            Label1.Text = "No se encontró ninguna actividad con el ID especificado.";
                            Label1.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        Label1.Text = "Error al actualizar el estado: " + ex.Message;
                        Label1.Visible = true;
                    }
                }
            }
        }

        private int ObtenerIdEmpleadoPorActividad(int idActividad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            int idEmpleado = -1;

            string query = "SELECT idEmpleado FROM actividades WHERE idActividad = @idActividad";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idActividad", idActividad);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out idEmpleado))
                        {
                            return idEmpleado;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        Label1.Text = "Error al obtener idEmpleado: " + ex.Message;
                        Label1.Visible = true;
                    }
                }
            }

            return -1;
        }

        private int ObtenerIdJefe()
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEmpleado))
            {
                return idEmpleado;
            }
            return -1;
        }

        private void InsertarRegistroActividad(int idJefe, int idEmpleado, DateTime fecha, string estado, int idActividad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            string query = @"
                INSERT INTO registroactividades (idJefe, idEmpleado, fecha, estado, idActividad)
                VALUES (@idJefe, @idEmpleado, @fecha, @estado, @idActividad)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idJefe", idJefe);
                    command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    command.Parameters.AddWithValue("@fecha", fecha);
                    command.Parameters.AddWithValue("@estado", estado);
                    command.Parameters.AddWithValue("@idActividad", idActividad);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        Label1.Text = "Error al insertar el registro: " + ex.Message;
                        Label1.Visible = true;
                    }
                }
            }
        }
    }
}
