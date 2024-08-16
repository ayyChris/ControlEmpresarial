using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Incapacidades
{
    public partial class RespuestaIncapacidad : System.Web.UI.Page
    {
        private string cadenaConexion = "Server=138.59.135.33;Port=3306;Database=tiusr38pl_gestion;Uid=gestion;Pwd=Ihnu00&34;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idIncapacidad = Request.QueryString["idIncapacidad"];
                if (!string.IsNullOrEmpty(idIncapacidad))
                {
                    CargarDetalleSolicitud(idIncapacidad);
                }
            }
        }

        private void CargarDetalleSolicitud(string idIncapacidad)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = @"
                        SELECT 
                            i.idEmpleado, 
                            i.FechaInicial, 
                            i.FechaFinal, 
                            i.Evidencia, 
                            i.Estado
                        FROM 
                            Incapacidades i
                        WHERE 
                            i.idIncapacidad = @idIncapacidad";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idIncapacidad", idIncapacidad);

                        using (MySqlDataReader lector = cmd.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                int idEmpleado = Convert.ToInt32(lector["idEmpleado"]);
                                lblFechaInicial.Text = Convert.ToDateTime(lector["FechaInicial"]).ToString("dd/MM/yyyy");
                                lblFechaFinal.Text = Convert.ToDateTime(lector["FechaFinal"]).ToString("dd/MM/yyyy");
                                lblEvidencia.Text = lector["Evidencia"].ToString();
                                lblEstado.Text = lector["Estado"].ToString();

                                // Cargar el nombre completo del empleado
                                CargarNombreEmpleado(idEmpleado);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: '{ex.Message}', icon: 'error', timer: 1500, showConfirmButton: false }});", true);
                }
            }
        }

        private void CargarNombreEmpleado(int idEmpleado)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT Nombre, Apellidos FROM Empleado WHERE idEmpleado = @idEmpleado";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                        using (MySqlDataReader lector = cmd.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                string nombre = lector["Nombre"].ToString();
                                string apellidos = lector["Apellidos"].ToString();
                                lblEmpleado.Text = $"{nombre} {apellidos}";
                            }
                            else
                            {
                                lblEmpleado.Text = "Nombre no encontrado";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: '{ex.Message}', icon: 'error', timer: 1500, showConfirmButton: false }});", true);
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            ActualizarEstadoSolicitud("Aceptada");
        }

        protected void btnDenegar_Click(object sender, EventArgs e)
        {
            ActualizarEstadoSolicitud("Denegada");
        }

        private void ActualizarEstadoSolicitud(string nuevoEstado)
        {
            string idIncapacidad = Request.QueryString["idIncapacidad"];
            if (!string.IsNullOrEmpty(idIncapacidad))
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    try
                    {
                        conexion.Open();
                        string query = "UPDATE Incapacidades SET Estado = @nuevoEstado WHERE idIncapacidad = @idIncapacidad";
                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                            cmd.Parameters.AddWithValue("@idIncapacidad", idIncapacidad);
                            cmd.ExecuteNonQuery();
                        }

                        // Calcular y agregar el impacto monetario
                        CalcularImpactoMonetario(idIncapacidad, nuevoEstado);
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire({{ title: 'Error', text: '{ex.Message}', icon: 'error', timer: 1500, showConfirmButton: false }});", true);
                    }
                }
            }
        }

        private void CalcularImpactoMonetario(string idIncapacidad, string nuevoEstado)
        {
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    using (var transaction = conexion.BeginTransaction())
                    {
                        // Consulta para obtener fechas de la incapacidad
                        string query = @"
                    SELECT FechaInicial, FechaFinal
                    FROM Incapacidades
                    WHERE idIncapacidad = @idIncapacidad";
                        using (MySqlCommand cmd = new MySqlCommand(query, conexion, transaction))
                        {
                            cmd.Parameters.AddWithValue("@idIncapacidad", idIncapacidad);

                            using (MySqlDataReader lector = cmd.ExecuteReader())
                            {
                                if (lector.Read())
                                {
                                    DateTime fechaInicial = Convert.ToDateTime(lector["FechaInicial"]);
                                    DateTime fechaFinal = Convert.ToDateTime(lector["FechaFinal"]);
                                    int totalDias = (fechaFinal - fechaInicial).Days + 1;

                                    // Cerrar el DataReader antes de realizar la siguiente consulta
                                    lector.Close();

                                    // Determinar días con reducción a partir del cuarto día
                                    int diasConReduccion = totalDias > 4 ? totalDias - 4 : 0;
                                    decimal porcentajeReduccion = diasConReduccion > 0 ? 40.00m : 0.00m;

                                    // Insertar el impacto monetario en la base de datos
                                    string insertQuery = @"
                                INSERT INTO ImpactoMonetarioIncapacidad (idIncapacidad, DiasReduccidos, MontoReducido)
                                VALUES (@idIncapacidad, @diasReduccidos, @porcentajeReduccion)";
                                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conexion, transaction))
                                    {
                                        insertCmd.Parameters.AddWithValue("@idIncapacidad", idIncapacidad);
                                        insertCmd.Parameters.AddWithValue("@diasReduccidos", diasConReduccion);
                                        insertCmd.Parameters.AddWithValue("@porcentajeReduccion", porcentajeReduccion);
                                        insertCmd.ExecuteNonQuery();
                                    }

                                    // Actualizar la columna DiasReduccion en la tabla Incapacidades
                                    string updateQuery = "UPDATE Incapacidades SET DiasReduccion = @diasReduccidos WHERE idIncapacidad = @idIncapacidad";
                                    using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conexion, transaction))
                                    {
                                        updateCmd.Parameters.AddWithValue("@diasReduccidos", diasConReduccion);
                                        updateCmd.Parameters.AddWithValue("@idIncapacidad", idIncapacidad);
                                        updateCmd.ExecuteNonQuery();
                                    }

                                    // Commit de la transacción
                                    transaction.Commit();

                                    // Mostrar mensaje de éxito con Swal
                                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                                        $"Swal.fire({{ title: 'Éxito', text: 'El impacto monetario se ha registrado exitosamente.', icon: 'success', timer: 4500, showConfirmButton: false }});", true);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Rollback en caso de error
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        $"Swal.fire({{ title: 'Error', text: '{ex.Message}', icon: 'error', timer: 4500, showConfirmButton: false }});", true);
                }
            }
        }




    }
}
