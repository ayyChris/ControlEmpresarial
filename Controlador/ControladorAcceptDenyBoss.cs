using ControlEmpresarial.Controlador;
using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Horas_Extra
{
    public partial class AceptarDenegarHorasExtraJefatura : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEmpleados();
                CargarNombreUsuario();
                CargarNotificaciones();
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


        private void CargarEmpleados()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            DataTable dtEmpleados = ObtenerColaboradores(connectionString);

            colaborador.DataSource = dtEmpleados;
            colaborador.DataTextField = "Nombre";
            colaborador.DataValueField = "idEmpleado";
            colaborador.DataBind();

            // Add a default item at the beginning
            colaborador.Items.Insert(0, new ListItem("-- Seleccione un colaborador --", "0"));
        }

        private DataTable ObtenerColaboradores(string connectionString)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idEmpleado, Nombre FROM empleado";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        protected void colaborador_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idEmpleado;
            if (int.TryParse(colaborador.SelectedValue, out idEmpleado))
            {
                // Obtén los datos basados en la selección.
                DataTable dtEvidencias = ObtenerEvidencias(idEmpleado);

                if (dtEvidencias.Rows.Count > 0)
                {
                    // Supongamos que la primera fila tiene la información que necesitas.
                    DataRow row = dtEvidencias.Rows[0];

                    lblFechaRespuesta.Text = row["FechaEvidencia"].ToString();
                    lblEvidencia.Text = row["EnlaceEvidencia"].ToString();
                }
                else
                {
                    lblFechaRespuesta.Text = "No hay datos disponibles.";
                    lblEvidencia.Text = "No hay datos disponibles.";
                }
            }
        }

        private DataTable ObtenerData(int idEmpleado)
        {
            DataTable dt = new DataTable();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT e.idEvidencia, e.idSolicitud, en.idEntrada " +
                               "FROM evidenciahorasextras e " +
                               "JOIN entradas en ON e.idEmpleado = en.idEmpleado " +
                               "WHERE e.idEmpleado = @idEmpleado AND e.Estado = 'Evidenciada'";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }

        private bool ActualizarEstado(int idSolicitud, int idEmpleado)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "UPDATE evidenciahorasextras " +
                               "SET Estado = 'Revisada' " +
                               "WHERE idSolicitud = @idSolicitud AND idEmpleado = @idEmpleado";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idSolicitud", idSolicitud);
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        lblMensaje.Text = "Error al actualizar: " + ex.Message;
                        lblMensaje.CssClass = "mensaje-error";
                        lblMensaje.Visible = true;
                        return false;
                    }
                }
            }
        }

        private DataTable ObtenerEvidencias(int idEmpleado)
        {
            DataTable dt = new DataTable();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT FechaEvidencia, EnlaceEvidencia " +
                               "FROM evidenciahorasextras " +
                               "WHERE idEmpleado = @idEmpleado AND Estado = 'Evidenciada'";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }

        private DataTable ObtenerDatosSolicitud(int idSolicitud)
        {
            DataTable dt = new DataTable();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT FechaFinalSolicitud, HorasSolicitadas, HoraInicialExtra " +
                               "FROM solicitudhorasextras " +
                               "WHERE idSolicitud = @idSolicitud";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idSolicitud", idSolicitud);
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }

        private DataTable ObtenerEntradas(int idEmpleado)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idEmpleado, DiaMarcado, HoraEntrada " +
                               "FROM entradas " +
                               "WHERE idEmpleado = @idEmpleado";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }

        private DataTable ObtenerSolicitudHorasExtras(int idEmpleado)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT FechaFinalSolicitud, HoraInicialExtra " +
                               "FROM solicitudhorasextras " +
                               "WHERE idEmpleado = @idEmpleado";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }

        // Método ValidarEntrada
        private bool ValidarEntrada(int idEmpleado, DateTime fechaSolicitud, TimeSpan horaSolicitud)
        {
            DataTable dtEntradas = ObtenerEntradas(idEmpleado);
            DataTable dtSolicitudes = ObtenerSolicitudHorasExtras(idEmpleado);

            // Verificar si hay datos en ambas tablas
            if (dtEntradas.Rows.Count > 0 && dtSolicitudes.Rows.Count > 0)
            {
                // Obtén los datos de la primera fila de cada DataTable
                DataRow entradaRow = dtEntradas.Rows[0];
                DataRow solicitudRow = dtSolicitudes.Rows[0];

                // Extraer los datos
                DateTime diaMarcado = Convert.ToDateTime(entradaRow["DiaMarcado"]);
                TimeSpan horaEntrada = TimeSpan.Parse(entradaRow["HoraEntrada"].ToString());
                DateTime fechaFinalSolicitud = Convert.ToDateTime(solicitudRow["FechaFinalSolicitud"]);
                TimeSpan horaInicialSolicitud = TimeSpan.Parse(solicitudRow["HoraInicialExtra"].ToString());

                lblDebugInfo.Text = $"Datos de Entrada: idEmpleado = {entradaRow["idEmpleado"]}, DiaMarcado = {diaMarcado.ToShortDateString()}, HoraEntrada = {horaEntrada}<br>" +
                                    $"Datos de Solicitud: FechaFinalSolicitud = {fechaFinalSolicitud.ToShortDateString()}, HoraInicialSolicitud = {horaInicialSolicitud}";

                // Verificar coincidencia de datos
                bool coincidencia = diaMarcado.Date == fechaSolicitud.Date && horaEntrada == horaSolicitud;

                // Mostrar el resultado de validación en la interfaz de usuario
                lblValidacion.Text = $"¿Coinciden los datos? {coincidencia}";

                return coincidencia;
            }
            else
            {
                lblValidacion.Text = "No se encontraron datos suficientes para validar.";
                lblValidacion.CssClass = "mensaje-error";
                return false;
            }
        }


        protected void DenegarButton_Click(object sender, EventArgs e)
        {
            int idEmpleado;
            if (int.TryParse(colaborador.SelectedValue, out idEmpleado))
            {
                DataTable dtEvidencias = ObtenerData(idEmpleado);

                if (dtEvidencias.Rows.Count > 0)
                {
                    DataRow row = dtEvidencias.Rows[0];

                    int idEvidencia = Convert.ToInt32(row["idEvidencia"]);
                    int idEntrada = Convert.ToInt32(row["idEntrada"]);
                    int idSolicitud = Convert.ToInt32(row["idSolicitud"]);

                    DataTable dtSolicitud = ObtenerDatosSolicitud(idSolicitud);
                    if (dtSolicitud.Rows.Count > 0)
                    {
                        DataRow solicitudRow = dtSolicitud.Rows[0];

                        DateTime fechaTrabajo = Convert.ToDateTime(solicitudRow["FechaFinalSolicitud"]);
                        int horasTrabajadas = Convert.ToInt32(solicitudRow["HorasSolicitadas"]);
                        TimeSpan horaInicialExtra = TimeSpan.Parse(solicitudRow["HoraInicialExtra"].ToString());

                        bool entradaValida = ValidarEntrada(idEmpleado, fechaTrabajo, horaInicialExtra);

                        if (entradaValida)
                        {
                            string Aceptacion = "Denegada";
                            bool insertado = InsertarHorasExtras(idEmpleado, idEvidencia, fechaTrabajo, horasTrabajadas, idEntrada, Aceptacion);

                            if (insertado)
                            {
                                bool estadoActualizado = ActualizarEstado(idSolicitud, idEmpleado);
                                if (estadoActualizado)
                                {
                                    lblMensaje.Text = "<i class='fas fa-thumbs-down'></i> Horas extras denegadas y estado actualizado correctamente.";
                                    lblMensaje.CssClass = "mensaje-error";
                                    lblMensaje.Visible = true;
                                }
                                else
                                {
                                    lblMensaje.Text = "<i class='fas fa-thumbs-down'></i> Horas extras denegadas, pero no se pudo actualizar el estado.";
                                    lblMensaje.CssClass = "mensaje-error";
                                    lblMensaje.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            lblMensaje.Text = "<i class='fas fa-thumbs-down'></i> No se encontró una entrada válida para la hora inicial de la solicitud.";
                            lblMensaje.CssClass = "mensaje-error";
                            lblMensaje.Visible = true;
                        }
                    }
                    else
                    {
                        lblMensaje.Text = "<i class='fas fa-thumbs-down'></i> No se encontraron datos de la solicitud.";
                        lblMensaje.CssClass = "mensaje-error";
                        lblMensaje.Visible = true;
                    }
                }
                else
                {
                    lblMensaje.Text = "<i class='fas fa-thumbs-down'></i> No hay evidencias disponibles para el empleado seleccionado.";
                    lblMensaje.CssClass = "mensaje-error";
                    lblMensaje.Visible = true;
                }
            }
            else
            {
                lblMensaje.Text = "<i class='fas fa-thumbs-down'></i> Seleccione un colaborador válido.";
                lblMensaje.CssClass = "mensaje-error";
                lblMensaje.Visible = true;
            }
        }

        protected void AceptarButton_Click(object sender, EventArgs e)
        {
            int idEmpleado;
            if (int.TryParse(colaborador.SelectedValue, out idEmpleado))
            {
                DataTable dtEvidencias = ObtenerData(idEmpleado);

                if (dtEvidencias.Rows.Count > 0)
                {
                    DataRow row = dtEvidencias.Rows[0];
                    int idEvidencia = Convert.ToInt32(row["idEvidencia"]);
                    int idEntrada = Convert.ToInt32(row["idEntrada"]);
                    int idSolicitud = Convert.ToInt32(row["idSolicitud"]);

                    DataTable dtSolicitud = ObtenerDatosSolicitud(idSolicitud);
                    if (dtSolicitud.Rows.Count > 0)
                    {
                        DataRow solicitudRow = dtSolicitud.Rows[0];
                        DateTime fechaTrabajo = Convert.ToDateTime(solicitudRow["FechaFinalSolicitud"]);
                        int horasTrabajadas = Convert.ToInt32(solicitudRow["HorasSolicitadas"]);
                        TimeSpan horaInicialExtra = TimeSpan.Parse(solicitudRow["HoraInicialExtra"].ToString());

                        bool entradaValida = ValidarEntrada(idEmpleado, fechaTrabajo, horaInicialExtra);
                        if (entradaValida)
                        {
                            string Aceptacion = "Aceptada";
                            bool insertado = InsertarHorasExtras(idEmpleado, idEvidencia, fechaTrabajo, horasTrabajadas, idEntrada, Aceptacion);

                            if (insertado)
                            {
                                bool estadoActualizado = ActualizarEstado(idSolicitud, idEmpleado);
                                if (estadoActualizado)
                                {
                                    lblMensaje.Text = "<i class='fas fa-thumbs-up'></i> Horas extras aceptadas y estado actualizado correctamente.";
                                    lblMensaje.CssClass = "mensaje-exito";
                                    lblMensaje.Visible = true;
                                }
                                else
                                {
                                    lblMensaje.Text = "<i class='fas fa-thumbs-up'></i> Horas extras aceptadas, pero no se pudo actualizar el estado.";
                                    lblMensaje.CssClass = "mensaje-error";
                                    lblMensaje.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            lblMensaje.Text = "<i class='fas fa-thumbs-down'></i> No se encontró una entrada válida para la hora inicial de la solicitud.";
                            lblMensaje.CssClass = "mensaje-error";
                            lblMensaje.Visible = true;
                        }
                    }
                    else
                    {
                        lblMensaje.Text = "<i class='fas fa-thumbs-down'></i> No se encontraron datos de la solicitud.";
                        lblMensaje.CssClass = "mensaje-error";
                        lblMensaje.Visible = true;
                    }
                }
                else
                {
                    lblMensaje.Text = "<i class='fas fa-thumbs-down'></i> No hay evidencias disponibles para el empleado seleccionado.";
                    lblMensaje.CssClass = "mensaje-error";
                    lblMensaje.Visible = true;
                }
            }
            else
            {
                lblMensaje.Text = "<i class='fas fa-thumbs-down'></i> Seleccione un colaborador válido.";
                lblMensaje.CssClass = "mensaje-error";
                lblMensaje.Visible = true;
            }
        }


        private bool InsertarHorasExtras(int idEmpleado, int idEvidencia, DateTime fechaTrabajo, int horasTrabajadas, int idEntrada, string Aceptacion)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO horasextras (idEmpleado, idEvidencia, FechaTrabajo, HorasTrabajadas, idEntrada, Aceptacion) " +
                               "VALUES (@idEmpleado, @idEvidencia, @fechaTrabajo, @horasTrabajadas, @idEntrada, @Aceptacion)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@idEvidencia", idEvidencia);
                    cmd.Parameters.AddWithValue("@fechaTrabajo", fechaTrabajo);
                    cmd.Parameters.AddWithValue("@horasTrabajadas", horasTrabajadas);
                    cmd.Parameters.AddWithValue("@idEntrada", idEntrada);
                    cmd.Parameters.AddWithValue("@Aceptacion", Aceptacion);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        lblMensaje.Text = "Error al insertar los datos: " + ex.Message;
                        lblMensaje.CssClass = "mensaje-error";
                        lblMensaje.Visible = true;
                        return false;
                    }
                }
            }
        }
    }
}
