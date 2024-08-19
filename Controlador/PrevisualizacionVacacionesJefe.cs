using ControlEmpresarial.Controlador;
using ControlEmpresarial.Services;
using ControlEmpresarial.Vistas.Pagina_Principal;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas
{
    public partial class PrevisualizacionVacacionesJefe : Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarSolicitudes();
        }

        private void CargarSolicitudes()
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT idSolicitudVacaciones, idEmpleado, FechaPublicada, FechaVacacion, DiasDisfrutados,Estado " +
                                   "FROM SolicitudVacaciones WHERE Estado = 'Pendiente'";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvVacaciones.DataSource = dt;
                                gvVacaciones.DataBind();
                            }
                            else
                            {
                                dt.Rows.Add(dt.NewRow());
                                gvVacaciones.DataSource = dt;
                                gvVacaciones.DataBind();
                                gvVacaciones.Rows[0].Cells.Clear();
                                gvVacaciones.Rows[0].Cells.Add(new TableCell { ColumnSpan = dt.Columns.Count, Text = "No se encontraron registros." });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    System.Diagnostics.Debug.WriteLine("Error al cargar las solicitudes: " + ex.Message);
                }
            }
        }

        protected void gvVacaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VerDetalle")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvVacaciones.Rows[index];
                string idPermiso = row.Cells[0].Text;
                Response.Redirect("solicitudVacacionesJefatura.aspx?idSolicitud=" + idPermiso);
            }
        }

        protected void gvVacaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            { 
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text).ToString("yyyy-MM-dd");
                e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString("yyyy-MM-dd");
            }
        }

    }
}