﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InconsistenciasSupervisor.aspx.cs" Inherits="ControlEmpresarial.Vistas.Inconsistencias.InconsistenciasSupervisor" MasterPageFile="~/Vistas/Site3.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <section class="seccion-formulario">
            <div class="tarjeta-formulario" style="max-width: 1000px; margin: 0 auto;">
                <h2>
                    <span class="fuente-delgada">Inconsistencias</span><br />
                </h2>

                <div class="table-container">
                    <asp:GridView ID="gvJustificaciones" runat="server" AutoGenerateColumns="False" CssClass="tabla-inconsistencias">
                        <Columns>
                            <asp:BoundField DataField="idJustificacion" HeaderText="ID Justificación" />
                            <asp:BoundField DataField="NombreEmpleado" HeaderText="Nombre Empleado" />
                            <asp:BoundField DataField="NombreInconsistencia" HeaderText="Nombre Inconsistencia" />
                            <asp:BoundField DataField="Justificacion" HeaderText="Justificación" />
                            <asp:BoundField DataField="FechaJustificacion" HeaderText="Fecha Justificación" DataFormatString="{0:yyyy-MM-dd}" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
            </div>

        </section>
    </main>

    <div class="divisor-forma-personalizado">
        <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
            <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
        </svg>
    </div>
</asp:Content>
