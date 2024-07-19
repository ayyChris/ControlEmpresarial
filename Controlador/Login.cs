using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

namespace ControlEmpresarial.Vistas
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            string host = "138.59.135.33"; //IP
            int port = 3306; 
            string database = "tiusr38pl_gestion"; 
            string username = "gestion"; 
            string password = "Ihnu00&34"; 

            // Cadena de conexión
            string connectionString = $"Server={host};Port={port};Database={database};Uid={username};Pwd={password};";

            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("Conexión establecida correctamente.");
                string Pass = contrasena.Text;
                System.Diagnostics.Debug.WriteLine($"Valor de Pass: {Pass}");
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Login button clicked')", true);
                System.Diagnostics.Debug.WriteLine($"conexion exitosa");
                connection.Close();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error: {ex.Message}');", true);
            }
        }
    }
}