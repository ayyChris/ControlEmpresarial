<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoricoActividadesSupervisor.aspx.cs" Inherits="ControlEmpresarial.Vistas.HistoricoActividades" MasterPageFile="~/Vistas/Site3.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra">
        </section>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario">
                <h2><span class="fuente-delgada">Historico </span><span class="fuente-gruesa">Actividades</span></h2>
                <div class="table-container">
                    <asp:GridView ID="gvHorasActividades" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="idRegistroActividades" HeaderText="ID Registro Actividades" />
                            <asp:BoundField DataField="NombreJefe" HeaderText="Nombre Jefe" />
                            <asp:BoundField DataField="NombreEmpleado" HeaderText="Nombre Empleado" />
                            <asp:BoundField DataField="TituloActividad" HeaderText="Título Actividad" />
                            <asp:BoundField DataField="DescripcionActividad" HeaderText="Descripción Actividad" />
                            <asp:BoundField DataField="TipoActividad" HeaderText="Tipo Actividad" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <asp:Label ID="Label1" runat="server"></asp:Label>
        </section>
    </main>
    <div class="divisor-forma-personalizado">
        <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
            <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
        </svg>
    </div>
</asp:Content>
