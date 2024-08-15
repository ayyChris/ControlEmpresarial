using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Incapacidades
{
    public partial class RespuestaIncapacidad : System.Web.UI.Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idIncapacidad = Request.QueryString["idIncapacidad"];
                if (!string.IsNullOrEmpty(idIncapacidad))
                {
                    CargarDetalleSolicitud(idIncapacidad);
                }
            }
        }

        private void CargarDetalleSolicitud(string idIncapacidad)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();

                    // Consulta para obtener detalles de la incapacidad y el idEmpleado
                    string query = @"
                SELECT 
                    i.idEmpleado, 
                    i.FechaInicial, 
                    i.FechaFinal, 
                    i.Evidencia, 
                    i.Estado
                FROM 
                    Incapacidades i
                WHERE 
                    i.idIncapacidad = @idIncapacidad";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idIncapacidad", idIncapacidad);

                        using (MySqlDataReader lector = cmd.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                int idEmpleado = Convert.ToInt32(lector["idEmpleado"]);
                                lblFechaInicial.Text = Convert.ToDateTime(lector["FechaInicial"]).ToString("dd/MM/yyyy");
                                lblFechaFinal.Text = Convert.ToDateTime(lector["FechaFinal"]).ToString("dd/MM/yyyy");
                                lblEvidencia.Text = lector["Evidencia"].ToString();
                                lblEstado.Text = lector["Estado"].ToString();

                                // Cargar el nombre completo del empleado
                                CargarNombreEmpleado(idEmpleado);
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

        private void CargarNombreEmpleado(int idEmpleado)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    // Consulta para obtener el nombre y apellidos del empleado
                    string query = "SELECT Nombre, Apellidos FROM Empleado WHERE idEmpleado = @idEmpleado";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                        using (MySqlDataReader lector = cmd.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                string nombre = lector["Nombre"].ToString();
                                string apellidos = lector["Apellidos"].ToString();
                                lblEmpleado.Text = $"{nombre} {apellidos}";
                            }
                            else
                            {
                                lblEmpleado.Text = "Nombre no encontrado";
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
            ActualizarEstadoSolicitud("Denegada");
        }

        private void ActualizarEstadoSolicitud(string nuevoEstado)
        {
            string idIncapacidad = Request.QueryString["idIncapacidad"];
            if (!string.IsNullOrEmpty(idIncapacidad))
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    try
                    {
                        conexion.Open();
                        string query = "UPDATE Incapacidades SET Estado = @nuevoEstado WHERE idIncapacidad = @idIncapacidad";
                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                            cmd.Parameters.AddWithValue("@idIncapacidad", idIncapacidad);
                            cmd.ExecuteNonQuery();
                        }

                        AgregarResultadoPermiso(idIncapacidad, nuevoEstado);
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: '{ex.Message}', icon: 'error', timer: 1500, showConfirmButton: false }});", true);
                    }
                }
            }
        }



        private void AgregarResultadoPermiso(string idIncapacidad, string nuevoEstado)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = @"
                INSERT INTO RespuestaIncapacidades (idIncapacidad, FechaSolicitud, FechaInicial, FechaFinal, Evidencia, Estado, idEmpleado)
                SELECT idIncapacidad, NOW(), FechaInicial, FechaFinal, Evidencia, @nuevoEstado, idEmpleado
                FROM Incapacidades
                WHERE idIncapacidad = @idIncapacidad";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                        cmd.Parameters.AddWithValue("@idIncapacidad", idIncapacidad);
                        cmd.ExecuteNonQuery();
                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Éxito', text: 'La solicitud ha sido {nuevoEstado.ToLower()}da.', icon: 'success', timer: 1500, showConfirmButton: false }});", true);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: '{ex.Message}', icon: 'error', timer: 1500, showConfirmButton: false }});", true);
                }
            }
        }

    }
}