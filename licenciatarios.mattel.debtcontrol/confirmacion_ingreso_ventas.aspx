<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="confirmacion_ingreso_ventas.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.confirmacion_ingreso_ventas" %>

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
          <div class="stepactive"><span class="numberactive">2</span></div>
          <div class="box-stepactive">
            <asp:Label ID="Label5" runat="server" Text="CONFIRMACIÓN INGRESO DE VENTAS"></asp:Label>
            <span class="glyphicon glyphicon-share-alt"></span>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="stepconteiner">
          <div class="stepNOactive"><span class="numberNOactive">3</span></div>
          <div class="box-stepNOactive">
            <asp:Label ID="Label6" runat="server" Text="VENTAS DECLARADAS CORRECTAMENTE"></asp:Label>
            <span class="glyphicon glyphicon-thumbs-up"></span>
          </div>
        </div>
      </div>
    </div>
    <div><br /></div>
    <div class="box-content-form">
      <div id="divAlert" runat="server" class="alert alert-warning">
        <asp:Label ID="lblAlerta" runat="server"></asp:Label>
      </div>
      <div class="bx-lbl-data">
        <asp:Label ID="lblcontrato" runat="server" Text=""></asp:Label>
      </div>
      <div class="bx-lbl-data">
        <asp:Label ID="lblmesventa" runat="server" Text=""></asp:Label>
      </div>
      <div class="bx-lbl-data">
        <telerik:RadGrid ID="rdDetalleVenta" runat="server" OnNeedDataSource="rdDetalleVenta_NeedDataSource"
          AllowSorting="true" ShowStatusBar="True" GridLines="None" PageSize="10" AllowPaging="True" Skin="Sitefinity">
          <PagerStyle Mode="NextPrevAndNumeric" />
          <MasterTableView DataKeyNames="codigo_detalle" AutoGenerateColumns="false"
            CommandItemDisplay="Top" ShowHeader="true" TableLayout="Fixed">
            <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" />
            <Columns>
              <telerik:GridBoundColumn DataField="marca" HeaderText="Marca"
                UniqueName="marca">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="categoria" HeaderText="Categoria"
                UniqueName="categoria">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="subcategoria" HeaderText="subcategoria"
                UniqueName="subcategoria">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="producto" HeaderText="producto"
                UniqueName="producto">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="nom_royalty" HeaderText="Desc.Royalty"
                UniqueName="descroyalty">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="royalty" HeaderText="Royalty"
                UniqueName="royalty">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="bdi" HeaderText="BDI"
                UniqueName="bdi">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente"
                UniqueName="cliente">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="sku" HeaderText="SKU"
                UniqueName="sku">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="descripcion_producto" HeaderText="Descripcion Producto"
                UniqueName="descripcion_producto">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="precio_uni_venta_bruta" HeaderText="Precio Unitario Venta Bruta"
                UniqueName="precio_uni_venta_bruta">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="cantidad_venta_bruta" HeaderText="Q Venta Bruta"
                UniqueName="cantidad_venta_bruta">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="precio_unit_descue_devol" HeaderText="Precio Unitario Descuento / Devolución"
                UniqueName="precio_unit_descue_devol">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="cantidad_descue_devol" HeaderText="Q Descuento / Devolución"
                UniqueName="cantidad_descue_devol">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>
            </Columns>
          </MasterTableView>
        </telerik:RadGrid>
      </div>
      <div class="bx-lbl-title">
        <asp:CheckBox ID="chkbox_declaracion" Text="Declaro que el archivo cargado corresponde a las ventas de los productos licenciados de Mattel para el periodo señalado."
          runat="server" />
        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Debe seleccionar si declarar o no el periodo." Text="" ClientValidationFunction="ValidateCheckBox" Display="Dynamic" ValidationGroup="ValidPage"></asp:CustomValidator>
      </div>

      <div class="bx-lbl-btn">
        <asp:Button ID="btnGuardar" CssClass="btnGuardar" runat="server" Text="Autorizar Periodo" OnClick="btnGuardar_Click" ValidationGroup="ValidPage" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="ValidPage" />
        <asp:Button ID="btnCancelar" CssClass="btnGuardar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
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
    <asp:HiddenField ID="hddCodReporteVenta" runat="server" />
    <asp:HiddenField ID="hddMesReporte" runat="server" />
    <asp:HiddenField ID="hddAnoReporte" runat="server" />
    <asp:HiddenField ID="hddNumContrato" runat="server" />
    <asp:HiddenField ID="hddNoContrato" runat="server" />
    <script type="text/javascript">
      function ValidateCheckBox(sender, args) {
        if (!document.getElementById('<%=chkbox_declaracion.ClientID %>').checked) {
          args.IsValid = false;
        } else {
          args.IsValid = true;
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
