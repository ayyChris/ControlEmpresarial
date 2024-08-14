<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreVisualIncosistenciasColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.Inconsistencias.PreVisualIncosistenciasColaborador" MasterPageFile="~/Vistas/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra">
        </section>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario">
                <h2><span class="fuente-delgada">Sus </span><span class="fuente-gruesa">Incosistencias</span></h2>
                            </div>
            <asp:Repeater ID="RepeaterInconsistencias" runat="server" OnItemCommand="RepeaterInconsistencias_ItemCommand">
                <itemtemplate>
                    <div class="tarjeta-item-inconsistencia">
                        <h3><%# Eval("TipoInconsistencia") %></h3>
                        <p>Fecha: <%# Eval("Fecha", "{0:dd/MM/yyyy}") %></p>
                        <p>Estado: <%# Eval("Estado") %></p>
                        <asp:Button CssClass="button-grid" ID="btnVerDetalles" runat="server" Text="Ver Detalles"
                            CommandName="VerDetalles"
                            CommandArgument='<%# Eval("idInconsistencia") %>' />
                    </div>
                </itemtemplate>
            </asp:Repeater>
            <asp:Label ID="lblCommandArgument" runat="server" Text='<%# Eval("idInconsistencia") %>' />

                <asp:Label ID="lblNoInconsistencias" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
        </section>
    </main>
    <div class="divisor-forma-personalizado">
        <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
            <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
        </svg>
    </div>
</asp:Content>
