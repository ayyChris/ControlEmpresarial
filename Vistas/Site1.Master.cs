using ControlEmpresarial.Controlador;
using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosDeUsuario();
                CargarNotificaciones();
            }
        }
        private void CargarDatosDeUsuario()
        {
            // Obtener la cookie de usuario
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null)
            {
                string nombre = cookie["Nombre"];
                string apellidos = cookie["Apellidos"];
                string idDepartamento = cookie["idDepartamento"];

                // Mostrar nombre completo
                lblNombre.Text = $"{nombre} {apellidos}";
                lblNombre.Visible = true;

                // Obtener el nombre del departamento
                if (!string.IsNullOrEmpty(idDepartamento))
                {
                    using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                    {
                        try
                        {
                            conexion.Open();
                            string query = "SELECT nombreDepartamento FROM departamento WHERE idDepartamento = @idDepartamento";

                            using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                            {
                                cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);
                                object result = cmd.ExecuteScalar();

                                if (result != null)
                                {
                                    string nombreDepartamento = result.ToString();
                                    lblNombre.Text = $"{nombreDepartamento} | {nombre} {apellidos}";
                                }
                                else
                                {
                                    lblNombre.Text = $"Error al obtener el departamento | {nombre} {apellidos}";
                                }
                            }
                        }
                        catch (MySqlException ex)
                        {
                            lblNombre.Text = $"Error en la base de datos: {ex.Message}";
                        }
                        catch (Exception ex)
                        {
                            lblNombre.Text = $"Error inesperado: {ex.Message}";
                        }
                    }
                }
                else
                {
                    lblNombre.Text = $"Error al obtener el departamento | {nombre} {apellidos}";
                }
            }
            else
            {
                lblNombre.Text = "Información de usuario no disponible.";
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
                    lblNombre.Text = "Error al extraer ID de empleado";
                    lblNombre.Visible = true;
                }
            }
            else
            {
                lblNombre.Text = "Cookie no encontrada";
                lblNombre.Visible = true;
            }
        }
    }
}