<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="ControlEmpresarial.Vistas.Pagina_Principal.inicio" %>
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
         .container-inicio {
         width: 60%;
         margin: 0 auto;
         padding: 40px;
         z-index: 2;
         display: flex;
         flex-direction: column;
         align-items: center;
         }
         .card-container {
         display: flex;
         flex-wrap: wrap;
         justify-content: center; /* Para centrar las cards */
         margin-top: 40px;
         }
         .card {
         margin: 10px; /* Eliminar margen entre las cards */
         max-width: 300px;
         text-align: center;
         transition: transform 0.2s ease-in-out, opacity 0.2s ease-in-out;
         padding: 20px;
         overflow: hidden;
         }
         .card:hover {
         transform: translateY(-10px);
         opacity: 0.8;
         }
         .card img {
         border-radius: 10%;
         width: 100px;
         height: 100px;
         transition: transform 0.3s ease-in-out;
         }
         .card:hover img {
         transform: scale(1.1); /* Agranda la imagen al hacer hover */
         }

         .testimonios {
            padding: 40px;
            border-radius: 10px;
            margin-bottom: 50px;
            text-align: center;
         }

        .testimonios h2 {
            color: #5E58F8;
            font-size: 2em;
            margin-bottom: 20px;
        }

        .testimonio-card {
            margin: 20px auto;
            max-width: 600px;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }

        .testimonio-card p {
            font-size: 1.2em;
            color: #555;
        }

        .testimonio-card h4 {
            margin: 10px 0 0;
            font-weight: bold;
            color: #7033ff;
        }
        .loader {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(255, 255, 255, 0.8);
            display: flex;
            justify-content: center;
            align-items: center;
            z-index: 9999;
        }

        .spinner {
            border: 8px solid #f3f3f3; 
            border-radius: 50%;
            border-top: 8px solid #5E58F8; 
            width: 50px;
            height: 50px;
            animation: spin 1s linear infinite;
        }

        .divisor {
            width: 100%;
            height: 5px;
            background-color: #5E58F8;
            margin: 40px 0;
        }

        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
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
                  <li><a class="activo" href="Inicio.aspx">Inicio</a></li>
                  <li><a href="Contactenos.aspx">Contactenos</a></li>
                  <li><a href="Login.aspx">Iniciar Sesion</a></li>
               </ul>
            </nav>
            <div class="cabecera-derecha">
               <button class="boton-logo">
               <img src="../../Imagenes/logo.png" alt="Logo">
               </button>
            </div>
         </header>
         <main>
            <div class="container-inicio">
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
                <br />
                <br />
                <br />
               <div class="divisor"></div>
                <br />
                <br />
                <br />
               <div class="card-container">
                  <div class="card">
                     <img src="../../Imagenes/icono-colaborador.png" alt="Empleados">
                     <h3>Registro de Empleados</h3>
                     <p>Gestión completa de los expedientes de los empleados, incluyendo su historial y detalles laborales.</p>
                  </div>
                  <div class="card">
                     <img src="../../Imagenes/icono-vacaciones.png" alt="Vacaciones">
                     <h3>Gestión de Vacaciones</h3>
                     <p>Control de días festivos, vacaciones colectivas y solicitud de vacaciones.</p>
                  </div>
                  <div class="card">
                     <img src="../../Imagenes/icono-marcas.png" alt="Marcas">
                     <h3>Control de Marcas</h3>
                     <p>Registro y control de marcas de entrada y salida, con reportes detallados.</p>
                  </div>
                  <div class="card">
                     <img src="../../Imagenes/icono-inconsistencias.png" alt="Inconsistencias">
                     <h3>Inconsistencias</h3>
                     <p>Registro y justificación de inconsistencias, con un sistema de aprobación y control.</p>
                  </div>
                  <div class="card">
                     <img src="../../Imagenes/icono-actividades.png" alt="Actividades">
                     <h3>Control de Actividades</h3>
                     <p>Supervisión y monitoreo de la productividad y rendimiento de las actividades diarias.</p>
                  </div>
               </div>
                <br />
                <br />
                <br />
                <div class="divisor"></div>
                <br />
                <br />
                <br />
                <div class="testimonios">
                    <h2>Testimonios</h2>
                    <div class="testimonio-card">
                        <p>"ActivitySync ha transformado la forma en que gestionamos nuestras actividades diarias. La interfaz es intuitiva y fácil de usar."</p>
                        <h4>Juan Pérez</h4>
                        <p>Gerente de Recursos Humanos</p>
                    </div>
                    <div class="testimonio-card">
                        <p>"La capacidad de controlar todas nuestras marcas y permisos en un solo lugar ha mejorado significativamente nuestra eficiencia."</p>
                        <h4>Ana Gómez</h4>
                        <p>Coordinadora de Operaciones</p>
                    </div>
                </div>
                <br />
                <br />
                <br />
            </div>
         </main>
         <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
               <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
         </div>
          <!-- Animación de Carga -->
        <div id="loader" class="loader">
            <div class="spinner"></div>
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
    <script>
        document.addEventListener("DOMContentLoaded", function() {
            document.getElementById("loader").style.display = "none";
        });
    </script>
   </body>
</html>