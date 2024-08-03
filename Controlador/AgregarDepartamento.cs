using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Colaborador
{
    public partial class AgregarDepartamento : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CargarNombreUsuario();
                CargarDepartamentos();
            }
        }

        protected void volverMenu_Click(object sender, EventArgs e)
        {

        }
        protected void ingresar_Click(object sender, EventArgs e)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;

            string nombreDepartamento = txtNombreDepartamento.Text.Trim();

            if (string.IsNullOrEmpty(nombreDepartamento))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ingrese un nombre de departamento válido.')", true);
                return;
            }

            // Validación: Verificar si el departamento ya existe en la base de datos
            if (DepartamentoExiste(nombreDepartamento, connectionString))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('El departamento ya existe.')", true);
                return;
            }

            // Insertar el departamento en la base de datos
            InsertarDepartamento(nombreDepartamento, connectionString);
        }

        private void CargarDepartamentos()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            DataTable dt = ObtenerDepartamentosDesdeBaseDeDatos(connectionString);

            // Asigna el DataTable como fuente de datos del GridView
            gridEmpleados.DataSource = dt;
            gridEmpleados.DataBind();
        }
        private DataTable ObtenerDepartamentosDesdeBaseDeDatos(string connectionString)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT nombreDepartamento FROM Departamento ORDER BY nombreDepartamento";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            foreach (DataRow row in dt.Rows)
            {
                System.Diagnostics.Debug.WriteLine("Nombre: " + row["nombreDepartamento"]);
            }

            return dt;
        }

        private bool DepartamentoExiste(string nombreDepartamento, string connectionString)
        {
            bool existe = false;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Departamento WHERE nombreDepartamento = @nombreDepartamento";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombreDepartamento", nombreDepartamento);

                try
                {
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                        existe = true;
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error al verificar la existencia del departamento: {ex.Message}')", true);
                }
            }

            return existe;
        }

        private void InsertarDepartamento(string nombreDepartamento, string connectionString)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Departamento (nombreDepartamento) VALUES (@nombreDepartamento)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombreDepartamento", nombreDepartamento);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Departamento ingresado correctamente.')", true);
                        // Aquí puedes limpiar el TextBox u ofrecer otra interacción al usuario
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('No se pudo ingresar el departamento.')", true);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error al ingresar el departamento: {ex.Message}')", true);
                }
            }
        }
        /*private void CargarNombreUsuario()
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
        }*/
    }
}
