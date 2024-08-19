using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Vacaciones
{
    public partial class SolicitudVacacionesJefatura : Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idSolicitud = Request.QueryString["idSolicitud"];
                if (!string.IsNullOrEmpty(idSolicitud))
                {
                    CargarDetalleSolicitud(idSolicitud);
                }
            }
        }

        private void CargarDetalleSolicitud(string idSolicitud)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    // Ajusta la consulta para que coincida con la nueva estructura
                    string query = "SELECT idEmpleado, FechaPublicada, FechaVacacion, DiasDisfrutados FROM SolicitudVacaciones WHERE idSolicitudVacaciones = @idSolicitud";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idSolicitud", idSolicitud);

                        using (MySqlDataReader lector = cmd.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                lblEmpleado.Text = lector["idEmpleado"].ToString();
                                lblFechaSolicitud.Text = Convert.ToDateTime(lector["FechaPublicada"]).ToString("dd/MM/yyyy");
                                lblFechaVacacion.Text = Convert.ToDateTime(lector["FechaVacacion"]).ToString("dd/MM/yyyy");
                                lblDiasDisfrutar.Text = lector["DiasDisfrutados"].ToString();
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
            ActualizarEstadoSolicitud("Aceptada");
        }

        protected void btnDenegar_Click(object sender, EventArgs e)
        {
            string idEmpleado = lblEmpleado.Text; // Asumiendo que lblEmpleado contiene el idEmpleado
            if (!string.IsNullOrEmpty(idEmpleado))
            {
                AgregarDiaRechazado(idEmpleado);
            }
            ActualizarEstadoSolicitud("Denegada");
        }

        private void ActualizarEstadoSolicitud(string nuevoEstado)
        {
            string idSolicitud = Request.QueryString["idSolicitud"];
            if (!string.IsNullOrEmpty(idSolicitud))
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    try
                    {
                        conexion.Open();
                        string query = "UPDATE SolicitudVacaciones SET Estado = @nuevoEstado WHERE idSolicitudVacaciones = @idSolicitud";
                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                            cmd.Parameters.AddWithValue("@idSolicitud", idSolicitud);
                            cmd.ExecuteNonQuery();
                        }

                        AgregarResultadoPermiso(idSolicitud, nuevoEstado);
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: '{ex.Message}', icon: 'error', timer: 1500, showConfirmButton: false }});", true);
                    }
                }
            }
        }


        private void AgregarResultadoPermiso(string idSolicitud, string nuevoEstado)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = @"
                INSERT INTO RespuestaVacaciones (idSolicitudVacaciones, FechaSolicitud, FechaVacacion, DiasDisfrutados, Estado, idEmpleado)
                SELECT idSolicitudVacaciones, FechaPublicada, FechaVacacion, DiasDisfrutados, @nuevoEstado, idEmpleado
                FROM SolicitudVacaciones
                WHERE idSolicitudVacaciones = @idSolicitud";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                        cmd.Parameters.AddWithValue("@idSolicitud", idSolicitud);
                        cmd.ExecuteNonQuery();
                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", @"
                Swal.fire({
                    title: 'Éxito',
                    text: 'La solicitud ha sido " + nuevoEstado.ToLower() + @".',
                    icon: 'success',
                    showConfirmButton: true
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = 'TablaPreVisualizacionVacaciones.aspx'; // Cambia a la página de destino deseada
                    }
                });
            ", true);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: '{ex.Message}', icon: 'error', timer: 1500, showConfirmButton: false }});", true);
                }
            }
        }

        private void AgregarDiaRechazado(string idEmpleado)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = "UPDATE Empleado SET DiasDeVacaciones = DiasDeVacaciones + 1 WHERE idEmpleado = @idEmpleado";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: '{ex.Message}', icon: 'error', timer: 1500, showConfirmButton: false }});", true);
                }
            }
        }

    }
}