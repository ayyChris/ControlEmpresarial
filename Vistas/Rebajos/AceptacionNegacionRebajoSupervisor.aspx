<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AceptacionNegacionRebajoSupervisor.aspx.cs" Inherits="ControlEmpresarial.Vistas.Rebajos.AceptacionNegacionRebajoSupervisor" MasterPageFile="~/Vistas/Site3.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra" />
        </section>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto; padding: 30px;">
                <h2><span class="fuente-delgada">Solicitudes de</span><br /><span class="fuente-gruesa">Rebajos</span></h2>
                <br />
                <p style="margin-bottom: 15px;">Rebajo</p>
                <br />

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Empleado:</label>
                <asp:Label ID="lblEmpleado" runat="server" Text=""></asp:Label>

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Fecha:</label>
                <asp:Label ID="lblFechaSolicitud" runat="server" Text=""></asp:Label>

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Motivo:</label>
                <asp:Label ID="lblMotivo" runat="server" Text=""></asp:Label>

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Monto en porcentaje:</label>
                <asp:Label ID="lblPorcentajeRebajo" runat="server" Text=""></asp:Label>

                <br />
                <asp:Button ID="btnAceptar" CssClass="button" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" style="margin-top: 20px;" />
                <br />
                <asp:Button ID="btnDenegar" runat="server" CssClass="button-blanco" Text="Denegar" OnClick="btnDenegar_Click" style="margin-top: 10px;" />
            </div>
        </section>
    </main>
</asp:Content>
