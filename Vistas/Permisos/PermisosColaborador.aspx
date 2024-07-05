﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PermisosColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.Permisos.PermisosColaborador" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Permisos</title>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet"/>
    <link href="~/Estilos/app.css" type="text/css" rel="stylesheet"/>
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
                    <li><a class="activo" href="#">Marcas</a></li>
                </ul>
            </nav>
            <div class="cabecera-derecha">
                <button class="boton-notificacion">
                    <img src="~/Imagenes/notificacion.gif" alt="Notificación"/>
                </button>
            </div>
        </header>
        <main>
             <section class="seccion-formulario">
                <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                    <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                    <h2><span class="fuente-delgada">Solicitud de</span><br/><span class="fuente-gruesa">Permisos</span></h2>
                    <p>Ingrese las actividades que ha realizado.</p>
                    <br />
                    <br />
                    <div class="Division-elementos">
                        <div>
                             <label class="fuente-morada">Inicio</label>
                              <asp:TextBox type="date" id="inicio" runat="server" >Inicio</asp:TextBox>
                        </div>
                        <div>
                            <label class="fuente-morada">Final</label>
                            <asp:TextBox type="date" id="final" runat="server" >Final</asp:TextBox>
                        </div>
                    </div>
                    <label class="fuente-morada">Tipo</label>
                    <asp:TextBox ID="tipo" runat="server" placeholder="Ej: Médico"></asp:TextBox>
                    <label class="fuente-morada">Motivo</label>
                    <asp:TextBox ID="motivo" runat="server" TextMode="MultiLine" Rows="4" Columns="40" placeholder="Ingrese el motivo de la solicitud."></asp:TextBox>
                    <asp:Button ID="submit" runat="server" Text="Enviar" CssClass="button"/>
                </div>
            </section>
                <section class="seccion-imagen">
                    <img src="../../Imagenes/office.jpg" alt="Image of office">
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