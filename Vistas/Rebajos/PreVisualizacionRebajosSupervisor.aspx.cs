using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Rebajos
{
    public partial class PreVisualizacionRebajosSupervisor : System.Web.UI.Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarRebajosPendientes();
            }
        }

        private void CargarRebajosPendientes()
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT idRebajo, idEmpleado, TipoRebajoID, Fecha, Estado, Motivo, Monto FROM rebajo WHERE Estado = 'Pendiente'", conexion);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvRebajos.DataSource = dt;
                gvRebajos.DataBind();
            }
        }

        protected void gvRebajos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VerDetalle")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvRebajos.Rows[index];
                string idRebajo = row.Cells[0].Text;

                // Redirigir a la página de detalles con el idRebajo en la query string
                Response.Redirect($"AceptacionNegacionRebajoSupervisor.aspx?idRebajo={idRebajo}");
            }
        }

    }
}