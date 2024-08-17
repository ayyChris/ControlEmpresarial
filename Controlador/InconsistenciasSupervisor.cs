using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace ControlEmpresarial.Vistas.Inconsistencias
{
    public partial class InconsistenciasSupervisor : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarJustificaciones();
            }
        }

        private void CargarJustificaciones()
        {
            string query = @"
                SELECT 
                    ji.idJustificacion,
                    e.Nombre AS NombreEmpleado,
                    ti.Nombre AS NombreInconsistencia,
                    ji.Justificacion,
                    ji.FechaJustificacion,
                    ji.Estado
                FROM 
                    justificacioninconsistencia ji
                JOIN 
                    inconsistencias i ON ji.idInconsistencia = i.idInconsistencia
                JOIN 
                    empleado e ON i.idEmpleado = e.idEmpleado
                JOIN 
                    tiposinconsistencia ti ON i.idTipoInconsistencia = ti.idTipoInconsistencia";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        adapter.Fill(dt);
                        gvJustificaciones.DataSource = dt;
                        gvJustificaciones.DataBind();
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores (opcional)
                        // Puedes registrar el error en un archivo o base de datos
                        lblErrorMessage.Text = "Error al cargar los datos: " + ex.Message;
                        lblErrorMessage.Visible = true;
                    }
                }
            }
        }
    }
}
