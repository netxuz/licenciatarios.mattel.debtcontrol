<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reporting_regional.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.reporting_regional" %>

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
      <asp:Label ID="lbltitulo" runat="server" Text="REPORTING REGIONAL (AR)"></asp:Label>
    </div>
    <div class="box-content-form">
      <div class="bx-lbl-data">
        <div class="bx-lbl-title">
          <asp:Label ID="lblAno" runat="server" Text="Ano:"></asp:Label>
        </div>
      </div>
      <div class="bx-lbl-data">
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="sVal" ControlToValidate="cmbox_ano" ErrorMessage="*" ValidationGroup="ValidPage"></asp:RequiredFieldValidator>
        <asp:DropDownList ID="cmbox_ano" CssClass="box-select" runat="server">
          <asp:ListItem Text="<< Seleccione Año >>" Value=""></asp:ListItem>
        </asp:DropDownList>
      </div>
      <div class="bx-lbl-btn">
        <div class="bx-lbl-data">
          <asp:Button ID="btnGenerar" CssClass="btnGuardar" runat="server" Text="Generar AR" OnClick="btnGenerar_Click" ValidationGroup="ValidPage" />
          <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="False" ShowSummary="False" ValidationGroup="ValidPage" />
        </div>
      </div>
      <div class="bx-lbl-data" runat="server">
        <telerik:RadGrid ID="rdGridReporting" runat="server"
          OnNeedDataSource="rdGridReporting_NeedDataSource"
          OnItemCommand="rdGridReporting_ItemCommand"
          OnItemDataBound="rdGridReporting_ItemDataBound"
          AllowPaging="True" AllowSorting="true" ShowStatusBar="True" PageSize="10" GridLines="None"
          AllowAutomaticUpdates="true" AllowAutomaticInserts="true" AllowAutomaticDeletes="true"
          Skin="Sitefinity">
          <PagerStyle Mode="NextPrevAndNumeric" />
          <MasterTableView DataKeyNames="cod_reporting" AutoGenerateColumns="false" ShowHeader="true"
            TableLayout="Fixed" ShowHeadersWhenNoRecords="true">
            <Columns>

              <telerik:GridTemplateColumn UniqueName="BtnBajarReporting" ItemStyle-HorizontalAlign="Center" HeaderText="Factura" HeaderStyle-Width="100">
                <ItemTemplate>
                  <asp:ImageButton ID="BtnBajarReporting" runat="server" ImageUrl="/images/btn_pencil2.jpg" CommandName="BajarReporting" CommandArgument="CommandArgument" />
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" Width="100" />
                <ItemStyle HorizontalAlign="Center" CssClass="CursorHand" />
              </telerik:GridTemplateColumn>

              <telerik:GridBoundColumn DataField="nom_reporting" HeaderText="Reporting"
                UniqueName="Reporting">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="ano_reporting" HeaderText="Año"
                UniqueName="ano_reporting">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="fech_reporting" HeaderText="Fecha"
                UniqueName="fech_reporting">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="est_reporting" HeaderText="Estado"
                UniqueName="est_reporting">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" Width="100" />
                <ItemStyle HorizontalAlign="center" Width="100" />
              </telerik:GridBoundColumn>
            </Columns>
          </MasterTableView>
        </telerik:RadGrid>
      </div>
    </div>
    <div style="height: 20px">
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
