using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlEmpresarial.Vistas.Incapacidades
{
    public partial class SolicitarIncapacidad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTiposIncapacidades();
                // Obtener la cookie
                HttpCookie cookie = Request.Cookies["UserInfo"];
                if (cookie != null)
                {
                    // Extraer el valor de idEmpleado
                    string idEmpleadoValue = ConseguirCookie(cookie.Value, "idEmpleado");
                    string idDepartamentoValue = ConseguirCookie(cookie.Value, "idDpertamento");

                    // Asegúrate de que idEmpleadoValue no sea null y conviértelo a entero
                    if (idEmpleadoValue != null && int.TryParse(idEmpleadoValue, out int idEmpleado))
                    {
                        System.Diagnostics.Debug.WriteLine("idEmpleado extraído de la cookie: " + idEmpleado);
                        System.Diagnostics.Debug.WriteLine("idDepartamentoValue extraído de la cookie: " + idDepartamentoValue);
                    }
                    else
                    {
                        lblMensaje.Text = "Error: ID de empleado no válido.";
                    }
                }
                else
                {
                    lblMensaje.Text = "Error: Cookie no encontrada.";
                }
            }
        }

        private string ConseguirCookie(string cookieString, string key)
        {
            // Divide la cadena de cookie en pares clave-valor
            var pairs = cookieString.Split('&');
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2 && keyValue[0] == key)
                {
                    return keyValue[1];
                }
            }
            return null;
        }

        private void CargarTiposIncapacidades()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            DataTable dtPuestos = ObtenerTiposIncapacidadDesdeBaseDeDatos(connectionString);

            tipoIncapacidad.DataTextField = "Nombre";
            tipoIncapacidad.DataValueField = "idTipoIncapacidad";
            tipoIncapacidad.DataSource = dtPuestos;
            tipoIncapacidad.DataBind();

            // Agregar elemento por defecto al inicio
            tipoIncapacidad.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        private DataTable ObtenerTiposIncapacidadDesdeBaseDeDatos(string connectionString)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // Modifica la consulta para seleccionar tanto el idTipoIncapacidad como el Nombre
                string query = "SELECT idTipoIncapacidad, Nombre FROM TiposIncapacidad ORDER BY Nombre";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }

            // Verifica que los datos se han cargado correctamente
            foreach (DataRow row in dt.Rows)
            {
                System.Diagnostics.Debug.WriteLine("idTipoIncapacidad: " + row["idTipoIncapacidad"]);
                System.Diagnostics.Debug.WriteLine("Nombre: " + row["Nombre"]);
            }

            return dt;
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            // Inicializar variables para las fechas
            DateTime fechaInicioTexto;
            DateTime fechaFinalTexto;

            // Inicializar las fechas con valores predeterminados
            fechaInicioTexto = DateTime.MinValue;
            fechaFinalTexto = DateTime.MinValue;

            // Obtener el texto de los TextBox y convertir a DateTime
            bool fechaInicioValida = DateTime.TryParse(fechaInicio.Text, out fechaInicioTexto);
            bool fechaFinalValida = DateTime.TryParse(fechaFinal.Text, out fechaFinalTexto);

            // Verificar si las fechas son válidas
            if (!fechaInicioValida || !fechaFinalValida)
            {
                lblMensaje.Text = "Error: Formato de fecha no válido.";
                lblMensaje.Visible = true;
                return;
            }

            // Verificar que la fecha final sea después de la fecha de inicio
            if (fechaFinalTexto < fechaInicioTexto)
            {
                lblMensaje.Text = "Error: La fecha final debe ser posterior a la fecha de inicio.";
                lblMensaje.Visible = true;
                return;
            }

            // Obtener el resto de los datos del formulario
            if (int.TryParse(ConseguirCookie(Request.Cookies["UserInfo"].Value, "idEmpleado"), out int idEmpleado) &&
                int.TryParse(ConseguirCookie(Request.Cookies["UserInfo"].Value, "idDepartamento"), out int idDepartamento) &&
                int.TryParse(tipoIncapacidad.SelectedValue, out int idTipoIncapacidad))
            {
                string evidencia = Evidencia.Text;

                // Llamar al método para insertar los datos
                InsertarIncapacidad(idEmpleado, idDepartamento, idTipoIncapacidad, fechaInicioTexto, fechaFinalTexto, evidencia);
            }
            else
            {
                lblMensaje.Text = "Error en los datos del formulario. Por favor, revise e intente de nuevo.";
                lblMensaje.Visible = true;
            }
        }


        private void InsertarIncapacidad(int idEmpleado, int idDepartamento, int idTipoIncapacidad, DateTime fechaInicial, DateTime fechaFinal, string evidencia)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;

            // Consulta SQL para insertar datos
            string query = @"
        INSERT INTO Incapacidades (idEmpleado, idDepartamento, idTipoIncapacidad, FechaInicial, FechaFinal, Evidencia, Estado)
        VALUES (@idEmpleado, @idDepartamento, @idTipoIncapacidad, @FechaInicial, @FechaFinal, @Evidencia, 'Pendiente')";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                // Añadir parámetros para evitar SQL Injection
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);
                cmd.Parameters.AddWithValue("@idTipoIncapacidad", idTipoIncapacidad);
                cmd.Parameters.AddWithValue("@FechaInicial", fechaInicial);
                cmd.Parameters.AddWithValue("@FechaFinal", fechaFinal);
                cmd.Parameters.AddWithValue("@Evidencia", evidencia);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblMensaje.Text = "La solicitud de incapacidad ha sido registrada con éxito.";
                    lblMensaje.Visible = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error al insertar la incapacidad: " + ex.Message);
                    lblMensaje.Text = "Error al registrar la solicitud de incapacidad. Inténtelo de nuevo.";
                    lblMensaje.Visible = true;
                }
            }
        }


    }
}
