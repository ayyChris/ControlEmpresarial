<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="solicitudHorasExtrasJefatura.aspx.cs" Inherits="ControlEmpresarial.Vistas.solicitudHorasExtras" MasterPageFile="~/Vistas/Site2.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario">
                <h2><span class="fuente-delgada">Solicitud de</span><br><span class="fuente-gruesa">Horas Extra</span></h2>
                <p>Haz tu solicitud para horas extra al colaborador.</p>
                <label class="fuente-morada" for="colaboradorACargo">Colaborador a cargo</label>
                <asp:DropDownList ID="colaborador" runat="server">
                    <asp:ListItem Text="Seleccione" Value="" />
                </asp:DropDownList>
                <br />
                <label class="fuente-morada" for="dia">Día</label>
                <asp:TextBox TextMode="Date" ID="dia" runat="server" placeholder="Ej. 30/06/2024" AutoPostBack="True" OnTextChanged="dia_TextChanged"></asp:TextBox>
                <br />
                <asp:Label ID="horarioLabel" CssClass="dynamic-label" runat="server"></asp:Label>
                <div class="entradas-horario">
                    <div>
                        <label class="fuente-morada" for="hora-inicio">Hora inicio</label>
                        <asp:TextBox TextMode="Time" ID="horaInicio" runat="server" ></asp:TextBox>
                    </div>
                    <div>
                        <label class="fuente-morada" for="hora-final">Hora final</label>
                        <asp:TextBox TextMode="Time" ID="horaFinal" runat="server" ></asp:TextBox>
                    </div>
                </div>
                <br />
                <label class="fuente-morada" for="motivo">Motivo</label>
                <asp:TextBox ID="motivo" TextMode="MultiLine" runat="server" placeholder="Ingresa el motivo de la solicitud de horas extra."></asp:TextBox>
                <br />
                <asp:Button ID="submit" runat="server" CssClass="button" Text="Enviar" OnClick="submit_Click"/>
                <i id="likeIcon" class="fas fa-thumbs-up" style="display:none;"></i>
                <asp:Label ID="lblMensaje" runat="server" Visible="false"></asp:Label>
            
            </div>
            <asp:Label ID="lblHoraInicio" runat="server" CssClass="debug-label"></asp:Label>
<asp:Label ID="lblHoraFinal" runat="server" CssClass="debug-label"></asp:Label>
<asp:Label ID="lblHorasSolicitadas" runat="server" CssClass="debug-label"></asp:Label>
<asp:Label ID="lblHorasPermitidasDiarias" runat="server" CssClass="debug-label"></asp:Label>
<asp:Label ID="lblHorasExtrasPrevias" runat="server" CssClass="debug-label"></asp:Label>
<asp:Label ID="lblHorasPermitidasSemanales" runat="server" CssClass="debug-label"></asp:Label>

        </section>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra">
        </section>
    </main>
    <div class="divisor-forma-personalizado">
        <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
            <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
        </svg>
    </div>
</asp:Content>
