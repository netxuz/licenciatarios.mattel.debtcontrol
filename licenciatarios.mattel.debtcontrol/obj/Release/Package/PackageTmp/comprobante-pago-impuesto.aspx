<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="comprobante-pago-impuesto.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.comprobante_pago_impuesto" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="style/style.css" rel="stylesheet" />
  <script type="text/javascript" src="/Resources/lib/jquery-1.10.1.min.js"></script>
  <script type="text/javascript" src="/Resources/lib/jquery.mousewheel-3.0.6.pack.js"></script>
  <script type="text/javascript" src="/Resources/source/jquery.fancybox.js?v=2.1.5"></script>
  <link rel="stylesheet" type="text/css" href="/Resources/source/jquery.fancybox.css?v=2.1.5" media="screen" />
</head>
<body class="body-form">
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Telerik" runat="server"></telerik:RadWindowManager>
    <div class="box-titulo">
      <asp:Label ID="lbltitulo" runat="server" Text="INGRESO COMPROBANTE PAGO RETENCIÓN DE IMPUESTO"></asp:Label>
    </div>
    <div class="box-content-form">
      <div class="bx-lbl-title">
        <asp:Label ID="lblingresocomprobante" runat="server" Text="Para validar su pago de retención de impuestos, por favor ingrese los datos del comprobante"></asp:Label>
      </div>
      <div class="bx-lbl-title">
        <asp:Label ID="lblcontrato" runat="server" Text="Contrato:"></asp:Label>
      </div>
      <div class="bx-lbl-data">
        <asp:DropDownList ID="cmbox_contrato" CssClass="box-select" runat="server" OnSelectedIndexChanged="cmbox_contrato_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>
      </div>
      <div class="bx-lbl-title">
        <asp:Label ID="lblmesventa" runat="server" Text="Periodo:"></asp:Label>
      </div>
      <div class="bx-lbl-data">
        <asp:DropDownList ID="ddlmesventa" CssClass="box-select" runat="server">
        </asp:DropDownList>
      </div>
      <div class="bx-lbl-title">
        <asp:Label ID="Label1" runat="server" Text="Comprobante:"></asp:Label>
      </div>
      <div class="bx-lbl-data">
        <telerik:RadUpload ID="RadUpload1" runat="server" MaxFileInputsCount="1" ControlObjectsVisibility="None"></telerik:RadUpload>
      </div>
      <div class="bx-lbl-title">
        <asp:CheckBox ID="chkbox_declaracion" Text="Declaro que el archivo adjunto corresponde al comprobante de pago de retención de impuesto." runat="server" />
        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Debe declarar que el archivo corresponde al pago de retención de impuesto." Text="" ClientValidationFunction="ValidateCheckBox" Display="Dynamic" ValidationGroup="ValidPage"></asp:CustomValidator>
      </div>
      <div class="bx-lbl-btn">
        <asp:Button ID="btnGuardar" CssClass="btnGuardar" runat="server" Text="Cargar Comprobante" OnClick="btnGuardar_Click" ValidationGroup="ValidPage" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="ValidPage" />
      </div>
    </div>
    <div class="box-pie">
      <asp:Button ID="btnInstructivo" runat="server" CssClass="btnPie" Text="Instructivo" OnClick="btnInstructivo_Click" />
      <asp:Button ID="btnContacto" runat="server" CssClass="btnPie" Text="Contacto" OnClick="btnContacto_Click" />
      <input type="button" class="btnPie" value="Tipo Cambio" onclick="doNothing();" />
      <a id="tipo_cambio" href="tipo_cambio.aspx"></a>
    </div>
    <div class="box-pie-contactos">
      <div class="box-img-foot">
        <img src="images/logo-foot.png" />
      </div>
      <div class="bx-lbl-foot">
        <asp:Label ID="Label2" runat="server" Text="Teléfono: +562 25996100 | Email: licenciatariosmattel@debtcontrol.cl"></asp:Label>
      </div>
    </div>
    <script type="text/javascript">
      function ValidateCheckBox(sender, args) {
        if (document.getElementById('<%=chkbox_declaracion.ClientID %>').checked == false) {
          args.IsValid = false;
        }
      }
      function doNothing() {
        var obj = document.getElementById("tipo_cambio")
        obj.click();
        return false;
      }

      $(document).ready(function () {
        $("a#tipo_cambio").fancybox({
          'autoDimensions': false,
          'width': 500,
          'height': 800,
          'transitionIn': 'elastic',
          'transitionOut': 'elastic',
          'type': 'iframe'
        });
      });

    </script>
  </form>
</body>
</html>
