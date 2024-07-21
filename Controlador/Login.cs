using MySql.Data.MySqlClient;
using System;
using System.Net.Mail;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas
{
    public partial class Login : Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Eliminar todas las cookies
                if (Request.Cookies["UserInfo"] != null)
                {
                    HttpCookie cookie = new HttpCookie("UserInfo");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string cedula = txtCedula.Text;
                    string contrasena = txtContrasena.Text;

                    // Consulta para validar las credenciales del usuario
                    string consulta = "SELECT * FROM Empleado WHERE Cedula = @Cedula AND Contraseña = @Contraseña";
                    using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Cedula", cedula);
                        cmd.Parameters.AddWithValue("@Contraseña", contrasena);

                        using (MySqlDataReader lector = cmd.ExecuteReader())
                        {
                            if (lector.HasRows)
                            {
                                lector.Read();

                                // Obtener información del usuario
                                string idEmpleado = lector["idEmpleado"].ToString();
                                string nombre = lector["Nombre"].ToString();
                                string apellidos = lector["Apellidos"].ToString();
                                string correo = lector["Correo"].ToString();
                                string idPuesto = lector["idPuesto"].ToString();
                                string idDepartamento = lector["idDepartamento"].ToString();

                                // Crear cookie con información del usuario
                                HttpCookie userCookie = new HttpCookie("UserInfo");
                                userCookie["idEmpleado"] = idEmpleado;
                                userCookie["Nombre"] = nombre;
                                userCookie["Apellidos"] = apellidos;
                                userCookie["Cedula"] = cedula;
                                userCookie["Correo"] = correo;
                                userCookie["idPuesto"] = idPuesto;
                                userCookie["idDepartamento"] = idDepartamento;
                                userCookie.Expires = DateTime.Now.AddMinutes(30); // Expira en 30 minutos
                                Response.Cookies.Add(userCookie);

                                // Generar un código de verificación
                                string codigoVerificacion = GenerarCodigoVerificacion();

                                // Eliminar códigos anteriores para el mismo empleado
                                EliminarCodigosPrevios(idEmpleado);

                                // Insertar el código en la base de datos
                                InsertarCodigo(idEmpleado, codigoVerificacion);

                                // Enviar el código al correo del usuario
                                //EnviarCodigoVerificacion(correo, codigoVerificacion);

                                // Redirigir al usuario a la página de verificación
                                Response.Redirect("../PaginaPrincipal/DobleFactor.aspx");
                            }
                            else
                            {
                                // Mostrar mensaje de error
                                // ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Credenciales inválidas');", true);
                                lblMensaje.Text = "Cedula y/o contraseña inválida.";
                                lblMensaje.ForeColor = System.Drawing.Color.Red;
                                lblMensaje.Visible = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error: {ex.Message}');", true);
                }
            }
        }

        private string GenerarCodigoVerificacion()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString();
        }

        private void EnviarCodigoVerificacion(string correo, string codigoVerificacion)
        {
            MailMessage correoMensaje = new MailMessage("activitysync.apsw@gmail.com", correo);
            correoMensaje.Subject = "Código de Verificación";
            correoMensaje.Body = $"Tu código de verificación es: {codigoVerificacion}";

            SmtpClient clienteSmtp = new SmtpClient("smtp.gmail.com"); // Configura tu servidor SMTP
            clienteSmtp.Port = 587; // Puerto SMTP
            clienteSmtp.Credentials = new System.Net.NetworkCredential("activitysync.apsw@gmail.com", "ASPW2024");
            clienteSmtp.EnableSsl = true;

            try
            {
                clienteSmtp.Send(correoMensaje);
            }
            catch (Exception ex)
            {
                // Manejar errores al enviar el correo
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error al enviar el código de verificación: {ex.Message}');", true);
            }
        }

        public void InsertarCodigo(string idEmpleado, string codigo)
        {
            //eliminar token previos
            string query = "INSERT INTO Codigo (idEmpleado, Codigo) VALUES (@idEmpleado, @Codigo)";

            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    using (MySqlCommand command = new MySqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                        command.Parameters.AddWithValue("@Codigo", codigo);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public void EliminarCodigosPrevios(string idEmpleado)
        {
            string query = "DELETE FROM Codigo WHERE idEmpleado = @idEmpleado";

            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    using (MySqlCommand command = new MySqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                        // Ejecutar la consulta de eliminación. No afectará si no hay registros.
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
