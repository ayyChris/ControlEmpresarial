<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TablaPreVisualizacionVacaciones.aspx.cs" Inherits="ControlEmpresarial.Vistas.PrevisualizacionVacacionesJefe" MasterPageFile= "~/Vistas/Site2.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra">
        </section>
        <section class="seccion-formulario">
            <div class="table-container">
                <asp:GridView ID="gvVacaciones" runat="server" AutoGenerateColumns="False" OnRowCommand="gvVacaciones_RowCommand" OnRowDataBound="gvVacaciones_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="idSolicitudVacaciones" HeaderText="ID Solicitud" />
                        <asp:BoundField DataField="idEmpleado" HeaderText="Empleado" />
                        <asp:BoundField DataField="FechaPublicada" HeaderText="Fecha de la Solicitud" />
                        <asp:BoundField DataField="FechaVacacion" HeaderText="Fecha del día a disfrutar" />
                        <asp:BoundField DataField="DiasDisfrutados" HeaderText="Días a disfrutar" />
                        <asp:ButtonField ControlStyle-CssClass="button" ButtonType="Button" CommandName="VerDetalle" Text="Ver Detalle" />
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