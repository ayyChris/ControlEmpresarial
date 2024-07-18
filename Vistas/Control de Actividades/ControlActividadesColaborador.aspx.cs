using System;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Control_de_Actividades
{
    public partial class ControlActividadesColaborador : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicialización si es necesario
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            // Recoger los valores de los controles del lado del servidor
            string inicioText = inicio.Text;
            string finalText = final.Text;
            string horasText = horas.Text;
            string actividadText = actividad.Text;

            debugLabel.Text = $"Inicio: {inicioText}<br/>Final: {finalText}<br/>Horas: {horasText}<br/>Actividad: {actividadText}";

            // Lógica adicional para guardar datos en la base de datos o procesar los datos
        }
    }
}
