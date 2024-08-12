using ControlEmpresarial.Services;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ControlEmpresarial.Controlador.Vacaciones
{
    public partial class calendarioVacacionesColaborador : Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";
        private DateTime FechaActual
        {
            get
            {
                if (ViewState["FechaActual"] == null)
                {
                    ViewState["FechaActual"] = DateTime.Now;
                }
                return (DateTime)ViewState["FechaActual"];
            }
            set
            {
                ViewState["FechaActual"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie cookie = Request.Cookies["UserInfo"];
                if (cookie != null)
                {
                    // Extraer el valor de idEmpleado
                    string idEmpleadoValue = ConseguirCookie(cookie.Value, "idEmpleado");

                    if (idEmpleadoValue != null && int.TryParse(idEmpleadoValue, out int idEmpleado))
                    {
                        System.Diagnostics.Debug.WriteLine("idEmpleado extraído de la cookie: " + idEmpleado);
                        RenderizarCalendario(FechaActual, idEmpleado);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Error: ID de empleado no válido.");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Error: Cookie no encontrada.");
                }
            }
        }

        protected void btnPrevMonth_Click(object sender, EventArgs e)
        {
            FechaActual = FechaActual.AddMonths(-1);
            // Intentar extraer idEmpleado de la cookie
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null)
            {
                string idEmpleadoValue = ConseguirCookie(cookie.Value, "idEmpleado");
                if (idEmpleadoValue != null && int.TryParse(idEmpleadoValue, out int idEmpleado))
                {
                    RenderizarCalendario(FechaActual, idEmpleado);
                }
            }
        }

        protected void btnNextMonth_Click(object sender, EventArgs e)
        {
            FechaActual = FechaActual.AddMonths(1);
            HttpCookie cookie = Request.Cookies["UserInfo"];
            if (cookie != null)
            {
                string idEmpleadoValue = ConseguirCookie(cookie.Value, "idEmpleado");
                if (idEmpleadoValue != null && int.TryParse(idEmpleadoValue, out int idEmpleado))
                {
                    RenderizarCalendario(FechaActual, idEmpleado);
                }
            }
        }

        protected void RenderizarCalendario(DateTime fecha,int idEmpleado)
        {
            monthYear.InnerText = fecha.ToString("MMMM yyyy");

            // Crear la lista de días del calendario
            var diasCalendario = new List<DiaCalendario>();
            int primerDiaDelMes = (int)new DateTime(fecha.Year, fecha.Month, 1).DayOfWeek;
            if (primerDiaDelMes == 0) primerDiaDelMes = 7; // Domingo como el primer día de la semana

            // Agregar días vacíos al principio del mes
            for (int i = 1; i < primerDiaDelMes; i++)
            {
                diasCalendario.Add(new DiaCalendario { Dia = "", Class = "" });
            }

            // Obtener días libres del empleado
            var diasLibres = ObtenerDiasLibres(fecha.Year, fecha.Month, idEmpleado);

            // Agregar días reales del mes
            int diasEnElMes = DateTime.DaysInMonth(fecha.Year, fecha.Month);
            for (int i = 1; i <= diasEnElMes; i++)
            {
                var claseDia = diasLibres.Contains(i) ? "dia-libres" : "";
                diasCalendario.Add(new DiaCalendario { Dia = i.ToString(), Class = claseDia });
            }

            // Enlazar los datos al repetidor
            calendarRepeater.DataSource = diasCalendario;
            calendarRepeater.DataBind();
        }

        protected HashSet<int> ObtenerDiasLibres(int anio, int mes, int idEmpleado)
        {
            var diasLibres = new HashSet<int>();

            string consulta = @"
        SELECT DAY(FechaVacacion) AS Dia
        FROM RespuestaVacaciones
        WHERE YEAR(FechaVacacion) = @Anio
        AND MONTH(FechaVacacion) = @Mes
        AND idEmpleado = @IdEmpleado
        AND Estado = 'Aceptada'";

            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Anio", anio);
                        cmd.Parameters.AddWithValue("@Mes", mes);
                        cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                diasLibres.Add(reader.GetInt32("Dia"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores en dias libres
                    System.Diagnostics.Debug.WriteLine("Error al obtener días libres: " + ex.Message);
                }
            }

            return diasLibres;
        }



        private string ConseguirCookie(string cookieString, string key)
        {
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

        protected class DiaCalendario
        {
            public string Dia { get; set; }
            public string Class { get; set; }
        }
    }
}