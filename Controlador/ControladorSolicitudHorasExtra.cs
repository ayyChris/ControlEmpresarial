using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace ControlEmpresarial.Vistas
{
    public partial class solicitudHorasExtras : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEmpleados();
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            string idEmpleado = colaborador.SelectedItem.Value;
            string colaboradorSeleccionado = colaborador.SelectedItem.Text;
            string dia = this.dia.Text;
            string horaInicio = this.horaInicio.Text;
            string horaFinal = this.horaFinal.Text;
            string motivo = this.motivo.Text;
            DateTime thisDay = DateTime.Today;

            // Convertimos las horas a enteros
            int horaInicioInt = Convert.ToInt32(horaInicio);
            int horaFinalInt = Convert.ToInt32(horaFinal);

            // Calculamos las horas solicitadas
            int horasTotales = horaFinalInt - horaInicioInt;

            string Activo = "Activo";

            // Llama al método para insertar los datos en la base de datos
            InsertarHoraExtra(idEmpleado, colaboradorSeleccionado, thisDay, dia, horaInicioInt, horaFinalInt, horasTotales, motivo, Activo);

            string mensaje = $"Colaborador: {colaboradorSeleccionado}<br />IDEmpleado: {idEmpleado}<br />Dia solicitud: {thisDay}<br />Día: {dia}<br />Hora inicio: {horaInicio}<br />Hora final: {horaFinal}<br />Motivo: {motivo}";
            string mensajeEnviado = "Horas Extra enviadas correctamente!";
            // Mostrar el mensaje en el Label

            lblMensaje.Text = mensajeEnviado;
            lblMensaje.Visible = true;
        }

        private void CargarEmpleados()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            DataTable dtEmpleados = ObtenerColaboradores(connectionString);

            colaborador.DataSource = dtEmpleados;
            colaborador.DataTextField = "Nombre";
            colaborador.DataValueField = "idEmpleado";
            colaborador.DataBind();

            // Add a default item at the beginning
            colaborador.Items.Insert(0, new ListItem("-- Seleccione un colaborador --", "0"));
        }

        private DataTable ObtenerColaboradores(string connectionString)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idEmpleado, Nombre FROM empleado";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        private void InsertarHoraExtra(string idEmpleado, string colaborador, DateTime FechaInicio, string dia, int horaInicio, int horaFinal, int horasTotales, string motivo, string estado)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO solicitudHorasExtras (idEmpleado, NombreEmpleado, FechaInicioSolicitud, FechaFinalSolicitud, HoraInicialExtra, HoraFinalExtra, HorasSolicitadas, Motivo, Estado) " +
                               "VALUES (@idEmpleado, @NombreEmpleado, @FechaInicioSolicitud, @FechaFinalSolicitud, @HoraInicialExtra, @HoraFinalExtra, @HorasSolicitadas, @Motivo, @Estado)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                cmd.Parameters.AddWithValue("@NombreEmpleado", colaborador);
                cmd.Parameters.AddWithValue("@FechaInicioSolicitud", FechaInicio);
                cmd.Parameters.AddWithValue("@FechaFinalSolicitud", Convert.ToDateTime(dia));
                cmd.Parameters.AddWithValue("@HoraInicialExtra", horaInicio);
                cmd.Parameters.AddWithValue("@HoraFinalExtra", horaFinal);
                cmd.Parameters.AddWithValue("@HorasSolicitadas", horasTotales);
                cmd.Parameters.AddWithValue("@Motivo", motivo);
                cmd.Parameters.AddWithValue("@Estado", estado);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
