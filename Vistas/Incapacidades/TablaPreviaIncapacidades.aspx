<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TablaPreviaIncapacidades.aspx.cs" Inherits="ControlEmpresarial.Vistas.Incapacidades.TablaPreviaIncapacidades" MasterPageFile="~/Vistas/Site3.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <main>
            <section class="seccion-imagen">
               <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando"/>
            </section>
            <section class="seccion-formulario">
                <div class="table-container">
                    <asp:GridView ID="gridIncapacidades" runat="server" AutoGenerateColumns="False" CssClass="table" OnRowCommand="gridIncapacidades_RowCommand"  OnRowDataBound="gridIncapacidades_RowDataBound">
                 <Columns>
                     <asp:BoundField DataField="idIncapacidad" HeaderText="ID Solicitud" />
                     <asp:BoundField DataField="idEmpleado" HeaderText="Empleado" />
                     <asp:BoundField DataField="FechaInicial" HeaderText="Fecha inicial" />
                     <asp:BoundField DataField="FechaFinal" HeaderText="Fecha Final" />
                     <asp:BoundField DataField="Evidencia" HeaderText="Evidencia" />
                     <asp:BoundField DataField="Estado" HeaderText="Estado" />
                     <asp:ButtonField ControlStyle-CssClass="button" ButtonType="Button" CommandName="VerDetalle" Text="Ver Detalle" />
                 </Columns>
             </asp:GridView>
         </div>
                <asp:Label ID="lblMensaje" runat="server"></asp:Label>    
            </section>
        </main>

         <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
                <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
        </div>

</asp:Content>