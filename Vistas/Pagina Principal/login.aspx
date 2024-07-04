<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ControlEmpresarial.Vistas.login" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Solicitar Vacaciones</title>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet">
    <link href="~/Estilos/app.css" type="text/css" rel="stylesheet">
    <style>
        .inicioSesion {
            border: none;
            border-bottom: 3px solid; 
            padding: 10px;
            width: 100%;
            box-sizing: border-box;
            outline: none; 
        }

        .inicioSesion:focus {
            border-bottom-color: #5E58F8; 
        }
        .boton-logo {
            background: none;
            border: none;
            cursor: pointer;
            display: flex;
            justify-content: center; 
            align-items: center; 
        }

        .boton-logo img {
            width: 130px; 
            height: 70px; 
        }
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <div class="cabecera-izquierda">
                <h1>Activity Sync</h1>
                <p>Por favor ingrese sus credenciales.</p>
            </div>
            <nav>
                <ul>
                    <li><a href="#">Inicio</a></li>
                    <li><a href="#">Contactenos</a></li>
                    <li><a class="activo" href="#">Iniciar Sesion</a></li>
                </ul>
            </nav>
            <div class="cabecera-derecha">
                <button class="boton-logo">
                    <img src="../../Imagenes/logo.png" alt="Notificación">
                </button>
            </div>
        </header>
        <main>
            <section class="seccion-formulario">
                <div class="tarjeta-formulario-login" style="max-width: 700px; margin: 0 auto;">
                    <h2><span class="fuente-gruesa">Inicia sesion</span><br><span class="fuente-delgada">en tu cuenta</span></h2>
                    <p></p>
                    <br />
                    <div>
                        <label class="fuente-delgada" for="cedula">Cedula</label>
                        <asp:TextBox CssClass="inicioSesion" ID="cedula" runat="server" placeholder="Ej. 305520445"></asp:TextBox>
                    </div>
                    <br />
                    <div>
                        <label class="fuente-delgada" for="contrasena">Contraseña</label>
                        <asp:TextBox CssClass="inicioSesion" ID="contrasena" runat="server"></asp:TextBox>
                    </div>
                    <br />
                    <label class="fuente-morada" for="contrasena">Olvidé mi contraseña</label>
                    <br />
                    <br />
                    <asp:Button ID="submit" runat="server" Text="Ingresar" CssClass="button" />
                    <br />
                    <asp:Button ID="crearCuenta" runat="server" Text="Crear Cuenta" CssClass="button-blanco" />
                    <asp:Label ID="lblMensaje" runat="server" Visible="false"></asp:Label>

                </div>
            </section>
            <section class="seccion-imagen">
                <img src="../../Imagenes/planta.png" alt="Ilustración de una persona trabajando horas extra">
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

