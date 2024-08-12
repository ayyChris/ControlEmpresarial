using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Pagina_Principal
{
    public partial class MenuColaborador : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Obtener el ID del empleado de la cookie
                string empleadoIdStr = Request.Cookies["idEmpleado"]?.Value;

                if (int.TryParse(empleadoIdStr, out int empleadoId))
                {
                    DateTime fechaActual = DateTime.Today;
                    VerificarInconsistencia(empleadoId, fechaActual);
                }
                else
                {
                    Response.Write("No se pudo obtener el ID del empleado de la cookie.");
                }
            }
        }

        private void VerificarInconsistencia(int empleadoId, DateTime fecha)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT COUNT(*) AS Inconsistencias
                    FROM Entradas e
                    JOIN Empleado emp ON e.idEmpleado = emp.idEmpleado
                    JOIN horario h ON emp.idHorario = h.idHorario
                    WHERE e.idEmpleado = @EmpleadoId
                      AND e.DiaMarcado = @Fecha
                      AND e.HoraEntrada > h.horaEntrada
                      AND FIND_IN_SET(
                          CASE 
                              WHEN DATENAME(WEEKDAY, @Fecha) = 'Monday' THEN 'Lunes'
                              WHEN DATENAME(WEEKDAY, @Fecha) = 'Tuesday' THEN 'Martes'
                              WHEN DATENAME(WEEKDAY, @Fecha) = 'Wednesday' THEN 'Miércoles'
                              WHEN DATENAME(WEEKDAY, @Fecha) = 'Thursday' THEN 'Jueves'
                              WHEN DATENAME(WEEKDAY, @Fecha) = 'Friday' THEN 'Viernes'
                              WHEN DATENAME(WEEKDAY, @Fecha) = 'Saturday' THEN 'Sábado'
                              WHEN DATENAME(WEEKDAY, @Fecha) = 'Sunday' THEN 'Domingo'
                          END, 
                          h.diaSemana
                      ) > 0";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmpleadoId", empleadoId);
                    command.Parameters.AddWithValue("@Fecha", fecha);

                    connection.Open();
                    int inconsistencias = (int)command.ExecuteScalar();
                    connection.Close();

                    if (inconsistencias > 0)
                    {
                        Response.Write($"Se encontraron inconsistencias para el empleado {empleadoId} en la fecha {fecha.ToShortDateString()}.");
                    }
                    else
                    {
                        Response.Write($"No se encontraron inconsistencias para el empleado {empleadoId} en la fecha {fecha.ToShortDateString()}.");
                    }
                }
            }
        }
    }
}
