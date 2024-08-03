<%@ Page EnableEventValidation="true" Language="C#" AutoEventWireup="true" CodeBehind="TablePreAceptacionActividadColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.Control_de_Actividades.TablePreAceptacionJefatura" MasterPageFile="~/Vistas/Site2.master"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <main>
            <section class="seccion-imagen">
               <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra"/>
            </section>
            <section class="seccion-formulario">
               <div class="tarjeta-formulario">
                  <h2>Actividades</h2>
               </div>
                <div class="table-container">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" EnableViewState="true" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="Actividad" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                        <asp:BoundField DataField="Titulo" HeaderText="Titulo" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                        <asp:TemplateField HeaderText="Acción">
                            <ItemTemplate>
                                <asp:Button ID="btnAccion" CssClass="button-grid" runat="server" Text="Solicitar" CommandName="Accion" CommandArgument='<%# Container.DataItemIndex %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>
                     <asp:Label ID="Label1" runat="server" EnableViewState="true" Visible="false"></asp:Label>
        </div>
            </section>
         </main>
         <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
               <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
         </div>

</asp:Content>