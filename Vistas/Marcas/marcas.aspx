<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="marcas.aspx.cs" Inherits="ControlEmpresarial.Vistas.marcas" MasterPageFile="~/Vistas/Site1.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

        document.addEventListener("DOMContentLoaded", function () {
            updateClock();
        });
    </script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <main>
        <div class="marcas-contenedor">
            <img src="../../Imagenes/reloj.png" alt="Clock Icon" class="clock-icon" />
            <div class="info">
                <h2 id="clock">Hora</h2>
                <p><asp:Label ID="lblDiaSemana" runat="server" Text="Día de la semana" /></p>
                <p><asp:Label ID="lblHorario" runat="server" Text="Horario" /></p>
                <div class="buttons">
                    <br />
                    <asp:Button ID="btnEntrada" runat="server" Text="Entrada" class="button" OnClick="btnEntrada_Click"/>
                    <br />
                    <asp:Button ID="btnSalida" runat="server" Text="Salida" class="button-blanco" OnClick="btnSalida_Click"/>
                </div>
                <br />
            </div>
        </div>
    </main>
    <div class="divisor-forma-personalizado">
        <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
            <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
        </svg>
    </div>
</asp:Content>
