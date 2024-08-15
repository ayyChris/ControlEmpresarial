<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitarIncapacidad.aspx.cs" Inherits="ControlEmpresarial.Vistas.Incapacidades.SolicitarIncapacidad" MasterPageFile= "~/Vistas/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <main>
            <section class="seccion-formulario">
               <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                  <h2><span class="fuente-delgada">Solicitud de</span><br><span class="fuente-gruesa">Incapacidad</span></h2>
                  <p>Has tu solicitud para registrar su incapacidad.</p>
                  <br />
                  <br />
                  <div class="entradas-horario">
                  <div>
                <label class="fuente-morada" for="horario">Tipo de incapacidad</label>
                        <asp:DropDownList ID="tipoIncapacidad" runat="server">
                            <asp:ListItem Text="Seleccione" Value="" />
                        </asp:DropDownList>
                    </div>
                     <div>
                        <label class="fuente-morada" for="hora-inicio">Fecha de inicio</label>
                        <asp:TextBox ID="fechaInicio" runat="server" placeholder="Ej. 30/06/2024" TextMode="Date"></asp:TextBox>
                     </div>
                     <div>
                        <label class="fuente-morada" for="hora-final">Fecha final</label>
                        <asp:TextBox ID="fechaFinal" runat="server" placeholder="Ej. 30/06/2024" TextMode="Date"></asp:TextBox>
                     </div>
                  </div>
                   <br />
                 <label class="fuente-morada" for="motivo">Evidencia</label>
                 <asp:TextBox ID="Evidencia" TextMode="MultiLine" runat="server" placeholder="Ingresa la evidencia de la incapacidad."></asp:TextBox>
                 <br />
                  <br />
                  <asp:Button ID="submit" runat="server" Text="Enviar" CssClass="button" OnClick="submit_Click"/>
                  <asp:Label ID="lblMensaje" runat="server" Visible="false"></asp:Label>
               </div>
            </section>
         </main>
         <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
               <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
         </div>
      </asp:Content>  