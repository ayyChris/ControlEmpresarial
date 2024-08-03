using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Permisos
{
    public partial class AceptacionNegacionPermisoJefe : Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idPermiso = Request.QueryString["idPermiso"];
                if (!string.IsNullOrEmpty(idPermiso))
                {
                    CargarDetallePermiso(idPermiso);
                }
            }
        }

        private void CargarDetallePermiso(string idPermiso)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT idEmpleado, FechaPublicada, FechaDeseadaInicial, FechaDeseadaFinal, Tipo, Motivo FROM solicitudpermiso WHERE idPermiso = @idPermiso";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idPermiso", idPermiso);

                        using (MySqlDataReader lector = cmd.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                lblEmpleado.Text = lector["idEmpleado"].ToString();
                                lblFechaSolicitud.Text = Convert.ToDateTime(lector["FechaPublicada"]).ToString("dd/MM/yyyy");
                                lblInicioPermiso.Text = Convert.ToDateTime(lector["FechaDeseadaInicial"]).ToString("dd/MM/yyyy");
                                lblFinalPermiso.Text = Convert.ToDateTime(lector["FechaDeseadaFinal"]).ToString("dd/MM/yyyy");
                                lblTipo.Text = lector["Tipo"].ToString();
                                lblMotivo.Text = lector["Motivo"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: '{ex.Message}', icon: 'error', timer: 1500, showConfirmButton: false }});", true);
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            ActualizarEstadoPermiso("Aceptada");
        }

        protected void btnDenegar_Click(object sender, EventArgs e)
        {
            ActualizarEstadoPermiso("Denegada");
        }

        private void ActualizarEstadoPermiso(string nuevoEstado)
        {
            string idPermiso = Request.QueryString["idPermiso"];
            if (!string.IsNullOrEmpty(idPermiso))
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    try
                    {
                        conexion.Open();
                        string query = "UPDATE solicitudpermiso SET Estado = @nuevoEstado WHERE idPermiso = @idPermiso";
                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                            cmd.Parameters.AddWithValue("@idPermiso", idPermiso);
                            cmd.ExecuteNonQuery();
                        }

                        AgregarResultadoPermiso(idPermiso, nuevoEstado);
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: '{ex.Message}', icon: 'error', timer: 1500, showConfirmButton: false }});", true);
                    }
                }
            }
        }

        private void AgregarResultadoPermiso(string idPermiso, string nuevoEstado)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = @"
                        INSERT INTO resultadopermiso (idEmpleado, FechaPermiso, FechaDeseadaInicial, FichaDeseadaFinal, Estado, idSolicitudPermiso)
                        SELECT idEmpleado, FechaPublicada, FechaDeseadaInicial, FechaDeseadaFinal, @nuevoEstado, idPermiso
                        FROM solicitudpermiso
                        WHERE idPermiso = @idPermiso";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                        cmd.Parameters.AddWithValue("@idPermiso", idPermiso);
                        cmd.ExecuteNonQuery();
                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Éxito', text: 'El permiso ha sido {nuevoEstado.ToLower()}do.', icon: 'success', timer: 1500, showConfirmButton: false }});", true);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: '{ex.Message}', icon: 'error', timer: 1500, showConfirmButton: false }});", true);
                }
            }
        }
    }
}
