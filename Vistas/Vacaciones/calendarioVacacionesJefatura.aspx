<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calendarioVacacionesJefatura.aspx.cs" Inherits="ControlEmpresarial.Controlador.Vacaciones.calendarioVacacionesJefatura" MasterPageFile="~/Vistas/Site2.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <div class="container">
            <h2>Calendario de Vacaciones</h2>
            <div class="calendar-header">
                <asp:Button ID="btnPrevMonth" runat="server" Text="&#8249;" OnClick="btnPrevMonth_Click" CssClass="calendar-button-left" />
                <h3 id="monthYear" runat="server" class="month-year"></h3>
                <asp:Button ID="btnNextMonth" runat="server" Text="&#8250;" OnClick="btnNextMonth_Click" CssClass="calendar-button-right" />
            </div>
            <div class="calendar" id="calendar">
                <asp:Repeater ID="calendarRepeater" runat="server">
                    <ItemTemplate>
                        <div class="day <%# Eval("Class") %>">
                            <%# Eval("Dia") %>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="calendar-footer">
                <span>&#9632; = Días Libres</span>
            </div>
        </div>
    </main>
    <div class="divisor-forma-personalizado">
        <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
            <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
        </svg>
    </div>
</asp:Content>
