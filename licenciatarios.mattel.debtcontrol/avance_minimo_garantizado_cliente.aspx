<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="avance_minimo_garantizado_cliente.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.avance_minimo_garantizado_cliente" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="style/style.css" rel="stylesheet" />
  <script type="text/javascript" src="/Resources/lib/jquery-1.10.1.min.js"></script>
  <script type="text/javascript" src="/Resources/lib/jquery.mousewheel-3.0.6.pack.js"></script>
  <script type="text/javascript" src="/Resources/source/jquery.fancybox.js?v=2.1.5"></script>
  <link rel="stylesheet" type="text/css" href="/Resources/source/jquery.fancybox.css?v=2.1.5" media="screen" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
      <ContentTemplate>
        <div class="box-titulo">
          <asp:Label ID="lbltitulo" runat="server" Text="MÍNIMO GARANTIZADO"></asp:Label>
        </div>
        <div class="box-content-form">
          <div class="bx-lbl-data">
            <div class="bx-lbl-filtros">
              <div class="bx-lbl-title">
                <asp:Label ID="lblLicenciatario" runat="server" Text="Licenciatario:"></asp:Label>
              </div>
              <div class="bx-lbl-data">
                <asp:DropDownList ID="cmbox_licenciatario" CssClass="box-select" runat="server" OnSelectedIndexChanged="cmbox_licenciatario_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
              </div>
            </div>
            <div class="bx-lbl-filtros">
              <div class="bx-lbl-title">
                <asp:Label ID="lblcontrato" runat="server" Text="Contrato:"></asp:Label>
              </div>
              <div class="bx-lbl-data">
                <asp:DropDownList ID="cmbox_contrato" CssClass="box-select" runat="server" OnSelectedIndexChanged="cmbox_contrato_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
              </div>
            </div>
            <div class="bx-lbl-filtros">
              <div class="bx-lbl-title">
                <asp:Label ID="lblPeriodo" runat="server" Text="Periodo:"></asp:Label>
              </div>
              <div class="bx-lbl-data">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="sVal" ControlToValidate="cmbox_periodo_minimo" ErrorMessage="*" ValidationGroup="ValidPage"></asp:RequiredFieldValidator>
                <asp:DropDownList ID="cmbox_periodo_minimo" CssClass="box-select" runat="server">
                  <asp:ListItem Text="<< Seleccione Año >>" Value="0"></asp:ListItem>
                </asp:DropDownList>
              </div>
            </div>
          </div>
          <div class="bx-lbl-data">
            <asp:Button ID="btnBuscar" CssClass="btnGuardar" runat="server" Text="Aceptar" OnClick="btnBuscar_Click" ValidationGroup="ValidPage" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="False" ShowSummary="False" ValidationGroup="ValidPage" />
          </div>
          <div class="bx-lbl-data" id="idGrilla" runat="server" visible="false">
          </div>
        </div>
        <div class="box-pie">
          <asp:Button ID="btnInstructivo" runat="server" CssClass="btnPie" Text="Instructivo" OnClick="btnInstructivo_Click" />
          <asp:Button ID="btnContacto" runat="server" CssClass="btnPie" Text="Contacto" OnClick="btnContacto_Click" />
          <input type="button" class="btnPie" value="Tipo Cambio" onclick="doNothing();" />
          <a id="tipo_cambio" href="tipo_cambio.aspx"></a>
        </div>
        <div class="box-pie-contactos">
          <div class="box-img-foot">
            <img src="images/logo-foot.png" />
          </div>
          <div class="bx-lbl-foot">
            <asp:Label ID="Label2" runat="server" Text="Teléfono: +562 25996100 | Email: licenciatariosmattel@debtcontrol.cl"></asp:Label>
          </div>
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
      function doNothing() {
        var obj = document.getElementById("tipo_cambio")
        obj.click();
        return false;
        alert('paso');
      }

      $(document).ready(function () {
        $("a#tipo_cambio").fancybox({
          'autoDimensions': false,
          'width': 500,
          'height': 800,
          'transitionIn': 'elastic',
          'transitionOut': 'elastic',
          'type': 'iframe'
        });
      });

    </script>
  </form>
</body>
</html>
