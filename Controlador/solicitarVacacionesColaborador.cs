using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

namespace ControlEmpresarial.Controlador
{
    public partial class SolicitarVacacion : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("HOLAAAAAAAA");
            if (!IsPostBack)
            {
                // Obtener la cookie
                HttpCookie cookie = Request.Cookies["UserCookie"];
                if (cookie != null)
                {
                    // Extraer el valor de idEmpleado
                    string idEmpleadoValue = ConseguirCookie(cookie.Value, "idEmpleado");
                    System.Diagnostics.Debug.WriteLine("idEmpleado extraído de la cookie: " + idEmpleado);

                    if (idEmpleadoValue != null)
                    {
                        if (int.TryParse(idEmpleadoValue, out int idEmpleado))
                        {
                            LoadEmployeeVacationDays(idEmpleado);
                        }
                        else
                        {
                            vacacionesCountLabel.Text = "0";
                        }
                    }
                }
            }
        }

        private string ConseguirCookie(string cookieString, string key)
        {
            // Divide la cadena de cookie en pares clave-valor
            var pairs = cookieString.Split('&');
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2 && keyValue[0] == key)
                {
                    return keyValue[1];
                }
            }
            return null;
        }
        private void CargarDiasDisponibles(int idEmpleado)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT DiasDeVacaciones FROM Empleado WHERE idEmpleado = @idEmpleado";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        int diasDeVacaciones = Convert.ToInt32(result);
                        vacacionesCountLabel.Text = diasDeVacaciones.ToString();
                    }
                    else
                    {
                        vacacionesCountLabel.Text = "0"; // Default value if no record found
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error al encontrar el label: {ex.Message}')", true);
                }
            }
        }



        protected void submit_Click(object sender, EventArgs e)
        {
           
        }
    }

}
