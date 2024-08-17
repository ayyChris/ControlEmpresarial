<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AceptacionJustificacionInconsistenciaJefe.aspx.cs" Inherits="ControlEmpresarial.Vistas.Inconsistencias.AceptacionJustificacionInconsistenciaJefe" MasterPageFile="~/Vistas/Site2.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra" />
        </section>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                <h2>
                    <span class="fuente-delgada">Inconsistencia de</span><br />
                    <asp:Label ID="lblNombreEmpleado" CssClass="fuente-gruesa" runat="server"></asp:Label>
                </h2>
                <br />
                <br />
                <asp:Label ID="lblTipoInconsistencia" CssClass="info-label" runat="server">
                    <asp:Literal ID="litTipoInconsistencia" runat="server"></asp:Literal>
                </asp:Label>
                <br />
                <asp:Label ID="lblFechaJustificacion" CssClass="info-label" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblDescripcion" CssClass="info-label" runat="server">
                    <asp:Literal ID="litDescripcion" runat="server"></asp:Literal>
                </asp:Label>
                <br />
                <asp:Button ID="AceptarButton" CssClass="button" runat="server" Text="Aceptar" OnClick="AceptarButton_Click" />
                <br />
                <asp:Button ID="DenegarButton" CssClass="button-blanco" runat="server" Text="Denegar" OnClick="DenegarButton_Click" />
                <asp:Label ID="Label1" runat="server" CssClass="like-icon" Visible="false"></asp:Label>

                <asp:Label ID="lblSuccessMessage" runat="server" CssClass="success-message" Visible="false"></asp:Label>
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" Visible="false"></asp:Label>

            </div>
        </section>
    </main>

    <div class="divisor-forma-personalizado">
        <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
            <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
        </svg>
    </div>
</asp:Content>
