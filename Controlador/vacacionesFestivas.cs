using MySql.Data.MySqlClient;
using System;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Vacaciones
{
    public partial class vacacionesFestivas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submit_Click(object sender, EventArgs e)
        {
            DateTime fechaOriginalInicialTexto;
            DateTime fechaOriginalFinalTexto;
            DateTime fechaDeseadaInicialTexto;
            DateTime fechaDeseadaFinalTexto;

            // Inicializar las fechas a valores por defecto
            fechaOriginalInicialTexto = DateTime.MinValue;
            fechaOriginalFinalTexto = DateTime.MinValue;
            fechaDeseadaInicialTexto = DateTime.MinValue;
            fechaDeseadaFinalTexto = DateTime.MinValue;

            // Obtener y convertir el texto de los TextBox a DateTime
            bool fechaOriginalInicioValida = DateTime.TryParseExact(fechaOriginalInicial.Text, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out fechaOriginalInicialTexto);
            bool fechaOriginalFinalValida = DateTime.TryParseExact(fechaOriginalFinal.Text, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out fechaOriginalFinalTexto);
            bool fechaDeseadaInicioValida = DateTime.TryParseExact(fechaDeseadaInicial.Text, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out fechaDeseadaInicialTexto);
            bool fechaDeseadaFinalValida = DateTime.TryParseExact(fechaDeseadaFinal.Text, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out fechaDeseadaFinalTexto);

            // Validar que todas las fechas son válidas
            if (!fechaOriginalInicioValida || !fechaOriginalFinalValida || !fechaDeseadaInicioValida || !fechaDeseadaFinalValida)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Ingrese fechas válidas en el formato yyyy-MM-dd.')", true);
                return;
            }

            // Verificar que la fecha final original es posterior a la fecha inicial original
            if (fechaOriginalFinalTexto < fechaOriginalInicialTexto)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('La fecha final original debe ser posterior a la fecha inicial original.')", true);
                return;
            }

            // Verificar que la fecha final deseada es posterior a la fecha inicial deseada
            if (fechaDeseadaFinalTexto < fechaDeseadaInicialTexto)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('La fecha final deseada debe ser posterior a la fecha inicial deseada.')", true);
                return;
            }

            // Obtener el motivo
            string motivo = Txtmotivo.Text;

            // Obtener el valor de las cookies para idDepartamento
            string cookiesString = Request.Cookies["UserInfo"].Value;
            string[] keyValuePairs = cookiesString.Split('&');
            string idDepartamentoValue = null;

            foreach (string pair in keyValuePairs)
            {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2 && keyValue[0] == "idDepartamento")
                {
                    idDepartamentoValue = keyValue[1];
                    break;
                }
            }

            //  idDepartamento no sea nulo y sea un entero válido
            if (idDepartamentoValue == null || !int.TryParse(idDepartamentoValue, out int idDepartamento))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('No se pudo obtener el departamento.')", true);
                return;
            }

            // Conexión a la base de datos
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Verificar si las fechas deseadas están en VacacionesColectivas
                string checkQuery = @"
                SELECT COUNT(*) 
                FROM VacacionesColectivas 
                WHERE 
                    FechaVacacion BETWEEN @FechaDeseadaInicial AND @FechaDeseadaFinal";

                using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@FechaDeseadaInicial", fechaDeseadaInicialTexto.ToString("yyyy-MM-dd"));
                    checkCommand.Parameters.AddWithValue("@FechaDeseadaFinal", fechaDeseadaFinalTexto.ToString("yyyy-MM-dd"));

                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    // Si el conteo es mayor que 0, significa que ya existen fechas en VacacionesColectivas
                    if (count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Las fechas deseadas ya están registradas como vacaciones colectivas. No se puede solicitar en este rango.')", true);
                        return;
                    }
                }

                // Insertar las fechas en VacacionesColectivas
                DateTime fechaActual = fechaDeseadaInicialTexto;
                string insertQuery = @"
                INSERT INTO VacacionesColectivas (FechaVacacion, Motivo) 
                VALUES (@FechaVacacion, @Motivo)";

                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Motivo", motivo);

                    while (fechaActual <= fechaDeseadaFinalTexto)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@FechaVacacion", fechaActual.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Motivo", motivo);

                        command.ExecuteNonQuery();
                        fechaActual = fechaActual.AddDays(1);
                    }
                }

                // Insertar los rangos de fechas en DiasFestivos
                string insertDiasFestivosQuery = @"
                INSERT INTO DiasFestivos (FechaOriginalVacacion, FechaDeseadaVacacion, Motivo, idDepartamento) 
                VALUES (@FechaOriginalVacacion, @FechaDeseadaVacacion, @Motivo, @idDepartamento)";

                using (MySqlCommand command = new MySqlCommand(insertDiasFestivosQuery, connection))
                {
                    command.Parameters.AddWithValue("@FechaOriginalVacacion", fechaOriginalInicialTexto.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@FechaDeseadaVacacion", fechaDeseadaFinalTexto.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@Motivo", motivo);
                    command.Parameters.AddWithValue("@idDepartamento", idDepartamento);

                    command.ExecuteNonQuery();
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Días festivos registrados adecuadamente.')", true);
            }
        }
    }
}
