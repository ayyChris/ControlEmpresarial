<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnviarMensajeColaboradores.aspx.cs" Inherits="ControlEmpresarial.Vistas.Colaborador.EnviarMensajeColaboradores" MasterPageFile="~/Vistas/Site2.master" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
<main>
   <section class="seccion-formulario">
      <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
      <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
         <h2><span class="fuente-delgada">Enviar Mensaje a</span><br/><span class="fuente-gruesa">Colaborador</span></h2>
         <p>Envia mensajes a colaboradores con IA</p>
         <br />
         <br />
         <label class="fuente-morada">Colaborador</label>
         <asp:DropDownList ID="ddlColaborador" runat="server">
            <asp:ListItem Text="Seleccione" Value="" />
        </asp:DropDownList>
         <label class="fuente-morada">Prompt</label>
         <asp:TextBox ID="txtPrompt" runat="server" placeholder=""></asp:TextBox>
          <asp:Button ID="generarPrompt" runat="server" Text="Generar mensaje" CssClass="button-blanco" OnClick="generarPrompt_Click" />
          <br />
          <br />
         <label class="fuente-morada">Mensaje</label>
         <asp:TextBox ID="txtMensaje" runat="server" TextMode="MultiLine" Rows="4" Columns="40" placeholder=""></asp:TextBox>
         <asp:Button ID="submit" runat="server" Text="Enviar" CssClass="button" OnClick="submit_Click" />
            <i id="likeIcon" class="fas fa-thumbs-up" style="display:none;"></i>
            <asp:Label ID="lblMensaje" runat="server" Visible="false"></asp:Label>
      </div>
   </div>
   </section>
   <section class="seccion-imagen">
      <img src="../../Imagenes/colaborador.png" alt="Image of office">
   </section>
</main>
</asp:Content>