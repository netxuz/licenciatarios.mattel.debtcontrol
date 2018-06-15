<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logeventos.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.adm.logeventos" %>

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
              <asp:Label ID="lblTitulo" runat="server" Text="LOG DE EVENTOS"></asp:Label>
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
                <asp:Label ID="Label1" runat="server" Text="CONTRATO"></asp:Label>
              </div>
              <div>
                <asp:TextBox ID="txt_no_contrato" runat="server" CssClass="styleTxtMan"></asp:TextBox>
              </div>
            </div>
            <div class="bloque-left">
              <div>
                <asp:Label ID="Label2" runat="server" Text="FLUJO"></asp:Label>
              </div>
              <div>
                <asp:DropDownList ID="dropdownflujo" runat="server" CssClass="styleBoxSelectLicenciatario">
                  <asp:ListItem Value="" Text="<< TODOS >>"></asp:ListItem>
                  <asp:ListItem Value="1" Text="LOGIN"></asp:ListItem>
                  <asp:ListItem Value="2" Text="INGRESO DE VENTAS LICENCIATARIO"></asp:ListItem>
                  <asp:ListItem Value="3" Text="COMPROBANTE DE IMPUESTO"></asp:ListItem>
                  <asp:ListItem Value="4" Text="FACTURACION ADVANCE"></asp:ListItem>
                  <asp:ListItem Value="5" Text="FACTURACION Q"></asp:ListItem>
                  <asp:ListItem Value="6" Text="FACTURACION SHORT FALL"></asp:ListItem>
                </asp:DropDownList>
              </div>
            </div>
            <div>
              <div>
                <asp:Label ID="Label3" runat="server" Text="FECHA"></asp:Label>
              </div>
              <div>
                <telerik:RadDatePicker ID="RadDatePicker1" runat="server">
                  <Calendar RangeMinDate="1900-01-01" runat="server">
                  </Calendar>
                </telerik:RadDatePicker>
                <asp:Label ID="Label4" runat="server" Text="HASTA"></asp:Label>
                <telerik:RadDatePicker ID="RadDatePicker2" runat="server">
                  <Calendar RangeMinDate="1900-01-01" runat="server">
                  </Calendar>
                </telerik:RadDatePicker>

              </div>
            </div>
          </div>
          <div>
            <div class="Botonera">
              <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" ValidationGroup="ValidPage" />
              <asp:CustomValidator ID="CustomValidator" runat="server" ErrorMessage="" Text="" ClientValidationFunction="ValidaRangeFecha" Display="Dynamic" ValidationGroup="ValidPage"></asp:CustomValidator>
            </div>
          </div>
        </div>
        <div id="obgrilla" runat="server" visible="false">
          <telerik:RadGrid ID="RadGrid1" runat="server" ShowStatusBar="True" AutoGenerateColumns="false" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemCommand="RadGrid1_ItemCommand" OnItemDataBound="RadGrid1_ItemDataBound"
            AllowSorting="True" PageSize="100" AllowPaging="True" GridLines="None" Skin="Sitefinity">
            <ExportSettings HideStructureColumns="true"></ExportSettings>
            <PagerStyle Mode="NextPrevAndNumeric" />
            <MasterTableView DataKeyNames="arch_log, ind_reporsitorio_arch" AutoGenerateColumns="false" ShowHeader="true"
              TableLayout="Fixed" ShowHeadersWhenNoRecords="true" CommandItemDisplay="Top">
              <CommandItemSettings ShowExportToExcelButton="true" ShowRefreshButton="false" ShowAddNewRecordButton="false" />
              <Columns>

                <telerik:GridBoundColumn UniqueName="nom_deudor" HeaderText="Licenciatario" DataField="nom_deudor" AllowSorting="true">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Left" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="no_contrato" HeaderText="Contrato" DataField="no_contrato" AllowSorting="true">
                  <HeaderStyle Width="100px" Font-Size="Smaller" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="nom_user" HeaderText="Usuario" DataField="nom_user" AllowSorting="true">
                  <HeaderStyle Width="200px" Font-Size="Smaller" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Left" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="fch_log" HeaderText="Fecha" DataField="fch_log" AllowSorting="true">
                  <HeaderStyle Width="150px" Font-Size="Smaller" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="accion_log" HeaderText="Acción" DataField="accion_log" AllowSorting="true">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Left" />
                </telerik:GridBoundColumn>

                <telerik:GridTemplateColumn UniqueName="BtnVerArchivo" ItemStyle-HorizontalAlign="Center" HeaderText="Archivo" HeaderStyle-Width="100">
                <ItemTemplate>
                  <asp:ImageButton ID="btnDownFile" runat="server" ImageUrl="../images/download.png" CommandName="DownFile" CommandArgument="CommandArgument" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="CursorHand" />
              </telerik:GridTemplateColumn>
              </Columns>
            </MasterTableView>
          </telerik:RadGrid>
        </div>
        <div>
          <br />
          <br />
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

                  <telerik:RadGrid ID="rdLog" runat="server" ShowStatusBar="True" AutoGenerateColumns="false" OnNeedDataSource="rdLog_NeedDataSource"
                    AllowSorting="True" PageSize="7" AllowPaging="True" GridLines="None" Skin="Sitefinity" OnItemCreated="rdLog_ItemCreated">
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

      <script type="text/javascript">
        function ValidaRangeFecha(sender, args) {
          var radInput1 = $find('RadDatePicker1');
          var radInput2 = $find('RadDatePicker2');
          args.IsValid = true;

          if (((radInput1.get_selectedDate() != null) && (radInput2.get_selectedDate() == null)) || ((radInput1.get_selectedDate() == null) && (radInput2.get_selectedDate() != null))) {
            alert('Rango de fecha incorrecto.');
            args.IsValid = false;
          }

          if ((radInput1.get_selectedDate() != null) && (radInput2.get_selectedDate() != null))
            if (radInput1.get_selectedDate() > radInput2.get_selectedDate()) {
              alert('El rango de fecha esta incorrecta. La fecha de inicio debe ser mayor igual que la fecha final');
              args.IsValid = false;
            }
        }

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

