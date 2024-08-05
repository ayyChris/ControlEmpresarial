using ControlEmpresarial.Controlador;
using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Horas_Extra
{
    public partial class PreAceptacionHorasExtra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie userCookie = Request.Cookies["UserInfo"];
                if (userCookie != null)
                {
                    int idEmpleado = int.Parse(userCookie["idEmpleado"]);
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

        protected void gridViewHorasExtra_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Asegúrate de que estamos trabajando con una fila de datos (no el encabezado o pie de página)
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Formatear las fechas
                DateTime fechaInicioSolicitud = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "FechaInicioSolicitud"));
                DateTime fechaFinalSolicitud = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "FechaFinalSolicitud"));
                e.Row.Cells[1].Text = fechaInicioSolicitud.ToString("dd/MM/yyyy");
                e.Row.Cells[2].Text = fechaFinalSolicitud.ToString("dd/MM/yyyy");

                // Formatear las horas
                TimeSpan horaInicialExtra = TimeSpan.Parse(DataBinder.Eval(e.Row.DataItem, "HoraInicialExtra").ToString());
                TimeSpan horaFinalExtra = TimeSpan.Parse(DataBinder.Eval(e.Row.DataItem, "HoraFinalExtra").ToString());
                e.Row.Cells[3].Text = horaInicialExtra.ToString(@"hh\:mm");
                e.Row.Cells[4].Text = horaFinalExtra.ToString(@"hh\:mm");
            }
        }
    }
}
