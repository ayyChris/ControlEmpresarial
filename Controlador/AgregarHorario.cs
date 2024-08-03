using MySql.Data.MySqlClient;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Colaborador
{
    public partial class AgregarHorario : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
        protected void volverMenu_Click(object sender, EventArgs e)
        {

        }

        protected void ingresar_Click(object sender, EventArgs e)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;

            string tipoJornada = ddlTipoJornada.SelectedValue;
            string diaInicio = ddlDiaSemana1.SelectedValue;
            string diaFin = ddlDiaSemana2.SelectedValue;
            string horaEntrada = txtHoraEntrada.Text.Trim();
            string horaSalida = txtHoraSalida.Text.Trim();
            System.Diagnostics.Debug.WriteLine($"checkpoint1");

            try
            {
                // Validación: Verificar si los días seleccionados forman un rango de 5 días
                if (!ValidarRangoDias(diaInicio, diaFin))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('El rango de días debe ser de 5 días consecutivos (Lunes a Viernes, Martes a Sábado).')", true);
                    return;
                }

                // Validación: Calcular la cantidad de horas y verificar si es exactamente 8 horas
                int cantidadHoras = CalcularCantidadHoras(horaEntrada, horaSalida);
                if (cantidadHoras != 8)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('El horario debe ser exactamente de 8 horas.')", true);
                    return;
                }

                // Construir la cadena de días para base de datos
                string diaSemana = $"{diaInicio},{diaFin}";

                // Insertar el horario en la base de datos
                InsertarHorario(tipoJornada, diaSemana, horaEntrada, horaSalida, cantidadHoras, connectionString);
                System.Diagnostics.Debug.WriteLine($"checkpoint2");
            }
            catch (ArgumentException ex)
            {
                // Días no válidos
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error: {ex.Message}')", true);
            }
            catch (FormatException ex)
            {
                // Formato incorrecto de hora
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error en formato de hora: {ex.Message}')", true);
                System.Diagnostics.Debug.WriteLine($"Formato incorrecto de hora: {ex.Message}");
            }
            catch (MySqlException ex)
            {
                // Error de MySQL
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error de MySQL: {ex.Message} - Código: {ex.Number}')", true);
                System.Diagnostics.Debug.WriteLine($"Error de MySQL: {ex.Message} - Código: {ex.Number}");
            }
            catch (Exception ex)
            {
                // Otros errores inesperados
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error inesperado: {ex.Message}')", true);
                System.Diagnostics.Debug.WriteLine($"Error inesperado: {ex.Message}");
            }
        }

        private bool ValidarRangoDias(string diaInicio, string diaFin)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"checkpoint3");
                // Convertir días a enum DayOfWeek
                DayOfWeek diaInicioEnum = ConvertirDiaSemana(diaInicio);
                DayOfWeek diaFinEnum = ConvertirDiaSemana(diaFin);

                // Validar que sean 5 días consecutivos
                int cantidadDias = (diaFinEnum - diaInicioEnum + 1 + 7) % 7;
                return cantidadDias == 5;
            }
            catch (ArgumentException ex)
            {
                // Día no válido
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error: {ex.Message}')", true);
                return false;
            }
            catch (Exception ex)
            {
                // Otros errores inesperados
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error inesperado: {ex.Message}')", true);
                return false;
            }
        }

        private int CalcularCantidadHoras(string horaEntrada, string horaSalida)
        {
            System.Diagnostics.Debug.WriteLine($"checkpoint4");
            // Calcular diferencia de horas
            TimeSpan horaInicio = TimeSpan.Parse(horaEntrada);
            TimeSpan horaFinal = TimeSpan.Parse(horaSalida);
            TimeSpan diferencia = horaFinal - horaInicio;

            // Retornar cantidad de horas en formato entero
            return (int)diferencia.TotalHours;
        }

        private void InsertarHorario(string tipoJornada, string diaSemana, string horaEntrada, string horaSalida, int cantidadHoras, string connectionString)
        {
            try
            {
                // Convertir las cadenas de hora al formato SQL requerido
                string horaEntradaSQL = ConvertirHoraParaSQL(horaEntrada);
                string horaSalidaSQL = ConvertirHoraParaSQL(horaSalida);

                // Debug para verificar los valores antes de la inserción
                System.Diagnostics.Debug.WriteLine(tipoJornada);
                System.Diagnostics.Debug.WriteLine(diaSemana);
                System.Diagnostics.Debug.WriteLine(horaEntradaSQL);
                System.Diagnostics.Debug.WriteLine(horaSalidaSQL);
                System.Diagnostics.Debug.WriteLine(cantidadHoras);

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO Horario (TipoJornada, diaSemana, horaEntrada, horaSalida, cantidadHoras) " +
                                   "VALUES (@TipoJornada, @diaSemana, @horaEntrada, @horaSalida, @cantidadHoras)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TipoJornada", tipoJornada);
                    cmd.Parameters.AddWithValue("@diaSemana", diaSemana);
                    cmd.Parameters.AddWithValue("@horaEntrada", horaEntradaSQL); //formato hh:mm:ss
                    cmd.Parameters.AddWithValue("@horaSalida", horaSalidaSQL);   //formato hh:mm:ss
                    cmd.Parameters.AddWithValue("@cantidadHoras", cantidadHoras);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Horario ingresado correctamente.')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('No se pudo ingresar el horario.')", true);
                    }
                }
            }
            catch (FormatException ex)
            {
                // Capturar error de formato de hora
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error en formato de hora: {ex.Message}')", true);
            }
            catch (MySqlException ex)
            {
                // Capturar error específico de MySQL
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error al ingresar el horario: {ex.Message} - {ex.InnerException?.Message}')", true);
            }
            catch (Exception ex)
            {
                // Capturar otros errores generales
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error general al ingresar el horario: {ex.Message}')", true);
            }
        }

        private string ConvertirHoraParaSQL(string hora)
        {
            try
            {
                // Parsear la cadena de hora en un TimeSpan
                TimeSpan tiempo = TimeSpan.ParseExact(hora, "hh\\:mm", CultureInfo.InvariantCulture);

                // Formatear el TimeSpan en formato "hh:mm:ss"
                return tiempo.ToString(@"hh\:mm\:ss");
            }
            catch (FormatException ex)
            {
                // Capturar errores de formato y relanzar excepción
                throw new FormatException("Formato incorrecto de hora. Debe ser hh:mm.", ex);
            }
            catch (Exception ex)
            {
                // Capturar otros errores inesperados
                throw new Exception("Error al convertir hora para SQL.", ex);
            }
        }

        private DayOfWeek ConvertirDiaSemana(string nombreDia)
        {
            System.Diagnostics.Debug.WriteLine($"checkpointdias");
            switch (nombreDia.ToLower())
            {
                case "lunes":
                    return DayOfWeek.Monday;
                case "martes":
                    return DayOfWeek.Tuesday;
                case "miércoles":
                    return DayOfWeek.Wednesday;
                case "jueves":
                    return DayOfWeek.Thursday;
                case "viernes":
                    return DayOfWeek.Friday;
                case "sábado":
                    return DayOfWeek.Saturday;
                case "domingo":
                    return DayOfWeek.Sunday;
                default:
                    throw new ArgumentException($"El día '{nombreDia}' no es válido.");
            }
        }
        
    }
}
