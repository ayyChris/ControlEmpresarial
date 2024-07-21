<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.Pagina_Principal.MenuColaborador" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
   <head runat="server">
      <title>ActivitySync</title>
      <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet"/>
      <link href="../../Estilos/app.css" rel="stylesheet"/>
      <style>
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
               <h1>Colaborador</h1>
               <p>Christian Barquero</p>
            </div>
            <nav>
               <ul>
                  <li class="has-submenu">
                     <a href="#">Horas Extras</a>
                     <ul class="submenu">
                        <li><a href="../Horas Extra/PreAceptacionHorasExtra.aspx">Solicitudes Horas Extras</a></li>
                     </ul>
                  </li>
                  <li class="has-submenu">
                     <a href="#">Permisos</a>
                     <ul class="submenu">
                        <li><a href="../Permisos/PermisosColaborador.aspx">Solicitar Permiso</a></li>
                     </ul>
                  </li>
                  <li class="has-submenu">
                     <a href="#">Inconsistencias</a>
                     <ul class="submenu">
                        <li><a href="#">Justificar Inconsistencia.</a></li>
                     </ul>
                  </li>
                  <li class="has-submenu">
                     <a href="#">Reposiciones</a>
                     <ul class="submenu">
                        <li><a href="#">Revisar Reposiciones</a></li>
                     </ul>
                  </li>
                  <li class="has-submenu">
                     <a href="#">Vacaciones</a>
                     <ul class="submenu">
                        <li><a href="../Vacaciones/solicitarVacacionColaborador.aspx">Solicitar Vacaciones</a></li>
                        <li><a href="../Vacaciones/calendarioVacaciones.aspx">Calendario de Vacaciones</a></li>
                     </ul>
                  </li>
                  <li class="has-submenu">
                     <a href="#">Actividades</a>
                     <ul class="submenu">
                        <li><a href="../Control de Actividades/ControlActividadesColaborador.aspx">Registrar Actividades</a></li>
                     </ul>
                  </li>
                  <li class="has-submenu">
                     <a href="#">Marcas</a>
                     <ul class="submenu">
                        <li><a href="../Marcas/marcas.aspx">Registre su hora de marca</a></li>
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
         <div class="container-menu">
            <div class="header-menu">
               <h1>Revisa nuestras opciones de gestión para el usuario</h1>
               <p>ActivitySync ofrecemos distintas opciones para la gestión de nuestros empleados con la intención de mantener un sistema autogestionable.</p>
            </div>
            <div class="options-menu">
               <div class="option-menu">
                  <a href="#"><img src="../../Imagenes/icono-marcas.png" alt="Control de marcas" /></a>
                  <h3>Control de marcas</h3>
                  <p>Registre su hora de entrada y de salida de cada día laboral respectivo bajo su horario.</p>
               </div>
               <div class="option-menu">
                  <a href="#"><img src="../../Imagenes/icono-actividades.png" alt="Control de actividades" /></a>
                  <h3>Control de actividades</h3>
                  <p>Registre las actividades que ha hecho alrededor de ese día y semana, para que sea guardado como evidencia de su productividad.</p>
               </div>
               <div class="option-menu">
                  <a href="#"><img src="../../Imagenes/icono-vacaciones.png" alt="Vacaciones" /></a>
                  <h3>Vacaciones</h3>
                  <p>Puede solicitar vacaciones, revisar los días libres disponibles de vacaciones y las siguientes vacaciones de la empresa planeadas para usted.</p>
               </div>
               <div class="option-menu">
                  <a href="#"><img src="../../Imagenes/icono-horasExtras.png" alt="Horas Extras" /></a>
                  <h3>Horas Extras</h3>
                  <p>Puede administrar si tiene horas extras pendientes que realizar en su jornada laboral.</p>
               </div>
               <div class="option-menu">
                  <a href="#"><img src="../../Imagenes/icono-reposiciones.png" alt="Reposiciones" /></a>
                  <h3>Reposiciones</h3>
                  <p>Si por alguna razón ha tenido un tipo de inconsistencia por un motivo externo, puede reponer sus horas laborales.</p>
               </div>
               <div class="option-menu">
                  <a href="#"><img src="../../Imagenes/icono-inconsistencias.png" alt="Inconsistencias" /></a>
                  <h3>Inconsistencias</h3>
                  <p>Si alguna razón externa ha presentado una inconsistencia a su horario laboral, puede justificarla aquí.</p>
               </div>
               <div class="option-menu">
                  <a href="#"><img src="../../Imagenes/icono-permisos.png" alt="Permisos" /></a>
                  <h3>Permisos</h3>
                  <p>Si necesita solicitar un permiso ante una situación, lo puede hacer en este apartado.</p>
               </div>
            </div>
         </div>
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
                  <li><a href="#">Horas Extras</a></li>
                  <li><a href="#">Permisos</a></li>
                  <li><a href="#">Inconsistencias</a></li>
                  <li><a href="#">Reposiciones</a></li>
                  <li><a href="#">Vacaciones</a></li>
                  <li><a href="#">Actividades</a></li>
                  <li><a href="#">Marcas</a></li>
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
      </form>
   </body>
</html>