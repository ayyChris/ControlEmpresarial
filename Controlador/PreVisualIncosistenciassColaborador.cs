using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace ControlEmpresarial.Vistas.Inconsistencias
{
    public partial class PreVisualIncosistenciasColaborador : System.Web.UI.Page
    {
        // Método que se ejecuta al cargar la página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarInconsistencias();
            }
        }
        // Método para obtener los datos de la base de datos
        private DataTable ObtenerInconsistencias(int empleadoId)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            DataTable dt = new DataTable();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
            SELECT i.idInconsistencia, ti.nombre AS TipoInconsistencia, i.Fecha, i.Estado
            FROM inconsistencias i
            JOIN tiposinconsistencia ti ON i.idTipoInconsistencia = ti.idTipoInconsistencia
            WHERE i.idEmpleado = @EmpleadoId
            AND i.Estado = 'Activo'  -- Filtra solo las inconsistencias con estado 'Activo'
            ORDER BY i.Fecha DESC";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmpleadoId", empleadoId);

                    try
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        adapter.Fill(dt);

                        // Debugging: Check if data is being fetched correctly
                        System.Diagnostics.Debug.WriteLine($"Rows fetched: {dt.Rows.Count}");
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = $"Error al obtener datos: {ex.Message}";
                        lblError.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }

            return dt;
        }



        // Método para cargar los datos en el Repeater
        private void CargarInconsistencias()
        {
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            if (userCookie != null)
            {
                int empleadoId;
                if (int.TryParse(userCookie["idEmpleado"], out empleadoId))
                {
                    DataTable dtInconsistencias = ObtenerInconsistencias(empleadoId);

                    if (dtInconsistencias.Rows.Count > 0)
                    {
                        RepeaterInconsistencias.DataSource = dtInconsistencias;
                        RepeaterInconsistencias.DataBind();
                    }
                    else
                    {
                        lblNoInconsistencias.Text = "No hay inconsistencias para mostrar.";
                        lblNoInconsistencias.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "ID de empleado no válido.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                lblError.Text = "No se ha encontrado la cookie de usuario.";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected void RepeaterInconsistencias_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Verifica si el CommandName es el esperado
            if (e.CommandName == "VerDetalles")
            {
                // Verifica el CommandArgument
                string idInconsistencia = e.CommandArgument.ToString();
                lblError.Text = $"ID Inconsistencia: {idInconsistencia}"; // Debugging line
                lblError.ForeColor = System.Drawing.Color.Red;

                // Log de depuración
                System.Diagnostics.Debug.WriteLine($"Redireccionando a: ~/Vistas/Inconsistencias/EvidenciaIncosistenciaColaborador.aspx?idInconsistencia={idInconsistencia}");

                try
                {
                    // Redirige a la página de detalles
                    Response.Redirect("~/Vistas/Inconsistencias/EvidenciaIncosistenciaColaborador.aspx?idInconsistencia=" + idInconsistencia);
                }
                catch (Exception ex)
                {
                    lblError.Text = $"Error al redirigir: {ex.Message}";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                // Verifica si el CommandName no coincide con los esperados
                System.Diagnostics.Debug.WriteLine($"CommandName no reconocido: {e.CommandName}");
            }
        }

    }
}
