<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreAceptacionHorasExtra.aspx.cs" Inherits="ControlEmpresarial.Vistas.Horas_Extra.PreAceptacionHorasExtra"   MasterPageFile="~/Vistas/Site1.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <main>
            <section class="seccion-imagen">
               <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra"/>
            </section>
            <section class="seccion-formulario">
                <div class="table-container">
                    <asp:GridView ID="gridViewHorasExtra" runat="server" AutoGenerateColumns="False" CssClass="table" OnRowCommand="gridViewHorasExtra_RowCommand"  OnRowDataBound="gridViewHorasExtra_RowDataBound">
                 <Columns>
                     <asp:BoundField DataField="FechaInicioSolicitud" HeaderText="Fecha de Solicitud" />
                     <asp:BoundField DataField="FechaFinalSolicitud" HeaderText="Día" />
                     <asp:BoundField DataField="HoraInicialExtra" HeaderText="Hora Inicial" />
                     <asp:BoundField DataField="HoraFinalExtra" HeaderText="Hora Final" />
                     <asp:BoundField DataField="HorasSolicitadas" HeaderText="Horas Totales" />
                     <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                     <asp:TemplateField HeaderText="Acción">
                         <ItemTemplate>
                             <asp:Button ID="btnAccion" runat="server" Text="Acción" CommandName="Accion" CommandArgument='<%# Eval("idSolicitud") %>' />
                         </ItemTemplate>
                     </asp:TemplateField>
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