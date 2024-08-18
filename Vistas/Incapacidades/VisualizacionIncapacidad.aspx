<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualizacionIncapacidad.aspx.cs" Inherits="ControlEmpresarial.Vistas.VisualizacionIncapacidad" MasterPageFile= "~/Vistas/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
    /* Estilos para el GridView */
    .grid-view {
        width: 100%;
        border-collapse: collapse;
        margin-bottom: 20px;
    }

    .grid-view th {
        background-color: #f2f2f2;
        color: #333;
        text-align: left;
        padding: 10px;
        font-weight: bold;
    }

    .grid-view td {
        padding: 10px;
        border-bottom: 1px solid #ddd;
        color: #333;
    }

    .grid-view tr:nth-child(even) {
        background-color: #f9f9f9;
    }

    .grid-view tr:hover {
        background-color: #f1f1f1;
    }

    .button {
        background-color: #4CAF50;
        color: white;
        border: none;
        padding: 8px 16px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        font-size: 14px;
        cursor: pointer;
        border-radius: 4px;
    }

    .button:hover {
        background-color: #45a049;
    }
</style>
         <main>
            <section class="seccion-formulario">
    <div class="tarjeta-formulario" style="max-width: 1400px; margin: 0 auto;"">
        <h2><span class="fuente-delgada">Historial de</span><br/><span class="fuente-gruesa">Incapacidades</span></h2>
        <p>Todos las solicitudes de incapacidad hasta hoy.</p>
        <br />
        <br />
        <asp:GridView ID="gvIncapacidades" runat="server" AutoGenerateColumns="False" CssClass="grid-view">
            <Columns>
                <asp:BoundField DataField="idIncapacidad" HeaderText="Incapacidad" />
                <asp:BoundField DataField="idEmpleado" HeaderText="Empleado" Visible="false" /> 
                <asp:BoundField DataField="TipoIncapacidad" HeaderText="ID del tipo de incapacidad" />
                <asp:BoundField DataField="FechaInicial" HeaderText="Fecha Inicial" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" />
                <asp:BoundField DataField="FechaFinal" HeaderText="Fecha Final" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" />
                <asp:BoundField DataField="Evidencia" HeaderText="Evidencia" />
                <asp:BoundField DataField="DiasReduccion" HeaderText="Dias con saldo reducido" />
                <asp:BoundField DataField="Estado" HeaderText="Estado" />
            </Columns>
        </asp:GridView>
    </div>
</section>
         </main>
            <div class="divisor-forma-personalizado">
      <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
         <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
      </svg>
   </div>
</asp:Content>  