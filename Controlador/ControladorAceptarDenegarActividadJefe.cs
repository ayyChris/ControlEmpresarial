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
                string idActividadStr = Request.QueryString["id"];
                if (int.TryParse(idActividadStr, out int idActividad))
                {
                    CargarActividad(idActividad);
                }
                else
                {
                    Label1.Text = "ID de actividad no válida.";
                    Label1.Visible = true;
                }
            }
        }

        private void CargarActividad(int idActividad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Consulta SQL ajustada para seleccionar los datos requeridos
                string query = @"
            SELECT a.fecha, a.descripcion, a.Titulo, a.horaInicio, a.horaFin, e.Nombre
            FROM actividadesregistradas a
            JOIN empleado e ON a.idEnviador = e.idEmpleado
            WHERE a.id = @idActividad";

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
                                // Mostrar los datos en los controles
                                lblNombreEmpleado.Text = reader["Nombre"].ToString(); // Nombre del empleado
                                LblTitulo.Text = reader["Titulo"].ToString();
                                lblFecha.Text = Convert.ToDateTime(reader["fecha"]).ToString("dd/MM/yyyy"); // Solo la fecha

                                // Manejo del TimeSpan para horaInicio y horaFin
                                TimeSpan horaInicio = (TimeSpan)reader["horaInicio"];
                                TimeSpan horaFin = (TimeSpan)reader["horaFin"];

                                lblHoraInicio.Text = horaInicio.ToString(@"hh\:mm"); // Hora de inicio
                                lblHoraFin.Text = horaFin.ToString(@"hh\:mm"); // Hora final

                                lblDescripcion.Text = reader["descripcion"].ToString();
                            }
                            else
                            {
                                Label1.Text = "Actividad no encontrada.";
                                Label1.Visible = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Label1.Text = "Error al cargar la actividad: " + ex.Message;
                        Label1.Visible = true;
                    }
                }
            }
        }

        private void ActualizarEstadoActividad(int idActividad, string nuevoEstado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            string query = "UPDATE actividadesregistradas SET estado = @nuevoEstado WHERE id = @idActividad AND estado = 'Pendiente'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                    command.Parameters.AddWithValue("@idActividad", idActividad);

                    try
                    {
                        connection.Open();
                        int filasAfectadas = command.ExecuteNonQuery();

                        // Verifica si se actualizó alguna fila
                        if (filasAfectadas == 0)
                        {
                            // No se actualizó ninguna fila, lo que significa que el estado no era "Pendiente"
                            Label1.Text = $"La actividad (ID: {idActividad}) no estaba en estado 'Pendiente'.";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        Label1.Text = "Error al actualizar el estado de la actividad: " + ex.Message;
                        Label1.Visible = true;
                    }
                }
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

            ActualizarEstadoActividad(idActividad, "Aceptada");
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
            ActualizarEstadoActividad(idActividad, "Denegada");
            // Insertar el registro de actividad
            InsertarRegistroActividad(idJefe, idEmpleado, fecha, estado, idActividad);


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


        private int ObtenerIdEmpleadoPorActividad(int idActividad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            int idEnviador = -1; // Valor predeterminado para cuando no se encuentra el ID

            // Definir la consulta SQL
            string query = "SELECT idEnviador FROM actividadesregistradas WHERE id = @idActividad";

            // Crear y abrir la conexión a la base de datos
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Crear el comando SQL
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Añadir el parámetro para evitar SQL Injection
                        command.Parameters.AddWithValue("@idActividad", idActividad);

                        // Ejecutar el comando y leer el resultado
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Obtener el valor del idEnviador
                                idEnviador = reader.GetInt32("idEnviador");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Label1.Text = "Error al conseguir la idEnviador: " + ex.Message;
                    Label1.Visible = true;
                }
            }

            // Devolver el idEnviador encontrado o -1 si no se encontró
            return idEnviador;
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
