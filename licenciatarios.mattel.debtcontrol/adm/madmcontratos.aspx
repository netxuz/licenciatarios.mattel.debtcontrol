<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="madmcontratos.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.adm.madmcontratos" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="/style/admstyle.css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
    <div class="entorno">
      <div class="panel">
        <div class="modulo">
          <div class="bloque">
            <div>
              <div>
                <asp:Label ID="Label1" runat="server" Text="NUMERO CONTRATO"></asp:Label>
              </div>
              <div>
                <asp:TextBox ID="txt_contrato" runat="server" CssClass="styleTxtMan"></asp:TextBox>
              </div>
            </div>
            <div class="Botonera">
              <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
            </div>
          </div>
        </div>
        <div>
          <telerik:RadGrid ID="rdContrato" runat="server" ShowStatusBar="True" AutoGenerateColumns="false" AllowSorting="True" PageSize="15" AllowPaging="True" GridLines="None" Skin="Windows7"
            OnNeedDataSource="rdContrato_NeedDataSource" OnItemCommand="rdContrato_ItemCommand" OnItemDataBound="rdContrato_ItemDataBound">
            <MasterTableView DataKeyNames="num_contrato" AutoGenerateColumns="false" ShowHeader="true" TableLayout="Fixed" ShowHeadersWhenNoRecords="true">
              <Columns>

                <telerik:GridButtonColumn ButtonCssClass="btnGrid" HeaderStyle-Width="100px" ButtonType="PushButton" Text="Ver" UniqueName="Editar" CommandName="cmdEdit">
                </telerik:GridButtonColumn>

                <telerik:GridBoundColumn UniqueName="licenciatario" HeaderText="Licenciatario" DataField="licenciatario">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" Width="400px" />
                  <ItemStyle HorizontalAlign="Left" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="no_contrato" HeaderText="N° Contrato" DataField="no_contrato">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" Width="100px" />
                  <ItemStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="tipo_contrato" HeaderText="Tipo Contrato" DataField="tipo_contrato">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" Width="100px" />
                  <ItemStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="fech_inicio" HeaderText="Ficha Inicio" DataField="fech_inicio" DataType="System.DateTime" DataFormatString="{0:d/M/yyyy}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" Width="100px" />
                  <ItemStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="fech_termino" HeaderText="Ficha Termino" DataField="fech_termino" DataType="System.DateTime" DataFormatString="{0:d/M/yyyy}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" Width="100px"  />
                  <ItemStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn UniqueName="aprobado" HeaderText="Estado" DataField="aprobado">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" Width="100px" />
                  <ItemStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>

              </Columns>
            </MasterTableView>
          </telerik:RadGrid>
        </div>
      </div>
    </div>
    <div>
      <br />
      <br />
    </div>
  </form>
</body>
</html>
