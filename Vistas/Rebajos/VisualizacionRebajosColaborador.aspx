<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualizacionRebajosColaborador.aspx.cs" Inherits="ControlEmpresarial.Vistas.Rebajos.VisualizacionRebajosColaborador" MasterPageFile="~/Vistas/Site1.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        /* Estilos para el GridView */
        .grid-view {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }

        .grid-view th {
            background-color: #f2f2f2;
            color: #333;
            text-align: left;
            padding: 10px;
            font-weight: bold;
        }

        .grid-view td {
            padding: 10px;
            border-bottom: 1px solid #ddd;
            color: #333;
        }

        .grid-view tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        .grid-view tr:hover {
            background-color: #f1f1f1;
        }

        .button {
            background-color: #4CAF50;
            color: white;
            border: none;
            padding: 8px 16px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 14px;
            cursor: pointer;
            border-radius: 4px;
        }

        .button:hover {
            background-color: #45a049;
        }
    </style>

    <main>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario" style="max-width: 900px; margin: 0 auto;">
                <h2><span class="fuente-delgada">Historial de</span><br/><span class="fuente-gruesa">Rebajos</span></h2>
                <p>Todos los rebajos a dia de hoy</p>
                <br />
                <br />
                <asp:GridView ID="gvRebajos" runat="server" AutoGenerateColumns="False" CssClass="grid-view">
                    <Columns>
                        <asp:BoundField DataField="idRebajo" HeaderText="Rebajo" />
                        <asp:BoundField DataField="idEmpleado" HeaderText="Empleado" />
                        <asp:BoundField DataField="TipoRebajoID" HeaderText="Tipo de Rebajo ID" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                        <asp:BoundField DataField="Monto" HeaderText="Monto" />
                    </Columns>
                </asp:GridView>
            </div>
        </section>
        <section class="seccion-imagen">
            <img src="../../Imagenes/colaborador.png" alt="Image of office">
        </section>
    </main>
</asp:Content>