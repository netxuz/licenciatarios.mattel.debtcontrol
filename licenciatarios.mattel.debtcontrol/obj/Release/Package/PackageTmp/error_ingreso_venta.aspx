<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error_ingreso_venta.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.error_ingreso_venta" %>

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
  <link rel="stylesheet" href="/bootstrap/css/bootstrap.min.css" />
  <script src="/bootstrap/js/bootstrap.min.js"></script>
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
    <div class="row">
      <div class="col-md-4">
        <div class="stepconteiner">
          <div class="stepNOactive"><span class="numberNOactive">1</span></div>
          <div class="box-stepNOactive">
            <asp:Label ID="Label3" runat="server" Text="INGRESO DE VENTAS"></asp:Label>
            <span class="glyphicon glyphicon-share-alt"></span>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="stepconteiner">
          <div class="stepError"><span class="numberError">2</span></div>
          <div class="box-stepError">
            <asp:Label ID="Label5" runat="server" Text="ERROR EN INGRESO DE VENTAS"></asp:Label>
            <span class="glyphicon glyphicon-thumbs-down"></span>
          </div>
        </div>
      </div>
      <div class="col-md-4">
      </div>
    </div>
    <div><br /></div>
    <div class="box-content-form">
      <div id="divAlert" class="alert alert-warning">
        <asp:Label ID="lblerror0" runat="server" Text=""></asp:Label><br />
        <br />
        <asp:Label ID="lblerror1" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblerror2" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Por favor, valide lo indicado anteriormente, modifique el archivo y vuelva a cargarlo ingresando al módulo de Ingreso de Ventas"></asp:Label>
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
