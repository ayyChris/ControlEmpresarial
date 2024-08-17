<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reposicionesPendientesJefatura.aspx.cs" Inherits="ControlEmpresarial.Vistas.Reposicion.reposicionesPendientesJefatura" MasterPageFile="~/Vistas/Site2.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra" />
        </section>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario" style="max-width: 700px; margin: 0 auto; padding: 30px;">
                <h2><span class="fuente-delgada">Reposiciones</span><br /><span class="fuente-gruesa">Pendientes</span></h2>
                <br />
                <p style="margin-bottom: 15px;">Reposiciones por asignar</p>
                <br />
                <label class="fuente-morada" style="font-weight: bold; display: block; margin-bottom: 5px;">Inconsistencias listas para reponer:</label>
                <asp:DropDownList ID="incosistenciasPorReponer" runat="server" AutoPostBack="True"></asp:DropDownList>

                <div class="entradas-horario">
                    <div>
                        <label class="fuente-morada" for="txtFechaInicio">Fecha inicio:</label>
                        <asp:TextBox TextMode="Date" ID="txtFechaInicio" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <label class="fuente-morada" for="txtFechaFinal">Fecha final:</label>
                        <asp:TextBox TextMode="Date" ID="txtFechaFinal" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="entradas-horario">
                    <div>
                        <label class="fuente-morada" for="txthoraInicio">Hora inicio:</label>
                        <asp:TextBox TextMode="Time" ID="txthoraInicio" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <label class="fuente-morada" for="txthoraFinal">Hora final:</label>
                        <asp:TextBox TextMode="Time" ID="txthoraFinal" runat="server"></asp:TextBox>
                    </div>
                </div>
                <asp:Button ID="btnAsignar" CssClass="button" runat="server" Text="Aceptar" OnClick="btnAsignar_Click" style="margin-top: 20px;" />
                
            </div>
        </section>
    </main>
</asp:Content>
