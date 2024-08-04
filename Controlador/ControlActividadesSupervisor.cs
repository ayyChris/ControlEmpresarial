using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Control_de_Actividades
{
    public partial class ControlActividadesSupervisor : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDepartamentos();
            }
        }

        protected void departamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Contadores de actividades cuando se selecciona un departamento diferente
            ContarYMostrarActividades();
        }

        private void ContarYMostrarActividades()
        {
            try
            {
                string idDepartamentoSeleccionado = departamento.SelectedValue;

                if (string.IsNullOrEmpty(idDepartamentoSeleccionado) || idDepartamentoSeleccionado == "")
                {
                    // No hay departamento seleccionado
                    PendientesDia.Text = "0";
                    RealizadasDia.Text = "0";
                    DenegadasDia.Text = "0";
                    PendientesQuincena.Text = "0";
                    RealizadasQuincena.Text = "0";
                    DenegadasQuincena.Text = "0";
                    return;
                }

                int pendientesDia = ContarActividadesPorEstadoYPeriodo("Pendiente", "DIA", idDepartamentoSeleccionado);
                int aceptadasDia = ContarActividadesPorEstadoYPeriodo("Aceptada", "DIA", idDepartamentoSeleccionado);
                int denegadasDia = ContarActividadesPorEstadoYPeriodo("Denegada", "DIA", idDepartamentoSeleccionado);

                int pendientesQuincena = ContarActividadesPorEstadoYPeriodo("Pendiente", "QUINCENA", idDepartamentoSeleccionado);
                int aceptadasQuincena = ContarActividadesPorEstadoYPeriodo("Aceptada", "QUINCENA", idDepartamentoSeleccionado);
                int denegadasQuincena = ContarActividadesPorEstadoYPeriodo("Denegada", "QUINCENA", idDepartamentoSeleccionado);

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

        private void CargarDepartamentos()
        {
            DataTable dt = ObtenerDepartamentos(connectionString);

            // Enlazar datos al DropDownList
            departamento.DataSource = dt;
            departamento.DataTextField = "nombreDepartamento"; // Nombre del departamento a mostrar
            departamento.DataValueField = "idDepartamento";    // Valor asociado con cada opción
            departamento.DataBind();

            // Añadir un elemento predeterminado opcional
            departamento.Items.Insert(0, new ListItem("Selecciona un departamento", ""));
        }

        private DataTable ObtenerDepartamentos(string connectionString)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idDepartamento, nombreDepartamento FROM departamento";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);

                try
                {
                    conn.Open();
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    throw new Exception("Error al obtener departamentos: " + ex.Message);
                }
            }
            return dt;
        }

        private int ContarActividadesPorEstadoYPeriodo(string estado, string periodo, string idDepartamento)
        {
            string query = "";

            if (periodo == "DIA")
            {
                query = "SELECT COUNT(*) FROM actividadesregistradas WHERE estado = @estado AND DATE(fecha) = CURDATE() AND idDepartamento = @idDepartamento";
            }
            else if (periodo == "QUINCENA")
            {
                query = "SELECT COUNT(*) FROM actividadesregistradas WHERE estado = @estado AND DAY(fecha) BETWEEN 1 AND 15 AND MONTH(fecha) = MONTH(CURDATE()) AND YEAR(fecha) = YEAR(CURDATE()) AND idDepartamento = @idDepartamento";
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
                command.Parameters.AddWithValue("@idDepartamento", idDepartamento);

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
    }
}
