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
    public partial class ControlActividadesColaborador : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarNombreUsuario();
                CargarNotificaciones();
                CargarDatosDropDownList();
            }
        }

        private void CargarDatosDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            int idEmpleado = ObtenerIdEmpleado();

            if (idEmpleado != -1)
            {
                string query = @"
        SELECT ar.Titulo, ar.id
        FROM actividadesregistradas ar
        INNER JOIN relacionactividadempleado re ON ar.id = re.idActividad
        WHERE re.idEmpleado = @idEmpleado
        AND re.Estado = 'Espera'";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                        try
                        {
                            connection.Open();
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                DropDownList1.Items.Clear();
                                while (reader.Read())
                                {
                                    string titulo = reader["Titulo"].ToString();
                                    string id = reader["id"].ToString();
                                    DropDownList1.Items.Add(new ListItem(titulo, id));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Manejo de errores
                            debugLabel.Text = "Error al cargar los datos: " + ex.Message;
                            debugLabel.Visible = true;
                        }
                    }
                }
            }
            else
            {
                debugLabel.Text = "Error al obtener el idEmpleado.";
                debugLabel.Visible = true;
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
                    debugLabel.Text = "Error al extraer ID de empleado";
                    debugLabel.Visible = true;
                }
            }
            else
            {
                debugLabel.Text = "Cookie no encontrada";
                debugLabel.Visible = true;
            }
        }

        private int ObtenerIdEmpleado()
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEmpleado))
            {
                return idEmpleado;
            }
            return -1;
        }
        private bool ValidarCampos(out string mensajeError)
        {
            mensajeError = string.Empty;

            // Verificar si se ha seleccionado una actividad
            if (string.IsNullOrEmpty(DropDownList1.SelectedValue))
            {
                mensajeError = "Por favor, seleccione una actividad.";
                return false;
            }

            // Verificar si la fecha de inicio está presente y es válida
            if (string.IsNullOrEmpty(inicio.Text) || !DateTime.TryParse(inicio.Text, out _))
            {
                mensajeError = "Por favor, ingrese una fecha de inicio válida.";
                return false;
            }

            // Verificar si la fecha de fin está presente y es válida
            if (string.IsNullOrEmpty(final.Text) || !DateTime.TryParse(final.Text, out _))
            {
                mensajeError = "Por favor, ingrese una fecha de fin válida.";
                return false;
            }

            // Verificar si la descripción de la actividad está presente
            if (string.IsNullOrEmpty(actividad.Text))
            {
                mensajeError = "Por favor, ingrese una descripción para la actividad.";
                return false;
            }

            // Todos los campos están presentes y válidos
            return true;
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            string mensajeError;

            // Validar campos
            if (!ValidarCampos(out mensajeError))
            {
                debugLabel.Text = mensajeError;
                debugLabel.Visible = true;
                return;
            }

            int idEmpleado = ObtenerIdEmpleado();
            if (idEmpleado == -1)
            {
                debugLabel.Text = "Error al obtener el idEmpleado.";
                debugLabel.Visible = true;
                return;
            }

            string idActividadRegistrada = DropDownList1.SelectedValue;
            string descripcion = actividad.Text;
            DateTime fechaInicio = Convert.ToDateTime(inicio.Text);
            DateTime fechaFin = Convert.ToDateTime(final.Text);

            InsertarActividad(idEmpleado, idActividadRegistrada, descripcion, fechaInicio, fechaFin);
        }

        private void InsertarActividad(int idEmpleado, string idActividadRegistrada, string descripcion, DateTime fechaInicio, DateTime fechaFin)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            string query = @"
            INSERT INTO actividades (Descripcion, FechaInicio, FechaFin, Estado, idEmpleado, idActividadRegistrada) 
            VALUES (@Descripcion, @FechaInicio, @FechaFin, 'Validada', @idEmpleado, @idActividadRegistrada)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Descripcion", descripcion);
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);
                    command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    command.Parameters.AddWithValue("@idActividadRegistrada", idActividadRegistrada);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        // Mostrar mensaje de éxito
                        debugLabel.Text = "Actividad evidenciada";
                        debugLabel.Visible = true;

                        // Mostrar icono de like
                        debugLabel.Text += " &#128077;"; // Icono de "like"
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        debugLabel.Text = "Error al insertar la actividad: " + ex.Message;
                        debugLabel.Visible = true;
                    }
                }
            }
        }
    }
}
