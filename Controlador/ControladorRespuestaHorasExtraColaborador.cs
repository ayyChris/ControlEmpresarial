using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Horas_Extra
{
    public partial class RespuestaHorasExtra : Page
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        private string idSolicitud;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                idSolicitud = Request.QueryString["idSolicitud"];
                if (!string.IsNullOrEmpty(idSolicitud))
                {
                    // Usar el idSolicitud para cargar datos o realizar acciones
                    lblSolicitudId.Text = "ID de Solicitud: " + idSolicitud;
                    CargarDatos();
                }
            }
        }

        private void CargarDatos()
        {
            DataTable dt = ObtenerDatosBD();
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                lblHorasSolicitadas.Text = "Horas Solicitadas: " + row["HorasSolicitadas"].ToString();
                lblMotivo.Text = "Motivo: " + row["Motivo"].ToString();
                lblHoraInicialExtra.Text = "Hora Inicial Extra: " + row["HoraInicialExtra"].ToString();
                lblHoraFinalExtra.Text = "Hora Final Extra: " + row["HoraFinalExtra"].ToString();
            }
        }

        private DataTable ObtenerDatosBD()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT HorasSolicitadas, Motivo, HoraInicialExtra, HoraFinalExtra " +
                               "FROM solicitudhorasextras " +
                               "WHERE idSolicitud = @idSolicitud";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idSolicitud", idSolicitud);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
