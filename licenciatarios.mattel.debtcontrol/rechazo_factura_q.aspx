<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rechazo_factura_q.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.rechazo_factura_q" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="style/style.css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <div id="bx_msRechazo" runat="server">
      <div class="box-titulo">
        <asp:Label ID="lbltitulo" runat="server" Text="RECHAZO FACTURA"></asp:Label>
      </div>
      <div class="box-content-form">
        <div class="bx-lbl-title">
          <asp:Label ID="lblingresocomprobante" runat="server" Text="Ingresar rechazo"></asp:Label>
        </div>
        <div class="bx-lbl-data">
          <asp:TextBox ID="txtcomentarios" runat="server" Rows="7" Columns="70" TextMode="MultiLine" CssClass="styleTxtObs"></asp:TextBox>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator" runat="server" ErrorMessage="*" ControlToValidate="txtcomentarios" Display="Dynamic" ValidationGroup="ValidPage"></asp:RequiredFieldValidator>
        </div>
        <div class="bx-lbl-btn">
          <asp:Button ID="BtngEnviar" CssClass="btnGuardar" runat="server" Text="Enviar" OnClick="BtngEnviar_Click" ValidationGroup="ValidPage" />
        </div>
      </div>
      <br />
      <br />
    </div>
    <div id="bx_msRealizado" runat="server" visible="false">
      <div class="box-content-form">
        <div class="box-titulo">
          <asp:Label ID="lblMsnEnv" runat="server" Text="MENSAJE DE RECHAZO ENVIADO"></asp:Label>
        </div>
      </div>
    </div>
    <asp:HiddenField ID="numcontrato" runat="server" />
    <asp:HiddenField ID="periodo" runat="server" />
    <asp:HiddenField ID="ano_reporte" runat="server" />
  </form>
</body>
</html>
