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

namespace ControlEmpresarial.Vistas.Incapacidades
{
    public partial class TablaPreviaIncapacidades : System.Web.UI.Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie cookie = Request.Cookies["UserInfo"];
                if (cookie != null)
                {
                    // Extraer el valor de idEmpleado
                    string idEmpleadoValue = ConseguirCookie(cookie.Value, "idEmpleado");
                    string idDepartamentoValue = ConseguirCookie(cookie.Value, "idDepartamento");

                    // Asegúrate de que idDepartamentoValue no sea null y conviértelo a entero
                    if (idDepartamentoValue != null && int.TryParse(idDepartamentoValue, out int idDepartamento))
                    {
                        CargarSolicitudes(idDepartamento);
                    }
                    else
                    {
                        lblMensaje.Text = "Error: ID de departamento no válido.";
                        lblMensaje.Visible = true;
                    }
                }
                else
                {
                    lblMensaje.Text = "Error: Cookie no encontrada.";
                    lblMensaje.Visible = true;
                }
            }
        }


        private void CargarSolicitudes(int idDepartamento)
        {
            string query = @"
        SELECT 
            i.idIncapacidad, 
            i.idEmpleado, 
            i.FechaInicial, 
            i.FechaFinal, 
            i.Evidencia, 
            i.Estado,
            t.Nombre AS TipoIncapacidad
        FROM 
            Incapacidades i
        LEFT JOIN 
            TiposIncapacidad t ON i.idTipoIncapacidad = t.idTipoIncapacidad
        WHERE 
            i.idDepartamento = @idDepartamento";

            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        // Añadir el parámetro para el filtro por idDepartamento
                        cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);

                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gridIncapacidades.DataSource = dt;
                                gridIncapacidades.DataBind();
                            }
                            else
                            {
                                dt.Rows.Add(dt.NewRow());
                                gridIncapacidades.DataSource = dt;
                                gridIncapacidades.DataBind();
                                gridIncapacidades.Rows[0].Cells.Clear();
                                gridIncapacidades.Rows[0].Cells.Add(new TableCell { ColumnSpan = dt.Columns.Count, Text = "No se encontraron registros." });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    System.Diagnostics.Debug.WriteLine("Error al cargar las solicitudes: " + ex.Message);
                    lblMensaje.Text = "Error al cargar las solicitudes. Inténtelo de nuevo.";
                    lblMensaje.Visible = true;
                }
            }
        }


        protected void gridIncapacidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VerDetalle")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridIncapacidades.Rows[index];
                string idPermiso = row.Cells[0].Text;
                Response.Redirect("RespuestaIncapacidad.aspx?idIncapacidad=" + idPermiso);
            }
        }

        protected void gridIncapacidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text).ToString("yyyy-MM-dd");
                e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString("yyyy-MM-dd");
            }
        }

        private string ConseguirCookie(string cookieString, string key)
        {
            // Divide la cadena de cookie en pares clave-valor
            var pairs = cookieString.Split('&');
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2 && keyValue[0] == key)
                {
                    return keyValue[1];
                }
            }
            return null;
        }


    }
}