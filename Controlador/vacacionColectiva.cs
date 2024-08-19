using MySql.Data.MySqlClient;
using System;
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ingrese fechas válidas.')", true);
                return;
            }

            // Verificar que la fecha final no sea antes de la fecha de inicio
            if (fechaFinalTexto < fechaInicioTexto)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('La fecha final debe ser posterior a la fecha de inicio.')", true);
                return;
            }

            // Obtener el motivo
            string motivo = Txtmotivo.Text;

            // Conexión a la base de datos
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Verificar si ya existen fechas solapadas
                DateTime fechaActual = fechaInicioTexto;
                bool fechasSolapadas = false;

                while (fechaActual <= fechaFinalTexto)
                {
                    string checkQuery = @"
                    SELECT COUNT(*) 
                    FROM VacacionesColectivas 
                    WHERE FechaVacacion = @FechaVacacion";

                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@FechaVacacion", fechaActual.ToString("yyyy-MM-dd"));
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            fechasSolapadas = true;
                            break;
                        }
                    }

                    fechaActual = fechaActual.AddDays(1);
                }

                if (fechasSolapadas)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Una o más fechas ya están registradas. Intente con fechas diferentes.')", true);
                    return;
                }

                // Insertar cada fecha individual en la base de datos
                fechaActual = fechaInicioTexto;

                while (fechaActual <= fechaFinalTexto)
                {
                    string insertQuery = @"
                    INSERT INTO VacacionesColectivas (FechaVacacion, Motivo) 
                    VALUES (@FechaVacacion, @Motivo)";

                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@FechaVacacion", fechaActual.ToString("yyyy-MM-dd"));
                        insertCommand.Parameters.AddWithValue("@Motivo", motivo);

                        insertCommand.ExecuteNonQuery();
                    }

                    fechaActual = fechaActual.AddDays(1);
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Vacaciones registradas adecuadamente.')", true);
            }
        }
    }
}
