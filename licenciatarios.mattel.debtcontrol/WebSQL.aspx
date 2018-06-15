<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebSQL.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.WebSQL" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:TextBox ID="txt_query" runat="server" TextMode="MultiLine" Width="800px" Height="200px"></asp:TextBox>
    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" />
    </div>
    </form>
</body>
</html>
