using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI;
using ControlEmpresarial.Services;
using System.Web;
using ControlEmpresarial.Controlador;

namespace ControlEmpresarial.Vistas.Control_de_Actividades
{
    public partial class RegistroActividadesJefe : Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTiposDeActividad();
                CargarNombreUsuario();
                CargarNotificaciones();
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
                if (int.TryParse(cookie["idEmpleado"], out int idEmpleado))
                {
                    NotificacionService service = new NotificacionService();
                    List<Notificacion> notificaciones = service.ObtenerNotificaciones(idEmpleado);

                    repeaterNotificaciones.DataSource = notificaciones;
                    repeaterNotificaciones.DataBind();
                }
                else
                {
                    lblNombre.Text = "Error al extraer ID de empleado";
                    lblNombre.Visible = true;
                }
            }
            else
            {
                lblNombre.Text = "Cookie no encontrada";
                lblNombre.Visible = true;
            }
        }

        protected void CargarTiposDeActividad()
        {
            List<string> tiposDeActividad = ObtenerTiposDeActividadDesdeBaseDeDatos();
            dropdownTipoActividad.DataSource = tiposDeActividad;
            dropdownTipoActividad.DataBind();
        }

        protected List<string> ObtenerTiposDeActividadDesdeBaseDeDatos()
        {
            List<string> tiposDeActividad = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT Tipo FROM tipoactividad";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                try
                {
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tiposDeActividad.Add(reader["Tipo"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    debugLabel.Text = "Error al cargar tipos de actividad: " + ex.Message;
                }
            }

            return tiposDeActividad;
        }

        protected void AgregarTipoActividad_Click(object sender, EventArgs e)
        {
            agregarTipoActividad.Enabled = false;

            string nuevoTipoActividad = tipoActividad.Text;

            if (!string.IsNullOrWhiteSpace(nuevoTipoActividad))
            {
                InsertarTipoActividadEnBaseDeDatos(nuevoTipoActividad);
                CargarTiposDeActividad();
                tipoActividad.Text = string.Empty;
            }

            agregarTipoActividad.Enabled = true;
        }

        protected void InsertarTipoActividadEnBaseDeDatos(string tipoActividad)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO tipoactividad (Tipo) VALUES (@Tipo)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Tipo", tipoActividad);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    debugLabel.Text = rowsAffected > 0 ? "Tipo de actividad agregado exitosamente." : "No se agregó ningún tipo de actividad.";
                }
                catch (Exception ex)
                {
                    debugLabel.Text = "Error al agregar tipo de actividad: " + ex.Message;
                }
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            string tituloActividad = titulo.Text;
            string descripcionActividad = actividad.Text;
            string tipoActividad = dropdownTipoActividad.SelectedItem.Text; // Obtiene el texto del tipo de actividad seleccionado

            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEnviador))
            {
                int idDepartamento = ObtenerIdDepartamento(idEnviador);

                int idTipo = ObtenerIdTipoPorTipo(tipoActividad);

                debugLabel.Text = $"Datos para insertar: idEnviador={idEnviador}, idDepartamento={idDepartamento}, descripcion={descripcionActividad}, titulo={tituloActividad}, idTipo={idTipo}";

                bool exito = InsertarActividadEnBaseDeDatos(idEnviador, idDepartamento, descripcionActividad, tituloActividad, idTipo.ToString());

                if (exito)
                {
                    debugLabel.Text = "<i class='fas fa-thumbs-up'></i> Actividad guardada exitosamente.";
                }
                else
                {
                    debugLabel.Text = "<i class='fas fa-thumbs-down'></i> No se guardó ninguna actividad.";
                }
            }
            else
            {
                debugLabel.Text = "Error al obtener el ID del empleado.";
            }
        }


        private bool InsertarActividadEnBaseDeDatos(int idEnviador, int idDepartamento, string descripcion, string titulo, string idTipo)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO actividadesregistradas (idEnviador, idDepartamento, descripcion, fecha, estado, Titulo, idTipo) " +
                               "VALUES (@idEnviador, @idDepartamento, @descripcion, @fecha, @estado, @Titulo, @idTipo)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEnviador", idEnviador);
                cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now); // Fecha y hora actuales
                cmd.Parameters.AddWithValue("@estado", "Pendiente"); // Valor predeterminado
                cmd.Parameters.AddWithValue("@Titulo", titulo);
                cmd.Parameters.AddWithValue("@idTipo", idTipo);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        debugLabel.Text = $"Actividad guardada exitosamente. Filas afectadas: {rowsAffected}.";
                        return true;
                    }
                    else
                    {
                        debugLabel.Text = "No se guardó ninguna actividad. Filas afectadas: 0.";
                        return false;
                    }
                }
                catch (MySqlException mysqlEx)
                {
                    // Error específico de MySQL
                    debugLabel.Text = $"Error MySQL: {mysqlEx.Message}";
                }
                catch (Exception ex)
                {
                    // Error general
                    debugLabel.Text = $"Error general: {ex.Message}";
                }
                finally
                {
                    // Asegúrate de cerrar la conexión
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return false;
            }
        }


        private int ObtenerIdDepartamento(int idEmpleado)
        {
            int idDepartamento = 0; // Valor predeterminado en caso de error

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idDepartamento FROM empleado WHERE idEmpleado = @idEmpleado";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        idDepartamento = Convert.ToInt32(result);
                        debugLabel.Text = $"ID Departamento: {idDepartamento}"; // Mensaje de depuración
                    }
                    else
                    {
                        debugLabel.Text = "No se encontró el departamento para el empleado.";
                    }
                }
                catch (Exception ex)
                {
                    debugLabel.Text = "Error al obtener el ID del departamento: " + ex.Message;
                }
            }

            return idDepartamento;
        }

        private int ObtenerIdTipoPorTipo(string tipo)
        {
            int idTipo = 0; // Valor predeterminado en caso de error

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idTipo FROM tipoactividad WHERE Tipo = @Tipo";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Tipo", tipo);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        idTipo = Convert.ToInt32(result);
                    }
                    else
                    {
                        debugLabel.Text = "No se encontró el tipo de actividad.";
                    }
                }
                catch (Exception ex)
                {
                    debugLabel.Text = "Error al obtener el ID del tipo de actividad: " + ex.Message;
                }
            }

            return idTipo;
        }

    }
}
