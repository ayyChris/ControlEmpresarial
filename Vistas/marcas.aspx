<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="marcas.aspx.cs" Inherits="ControlEmpresarial.Vistas.marcas" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Marcas</title>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet">
    <link href="../Estilos/app.css" type="text/css" rel="stylesheet">
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

        .container {
            display: flex;
            align-items: center;
            background-color: white;
            padding: 5rem;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }

        .clock-icon {
            width: 400px;
            height: 400px;
        }

        .info {
            font-size: 1.5em;
            padding: 0 0 0 300px; /* top, right, bottom, left */
        }

        .info h2 {
            color: #7033ff;
        }

        .info p {
            margin-top: 10px; /* Añadir espacio entre párrafos */
        }

        .button {
            margin-top: 20px; /* Añadir espacio superior a los botones */
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
                    <li><a href="#">Permisos</a></li>
                    <li><a href="#">Inconsistencias</a></li>
                    <li><a href="#">Vacaciones</a></li>
                    <li><a href="#">Actividades</a></li>
                    <li><a href="#">Solicitudes</a></li>
                    <li><a class="activo" href="#">Marcas</a></li>
                </ul>
            </nav>
            <div class="cabecera-derecha">
                <button class="boton-notificacion">
                    <img src="../Imagenes/notificacion.gif" alt="Notificación">
                </button>
            </div>
        </header>
        <main>
            <div class="container">
                <img src="../Imagenes/reloj.png" alt="Clock Icon" class="clock-icon" />
                <div class="info">
                    <h2 id="clock">Hora</h2>
                    <p id="dayOfWeek" runat="server"><%= DateTime.Now.ToString("dddd") %></p>
                    <p>7:00 am - 17:00 pm</p>
                    <div class="buttons">
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
