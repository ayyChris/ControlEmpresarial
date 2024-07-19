<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgregarPuestoTrabajo.aspx.cs" Inherits="ControlEmpresarial.Vistas.Colaborador.AgregarPuestoTrabajo" %>

<!DOCTYPE html>
<html lang="es">
   <head runat="server">
      <meta charset="UTF-8">
      <meta name="viewport" content="width=device-width, initial-scale=1.0">
      <title>Solicitar Vacaciones</title>
      <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet">
      <link href="~/Estilos/app.css" type="text/css" rel="stylesheet">
      <style>
         .inicioSesion {
         border: none;
         border-bottom: 3px solid; 
         padding: 10px;
         width: 100%;
         box-sizing: border-box;
         outline: none; 
         }
         .inicioSesion:focus {
         border-bottom-color: #5E58F8; 
         }
         .boton-logo {
         background: none;
         border: none;
         cursor: pointer;
         display: flex;
         justify-content: center; 
         align-items: center; 
         }
         .boton-logo img {
         width: 130px; 
         height: 70px; 
         }
         .divisor-forma-personalizado {
         position: relative; 
         width: 100%;
         overflow: hidden;
         line-height: 0;
         transform: rotate(180deg);
         z-index: 1;
         margin-top: -300px;
         }
         .divisor-forma-personalizado svg {
         display: block;
         width: calc(100% + 1.3px);
         height: 405px;
         }
         .divisor-forma-personalizado .relleno-forma {
         fill: #5E58F8;
         }
         @media (min-width: 768px) and (max-width: 1023px) {
         .divisor-forma-personalizado svg {
         width: calc(100% + 1.3px);
         height: 500px;
         }
         }

         header nav ul {
list-style-type: none;
padding: 0;
margin: 0;
display: flex;
gap: 20px; 
}
header nav ul li {
position: relative;
}
header nav ul li a {
text-decoration: none;
padding: 10px;
display: block;
color: #000; 
}
header nav ul li.has-submenu .submenu {
display: none;
position: absolute;
top: 100%;
left: 0;
background-color: white;
box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
z-index: 1;
min-width: 200px;
}
header nav ul li.has-submenu:hover .submenu {
display: block;
color:#5E58F8;
}
header nav ul li.has-submenu .submenu li {
padding: 0;
}
header nav ul li.has-submenu .submenu li a {
padding: 10px;
color: black;
white-space: nowrap;
display: block;
}
header nav ul li.has-submenu .submenu li a:hover {
color: #5E58F8; /*color texto*/
}
      </style>
   </head>
   <body>
      <form id="form1" runat="server">
          <header>
    <div class="cabecera-izquierda">
       <h1>Supervisor</h1>
       <p>Jaim Martinez</p>
    </div>
    <nav>
   <ul>
      <li class="has-submenu">
         <a href="#">Rebajos</a>
         <ul class="submenu">
            <li><a href="../Rebajos/VisualizacionRebajos.aspx">Historial de Rebajos</a></li>
         </ul>
      </li>
      <li class="has-submenu">
         <a href="#">Vacaciones Colectivas</a>
         <ul class="submenu">
            <li><a href="../Vacaciones/vacacionColectiva.aspx">Solicitar Permiso</a></li>
         </ul>
      </li>
      <li class="has-submenu">
         <a href="#">Colaborador</a>
         <ul class="submenu">
            <li><a href="../Vacaciones/VisualizacionColaboradorSupervisor.aspx">Registrar Colaborador</a></li>
            <li><a href="#">Ver Colaboradores</a></li>
         </ul>
      </li>
      <li class="has-submenu">
         <a href="#">Incapacidades</a>
         <ul class="submenu">
            <li><a href="../Incapacidades/VisualizacionIncapacidad.aspx">Historial de incapacidades</a></li>
         </ul>
      </li>
      <li class="has-submenu">
         <a href="#">Inconsistencias</a>
         <ul class="submenu">
            <li><a href="../Inconsistencias/VisualizacionInconsistencias.aspx">Historial de inconsistencias</a></li>
         </ul>
      </li>
      <li class="has-submenu">
         <a href="#">Permisos</a>
         <ul class="submenu">
            <li><a href="../Permisos/VisualizacionPermisos.aspx">Historial de Permisos</a></li>
         </ul>
      </li>
      <li class="has-submenu">
         <a href="#">Reposiciones</a>
         <ul class="submenu">
            <li><a href="../Reposicion/.aspx">Historial de Reposiciones</a></li>
         </ul>
      </li>
   </ul>
</nav>
    <div class="cabecera-derecha">
       <button class="boton-notificacion">
       <img src="../../Imagenes/notificacion.gif" alt="Notificación"/>
       </button>
    </div>
 </header>
         <main>
            <section class="seccion-formulario">
               <div class="tarjeta-formulario-login" style="max-width: 700px; margin: 0 auto;">
                  <h2><span class="fuente-gruesa">Crea un</span><br><span class="fuente-delgada">Puesto de trabajo</span></h2>
                  <p></p>
                  <br />
                  <div>
                     <label class="fuente-delgada" for="cedula">Puesto de trabajo</label>
                     <asp:TextBox CssClass="inicioSesion" ID="puestoTrabajo" runat="server" placeholder="Ej.Jefatura"></asp:TextBox>
                  </div>
                  <br />
                  <br />
                  <br />
                  <asp:Button ID="ingresar" runat="server" Text="Ingresar" CssClass="button" OnClick="ingresar_Click" />
                  <br />
                  <asp:Button ID="volverMenu" runat="server" Text="Volver al menú" CssClass="button-blanco" OnClick="volverMenu_Click" />
               </div>
            </section>
         </main>
         <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
               <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
         </div>
      </form>
      <footer class="footer">
   <div>
      <h3>About</h3>
      <p>"ActivitySync proporciona una solución integral para la gestión eficiente de actividades dentro de tu empresa. Desde la 
         marcación de inconsistencias hasta el registro de entradas y salidas de empleados, así como el seguimiento detallado de 
         sus actividades diarias, nuestro software está diseñado para optimizar la productividad y mejorar la organización empresarial."
      </p>
   </div>
   <div>
      <h3>Tags</h3>
      <ul class="tag-list">
         <li><a href="#">Rebajos Extras</a></li>
         <li><a href="#">Vacaciones Colectivas</a></li>
      </ul>
   </div>
   <div>
      <h3>Contactenos</h3>
      <p>ActivitySync ofrece servicios al cliente cuales estén interesados a nuestra gestión de actividades</p>
      <p>Contactenos:</p>
      <ul>
         <li>Email: support@activitysync.com</li>
         <li>Phone: +1-800-123-4567</li>
      </ul>
   </div>
</footer>
   </body>
</html>