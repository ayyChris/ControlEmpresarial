using ControlEmpresarial.Controlador;
using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Control_de_Actividades
{
    public partial class PreAceptacionActividadJefatura : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarActividadesGrid(); 
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Accion")
            {
                // Obtén el idActividad del CommandArgument
                string evidencia = e.CommandArgument.ToString();

                // Realiza la redirección a la nueva página con idActividad como parámetro
                Response.Redirect($"../Control de Actividades/NegacionAceptacionActividadJefe.aspx?evidencia={evidencia}");
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtener el valor de la celda de fecha
                DateTime fechaFin;
                bool isDate = DateTime.TryParse(DataBinder.Eval(e.Row.DataItem, "FechaFin").ToString(), out fechaFin);

                if (isDate)
                {
                    // Formatear la fecha y asignarla de nuevo a la celda
                    e.Row.Cells[4].Text = fechaFin.ToString("dd/MM/yyyy"); // Asegúrate de que el índice de la celda sea correcto
                }
            }
        }
        private void CargarActividadesGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            int idEmpleado = ObtenerIdEmpleado();
            if (idEmpleado == -1)
            {
                Label1.Text = "Error al obtener el idEmpleado.";
                Label1.Visible = true;
                return;
            }

            string query = @"
        SELECT a.idActividad AS Evidencia, ar.Titulo, a.Descripcion, a.FechaFin, e.Nombre
        FROM actividades a
        INNER JOIN empleado e ON a.idEmpleado = e.idEmpleado
        INNER JOIN actividadesregistradas ar ON a.idActividadRegistrada = ar.id
        WHERE e.idDepartamento IN (
            SELECT ar.idDepartamento 
            FROM actividadesregistradas ar 
            WHERE ar.idEnviador = @idEmpleado
        )
        AND a.Estado = 'Validada'"; 

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            GridView1.DataSource = reader;
                            GridView1.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        Label1.Text = "Error al cargar los datos: " + ex.Message;
                        Label1.Visible = true;
                    }
                }
            }
        }


        private int ObtenerIdEmpleado()
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEmpleado))
            {
                return idEmpleado;
            }
            return -1;
        }
    }

}
