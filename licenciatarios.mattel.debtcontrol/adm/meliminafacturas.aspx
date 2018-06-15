<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="meliminafacturas.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.meliminafacturas" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="/style/admstyle.css" rel="stylesheet" />
  <script type="text/javascript" src="/Resources/lib/jquery-1.10.1.min.js"></script>
  <link rel="stylesheet" href="/bootstrap/css/bootstrap.min.css" />
  <script src="/bootstrap/js/bootstrap.min.js"></script>
</head>
<body class="bodyadm">
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
      <ContentTemplate>
        <div id="MasterPage">
          <div class="BloqueMenu">
            <div class="lblTitulo">
              <asp:Label ID="lblTitulo" runat="server" Text="ELIMINACION DE FACTURAS"></asp:Label>
            </div>
          </div>
        </div>
        <div class="modulo">
          <div class="bloque">
            <div class="bloque-left">
              <div>
                <asp:Label ID="lbl3" runat="server" Text="LICENCIATARIO"></asp:Label>
              </div>
              <div>
                <asp:TextBox ID="txt_licenciatario" runat="server" CssClass="styleTxtLicenciatario" Enabled="false"></asp:TextBox>
                <asp:HiddenField ID="hdd_nkey_deudor" runat="server" />
                <button type="button" class="btn btn-default btn-sm" data-toggle="modal" data-target="#myModal">
                  <span class="glyphicon glyphicon-search"></span>
                </button>
              </div>
            </div>
            <div>
              <div>
                <asp:Label ID="Label1" runat="server" Text="FACTURA"></asp:Label>
              </div>
              <div>
                <asp:TextBox ID="txt_factura" runat="server" CssClass="styleTxtMan" placeholder="ej: FE104"></asp:TextBox>
              </div>
            </div>
            <div class="Botonera">
              <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="ValidPage" OnClick="btnBuscar_Click" />
            </div>
          </div>
        </div>
        <div id="obgrilla" runat="server" visible="false">
          <telerik:RadGrid ID="RadGrid1" runat="server" ShowStatusBar="True" AutoGenerateColumns="false" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemCommand="RadGrid1_ItemCommand"
            OnItemDataBound="RadGrid1_ItemDataBound" AllowSorting="True" PageSize="15" AllowPaging="True" GridLines="None" Skin="Sitefinity">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <MasterTableView DataKeyNames="codigo_factura, num_contrato, tipo_factura" AutoGenerateColumns="false" ShowHeader="true"
              TableLayout="Fixed" ShowHeadersWhenNoRecords="true" CommandItemDisplay="Top">
              <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false" />
              <Columns>

                <telerik:GridButtonColumn ButtonType="LinkButton" Text="" ConfirmText="Desea eliminar la Factura?" ButtonCssClass="glyphicon glyphicon-trash" UniqueName="Eiminar" CommandName="cmdDelete">
                  <HeaderStyle Font-Size="Smaller" Width="50px" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Center" CssClass="" />
                </telerik:GridButtonColumn>

                <telerik:GridBoundColumn DataField="licenciatario" HeaderText="Licenciatario"
                  UniqueName="licenciatario">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="no_contrato" HeaderText="Contrato"
                  UniqueName="no_contrato">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

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

              </Columns>
            </MasterTableView>
          </telerik:RadGrid>
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Modal -->

    <div id="myModal" class="modal fade" role="dialog">
      <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal">&times;</button>
            <h4 class="modal-title">Licenciatarios</h4>
          </div>
          <div class="modal-body">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <div class="row">
                  <div class="col-xs-4">
                    <asp:TextBox ID="txtBuscarUsuario" CssClass="form-control" runat="server"></asp:TextBox>
                  </div>
                  <div class="col-xs-4">
                    <asp:Button ID="BtnBuscarLicenciatarios" runat="server" Text="Buscar" CssClass="btn btn-danger" OnClick="BtnBuscarLicenciatarios_Click" />
                  </div>
                </div>
                <div class="row">&nbsp;</div>
                <div class="row">

                  <telerik:RadGrid ID="rdLog" runat="server" ShowStatusBar="True" AutoGenerateColumns="false" OnNeedDataSource="rdLog_NeedDataSource" OnItemCreated="rdLog_ItemCreated"
                    AllowSorting="True" PageSize="7" AllowPaging="True" GridLines="None" Skin="Sitefinity">
                    <MasterTableView DataKeyNames="nkey_deudor,snombre" ClientDataKeyNames="nkey_deudor,snombre" AutoGenerateColumns="false" ShowHeader="true"
                      TableLayout="Fixed" ShowHeadersWhenNoRecords="true" CommandItemDisplay="Top">
                      <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false" />
                      <Columns>

                        <telerik:GridButtonColumn ButtonType="LinkButton" Text="" ButtonCssClass="glyphicon glyphicon-check" UniqueName="SelectRecord">
                          <HeaderStyle Font-Size="Smaller" Width="50px" HorizontalAlign="Center" />
                          <ItemStyle HorizontalAlign="Center" CssClass="" />
                        </telerik:GridButtonColumn>

                        <telerik:GridBoundColumn UniqueName="snombre" DataField="snombre" AllowSorting="true">
                          <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                          <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridBoundColumn>

                      </Columns>
                    </MasterTableView>
                  </telerik:RadGrid>

                </div>
                </div>
              </ContentTemplate>
            </asp:UpdatePanel>
            <div class="modal-footer">
              <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>
          </div>
        </div>
      </div>
    </div>
    <script type="text/javascript">
      function onSelectClient(index) {
        var firstDataItem = $find("<%= rdLog.MasterTableView.ClientID %>").get_dataItems()[0];

        var grid = $find("<%=rdLog.ClientID %>");
        var MasterTable = grid.get_masterTableView();
        var row = MasterTable.get_dataItems()[index];

        //var keyValues = 'nkey_deudor: "' + row.getDataKeyValue("nkey_deudor") + '"' + ' \r\n' + 'snombre: "' + row.getDataKeyValue("snombre") + '"';
        //alert(keyValues);
        $('#txt_licenciatario').val(row.getDataKeyValue("snombre"));
        $('#hdd_nkey_deudor').val(row.getDataKeyValue("nkey_deudor"));
        $('#myModal').modal('hide');
      }
    </script>
  </form>
</body>
</html>
