using ControlEmpresarial.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using MySql.Data.MySqlClient;
using System.Web;

namespace ControlEmpresarial.Vistas
{
    public partial class solicitudHorasExtras : System.Web.UI.Page
    {
        private NotificacionService notificacionService = new NotificacionService();

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

            // Convertimos las horas a TimeSpan
            TimeSpan horaInicioTS = TimeSpan.Parse(horaInicio);
            TimeSpan horaFinalTS = TimeSpan.Parse(horaFinal);

            // Calculamos las horas solicitadas
            double horasTotales = (horaFinalTS - horaInicioTS).TotalHours;

            string Activo = "Activo";

            // Llama al método para insertar los datos en la base de datos
            InsertarHoraExtra(idEmpleado, colaboradorSeleccionado, thisDay, dia, horaInicioTS, horaFinalTS, horasTotales, motivo, Activo);
            string mensajeEnviado = "Horas Extra enviadas correctamente!";
            // Mostrar el mensaje en el Label
            lblMensaje.Text = mensajeEnviado;
            lblMensaje.Visible = true;

            // Insertar la notificación
            InsertarNotificacion(Convert.ToInt32(idEmpleado), "Hora Extra Solicitada", motivo, thisDay);

            // Script para mostrar el ícono
            ClientScript.RegisterStartupScript(this.GetType(), "showLikeIcon", "<script>document.getElementById('likeIcon').style.display = 'inline-block';</script>");
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

        private void InsertarHoraExtra(string idEmpleado, string colaborador, DateTime FechaInicio, string dia, TimeSpan horaInicio, TimeSpan horaFinal, double horasTotales, string motivo, string estado)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            // Obtener idEnviador de la cookie
            HttpCookie userCookie = HttpContext.Current.Request.Cookies["UserInfo"];
            if (userCookie == null || !int.TryParse(userCookie["idEmpleado"], out int idEnviador))
            {
                throw new InvalidOperationException("No se pudo obtener el idEnviador de la cookie.");
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"
            INSERT INTO solicitudHorasExtras (idEmpleado, NombreEmpleado, FechaInicioSolicitud, FechaFinalSolicitud, HoraInicialExtra, HoraFinalExtra, HorasSolicitadas, Motivo, Estado, idEnviador) 
            VALUES (@idEmpleado, @NombreEmpleado, @FechaInicioSolicitud, @FechaFinalSolicitud, @HoraInicialExtra, @HoraFinalExtra, @HorasSolicitadas, @Motivo, @Estado, @idEnviador)";

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
                cmd.Parameters.AddWithValue("@idEnviador", idEnviador);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void InsertarNotificacion(int idEmpleado, string titulo, string motivo, DateTime fecha)
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            int idEnviador = Convert.ToInt32(cookie["idEmpleado"]);

            notificacionService.InsertarNotificacion(idEnviador, idEmpleado, titulo, motivo, fecha);
        }
    }
}
