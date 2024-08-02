<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistroActividadesColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.Control_de_Actividades.RegistroActividadesColaborador" MasterPageFile="~/Vistas/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <main>
            <section class="seccion-formulario">
                <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto;">
                    <div class="contenido-horas">
                        <div>
                            <h2><span class="fuente-delgada">Control de</span><br /><span class="fuente-gruesa">Actividades</span></h2>
                            <p>Ingrese las actividades. </p>
                                            </div>
                    <div>
                                                <asp:Label ID="LabelHorasRestantes" runat="server" CssClass="label-horas-restantes" ></asp:Label>
                    </div>
                    </div>
                    <label class="fuente-morada">Título</label>
                    <asp:TextBox ID="titulo" runat="server" placeholder="Ingrese la actividad realizada."></asp:TextBox>
                    <br />
                    <div class="tipo-actividad-container">
                        <label class="fuente-morada">Seleccionar Tipo de actividad</label>
                        <asp:DropDownList ID="dropdownTipoActividad" runat="server" CssClass="dropdown"></asp:DropDownList>
                    </div>
                    <div class="entradas-horario">
                        <div>
                            <label class="fuente-morada" for="hora-inicio">Hora inicio</label>
                            <asp:TextBox TextMode="Time" ID="horaInicio" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label class="fuente-morada" for="hora-final">Hora final</label>
                            <asp:TextBox TextMode="Time" ID="horaFinal" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <label class="fuente-morada">Descripción</label>
                    <asp:TextBox ID="actividad" runat="server" TextMode="MultiLine" Rows="4" Columns="40" placeholder="Ingrese la actividad realizada."></asp:TextBox>
                    <asp:Button ID="submit" runat="server" Text="Enviar" CssClass="button" OnClick="Submit_Click" />
                    <asp:Label ID="debugLabel" runat="server" CssClass="debug-label" EnableViewState="true" />
                    <asp:Label ID="Label1" runat="server" CssClass="debug-label" EnableViewState="false" />
                     <asp:Label ID="Label2" runat="server" CssClass="debug-label" EnableViewState="false" />

                </div>
            </section>
        </main>



         <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
               <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
         </div>
</asp:Content>
