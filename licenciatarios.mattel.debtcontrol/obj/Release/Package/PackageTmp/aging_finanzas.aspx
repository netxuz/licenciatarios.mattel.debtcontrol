<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="aging_finanzas.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.aging_finanzas" %>

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
      <asp:Label ID="lbltitulo" runat="server" Text="Aging de Deuda"></asp:Label>
    </div>
    <div class="box-content-form">
      <div class="bx-lbl-data">
        <div class="bx-lbl-data" runat="server">
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
              <telerik:AjaxSetting AjaxControlID="rdGridAging">
                <UpdatedControls>
                  <telerik:AjaxUpdatedControl ControlID="rdGridAging" />
                </UpdatedControls>
              </telerik:AjaxSetting>
            </AjaxSettings>
          </telerik:RadAjaxManager>
          <telerik:RadGrid ID="rdGridAging" runat="server" 
            OnNeedDataSource="rdGridAging_NeedDataSource" 
            OnItemCommand="rdGridAging_ItemCommand"
            AllowPaging="True" AllowSorting="true" ShowStatusBar="True" PageSize="10" GridLines="None"
            AllowAutomaticUpdates="true" AllowAutomaticInserts="true" AllowAutomaticDeletes="true"
            Skin="Sitefinity">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <ExportSettings HideStructureColumns="true"></ExportSettings>
            <MasterTableView AutoGenerateColumns="false" ShowHeader="true"
              TableLayout="Fixed" ShowHeadersWhenNoRecords="true" ShowFooter="true" CommandItemDisplay="Top">
              <CommandItemSettings ShowExportToExcelButton="true" ShowRefreshButton="false" ShowAddNewRecordButton="false" />
              <Columns>
                <telerik:GridBoundColumn DataField="ncodigodeudor" HeaderText="Código"
                  UniqueName="ncodigodeudor">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="snombre" HeaderText="Deudor"
                  UniqueName="snombre">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="Total" HeaderText="Deuda Total"
                  UniqueName="Total" Aggregate="Sum" DataType="System.Double" DataFormatString="{0:N0}" FooterAggregateFormatString="{0:N0}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="total_0" HeaderText="Deuda por Vencer"
                  UniqueName="total_0" Aggregate="Sum" DataType="System.Double" DataFormatString="{0:N0}" FooterAggregateFormatString="{0:N0}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="total_15" HeaderText="1 a 15 Vencida"
                  UniqueName="total_15" Aggregate="Sum" DataType="System.Double" DataFormatString="{0:N0}" FooterAggregateFormatString="{0:N0}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="total_30" HeaderText="16 a 30 Vencida"
                  UniqueName="total_30" Aggregate="Sum" DataType="System.Double" DataFormatString="{0:N0}" FooterAggregateFormatString="{0:N0}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="total_60" HeaderText="31 a 60 Vencida"
                  UniqueName="total_60" Aggregate="Sum" DataType="System.Double" DataFormatString="{0:N0}" FooterAggregateFormatString="{0:N0}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="total_90" HeaderText="61 a 90 Vencida"
                  UniqueName="total_90" Aggregate="Sum" DataType="System.Double" DataFormatString="{0:N0}" FooterAggregateFormatString="{0:N0}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="total_180" HeaderText="91 a 180 Vencida"
                  UniqueName="total_180" Aggregate="Sum" DataType="System.Double" DataFormatString="{0:N0}" FooterAggregateFormatString="{0:N0}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="total_360" HeaderText="180 a 360 Vencida"
                  UniqueName="total_360" Aggregate="Sum" DataType="System.Double" DataFormatString="{0:N0}" FooterAggregateFormatString="{0:N0}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
              </Columns>
            </MasterTableView>
          </telerik:RadGrid>
        </div>
      </div>
    </div>
    <div style="height: 20px">
    </div>
    <div class="box-pie">
      <asp:Button ID="btnInstructivo" runat="server" CssClass="btnPie" Text="Instructivo" onclick="btnInstructivo_Click" />
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
