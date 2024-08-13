using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Inconsistencias
{
    public partial class AgregarTipoIncosistenciasSupervisor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTiposInconsistencias();
            }
        }
        private void CargarTiposInconsistencias()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT idTipoInconsistencia, Nombre, Descripcion, ReponerTiempo FROM tiposinconsistencia";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        gridViewTipoIncosistencia.DataSource = dataTable;
                        gridViewTipoIncosistencia.DataBind();
                    }
                }
            }
        }
        protected void agregarTipoIncosistencia_Click(object sender, EventArgs e)
        {
            // Obtener valores de los controles
            string tipo = tipoIncosistencia.Text.Trim();
            string descripcion = descripcionIncosistencia.Text.Trim();
            string reponerTiempoSeleccionado = reponerTiempo.SelectedValue; // Aquí obtenemos el valor seleccionado del DropDownList

            // Validar campos
            if (string.IsNullOrEmpty(tipo))
            {
                debugLabel.Text = "El tipo de inconsistencia es obligatorio.";
                debugLabel.Visible = true;
                return;
            }

            // Insertar datos en la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO tiposinconsistencia (Nombre, Descripcion, ReponerTiempo) VALUES (@Nombre, @Descripcion, @ReponerTiempo)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", tipo);
                    command.Parameters.AddWithValue("@Descripcion", descripcion);
                    command.Parameters.AddWithValue("@ReponerTiempo", reponerTiempoSeleccionado); // Aquí asignamos el valor seleccionado

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        debugLabel.Text = "Tipo de inconsistencia agregado correctamente.";
                        debugLabel.ForeColor = System.Drawing.Color.Green;
                        debugLabel.Visible = true;

                        // Limpiar campos
                        tipoIncosistencia.Text = "";
                        descripcionIncosistencia.Text = "";
                        reponerTiempo.SelectedIndex = -1; // Limpiar la selección del DropDownList

                        // Recargar los datos en el GridView
                        CargarTiposInconsistencias();
                    }
                    catch (Exception ex)
                    {
                        debugLabel.Text = "Error al agregar tipo de inconsistencia: " + ex.Message;
                        debugLabel.Visible = true;
                    }
                }
            }
        }



        protected void GridViewTipoIncosistencia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int idTipoInconsistencia = Convert.ToInt32(e.CommandArgument);

                string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "DELETE FROM tiposinconsistencia WHERE idTipoInconsistencia = @idTipoInconsistencia";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idTipoInconsistencia", idTipoInconsistencia);

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            // Recargar los datos en el GridView después de eliminar
                            CargarTiposInconsistencias();
                        }
                        catch (Exception ex)
                        {
                            debugLabel.Text = "Error al eliminar tipo de inconsistencia: " + ex.Message;
                            debugLabel.Visible = true;
                        }
                    }
                }
            }
        }
    }
}
