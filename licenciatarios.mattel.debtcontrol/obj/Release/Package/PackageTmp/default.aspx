<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol._default" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="style/style.css" rel="stylesheet" />
</head>
<body class="blcontentbienvenida">
  <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Telerik" runat="server"></telerik:RadWindowManager>
    <div class="content">
      <div class="in" id="programas">
        <div class="row">
          <div class="col_md_00">
            <div class="bllogin">
              <div class="box-login-text">
                <asp:Label ID="Label1" runat="server" CssClass="lbllogin" Text="País"></asp:Label>
              </div>
              <div class="box-login-input">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="sVal" ControlToValidate="cmbox_pais" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:DropDownList ID="cmbox_pais" CssClass="lbltexboxlogin" runat="server">
                  <asp:ListItem Text="<< Seleccione País >>" Value=""></asp:ListItem>
                  <asp:ListItem Text="Chile" Value="CL"></asp:ListItem>
                </asp:DropDownList>
              </div>
              <div class="box-login-text">
                <asp:Label ID="lbl_empresa" runat="server" CssClass="lbllogin" Text="Empresa"></asp:Label>
              </div>
              <div class="box-login-input">
                <asp:RequiredFieldValidator ID="valRutEmpresa" runat="server" CssClass="sVal" ControlToValidate="txt_box_empresa" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txt_box_empresa" CssClass="lbltexboxlogin" runat="server"></asp:TextBox>
              </div>
              <div class="box-login-text">
                <asp:Label ID="lbl_rut" runat="server" CssClass="lbllogin" Text="Usuario"></asp:Label>
              </div>
              <div class="box-login-input">
                <asp:RequiredFieldValidator ID="valRut" runat="server" CssClass="sVal" ControlToValidate="txt_rut" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txt_rut" CssClass="lbltexboxlogin" runat="server"></asp:TextBox>
              </div>
              <div class="box-login-text">
                <asp:Label ID="lbl_paswword" runat="server" CssClass="lbllogin" Text="Clave"></asp:Label>
              </div>
              <div class="box-login-input">
                <asp:RequiredFieldValidator ID="valPwd" runat="server" CssClass="sVal" ControlToValidate="txt_password" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txt_password" TextMode="Password" CssClass="lbltexboxlogin" runat="server"></asp:TextBox>
              </div>
              <div class="box-login-text">
                <asp:Button ID="btnAceptar" CssClass="btnLogin" runat="server" Text="Entrar" OnClick="btnAceptar_Click" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </form>
</body>
</html>
