<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.Pagina_Principal.MenuColaborador" %>

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
                <h1>Colaborador</h1>
                <p>Christian Barquero</p>
            </div>
            <nav>
                <ul>
                    <li><a href="#">Horas Extras</a></li>
                    <li><a href="#">Incapacidades</a></li>
                    <li><a href="#">Permisos</a></li>
                    <li><a href="#">Inconsistencias</a></li>
                    <li><a href="#">Vacaciones</a></li>
                    <li><a href="#">Actividades</a></li>
                    <li><a href="#">Solicitudes</a></li>
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
