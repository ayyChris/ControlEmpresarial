using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI;
using System.Web;

namespace ControlEmpresarial.Vistas.Control_de_Actividades
{
    public partial class RegistroActividadesColaborador : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTiposDeActividad();
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
                catch (MySqlException mysqlEx)
                {
                    debugLabel.Text = $"Error al cargar tipos de actividad: {mysqlEx.Message}\n{mysqlEx.StackTrace}";
                }
                catch (Exception ex)
                {
                    debugLabel.Text = $"Error general: {ex.Message}\n{ex.StackTrace}";
                }
            }

            return tiposDeActividad;
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            // Obtener los valores de los controles TextBox
            string tituloActividad = titulo.Text;
            string descripcionActividad = actividad.Text;
            string tipoActividad = dropdownTipoActividad.SelectedItem.Text;
            string horaInicioInput = horaInicio.Text;
            string horaFinalInput = horaFinal.Text;

            try
            {
                TimeSpan horaInicio = TimeSpan.Parse(horaInicioInput);
                TimeSpan horaFinal = TimeSpan.Parse(horaFinalInput);

                HttpCookie cookie = Request.Cookies["UserInfo"];
                if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEnviador))
                {
                    int idDepartamento = ObtenerIdDepartamento(idEnviador);
                    int idTipo = ObtenerIdTipoPorTipo(tipoActividad);

                    debugLabel.Text = $"Datos para insertar: idEnviador={idEnviador}, idDepartamento={idDepartamento}, descripcion={descripcionActividad}, titulo={tituloActividad}, idTipo={idTipo}, horaInicio={horaInicio}, horaFinal={horaFinal}";

                    // Llamada al método de inserción con los nuevos parámetros
                    bool exito = InsertarActividadEnBaseDeDatos(idEnviador, idDepartamento, descripcionActividad, tituloActividad, idTipo.ToString(), horaInicio, horaFinal);

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
            catch (FormatException ex)
            {
                Label1.Text = "Formato de hora inválido: " + ex.Message;
            }
            catch (Exception ex)
            {
                Label1.Text = "Error general: " + ex.Message;
            }
        }

        private bool InsertarActividadEnBaseDeDatos(int idEnviador, int idDepartamento, string descripcion, string titulo, string idTipo, TimeSpan horaInicio, TimeSpan horaFin)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO actividadesregistradas (idEnviador, idDepartamento, descripcion, fecha, estado, Titulo, idTipo, horaInicio, horaFin) " +
                               "VALUES (@idEnviador, @idDepartamento, @descripcion, @fecha, @estado, @Titulo, @idTipo, @horaInicio, @horaFin)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEnviador", idEnviador);
                cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now); // Fecha y hora actuales
                cmd.Parameters.AddWithValue("@estado", "Pendiente"); // Valor predeterminado
                cmd.Parameters.AddWithValue("@Titulo", titulo);
                cmd.Parameters.AddWithValue("@idTipo", idTipo);
                cmd.Parameters.AddWithValue("@horaInicio", horaInicio.ToString(@"hh\:mm\:ss"));
                cmd.Parameters.AddWithValue("@horaFin", horaFin.ToString(@"hh\:mm\:ss"));

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
                    debugLabel.Text = $"Error MySQL: {mysqlEx.Message}\n{mysqlEx.StackTrace}";
                }
                catch (Exception ex)
                {
                    debugLabel.Text = $"Error general: {ex.Message}\n{ex.StackTrace}";
                }
                finally
                {
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
                        Label1.Text = $"ID Departamento: {idDepartamento}"; // Mensaje de depuración
                    }
                    else
                    {
                        Label1.Text = "No se encontró el departamento para el empleado.";
                    }
                }
                catch (MySqlException mysqlEx)
                {
                    Label1.Text = $"Error MySQL al obtener el ID del departamento: {mysqlEx.Message}\n{mysqlEx.StackTrace}";
                }
                catch (Exception ex)
                {
                    Label1.Text = $"Error general al obtener el ID del departamento: {ex.Message}\n{ex.StackTrace}";
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
                catch (MySqlException mysqlEx)
                {
                    Label1.Text = $"Error MySQL al obtener el ID del tipo de actividad: {mysqlEx.Message}\n{mysqlEx.StackTrace}";
                }
                catch (Exception ex)
                {
                    Label1.Text = $"Error general al obtener el ID del tipo de actividad: {ex.Message}\n{ex.StackTrace}";
                }
            }

            return idTipo;
        }
    }
}
