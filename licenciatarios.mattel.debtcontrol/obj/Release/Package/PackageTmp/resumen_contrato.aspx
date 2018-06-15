<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="resumen_contrato.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.resumen_contrato" %>

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
      <asp:Label ID="lbltitulo" runat="server" Text="RESUMEN CONTRATO"></asp:Label>
    </div>
    <div class="box-content-form">
      <div class="bx-lbl-data">
        <div class="bx-lbl-filtros">
          <div class="bx-lbl-title">
            <asp:Label ID="lblPeriodo" runat="server" Text="Periodo:"></asp:Label>
          </div>
          <div class="bx-lbl-data">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="sVal" ControlToValidate="cmbox_periodo" ErrorMessage="*" ValidationGroup="ValidPage"></asp:RequiredFieldValidator>
            <asp:DropDownList ID="cmbox_periodo" CssClass="box-select" runat="server">
              <asp:ListItem Text="<< Seleccione Periodo >>" Value=""></asp:ListItem>
              <asp:ListItem Text="Q1" Value="Q1"></asp:ListItem>
              <asp:ListItem Text="Q2" Value="Q2"></asp:ListItem>
              <asp:ListItem Text="Q3" Value="Q3"></asp:ListItem>
              <asp:ListItem Text="Q4" Value="Q4"></asp:ListItem>
            </asp:DropDownList>
          </div>
        </div>
        <div class="bx-lbl-filtros">
          <div class="bx-lbl-title">
            <asp:Label ID="lblAno" runat="server" Text="Ano:"></asp:Label>
          </div>
          <div class="bx-lbl-data">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="sVal" ControlToValidate="cmbox_ano" ErrorMessage="*" ValidationGroup="ValidPage"></asp:RequiredFieldValidator>
            <asp:DropDownList ID="cmbox_ano" CssClass="box-select" runat="server">
              <asp:ListItem Text="<< Seleccione Año >>" Value=""></asp:ListItem>
            </asp:DropDownList>
          </div>
        </div>
      </div>
      <div class="bx-lbl-data">
        <asp:Button ID="btnBuscar" CssClass="btnGuardar" runat="server" Text="Aceptar" OnClick="btnBuscar_Click" ValidationGroup="ValidPage" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="False" ShowSummary="False" ValidationGroup="ValidPage" />
      </div>
      <div class="bx-lbl-data" id="idGrilla" runat="server" visible="false">
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
            <telerik:AjaxSetting AjaxControlID="rdGridReporteVenta">
              <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rdGridReporteVenta" />
              </UpdatedControls>
            </telerik:AjaxSetting>
          </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadGrid ID="rdGridReporteVenta" runat="server"
          OnNeedDataSource="rdGridReporteVenta_NeedDataSource"
          OnItemCommand="rdGridReporteVenta_ItemCommand"
          OnItemDataBound="rdGridReporteVenta_ItemDataBound"
          AllowPaging="True" AllowSorting="true" ShowStatusBar="True" PageSize="10" GridLines="None"
          AllowAutomaticUpdates="true" AllowAutomaticInserts="true" AllowAutomaticDeletes="true"
          Skin="Sitefinity">
          <PagerStyle Mode="NextPrevAndNumeric" />
          <ExportSettings HideStructureColumns="true"></ExportSettings>
          <MasterTableView AutoGenerateColumns="false" ShowHeader="true"
            TableLayout="Fixed" ShowHeadersWhenNoRecords="true" ShowFooter="true" CommandItemDisplay="Top">
            <CommandItemSettings ShowExportToExcelButton="true" ShowRefreshButton="false" ShowAddNewRecordButton="false" />
            <Columns>
              <telerik:GridBoundColumn DataField="Licenciatario" HeaderText="Licenciatario"
                UniqueName="Licenciatario">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="Contrato" HeaderText="Contrato"
                UniqueName="Contrato">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="Inicio" HeaderText="Inicio"
                UniqueName="Inicio">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="Termino" HeaderText="Termino"
                UniqueName="Termino">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="Por vencer" HeaderText="Por vencer"
                UniqueName="porvencer">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="mesuno"
                UniqueName="mesuno">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="mesdos"
                UniqueName="mesdos">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="mestres"
                UniqueName="mestres">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="Numero Factura" HeaderText="Numero Factura"
                UniqueName="num_factura">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="Fecha Factura" HeaderText="Fecha Factura"
                UniqueName="Fecha Factura">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="Fecha Comprobante" HeaderText="Fecha Comprobante"
                UniqueName="FechaComprobante">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridTemplateColumn UniqueName="BtnBajarComprobante" ItemStyle-HorizontalAlign="Center" HeaderText="Comprobante SII" HeaderStyle-Width="100">
                <ItemTemplate>
                  <asp:ImageButton ID="BtnBajarComprobante" runat="server" ImageUrl="/images/btn_pencil2.jpg" CommandName="BajarComprobante" CommandArgument="CommandArgument" />
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" Width="100" />
                <ItemStyle HorizontalAlign="Center" CssClass="CursorHand" />
              </telerik:GridTemplateColumn>
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
