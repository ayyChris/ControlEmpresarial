using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Reposicion
{
    public partial class historicoReposicionesColaborador : System.Web.UI.Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarReposiciones();
        }

        private void CargarReposiciones()
        {
            // Obtener el ID del empleado desde la cookie
            int idEmpleado;
            HttpCookie cookie = Request.Cookies["userInfo"];
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out idEmpleado))
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    string consulta = @"
                        SELECT idReposicion, idEmpleado, Fecha, Estado, idInconsistencia, 
                               FechaInicialReposicion, FechaFinalReposicion, 
                               HoraInicialReposicion, HoraFinalReposicion
                        FROM solicitudreposicion
                        WHERE idEmpleado = @idEmpleado";

                    using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                        try
                        {
                            conexion.Open();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            // Asignar los datos al GridView
                            gvReposiciones.DataSource = dt;
                            gvReposiciones.DataBind();
                        }
                        catch (Exception ex)
                        {
                            // Manejo de excepciones si ocurre un problema al consultar los datos
                            ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"Swal.fire('Error', 'No se pudo cargar los datos. {ex.Message}', 'error');", true);
                        }
                    }
                }
            }
            else
            {
                // Manejo si la cookie no está disponible o el ID no es válido
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Error', 'No se pudo obtener el ID del empleado.', 'error');", true);
            }
        }
    }
}