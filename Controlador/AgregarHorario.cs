using MySql.Data.MySqlClient;
using System;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Colaborador
{
    public partial class AgregarHorario : Page
    {
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
                //  días no válidos
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error: {ex.Message}')", true);
            }
            catch (FormatException ex)
            {
                //  formato incorrecto de hora
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error en formato de hora: {ex.Message}')", true);
            }
            catch (MySqlException ex)
            {
                // SQL
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error de MySQL: {ex.Message} - Código: {ex.Number}')", true);
            }
            catch (Exception ex)
            {
                //  errores inesperados
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error inesperado: {ex.Message}')", true);
            }
        }



        private bool ValidarRangoDias(string diaInicio, string diaFin)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"checkpoint3");
                // convertir dias en español
                DayOfWeek diaInicioEnum = ConvertirDiaSemana(diaInicio);
                DayOfWeek diaFinEnum = ConvertirDiaSemana(diaFin);

                // 5 dias consecutivos
                int cantidadDias = (diaFinEnum - diaInicioEnum + 1 + 7) % 7; // Sumar 7 y luego tomar módulo 7 para manejar los casos circulares
                return cantidadDias == 5;
                
            }
            catch (ArgumentException ex)
            {
                // dia no valido
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error: {ex.Message}')", true);
                return false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error inesperado: {ex.Message}')", true);
                return false;
            }
        }



        private int CalcularCantidadHoras(string horaEntrada, string horaSalida)
        {
            System.Diagnostics.Debug.WriteLine($"checkpoint4");
            TimeSpan horaInicio = TimeSpan.Parse(horaEntrada);
            TimeSpan horaFinal = TimeSpan.Parse(horaSalida);

            // Calcular la diferencia de horas
            TimeSpan diferencia = horaFinal - horaInicio;

            // Retornar la cantidad de horas en formato entero
            return (int)diferencia.TotalHours;
        }

        private void InsertarHorario(string tipoJornada, string diaSemana, string horaEntrada, string horaSalida, int cantidadHoras, string connectionString)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO Horario (TipoJornada, diaSemana, horaEntrada, horaSalida, cantidadHoras) " +
                                   "VALUES (@tipoJornada, @diaSemana, @horaEntrada, @horaSalida, @cantidadHoras)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tipoJornada", tipoJornada);
                    cmd.Parameters.AddWithValue("@diaSemana", diaSemana);
                    cmd.Parameters.AddWithValue("@horaEntrada", horaEntrada);
                    cmd.Parameters.AddWithValue("@horaSalida", horaSalida);
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
            catch (MySqlException ex)
            {
                // SQL
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error al ingresar el horario: {ex.Message} - {ex.InnerException?.Message}')", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Error general al ingresar el horario: {ex.Message}')", true);
            }
        }



        private DayOfWeek ConvertirDiaSemana(string nombreDia)
        {
            System.Diagnostics.Debug.WriteLine($"checkpointdias");
            switch (nombreDia.ToLower()) // Convertir a minúsculas para evitar problemas de mayúsculas/minúsculas
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
