<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="musuario.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.adm.musuario" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="/style/admstyle.css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <div class="entorno">
      <div class="panel">
        <div>
          <asp:Button ID="btnGrabar" runat="server" Text="Grabar"
            OnClick="btnGrabar_Click" />
        </div>
        <div>
          <div>
            <asp:Label ID="lblnombre" runat="server" Text="Nombre"></asp:Label>
          </div>
          <div>
            <asp:TextBox ID="txtnombre" runat="server" CssClass="styleTxtMan"></asp:TextBox>
          </div>
        </div>
        <div>
          <div>
            <asp:Label ID="lblapellido" runat="server" Text="Apellido"></asp:Label>
          </div>
          <div>
            <asp:TextBox ID="txtapellido" runat="server" CssClass="styleTxtMan"></asp:TextBox>
          </div>
        </div>
        <div>
          <div>
            <asp:Label ID="lblemail" runat="server" Text="Email"></asp:Label>
          </div>
          <div>
            <asp:TextBox ID="txtemail" runat="server" CssClass="styleTxtMan"></asp:TextBox>
          </div>
        </div>
        <div>
          <div>
            <asp:Label ID="lbllogin" runat="server" Text="Login"></asp:Label>
          </div>
          <div>
            <asp:TextBox ID="txtlogin" runat="server" CssClass="styleTxtMan"></asp:TextBox>
          </div>
        </div>
        <div>
          <div>
            <asp:Label ID="lblpassword" runat="server" Text="Password"></asp:Label>
          </div>
          <div>
            <asp:TextBox ID="txtpassword" runat="server" TextMode="Password" CssClass="styleTxtMan"></asp:TextBox>
          </div>
        </div>
        <div>
          <div>
            <asp:Label ID="lblestado" runat="server" Text="Estado"></asp:Label>
          </div>
          <div>
            <asp:DropDownList ID="ddlestado" runat="server" CssClass="styleBoxSelect">
              <asp:ListItem Value="V" Text="Vigente"></asp:ListItem>
              <asp:ListItem Value="N" Text="No vigente"></asp:ListItem>
            </asp:DropDownList>
          </div>
        </div>
        <div>
          <div>
            <asp:Label ID="lblTipoUsuario" runat="server" Text="Tipo Usuario"></asp:Label>
          </div>
          <div>
            <asp:DropDownList ID="ddlTipoUsuario" runat="server" CssClass="styleBoxSelect">
              <asp:ListItem Value="1" Text="Debtcontrol"></asp:ListItem>
              <asp:ListItem Value="2" Text="Licenciatario"></asp:ListItem>
              <asp:ListItem Value="3" Text="Mattel"></asp:ListItem>
              <asp:ListItem Value="4" Text="Invitado"></asp:ListItem>
            </asp:DropDownList>
          </div>
        </div>
        <br />
        <div id="idAtrib" runat="server" visible="false">
          <div>
            <div>
              <asp:Label ID="lblRoles" runat="server" Text="Roles"></asp:Label>
            </div>
            <div>
              <asp:RadioButtonList ID="rdbtnlist_roles" runat="server" CssClass="styleBoxSelect">
                <asp:ListItem Text="Mattel" Value="4"></asp:ListItem>
                <asp:ListItem Text="Licenciatario" Value="5"></asp:ListItem>
              </asp:RadioButtonList>
            </div>
          </div>
          <br />
          <div id="idcliente" runat="server" visible="false">
            <div>
              <asp:Label ID="Label1" runat="server" Text="Indique que menú puede ver el usuario"></asp:Label>
            </div>
            <div>
              <asp:DropDownList ID="ddlperfil" runat="server" CssClass="styleBoxSelect">
                <asp:ListItem Value="T" Text="Todos"></asp:ListItem>
                <asp:ListItem Value="F" Text="Facturador"></asp:ListItem>
                <asp:ListItem Value="R" Text="Reportes"></asp:ListItem>
              </asp:DropDownList>
            </div>
            <div><br /></div>
            <div>
              <asp:Label ID="lblusucliente" runat="server" Text="Asocie el usuario a un cliente"></asp:Label>
            </div>
            <div>
              <asp:DropDownList ID="rdCmbCliente" runat="server" CssClass="styleBoxSelect"></asp:DropDownList>
            </div>
          </div>
          <div id="idLicenciatario" runat="server" visible="false">
            <div>
              <asp:Label ID="lblusulicenciatario" runat="server" Text="Asocie el usuario a un licenciatario "></asp:Label>
            </div>
            <div>
              <asp:DropDownList ID="rdCmbLicencitarios" runat="server" CssClass="styleBoxSelect"></asp:DropDownList>
            </div>
          </div>
        </div>
      </div>
    </div>
    <asp:HiddenField ID="hdd_codusuario" runat="server" />
    <br />
    <br />
    <br />
    <br />
  </form>
</body>
</html>
