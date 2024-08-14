using System;
using System.Web;
using MySql.Data.MySqlClient;

namespace ControlEmpresarial.Vistas.Inconsistencias
{
    public partial class EvidenciaIncosistenciaColaborador : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Obtener el idInconsistencia de la URL
                string idInconsistencia = Request.QueryString["idInconsistencia"];
                if (!string.IsNullOrEmpty(idInconsistencia))
                {
                    hfIdInconsistencia.Value = idInconsistencia;

                    // Obtener el nombre y la descripción del tipo de inconsistencia
                    var tipoInconsistencia = ObtenerTipoInconsistencia(idInconsistencia);
                    lblSaludo.Text = "Hola, " + GetUserName() + "!"; // Método para obtener el nombre del usuario
                    lblPregunta.Text = $"¿Por qué tuviste esta inconsistencia?<br/>{tipoInconsistencia.Nombre}<br/> Descripción: {tipoInconsistencia.Descripcion}";
                }
                else
                {
                    lblError.Text = "No se ha especificado una inconsistencia.";
                }
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            // Obtener el idInconsistencia del HiddenField
            if (int.TryParse(hfIdInconsistencia.Value, out int idInconsistencia))
            {
                string justificacion = txtJustificacion.Text;

                // Validar que la justificación no esté vacía
                if (!string.IsNullOrWhiteSpace(justificacion))
                {
                    // Insertar la justificación en la base de datos
                    InsertarJustificacion(idInconsistencia, justificacion);

                    // Actualizar el estado de la inconsistencia
                    ActualizarEstadoInconsistencia(idInconsistencia);

                    // Mostrar un mensaje de éxito
                    lblSaludo.Text = "Justificación enviada.";
                }
                else
                {
                    lblError.Text = "La justificación no puede estar vacía.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                lblError.Text = "ID de inconsistencia no válido.";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }


        private void ActualizarEstadoInconsistencia(int idInconsistencia)
        {
            string query = @"
        UPDATE inconsistencias
        SET Estado = @estado
        WHERE idInconsistencia = @idInconsistencia";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@estado", "Justificada");
                    cmd.Parameters.AddWithValue("@idInconsistencia", idInconsistencia);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Manejar excepciones adecuadamente
                        lblError.Text = "Error al actualizar el estado de la inconsistencia: " + ex.Message;
                        lblError.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }


        private void InsertarJustificacion(int idInconsistencia, string justificacion)
        {
            string query = @"
        INSERT INTO justificacioninconsistencia (idInconsistencia, Justificacion, FechaJustificacion)
        VALUES (@idInconsistencia, @justificacion, @fechaJustificacion)";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idInconsistencia", idInconsistencia);
                    cmd.Parameters.AddWithValue("@justificacion", justificacion);
                    cmd.Parameters.AddWithValue("@fechaJustificacion", DateTime.Now.Date);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Manejar excepciones adecuadamente
                        lblError.Text = "Error al insertar la justificación: " + ex.Message;
                        lblError.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }


        private string GetUserName()
        {
            // Obtener la cookie del usuario
            HttpCookie userCookie = Request.Cookies["UserInfo"];
            if (userCookie != null)
            {
                string nombre = userCookie["Nombre"];
                return nombre ?? "Usuario";
            }
            return "Usuario";
        }

        private (string Nombre, string Descripcion) ObtenerTipoInconsistencia(string idInconsistencia)
        {
            string nombre = "Desconocido";
            string descripcion = "Descripción no disponible";

            // Consulta para obtener el idTipoInconsistencia basado en idInconsistencia
            string queryIdTipoInconsistencia = @"
        SELECT idTipoInconsistencia
        FROM inconsistencias
        WHERE idInconsistencia = @idInconsistencia";

            // Consulta para obtener el Nombre y Descripción basado en idTipoInconsistencia
            string queryTipoInconsistencia = @"
        SELECT Nombre, Descripcion
        FROM tiposinconsistencia
        WHERE idTipoInconsistencia = @idTipoInconsistencia";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Obtener el idTipoInconsistencia
                    int idTipoInconsistencia;
                    using (MySqlCommand cmd = new MySqlCommand(queryIdTipoInconsistencia, conn))
                    {
                        cmd.Parameters.AddWithValue("@idInconsistencia", idInconsistencia);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            idTipoInconsistencia = Convert.ToInt32(result);
                        }
                        else
                        {
                            // Si no se encuentra el idTipoInconsistencia
                            return (nombre, descripcion);
                        }
                    }

                    // Obtener el Nombre y Descripción usando idTipoInconsistencia
                    using (MySqlCommand cmd = new MySqlCommand(queryTipoInconsistencia, conn))
                    {
                        cmd.Parameters.AddWithValue("@idTipoInconsistencia", idTipoInconsistencia);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nombre = reader["Nombre"].ToString();
                                descripcion = reader["Descripcion"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejar excepciones adecuadamente
                    lblError.Text = "Error al obtener el tipo de inconsistencia: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }

            return (nombre, descripcion);
        }

    }
}
