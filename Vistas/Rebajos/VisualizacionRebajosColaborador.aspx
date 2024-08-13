<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualizacionRebajosColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.VisualizacionRebajosColaborador"  MasterPageFile="~/Vistas/Site1.master"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <main>
            <section class="seccion-formulario">
               <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
               <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                  <h2><span class="fuente-delgada">Historial de</span><br/><span class="fuente-gruesa">Rebajos</span></h2>
                  <p>Visualice todos los rebajos hasta la fecha de hoy.</p>
                  <br />
                  <br />
                    <asp:GridView ID="gvRebajos" runat="server" AutoGenerateColumns="False" OnRowCommand="gvRebajos_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="idRebajo" HeaderText="Rebajo" />
                            <asp:BoundField DataField="idEmpleado" HeaderText="idEmpleado" />
                            <asp:BoundField DataField="TipoRebajoID" HeaderText="TipoRebajoID" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                            <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                            <asp:BoundField DataField="Monto" HeaderText="Monto" />
                        </Columns>
                    </asp:GridView>
               </div>
            </div>
            </section>
            <section class="seccion-imagen">
               <img src="../../Imagenes/colaborador.png" alt="Image of office">
            </section>
         </main>
</asp:Content>
