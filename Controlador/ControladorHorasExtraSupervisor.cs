using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Horas_Extra
{
    public partial class ControladorHorasExtraSupervisor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicializar los Label con valores 0
                HorasExtraSolicitadas.Text = "0";
                HorasExtraAceptadas.Text = "0";
                Evidenciadas.Text = "0";

                // Cargar los departamentos en el DropDownList
                CargarDepartamentos();
            }
        }
        private DataTable ObtenerDatosHorasExtra(int idDepartamento, DateTime fechaInicioSemana, DateTime fechaFinSemana)
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            string query = @"
        SELECT 
            d.nombreDepartamento AS Departamento,
            SUM(s.HorasSolicitadas) AS TotalHorasSolicitadas,
            SUM(CASE WHEN r.Respuesta = 1 THEN s.HorasSolicitadas ELSE 0 END) AS TotalHorasAceptadas,
            SUM(CASE WHEN e.EnlaceEvidencia IS NOT NULL THEN s.HorasSolicitadas ELSE 0 END) AS TotalHorasEvidenciadas
        FROM 
            solicitudHorasExtras s
        LEFT JOIN 
            respuestahorasextras r ON s.idSolicitud = r.idSolicitud
        LEFT JOIN 
            evidenciahorasextras e ON s.idSolicitud = e.idSolicitud
        INNER JOIN 
            empleado emp ON s.idEmpleado = emp.idEmpleado
        INNER JOIN 
            departamento d ON emp.idDepartamento = d.idDepartamento
        WHERE 
            emp.idDepartamento = @idDepartamento
            AND s.FechaInicioSolicitud >= @fechaInicioSemana
            AND s.FechaFinalSolicitud <= @fechaFinSemana
        GROUP BY 
            d.nombreDepartamento";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);
                    cmd.Parameters.AddWithValue("@fechaInicioSemana", fechaInicioSemana.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@fechaFinSemana", fechaFinSemana.ToString("yyyy-MM-dd"));

                    try
                    {
                        conn.Open();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        Label1.Text = "Error al obtener datos de horas extra: " + ex.Message;
                    }
                }
            }
            return dt;
        }

        protected void Departamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idDepartamento;
            if (int.TryParse(departamento.SelectedValue, out idDepartamento))
            {
                // Obtener las fechas de la semana actual
                (DateTime inicioSemana, DateTime finSemana) = ObtenerFechasSemanaActual();

                // Obtener los datos de horas extra para el departamento
                DataTable dtDatos = ObtenerDatosHorasExtra(idDepartamento, inicioSemana, finSemana);

                // Actualizar los Label con los datos obtenidos
                if (dtDatos.Rows.Count > 0)
                {
                    DataRow row = dtDatos.Rows[0];
                    HorasExtraSolicitadas.Text = row["TotalHorasSolicitadas"].ToString();
                    HorasExtraAceptadas.Text = row["TotalHorasAceptadas"].ToString();
                    Evidenciadas.Text = row["TotalHorasEvidenciadas"].ToString();
                }
                else
                {
                    // Si no hay datos, mostrar mensajes o valores por defecto
                    HorasExtraSolicitadas.Text = "0";
                    HorasExtraAceptadas.Text = "0";
                    Evidenciadas.Text = "0";
                    Label1.Text = "No se encontraron datos para el departamento seleccionado.";
                }
            }
            else
            {
                Label1.Text = "Error al seleccionar un departamento.";
            }
        }

        private void CargarDepartamentos()
        {
            // Obtener la lista de departamentos desde la base de datos
            DataTable dtDepartamentos = ObtenerDepartamentos();
            departamento.DataSource = dtDepartamentos;
            departamento.DataTextField = "nombreDepartamento"; // Ajusta según tu tabla
            departamento.DataValueField = "idDepartamento"; // Ajusta según tu tabla
            departamento.DataBind();
            departamento.Items.Insert(0, new ListItem("Seleccione un departamento", "0"));
        }

        private DataTable ObtenerDepartamentos()
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            string query = "SELECT idDepartamento, nombreDepartamento FROM departamento"; // Ajusta la tabla si es necesario

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        Label1.Text = "Error al obtener departamentos: " + ex.Message;
                    }
                }
            }
            return dt;
        }

        private (DateTime, DateTime) ObtenerFechasSemanaActual()
        {
            DateTime today = DateTime.Today;
            int daysUntilMonday = (int)DayOfWeek.Monday - (int)today.DayOfWeek;
            if (daysUntilMonday > 0)
            {
                daysUntilMonday -= 7; // Para la semana pasada si estamos en fin de semana
            }

            DateTime startOfWeek = today.AddDays(daysUntilMonday);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            return (startOfWeek, endOfWeek);
        }
    }
}
