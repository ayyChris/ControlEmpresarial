using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;

namespace ControlEmpresarial.Vistas
{
    public partial class HistoricoActividades : System.Web.UI.Page
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
                    Label1.Text = $"Error: {ex.Message}";
                }
            }
        }

        private void CargarDatos()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                ra.idRegistroActividades AS idRegistroActividades,
                ej.Nombre AS NombreJefe,
                ee.Nombre AS NombreEmpleado,
                ar.Titulo AS TituloActividad,
                ar.descripcion AS DescripcionActividad,
                ta.Tipo AS TipoActividad,
                ra.estado
            FROM 
                RegistroActividades ra
            JOIN 
                Empleado ej ON ra.idJefe = ej.idEmpleado
            JOIN 
                Empleado ee ON ra.idEmpleado = ee.idEmpleado
            JOIN 
                ActividadesRegistradas ar ON ra.idActividad = ar.id
            JOIN 
                TipoActividad ta ON ar.idTipo = ta.idTipo
        ";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    DataTable dt = new DataTable();

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        conn.Open();
                        da.Fill(dt);
                    }

                    // Asignar el DataTable al GridView
                    gvHorasActividades.DataSource = dt;
                    gvHorasActividades.DataBind();
                }
            }
        }


    }
}
