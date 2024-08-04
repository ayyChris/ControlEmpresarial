using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Services
{
    public partial class ControlActividadesJefatura : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Obtener idEmpleado de la cookie
                string idEmpleado = GetIdEmpleadoFromCookie();
                Label1.Text = "idEmpleado obtenido de la cookie: " + idEmpleado; // Debug line

                if (!string.IsNullOrEmpty(idEmpleado))
                {
                    int idDepartamento = ObtenerIdDepartamento(idEmpleado);

                    // Verificar si idDepartamento se ha obtenido correctamente
                    Label1.Text += " | idDepartamento: " + idDepartamento; // Debug line

                    // Cargar empleados del mismo departamento
                    CargarEmpleados(idDepartamento);

                    // Contadores de actividades al cargar la página por primera vez
                    ContarYMostrarActividades();
                }
                else
                {
                    Label1.Text = "idEmpleado no se pudo obtener de la cookie.";
                }
            }
        }



        protected void colaborador_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Contadores de actividades cuando se selecciona un empleado diferente
            ContarYMostrarActividades();
        }

        private void ContarYMostrarActividades()
        {
            try
            {
                string idEnviadorSeleccionado = colaborador.SelectedValue;

                if (string.IsNullOrEmpty(idEnviadorSeleccionado) || idEnviadorSeleccionado == "0")
                {
                    // No hay colaborador seleccionado
                    PendientesDia.Text = "0";
                    RealizadasDia.Text = "0";
                    DenegadasDia.Text = "0";
                    PendientesQuincena.Text = "0";
                    RealizadasQuincena.Text = "0";
                    DenegadasQuincena.Text = "0";
                    return;
                }

                int pendientesDia = ContarActividadesPorEstadoYPeriodo("Pendiente", "DIA", idEnviadorSeleccionado);
                int aceptadasDia = ContarActividadesPorEstadoYPeriodo("Aceptada", "DIA", idEnviadorSeleccionado);
                int denegadasDia = ContarActividadesPorEstadoYPeriodo("Denegada", "DIA", idEnviadorSeleccionado);

                int pendientesQuincena = ContarActividadesPorEstadoYPeriodo("Pendiente", "QUINCENA", idEnviadorSeleccionado);
                int aceptadasQuincena = ContarActividadesPorEstadoYPeriodo("Aceptada", "QUINCENA", idEnviadorSeleccionado);
                int denegadasQuincena = ContarActividadesPorEstadoYPeriodo("Denegada", "QUINCENA", idEnviadorSeleccionado);

                PendientesDia.Text = pendientesDia.ToString();
                RealizadasDia.Text = aceptadasDia.ToString();
                DenegadasDia.Text = denegadasDia.ToString();

                PendientesQuincena.Text = pendientesQuincena.ToString();
                RealizadasQuincena.Text = aceptadasQuincena.ToString();
                DenegadasQuincena.Text = denegadasQuincena.ToString();
            }
            catch (Exception ex)
            {
                Label1.Text = "Error: " + ex.Message;
            }
        }

        private void CargarEmpleados(int idDepartamento)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
                DataTable dtEmpleados = ObtenerColaboradores(connectionString, idDepartamento);

                if (dtEmpleados != null && dtEmpleados.Rows.Count > 0)
                {
                    colaborador.DataSource = dtEmpleados;
                    colaborador.DataTextField = "Nombre";
                    colaborador.DataValueField = "idEmpleado";
                    colaborador.DataBind();

                    colaborador.Items.Insert(0, new ListItem("-- Seleccione un colaborador --", "0"));
                }
                else
                {
                    Label1.Text = "No se encontraron colaboradores.";
                }
            }
            catch (Exception ex)
            {
                Label1.Text = "Error al cargar empleados: " + ex.Message;
            }
        }

        private DataTable ObtenerColaboradores(string connectionString, int idDepartamento)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idEmpleado, Nombre FROM empleado WHERE idDepartamento = @idDepartamento";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);

                try
                {
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);

                    // Debug line
                    Label1.Text += " | Número de empleados obtenidos: " + dt.Rows.Count;
                }
                catch (Exception ex)
                {
                    Label1.Text = "Error en la consulta: " + ex.Message;
                }
            }
            return dt;
        }



        private int ContarActividadesPorEstadoYPeriodo(string estado, string periodo, string idEnviador)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            string query = "";

            if (periodo == "DIA")
            {
                query = "SELECT COUNT(*) FROM actividadesregistradas WHERE estado = @estado AND DATE(fecha) = CURDATE() AND idEnviador = @idEnviador";
            }
            else if (periodo == "QUINCENA")
            {
                query = "SELECT COUNT(*) FROM actividadesregistradas WHERE estado = @estado AND DAY(fecha) BETWEEN 1 AND 15 AND MONTH(fecha) = MONTH(CURDATE()) AND YEAR(fecha) = YEAR(CURDATE()) AND idEnviador = @idEnviador";
            }
            else
            {
                // Periodo inválido
                throw new ArgumentException("Periodo inválido");
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@estado", estado);
                command.Parameters.AddWithValue("@idEnviador", idEnviador);

                try
                {
                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    // Mostrar error en Label1
                    Label1.Text = "Error al contar actividades: " + ex.Message;
                    return 0;
                }
            }
        }

        private string GetIdEmpleadoFromCookie()
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEmpleado))
            {
                return idEmpleado.ToString();
            }
            return null; // Si no se puede obtener el idEmpleado, devolvemos null
        }


        private int ObtenerIdDepartamento(string idEmpleado)
        {
            int idDepartamento = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT idDepartamento FROM empleado WHERE idEmpleado = @idEmpleado";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    idDepartamento = result != null ? Convert.ToInt32(result) : 0;
                }
                catch (Exception ex)
                {
                    // Mostrar error en Label1
                    Label1.Text = "Error al obtener idDepartamento: " + ex.Message;
                }
            }
            return idDepartamento;
        }
    }
}
