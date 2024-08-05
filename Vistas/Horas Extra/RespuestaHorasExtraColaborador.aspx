<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RespuestaHorasExtraColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.Horas_Extra.RespuestaHorasExtra" MasterPageFile="~/Vistas/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main>
          <section class="seccion-imagen">
                <img src="../../Imagenes/PulgaresArriba.png" alt="Ilustración de una persona trabajando horas extra"/>
            </section>

          <section class="seccion-formulario">
              <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                  <h2><span class="fuente-delgada">Solicitud de </span><span class="fuente-gruesa">Horas Extra</span></h2>
            <br />
            <br />
            <!-- Label para mostrar el idSolicitud -->
                 <asp:Label ID="lblSolicitudId" runat="server" Text="ID de Solicitud: " Font-Bold="True" />
                  <br />
                  <br />
                   <label>Horas Solicitadas:</label>
            <asp:Label ID="lblHorasSolicitadas" runat="server" />
            <br />
                  <div class="Division-elementos">
                <div>
                    <label>Incio:</label>
                    <asp:Label ID="lblHoraInicialExtra" runat="server" />
                </div>
                <div>
                    <label>Fin:</label>
                    <asp:Label ID="lblHoraFinalExtra" runat="server" />
                </div>
            </div>
            <asp:Label ID="lblMotivo" runat="server" />
            <br />
            <asp:Button ID="AceptarButton" CssClass="button" runat="server" Text="Aceptar" OnClick="AceptarButton_Click" />
            <br />
            <asp:Button ID="DenegarButton" runat="server" CssClass="button-blanco" Text="Denegar" OnClick="DenegarButton_Click" />
                  <asp:Label ID="Label1" runat="server"></asp:Label>
              </div>
          </section>
      </main>

      <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
                <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
        </div>
</asp:Content>