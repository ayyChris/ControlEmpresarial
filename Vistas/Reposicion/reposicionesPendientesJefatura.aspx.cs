using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Reposicion
{
    public partial class reposicionesPendientesJefatura : System.Web.UI.Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarInconsistenciasPorReponer();
            }
        }

        private void CargarInconsistenciasPorReponer()
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                string consulta = @"
                    SELECT i.idInconsistencia, i.Fecha, e.Nombre as Empleado, j.Justificacion, ti.Nombre as TipoInconsistencia
                    FROM inconsistencias i
                    INNER JOIN justificacioninconsistencia j ON i.idInconsistencia = j.idInconsistencia
                    INNER JOIN tiposinconsistencia ti ON i.idTipoInconsistencia = ti.idTipoInconsistencia
                    INNER JOIN empleado e ON i.idEmpleado = e.idEmpleado
                    WHERE i.Estado = 'Justificada' AND j.Estado = 'Aceptado' AND ti.ReponerTiempo = 'Si'";

                MySqlCommand cmd = new MySqlCommand(consulta, conexion);
                conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                incosistenciasPorReponer.Items.Clear();

                while (reader.Read())
                {
                    string inconsistencia = $"{reader["idInconsistencia"]} - {reader["Empleado"]} - {reader["TipoInconsistencia"]} - {reader["Fecha"]:yyyy-MM-dd}";
                    incosistenciasPorReponer.Items.Add(new ListItem(inconsistencia, reader["idInconsistencia"].ToString()));
                }

                reader.Close();
            }
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            // Verifica que se haya seleccionado una inconsistencia
            if (incosistenciasPorReponer.SelectedIndex == -1)
            {
                // Notifica al usuario que debe seleccionar una inconsistencia
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Error', 'Debe seleccionar una inconsistencia.', 'error');", true);
                return;
            }

            // Recupera el ID de la inconsistencia seleccionada
            int idInconsistencia = int.Parse(incosistenciasPorReponer.SelectedValue);

            // Recupera los datos del formulario
            DateTime fechaInicio;
            DateTime fechaFinal;
            TimeSpan horaInicio;
            TimeSpan horaFinal;

            // Validar que los datos de fecha y hora sean válidos
            if (!DateTime.TryParse(txtFechaInicio.Text, out fechaInicio) ||
                !DateTime.TryParse(txtFechaFinal.Text, out fechaFinal) ||
                !TimeSpan.TryParse(txthoraInicio.Text, out horaInicio) ||
                !TimeSpan.TryParse(txthoraFinal.Text, out horaFinal))
            {
                // Notifica al usuario si hay un error en la fecha o la hora
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Error', 'Fechas o horas no válidas.', 'error');", true);
                return;
            }

            // Obtener el idEmpleado desde la inconsistencia seleccionada
            int idEmpleado;
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                string consultaEmpleado = "SELECT idEmpleado FROM inconsistencias WHERE idInconsistencia = @idInconsistencia";
                using (MySqlCommand cmdEmpleado = new MySqlCommand(consultaEmpleado, conexion))
                {
                    cmdEmpleado.Parameters.AddWithValue("@idInconsistencia", idInconsistencia);

                    try
                    {
                        conexion.Open();
                        idEmpleado = Convert.ToInt32(cmdEmpleado.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones si ocurre un problema al obtener el idEmpleado
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"Swal.fire('Error', 'No se pudo obtener el ID del empleado. {ex.Message}', 'error');", true);
                        return;
                    }
                }
            }

            // Inserta los datos en la base de datos
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                string consulta = @"
            INSERT INTO solicitudreposicion (idEmpleado, Fecha, Estado, idInconsistencia, FechaInicialReposicion, FechaFinalReposicion, HoraInicialReposicion, HoraFinalReposicion)
            VALUES (@idEmpleado, @Fecha, @Estado, @idInconsistencia, @FechaInicialReposicion, @FechaFinalReposicion, @HoraInicialReposicion, @HoraFinalReposicion)";

                using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@Fecha", DateTime.Now); // O la fecha actual o la que corresponda
                    cmd.Parameters.AddWithValue("@Estado", "Pendiente"); // Ajusta según sea necesario
                    cmd.Parameters.AddWithValue("@idInconsistencia", idInconsistencia);
                    cmd.Parameters.AddWithValue("@FechaInicialReposicion", fechaInicio.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@FechaFinalReposicion", fechaFinal.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@HoraInicialReposicion", horaInicio.ToString(@"hh\:mm"));
                    cmd.Parameters.AddWithValue("@HoraFinalReposicion", horaFinal.ToString(@"hh\:mm"));

                    try
                    {
                        conexion.Open();
                        cmd.ExecuteNonQuery();
                        // Notifica al usuario que la inserción fue exitosa
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire('Éxito', 'Reposición asignada correctamente.', 'success');", true);
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones y notificación al usuario
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"Swal.fire('Error', 'No se pudo asignar la reposición. {ex.Message}', 'error');", true);
                    }
                }
            }
        }
    }
}
