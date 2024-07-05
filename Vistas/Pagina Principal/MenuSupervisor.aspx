<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuSupervisor.aspx.cs" Inherits="ControlEmpresarial.Vistas.Pagina_Principal.MenuSupervisor" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ActivitySync</title>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet"/>
    <link href="../../Estilos/app.css" type="text/css" rel="stylesheet"/>
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
            <li><a class="activo" href="#">Supervisor</a></li>
            <li><a href="#">Rebajos</a></li>
            <li><a href="#">Vacaciones Colectivas</a></li>
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
                <h1>Revisa nuestras opciones de gestión para la jefatura</h1>
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
            </div>
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
