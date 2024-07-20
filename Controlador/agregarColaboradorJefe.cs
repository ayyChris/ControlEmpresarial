using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace ControlEmpresarial.Vistas.Colaborador
{
    public partial class agregarColaboradorJefe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPuestos();
                CargarTiposJornada();
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"checkpoint1");
            string nombre = nombreUsuario.Text.Trim();
            string apellidos = apellidosUsuario.Text.Trim();
            string cedula = cedulaUsuario.Text.Trim();
            string correo = correoUsuario.Text.Trim();
            string contraseña = passwordUsuario.Text.Trim();
            int idPuesto = Convert.ToInt32(puesto.SelectedValue);
            int idHorario = Convert.ToInt32(horario.SelectedValue);
            System.Diagnostics.Debug.WriteLine(nombre);
            System.Diagnostics.Debug.WriteLine(apellidos);
            System.Diagnostics.Debug.WriteLine(cedula);
            System.Diagnostics.Debug.WriteLine(correo);
            System.Diagnostics.Debug.WriteLine(contraseña);
            System.Diagnostics.Debug.WriteLine(idPuesto);
            System.Diagnostics.Debug.WriteLine(idHorario);

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellidos) || string.IsNullOrEmpty(cedula) || string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contraseña) || idPuesto == 0 || idHorario == 0)
            {
                MostrarAlerta("Por favor complete todos los campos.");
                return;
            }

            // Valores quemados
            int diasDeVacaciones = 0;
            string estado = "Activo"; 
            int idDepartamento = 1; 

            // Guardar en la base de datos
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            try
            {
                GuardarEmpleadoEnBaseDeDatos(nombre, apellidos, cedula, correo, contraseña, diasDeVacaciones, estado, idPuesto, idHorario, idDepartamento, connectionString);

                // Mostrar mensaje de éxito
                MostrarAlerta("Empleado registrado correctamente.");
                LimpiarCampos();
            }
            catch (MySqlException ex)
            {
                MostrarAlerta($"Error de MySQL: {ex.Message} - Código: {ex.Number}");
            }
            catch (Exception ex)
            {
                MostrarAlerta($"Error al registrar empleado: {ex.Message}");
            }
        }

        private void CargarPuestos()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            DataTable dtPuestos = ObtenerPuestosDesdeBaseDeDatos(connectionString);

            puesto.DataTextField = "nombrePuesto";
            puesto.DataValueField = "idPuesto";
            puesto.DataSource = dtPuestos;
            puesto.DataBind();

            // Agregar elemento por defecto al inicio
            puesto.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        private void CargarTiposJornada()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            DataTable dtTiposJornada = ObtenerTiposJornadaDesdeBaseDeDatos(connectionString);

            horario.DataTextField = "TipoJornada";
            horario.DataValueField = "idHorario"; // Usaremos idHorario como valor
            horario.DataSource = dtTiposJornada;
            horario.DataBind();

            // Agregar elemento por defecto al inicio
            horario.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        private DataTable ObtenerPuestosDesdeBaseDeDatos(string connectionString)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idPuesto, nombrePuesto FROM PuestoTrabajo ORDER BY nombrePuesto";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        private DataTable ObtenerTiposJornadaDesdeBaseDeDatos(string connectionString)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT idHorario, TipoJornada FROM Horario ORDER BY TipoJornada";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        private void GuardarEmpleadoEnBaseDeDatos(string nombre, string apellidos, string cedula, string correo, string contraseña, int diasDeVacaciones, 
            string estado, int idPuesto, int idHorario, int idDepartamento, string connectionString)
        {
            System.Diagnostics.Debug.WriteLine($"checkpoint2");

            System.Diagnostics.Debug.WriteLine(nombre);
            System.Diagnostics.Debug.WriteLine(apellidos);
            System.Diagnostics.Debug.WriteLine(cedula);
            System.Diagnostics.Debug.WriteLine(correo);
            System.Diagnostics.Debug.WriteLine(contraseña);
            System.Diagnostics.Debug.WriteLine(diasDeVacaciones);
            System.Diagnostics.Debug.WriteLine(estado);
            System.Diagnostics.Debug.WriteLine(idPuesto);
            System.Diagnostics.Debug.WriteLine(idHorario);
            System.Diagnostics.Debug.WriteLine(idDepartamento);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Empleado (Nombre, Apellidos, Cedula, Correo, Contraseña, DiasDeVacaciones, Estado, idPuesto, idHorario, idDepartamento) " +
                               "VALUES (@Nombre, @Apellidos, @Cedula, @Correo, @Contraseña, @DiasDeVacaciones, @Estado, @idPuesto, @idHorario, @idDepartamento)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellidos", apellidos);
                cmd.Parameters.AddWithValue("@Cedula", cedula);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@Contraseña", contraseña);
                cmd.Parameters.AddWithValue("@DiasDeVacaciones", diasDeVacaciones);
                cmd.Parameters.AddWithValue("@Estado", estado);
                cmd.Parameters.AddWithValue("@idPuesto", idPuesto);
                cmd.Parameters.AddWithValue("@idHorario", idHorario);
                cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void LimpiarCampos()
        {
            nombreUsuario.Text = "";
            apellidosUsuario.Text = "";
            cedulaUsuario.Text = "";
            correoUsuario.Text = "";
            passwordUsuario.Text = "";
            puesto.SelectedIndex = 0;
            horario.SelectedIndex = 0; 
        }

        private void MostrarAlerta(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('{mensaje}');", true);
        }
    }
}
