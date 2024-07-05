<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuJefatura.aspx.cs" Inherits="ControlEmpresarial.Vistas.Pagina_Principal.MenuJefatura" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ActivitySync</title>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet"/>
    <link href="~/Estilos/app.css" type="text/css" rel="stylesheet"/>
    <style>
        .container-menu {
            background-color: #5E58F8;
            color: white;
            text-align: center;
            padding: 50px;
        }
        .header-menu {
            margin-bottom: 50px;
        }
        .options-menu {
            display: flex;
            flex-wrap: wrap;
            justify-content: space-around;
        }
        .option-menu {
            width: 30%;
            margin: 10px 0;
        }
        .option img {
            width: 50px;
            height: 50px;
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
            <li><a class="activo" href="#">Jefatura</a></li>
            <li><a href="#">Permisos</a></li>
            <li><a href="#">Incapacidades</a></li>
            <li><a href="#">Inconsistencias</a></li>
            <li><a href="#">Asignar Actividades</a></li>
            <li><a href="#">Reposición</a></li>
            <li><a href="#">Horas Extras</a></li>
            <li><a href="#">Vacaciones</a></li>
            <li><a href="#">Marcas</a></li>
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
                    <a href="#"><img src="../../Imagenes/icono-permisos.png" alt="Permisos" /></a>
                    <h3>Permisos</h3>
                    <p>Revise los permisos que ha solicitado los empleados.</p>
                </div>
                <div class="option-menu">
                    <a href="#"><img src="../../Imagenes/icono-escritura.png" alt="Control de actividades" /></a>
                    <h3>Incapacidades</h3>
                    <p>Registre las incapacidades del empleado.</p>
                </div>
                <div class="option-menu">
                    <a href="#"><img src="../../Imagenes/icono-inconsistencias.png" alt="Vacaciones" /></a>
                    <h3>Inconsistencias</h3>
                    <p>Puede revisar las inconsistencias de vacaciones de cada empleado.</p>
                </div>
                <div class="option-menu">
                    <a href="#"><img src="../../Imagenes/icono-tiempo.png" alt="Horas Extras" /></a>
                    <h3>Asignar Actividades</h3>
                    <p>Puede asignar actividades para su departamento.</p>
                </div>
                <div class="option-menu">
                    <a href="#"><img src="../../Imagenes/icono-reposiciones.png" alt="Reposiciones" /></a>
                    <h3>Reposiciones</h3>
                    <p>Puede verificar las reposiciones hechas por el empleado.</p>
                </div>
                <div class="option-menu">
                    <a href="#"><img src="../../Imagenes/icono-horasExtras.png" alt="Inconsistencias" /></a>
                    <h3>Horas Extras</h3>
                    <p>Puede asignar horas extras para un cierto colaborador.</p>
                </div>
                <div class="option-menu">
                    <a href="#"><img src="../../Imagenes/icono-vacaciones.png" alt="Inconsistencias" /></a>
                    <h3>Vacaciones</h3>
                    <p>Puede validar las vacaciones solicitadas por un colaborador.</p>
                </div>
                <div class="option-menu">
                    <a href="#"><img src="../../Imagenes/icono-marcas.png" alt="Inconsistencias" /></a>
                    <h3>Marcas</h3>
                    <p>Registre su hora de entrada y de salida de cada día laboral respectivo bajo su horario.</p>
                </div>
                
            </div>
        </div>
        <footer class="footer">
            <div>
                <h3>About</h3>
                <p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when 
                    an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into 
                    electronic typesetting, remaining essentially unchanged.</p>
            </div>
            <div>
                <h3>Tags</h3>
                <p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when 
                    an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into 
                    electronic typesetting, remaining essentially unchanged.</p>
            </div>
            <div>
                <h3>Recent Comments</h3>
                <p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when 
                    an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into 
                    electronic typesetting, remaining essentially unchanged.</p>
            </div>
        </footer>
    </form>
</body>
</html>
