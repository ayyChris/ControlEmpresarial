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
                CargarNombreUsuario();
                CargarNotificaciones();
                CargarDatosTabla();
            }
        }




        private void CargarNombreUsuario()
        {
            // Obtener el nombre de las cookies
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null)
            {
                string nombre = cookie["Nombre"];
                string apellidos = cookie["Apellidos"];
                lblNombre.Text = nombre + " " + apellidos;
                lblNombre.Visible = true;
            }
            else
            {
                lblNombre.Text = "Error";
                lblNombre.Visible = true;
            }
        }

        private void CargarNotificaciones()
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null)
            {
                // Intentar extraer el idEmpleado de la cookie
                if (int.TryParse(cookie["idEmpleado"], out int idEmpleado))
                {
                    // Obtener las notificaciones usando el idEmpleado extraído
                    NotificacionService service = new NotificacionService();
                    List<Notificacion> notificaciones = service.ObtenerNotificaciones(idEmpleado);

                    // Enlazar los datos al repeater
                    repeaterNotificaciones.DataSource = notificaciones;
                    repeaterNotificaciones.DataBind();
                }
                else
                {
                    // Manejar caso en el que idEmpleado no es válido
                    Label1.Text = "Error al extraer ID de empleado";
                    Label1.Visible = true;
                }
            }
            else
            {
                Label1.Text = "Cookie no encontrada";
                Label1.Visible = true;
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

                            // Asignar datos al GridView
                            GridView1.DataSource = dataTable;
                            GridView1.DataBind();
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
                    int idActividad = Convert.ToInt32(row.Cells[0].Text);
                    int idEmpleado = ObtenerIdEmpleado();

                    if (idEmpleado != -1)
                    {
                        InsertarRelacionActividadEmpleado(idActividad, idEmpleado);
                        ActualizarEstadoActividad(idActividad, "Completada");

                        // Configurar el texto del Label con HTML y el ícono
                        Label1.Text = "Actividad evidenciada <i class='fas fa-thumbs-up'></i>";
                        Label1.Visible = true;
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



        private void ActualizarEstadoActividad(int idActividad, string nuevoEstado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            string query = "UPDATE actividadesregistradas SET estado = @nuevoEstado WHERE id = @idActividad AND estado = 'Pendiente'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                    command.Parameters.AddWithValue("@idActividad", idActividad);

                    try
                    {
                        connection.Open();
                        int filasAfectadas = command.ExecuteNonQuery();

                        // Verifica si se actualizó alguna fila
                        if (filasAfectadas == 0)
                        {
                            // No se actualizó ninguna fila, lo que significa que el estado no era "Pendiente"
                            Label1.Text = $"La actividad (ID: {idActividad}) no estaba en estado 'Pendiente'.";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        Label1.Text = "Error al actualizar el estado de la actividad: " + ex.Message;
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
            return -1; // Valor de error si no se puede obtener el idEmpleado
        }

        private void InsertarRelacionActividadEmpleado(int idActividad, int idEmpleado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            string query = "INSERT INTO relacionactividadempleado (idActividad, idEmpleado, Estado) VALUES (@idActividad, @idEmpleado, @Estado)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idActividad", idActividad);
                    command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    command.Parameters.AddWithValue("@Estado", "Espera");

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        Label1.Text = "Error al insertar en relacionactividadempleado: " + ex.Message;
                        Label1.Visible = true;
                    }
                }
            }
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
                        }
                        catch (Exception ex)
                        {
                            // Manejo de errores (opcional, dependiendo de tus necesidades)
                            throw new Exception("Error al obtener el idDepartamento: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                // Cookie no encontrada o idEmpleado inválido (opcional, dependiendo de tus necesidades)
                throw new Exception("Cookie no encontrada o idEmpleado inválido");
            }

            return idDepartamento;
        }


    }
}
