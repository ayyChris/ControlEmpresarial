﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreAceptarDenegarInconsistenciasJefe.aspx.cs" Inherits="ControlEmpresarial.Vistas.Inconsistencias.AceptarDenegarInconsistenciasJefeç" MasterPageFile="~/Vistas/Site2.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="seccion-imagen">
            <img src="../../Imagenes/jefe.png" alt="Ilustración de una persona trabajando horas extra" />
        </section>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario">
                <h2>Inconsistencias</h2>
            </div>
            <div class="table-container">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" EnableViewState="true" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="idJustificacion" HeaderText="ID Justificación" />
                        <asp:BoundField DataField="Justificacion" HeaderText="Justificación" />
                        <asp:BoundField DataField="FechaJustificacion" HeaderText="Fecha Justificación" DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="EstadoJustificacion" HeaderText="Estado Justificación" />
                        <asp:BoundField DataField="NombreEmpleado" HeaderText="Empleado" />
                        <asp:BoundField DataField="TipoInconsistencia" HeaderText="Tipo de Inconsistencia" />
                        <asp:TemplateField HeaderText="Acción">
                            <ItemTemplate>
                                <asp:Button ID="btnAccion" CssClass="button-grid" runat="server" Text="Acción" CommandName="Accion" CommandArgument='<%# Eval("idJustificacion") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:Label ID="Label2" runat="server" EnableViewState="true" Visible="false"></asp:Label>
            </div>


            <asp:Label ID="Label1" runat="server" EnableViewState="true" Visible="false"></asp:Label>
            </div>
        </section>
    </main>
    <div class="divisor-forma-personalizado">
        <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
            <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
        </svg>
    </div>
</asp:Content>
