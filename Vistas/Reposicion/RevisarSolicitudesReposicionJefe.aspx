<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RevisarSolicitudesReposicionJefe.aspx.cs" Inherits="ControlEmpresarial.Vistas.Reposicion.RevisarSolicitudesReposicionJefe" MasterPageFile="~/Vistas/Site2.master" %>

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
            <div class="tarjeta-formulario" style="max-width: 1300px; margin: 0 auto;">
                <h2><span class="fuente-delgada">Revision de</span><br/><span class="fuente-gruesa">Reposiciones</span></h2>
                <p>Todos los registros de reposiciones para revisar</p>
                <br />
                <br />
                <asp:GridView ID="gvReposiciones" runat="server" AutoGenerateColumns="False" CssClass="grid-view" OnRowCommand="gvReposiciones_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="idEvidencia" HeaderText="Evidencia" />
                        <asp:BoundField DataField="idReposicion" HeaderText="Reposición" />
                        <asp:BoundField DataField="idInconsistencia" HeaderText="Inconsistencia" />
                        <asp:BoundField DataField="EnlaceEvidencia" HeaderText="Evidencia" />
                        <asp:BoundField DataField="FechaEvidencia" HeaderText="Fecha de Evidencia" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:ButtonField ControlStyle-CssClass="button" ButtonType="Button" CommandName="Revisar" Text="Revisar Reposicion" />
                    </Columns>
                </asp:GridView>
            </div>
        </section>
        <section class="seccion-imagen">
            <img src="../../Imagenes/colaborador.png" alt="Imagen del colaborador" />
        </section>
    </main>
</asp:Content>