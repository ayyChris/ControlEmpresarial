﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuSupervisor.aspx.cs" Inherits="ControlEmpresarial.Vistas.MenuSupervisor" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
   <head runat="server">
      <title>ActivitySync</title>
      <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet"/>
      <link href="../../Estilos/app.css" type="text/css" rel="stylesheet"/>
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

         /*css*/
         .options-menu {
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between; 
        }
        
        .option-menu {
            width: calc(33.33% - 20px); 
            margin-bottom: 20px; 
            text-align: center; 
        }
        
        .option-menu.full-width {
            width: 100%; 
        }
      </style>
   </head>
   <body>
      <form id="form1" runat="server">
         <header>
            <div class="cabecera-izquierda">
               <h1>Supervisor</h1>
               <p><asp:Label ID="lblNombre" runat="server" Text="Label"></asp:Label></p>
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
                         <li><a href="../Colaborador/AgregarDepartamento.aspx">Añadir Departamento</a></li>
                         <li><a href="../Colaborador/AgregarPuestoTrabajo.aspx">Añadir Puesto de trabajo</a></li>
                         <li><a href="../Colaborador/AgregarHorario.aspx">Añadir Horario</a></li>
                        <li><a href="../Colaborador/VisualizacionColaboradorSupervisor.aspx">Ver Colaboradores</a></li>
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
         <div class="container-menu">
            <div class="header-menu">
               <h1>Revisa nuestras opciones de gestión para los supervisores</h1>
               <p>ActivitySync ofrecemos distintas opciones para la gestión de nuestros empleados con la intención de mantener un sistema autogestionable.</p>
            </div>
            <div class="options-menu">
            <div class="option-menu">
                <a href="#"><img src="../../Imagenes/icono-escritura.png" alt="Permisos" /></a>
                <h3>Rebajos</h3>
                <p>Revise el historial de rebajos que se han registrado.</p>
            </div>
            <div class="option-menu">
                <a href="#"><img src="../../Imagenes/icono-vacaciones.png" alt="Control de actividades" /></a>
                <h3>Vacaciones Colectivas</h3>
                <p>Registre las vacaciones colectivas para los colaboradores y la jefatura.</p>
            </div>
            <div class="option-menu full-width">
                <a href="../Colaborador/VisualizacionColaboradorSupervisor.aspx"><img src="../../Imagenes/icono-colaborador.png" alt="Colaboradores" /></a>
                <h3>Colaboradores</h3>
                <p>Vea los colaboradores de los departamentos.</p>
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
      </form>
   </body>
</html>