<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reporte_ventas_finanzas.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.reporte_ventas_finanzas" %>

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
      <asp:Label ID="lbltitulo" runat="server" Text="REPORTES DE VENTAS"></asp:Label>
    </div>
    <div class="box-content-form">
      <div class="bx-lbl-data">
        <div class="bx-lbl-filtros">
          <div class="bx-lbl-title">
            <asp:Label ID="lblLicenciatario" runat="server" Text="Licenciatario:"></asp:Label>
          </div>
          <div class="bx-lbl-data">
            <asp:DropDownList ID="cmbox_licenciatario" CssClass="box-select" runat="server"
              OnSelectedIndexChanged="cmbox_licenciatario_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
          </div>
        </div>
        <div class="bx-lbl-filtros">
          <div class="bx-lbl-title">
            <asp:Label ID="lblcontrato" runat="server" Text="Contrato:"></asp:Label>
          </div>
          <div class="bx-lbl-data">
            <asp:DropDownList ID="cmbox_contrato" CssClass="box-select" runat="server">
            </asp:DropDownList>
          </div>
        </div>
        <div class="bx-lbl-filtros">
          <div class="bx-lbl-title">
            <asp:Label ID="Label1" runat="server" CssClass="sFch" Text="Desde:"></asp:Label>
          </div>
          <div class="bx-lbl-data">
            <asp:DropDownList ID="ddlist_mes_ini" CssClass="box-select-mes" runat="server">
              <asp:ListItem Text="MES" Value="0"></asp:ListItem>
              <asp:ListItem Text="ENERO" Value="1"></asp:ListItem>
              <asp:ListItem Text="FEBRERO" Value="2"></asp:ListItem>
              <asp:ListItem Text="MARZO" Value="3"></asp:ListItem>
              <asp:ListItem Text="ABRIL" Value="4"></asp:ListItem>
              <asp:ListItem Text="MAYO" Value="5"></asp:ListItem>
              <asp:ListItem Text="JUNIO" Value="6"></asp:ListItem>
              <asp:ListItem Text="JULIO" Value="7"></asp:ListItem>
              <asp:ListItem Text="AGOSTO" Value="8"></asp:ListItem>
              <asp:ListItem Text="SEPTIEMBRE" Value="9"></asp:ListItem>
              <asp:ListItem Text="OCTUBRE" Value="10"></asp:ListItem>
              <asp:ListItem Text="NOVIEMBRE" Value="11"></asp:ListItem>
              <asp:ListItem Text="DICIEMBRE" Value="12"></asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlist_ano_ini" CssClass="box-select-ano" runat="server">
              <asp:ListItem Text="AÑO" Value="0"></asp:ListItem>
            </asp:DropDownList>
          </div>
        </div>
        <div class="bx-lbl-filtros">
          <div class="bx-lbl-title">
            <asp:Label ID="Label2" runat="server" CssClass="sFch" Text="Hasta:"></asp:Label>
          </div>
          <div class="bx-lbl-data">
            <asp:DropDownList ID="ddlist_mes_fin" CssClass="box-select-mes" runat="server">
              <asp:ListItem Text="MES" Value="0"></asp:ListItem>
              <asp:ListItem Text="ENERO" Value="1"></asp:ListItem>
              <asp:ListItem Text="FEBRERO" Value="2"></asp:ListItem>
              <asp:ListItem Text="MARZO" Value="3"></asp:ListItem>
              <asp:ListItem Text="ABRIL" Value="4"></asp:ListItem>
              <asp:ListItem Text="MAYO" Value="5"></asp:ListItem>
              <asp:ListItem Text="JUNIO" Value="6"></asp:ListItem>
              <asp:ListItem Text="JULIO" Value="7"></asp:ListItem>
              <asp:ListItem Text="AGOSTO" Value="8"></asp:ListItem>
              <asp:ListItem Text="SEPTIEMBRE" Value="9"></asp:ListItem>
              <asp:ListItem Text="OCTUBRE" Value="10"></asp:ListItem>
              <asp:ListItem Text="NOVIEMBRE" Value="11"></asp:ListItem>
              <asp:ListItem Text="DICIEMBRE" Value="12"></asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlist_ano_fin" CssClass="box-select-ano" runat="server">
              <asp:ListItem Text="AÑO" Value="0"></asp:ListItem>
            </asp:DropDownList>
          </div>
        </div>
      </div>
      <div class="bx-lbl-data">
        <asp:Button ID="btnBuscar" CssClass="btnGuardar" runat="server" Text="Aceptar" OnClick="btnBuscar_Click" />
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
          OnItemDataBound="rdGridReporteVenta_ItemDataBound" OnItemCommand="rdGridReporteVenta_ItemCommand"
          AllowPaging="True" AllowSorting="true" ShowStatusBar="True" PageSize="10" GridLines="None"
          AllowAutomaticUpdates="true" AllowAutomaticInserts="true" AllowAutomaticDeletes="true"
          Skin="Sitefinity">
          <PagerStyle Mode="NextPrevAndNumeric" />
          <ExportSettings HideStructureColumns="true"></ExportSettings>
          <MasterTableView AutoGenerateColumns="false" ShowHeader="true" ShowFooter="true"
            TableLayout="Fixed" ShowHeadersWhenNoRecords="true" CommandItemDisplay="Top">
            <CommandItemSettings ShowExportToExcelButton="true" ShowRefreshButton="false" ShowAddNewRecordButton="false" />
            <Columns>
              <telerik:GridBoundColumn DataField="mes" HeaderText="Mes"
                UniqueName="Mes">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="ano" HeaderText="Año"
                UniqueName="Ano">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="licenciatario" HeaderText="Licenciatario"
                UniqueName="licenciatario">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="no_contrato" HeaderText="Contrato"
                UniqueName="no_contrato">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="nom_marca" HeaderText="Marca"
                UniqueName="nom_marca">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="nom_categoria" HeaderText="Categoría"
                UniqueName="Categoría">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="nom_subcategoria" HeaderText="Sub-Categoría"
                UniqueName="Sub-Categoría">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="producto" HeaderText="Producto"
                UniqueName="Producto">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="royalty" HeaderText="Royalty (%)"
                UniqueName="Royalty">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="bdi" HeaderText="BDI (%)"
                UniqueName="BDI">
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

              <telerik:GridBoundColumn DataField="precio_uni_venta_bruta" HeaderText="Precio Unit. Venta"
                UniqueName="precio_uni_venta_bruta">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="cantidad_venta_bruta" HeaderText="Unid."
                UniqueName="cantidad_venta_bruta">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn HeaderText="Total Local $." DataField="total_local"
                UniqueName="total_local" Aggregate="Sum" DataType="System.Double" DataFormatString="{0:N0}" FooterAggregateFormatString="{0:N0}">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="precio_uni_devolucion" HeaderText="Precio Unit. Dev"
                UniqueName="precio_uni_devolucion">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn DataField="cantidad_q_devolucion" HeaderText="Q."
                UniqueName="cantidad_q_devolucion">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn HeaderText="Total Local $"
                UniqueName="total_local_devol" DataField="total_local_devol" Aggregate="Sum" DataType="System.Double" DataFormatString="{0:N0}" FooterAggregateFormatString="{0:N0}">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn HeaderText="Venta Neta Local $"
                UniqueName="venta_neta">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn HeaderText="Tipo Cambio"
                UniqueName="tipo_cambio">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn HeaderText="Venta Neta USD"
                UniqueName="venta_neta_usd">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn HeaderText="Royalty USD"
                UniqueName="royalty_usd">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>

              <telerik:GridBoundColumn HeaderText="BDI USD"
                UniqueName="bdi_usd">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
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
        <asp:Label ID="Label3" runat="server" Text="Teléfono: +562 25996100 | Email: licenciatariosmattel@debtcontrol.cl"></asp:Label>
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
