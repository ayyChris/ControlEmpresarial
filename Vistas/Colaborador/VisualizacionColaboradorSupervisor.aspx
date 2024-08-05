<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualizacionColaboradorSupervisor.aspx.cs" Inherits="ControlEmpresarial.Vistas.VisualizacionColaboradorSupervisor" MasterPageFile="~/Vistas/Site3.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
     <style>
        .seccion-imagen {
    flex: 1;
    display: flex;
    justify-content: center;
    align-items: center;
    position: relative;
    z-index: -2;
}

.seccion-imagen img {
    position: relative;
    max-width: 70%; /* Reducido el tamaño máximo de la imagen */
    height: auto; /* Ajuste automático de la altura según el ancho */
    z-index:-3;
}
    </style>
    <main>
            <section class="seccion-imagen">
               <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra">
            </section>
            <section class="seccion-formulario">
               <div class="tarjeta-formulario">
                  <h2>Departamento</h2>
                  <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged"></asp:DropDownList>
               </div>
               <div class="table-container">
                  <asp:GridView ID="gridEmpleados" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
                        <asp:BoundField DataField="Cedula" HeaderText="Cedula" />
                        <asp:BoundField DataField="NombrePuesto" HeaderText="Puesto" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    </Columns>
                </asp:GridView>

               </div>
            </section>
         </main>
                 <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
               <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
         </div>
</asp:Content>