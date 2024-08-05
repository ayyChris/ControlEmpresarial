using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace ControlEmpresarial.Vistas
{
    public partial class HistoricoHorasExtras : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    // Asignar el mensaje de error al Label
                    Label1.Text = $"Error: {ex.Message}";
                }
            }
        }



        private void CargarDatos()
        {
            

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"SELECT 
                                    he.idHorasExtras,
                                    e.Nombre AS NombreEmpleado,
                                    ev.EnlaceEvidencia,
                                    he.HorasTrabajadas,
                                    he.Aceptacion
                                 FROM 
                                    horasextras he
                                 JOIN 
                                    evidenciahorasextras ev ON he.idEvidencia = ev.idEvidencia
                                 JOIN 
                                    empleado e ON he.idEmpleado = e.idEmpleado"; 

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    DataTable dt = new DataTable();

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        conn.Open();
                        da.Fill(dt);
                    }

                    // Asignar el DataTable al GridView
                    gvHorasExtras.DataSource = dt;
                    gvHorasExtras.DataBind();
                }
            }
        }
    }
}