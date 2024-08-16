using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Rebajos
{
    public partial class AceptacionNegacionRebajoSupervisor : System.Web.UI.Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idRebajo = Request.QueryString["idRebajo"];
                if (!string.IsNullOrEmpty(idRebajo))
                {
                    CargarDetalleRebajo(idRebajo);
                }
            }
        }

        private void CargarDetalleRebajo(string idRebajo)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT idEmpleado, Fecha, Motivo, Monto FROM rebajo WHERE idRebajo = @idRebajo", conexion);
                cmd.Parameters.AddWithValue("@idRebajo", idRebajo);
                conexion.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblEmpleado.Text = reader["idEmpleado"].ToString();
                    lblFechaSolicitud.Text = reader["Fecha"].ToString();
                    lblMotivo.Text = reader["Motivo"].ToString();
                    lblPorcentajeRebajo.Text = reader["Monto"].ToString();
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            string idRebajo = Request.QueryString["idRebajo"];
            ActualizarEstadoRebajo(idRebajo, "Aceptado");
            Response.Redirect("PreVisualizacionRebajosSupervisor.aspx");
        }

        protected void btnDenegar_Click(object sender, EventArgs e)
        {
            string idRebajo = Request.QueryString["idRebajo"];
            ActualizarEstadoRebajo(idRebajo, "Denegado");
            Response.Redirect("PreVisualizacionRebajosSupervisor.aspx");
        }

        private void ActualizarEstadoRebajo(string idRebajo, string nuevoEstado)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE rebajo SET Estado = @Estado WHERE idRebajo = @idRebajo", conexion);
                cmd.Parameters.AddWithValue("@Estado", nuevoEstado);
                cmd.Parameters.AddWithValue("@idRebajo", idRebajo);
                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}