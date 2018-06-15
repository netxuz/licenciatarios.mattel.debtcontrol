<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="status-facturas.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.status_facturas" %>

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
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
    <div class="box-titulo">
      <asp:Label ID="lbltitulo" runat="server" Text="STATUS DE FACTURAS"></asp:Label>
    </div>
    <div class="box-content-form">
      <div class="bx-lbl-title">
        <asp:Label ID="lblcontrato" runat="server" Text="Contrato:"></asp:Label>
      </div>
      <div class="bx-lbl-data">
        <asp:DropDownList ID="cmbox_contrato" CssClass="box-select" runat="server" OnSelectedIndexChanged="cmbox_contrato_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>
      </div>
      <div class="bx-lbl-data">
        <script type="text/javascript">
          function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0) {
              args.set_enableAjax(false);
            }
          }
        </script>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
          <ClientEvents OnRequestStart="onRequestStart" />
          <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rdGridFactura">
              <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rdGridFactura" />
              </UpdatedControls>
            </telerik:AjaxSetting>
          </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadGrid ID="rdGridFactura" runat="server"
          OnNeedDataSource="rdGridFactura_NeedDataSource"
          OnItemDataBound="rdGridFactura_ItemDataBound" OnItemCommand="rdGridFactura_ItemCommand"
          AllowPaging="True" AllowSorting="true" ShowStatusBar="True" PageSize="10" GridLines="None"
          AllowAutomaticUpdates="true" AllowAutomaticInserts="true" AllowAutomaticDeletes="true"
          Skin="Sitefinity">
          <ExportSettings HideStructureColumns="true"></ExportSettings>
          <PagerStyle Mode="NextPrevAndNumeric" />
          <MasterTableView DataKeyNames="codigo_factura,num_contrato,tipo_factura,pdf_generado" AutoGenerateColumns="false" ShowHeader="true"
            TableLayout="Fixed" ShowHeadersWhenNoRecords="true" CommandItemDisplay="Top">
            <CommandItemSettings ShowExportToExcelButton="true" ShowRefreshButton="false" ShowAddNewRecordButton="false" />
            <Columns>
              <telerik:GridTemplateColumn UniqueName="BtnVerFactura" ItemStyle-HorizontalAlign="Center" HeaderText="Factura" HeaderStyle-Width="100">
                <ItemTemplate>
                  <asp:ImageButton ID="btnVerFactura" runat="server" ImageUrl="/images/pdf150.jpg" CommandName="VerFactura" CommandArgument="CommandArgument" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="CursorHand" />
              </telerik:GridTemplateColumn>

              <telerik:GridBoundColumn DataField="num_invoice" HeaderText="Número Invoice"
                UniqueName="num_invoice">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>
              <telerik:GridBoundColumn DataField="periodo" HeaderText="Periodo"
                UniqueName="periodo">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
              </telerik:GridBoundColumn>
              <telerik:GridBoundColumn DataField="date_invoce" HeaderText="Emisión"
                UniqueName="date_invoce">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
              </telerik:GridBoundColumn>
              <telerik:GridBoundColumn DataField="due_date" HeaderText="Vencimiento"
                UniqueName="due_date">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
              </telerik:GridBoundColumn>
              <telerik:GridBoundColumn DataField="total" HeaderText="Monto"
                UniqueName="total">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
              </telerik:GridBoundColumn>
              <telerik:GridBoundColumn HeaderText="Saldo"
                UniqueName="saldo">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
              </telerik:GridBoundColumn>
              <telerik:GridBoundColumn HeaderText="Fecha de Pago"
                UniqueName="fecha_pago">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
              </telerik:GridBoundColumn>
            </Columns>
          </MasterTableView>
        </telerik:RadGrid>

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
