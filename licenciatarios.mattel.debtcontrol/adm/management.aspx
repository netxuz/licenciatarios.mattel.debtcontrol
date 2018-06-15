<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="management.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.adm.management" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="/style/style.css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="idUpdatePanel" runat="server">
      <ContentTemplate>
        <telerik:RadSplitter ID="RadSplitter" runat="server" Width="100%" Height="100%"
          Orientation="Horizontal">
          <telerik:RadPane ID="RadPaneTop" CssClass="blinicial" runat="server" Height="120">
            <div class="bx-menu">
              <div>
                <telerik:RadMenu ID="rdMenu" runat="server" OnItemClick="rdMenu_ItemClick">
                  <Items>
                    <telerik:RadMenuItem Text="CUENTAS"></telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="CONFIGURACION"></telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="LOG"></telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="ELIMINACIÓN DE FACTURAS"></telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="ADMINISTRACIÓN DE CONTRATOS"></telerik:RadMenuItem>
                  </Items>
                </telerik:RadMenu>
                <telerik:RadMenu ID="rdSubMenu" runat="server" Flow="Horizontal" Visible="false"
                  OnItemClick="rdSubMenu_ItemClick">
                </telerik:RadMenu>
              </div>
            </div>
          </telerik:RadPane>
          <telerik:RadPane ID="RadPaneDown" runat="server">
          </telerik:RadPane>
        </telerik:RadSplitter>
      </ContentTemplate>
    </asp:UpdatePanel>
  </form>
</body>
</html>
