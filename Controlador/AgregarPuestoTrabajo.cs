using MySql.Data.MySqlClient;
using System;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Colaborador
{
    public partial class AgregarPuestoTrabajo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarNombreUsuario();
            }
        }
        protected void volverMenu_Click(object sender, EventArgs e)
        {

        }
        protected void ingresar_Click(object sender, EventArgs e)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;

            string nombrePuestoTrabajo = puestoTrabajo.Text.Trim();

            if (string.IsNullOrEmpty(nombrePuestoTrabajo))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ingrese un puesto de trabajo válido.')", true);
                return;
            }

            // Validación: Verificar si el puesto ya existe en la base de datos
            if (PuestoExiste(nombrePuestoTrabajo, connectionString))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('El puesto de trabajo ya existe.')", true);
                return;
            }

            // Insertar el puesto en la base de datos
            InsertarPuesto(nombrePuestoTrabajo, connectionString);
        }

        private bool PuestoExiste(string nombrePuestoTrabajo, string connectionString)
        {
            bool existe = false;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM PuestoTrabajo WHERE nombrePuesto = @nombrePuesto";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombrePuesto", nombrePuestoTrabajo);

                try
                {
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                        existe = true;
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error al verificar la existencia del puesto de trabajo: {ex.Message}')", true);
                }
            }

            return existe;
        }

        private void InsertarPuesto(string nombrePuestoTrabajo, string connectionString)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO PuestoTrabajo (nombrePuesto) VALUES (@nombrePuesto)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombrePuesto", nombrePuestoTrabajo);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Puesto de trabajo ingresado correctamente.')", true);
                        // Aquí puedes limpiar el TextBox u ofrecer otra interacción al usuario
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('No se pudo ingresar el puesto de trabajo.')", true);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error al ingresar el puesto de trabajo: {ex.Message}')", true);
                }
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
    }
}
