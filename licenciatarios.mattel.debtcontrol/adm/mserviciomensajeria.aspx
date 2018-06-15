<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mserviciomensajeria.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.adm.mserviciomensajeria" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="/style/admstyle.css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
    <div id="MasterPage">
      <div class="BloqueMenu">
        <div class="lblTitulo">
          <asp:Label ID="lblTitulo" runat="server" Text="CONFIGURACION DE SERVICIO DE MENSAJES"></asp:Label>
        </div>
        <div class="Botonera">
          <asp:Button ID="btnSave" runat="server" Text="Grabar" OnClick="btnSave_Click" />
        </div>
      </div>
    </div>
    <div class="modulo">
      <div class="bloque">
        <div>
          <asp:Label ID="lbl1" runat="server" CssClass="lblTitle" Text="Configuración Recordatario a Licenciatario para ingresar ventas"></asp:Label>
        </div>
        <div>
          <asp:Label ID="lbl3" runat="server" Text="Día tope de ingreso de reporte de venta"></asp:Label>
          <asp:TextBox ID="txt_dia_tope_ingventa1" runat="server" CssClass="styleTxtMan" Width="20px"></asp:TextBox>
        </div>
        <div>
          <asp:Label ID="Label1" runat="server" Text="Cantidad de días antes a la fecha tope, para envíar mensaje"></asp:Label>
          <asp:TextBox ID="txt_cant_ant_tope_ingventa" runat="server" CssClass="styleTxtMan" Width="20px"></asp:TextBox>
        </div>
      </div>
      <div><hr/></div>
      <div><br /></div>
      <div class="bloque">
        <div>
          <asp:Label ID="lbl4" runat="server" CssClass="lblTitle" Text="Configuración notificación a Licenciatario del no ingreso de reporte de ventas"></asp:Label>
        </div>
        <div>
          <asp:Label ID="lbl5" runat="server" Text="Día tope de ingreso de reporte de venta"></asp:Label>
          <asp:TextBox ID="txt_dia_tope_ingventa2" runat="server" CssClass="styleTxtMan" Width="20px"></asp:TextBox>
        </div>
        <div>
          <asp:Label ID="lbl6" runat="server" Text="Cantidad de días después a la fecha tope, para envíar mensaje de notificación."></asp:Label>
          <asp:TextBox ID="txt_cant_desp_tope_ingventa" runat="server" CssClass="styleTxtMan" Width="20px"></asp:TextBox>
        </div>
      </div>
      <div><hr/></div>
      <div><br /></div>
      <div class="bloque">
        <div>
          <asp:Label ID="lbl7" runat="server" CssClass="lblTitle" Text="Configuración recordatario a Licenciatario para subir comprobante de retención SII"></asp:Label>
        </div>
        <div>
          <asp:Label ID="lbl8" runat="server" Text="Cantidad de días después del cierre del Q"></asp:Label>
          <asp:TextBox ID="txt_cant_desp_q" runat="server" CssClass="styleTxtMan" Width="20px"></asp:TextBox>
        </div>
      </div>
      <div><hr/></div>
      <div><br /></div>
      <div class="bloque">
        <div>
          <asp:Label ID="lbl9" runat="server" CssClass="lblTitle" Text="Listado de Licenciatarios a Ejecutivo"></asp:Label>
        </div>
        <div>
          <asp:Label ID="lbl10" runat="server" Text="Día tope de ingreso de reporte de venta"></asp:Label>
          <asp:TextBox ID="txt_dia_tope_ingventa3" runat="server" CssClass="styleTxtMan" Width="20px"></asp:TextBox>
        </div>
        <div>
          <asp:Label ID="lbl11" runat="server" Text="Cantidad de días después a la fecha tope, para envíar mensaje a ejecutivo con el listado."></asp:Label>
          <asp:TextBox ID="txt_cant_desp_tope_ingventa2" runat="server" CssClass="styleTxtMan" Width="20px"></asp:TextBox>
        </div>
      </div>
      <div><hr/></div>
      <div><br /></div>
      <div class="bloque">
        <div>
          <asp:Label ID="Label2" runat="server" CssClass="lblTitle" Text="Horario de ejecucion de creación de facturas."></asp:Label>
        </div>
        <div>
          <asp:Label ID="Label3" runat="server" Text="Horario"></asp:Label>
          <asp:TextBox ID="txt_horario_factura" runat="server" CssClass="styleTxtMan" Width="20px"></asp:TextBox>
        </div>
      </div>
      <div><hr/></div>
      <div><br /></div>
      <div class="bloque">
        <div>
          <asp:Label ID="Label6" runat="server" CssClass="lblTitle" Text="Horario de ejecucion de envío de correo a licenciatarios por facturas generadas."></asp:Label>
        </div>
        <div>
          <asp:Label ID="Label7" runat="server" Text="Horario"></asp:Label>
          <asp:TextBox ID="txt_horario_fact_generadas" runat="server" CssClass="styleTxtMan" Width="20px"></asp:TextBox>
        </div>
      </div>
      <div><hr/></div>
      <div><br /></div>
      <div class="bloque">
        <div>
          <asp:Label ID="Label4" runat="server" CssClass="lblTitle" Text="Alertas de facturas advance y de Q pendientes de aprobar."></asp:Label>
        </div>
        <div>
          <asp:Label ID="Label5" runat="server" Text="Cantidad de días de espera para enviar alerta"></asp:Label>
          <asp:TextBox ID="txt_dias_espera" runat="server" CssClass="styleTxtMan" Width="20px"></asp:TextBox>
        </div>
      </div>
      <div><hr/></div>
      <div><br /></div>
      <div class="bloque">
        <div>
          <asp:Label ID="Label8" runat="server" CssClass="lblTitle" Text="Cierre de Q."></asp:Label>
        </div>
        <div>
          <asp:Label ID="Label9" runat="server" Text="Cantidad de horas para cerrar Q."></asp:Label>
          <asp:TextBox ID="txt_cant_hrs_cierre" runat="server" CssClass="styleTxtMan" Width="20px"></asp:TextBox>
        </div>
        <div>
          <asp:Label ID="Label10" runat="server" Text="Horario de ejecución para cerrar"></asp:Label>
          <asp:TextBox ID="txt_horario_cierre" runat="server" CssClass="styleTxtMan" Width="20px"></asp:TextBox>
        </div>
      </div>
    </div>
  </form>
</body>
</html>
