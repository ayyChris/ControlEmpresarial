﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TiposIncapacidades.aspx.cs" Inherits="ControlEmpresarial.Vistas.Incapacidades.AgregarTipoIncapacidad" MasterPageFile="~/Vistas/Site3.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <section class="seccion-formulario">
               <div class="tarjeta-formulario-login" style="max-width: 700px; margin: 0 auto;">
                  <h2><span class="fuente-gruesa">Crea un</span><br><span class="fuente-delgada">tipo de incapacidad</span></h2>
                  <p></p>
                  <br />
                  <div>
                     <label class="fuente-delgada" for="cedula">Incapacidades</label>
                     <asp:TextBox CssClass="inicioSesion" ID="tipoincapacidad" runat="server" placeholder="Ej.Discapacidad"></asp:TextBox>
                  </div>
                  <br />
                   <div>
                   <label class="fuente-delgada" for="cedula">Descripcion</label>
                   <asp:TextBox CssClass="inicioSesion" ID="descripcion" runat="server" placeholder="Ej.Descripcion"></asp:TextBox>
                </div>
                <br />
                  <br />
                  <br />
                  <asp:Button ID="ingresar" runat="server" Text="Ingresar" CssClass="button" OnClick="ingresar_Click" />
                  <br />
                  <asp:Button ID="volverMenu" runat="server" Text="Volver al menú" CssClass="button-blanco" OnClick="volverMenu_Click" />

                   <div class="table-container">
                <asp:GridView ID="gridIncapacidades" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Tipo de Incapacidad" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
            </section>
         <div class="divisor-forma-personalizado">
            <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
               <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
            </svg>
         </div>
</asp:Content>