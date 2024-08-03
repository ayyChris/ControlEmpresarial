using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Vacaciones
{
    public partial class vacacionesFestivas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // Asegúrate de cargar los departamentos solo en la primera carga de la página
            {
                CargarDepartamento();
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            DateTime fechaInicioTexto;
            DateTime fechaFinalTexto;

            fechaInicioTexto = DateTime.MinValue;
            fechaFinalTexto = DateTime.MinValue;

            // Obtener el texto de los TextBox y convertir a DateTime
            bool fechaInicioValida = DateTime.TryParse(fechaInicio.Text, out fechaInicioTexto);
            bool fechaFinalValida = DateTime.TryParse(fechaFinal.Text, out fechaFinalTexto);

            // Validar que las fechas son válidas
            if (!fechaInicioValida || !fechaFinalValida)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ingrese fechas válidas.')", true);
                return;
            }

            // Verificar que la fecha final no sea antes de la fecha de inicio
            if (fechaFinalTexto < fechaInicioTexto)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('La fecha final debe ser posterior a la fecha de inicio.')", true);
                return;
            }

            // Obtener el motivo
            string motivo = Txtmotivo.Text;

            // Obtener el idDepartamento seleccionado
            string idDepartamento = departamento.SelectedValue;
            if (string.IsNullOrEmpty(idDepartamento))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Seleccione un departamento.')", true);
                return;
            }

            // Conexión a la base de datos
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Validar cada fecha en el rango
                DateTime fechaActual = fechaInicioTexto;
                while (fechaActual <= fechaFinalTexto)
                {
                    string checkQuery = @"
                        SELECT COUNT(*) 
                        FROM VacacionesColectivas 
                        WHERE FechaVacacion = @FechaVacacion";

                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@FechaVacacion", fechaActual.ToString("yyyy-MM-dd"));

                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        // Si el conteo es mayor que 0, significa que la fecha ya está en VacacionesColectivas
                        if (count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('La fecha " + fechaActual.ToString("dd/MM/yyyy") + " ya está registrada como vacaciones colectivas. No se puede solicitar en este rango.')", true);
                            return;
                        }
                    }

                    fechaActual = fechaActual.AddDays(1);
                }

                // Insertar cada fecha individual en la base de datos
                fechaActual = fechaInicioTexto;
                while (fechaActual <= fechaFinalTexto)
                {
                    string insertQuery = @"
                        INSERT INTO DiasFestivos (idDepartamento, FechaVacacion, Motivo) 
                        VALUES (@idDepartamento, @FechaVacacion, @Motivo)";

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idDepartamento", idDepartamento);
                        command.Parameters.AddWithValue("@FechaVacacion", fechaActual.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Motivo", motivo);

                        command.ExecuteNonQuery();
                    }

                    fechaActual = fechaActual.AddDays(1);
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Vacaciones registradas adecuadamente.')", true);
            }
        }

        private void CargarDepartamento()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            DataTable dtPuestos = ObtenerDepartamentoDesdeBaseDeDatos(connectionString);

            departamento.DataTextField = "nombreDepartamento";
            departamento.DataValueField = "idDepartamento";
            departamento.DataSource = dtPuestos;
            departamento.DataBind();

            // Agregar elemento por defecto al inicio
            departamento.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        private DataTable ObtenerDepartamentoDesdeBaseDeDatos(string connectionString)
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
            return dt;
        }
    }
}
