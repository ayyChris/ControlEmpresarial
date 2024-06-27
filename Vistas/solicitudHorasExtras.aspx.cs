using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas
{
    public partial class solicitudHorasExtras : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                colaborador.Items.Add(new ListItem("Colaborador 1", "1"));
                colaborador.Items.Add(new ListItem("Colaborador 2", "2"));
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {

        }
    }
}