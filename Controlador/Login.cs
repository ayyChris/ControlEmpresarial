using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

namespace ControlEmpresarial.Vistas
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            string Pass = contrasena.Text;
            System.Diagnostics.Debug.WriteLine($"Valor de Pass: {Pass}");
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Login button clicked')", true);
        }
    }
}