﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RespuestaHorasExtraColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.Horas_Extra.RespuestaHorasExtra" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Horas Extra</title>
     <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet"/>
    <link href="../../Estilos/app.css" rel="stylesheet"/>
    <style>
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
            <h1>Colaborador</h1>
            <p><asp:Label ID="lblNombre" runat="server" Text="Label"></asp:Label></p>
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
                <img src="../../Imagenes/notificacion.gif" alt="Notificación"/>
            </button>
        </div>
    </header>

      <main>
          <section class="seccion-imagen">
                <img src="../../Imagenes/PulgaresArriba.png" alt="Ilustración de una persona trabajando horas extra"/>
            </section>

          <section class="seccion-formulario">
              <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                  <h2><span class="fuente-delgada">Solicitud </span><span class="fuente-gruesa">de</span></h2>
            <br />
            <p>Horas Extra</p>
            <br />
            <!-- Label para mostrar el idSolicitud -->
                 <asp:Label ID="lblSolicitudId" runat="server" Text="ID de Solicitud: " Font-Bold="True" />
                   <label>Horas Solicitadas:</label>
            <asp:Label ID="lblHorasSolicitadas" runat="server" />
            <div class="Division-elementos">
                <div>
                    <label>Incio:</label>
                    <asp:Label ID="lblHoraInicialExtra" runat="server" />
                </div>
                <div>
                    <label>Fin:</label>
                    <asp:Label ID="lblHoraFinalExtra" runat="server" />
                </div>
            </div>
            <asp:Label ID="lblMotivo" runat="server" />
            <br />
            <asp:Button ID="AceptarButton" CssClass="button" runat="server" Text="Aceptar" OnClick="AceptarButton_Click" />
            <br />
            <asp:Button ID="DenegarButton" runat="server" CssClass="button-blanco" Text="Denegar" OnClick="DenegarButton_Click" />

              </div>
          </section>
      </main>

      <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
                <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
        </div>

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
