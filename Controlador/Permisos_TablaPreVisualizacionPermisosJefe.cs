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
    public partial class TablaPreVisualizacionPermisosJefe : Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarPermisos();
        }

        private void CargarPermisos()
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT idPermiso, idEmpleado, FechaPublicada, FechaDeseadaInicial, FechaDeseadaFinal, Tipo, Motivo FROM solicitudpermiso WHERE Estado = 'Pendiente'";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvPermisos.DataSource = dt;
                                gvPermisos.DataBind();
                            }
                            else
                            {
                                dt.Rows.Add(dt.NewRow());
                                gvPermisos.DataSource = dt;
                                gvPermisos.DataBind();
                                gvPermisos.Rows[0].Cells.Clear();
                                gvPermisos.Rows[0].Cells.Add(new TableCell { ColumnSpan = dt.Columns.Count, Text = "No se encontraron registros." });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                }
            }
        }

        protected void gvPermisos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VerDetalle")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvPermisos.Rows[index];
                string idPermiso = row.Cells[0].Text;
                Response.Redirect("AceptacionNegacionPermisoJefe.aspx?idPermiso=" + idPermiso);
            }
        }
    }
}
