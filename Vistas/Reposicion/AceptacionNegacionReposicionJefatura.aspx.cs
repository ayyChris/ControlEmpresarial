using System;
using System.Web;
using System.Web.UI;
using MySql.Data.MySqlClient;

namespace ControlEmpresarial.Vistas.Reposicion
{
    public partial class AceptacionNegacionReposicionJefatura : System.Web.UI.Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idReposicion = Request.QueryString["idReposicion"];
                if (!string.IsNullOrEmpty(idReposicion))
                {
                    CargarInformacion(idReposicion);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Error', 'No se pudo obtener el ID de la reposición.', 'error');", true);
                }
            }
        }

        private void CargarInformacion(string idReposicion)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                string consulta = @"
                    SELECT sr.idReposicion, sr.idInconsistencia, sr.idEmpleado, er.EnlaceEvidencia, er.FechaEvidencia
                    FROM solicitudreposicion sr
                    JOIN evidenciareposicion er ON sr.idReposicion = er.idReposicion
                    WHERE sr.idReposicion = @idReposicion";

                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@idReposicion", idReposicion);

                    try
                    {
                        conexion.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblReposicion.Text = reader["idReposicion"].ToString();
                                lblInconsisntencia.Text = reader["idInconsistencia"].ToString();
                                lblEmpleado.Text = reader["idEmpleado"].ToString();
                                lblEvidencia.Text = reader["EnlaceEvidencia"].ToString();
                                lblFecha.Text = Convert.ToDateTime(reader["FechaEvidencia"]).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Error', 'No se encontraron datos para la reposición.', 'error');", true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"Swal.fire('Error', 'No se pudo cargar la información. {ex.Message}', 'error');", true);
                    }
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            ActualizarEstado("Aceptada");
        }

        protected void btnDenegar_Click(object sender, EventArgs e)
        {
            ActualizarEstado("Denegada");
        }

        private void ActualizarEstado(string estado)
        {
            string idReposicion = lblReposicion.Text;
            string idEmpleado = lblEmpleado.Text;
            string fechaTrabajo = DateTime.Now.ToString("yyyy-MM-dd");

            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                MySqlTransaction transaction = null;
                try
                {
                    conexion.Open();
                    transaction = conexion.BeginTransaction();

                    // Insertar en resultadoreposicion
                    string consultaInsert = @"
                INSERT INTO resultadoreposicion (idReposicion, idEmpleado, FechaTrabajo, Estado, idInconsistencia)
                VALUES (@idReposicion, @idEmpleado, @FechaTrabajo, @Estado, @idInconsistencia)";

                    using (MySqlCommand cmdInsert = new MySqlCommand(consultaInsert, conexion, transaction))
                    {
                        cmdInsert.Parameters.AddWithValue("@idReposicion", idReposicion);
                        cmdInsert.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                        cmdInsert.Parameters.AddWithValue("@FechaTrabajo", fechaTrabajo);
                        cmdInsert.Parameters.AddWithValue("@Estado", estado);
                        cmdInsert.Parameters.AddWithValue("@idInconsistencia", lblInconsisntencia.Text);

                        cmdInsert.ExecuteNonQuery();
                    }

                    // Actualizar estado en solicitudreposicion
                    string consultaUpdateSolicitud = @"
                UPDATE solicitudreposicion
                SET Estado = @Estado
                WHERE idReposicion = @idReposicion";

                    using (MySqlCommand cmdUpdateSolicitud = new MySqlCommand(consultaUpdateSolicitud, conexion, transaction))
                    {
                        cmdUpdateSolicitud.Parameters.AddWithValue("@Estado", estado);
                        cmdUpdateSolicitud.Parameters.AddWithValue("@idReposicion", idReposicion);

                        cmdUpdateSolicitud.ExecuteNonQuery();
                    }

                    // Actualizar estado en evidenciareposicion
                    string consultaUpdateEvidencia = @"
                UPDATE evidenciareposicion
                SET Estado = @Estado
                WHERE idReposicion = @idReposicion";

                    using (MySqlCommand cmdUpdateEvidencia = new MySqlCommand(consultaUpdateEvidencia, conexion, transaction))
                    {
                        cmdUpdateEvidencia.Parameters.AddWithValue("@Estado", estado);
                        cmdUpdateEvidencia.Parameters.AddWithValue("@idReposicion", idReposicion);

                        cmdUpdateEvidencia.ExecuteNonQuery();
                    }

                    // Commit transaction
                    transaction.Commit();

                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"Swal.fire('Éxito', 'La reposición ha sido {estado.ToLower()} correctamente.', 'success');", true);
                    Response.Redirect("RevisarSolicitudesReposicionJefe.aspx");
                }
                catch (Exception ex)
                {
                    // Rollback transaction on error
                    if (transaction != null)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception rollbackEx)
                        {
                            // Manejo de errores en el rollback
                            ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"Swal.fire('Error', 'No se pudo revertir la transacción. {rollbackEx.Message}', 'error');", true);
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"Swal.fire('Error', 'No se pudo actualizar la reposición. {ex.Message}', 'error');", true);
                }
            }
        }
    }
}
