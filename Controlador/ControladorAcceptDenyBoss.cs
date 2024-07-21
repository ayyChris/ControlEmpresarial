using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Horas_Extra
{
    public partial class AceptarDenegarHorasExtraJefatura : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEmpleados();
            }
        }

        private void CargarEmpleados()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            DataTable dtEmpleados = ObtenerColaboradores(connectionString);

            colaborador.DataSource = dtEmpleados;
            colaborador.DataTextField = "Nombre";
            colaborador.DataValueField = "idEmpleado";
            colaborador.DataBind();

            // Add a default item at the beginning
            colaborador.Items.Insert(0, new ListItem("-- Seleccione un colaborador --", "0"));
        }

        private DataTable ObtenerColaboradores(string connectionString)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idEmpleado, Nombre FROM empleado";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        protected void colaborador_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idEmpleado;
            if (int.TryParse(colaborador.SelectedValue, out idEmpleado))
            {
                // Obtén los datos basados en la selección.
                DataTable dtEvidencias = ObtenerEvidencias(idEmpleado);

                if (dtEvidencias.Rows.Count > 0)
                {
                    // Supongamos que la primera fila tiene la información que necesitas.
                    DataRow row = dtEvidencias.Rows[0];

                    lblFechaRespuesta.Text = row["FechaEvidencia"].ToString();
                    lblEvidencia.Text = row["EnlaceEvidencia"].ToString();
                }
                else
                {
                    lblFechaRespuesta.Text = "No hay datos disponibles.";
                    lblEvidencia.Text = "No hay datos disponibles.";
                }
            }
        }
        private DataTable ObtenerData(int idEmpleado)
        {
            DataTable dt = new DataTable();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idEvidencia, idSolicitud " +
                               "FROM evidenciahorasextras " +
                               "WHERE idEmpleado = @idEmpleado AND Estado = 'Evidenciada'";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }
        private DataTable ObtenerEvidencias(int idEmpleado)
        {
            DataTable dt = new DataTable();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT FechaEvidencia, EnlaceEvidencia " +
                               "FROM evidenciahorasextras " +
                               "WHERE idEmpleado = @idEmpleado AND Estado = 'Evidenciada'";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }

    }
}
