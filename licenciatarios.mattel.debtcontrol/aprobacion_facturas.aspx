<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="aprobacion_facturas.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.aprobacion_facturas" %>

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
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Telerik" runat="server"></telerik:RadWindowManager>
    <div class="box-titulo">
      <asp:Label ID="lbltitulo" runat="server" Text="APROBACION DE FACTURAS Q"></asp:Label>
    </div>
    <div class="box-content-form">
      <div class="bx-lbl-title">
        <asp:Label ID="lblcontrato" runat="server" Text="Contrato: "></asp:Label>
        <asp:Label ID="lblNoContrato" runat="server" Text=""></asp:Label>
      </div>
      <div class="bx-lbl-data" id="idGrilla" runat="server" visible="false">
      </div>
      <div class="bx-lbl-btn" id="idBtnSave" runat="server" visible="false">
        <asp:Button ID="btnAprobar" CssClass="btnGuardar" runat="server" Text="Aprobar" OnClick="btnAprobar_Click" />
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
    <asp:HiddenField ID="hdd_num_contrato" runat="server" />
    <asp:HiddenField ID="hdd_no_contrato" runat="server" />
    <asp:HiddenField ID="hdd_periodo" runat="server" />
    <asp:HiddenField ID="hdd_ano_periodo" runat="server" />
    <script type="text/javascript">
      function onRequestStart(sender, args) {
        if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0) {
          args.set_enableAjax(false);
        }
      }

      function doNothing() {
        var obj = document.getElementById("tipo_cambio")
        obj.click();
        return false;
        alert('paso');
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
