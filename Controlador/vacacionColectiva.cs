using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas
{
        public partial class VacacionColectiva : Page
        {
            protected void submit_Click(object sender, EventArgs e)
            {
                DateTime fechaInicioTexto;
                DateTime fechaFinalTexto;

                fechaInicioTexto = DateTime.MinValue;
                fechaFinalTexto = DateTime.MinValue;

                // Obtener el texto de los TextBox y convertir a DateTime
                bool fechaInicioValida = DateTime.TryParse(fechaInicio.Text, out fechaInicioTexto);
                bool fechaFinalValida = DateTime.TryParse(fechaFinal.Text, out fechaFinalTexto);

                // Validar que las fechas son válidas
                if (!fechaInicioValida || !fechaFinalValida)
                {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ingrese vacaciones validas.')", true);
                return;
                }

                // Verificar que la fecha final no sea antes de la fecha de inicio
                if (fechaFinalTexto < fechaInicioTexto)
                {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('La fecha posterior debe ser posterior a la fecha de inicio.')", true);
                return;
            }

                // Obtener el motivo
                string motivo = Txtmotivo.Text;

                // Conexión a la base de datos
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Insertar cada fecha individual en la base de datos
                    DateTime fechaActual = fechaInicioTexto;
                    while (fechaActual <= fechaFinalTexto)
                    {
                        string insertQuery = @"
                        INSERT INTO VacacionesColectivas (FechaVacacion, Motivo) 
                        VALUES (@FechaVacacion, @Motivo)";

                        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@FechaVacacion", fechaActual.ToString("yyyy-MM-dd"));
                            command.Parameters.AddWithValue("@Motivo", motivo);

                            command.ExecuteNonQuery();
                        }

                        fechaActual = fechaActual.AddDays(1);
                    }

                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Vacaciones registradas adecuadamente.')", true);
                }
            }
        }
    }