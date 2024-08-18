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
    public partial class RevisarSolicitudesReposicionJefe : System.Web.UI.Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarReposiciones();
        }

        private void CargarReposiciones()
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                // Actualiza la consulta para incluir la cláusula WHERE
                string consulta = @"
            SELECT idEvidencia, idReposicion, idInconsistencia, EnlaceEvidencia, FechaEvidencia, Estado
            FROM evidenciareposicion
            WHERE Estado = 'Revision'";

                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                {
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


        protected void gvReposiciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Revisar")
            {
                // Obtener el índice de la fila
                int index = Convert.ToInt32(e.CommandArgument);

                // Obtener el ID de la reposición de la segunda columna (índice 1) de la fila seleccionada
                GridViewRow row = gvReposiciones.Rows[index];
                string idReposicion = row.Cells[1].Text;

                // Redirigir a la nueva página de revisión con el ID de la reposición en la URL
                Response.Redirect($"AceptacionNegacionReposicionJefatura.aspx?idReposicion={idReposicion}");
            }
        }


    }
}