using ControlEmpresarial.Controlador;
using ControlEmpresarial.Services;
using ControlEmpresarial.Vistas.Pagina_Principal;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas
{
    public partial class PermisosColaborador : Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private NotificacionService notificacionService = new NotificacionService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtInicio.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
                txtFinal.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
                CargarTiposPermiso(); // Cargar los tipos de permiso en el dropdown
            }
        }

        private void CargarTiposPermiso()
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT idPermiso, Tipo FROM tipoPermiso";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            ddlTipoPermiso.DataSource = reader;
                            ddlTipoPermiso.DataTextField = "Tipo";
                            ddlTipoPermiso.DataValueField = "idPermiso";
                            ddlTipoPermiso.DataBind();
                        }
                    }
                    ddlTipoPermiso.Items.Insert(0, new ListItem("-- Seleccione un tipo de permiso --", "0"));
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = $"Error al cargar tipos de permiso: {ex.Message}";
                    lblMensaje.Visible = true;
                }
            }
        }


        protected void submit_Click(object sender, EventArgs e)
        {
            string fechaInicio = txtInicio.Text;
            string fechaFinal = txtFinal.Text;
            string tipo = ddlTipoPermiso.SelectedItem.Text; // Obtener el tipo de permiso desde el dropdown
            string motivo = txtMotivo.Text;
            DateTime fecha = DateTime.Today;

            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null && int.TryParse(cookie["idEmpleado"], out int idEmpleado))
            {
                solicitarPermiso(idEmpleado, fecha, fechaInicio, fechaFinal, tipo, motivo);

                lblMensaje.ForeColor = System.Drawing.Color.Green;
                lblMensaje.Text = "Permiso enviado correctamente!";
                lblMensaje.Visible = true;

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
