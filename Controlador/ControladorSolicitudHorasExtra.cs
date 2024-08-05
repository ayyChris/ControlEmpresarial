using ControlEmpresarial.Services;
using System;
using System.Data;
using System.Web.UI;
using MySql.Data.MySqlClient;
using System.Web;
using ControlEmpresarial.Controlador;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

namespace ControlEmpresarial.Vistas
{
    public partial class solicitudHorasExtras : System.Web.UI.Page
    {
        private NotificacionService notificacionService = new NotificacionService();
        private string horarioTrabajador = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEmpleados();
            }
        }

        protected void dia_TextChanged(object sender, EventArgs e)
        {
            string diaSeleccionado = dia.Text; // Obtener el día seleccionado en formato "yyyy-MM-dd"
            if (DateTime.TryParse(diaSeleccionado, out DateTime fecha))
            {
                if (colaborador.SelectedItem != null && colaborador.SelectedItem.Value != "0")
                {
                    int idEmpleado = int.Parse(colaborador.SelectedItem.Value);

                    // Diccionario para la conversión de días de la semana
                    Dictionary<string, string> diasSemanaEsp = new Dictionary<string, string>()
                    {
                        { "Sunday", "Domingo" },
                        { "Monday", "Lunes" },
                        { "Tuesday", "Martes" },
                        { "Wednesday", "Miércoles" },
                        { "Thursday", "Jueves" },
                        { "Friday", "Viernes" },
                        { "Saturday", "Sábado" }
                    };

                    string diaSemanaEn = fecha.DayOfWeek.ToString(); // Día de la semana en inglés
                    string diaSemanaEs = diasSemanaEsp[diaSemanaEn]; // Día de la semana en español

                    horarioLabel.Text = "Buscando horario para: " + diaSemanaEs; // Mensaje de depuración

                    string horario = ObtenerHorarioPorDia(idEmpleado, diaSemanaEs);
                    if (!string.IsNullOrEmpty(horario))
                    {
                        horarioLabel.Text = horario;
                        horarioTrabajador = horario;
                    }
                    else
                    {
                        horarioLabel.Text = "No se encontró horario para " + diaSemanaEs;
                    }
                }
                else
                {
                    horarioLabel.Text = "Seleccione un colaborador.";
                }
            }
            else
            {
                horarioLabel.Text = "Fecha no válida";
            }
        }

        private string ObtenerHorarioPorDia(int idEmpleado, string diaSemana)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            string horario = "";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"
                SELECT h.horaEntrada, h.horaSalida, h.diaSemana
                FROM horario h
                JOIN empleado e ON e.idHorario = h.idHorario
                WHERE e.idEmpleado = @idEmpleado";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                try
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string dias = reader.GetString("diaSemana");
                            if (dias.Split(',').Contains(diaSemana))
                            {
                                TimeSpan horaEntrada = reader.GetTimeSpan("horaEntrada");
                                TimeSpan horaSalida = reader.GetTimeSpan("horaSalida");
                                horario = $"{diaSemana}: {horaEntrada:hh\\:mm} - {horaSalida:hh\\:mm}";
                                break;
                            }
                        }
                        if (string.IsNullOrEmpty(horario))
                        {
                            horarioLabel.Text = "No se encontró horario en la base de datos."; // Mensaje de depuración
                        }
                    }
                }
                catch (Exception ex)
                {
                    horarioLabel.Text = "Error al obtener horario: " + ex.Message;
                }
            }
            return horario;
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            string idEmpleado = colaborador.SelectedItem.Value;
            string colaboradorSeleccionado = colaborador.SelectedItem.Text;
            string dia = this.dia.Text;
            string horaInicio = this.horaInicio.Text;
            string horaFinal = this.horaFinal.Text;
            string motivo = this.motivo.Text;
            DateTime thisDay = DateTime.Today;

            // Convertimos las horas a TimeSpan
            TimeSpan horaInicioTS = TimeSpan.Parse(horaInicio);
            TimeSpan horaFinalTS = TimeSpan.Parse(horaFinal);

            // Calculamos las horas solicitadas
            double horasTotales = (horaFinalTS - horaInicioTS).TotalHours;

            // Validar que las horas solicitadas no superen las horas permitidas diarias
            if (!ValidarHorasExtraDiarias(idEmpleado, dia, horaInicioTS, horaFinalTS))
            {
                lblMensaje.Text = "La solicitud de horas extra supera el límite permitido para el día.";
                lblMensaje.Visible = true;
                return;
            }

            // Validar que las horas solicitadas no superen las horas permitidas semanalmente
            if (!ValidarHorasExtraSemanales(idEmpleado, dia, horaInicioTS, horaFinalTS))
            {
                lblMensaje.Text = "La solicitud de horas extra supera el límite permitido para la semana.";
                lblMensaje.Visible = true;
                return;
            }

            string Activo = "Activo";

            // Llama al método para insertar los datos en la base de datos
            InsertarHoraExtra(idEmpleado, colaboradorSeleccionado, thisDay, dia, horaInicioTS, horaFinalTS, horasTotales, motivo, Activo);
            lblMensaje.Text = "Horas Extra enviadas correctamente!";
            lblMensaje.Visible = true;

            // Insertar la notificación
            InsertarNotificacion(Convert.ToInt32(idEmpleado), "Hora Extra Solicitada", motivo, thisDay);

            // Script para mostrar el ícono
            ClientScript.RegisterStartupScript(this.GetType(), "showLikeIcon", "<script>document.getElementById('likeIcon').style.display = 'inline-block';</script>");
        }

        private bool ValidarHorasExtraDiarias(string idEmpleado, string dia, TimeSpan horaInicio, TimeSpan horaFinal)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            double horasPermitidasDiarias = 8; // Ajusta este valor según la legislación local

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"
        SELECT h.horaEntrada, h.horaSalida
        FROM horario h
        JOIN empleado e ON e.idHorario = h.idHorario
        WHERE e.idEmpleado = @idEmpleado";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                try
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            TimeSpan horaEntrada = reader.GetTimeSpan("horaEntrada");
                            TimeSpan horaSalida = reader.GetTimeSpan("horaSalida");
                            TimeSpan jornadaDiaria = horaSalida - horaEntrada;

                            if (horaInicio < horaEntrada || horaFinal > horaSalida || (horaFinal - horaInicio).TotalHours > horasPermitidasDiarias)
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = "Error al validar horario diario: " + ex.Message;
                    lblMensaje.Visible = true;
                }
            }

            return true;
        }

        private bool ValidarHorasExtraSemanales(string idEmpleado, string dia, TimeSpan horaInicio, TimeSpan horaFinal)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            double horasPermitidasSemanales = 48; // Ajusta este valor según la legislación local

            DateTime fechaInicioSemana = DateTime.Parse(dia).AddDays(-(int)DateTime.Parse(dia).DayOfWeek);
            DateTime fechaFinalSemana = fechaInicioSemana.AddDays(6);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"
        SELECT SUM(HorasSolicitadas) AS TotalHoras
        FROM solicitudHorasExtras
        WHERE idEmpleado = @idEmpleado
        AND FechaInicioSolicitud BETWEEN @fechaInicioSemana AND @fechaFinalSemana
        AND Estado = 'Aceptada'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                cmd.Parameters.AddWithValue("@fechaInicioSemana", fechaInicioSemana);
                cmd.Parameters.AddWithValue("@fechaFinalSemana", fechaFinalSemana);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    double horasTotalesSemanales = result != DBNull.Value ? Convert.ToDouble(result) : 0;

                    if ((horasTotalesSemanales + (horaFinal - horaInicio).TotalHours) > horasPermitidasSemanales)
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = "Error al validar horario semanal: " + ex.Message;
                    lblMensaje.Visible = true;
                }
            }

            return true;
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

        private void InsertarHoraExtra(string idEmpleado, string colaborador, DateTime FechaInicio, string dia, TimeSpan horaInicio, TimeSpan horaFinal, double horasTotales, string motivo, string estado)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            // Obtener idEnviador de la cookie
            HttpCookie userCookie = HttpContext.Current.Request.Cookies["UserInfo"];
            if (userCookie == null || !int.TryParse(userCookie["idEmpleado"], out int idEnviador))
            {
                throw new InvalidOperationException("No se pudo obtener el idEnviador de la cookie.");
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"
                INSERT INTO solicitudHorasExtras (idEmpleado, NombreEmpleado, FechaInicioSolicitud, FechaFinalSolicitud, HoraInicialExtra, HoraFinalExtra, HorasSolicitadas, Motivo, Estado, idEnviador) 
                VALUES (@idEmpleado, @NombreEmpleado, @FechaInicioSolicitud, @FechaFinalSolicitud, @HoraInicialExtra, @HoraFinalExtra, @HorasSolicitadas, @Motivo, @Estado, @idEnviador)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                cmd.Parameters.AddWithValue("@NombreEmpleado", colaborador);
                cmd.Parameters.AddWithValue("@FechaInicioSolicitud", FechaInicio);
                cmd.Parameters.AddWithValue("@FechaFinalSolicitud", Convert.ToDateTime(dia));
                cmd.Parameters.AddWithValue("@HoraInicialExtra", horaInicio);
                cmd.Parameters.AddWithValue("@HoraFinalExtra", horaFinal);
                cmd.Parameters.AddWithValue("@HorasSolicitadas", horasTotales);
                cmd.Parameters.AddWithValue("@Motivo", motivo);
                cmd.Parameters.AddWithValue("@Estado", estado);
                cmd.Parameters.AddWithValue("@idEnviador", idEnviador);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void InsertarNotificacion(int idEmpleado, string titulo, string motivo, DateTime fecha)
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            int idEnviador = Convert.ToInt32(cookie["idEmpleado"]);

            notificacionService.InsertarNotificacion(idEnviador, idEmpleado, titulo, motivo, fecha);
        }
    }
}
