using MySql.Data.MySqlClient;
using System;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas
{
    public partial class DobleFactor : Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submit_Click(object sender, EventArgs e)
        {
            // Obtener el código ingresado por el usuario
            string codigoIngresado = Codigo.Text.Trim();

            // Obtener el idEmpleado de la cookie
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null && !string.IsNullOrEmpty(cookie["idEmpleado"]))
            {
                int idEmpleado;
                if (int.TryParse(cookie["idEmpleado"], out idEmpleado))
                {
                    // Verificar el código en la base de datos
                    if (VerificarCodigo(idEmpleado, codigoIngresado))
                    {
                        // Obtener el idPuesto desde las cookies
                        string idPuestoCookie = cookie["idPuesto"];

                        // Redirigir según el rol del usuario
                        switch (idPuestoCookie)
                        {
                            case "1":
                                Response.Redirect("../PaginaPrincipal/MenuJefatura.aspx");
                                break;
                            case "2":
                                Response.Redirect("../PaginaPrincipal/MenuColaborador.aspx");
                                break;
                            case "3":
                                Response.Redirect("../PaginaPrincipal/MenuSupervisor.aspx");
                                break;
                            default:
                                lblMensaje.Text = "Rol de usuario no reconocido.";
                                lblMensaje.Visible = true;
                                break;
                        }
                    }
                    else
                    {
                        // Código inválido, notificar error
                        lblMensaje.Text = "Código de verificación inválido.";
                        lblMensaje.ForeColor = System.Drawing.Color.Red;
                        lblMensaje.Visible = true;
                    }
                }
                else
                {
                    lblMensaje.Text = "Error al procesar el id del empleado.";
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                    lblMensaje.Visible = true;
                }
            }
            else
            {
                lblMensaje.Text = "No se encontró la información del empleado en la cookie.";
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                lblMensaje.Visible = true;
            }
        }

        private bool VerificarCodigo(int idEmpleado, string codigoIngresado)
        {
            string query = "SELECT COUNT(*) FROM Codigo WHERE idEmpleado = @idEmpleado AND Codigo = @Codigo";

            using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                        command.Parameters.AddWithValue("@Codigo", codigoIngresado);

                        // Ejecutar la consulta y obtener el conteo de coincidencias
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        return count > 0; // Retorna true si hay coincidencias, false de lo contrario
                    }
                }
                catch (Exception ex)
                {
                    // Manejar excepciones
                    lblMensaje.Text = $"Error al verificar el código: {ex.Message}";
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                    lblMensaje.Visible = true;
                    return false;
                }
            }
        }
    }
}
