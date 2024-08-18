<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualizarHorasColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.VisualizarHorasColaborador" MasterPageFile="~/Vistas/Site3.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <main>
            <section class="seccion-formulario">
               <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                  <h2><span class="fuente-delgada">Rendimiento de</span><br/><span class="fuente-gruesa">Colaborador</span></h2>
                  <p>Visualice el rendimiento del colaborador de esta semana.</p>
                  <br />
                   <asp:DropDownList ID="colaborador" runat="server" AutoPostBack="True" OnSelectedIndexChanged="colaborador_SelectedIndexChanged"></asp:DropDownList>
                   <label class="fuente-morada">Horas por semana a hacer</label>
                    <asp:Label CssClass="fuente-delgada" ID="lblHorasMeta" runat="server" Text="">40</asp:Label> 
                   <br />
                    <br />
                    <label class="fuente-morada">Horas trabajadas</label>
                    <asp:Label CssClass="fuente-delgada" ID="lblHoras" runat="server" Text=""></asp:Label>
                    <br />
                   <br />
                    <label class="fuente-morada">Inconsistencias</label>
                    <asp:Label CssClass="fuente-delgada" ID="lblInconsistencias" runat="server" Text=""></asp:Label>
            </div>
            </section>
            <section class="seccion-imagen">
               <img src="../../Imagenes/colaborador.png" alt="Image of office">
            </section>
         </main>
</asp:Content>