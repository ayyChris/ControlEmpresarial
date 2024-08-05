<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvidenciaHorasExtra.aspx.cs" Inherits="ControlEmpresarial.Vistas.Horas_Extra.EvidenciaHorasExtra"  MasterPageFile="~/Vistas/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
             <section class="seccion-imagen">
                <img src="../../Imagenes/PulgaresArriba.png" alt="Ilustración de una persona trabajando horas extra"/>
            </section>

             <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                  <h2><span class="fuente-delgada">Evidencia </span><span class="fuente-gruesa">Horas Extra</span></h2>
            <br />
            <p>Horas Extra</p>
            <br />
                 <label class="fuente-morada" for="colaboradorACargo">Solicitud de Horas Extra</label>
                  <asp:DropDownList ID="colaborador" runat="server" AutoPostBack="True" OnSelectedIndexChanged="colaborador_SelectedIndexChanged">
                    <asp:ListItem Text="Seleccione" Value="" />
                </asp:DropDownList>

                  <br />
                 <asp:Label ID="DinamicDescription" runat="server" Style="font-weight: bold;"></asp:Label>

                 <br />
                 <br />
                  <label class="fuente-morada" for="motivo">Evidencia</label>
                  <asp:TextBox ID="Evidencia" TextMode="MultiLine" runat="server" placeholder="Ingresa la evidencia de la solicitud de horas extra."></asp:TextBox>
                  <br />
                 <asp:Button CssClass="button" ID="submit" runat="server" Text="Enviar" OnClick="submit_Click"/>
                 <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" Visible="False"></asp:Label>

            </div>
        </main>

        <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
                <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
        </div>
</asp:Content>