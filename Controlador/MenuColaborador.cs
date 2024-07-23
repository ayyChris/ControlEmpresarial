using ControlEmpresarial.Controlador;
using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas
{
    public partial class MenuColaborador : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarNombreUsuario();
                CargarNotificaciones();
            }
        }

        private void CargarNombreUsuario()
        {
            // Obtener el nombre de las cookies
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
