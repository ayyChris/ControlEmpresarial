<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvidenciaIncosistenciaColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.Inconsistencias.EvidenciaIncosistenciaColaborador" MasterPageFile="~/Vistas/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra">
        </section>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario">
                <h2><span class="fuente-delgada">Sus </span><span class="fuente-gruesa">Incosistencias</span></h2>
                <asp:Label ID="lblSaludo" runat="server" CssClass="label-saludo"></asp:Label>
                <br />
                <asp:Label ID="lblPregunta" runat="server" CssClass="label-pregunta"></asp:Label>
                
                <asp:HiddenField ID="hfIdInconsistencia" runat="server" />
                
                <asp:TextBox ID="txtJustificacion" runat="server" TextMode="MultiLine" Rows="5" placeholder="Justificala" CssClass="textbox-justificacion"></asp:TextBox>
                
                <asp:Button ID="btnEnviar" runat="server" Text="Enviar Justificación" CssClass="button-grid" OnClick="btnEnviar_Click" />
                
                <asp:Label ID="lblNoInconsistencias" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:Label ID="lblDebug" runat="server" ForeColor="Red" />

            </div>
        </section>
    </main>
    <div class="divisor-forma-personalizado">
        <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
            <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
        </svg>
    </div>
</asp:Content>
