<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AceptacionNegacionReposicionJefatura.aspx.cs" Inherits="ControlEmpresarial.Vistas.Reposicion.AceptacionNegacionReposicionJefatura" MasterPageFile="~/Vistas/Site2.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra" />
        </section>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto; padding: 30px;">
                <h2><span class="fuente-delgada">Solicitudes de</span><br /><span class="fuente-gruesa">Reposiciones</span></h2>
                <br />
                <p style="margin-bottom: 15px;">Reposiciones</p>
                <br />

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Reposicion:</label>
                <asp:Label ID="lblReposicion" runat="server" Text=""></asp:Label>

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Inconsisntencia:</label>
                <asp:Label ID="lblInconsisntencia" runat="server" Text=""></asp:Label>

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Empleado:</label>
                <asp:Label ID="lblEmpleado" runat="server" Text=""></asp:Label>

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Evidencia:</label>
                <asp:Label ID="lblEvidencia" runat="server" Text=""></asp:Label>

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Fecha:</label>
                <asp:Label ID="lblFecha" runat="server" Text=""></asp:Label>

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Se cumplio con las horas:</label>
                <asp:Label ID="Label1" runat="server" Text="">Si</asp:Label>
                <br />
                <asp:Button ID="btnAceptar" CssClass="button" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" style="margin-top: 20px;" />
                <br />
                <asp:Button ID="btnDenegar" runat="server" CssClass="button-blanco" Text="Denegar" OnClick="btnDenegar_Click" style="margin-top: 10px;" />
            </div>
        </section>
    </main>
</asp:Content>
