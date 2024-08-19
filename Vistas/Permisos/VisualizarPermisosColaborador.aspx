<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualizarPermisosColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.Permisos.VisualizarPermisosColaborador"  MasterPageFile="~/Vistas/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra">
        </section>
        <section class="seccion-formulario">
            <div class="table-container">
                <asp:GridView ID="gvPermisos" runat="server" AutoGenerateColumns="False" >
                    <Columns>
                        <asp:BoundField DataField="idPermiso" HeaderText="ID Permiso" />
                        <asp:BoundField DataField="idEmpleado" HeaderText="Empleado" />
                        <asp:BoundField DataField="FechaPublicada" HeaderText="Fecha de la Solicitud" />
                        <asp:BoundField DataField="FechaDeseadaInicial" HeaderText="Inicio de Permiso" />
                        <asp:BoundField DataField="FechaDeseadaFinal" HeaderText="Final de Permiso" />
                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                    </Columns>
                </asp:GridView>
            </div>
        </section>
    </main>
</asp:Content>