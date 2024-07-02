<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="solicitudHorasExtras.aspx.cs" Inherits="ControlEmpresarial.Vistas.solicitudHorasExtras" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Solicitud de Horas Extra</title>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet">
    <link href="../Estilos/app.css" rel="stylesheet">
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
                    <li><a href="#">Colaborador</a></li>
                    <li><a href="#">Asignar Actividades</a></li>
                    <li><a href="#">Reposición</a></li>
                    <li class="dropdown">
                        <a class="activo" href="#">Horas Extra</a>
                    </li>
                    <li><a href="#">Solicitudes</a></li>
                    <li><a href="#">Marcas</a></li>
                </ul>
            </nav>
            <div class="cabecera-derecha">
                <button class="boton-notificacion">
                    <img src="icons8-bell.gif" alt="Notificación">
                </button>
            </div>
        </header>
        <main>
            <section class="seccion-formulario">
                <div class="tarjeta-formulario">
                    <h2><span class="fuente-delgada">Solicitud de</span><br><span class="fuente-gruesa">Horas Extra</span></h2>
                    <p>Haz tu solicitud para horas extra al colaborador.</p>
                    <label class="fuente-morada" for="colaboradorACargo">Colaborador a cargo</label>
                    <asp:DropDownList ID="colaborador" runat="server">
                        <asp:ListItem Text="Seleccione" Value="" />
                    </asp:DropDownList>
                    <br />

                    <label class="fuente-morada" for="dia">Día</label>
                    <asp:TextBox ID="dia" runat="server" placeholder="Ej. 30/06/2024"></asp:TextBox>
                    <br />

                    <div class="entradas-horario">
                        <div>
                            <label class="fuente-morada" for="hora-inicio">Hora inicio</label>
                            <asp:TextBox ID="horaInicio" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label class="fuente-morada" for="hora-final">Hora final</label>
                            <asp:TextBox ID="horaFinal" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <br />

                    <label class="fuente-morada" for="motivo">Motivo</label>
                    <asp:TextBox ID="motivo" TextMode="MultiLine" runat="server" placeholder="Ingresa el motivo de la solicitud de horas extra."></asp:TextBox>
                    <br />
                    <asp:Button ID="submit" runat="server" Text="Enviar" OnClick="submit_Click"/>
                    <asp:Label ID="lblMensaje" runat="server" Visible="false"></asp:Label>

                </div>
            </section>
            <section class="seccion-imagen">
                <img src="../Imagenes/jefe.webp" alt="Ilustración de una persona trabajando horas extra">
            </section>
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
</body>
</html>
