<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="solicitudHorasExtras.aspx.cs" Inherits="ControlEmpresarial.Vistas.solicitudHorasExtras" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Solicitud de Horas Extra</title>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet">
    <style>
        /* Reiniciar algunos estilos predeterminados */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: "Montserrat", Sans-serif;
            background-color: #f7f7f7;
            color: #333;
            position: relative;
            min-height: 100vh;
        }

        header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 20px;
            background-color: #fff;
            border-bottom: 1px solid #ccc;
        }

        .cabecera-izquierda h1 {
            margin: 0;
            color: #3c31dd;
        }

        .cabecera-izquierda p {
            margin: 0;
            color: #666;
        }

        nav ul {
            display: flex;
            list-style: none;
        }

        nav ul li {
            margin-right: 20px;
        }

        nav ul li a {
            text-decoration: none;
            color: #333;
            font-weight: 500;
        }

        nav ul li a:hover {
            color: #7033ff; 
        }

        .cabecera-derecha .boton-notificacion {
            background: none;
            border: none;
            font-size: 24px;
            cursor: pointer;
        }

        main {
            display: flex;
            justify-content: space-between;
            padding: 40px;
            position: relative;
            z-index: 2;
        }

        .seccion-formulario {
            flex: 1;
            position: relative;
        }

        .tarjeta-formulario {
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            z-index: 2;
            position: relative;
        }

        .tarjeta-formulario h2 {
            font-size: 42px;
            color: #000000;
        }

        .fuente-delgada {
            font-weight: 100;
            color: black;
        }

        .fuente-gruesa {
            color: #7033ff;
        }

        .fuente-morada {
            color: #7033ff;
        }

        .activo {
            color: #7033ff;
            font-weight: 700;
        }

        .tarjeta-formulario p {
            margin-bottom: 20px;
            color: #666;
        }

        form {
            display: flex;
            flex-direction: column;
        }

        label {
            margin-bottom: 5px;
            font-weight: bold;
        }

        input, select, textarea {
            margin-bottom: 20px;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
            width: 100%;
        }

        .entradas-horario {
            display: flex;
            justify-content: space-between;
        }

        .entradas-horario div {
            flex: 1;
        }

        .entradas-horario div:first-child {
            margin-right: 10px;
        }

        button {
            padding: 15px;
            border: none;
            border-radius: 4px;
            background-color: #7033ff;
            color: #fff;
            font-size: 16px;
            cursor: pointer;
        }

        button:hover {
            background-color: #3c31dd;
        }

        .seccion-imagen {
            flex: 1;
            display: flex;
            justify-content: center;
            align-items: center;
            position: relative;
            z-index: 2;
        }

        .seccion-imagen img {
            position: relative;
            max-width: 100%;
            height: 580px;
        }

        .divisor-forma-personalizado {
            position: absolute;
            bottom: 0;
            left: 0;
            width: 100%;
            overflow: hidden;
            line-height: 0;
            transform: rotate(180deg);
        }

        .divisor-forma-personalizado svg {
            position: relative;
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
</body>
</html>
