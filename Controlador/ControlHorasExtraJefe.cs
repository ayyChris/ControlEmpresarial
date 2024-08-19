using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Horas_Extra
{
    public partial class ControlHorasExtraJefe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicializar los Label con valores 0
                HorasExtraSolicitadas.Text = "0";
                HorasExtraAceptadas.Text = "0";
                Evidenciadas.Text = "0";

                // Cargar los colaboradores en el DropDownList
                CargarColaboradores();
            }
        }

        protected void colaborador_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idEmpleado;
            if (int.TryParse(colaborador.SelectedValue, out idEmpleado))
            {
                // Obtener las fechas de la semana actual
                (DateTime inicioSemana, DateTime finSemana) = ObtenerFechasSemanaActual();

                // Obtener los datos de horas extra
                DataTable dtDatos = ObtenerDatosHorasExtra(idEmpleado, inicioSemana, finSemana);

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
                    Label1.Text = "No se encontraron datos para el colaborador seleccionado.";
                }
            }
            else
            {
                Label1.Text = "Error al seleccionar un colaborador.";
            }
        }

        private void CargarColaboradores()
        {
            // Obtener la lista de colaboradores desde la base de datos
            DataTable dtColaboradores = ObtenerColaboradores();
            colaborador.DataSource = dtColaboradores;
            colaborador.DataTextField = "Nombre"; // Ajusta según tu tabla
            colaborador.DataValueField = "idEmpleado"; // Ajusta según tu tabla
            colaborador.DataBind();
            colaborador.Items.Insert(0, new ListItem("Seleccione un colaborador", "0"));
        }

        private DataTable ObtenerColaboradores()
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            // Actualiza la consulta para incluir la cláusula WHERE
            string query = "SELECT idEmpleado, Nombre FROM empleado WHERE idPuesto = @idPuesto";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Añadir el parámetro para la consulta con el valor 2
                    cmd.Parameters.AddWithValue("@idPuesto", 2);

                    try
                    {
                        conn.Open();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        Label1.Text = "Error al obtener colaboradores: " + ex.Message;
                    }
                }
            }
            return dt;
        }



        private (DateTime, DateTime) ObtenerFechasSemanaActual()
        {
            DateTime hoy = DateTime.Today;
            DateTime inicioSemana = hoy.AddDays(-(int)hoy.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime finSemana = inicioSemana.AddDays(6);
            return (inicioSemana, finSemana);
        }

        private DataTable ObtenerDatosHorasExtra(int idEmpleado, DateTime fechaInicioSemana, DateTime fechaFinSemana)
        {
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            string query = @"
        SELECT 
            SUM(s.HorasSolicitadas) AS TotalHorasSolicitadas,
            SUM(CASE WHEN r.Respuesta = 1 THEN s.HorasSolicitadas ELSE 0 END) AS TotalHorasAceptadas,
            SUM(CASE WHEN e.EnlaceEvidencia IS NOT NULL THEN s.HorasSolicitadas ELSE 0 END) AS TotalHorasEvidenciadas
        FROM 
            solicitudHorasExtras s
        LEFT JOIN 
            respuestahorasextras r ON s.idSolicitud = r.idSolicitud
        LEFT JOIN 
            evidenciahorasextras e ON s.idSolicitud = e.idSolicitud
        WHERE 
            s.idEmpleado = @idEmpleado
            AND s.FechaInicioSolicitud >= @fechaInicioSemana
            AND s.FechaFinalSolicitud <= @fechaFinSemana
        GROUP BY 
            s.idEmpleado";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@fechaInicioSemana", fechaInicioSemana.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@fechaFinSemana", fechaFinSemana.ToString("yyyy-MM-dd"));

                    try
                    {
                        conn.Open();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);

                        // Mensaje de depuración para verificar datos
                        if (dt.Rows.Count > 0)
                        {
                            Console.WriteLine("Datos obtenidos correctamente.");
                        }
                        else
                        {
                            Console.WriteLine("No se encontraron datos.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Label1.Text = "Error al obtener datos de horas extra: " + ex.Message;
                    }
                }
            }
            return dt;
        }

    }
}
