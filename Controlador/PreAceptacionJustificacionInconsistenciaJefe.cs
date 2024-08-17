using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace ControlEmpresarial.Vistas.Inconsistencias
{

    public partial class AceptarDenegarInconsistenciasJefeç : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            DataTable dt = new DataTable();

            string query = @"
        SELECT 
            ji.idJustificacion,
            ji.Justificacion,
            ji.FechaJustificacion,
            ji.Estado AS EstadoJustificacion,
            i.idInconsistencia,
            e.Nombre AS NombreEmpleado,
            ti.Nombre AS TipoInconsistencia
        FROM 
            justificacioninconsistencia ji
        JOIN 
            inconsistencias i ON ji.idInconsistencia = i.idInconsistencia
        JOIN 
            empleado e ON i.idEmpleado = e.idEmpleado
        JOIN 
            tiposinconsistencia ti ON i.idTipoInconsistencia = ti.idTipoInconsistencia
        WHERE 
            ji.Estado = 'En espera';";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        Label1.Text = "Error al cargar los datos: " + ex.Message;
                        Label1.ForeColor = System.Drawing.Color.Red;
                        Label1.Visible = true;
                    }
                }
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Accion")
            {
                int idJustificacion;
                if (int.TryParse(e.CommandArgument.ToString(), out idJustificacion))
                {
                    // Redirigir a la página de aceptación con el ID de la justificación
                    Response.Redirect($"AceptacionJustificacionInconsistenciaJefe.aspx?id={idJustificacion}");
                }
                else
                {
                    Label2.Text = "ID de justificación no válido.";
                    Label2.Visible = true;
                }
            }
        }



        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Verifica que la fila es del tipo DataRow.
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Aquí puedes agregar lógica para manipular los datos de la fila.
                // Por ejemplo, cambiar el color de fondo de la fila basado en un valor específico.
                string estadoJustificacion = DataBinder.Eval(e.Row.DataItem, "EstadoJustificacion").ToString();
                if (estadoJustificacion == "En espera")
                {
                    e.Row.BackColor = System.Drawing.Color.LightYellow;
                }
            }
        }


    }
}