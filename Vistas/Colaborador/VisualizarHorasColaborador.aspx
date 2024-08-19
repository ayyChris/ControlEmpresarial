<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualizarHorasColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.VisualizarHorasColaborador" MasterPageFile="~/Vistas/Site2.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <main>
            <section class="seccion-formulario">
               <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                  <h2><span class="fuente-delgada">Rendimiento de</span><br/><span class="fuente-gruesa">Colaborador</span></h2>
                  <p>Visualice el rendimiento del colaborador de esta semana.</p>
                  <br />
                   <asp:DropDownList ID="colaborador" runat="server" AutoPostBack="True" OnSelectedIndexChanged="colaborador_SelectedIndexChanged"></asp:DropDownList>
                   <div class="matrix-container">
                        <h3>Horas e inconsistencias</h3>
                        <div class="matrix">
                            <div class="matrix-item">
                                <asp:Label ID="lblHorasMeta" CssClass="dynamic-data" runat="server">40,00</asp:Label>
                                <asp:Label ID="Label2" CssClass="static-label" runat="server" Text="Horas meta"></asp:Label>
                            </div>
                            <div class="matrix-item">
                                <asp:Label ID="lblHoras" CssClass="dynamic-data" runat="server"></asp:Label>
                                <asp:Label ID="Label3" CssClass="static-label" runat="server" Text="Horas trabajadas"></asp:Label>
                            </div>
                            <div class="matrix-item">
                                <asp:Label ID="lblInconsistencias" CssClass="dynamic-data" runat="server"></asp:Label>
                                <asp:Label ID="Label6" CssClass="static-label" runat="server" Text="Inconsistencias"></asp:Label>
                            </div>
                       </div>
                    </div>
                   <div class="matrix-container">
                     <h3>Dias trabajados</h3>
                     <div class="matrix">
                        <div class="matrix-item">
                            <asp:Label ID="lblLunes" CssClass="dynamic-data" runat="server"></asp:Label>
                            <asp:Label ID="LabelLunes" CssClass="static-label" runat="server" Text="Lunes"></asp:Label>
                        </div>
                        <div class="matrix-item">
                            <asp:Label ID="lblMartes" CssClass="dynamic-data" runat="server"></asp:Label>
                            <asp:Label ID="LabelMartes" CssClass="static-label" runat="server" Text="Martes"></asp:Label>
                        </div>
                        <div class="matrix-item">
                            <asp:Label ID="lblMiercoles" CssClass="dynamic-data" runat="server"></asp:Label>
                            <asp:Label ID="LabelMiercoles" CssClass="static-label" runat="server" Text="Miércoles"></asp:Label>
                        </div>
                        <div class="matrix-item">
                            <asp:Label ID="lblJueves" CssClass="dynamic-data" runat="server"></asp:Label>
                            <asp:Label ID="LabelJueves" CssClass="static-label" runat="server" Text="Jueves"></asp:Label>
                        </div>
                        <div class="matrix-item">
                            <asp:Label ID="lblViernes" CssClass="dynamic-data" runat="server"></asp:Label>
                            <asp:Label ID="LabelViernes" CssClass="static-label" runat="server" Text="Viernes"></asp:Label>
                        </div>
                        <div class="matrix-item">
                            <asp:Label ID="lblSabado" CssClass="dynamic-data" runat="server"></asp:Label>
                            <asp:Label ID="LabelSabado" CssClass="static-label" runat="server" Text="Sábado"></asp:Label>
                        </div>
                        <div class="matrix-item">
                            <asp:Label ID="lblDomingo" CssClass="dynamic-data" runat="server"></asp:Label>
                            <asp:Label ID="LabelDomingo" CssClass="static-label" runat="server" Text="Domingo"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            </section>
            <section class="seccion-imagen">
               <img src="../../Imagenes/colaborador.png" alt="Image of office">
            </section>
         </main>
</asp:Content>