<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testmail.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.testmail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="style/style.css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <div class="box-content-form">
      <div class="bx-lbl-title">
        <asp:Label ID="lblcontrato" runat="server" Text="HOST"></asp:Label>
      </div>
      <div class="bx-lbl-data">
        <asp:TextBox ID="txt_host" CssClass="styleTxt" runat="server"></asp:TextBox>
      </div>

      <div class="bx-lbl-title">
        <asp:Label ID="Label1" runat="server" Text="De"></asp:Label>
      </div>
      <div class="bx-lbl-data">
        <asp:TextBox ID="txt_de" CssClass="styleTxt" runat="server"></asp:TextBox>
      </div>

      <div class="bx-lbl-title">
        <asp:Label ID="Label3" runat="server" Text="Login"></asp:Label>
      </div>
      <div class="bx-lbl-data">
        <asp:TextBox ID="txt_login" CssClass="styleTxt" runat="server"></asp:TextBox>
      </div>

      <div class="bx-lbl-title">
        <asp:Label ID="Label4" runat="server" Text="Password"></asp:Label>
      </div>
      <div class="bx-lbl-data">
        <asp:TextBox ID="txt_pwd" CssClass="styleTxt" runat="server"></asp:TextBox>
      </div>

      <div class="bx-lbl-title">
        <asp:Label ID="Label2" runat="server" Text="Para"></asp:Label>
      </div>
      <div class="bx-lbl-data">
        <asp:TextBox ID="txt_para" CssClass="styleTxt" runat="server"></asp:TextBox>
      </div>
      
      <div class="bx-lbl-title">
        <asp:TextBox ID="txtcomentarios" runat="server" Rows="7" Columns="70" TextMode="MultiLine" CssClass="styleTxtObs"></asp:TextBox>
      </div>

      <div class="bx-lbl-btn">
        <asp:Button ID="BtngEnviar" CssClass="btnGuardar" runat="server" Text="Enviar" OnClick="BtngEnviar_Click" />
      </div>
      <div class=".bx-lbl-alert" id="box_msn" runat="server">
        <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
      </div>
    </div>
  </form>
</body>
</html>
