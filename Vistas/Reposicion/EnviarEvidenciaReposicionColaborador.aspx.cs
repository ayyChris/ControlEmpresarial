using System;
using System.Data;
using System.Web;
using System.Web.UI;
using MySql.Data.MySqlClient;

namespace ControlEmpresarial.Vistas.Reposicion
{
    public partial class EnviarEvidenciaReposicionColaborador : System.Web.UI.Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idReposicion = Request.QueryString["idReposicion"];
                if (!string.IsNullOrEmpty(idReposicion))
                {
                    CargarInformacionReposicion(idReposicion);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Error', 'No se pudo obtener el ID de la reposición.', 'error');", true);
                }
            }
        }

        private void CargarInformacionReposicion(string idReposicion)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                string consulta = @"
                    SELECT idReposicion, idEmpleado, idInconsistencia, Fecha, Estado, FechaInicialReposicion, HoraInicialReposicion, HoraFinalReposicion
                    FROM solicitudreposicion
                    WHERE idReposicion = @idReposicion";

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
                                lblEmpleado.Text = reader["idReposicion"].ToString();
                                lblFechaSolicitud.Text = reader["idInconsistencia"].ToString();
                                // Puedes agregar más controles si necesitas mostrar más datos
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"Swal.fire('Error', 'No se pudo cargar la información de la reposición. {ex.Message}', 'error');", true);
                    }
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            string idReposicion = lblEmpleado.Text;
            string idInconsistencia = lblFechaSolicitud.Text;
            string evidencia = txtEvidencia.Text;

            HttpCookie cookie = Request.Cookies["userInfo"];
            int idEmpleado;
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out idEmpleado))
            {
                if (ValidarMarcas(idEmpleado, idReposicion))
                {
                    using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                    {
                        string consulta = @"
                    INSERT INTO evidenciareposicion (idReposicion, idInconsistencia, idEmpleado, EnlaceEvidencia, FechaEvidencia, Estado)
                    VALUES (@idReposicion, @idInconsistencia, @idEmpleado, @EnlaceEvidencia, @FechaEvidencia, @Estado);

                    UPDATE solicitudreposicion
                    SET Estado = 'Revision'
                    WHERE idReposicion = @idReposicion;";

                        using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                        {
                            cmd.Parameters.AddWithValue("@idReposicion", idReposicion);
                            cmd.Parameters.AddWithValue("@idInconsistencia", idInconsistencia);
                            cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                            cmd.Parameters.AddWithValue("@EnlaceEvidencia", evidencia);
                            cmd.Parameters.AddWithValue("@FechaEvidencia", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Estado", "Revision");

                            try
                            {
                                conexion.Open();
                                cmd.ExecuteNonQuery();
                                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Éxito', 'La evidencia ha sido enviada correctamente y el estado de la solicitud ha sido actualizado.', 'success');", true);
                                Response.Redirect("visualizarReposicionesColaborador.aspx");
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"Swal.fire('Error', 'No se pudo enviar la evidencia ni actualizar el estado. {ex.Message}', 'error');", true);
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Error', 'No se encontraron registros de entrada y salida que coincidan con la reposición.', 'error');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Error', 'No se pudo obtener el ID del empleado.', 'error');", true);
            }
        }


        private bool ValidarMarcas(int idEmpleado, string idReposicion)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                string consulta = @"
                    SELECT COUNT(*) 
                    FROM entradas
                    WHERE idEmpleado = @idEmpleado 
                    AND DiaMarcado = (
                        SELECT FechaInicialReposicion 
                        FROM solicitudreposicion 
                        WHERE idReposicion = @idReposicion
                    )
                    AND HoraEntrada <= (
                        SELECT HoraInicialReposicion 
                        FROM solicitudreposicion 
                        WHERE idReposicion = @idReposicion
                    )
                    AND HoraSalida >= (
                        SELECT HoraFinalReposicion 
                        FROM solicitudreposicion 
                        WHERE idReposicion = @idReposicion
                    )";

                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@idReposicion", idReposicion);

                    try
                    {
                        conexion.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"Swal.fire('Error', 'No se pudo validar las marcas. {ex.Message}', 'error');", true);
                        return false;
                    }
                }
            }
        }
    }
}
