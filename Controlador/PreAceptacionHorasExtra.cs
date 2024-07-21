using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Horas_Extra
{
    public partial class PreAceptacionHorasExtra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarNombreUsuario();
                HttpCookie userCookie = Request.Cookies["UserInfo"];
                if (userCookie != null)
                {
                    // Obtener idEmpleado de la cookie
                    int idEmpleado = int.Parse(userCookie["idEmpleado"]);

                    // Usar idEmpleado para cargar datos
                    CargarDatosTabla(idEmpleado);
                }
                else
                {
                    // Manejar el caso en el que la cookie no existe
                    lblMensaje.Text = "No se ha encontrado la cookie de usuario.";
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                    lblMensaje.Visible = true;
                }
            }
        }

        private void CargarDatosTabla(int idEmpleado)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            DataTable dtHorasExtras = ObtenerDatosBD(connectionString, idEmpleado);

            // Vincular datos al GridView
            gridViewHorasExtra.DataSource = dtHorasExtras;
            gridViewHorasExtra.DataBind();
        }

        private DataTable ObtenerDatosBD(string connectionString, int idEmpleado)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idSolicitud, FechaInicioSolicitud, FechaFinalSolicitud, HoraInicialExtra, HoraFinalExtra, HorasSolicitadas, Motivo " +
                               "FROM solicitudhorasextras " +
                               "WHERE idEmpleado = @idEmpleado AND Estado = 'Activo'";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        protected void gridViewHorasExtra_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Accion")
            {
                // Obtener el idSolicitud de la fila actual
                string idSolicitud = e.CommandArgument.ToString();

                // Construir la URL dinámica
                string url = ResolveUrl("~/Vistas/Horas Extra/RespuestaHorasExtraColaborador.aspx");

                // Redirigir a la nueva página pasando el idSolicitud como parámetro en la URL
                Response.Redirect(url + "?idSolicitud=" + idSolicitud);
            }
        }
        private void CargarNombreUsuario()
        {
            // Obtener el nombre de las cookies
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null)
            {
                string nombre = cookie["Nombre"];
                string apellidos = cookie["Apellidos"];
                lblNombre.Text = nombre + " " + apellidos;
                lblNombre.Visible = true;
            }
            else
            {
                lblNombre.Text = "Error";
                lblNombre.Visible = true;
            }
        }

    }
}
