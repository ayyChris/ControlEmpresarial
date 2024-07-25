using ControlEmpresarial.Controlador;
using ControlEmpresarial.Services;
using ControlEmpresarial.Vistas.Pagina_Principal;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas
{
    public partial class PermisosColaborador : Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarNotificaciones();
            if (!IsPostBack)
            {
                CargarNombreUsuario();
               
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
        protected void submit_Click(object sender, EventArgs e)
        {
            string fechaInicio = txtInicio.Text;
            string fechaFinal = txtFinal.Text;
            string tipo = txtTipo.Text;
            string motivo = txtMotivo.Text;
            DateTime fecha = DateTime.Today;

            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEmpleado))
            {
                solicitarPermiso(idEmpleado, fecha, fechaInicio, fechaFinal, tipo, motivo);

                lblMensaje.ForeColor = System.Drawing.Color.Green;
                lblMensaje.Text = "Permiso enviado correctamente!";
                lblMensaje.Visible = true;

                // Insertar la notificación
                InsertarNotificacion(Convert.ToInt32(idEmpleado), "Permiso Solicitado", motivo, fecha);

                ClientScript.RegisterStartupScript(this.GetType(), "showLikeIcon", "<script>document.getElementById('likeIcon').style.display = 'inline-block';</script>");
            }
            else
            {
                lblMensaje.Text = "Error al enviar permiso";
                lblMensaje.Visible = true;
            }
        }

        private void solicitarPermiso(int idEmpleado, DateTime fechaPublicada, string fechaInicioStr, string fechaFinalStr, string tipo, string motivo)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = "INSERT INTO solicitudpermiso (idEmpleado, FechaPublicada, FechaDeseadaInicial, FechaDeseadaFinal, Tipo, Estado, Motivo) VALUES (@idEmpleado, @FechaPublicada, @FechaInicio, @FechaFinal, @Tipo, 'Pendiente', @Motivo)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        // Convertir las fechas de string a DateTime
                        DateTime fechaInicio = DateTime.Parse(fechaInicioStr);
                        DateTime fechaFinal = DateTime.Parse(fechaFinalStr);

                        cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                        cmd.Parameters.AddWithValue("@FechaPublicada", fechaPublicada);
                        cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@FechaFinal", fechaFinal);
                        cmd.Parameters.AddWithValue("@Tipo", tipo);
                        cmd.Parameters.AddWithValue("@Motivo", motivo);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = $"Error: {ex.Message}";
                    lblMensaje.Visible = true;
                }
            }
        }

        private void InsertarNotificacion(int idEmpleado, string titulo, string motivo, DateTime fecha)
        {
            HttpCookie cookie = Request.Cookies["UserInfo"];
            int idEnviador = Convert.ToInt32(cookie["idEmpleado"]);

            notificacionService.InsertarNotificacion(idEnviador, idEmpleado, titulo, motivo, fecha);
        }
    }
}
