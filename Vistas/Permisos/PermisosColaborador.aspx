<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PermisosColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.PermisosColaborador" MasterPageFile="~/Vistas/Site1.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <main>
            <section class="seccion-formulario">
               <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
               <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                  <h2><span class="fuente-delgada">Solicitud de</span><br/><span class="fuente-gruesa">Permisos</span></h2>
                  <p>Ingrese las actividades que ha realizado.</p>
                  <br />
                  <br />
                  <div class="Division-elementos">
                     <div>
                        <label class="fuente-morada">Inicio</label>
                        <asp:TextBox type="date" id="txtInicio" runat="server" >Inicio</asp:TextBox>
                     </div>
                     <div>
                        <label class="fuente-morada">Final</label>
                        <asp:TextBox type="date" id="txtFinal" runat="server" >Final</asp:TextBox>
                     </div>
                  </div>
                  <label class="fuente-morada">Tipo</label>
                  <asp:TextBox ID="txtTipo" runat="server" placeholder="Ej: Médico"></asp:TextBox>
                  <label class="fuente-morada">Motivo</label>
                  <asp:TextBox ID="txtMotivo" runat="server" TextMode="MultiLine" Rows="4" Columns="40" placeholder="Ingrese el motivo de la solicitud."></asp:TextBox>
                  <asp:Button ID="submit" runat="server" Text="Enviar" CssClass="button" OnClick="submit_Click" />
                     <i id="likeIcon" class="fas fa-thumbs-up" style="display:none;"></i>
                     <asp:Label ID="lblMensaje" runat="server" Visible="false"></asp:Label>
               </div>
            </div>
            </section>
            <section class="seccion-imagen">
               <img src="../../Imagenes/office.jpg" alt="Image of office">
            </section>
         </main>
         <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
               <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
         </div>
</asp:Content>       