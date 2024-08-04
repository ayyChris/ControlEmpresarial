using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Control_de_Actividades
{
    public partial class AgregarTipoActividadesSupervisor : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTiposDeActividad();
            }
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

        private void CargarTiposDeActividad()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idTipo, Tipo FROM tipoactividad";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    adapter.Fill(dt);
                    gridViewTipoActividades.DataSource = dt;
                    gridViewTipoActividades.DataBind();
                }
                catch (Exception ex)
                {
                    debugLabel.Text = "Error al cargar tipos de actividad: " + ex.Message;
                }
            }
        }

        protected void GridViewTipoActividades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int idTipo = Convert.ToInt32(e.CommandArgument);
                EliminarTipoActividad(idTipo);
                CargarTiposDeActividad(); // Recarga los datos del GridView
            }
        }

        private void EliminarTipoActividad(int idTipo)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM tipoactividad WHERE idTipo = @idTipo";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idTipo", idTipo);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    debugLabel.Text = rowsAffected > 0 ? "Tipo de actividad eliminado exitosamente." : "No se eliminó ningún tipo de actividad.";
                }
                catch (Exception ex)
                {
                    debugLabel.Text = "Error al eliminar tipo de actividad: " + ex.Message;
                }
            }
        }

    }
}
