<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgregarTipoActividadesSupervisor.aspx.cs" Inherits="ControlEmpresarial.Vistas.Control_de_Actividades.AgregarTipoActividadesSupervisor"  MasterPageFile="~/Vistas/Site3.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                <h2><span class="fuente-delgada">Registro de</span><br/><span>Actividades</span></h2>
                <br />
                <br />
                <div class="tipo-actividad-container">
                    <label class="fuente-morada">Tipo de actividad</label>
                    <asp:TextBox ID="tipoActividad" runat="server" placeholder="Ingrese el tipo de actividad"></asp:TextBox>
                    <asp:Button ID="agregarTipoActividad" runat="server" Text="Agregar" CssClass="button1" OnClick="AgregarTipoActividad_Click" />
                </div>
                <asp:Label ID="debugLabel" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                
                <!-- GridView para mostrar tipos de actividad -->
                <div class="table-container">
                    <asp:GridView ID="gridViewTipoActividades" runat="server" AutoGenerateColumns="False" CssClass="gridview" OnRowCommand="GridViewTipoActividades_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="idTipo" HeaderText="ID" ReadOnly="True" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo de Actividad" ReadOnly="True" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:Button ID="btnEliminar" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("idTipo") %>' Text="Eliminar" CssClass="button-delete" />
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
