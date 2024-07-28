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
            CargarEstadoMarcas();
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
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEmpleado))
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    try
                    {
                        conexion.Open();
                        string query = "SELECT MarcacionEntrada, MarcacionSalida FROM entradas WHERE idEmpleado = @idEmpleado AND DiaMarcado = @DiaMarcado";

                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                            cmd.Parameters.AddWithValue("@DiaMarcado", DateTime.Today);

                            using (MySqlDataReader lector = cmd.ExecuteReader())
                            {
                                if (lector.HasRows)
                                {
                                    lector.Read();
                                    bool entradaMarcada = lector.GetBoolean("MarcacionEntrada");
                                    bool salidaMarcada = lector.GetBoolean("MarcacionSalida");

                                    // Lógica de habilitación de botones
                                    if (entradaMarcada && salidaMarcada)
                                    {
                                        btnEntrada.Enabled = false;
                                        btnSalida.Enabled = false;
                                    }
                                    else if (entradaMarcada)
                                    {
                                        btnEntrada.Enabled = false;
                                        btnSalida.Enabled = true;
                                    }
                                    else
                                    {
                                        btnEntrada.Enabled = true;
                                        btnSalida.Enabled = false;
                                    }
                                }
                                else
                                {
                                    btnEntrada.Enabled = true;
                                    btnSalida.Enabled = false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMensaje.Text = $"Error al cargar estado de marcas: {ex.Message}";
                        lblMensaje.Visible = true;
                    }
                }
            }
            else
            {
                lblMensaje.Text = "Error al obtener la información del usuario.";
                lblMensaje.Visible = true;
            }
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

                        string query;
                        if (esEntrada)
                        {
                            query = "INSERT INTO entradas (idEmpleado, DiaMarcado, HoraEntrada, MarcacionEntrada) VALUES (@idEmpleado, @DiaMarcado, @HoraEntrada, 1) ON DUPLICATE KEY UPDATE HoraEntrada = @HoraEntrada, MarcacionEntrada = 1";
                        }
                        else
                        {
                            query = "UPDATE entradas SET HoraSalida = @HoraSalida, MarcacionSalida = 1 WHERE idEmpleado = @idEmpleado AND DiaMarcado = @DiaMarcado";
                        }

                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                            cmd.Parameters.AddWithValue("@DiaMarcado", DateTime.Today);

                            if (esEntrada)
                            {
                                cmd.Parameters.AddWithValue("@HoraEntrada", DateTime.Now.TimeOfDay);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@HoraSalida", DateTime.Now.TimeOfDay);
                            }

                            cmd.ExecuteNonQuery();
                        }

                        lblMensaje.Text = esEntrada ? "Entrada registrada correctamente." : "Salida registrada correctamente.";
                        lblMensaje.Visible = true;

                        CargarEstadoMarcas(); // Refrescar el estado de los botones
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
