<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnviarEvidenciaReposicionColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.Reposicion.EnviarEvidenciaReposicionColaborador"  MasterPageFile="~/Vistas/Site1.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra" />
        </section>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto; padding: 30px;">
                <h2><span class="fuente-delgada">Enviar evidencia de</span><br /><span class="fuente-gruesa">Rebajo</span></h2>
                <br />
                <p style="margin-bottom: 15px;">Evidencia</p>
                <br />

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Reposición:</label>
                <asp:Label ID="lblEmpleado" runat="server" Text=""></asp:Label>

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Inconsistencia:</label>
                <asp:Label ID="lblFechaSolicitud" runat="server" Text=""></asp:Label>

                <label style="font-weight: bold; display: block; margin-top: 15px; margin-bottom: 5px;">Evidencia:</label>
                <asp:TextBox CssClass="inicioSesion" ID="txtEvidencia" runat="server"></asp:TextBox>

                <br />
                <asp:Button ID="btnEnviarEvidencia" CssClass="button" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" style="margin-top: 20px;" />
            </div>
        </section>
    </main>
</asp:Content>
