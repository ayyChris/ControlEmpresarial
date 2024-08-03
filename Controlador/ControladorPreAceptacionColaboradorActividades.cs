using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI;
using ControlEmpresarial.Services;
using System.Web;
using ControlEmpresarial.Controlador;

namespace ControlEmpresarial.Vistas.Control_de_Actividades
{
    public partial class TablePreAceptacionJefatura : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label1.Text = "Cargando datos...";
                CargarDatosTabla();
            }
        }

        private void CargarDatosTabla()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            int? idDepartamento = ObtenerIdDepartamento();

            if (idDepartamento.HasValue)
            {
                string query = @"
                    SELECT 
                        a.id,
                        a.Fecha, 
                        a.Titulo, 
                        a.Descripcion, 
                        t.Tipo
                    FROM 
                        actividadesregistradas a
                    INNER JOIN 
                        tipoactividad t 
                    ON 
                        a.idTipo = t.idTipo
                    WHERE 
                        a.idDepartamento = @idDepartamento
                    AND
                        a.Estado = 'Pendiente'"; // Agrega la condición para el estado

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idDepartamento", idDepartamento.Value);

                        try
                        {
                            connection.Open();
                            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                            System.Data.DataTable dataTable = new System.Data.DataTable();
                            dataAdapter.Fill(dataTable);

                            if (dataTable.Rows.Count > 0)
                            {
                                // Asignar datos al GridView
                                GridView1.DataSource = dataTable;
                                GridView1.DataBind();
                                Label1.Text = "Datos cargados correctamente.";
                            }
                            else
                            {
                                Label1.Text = "No se encontraron datos.";
                                Label1.Visible = true;
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
            else
            {
                Label1.Text = "Error: idDepartamento no encontrado.";
                Label1.Visible = true;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtener el valor de la celda de fecha
                DateTime fecha = (DateTime)DataBinder.Eval(e.Row.DataItem, "Fecha");
                // Formatear la fecha y asignarla de nuevo a la celda
                e.Row.Cells[1].Text = fecha.ToString("dd/MM/yyyy");
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Accion")
            {
                int index;
                if (int.TryParse(e.CommandArgument.ToString(), out index) && index >= 0 && index < GridView1.Rows.Count)
                {
                    GridViewRow row = GridView1.Rows[index];
                    int idActividad = Convert.ToInt32(row.Cells[0].Text); // Asegúrate de que el DataField='id' esté en la primera columna o ajusta el índice si es necesario

                    // Puedes obtener idEmpleado aquí si es necesario
                    int idEmpleado = ObtenerIdEmpleado();

                    if (idEmpleado != -1)
                    {

                        Response.Redirect($"NegacionAceptacionActividadJefe.aspx?id={idActividad}");
                    }
                    else
                    {
                        Label1.Text = "Error al obtener el idEmpleado.";
                        Label1.Visible = true;
                    }
                }
                else
                {
                    Label1.Text = "Índice de fila no válido.";
                    Label1.Visible = true;
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
            return -1; // Valor de error si no se puede obtener el idEmpleado
        }

        
        private int? ObtenerIdDepartamento()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            int? idDepartamento = null;

            // Obtener el idEmpleado de la cookie
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEmpleado))
            {
                string query = "SELECT idDepartamento FROM empleado WHERE idEmpleado = @idEmpleado";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                        try
                        {
                            connection.Open();
                            object result = command.ExecuteScalar();

                            if (result != null && result != DBNull.Value)
                            {
                                idDepartamento = Convert.ToInt32(result);
                            }
                            else
                            {
                                Label1.Text = "idDepartamento no encontrado para el idEmpleado.";
                                Label1.Visible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Manejo de errores (opcional, dependiendo de tus necesidades)
                            Label1.Text = "Error al obtener el idDepartamento: " + ex.Message;
                            Label1.Visible = true;
                        }
                    }
                }
            }
            else
            {
                // Cookie no encontrada o idEmpleado inválido (opcional, dependiendo de tus necesidades)
                Label1.Text = "Cookie no encontrada o idEmpleado inválido.";
                Label1.Visible = true;
            }

            return idDepartamento;
        }
    }
}
