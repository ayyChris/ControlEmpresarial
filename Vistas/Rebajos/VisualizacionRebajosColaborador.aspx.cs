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
    public partial class VisualizacionRebajosColaborador : System.Web.UI.Page
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
            // Obtener el ID del empleado desde la cookie
            HttpCookie cookie = Request.Cookies["userInfo"];
            if (cookie != null)
            {
                string empleadoId = cookie["idEmpleado"];

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    // Modificar la consulta para filtrar por empleado y estado 'Aceptado'
                    string consulta = "SELECT idRebajo, idEmpleado, TipoRebajoID, Fecha, Estado, Motivo, Monto FROM rebajo WHERE idEmpleado = @idEmpleado AND Estado = 'Aceptado'";
                    MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                    cmd.Parameters.AddWithValue("@idEmpleado", empleadoId);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvRebajos.DataSource = dt;
                    gvRebajos.DataBind();
                }
            }
        }

    }
}