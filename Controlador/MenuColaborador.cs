using MySql.Data.MySqlClient;
using System;
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
    }
}
