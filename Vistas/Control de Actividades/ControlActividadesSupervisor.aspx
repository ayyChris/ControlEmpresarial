<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ControlActividadesSupervisor.aspx.cs" Inherits="ControlEmpresarial.Vistas.Control_de_Actividades.ControlActividadesSupervisor"   MasterPageFile="~/Vistas/Site3.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra"/>
        </section>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                <h2><span class="fuente-delgada">Control de</span><br/><span>Actividad por Colaborador</span></h2>
                <br />
                <br />
                <asp:DropDownList ID="departamento" runat="server" AutoPostBack="True" OnSelectedIndexChanged="departamento_SelectedIndexChanged">
</asp:DropDownList>


<asp:Label ID="Label1" runat="server" ForeColor="Red" Visible="false"></asp:Label>

                <div class="main-content">
    <!-- Sección de Actividades del Día -->
    <div class="matrix-container">
        <h3>Actividades del Día</h3>
        <div class="matrix">
            <div class="matrix-item">
                <asp:Label ID="PendientesDia" CssClass="dynamic-data" runat="server"></asp:Label>
                <asp:Label ID="Label2" CssClass="static-label" runat="server" Text="Pendientes"></asp:Label>
            </div>
            <div class="matrix-item">
                <asp:Label ID="RealizadasDia" CssClass="dynamic-data" runat="server"></asp:Label>
                <asp:Label ID="Label3" CssClass="static-label" runat="server" Text="Realizadas"></asp:Label>
            </div>
            <div class="matrix-item">
                <asp:Label ID="DenegadasDia" CssClass="dynamic-data" runat="server"></asp:Label>
                <asp:Label ID="Label6" CssClass="static-label" runat="server" Text="Denegadas"></asp:Label>
            </div>
        </div>
    </div>

    <!-- Sección de Actividades de la Quincena -->
    <div class="matrix-container">
        <h3>Actividades de la Quincena</h3>
        <div class="matrix">
            <div class="matrix-item">
                <asp:Label ID="PendientesQuincena" CssClass="dynamic-data" runat="server"></asp:Label>
                <asp:Label ID="Label5" CssClass="static-label" runat="server" Text="Pendientes"></asp:Label>
            </div>
            <div class="matrix-item">
                <asp:Label ID="RealizadasQuincena" CssClass="dynamic-data" runat="server"></asp:Label>
                <asp:Label ID="Label4" CssClass="static-label" runat="server" Text="Realizadas"></asp:Label>
            </div>
            <div class="matrix-item">
                <asp:Label ID="DenegadasQuincena" CssClass="dynamic-data" runat="server"></asp:Label>
                <asp:Label ID="Label7" CssClass="static-label" runat="server" Text="Denegadas"></asp:Label>
            </div>
        </div>
    </div>
</div>


                </div>
        </section>
    </main>
     <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
               <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
         </div>
</asp:Content>
