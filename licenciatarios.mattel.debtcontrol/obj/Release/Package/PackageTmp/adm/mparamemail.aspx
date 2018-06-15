<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mparamemail.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.adm.mparamemail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="/style/admstyle.css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="MasterPage">
      <div class="BloqueMenu">
        <div class="lblTitulo">
          <asp:Label ID="lblTitulo" runat="server" Text="CONFIGURACION DE MENSAJES"></asp:Label>
        </div>
        <div class="Botonera">
          <asp:Button ID="btnSave" runat="server" Text="Grabar" OnClick="btnSave_Click" ValidationGroup="ConfigEmail" />
        </div>
      </div>
      <div class="modulo">
        <div class="bloque">
          <div class="lblEmail">
            <asp:Label ID="lblEmail" runat="server" Text="Tipo Email"></asp:Label>
          </div>
          <telerik:radcombobox id="rdCmbEmails" runat="server" autopostback="true" CssClass="styleTxtMan" 
            Width="300px" OnSelectedIndexChanged="rdCmbEmails_SelectedIndexChanged">
          <Items>
            <telerik:RadComboBoxItem Value="R" Text="Recordatorio a Licenciatario para ingresar ventas" />
            <telerik:RadComboBoxItem Value="N" Text="Notificación a Licenciatario del no ingreso de ventas" />
            <telerik:RadComboBoxItem Value="P" Text="Recordatorio a Licenciatario para pagar retención SII" />
            <telerik:RadComboBoxItem Value="E" Text="Listado de Licenciatarios a Ejecutivo" />
            <telerik:RadComboBoxItem Value="L" Text="Envío de correo licenciatarios por facturas generadas" />
            <telerik:RadComboBoxItem Value="A" Text="Alertas de facturas advance y de Q pendientes de aprobar" />
          </Items>
        </telerik:radcombobox>
        </div>
        <div class="modulo">
          <div class="lblNomEmail">
            <asp:Label ID="lblNomEmail" runat="server" Text="Nombre"></asp:Label>
          </div>
          <asp:TextBox ID="txtNomEmail" runat="server" CssClass="styleTxtMan" Width="400px"></asp:TextBox>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNomEmail"
            Display="Static" ErrorMessage="*" ValidationGroup="ConfigEmail"></asp:RequiredFieldValidator>
        </div>
        <div class="modulo">
          <div class="lblAsunto">
            <asp:Label ID="lblAsunto" runat="server" Text="Asunto"></asp:Label>
          </div>
          <asp:TextBox ID="txtAsunto" runat="server" CssClass="styleTxtMan" Width="400px"></asp:TextBox>
          <asp:RequiredFieldValidator ID="valAsunto" runat="server" ControlToValidate="txtAsunto"
            Display="Static" ErrorMessage="*" ValidationGroup="ConfigEmail"></asp:RequiredFieldValidator>
        </div>
        <div class="modulo">
          <div class="lblCuerpo">
            <asp:Label ID="lblCuerpo" runat="server" Text="Cuerpo"></asp:Label>
          </div>
          <telerik:radeditor id="rdCuerpoEmail" runat="server" onclientpastehtml="OnClientPasteHtml">
        <Tools>
          <telerik:EditorToolGroup>
              <telerik:EditorTool Name="Bold" />
              <telerik:EditorTool Name="Italic" />
              <telerik:EditorTool Name="Underline" />
              <telerik:EditorSeparator />
              <telerik:EditorTool Name="ForeColor" />
              <telerik:EditorTool Name="BackColor" />
              <telerik:EditorSeparator />
              <telerik:EditorTool Name="FontName" />
              <telerik:EditorTool Name="RealFontSize" />
            </telerik:EditorToolGroup>
        </Tools>
        <Languages>
          <telerik:SpellCheckerLanguage Code="es-ES" Title="Español" />
        </Languages>
        <Content>
        </Content>
        </telerik:radeditor>
          <asp:RequiredFieldValidator ID="valCuerpoEmail" runat="server" ControlToValidate="rdCuerpoEmail"
            Display="Static" ErrorMessage="*" ValidationGroup="ConfigEmail"></asp:RequiredFieldValidator>
        </div>
      </div>
    </div>
    <asp:HiddenField ID="hdd_accion" runat="server" />
    <script language="JavaScript" type="text/javascript">
      function OnClientPasteHtml(sender, args) {
        var commandName = args.get_commandName();
        var value = args.get_value();
        if (commandName == 'Paste') {
          args.set_value(cleanUpText(value));
        }
      };
    </script>
  </form>
  </form>
</body>
</html>
