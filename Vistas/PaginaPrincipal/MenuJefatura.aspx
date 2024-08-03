<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuJefatura.aspx.cs" Inherits="ControlEmpresarial.Vistas.MenuJefatura" MasterPageFile="~/Vistas/Site2.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-menu">
            <div class="header-menu">
                <h1>Revisa nuestras opciones de gestión para la jefatura</h1>
                <p>ActivitySync ofrecemos distintas opciones para la gestión de nuestros empleados con la intención de mantener un sistema autogestionable.</p>
            </div>
            <div class="options-menu">
                <div class="option-menu">
                    <a href="../Permisos/TablaPreVisualizacionPermisosJefe.aspx"><img src="../../Imagenes/icono-permisos.png" alt="Permisos" /></a>
                    <h3>Permisos</h3>
                    <p>Revise los permisos que ha solicitado los empleados.</p>
                </div>
                <div class="option-menu">
                    <a href="../Incapacidades/SolicitarIncapacidadesJefatura.aspx"><img src="../../Imagenes/icono-escritura.png" alt="incapacidades" /></a>
                    <h3>Incapacidades</h3>
                    <p>Registre las incapacidades del empleado.</p>
                </div>
                <div class="option-menu">
                    <a href="../Inconsistencias/VisualizacionInconsistencias.aspx"><img src="../../Imagenes/icono-inconsistencias.png" alt="inconsistencias" /></a>
                    <h3>Inconsistencias</h3>
                    <p>Puede revisar las inconsistencias de cada empleado.</p>
                </div>
                <div class="option-menu">
                    <a href="../Control de Actividades/TablaPreAcepctacionJefatura.aspx"><img src="../../Imagenes/icono-tiempo.png" alt="Horas Extras" /></a>
                    <h3>Actividades</h3>
                    <p>Puede ver las actividades de su departamento.</p>
                </div>
                <div class="option-menu">
                    <a href="#"><img src="../../Imagenes/icono-reposiciones.png" alt="Reposiciones" /></a>
                    <h3>Reposiciones</h3>
                    <p>Puede verificar las reposiciones hechas por el empleado.</p>
                </div>
                <div class="option-menu">
                    <a href="../Horas Extra/PreAceptacionHorasExtra.aspx"><img src="../../Imagenes/icono-horasExtras.png" alt="Horas Extras" /></a>
                    <h3>Horas Extras</h3>
                    <p>Puede asignar horas extras para un cierto colaborador.</p>
                </div>
                <div class="option-menu">
                    <a href="../Vacaciones/solicitudVacacionesJefatura.aspx"><img src="../../Imagenes/icono-vacaciones.png" alt="Vacaciones" /></a>
                    <h3>Vacaciones</h3>
                    <p>Puede validar las vacaciones solicitadas por un colaborador.</p>
                </div>
                <div class="option-menu">
                    <a href="../Marcas/marcas.aspx"><img src="../../Imagenes/icono-marcas.png" alt="Marcas" /></a>
                    <h3>Marcas</h3>
                    <p>Registre su hora de entrada y de salida de cada día laboral respectivo bajo su horario.</p>
                </div>
                <div class="option-menu">
                    <a href="../Colaborador/agregarColaboradorJefe.aspx"><img src="../../Imagenes/icono-colaborador.png" alt="Colaboradores" /></a>
                    <h3>Colaboradores</h3>
                    <p>Registre un colaborador para su departamento.</p>
                </div>
            </div>
        </div>
</asp:Content>