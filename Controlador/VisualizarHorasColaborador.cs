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
    public partial class VisualizarHorasColaborador : Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEmpleados();
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
                string query = "SELECT idEmpleado, Nombre FROM empleado WHERE idPuesto = 2";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        protected void colaborador_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idEmpleado = int.Parse(colaborador.SelectedValue);

            if (idEmpleado != 0)
            {
                // Calcular las horas trabajadas en la última semana
                double horasTrabajadas = CalcularHorasTrabajadas(idEmpleado);
                lblHoras.Text = horasTrabajadas.ToString("F2");

                // Aquí puedes agregar el cálculo de las inconsistencias
                int inconsistencias = CalcularInconsistencias(idEmpleado);
                lblInconsistencias.Text = inconsistencias.ToString();
            }
            else
            {
                lblHoras.Text = "0";
                lblInconsistencias.Text = "0";
            }
        }

        private double CalcularHorasTrabajadas(int idEmpleado)
        {
            double totalHoras = 0;
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"SELECT HoraEntrada, HoraSalida, DiaMarcado 
                         FROM entradas 
                         WHERE idEmpleado = @idEmpleado 
                         AND WEEK(DiaMarcado) = WEEK(CURRENT_DATE) 
                         AND HoraEntrada IS NOT NULL 
                         AND HoraSalida IS NOT NULL";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TimeSpan horaEntrada = reader.GetTimeSpan("HoraEntrada");
                        TimeSpan horaSalida = reader.GetTimeSpan("HoraSalida");

                        // Ajuste para manejar turnos que cruzan la medianoche
                        if (horaSalida < horaEntrada && horaSalida.Hours < 12)
                        {
                            // Si la HoraSalida es antes de la hora de entrada y es un horario que cruza la medianoche
                            horaSalida = horaSalida.Add(new TimeSpan(24, 0, 0));
                        }

                        // Calcular la diferencia entre la hora de entrada y salida
                        totalHoras += (horaSalida - horaEntrada).TotalHours;
                    }
                }
            }

            return totalHoras;
        }




        private int CalcularInconsistencias(int idEmpleado)
        {
            int inconsistencias = 0;
            // Aquí puedes agregar la lógica para calcular las inconsistencias según tus necesidades
            return inconsistencias;
        }
    }
}
