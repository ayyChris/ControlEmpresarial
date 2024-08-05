<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuSupervisor.aspx.cs" Inherits="ControlEmpresarial.Vistas.MenuSupervisor" MasterPageFile="~/Vistas/Site3.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <div class="container-menu">
            <div class="header-menu">
               <h1>Revisa nuestras opciones de gestión para los supervisores</h1>
               <p>ActivitySync ofrecemos distintas opciones para la gestión de nuestros empleados con la intención de mantener un sistema autogestionable.</p>
            </div>
            <div class="options-menu">
            <div class="option-menu">
                <a href="#"><img src="../../Imagenes/icono-escritura.png" alt="Permisos" /></a>
                <h3 class="h3-footer">Rebajos</h3>
                <p>Revise el historial de rebajos que se han registrado.</p>
            </div>
            <div class="option-menu">
                <a href="#"><img src="../../Imagenes/icono-vacaciones.png" alt="Control de actividades" /></a>
                <h3 class="h3-footer">Vacaciones Colectivas</h3>
                <p>Registre las vacaciones colectivas para los colaboradores y la jefatura.</p>
            </div>
            <div class="option-menu full-width">
                <a href="../Colaborador/VisualizacionColaboradorSupervisor.aspx"><img src="../../Imagenes/icono-colaborador.png" alt="Colaboradores" /></a>
                <h3 class="h3-footer">Colaboradores</h3>
                <p>Vea los colaboradores de los departamentos.</p>
                </div>
            </div>
         </div>
</asp:Content>