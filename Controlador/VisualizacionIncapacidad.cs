using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas
{
    public partial class VisualizacionIncapacidad : Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarHistorialIncapacidades();
            }
        }

        private void CargarHistorialIncapacidades()
        {
            // Obtener el ID del empleado desde la cookie
            HttpCookie cookie = Request.Cookies["userInfo"];
            if (cookie != null)
            {
                string empleadoId = cookie["idEmpleado"];

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    // Consulta SQL para unir la tabla de incapacidades con la tabla de tipos de incapacidad
                    string consulta = @"
                SELECT 
                    i.idIncapacidad, 
                    i.idEmpleado, 
                    t.Nombre AS TipoIncapacidad,  -- Obtener el nombre del tipo de incapacidad
                    i.FechaInicial, 
                    i.FechaFinal, 
                    i.Evidencia, 
                    i.DiasReduccion, 
                    i.Estado 
                FROM 
                    Incapacidades i
                JOIN 
                    TiposIncapacidad t ON i.idTipoIncapacidad = t.idTipoIncapacidad
                WHERE 
                    i.idEmpleado = @idEmpleado";

                    MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                    cmd.Parameters.AddWithValue("@idEmpleado", empleadoId);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvIncapacidades.DataSource = dt;
                    gvIncapacidades.DataBind();
                }
            }
        }

    }

}
