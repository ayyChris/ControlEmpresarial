<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AceptarDenegarHorasExtraJefatura.aspx.cs" Inherits="ControlEmpresarial.Vistas.Horas_Extra.AceptarDenegarHorasExtraJefatura" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
   <title>Horas Extra</title>
     <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet"/>
    <link href="../../Estilos/app.css" rel="stylesheet"/>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css"/>

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
        <h1>Jefatura</h1>
        <p>Esteban Mata</p>
    </div>
     <nav>
    <ul>
        <li class="has-submenu">
            <a href="#">Permisos</a>
            <ul class="submenu">
                <li><a href="../Permisos/TablaPreVisualizacionPermisosJefe.aspx">Revisar Permisos</a></li>
                <li><a href="../Permisos/VisualizacionPermisos.aspx">Ver Permisos</a></li>
            </ul>
        </li>
        <li class="has-submenu">
            <a href="#">Incapacidades</a>
            <ul class="submenu">
                <li><a href="../Incapacidades/SolicitarIncapacidadesJefatura.aspx">Registrar incapacidades</a></li>
            </ul>
        </li>
        <li class="has-submenu">
            <a href="#">Inconsistencias</a>
            <ul class="submenu">
                <li><a href="../Inconsistencias/SolicitarIncapacidadesJefatura.aspx">Revisar Inconsistencias.</a></li>
            </ul>
        </li>
        <li class="has-submenu">
            <a href="#">Actividades</a>
            <ul class="submenu">
                <li><a href="../Control De Actividades/TablaPreAceptacionJefatura.aspx">Revisar Actividades de colaboradores</a></li>
                <li><a href="../Control De Actividades/TablaPreAceptacionJefatura.aspx">Historial de Actividades</a></li>
            </ul>
        </li>
        <li class="has-submenu">
            <a href="#">Reposiciones</a>
            <ul class="submenu">
                <li><a href="#">Verificar Reposiciones</a></li>
            </ul>
        </li>
        <li class="has-submenu">
            <a href="#">Horas Extras</a>
            <ul class="submenu">
                <li><a href="../Horas Extra/solicitudHorasExtrasJefatura.aspx">Asignar Horas Extras</a></li>
                <li><a href="../Horas Extra/AceptarDenegarHorasExtraJefatura.aspx">Aceptar Horas Extras</a></li>
                <li><a href="../Horas Extra/HistoricoHorasExtrasJefatura.aspx">Historial Horas Extras</a></li>
            </ul>
        </li>
        <li class="has-submenu">
            <a href="#">Vacaciones</a>
            <ul class="submenu">
                <li><a href="../Vacaciones/solicitudVacacionesJefatura.aspx">Aceptar Vacaciones de Colaboradores</a></li>
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
        <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra"/>
    </section>
    <section class="seccion-formulario">
         
        <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
            <h2><spam class="fuente-delgada">Empleado</spam></h2>
            <asp:DropDownList ID="colaborador" runat="server" AutoPostBack="True" OnSelectedIndexChanged="colaborador_SelectedIndexChanged"></asp:DropDownList>
            <h2><span class="fuente-delgada">Evidencia de</span><br/><span class="fuente-gruesa">Horas Extra</span></h2>
            <br />
            <label>Fecha de respuesta:</label>
            <asp:Label ID="lblFechaRespuesta" runat="server" CssClass="dinamic-label"></asp:Label>
            <br />
            <asp:Label ID="lblEvidencia" runat="server" CssClass="dinamic-label"></asp:Label>
            <br />
            <asp:Button ID="AceptarButton" CssClass="button" runat="server" Text="Aceptar" OnClick="AceptarButton_Click" />
            <br />
            <asp:Button ID="DenegarButton" runat="server" CssClass="button-blanco" Text="Denegar" OnClick="DenegarButton_Click"/>
            <asp:Label ID="lblDebugInfo" runat="server" CssClass="mensaje-info" Visible="false" />
            <asp:Label ID="lblValidacion" runat="server" CssClass="mensaje-info" Visible="false" />
            <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-info" Visible="false" />
            <asp:Label ID="Label1" runat="server" CssClass="mensaje-info" />
            <asp:Label ID="Label2" runat="server" CssClass="mensaje-info" />

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
    </form>
</body>
</html>
