using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Permisos
{
    public partial class VisualizarPermisosColaborador : System.Web.UI.Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPermisos();
            }
        }

        private void CargarPermisos()
        {
            // Obtener el idEmpleado de la cookie
            HttpCookie userInfoCookie = Request.Cookies["userInfo"];
            if (userInfoCookie != null)
            {
                string idEmpleado = userInfoCookie["idEmpleado"];

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    try
                    {
                        conexion.Open();
                        string query = @"
                            SELECT idPermiso, idEmpleado, FechaPublicada, FechaDeseadaInicial, FechaDeseadaFinal, Tipo, Estado, Motivo 
                            FROM solicitudpermiso 
                            WHERE idEmpleado = @idEmpleado";

                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);

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
                                    gvPermisos.Rows[0].Cells.Add(new TableCell
                                    {
                                        ColumnSpan = dt.Columns.Count,
                                        Text = "No se encontraron registros."
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        // Puedes agregar un mensaje de error aquí si es necesario
                    }
                }
            }
            else
            {
                // Manejo del caso en que la cookie no esté presente
                // Puedes agregar un mensaje de error aquí si es necesario
            }
        }
    }
}