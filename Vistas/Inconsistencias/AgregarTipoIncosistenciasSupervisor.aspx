<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgregarTipoIncosistenciasSupervisor.aspx.cs" Inherits="ControlEmpresarial.Vistas.Inconsistencias.AgregarTipoIncosistenciasSupervisor" MasterPageFile="~/Vistas/Site3.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                <h2><span class="fuente-delgada">Registro de</span><br/><span>Inconsistencias</span></h2>
                <br />
                <br />
                <div class="tipo-actividad-container">
                    <label class="fuente-morada">Tipo de Incosistencia</label>
                    <asp:TextBox ID="tipoIncosistencia" runat="server" placeholder="Ingrese el tipo de Incosistencia"></asp:TextBox>
                    <br />
                    <label class="fuente-morada">Reponer Tiempo</label>
                   <asp:DropDownList ID="reponerTiempo" runat="server">
                        <asp:ListItem Text="Sí" Value="Sí" />
                        <asp:ListItem Text="No" Value="No" />
                    </asp:DropDownList>

                    <br />
                </div>
                 <label class="fuente-morada">Descripción</label>
                    <asp:TextBox ID="descripcionIncosistencia" runat="server" TextMode="MultiLine" Rows="3" placeholder="Ingrese la descripción"></asp:TextBox>
                    <br />
                <asp:Button ID="agregarTipoIncosistencia" runat="server" Text="Agregar" CssClass="button1" OnClick="agregarTipoIncosistencia_Click" />
                <asp:Label ID="debugLabel" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                
                <!-- GridView para mostrar tipos de actividad -->
                <div class="table-container">
                    <asp:GridView ID="gridViewTipoIncosistencia" runat="server" AutoGenerateColumns="False" CssClass="gridview" OnRowCommand="GridViewTipoIncosistencia_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="idTipoInconsistencia" HeaderText="ID" ReadOnly="True" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ReadOnly="True" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ReadOnly="True" />
                            <asp:BoundField DataField="ReponerTiempo" HeaderText="Reponer Tiempo" ReadOnly="True" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:Button ID="btnEliminar" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("idTipoInconsistencia") %>' Text="Eliminar" CssClass="button-delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
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
