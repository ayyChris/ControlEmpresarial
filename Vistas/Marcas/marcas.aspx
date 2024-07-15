<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="marcas.aspx.cs" Inherits="ControlEmpresarial.Vistas.marcas" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Marcas</title>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet">
    <link href="~/Estilos/app.css" type="text/css" rel="stylesheet">
    <style>
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
                display:block;
            }
    </style>
    <script type="text/javascript">
        function updateClock() {
            var now = new Date();
            var hours = now.getHours();
            var minutes = now.getMinutes();
            var seconds = now.getSeconds();
            var ampm = hours >= 12 ? 'pm' : 'am';
            hours = hours % 12;
            hours = hours ? hours : 12; // the hour '0' should be '12'
            minutes = minutes < 10 ? '0' + minutes : minutes;
            seconds = seconds < 10 ? '0' + seconds : seconds;
            var strTime = hours + ':' + minutes + ':' + seconds + ' ' + ampm;
            document.getElementById('clock').innerHTML = strTime;
            setTimeout(updateClock, 1000);
        }
    </script>
</head>
<body onload="updateClock()">
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
                <li><a href="#">Solicitar Horas Extras</a></li>
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
                    <img src="../../Imagenes/notificacion.gif" alt="Notificación">
                </button>
            </div>
        </header>
        <main>
            <div class="marcas-contenedor">
                <img src="../../Imagenes/reloj.png" alt="Clock Icon" class="clock-icon" />
                <div class="info">
                    <h2 id="clock">Hora</h2>
                    <p id="dayOfWeek" runat="server"><%= DateTime.Now.ToString("dddd") %></p>
                    <p>7:00 am - 17:00 pm</p>
                    <div class="buttons">
                        <br />
                        <asp:Button ID="btnEntrada" runat="server" Text="Entrada" class="button"  />
                        <br />
                        <asp:Button ID="btnSalida" runat="server" Text="Salida" class="button-blanco"/>
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
            sus actividades diarias, nuestro software está diseñado para optimizar la productividad y mejorar la organización empresarial."</p>
    </div>
    <div>
        <h3>Tags</h3>
        <ul class="tag-list">
            <li><a href="#">Permisos</a></li>
            <li><a href="#">Incapacidades</a></li>
            <li><a href="#">Inconsistencias</a></li>
            <li><a href="#">Asignar Actividades</a></li>
            <li><a href="#">Reposiciones</a></li>
            <li><a href="#">Horas Extras</a></li>
            <li><a href="#">Vacaciones</a></li>
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
</body>
</html>
