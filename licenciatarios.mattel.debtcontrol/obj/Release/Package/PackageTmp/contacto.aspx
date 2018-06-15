<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contacto.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.contacto" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="style/style.css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Telerik" runat="server"></telerik:RadWindowManager>
    <div class="box-titulo">
      <asp:Label ID="lbltitulo" runat="server" Text="CONTACTENOS"></asp:Label>
    </div>
    <div class="box-content-form">
      <div class="bx-lbl-title">
        <asp:Label ID="lblNombre" runat="server" Text="Nombres:"></asp:Label>
      </div>
      <div class="bx-lbl-title">
        <asp:TextBox ID="txtnombres" runat="server" CssClass="styleTxt"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtnombres" Display="Dynamic" ValidationGroup="ValidPage"></asp:RequiredFieldValidator>
      </div>
      <div class="bx-lbl-title">
        <asp:Label ID="Label1" runat="server" Text="Celular:"></asp:Label>
      </div>
      <div class="bx-lbl-title">
        <asp:TextBox ID="txtcelular" runat="server" CssClass="styleTxt"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtcelular" Display="Dynamic" ValidationGroup="ValidPage"></asp:RequiredFieldValidator>
      </div>
      <div class="bx-lbl-title">
        <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
      </div>
      <div class="bx-lbl-title">
        <asp:TextBox ID="txtemail" runat="server" CssClass="styleTxt"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtemail" Display="Dynamic" ValidationGroup="ValidPage"></asp:RequiredFieldValidator>
        <asp:CustomValidator ID="valtxtEmailVal" runat="server" ErrorMessage="*" ControlToValidate="txtemail" Display="Dynamic" ValidationGroup="ValidPage" OnServerValidate="valtxtEmailVal_ServerValidate"></asp:CustomValidator>
      </div>
      <div class="bx-lbl-title">
        <asp:Label ID="lblComentarios" runat="server" Text="Comentarios:"></asp:Label>
      </div>
      <div class="bx-lbl-title">
        <asp:TextBox ID="txtcomentarios" runat="server" Rows="7" Columns="70" TextMode="MultiLine" CssClass="styleTxtObs"></asp:TextBox>
      </div>
      <div class="bx-lbl-btn">
        <asp:Button ID="BtngEnviar" CssClass="btnGuardar" runat="server" Text="Enviar" OnClick="BtngEnviar_Click" ValidationGroup="ValidPage" />
      </div>
    </div>
    <div class="box-pie-contactos">
      <div class="box-img-foot">
        <img src="images/logo-foot.png" />
      </div>
      <div class="bx-lbl-foot">
        <asp:Label ID="Label2" runat="server" Text="Teléfono: +562 25996100 | Email: licenciatariosmattel@debtcontrol.cl"></asp:Label>
      </div>
    </div>
  </form>
</body>
</html>
