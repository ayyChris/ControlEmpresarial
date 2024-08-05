using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

namespace ControlEmpresarial.Vistas
{
    public partial class VisualizacionColaboradorSupervisor : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {   
                System.Diagnostics.Debug.WriteLine("Page_Load: Page first loaded.");
                CargarDepartamentos();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Page_Load: Page posted back.");
            }
        }

        private void CargarDepartamentos()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            DataTable dtDepartamentos = ObtenerDepartamentosDesdeBaseDeDatos(connectionString);

            ddlDepartamento.DataTextField = "nombreDepartamento";
            ddlDepartamento.DataValueField = "idDepartamento";
            ddlDepartamento.DataSource = dtDepartamentos;
            ddlDepartamento.DataBind();

            ddlDepartamento.Items.Insert(0, new ListItem("Seleccione un departamento", ""));
        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idDepartamento = Convert.ToInt32(ddlDepartamento.SelectedValue);
            CargarEmpleadosPorDepartamento(idDepartamento);
        }


        private void CargarEmpleadosPorDepartamento(int idDepartamento)
        {
            DataTable dtEmpleados = ObtenerEmpleadosPorDepartamentoDesdeBaseDeDatos(idDepartamento);
            System.Diagnostics.Debug.WriteLine(idDepartamento);
            gridEmpleados.DataSource = dtEmpleados;
            gridEmpleados.DataBind();
        }
        private DataTable ObtenerDepartamentosDesdeBaseDeDatos(string connectionString)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idDepartamento, nombreDepartamento FROM Departamento ORDER BY nombreDepartamento";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            foreach (DataRow row in dt.Rows)
            {
                System.Diagnostics.Debug.WriteLine("Departamento: id = " + row["idDepartamento"] + ", nombre = " + row["nombreDepartamento"]);
            }

            return dt;
        }



        private DataTable ObtenerEmpleadosPorDepartamentoDesdeBaseDeDatos(int idDepartamento)
        {
            DataTable dt = new DataTable();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT Nombre, Apellidos, Cedula, NombrePuesto, Estado " +
                               "FROM Empleado " +
                               "INNER JOIN PuestoTrabajo ON Empleado.idPuesto = PuestoTrabajo.idPuesto " +
                               "WHERE idDepartamento = @idDepartamento";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
       
 }   
}