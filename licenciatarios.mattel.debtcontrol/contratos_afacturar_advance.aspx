<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contratos_afacturar_advance.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.contratos_afacturar_advance" %>

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
      <asp:Label ID="lbltitulo" runat="server" Text="APROBACION DE FACTURAS ADVANCE"></asp:Label>
    </div>
    <div class="bx-lbl-btn">
      <div class="bx-lbl-filtros">
        <div class="bx-lbl-data">
          <asp:Button ID="btnAprobar" CssClass="btnGuardar" runat="server" Text="Aprobar Todas" OnClick="btnAprobar_Click" OnClientClick="this.disabled=true" UseSubmitBehavior="false" />
        </div>
      </div>
      <div class="bx-lbl-filtros">
        <div class="bx-lbl-data">
          <asp:Button ID="btnRechazar" CssClass="btnGuardar" runat="server" Text="Rechazar Todas" OnClientClick="return openConfirmPopUp('');" />
        </div>
      </div>
    </div>
    <div class="box-content-form">
      <div class="bx-lbl-data" runat="server">
        <telerik:RadGrid ID="rdGridLicen" runat="server" OnNeedDataSource="rdGridLicen_NeedDataSource"
          OnItemCommand="rdGridLicen_ItemCommand" OnItemDataBound="rdGridLicen_ItemDataBound"
          AllowPaging="True" AllowSorting="true" ShowStatusBar="True" PageSize="10" GridLines="None"
          AllowAutomaticUpdates="true" AllowAutomaticInserts="true" AllowAutomaticDeletes="true"
          Skin="Sitefinity">
          <PagerStyle Mode="NextPrevAndNumeric" />
          <MasterTableView DataKeyNames="num_contrato, no_contrato" AutoGenerateColumns="false" ShowHeader="true"
            TableLayout="Fixed" ShowHeadersWhenNoRecords="true">
            <Columns>

              <telerik:GridTemplateColumn UniqueName="BtnAprobarFactura" ItemStyle-HorizontalAlign="Center" HeaderText="Factura" HeaderStyle-Width="100">
                <ItemTemplate>
                  <asp:ImageButton ID="BtnAprobarFactura" runat="server" ImageUrl="/images/btn_pencil2.jpg" CommandName="AprobarFactura" CommandArgument="CommandArgument" />
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" Width="100" />
                <ItemStyle HorizontalAlign="Center" CssClass="CursorHand" />
              </telerik:GridTemplateColumn>

              <telerik:GridBoundColumn DataField="licenciatario" HeaderText="Licenciatario"
                UniqueName="licenciatario">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="no_contrato" HeaderText="Contrato"
                UniqueName="no_contrato">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="periodo" HeaderText="Periodo"
                UniqueName="periodo">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" Width="100" />
                <ItemStyle HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn HeaderText="Monto"
                UniqueName="monto">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" Width="100" />
                <ItemStyle HorizontalAlign="Right" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn HeaderText="FECHA PRE FACTURACIÓN"
                UniqueName="fecha_full">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" Width="150" />
                <ItemStyle HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridTemplateColumn UniqueName="BtnAprobar" ItemStyle-HorizontalAlign="Center" HeaderText="Aprobar" HeaderStyle-Width="100">
                <ItemTemplate>
                  <asp:Button ID="btnAprobar" runat="server" CssClass="btnGrid" CommandName="Aprobar" Text="Aprobar" OnClientClick="this.disabled=true" UseSubmitBehavior="false" CommandArgument="CommandArgument" />
                </ItemTemplate>
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" Width="200" />
                <ItemStyle HorizontalAlign="Center" CssClass="CursorHand" Width="200" />
              </telerik:GridTemplateColumn>

              <telerik:GridTemplateColumn UniqueName="BtnRechazar" ItemStyle-HorizontalAlign="Center" HeaderText="Rechazar" HeaderStyle-Width="100">
                <ItemTemplate>
                  <asp:Button ID="btnRechazar" runat="server" CssClass="btnGrid" Text="Rechazar" OnClientClick='<%# String.Format("return openConfirmPopUp({0});",Eval("num_contrato"))%>' />
                </ItemTemplate>
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" Width="200" />
                <ItemStyle HorizontalAlign="Center" CssClass="CursorHand" Width="200" />
              </telerik:GridTemplateColumn>

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
      function openConfirmPopUp(num_contrato) {
        $.fancybox([
           { href: 'rechazo_factura.aspx?numcontrato=' + num_contrato }
        ], {
          'autoDimensions': false,
          'transitionIn': 'elastic',
          'transitionOut': 'elastic',
          'type': 'iframe'
        });
        return false;
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
