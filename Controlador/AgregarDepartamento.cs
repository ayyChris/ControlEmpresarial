using MySql.Data.MySqlClient;
using System;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Colaborador
{
    public partial class AgregarDepartamento : Page
    {
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
    }
}
