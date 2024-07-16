<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="inicio.aspx.cs" Inherits="ControlEmpresarial.Vistas.Pagina_Principal.inicio" %>
<!DOCTYPE html>
<html lang="es">
   <head runat="server">
      <meta charset="UTF-8">
      <meta name="viewport" content="width=device-width, initial-scale=1.0">
      <title>Inicio</title>
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
         position: relative; /* Cambiar a relative para mantener su posición, no mover*/
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
         .container {
         display: flex;
         flex-direction: column;
         align-items: center;
         margin-top: 10px;
         }
         .welcome-section {
         text-align: center;
         margin-bottom: 20px;
         }
         .welcome-section h1 {
         color: #000;
         font-size: 2.5em;
         margin: 0;
         }
         .welcome-section h1 span {
         color: #7033ff;
         }
         .welcome-section p {
         color: #000;
         font-size: 1.2em;
         margin: 10px 0 0 0;
         }
         .content-section {
         background: #7033ff;
         padding: 20px;
         border-radius: 10px;
         color: white;
         width: 80%;
         display: flex;
         justify-content: space-around;
         align-items: center;
         margin-bottom: 50px;
         }
         .content-section .left,
         .content-section .right {
         flex: 1;
         text-align: center;
         }
         .content-section .left {
         border-right: 1px solid #fff;
         padding-right: 20px;
         }
         .content-section .right {
         padding-left: 20px;
         }
         @media (max-width: 768px) {
         .content-section {
         flex-direction: column;
         }
         .content-section .left {
         border-right: none;
         border-bottom: 1px solid #fff;
         padding-right: 0;
         padding-bottom: 20px;
         margin-bottom: 20px;
         }
         .content-section .right {
         padding-left: 0;
         }
         }
      </style>
   </head>
   <body>
      <form id="form1" runat="server">
         <header>
            <div class="cabecera-izquierda">
               <h1>Activity Sync</h1>
               <p>Bienvenido!</p>
            </div>
            <nav>
               <ul>
                  <li><a class="activo" href="#">Inicio</a></li>
                  <li><a href="#">Contactenos</a></li>
                  <li><a href="#">Iniciar Sesion</a></li>
               </ul>
            </nav>
            <div class="cabecera-derecha">
               <button class="boton-logo">
               <img src="../../Imagenes/logo.png" alt="Logo">
               </button>
            </div>
         </header>
         <main>
            <div class="container">
               <div class="welcome-section">
                  <img src="../../Imagenes/colaborador.png" alt="Welcome Image" style="border-radius: 10%; width: 200px; height: 300px;">
                  <h1 class="fuente-delgada">Bienvenido al sistema de gestión de </h1>
                  <h1 style="color:#7033ff;">Activity Sync</h1>
               </div>
               <div class="content-section">
                  <div class="left">
                     <p>Tu control de tus labores como empleado de Imperion es nuestra prioridad</p>
                  </div>
                  <div class="right">
                     <p>"ActivitySync proporciona una solución integral para la gestión eficiente de actividades dentro de tu empresa. Desde la 
                        marcación de inconsistencias hasta el registro de entradas y salidas de empleados, así como el seguimiento detallado de 
                        sus actividades diarias, nuestro software está diseñado para optimizar la productividad y mejorar la organización empresarial."
                     </p>
                  </div>
               </div>
            </div>
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