<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RespuestaIncapacidad.aspx.cs" Inherits="ControlEmpresarial.Vistas.Incapacidades.RespuestaIncapacidad" MasterPageFile="~/Vistas/Site3.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra" />
        </section>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto; padding: 30px;">
                <h2><span class="fuente-delgada">Solicitud de</span><br /><span class="fuente-gruesa">Incapacidades</span></h2>
                <div class="form-group">
                    <label style="font-weight: bold;">Empleado:</label>
                    <asp:Label ID="lblEmpleado" runat="server" Text=""></asp:Label>
                </div>

                <div class="form-group">
                    <label style="font-weight: bold;">Fecha inicial de incapacidad:</label>
                    <asp:Label ID="lblFechaInicial" runat="server" Text=""></asp:Label>
                </div>

                <div class="form-group">
                    <label style="font-weight: bold;">Fecha final de incapacidad:</label>
                    <asp:Label ID="lblFechaFinal" runat="server" Text=""></asp:Label>
                </div>

                <div class="form-group">
                    <label style="font-weight: bold;">Evidencia:</label>
                    <asp:Label ID="lblEvidencia" runat="server" Text=""></asp:Label>
                </div>

                <div class="form-group">
                    <label style="font-weight: bold;">Estado:</label>
                    <asp:Label ID="lblEstado" runat="server" Text=""></asp:Label>
                </div>

                <br />
                <asp:Button ID="btnAceptar" CssClass="button" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" style="margin-top: 20px;" />
                <br />
                <asp:Button ID="btnDenegar" runat="server" CssClass="button-blanco" Text="Denegar" OnClick="btnDenegar_Click" style="margin-top: 10px;" />
            </div>
        </section>
    </main>
    <div class="divisor-forma-personalizado">
        <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
            <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
        </svg>
    </div>
</asp:Content>
