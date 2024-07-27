<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistroActividadesJefe.aspx.cs" Inherits="ControlEmpresarial.Vistas.Control_de_Actividades.RegistroActividadesJefe" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Actividades</title>
      <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet"/>
    <link href="../../Estilos/app.css" type="text/css" rel="stylesheet"/>
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
            z-index: 999;
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
            color: #5E58F8; /* color texto */
        }
        /* Sidebar styles */
.sidebar {
    height: 100%;
    width: 0;
    position: fixed;
    z-index: 9999;
    top: 0;
    right: 0;
    background-color: #333; /* Color de fondo más oscuro para el sidebar */
    color: #fff; /* Color del texto */
    overflow-x: hidden;
    transition: 0.5s;
    padding-top: 60px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* Sombra del box */
}

.sidebar a {
    padding: 8px 8px 8px 32px;
    text-decoration: none;
    font-size: 25px;
    color: #fff; /* Color de los enlaces */
    display: block;
    transition: 0.3s;
}

        .sidebar a:hover {
            color: #FF3EA5;
        }
.sidebar .closebtn {
    position: absolute;
    top: 0;
    right: 25px;
    font-size: 36px;
    color: #fff; /* Color del botón de cerrar */
}

.sidebar-content {
    padding: 15px;
    color: #fff; /* Color del texto dentro del contenido del sidebar */
}

.notification-card {
    background-color: #444; /* Color de fondo de las tarjetas de notificación */
    padding: 15px;
    margin: 15px 0;
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); /* Sombra suave para las tarjetas */
    transition: transform 0.3s;
}

.notification-card:hover {
    transform: scale(1.02); /* Efecto de hover para agrandar ligeramente las tarjetas */
}

.notification-divider {
    height: 2px;
    background-color: #FF3EA5; /* Color morado llamativo para la línea divisora */
    margin: 10px 0;
}

.notification-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.notification-title {
    margin: 0;
    font-size: 20px;
    color: #fff;
}

.notification-date {
    font-size: 14px;
    color: #ccc;
}

.notification-motivo {
    margin: 10px 0;
    font-size: 16px;
    color: #ddd;
}

.notification-enviador {
    font-size: 14px;
    color: #bbb;
}
.tipo-actividad-container {
        display: flex;
        align-items: center;
        gap: 10px; /* Espacio entre el TextBox y el Button */
    }

    .tipo-actividad-container label {
        margin-right: 10px; /* Espacio entre la etiqueta y el TextBox */
    }

    .button1 {
        background-color: #5E58F8;
        color: white;
        border: none;
        padding: 10px 20px;
        cursor: pointer;
        font-size: 16px;
        border-radius: 5px;
        transition: background-color 0.3s ease;
    }

    .button1:hover {
        background-color: #4B47C6;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
         <header>
            <div class="cabecera-izquierda">
                <h1>Jefatura</h1>
                <p><asp:Label ID="lblNombre" runat="server" Text="Label"></asp:Label></p>
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
                            <li><a href="../Control De Actividades/RegistroActividadesJefe.aspx">Registro de Actividades</a></li>
                            <li><a href="../Control De Actividades/TablaPreAceptacionJefatura.aspx">Revisar Actividades de colaboradores</a></li>
                            <li><a href="../Control De Actividades/HistoricoActividadesJefatura.aspx">Historial de Actividades</a></li>
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
                    <li class="has-submenu">
                        <a href="#">Administración</a>
                        <ul class="submenu">
                            <li><a href="../Vacaciones/solicitudVacacionesJefatura.aspx">Agrega un colaborador</a></li>
                        </ul>
                    </li>
                </ul>
            </nav>
            <div class="cabecera-derecha">
               <button type="button" id="notificacionesLink" class="boton-notificacion">
               <img src="../../Imagenes/notificacion.gif" alt="Notificación"/>
               </button>
            </div>

             <div id="mySidebar" class="sidebar">
                <a href="javascript:void(0)" class="closebtn" id="closeBtn">&times;</a>
                <div class="sidebar-content">
                    <h2>Notificaciones</h2>
                   <asp:Repeater ID="repeaterNotificaciones" runat="server">
                        <ItemTemplate>
                            <div class="notification-card">
                                <div class="notification-header">
                                    <h3 class="notification-title"><%# Eval("Titulo") %></h3>
                                    <span class="notification-date"><%# Eval("Fecha", "{0:dd/MM/yyyy}") %></span>
                                </div>
                                <p class="notification-motivo"><%# Eval("Motivo") %></p>
                                <span class="notification-enviador">Enviado por: <%# Eval("EnviadorNombre") %> <%# Eval("EnviadorApellidos") %></span>
                            </div>
                            <div class="notification-divider"></div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </header>
       <main>
            <section class="seccion-formulario">
                <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                    <div class="tipo-actividad-container">
                        <label class="fuente-morada">Tipo de actividad</label>
                        <asp:TextBox ID="tipoActividad" runat="server" placeholder="Ingrese el tipo de actividad"></asp:TextBox>
                       <asp:Button ID="agregarTipoActividad" runat="server" Text="Agregar" CssClass="button1" OnClick="AgregarTipoActividad_Click" />
                    </div>
                    <br />
                    <br />
                    <h2><span class="fuente-delgada">Control de</span><br /><span class="fuente-gruesa">Actividades</span></h2>
                    <p>Ingrese las actividades. </p>
                    <br />
                    <br />
                    <label class="fuente-morada">Título</label>
                    <asp:TextBox ID="titulo" runat="server" placeholder="Ingrese la actividad realizada."></asp:TextBox>
                    <br />
                    <div class="tipo-actividad-container">
                        <label class="fuente-morada">Seleccionar Tipo de actividad</label>
                        <asp:DropDownList ID="dropdownTipoActividad" runat="server" CssClass="dropdown"></asp:DropDownList>
                    </div>
                    <label class="fuente-morada">Descripción</label>
                    <asp:TextBox ID="actividad" runat="server" TextMode="MultiLine" Rows="4" Columns="40" placeholder="Ingrese la actividad realizada."></asp:TextBox>
                    <asp:Button ID="submit" runat="server" Text="Enviar" CssClass="button" OnClick="Submit_Click" />
                    <asp:Label ID="debugLabel" runat="server" CssClass="debug-label" EnableViewState="false" />
                    <asp:Label ID="Label1" runat="server" CssClass="debug-label" EnableViewState="false" />
                     <asp:Label ID="Label2" runat="server" CssClass="debug-label" EnableViewState="false" />

                </div>
            </section>
           u
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
                    sus actividades diarias, nuestro software está diseñado para optimizar la productividad y mejorar la organización empresarial."
                </p>
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
                <h3>Contáctenos</h3>
                <p>ActivitySync ofrece servicios al cliente cuales estén interesados a nuestra gestión de actividades</p>
                <p>Contáctenos:</p>
                <ul>
                    <li>Email: support@activitysync.com</li>
                    <li>Phone: +1-800-123-4567</li>
                </ul>
            </div>
        </footer>
    </form>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            var sidebar = document.getElementById("mySidebar");
            var openBtn = document.getElementById("notificacionesLink");
            var closeBtn = document.getElementById("closeBtn");

            openBtn.onclick = function () {
                sidebar.style.width = "300px";
            }

            closeBtn.onclick = function () {
                sidebar.style.width = "0";
            }

            window.onclick = function (event) {
                if (event.target == sidebar) {
                    sidebar.style.width = "0";
                }
            }
        });
    </script>
</body>
</html>
