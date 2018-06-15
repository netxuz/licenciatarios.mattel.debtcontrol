<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tipo_cambio.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.tipo_cambio" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="style/style.css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="box-titulo">
      <asp:Label ID="lbltitulo" runat="server" Text="TIPO DE CAMBIO"></asp:Label>
    </div>
    <div class="box-content-form">
      <asp:Label ID="espacio" runat="server" Text=" "></asp:Label>
      <div class="bx-lbl-data_cambio">
        <telerik:RadGrid ID="rdGridTipoCambio" runat="server" Width="400px" PageSize="24" 
          OnNeedDataSource="rdGridTipoCambio_NeedDataSource" 
          OnItemDataBound="rdGridTipoCambio_ItemDataBound" 
          ShowStatusBar="True" GridLines="None" Skin="Sitefinity">
          <MasterTableView AutoGenerateColumns="false" ShowHeader="true"
            TableLayout="Fixed">
            <Columns>
              <telerik:GridBoundColumn DataField="mes" HeaderStyle-Width="100px" HeaderText="Mes"
                UniqueName="mes">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>
              <telerik:GridBoundColumn DataField="ano" HeaderStyle-Width="100px" HeaderText="Año"
                UniqueName="ano">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>
              <telerik:GridBoundColumn DataField="valor_moneda" HeaderStyle-Width="100px" HeaderText="Valor USD"
                UniqueName="valor_moneda">
                <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
              </telerik:GridBoundColumn>
            </Columns>
          </MasterTableView>
        </telerik:RadGrid>
      </div>
    </div>
  </form>
</body>
</html>
