<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgregarHorario.aspx.cs" Inherits="ControlEmpresarial.Vistas.Colaborador.AgregarHorario" MasterPageFile="~/Vistas/Site3.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <section class="seccion-formulario">
    <div class="tarjeta-formulario-login" style="max-width: 700px; margin: 0 auto;">
        <h2><span class="fuente-gruesa">Crea un</span><br /><span class="fuente-delgada">Horario</span></h2>
        <br />
        <div>
            <label class="fuente-delgada" for="ddlJornada">Jornada</label>
            <asp:DropDownList ID="ddlTipoJornada" runat="server">
                <asp:ListItem Text="Diurna" Value="Diurna" />
                <asp:ListItem Text="Nocturna" Value="Nocturna" />
                <asp:ListItem Text="Mixta" Value="Mixta" />
            </asp:DropDownList>
        </div>
        <br />
        <div>
            <label class="fuente-delgada" for="ddlDiaSemana1">Día de la semana (Inicio)</label>
            <asp:DropDownList ID="ddlDiaSemana1" runat="server">
                <asp:ListItem Text="Lunes" Value="Lunes" />
                <asp:ListItem Text="Martes" Value="Martes" />
                 <asp:ListItem Text="Miercoles" Value="Miercoles" />
                 <asp:ListItem Text="Jueves" Value="Jueves" />
                 <asp:ListItem Text="Viernes" Value="Viernes" />
                 <asp:ListItem Text="Sabado" Value="Sabado" />
            </asp:DropDownList>
        </div>
        <br />
        <div>
            <label class="fuente-delgada" for="ddlDiaSemana2">Día de la semana (Fin)</label>
            <asp:DropDownList ID="ddlDiaSemana2" runat="server">
            <asp:ListItem Text="Lunes" Value="Lunes" />
            <asp:ListItem Text="Martes" Value="Martes" />
             <asp:ListItem Text="Miercoles" Value="Miercoles" />
             <asp:ListItem Text="Jueves" Value="Jueves" />
             <asp:ListItem Text="Viernes" Value="Viernes" />
             <asp:ListItem Text="Sabado" Value="Sabado" />
        </asp:DropDownList>
        </div>
        <br />
        <div>
    <label class="fuente-delgada" for="txtHoraEntrada">Hora de Entrada:</label>
    <asp:TextBox ID="txtHoraEntrada" runat="server" CssClass="inicioSesion" oninput="validarHoraMilitar(this)"></asp:TextBox>
</div>
        <br />
        <div>
    <label class="fuente-delgada" for="txtHoraSalida">Hora de Salida:</label>
    <asp:TextBox ID="txtHoraSalida" runat="server" CssClass="inicioSesion" oninput="validarHoraMilitar(this)"></asp:TextBox>
</div>
        <br />
        <asp:Button ID="ingresar" runat="server" Text="Ingresar" CssClass="button" OnClick="ingresar_Click" />
        <br />
        <asp:Button ID="volverMenu" runat="server" Text="Volver al menú" CssClass="button-blanco" OnClick="volverMenu_Click" />
    </div>
</section>
         <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
               <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
         </div>
       <script type="text/javascript">
           function validarHoraMilitar(event) {
               var keyCode = event.keyCode;
               var input = event.target;
               var valor = input.value + String.fromCharCode(keyCode);

               // Permitir solo números y dos puntos (:)
               if (!/^([01]?[0-9]|2[0-3]):[0-5][0-9]$/.test(valor)) {
                   event.preventDefault(); // Evitar la entrada del carácter
               }
           }
       </script>
    </asp:Content>