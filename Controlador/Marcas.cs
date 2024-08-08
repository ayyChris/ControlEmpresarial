using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas
{
    public partial class marcas : Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEstadoMarcas(); // Ahora siempre habilita ambos botones
                
            }
            CargarHorarioEmpleado();
        }


        private void CargarEstadoMarcas()
        {
            // Siempre habilitar ambos botones
            btnEntrada.Enabled = true;
            btnSalida.Enabled = true;
        }

        protected void btnEntrada_Click(object sender, EventArgs e)
        {
            RegistrarMarca(true);
        }

        protected void btnSalida_Click(object sender, EventArgs e)
        {
            RegistrarMarca(false);
        }

        private void RegistrarMarca(bool esEntrada)
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEmpleado) && int.TryParse(cookie["idDepartamento"], out int idDepartamento))
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    try
                    {
                        conexion.Open(); 

                        // Verificar si es un día laboral
                        if (!EsDiaLaboral(idEmpleado))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "Swal.fire({ title: 'Día No Laboral', text: 'Hoy no es un día laboral para ti.', icon: 'info', timer: 2500, showConfirmButton: false });", true);
                            return;
                        }

                        // Verificar si es un día festivo
                        if (EsDiaFestivo(idDepartamento))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "Swal.fire({ title: 'Día Festivo', text: 'Hoy es un día festivo, no se requiere marcar entrada.', icon: 'info', timer: 2500, showConfirmButton: false });", true);
                            return;
                        }

                        // Verificar si hay vacaciones colectivas
                        if (EsVacacionColectiva())
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "Swal.fire({ title: 'Vacaciones Colectivas', text: 'Hoy hay vacaciones colectivas, no se requiere marcar entrada.', icon: 'info', timer: 2500, showConfirmButton: false });", true);
                            return;
                        }

                        // Verificar si hay un permiso para el día actual en el rango de fechas
                        if (HayPermiso(idEmpleado))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "Swal.fire({ title: 'Permiso Aprobado', text: 'Hoy tienes un permiso aprobado, no se requiere marcar entrada.', icon: 'info', timer: 2500, showConfirmButton: false });", true);
                            return;
                        }

                        if (esEntrada)
                        {
                            // Insertar nueva entrada
                            string query = "INSERT INTO entradas (idEmpleado, DiaMarcado, HoraEntrada, MarcacionEntrada) VALUES (@idEmpleado, @DiaMarcado, @HoraEntrada, 1) ON DUPLICATE KEY UPDATE HoraEntrada = @HoraEntrada, MarcacionEntrada = 1";

                            using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                            {
                                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                                cmd.Parameters.AddWithValue("@DiaMarcado", DateTime.Today);
                                cmd.Parameters.AddWithValue("@HoraEntrada", DateTime.Now.TimeOfDay);
                                cmd.ExecuteNonQuery();
                            }

                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "Swal.fire({ title: '¡Entrada registrada!', text: 'Tu entrada se ha registrado correctamente.', icon: 'success', timer: 2500, showConfirmButton: false });", true);
                        }
                        else
                        {
                            // Obtener el idEntrada de la última entrada
                            string query = "SELECT idEntrada FROM entradas WHERE idEmpleado = @idEmpleado AND DiaMarcado = @DiaMarcado AND MarcacionEntrada = 1 ORDER BY idEntrada DESC LIMIT 1";

                            int idEntrada;
                            using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                            {
                                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                                cmd.Parameters.AddWithValue("@DiaMarcado", DateTime.Today);
                                object result = cmd.ExecuteScalar();

                                if (result != null && int.TryParse(result.ToString(), out idEntrada))
                                {
                                    // Actualizar la salida para la última entrada
                                    query = "UPDATE entradas SET HoraSalida = @HoraSalida, MarcacionSalida = 1 WHERE idEntrada = @idEntrada";

                                    using (MySqlCommand cmdUpdate = new MySqlCommand(query, conexion))
                                    {
                                        cmdUpdate.Parameters.AddWithValue("@idEntrada", idEntrada);
                                        cmdUpdate.Parameters.AddWithValue("@HoraSalida", DateTime.Now.TimeOfDay);
                                        cmdUpdate.ExecuteNonQuery();
                                    }

                                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "Swal.fire({ title: '¡Salida registrada!', text: 'Tu salida se ha registrado correctamente.', icon: 'success', timer: 2500, showConfirmButton: false });", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "Swal.fire({ title: 'Error', text: 'No se encontró una entrada para registrar la salida.', icon: 'error', timer: 2500, showConfirmButton: false });", true);
                                }
                            }
                        }

                        CargarEstadoMarcas(); // Actualizar el estado de los botones
                    }
                    catch (MySqlException ex)
                    {
                        // Manejo específico para errores de MySQL
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error en la base de datos: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    }
                    catch (Exception ex)
                    {
                        // Manejo general para otros tipos de excepciones
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error inesperado: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    }
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "Swal.fire({ title: 'Error', text: 'Error al registrar la marca.', icon: 'error', timer: 2500, showConfirmButton: false });", true);
            }
        }

        private bool EsDiaFestivo(int idDepartamento)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT COUNT(*) FROM diasfestivos WHERE FechaVacacion = @FechaHoy AND idDepartamento = @idDepartamento";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        // Pasar la fecha como DateTime directamente
                        cmd.Parameters.AddWithValue("@FechaHoy", DateTime.Today);
                        cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (MySqlException ex)
                {
                    // Manejo específico para errores de MySQL
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error en la base de datos: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
                catch (Exception ex)
                {
                    // Manejo general para otros tipos de excepciones
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error inesperado: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
            }
        }

        private bool EsDiaLaboral(int idEmpleado)
        {
            // Diccionario para convertir el nombre del día de la semana de inglés a español
            var diasSemana = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "Lunes" },
                { DayOfWeek.Tuesday, "Martes" },
                { DayOfWeek.Wednesday, "Miercoles" },
                { DayOfWeek.Thursday, "Jueves" },
                { DayOfWeek.Friday, "Viernes" },
                { DayOfWeek.Saturday, "Sabado" },
                { DayOfWeek.Sunday, "Domingo" }
            };

            // Obtén el nombre del día de la semana en español
            string diaActual = diasSemana[DateTime.Today.DayOfWeek];

            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = @"
            SELECT COUNT(*) 
            FROM Horario H
            INNER JOIN Empleado E ON E.idHorario = H.idHorario
            WHERE E.idEmpleado = @idEmpleado
            AND FIND_IN_SET(@diaActual, H.diaSemana) > 0";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                        cmd.Parameters.AddWithValue("@diaActual", diaActual);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error al verificar días laborales: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
            }
        }
        private bool HayPermiso(int idEmpleado)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = @"
                SELECT COUNT(*)
                FROM solicitudpermiso
                WHERE idEmpleado = @idEmpleado
                AND @FechaHoy BETWEEN FechaDeseadaInicial AND FechaDeseadaFinal
                AND Estado = 'Aceptada'"; // Solo considerar permisos aprobados

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                        cmd.Parameters.AddWithValue("@FechaHoy", DateTime.Today);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (MySqlException ex)
                {
                    // Manejo específico para errores de MySQL
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error en la base de datos: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
                catch (Exception ex)
                {
                    // Manejo general para otros tipos de excepciones
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error inesperado: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
            }
        }


        private bool EsVacacionColectiva()
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT COUNT(*) FROM vacacionescolectivas WHERE FechaVacaciom = @FechaHoy";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        // Pasar la fecha como DateTime directamente
                        cmd.Parameters.AddWithValue("@FechaHoy", DateTime.Today);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (MySqlException ex)
                {
                    // Manejo específico para errores de MySQL
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error en la base de datos: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
                catch (Exception ex)
                {
                    // Manejo general para otros tipos de excepciones
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: 'Error inesperado: {ex.Message}', icon: 'error', timer: 2500, showConfirmButton: false }});", true);
                    return false;
                }
            }
        }

        private void CargarHorarioEmpleado()
        {
            // Diccionario para mapear DayOfWeek a nombres en español
            var diasSemana = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "Lunes" },
                { DayOfWeek.Tuesday, "Martes" },
                { DayOfWeek.Wednesday, "Miércoles" },
                { DayOfWeek.Thursday, "Jueves" },
                { DayOfWeek.Friday, "Viernes" },
                { DayOfWeek.Saturday, "Sábado" },
                { DayOfWeek.Sunday, "Domingo" }
            };

            // Obtener el nombre del día de la semana en español
            string diaActual = diasSemana[DateTime.Today.DayOfWeek];

            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEmpleado))
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    try
                    {
                        conexion.Open();

                        string query = @"
                SELECT H.diaSemana, H.horaEntrada, H.horaSalida 
                FROM Horario H 
                INNER JOIN Empleado E ON E.idHorario = H.idHorario 
                WHERE E.idEmpleado = @idEmpleado
                AND FIND_IN_SET(@diaActual, H.diaSemana) > 0";

                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                            cmd.Parameters.AddWithValue("@diaActual", diaActual);

                            using (MySqlDataReader lector = cmd.ExecuteReader())
                            {
                                if (lector.Read())
                                {
                                    string diaSemana = lector["diaSemana"].ToString();
                                    TimeSpan horaEntrada = (TimeSpan)lector["horaEntrada"];
                                    TimeSpan horaSalida = (TimeSpan)lector["horaSalida"];

                                    // Asignar valores a los labels
                                    lblDiaSemana.Text = $"{diaActual}";
                                    lblHorario.Text = $"{horaEntrada.ToString(@"hh\:mm")} am - {horaSalida.ToString(@"hh\:mm")} pm";
                                }
                                else
                                {
                                    lblDiaSemana.Text = diaActual;
                                    lblHorario.Text = "Hoy no es un día laboral";
                                }
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        lblDiaSemana.Text = $"Error en la base de datos: {ex.Message}";
                        lblHorario.Text = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        lblDiaSemana.Text = $"Error inesperado: {ex.Message}";
                        lblHorario.Text = string.Empty;
                    }
                }
            }
            else
            {
                lblDiaSemana.Text = "Información de usuario no disponible.";
                lblHorario.Text = string.Empty;
            }
        }
    }
}
