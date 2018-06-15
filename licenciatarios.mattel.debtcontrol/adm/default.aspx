<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.adm._default" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/style/style.css" rel="stylesheet" />
</head>
<body class="blcontentbienvenida">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <div class="content">
            <div class="in" id="programas">
                <div class="row">
                    <div class="col_md_00">
                        <div class="blloginAdm">
                            <div class="box-login-text">
                                <asp:Label ID="lbl_rut" runat="server" CssClass="lbllogin" Text="Usuario"></asp:Label>
                            </div>
                            <div class="box-login-input">
                                <asp:TextBox ID="txt_rut" CssClass="lbltexboxlogin" runat="server"></asp:TextBox>
                            </div>
                            <div class="box-login-text">
                                <asp:Label ID="lbl_paswword" runat="server" CssClass="lbllogin" Text="Clave"></asp:Label>
                            </div>
                            <div class="box-login-input">
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
        <telerik:RadWindowManager ID="RadWindowManager" runat="server">
            <Windows>
                <telerik:RadWindow runat="server">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
    </form>
</body>
</html>
