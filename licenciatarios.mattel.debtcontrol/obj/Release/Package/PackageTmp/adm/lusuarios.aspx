<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lusuarios.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.adm.lusuarios" %>

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
      <div>
        <asp:Button ID="btnCrear" runat="server" Text="Crear" OnClick="btnCrear_Click" />
      </div>
      <div>
        <asp:TextBox ID="txt_buscar" runat="server" CssClass="styleTxtSearch"></asp:TextBox>
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
      </div>
      <div><br /></div>
      <div>
        <telerik:RadGrid ID="rdUsuarios" runat="server" ShowStatusBar="True" AutoGenerateColumns="false"
          AllowSorting="True" PageSize="15" AllowPaging="True" GridLines="None" OnNeedDataSource="rdUsuarios_NeedDataSource"
          OnItemCommand="rdUsuarios_ItemCommand" OnItemDataBound="rdUsuarios_ItemDataBound" Skin="Windows7">
          <MasterTableView DataKeyNames="cod_user" AutoGenerateColumns="false"
                ShowHeader="true" TableLayout="Fixed" ShowHeadersWhenNoRecords="true">
            <Columns>
              <telerik:GridButtonColumn ButtonCssClass="btnGrid" HeaderStyle-Width="100px" ButtonType="PushButton" Text="Editar" UniqueName="Editar" CommandName="cmdEdit">
              </telerik:GridButtonColumn>
              <telerik:GridButtonColumn ButtonCssClass="btnGrid" HeaderStyle-Width="100px" ButtonType="PushButton" Text="Eliminar" ConfirmText="Desea eliminar el usuario?" UniqueName="Eliminar" CommandName="cmdDelete">
              </telerik:GridButtonColumn>
              <telerik:GridBoundColumn UniqueName="NomUsuario" DataField="nom_user" HeaderText="Nombre"
                SortExpression="Nombre">
              </telerik:GridBoundColumn>
              <telerik:GridBoundColumn UniqueName="ApeUsuario" DataField="ape_user" HeaderText="Apellido"
                SortExpression="Apellido">
              </telerik:GridBoundColumn>
              <telerik:GridBoundColumn UniqueName="EstUsuario" DataField="est_user" HeaderText="Estado"
                SortExpression="Estado">
              </telerik:GridBoundColumn>
            </Columns>
          </MasterTableView>
        </telerik:RadGrid>
      </div>
    </div>
  </div>
  </form>
</body>
</html>
