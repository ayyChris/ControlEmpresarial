using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Incapacidades
{
    public partial class AgregarTipoIncapacidad : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTiposIncapacidades();
            }
        }

        protected void volverMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect("../PaginaPrincipal/MenuSupervisor.aspx");
        }

        private void CargarTiposIncapacidades()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            DataTable dt = ObtenerTiposIncapacidadesDesdeBaseDeDatos(connectionString);

            gridIncapacidades.DataSource = dt;
            gridIncapacidades.DataBind();
        }

        private DataTable ObtenerTiposIncapacidadesDesdeBaseDeDatos(string connectionString)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT Nombre,Descripcion FROM TiposIncapacidad ORDER BY Nombre";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        protected void ingresar_Click(object sender, EventArgs e)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;

            string tipoIncapacidad = tipoincapacidad.Text.Trim();
            string descripcionIncapacidad = descripcion.Text.Trim();

            if (string.IsNullOrEmpty(tipoIncapacidad) || string.IsNullOrEmpty(descripcionIncapacidad))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ingrese un tipo de incapacidad válido.')", true);
                return;
            }

            if (PuestoExiste(tipoIncapacidad, descripcionIncapacidad, connectionString))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('El tipo de incapacidad ya existe.')", true);
                return;
            }

            InsertarPuesto(tipoIncapacidad, descripcionIncapacidad, connectionString);
        }

        private bool PuestoExiste(string tipoIncapacidad, string descripcionIncapacidad, string connectionString)
        {
            bool existe = false;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM TiposIncapacidad WHERE Nombre = @Nombre AND Descripcion = @Descripcion";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", tipoIncapacidad);
                cmd.Parameters.AddWithValue("@Descripcion", descripcionIncapacidad);

                try
                {
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                        existe = true;
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error al verificar la existencia del tipo de incapacidad: {ex.Message}')", true);
                }
            }

            return existe;
        }

        private void InsertarPuesto(string tipoIncapacidad, string descripcionIncapacidad, string connectionString)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO TiposIncapacidad (Nombre,Descripcion) VALUES (@Nombre,@Descripcion)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", tipoIncapacidad);
                cmd.Parameters.AddWithValue("@Descripcion", descripcionIncapacidad);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Tipo de incapacidad ingresado correctamente.')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('No se pudo ingresar el Tipo de incapacidad.')", true);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error al ingresar el Tipo de incapacidad: {ex.Message}')", true);
                }
            }
        }
    }
}
