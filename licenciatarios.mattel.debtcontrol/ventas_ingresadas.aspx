<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ventas_ingresadas.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.ventas_ingresadas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="style/style.css" rel="stylesheet" />
  <script type="text/javascript" src="/Resources/lib/jquery-1.10.1.min.js"></script>
  <script type="text/javascript" src="/Resources/lib/jquery.mousewheel-3.0.6.pack.js"></script>
  <script type="text/javascript" src="/Resources/source/jquery.fancybox.js?v=2.1.5"></script>

  <link rel="stylesheet" href="/bootstrap/css/bootstrap.min.css" />
  <script src="/bootstrap/js/bootstrap.min.js"></script>
  <link rel="stylesheet" type="text/css" href="/Resources/source/jquery.fancybox.css?v=2.1.5" media="screen" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
      <div class="col-md-4">
        <div class="stepconteiner">
          <div class="stepNOactive"><span class="numberNOactive">1</span></div>
          <div class="box-stepNOactive">
            <asp:Label ID="Label1" runat="server" Text="INGRESO DE VENTAS"></asp:Label>
            <span class="glyphicon glyphicon-share-alt"></span>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="stepconteiner">
          <div class="stepNOactive"><span class="numberNOactive">2</span></div>
          <div class="box-stepNOactive">
            <asp:Label ID="Label5" runat="server" Text="CONFIRMACIÓN INGRESO DE VENTAS"></asp:Label>
            <span class="glyphicon glyphicon-share-alt"></span>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="stepconteiner">
          <div class="stepExito"><span class="numberExito">3</span></div>
          <div class="box-stepExito">
            <asp:Label ID="Label6" runat="server" Text="VENTAS DECLARADAS CORRECTAMENTE"></asp:Label>
            <span class="glyphicon glyphicon-thumbs-up"></span>
          </div>
        </div>
      </div>
    </div>
    <div><br /></div>
    <div class="box-content-form">
      <div class="alert alert-success">
        <strong>ATENCION!!</strong> Ha declarado correctamente las ventas para el periodo <asp:Label ID="lblperiodo" runat="server"></asp:Label>
      </div>
    </div>
    <div class="box-pie">
      <asp:Button ID="btnInstructivo" runat="server" CssClass="btnPie" Text="Instructivo" OnClick="btnInstructivo_Click" />
      <asp:Button ID="btnContacto" runat="server" CssClass="btnPie" Text="Contacto" OnClick="btnContacto_Click" />
      <asp:Button ID="btnTCambio" runat="server" CssClass="btnPie" Text="Tipo Cambio" OnClientClick="doNothing();" />
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
