using MySql.Data.MySqlClient;
using System;
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
                CargarNombreUsuario();
            }
            CargarEstadoMarcas(); // Ahora siempre habilita ambos botones
            CargarHorarioEmpleado();
        }

        private void CargarNombreUsuario()
        {
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
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEmpleado))
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    try
                    {
                        conexion.Open();

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

                            lblMensaje.Text = "Entrada registrada correctamente.";
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

                                    lblMensaje.Text = "Salida registrada correctamente.";
                                }
                                else
                                {
                                    lblMensaje.Text = "No se encontró una entrada para registrar la salida.";
                                }
                            }
                        }

                        lblMensaje.Visible = true;
                        CargarEstadoMarcas(); // Actualizar el estado de los botones
                    }
                    catch (Exception ex)
                    {
                        lblMensaje.Text = $"Error: {ex.Message}";
                        lblMensaje.Visible = true;
                    }
                }
            }
            else
            {
                lblMensaje.Text = "Error al registrar la marca.";
                lblMensaje.Visible = true;
            }
        }


        private void CargarHorarioEmpleado()
        {
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
                    WHERE E.idEmpleado = @idEmpleado";

                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                            using (MySqlDataReader lector = cmd.ExecuteReader())
                            {
                                if (lector.Read())
                                {
                                    string diaSemana = lector["diaSemana"].ToString();
                                    TimeSpan horaEntrada = (TimeSpan)lector["horaEntrada"];
                                    TimeSpan horaSalida = (TimeSpan)lector["horaSalida"];

                                    // Asignar valores a los labels
                                    lblDiaSemana.Text = diaSemana;
                                    lblHorario.Text = $"{horaEntrada.ToString(@"hh\:mm")} am - {horaSalida.ToString(@"hh\:mm")} pm";
                                }
                                else
                                {
                                    lblDiaSemana.Text = "Horario no encontrado";
                                    lblHorario.Text = string.Empty;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblDiaSemana.Text = $"Error: {ex.Message}";
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
