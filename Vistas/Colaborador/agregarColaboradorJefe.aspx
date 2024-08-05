<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="agregarColaboradorJefe.aspx.cs" Inherits="ControlEmpresarial.Vistas.Colaborador.agregarColaboradorJefe" MasterPageFile="~/Vistas/Site2.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style>
    .contenedor-principal {
        display: flex;
        align-items: flex-start;
        justify-content: space-between;
        gap: 20px;
        margin-top: 50px;
        padding: 20px; /* Añadido padding para el contenedor principal */
    }

    .tarjeta-formulario {
        background: #fff;
        padding: 40px; /* Aumentado el padding para el formulario */
        border-radius: 8px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        z-index: 2;
        position: relative;
        flex: 1; /* Permite que el formulario tome el espacio disponible */
        max-width: 50%; /* Ajusta el tamaño máximo del formulario */
        margin-left:50px;
    }

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
    
<div class="contenedor-principal">
               <div class="tarjeta-formulario">
                  <h2><span class="fuente-delgada">Agregar nuevo</span><br><span class="fuente-gruesa">Colaborador</span></h2>
                  <p>Ingresa los datos del nuevo colaborador.</p>
                  <br />
                  <br />
                  <div class="entradas-horario">
                     <div>
                        <label class="fuente-morada" for="hora-inicio">Nombre</label>
                        <asp:TextBox ID="nombreUsuario" runat="server" placeholder=""></asp:TextBox>
                     </div>
                     <div>
                        <label class="fuente-morada" for="hora-final">Apellidos</label>
                        <asp:TextBox ID="apellidosUsuario" runat="server" placeholder=""></asp:TextBox>
                     </div>
                  </div>
                  <div class="entradas-horario">
                     <div>
                        <label class="fuente-morada" for="hora-inicio">Cedula</label>
                        <asp:TextBox ID="cedulaUsuario" runat="server" placeholder=""></asp:TextBox>
                     </div>
                    <div>
                   <label class="fuente-morada" for="hora-inicio">Correo</label>
                   <asp:TextBox ID="correoUsuario" runat="server" placeholder=""></asp:TextBox>
                </div>
                     <div>
                        <label class="fuente-morada" for="hora-final">Contraseña</label>
                        <asp:TextBox ID="passwordUsuario" runat="server" placeholder=""></asp:TextBox>
                     </div>
                  </div>
                  <label class="fuente-morada" for="colaboradorACargo">Puesto</label>
                  <asp:DropDownList ID="puesto" runat="server">
                     <asp:ListItem Text="Seleccione" Value="" />
                  </asp:DropDownList>
                   <div>
            <label class="fuente-morada" for="horario">Tipo de Jornada</label>
            <asp:DropDownList ID="horario" runat="server">
                <asp:ListItem Text="Seleccione" Value="" />
            </asp:DropDownList>
        </div>
                  <asp:Button ID="submit" runat="server" Text="Enviar" CssClass="button" OnClick="submit_Click"/>
               </div>
        <div class="seccion-imagen">
    <img src="../../Imagenes/Colaborador.png" alt="Ilustración de una persona trabajando horas extra">
</div>
    </div>
         <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
               <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
         </div>
</asp:Content>