<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ingreso_ventas.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.ingreso_ventas1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="style/style.css" rel="stylesheet" />
  <script type="text/javascript" src="/Resources/lib/jquery-1.10.1.min.js"></script>
  <script type="text/javascript" src="/Resources/lib/jquery.mousewheel-3.0.6.pack.js"></script>
  <script type="text/javascript" src="/Resources/source/jquery.fancybox.js?v=2.1.5"></script>
  <link rel="stylesheet" type="text/css" href="/Resources/source/jquery.fancybox.css?v=2.1.5" media="screen" />
  <link rel="stylesheet" href="/bootstrap/css/bootstrap.min.css" />
  <script src="/bootstrap/js/bootstrap.min.js"></script>
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
    <script type="text/javascript">
      Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
      Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
      function BeginRequestHandler(sender, args) {
        var elem = args.get_postBackElement();
        if (elem.id == 'ImgBtn') {
          //obj.style.display = 'none';
        }
      }
      function EndRequestHandler(sender, args) {
        //obj.style.display = 'none';
      }
      function GotoDownloadPage(lngContrato) {
        window.location = 'DownloadGrid.ashx?pCodContrato=' + lngContrato;
      }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
      <ContentTemplate>
        <div class="row">
          <div class="col-md-4">
            <div class="stepconteiner">
              <div class="stepactive"><span class="numberactive">1</span></div>
              <div class="box-stepactive">
                <asp:Label ID="lbltitulo" runat="server" Text="INGRESO DE VENTAS"></asp:Label>
                <span class="glyphicon glyphicon-share-alt"></span>
              </div>
            </div>
          </div>
          <div class="col-md-4">
            <div class="stepconteiner">
              <div class="stepNOactive"><span class="numberNOactive">2</span></div>
              <div class="box-stepNOactive">
                <asp:Label ID="Label5" runat="server" Text="CONFIRMACIÓN INGRESO DE VENTAS"></asp:Label>
                <span class="glyphicon glyphicon-share-alt"></span>
              </div>
            </div>
          </div>
          <div class="col-md-4">
            <div class="stepconteiner">
              <div class="stepNOactive"><span class="numberNOactive">3</span></div>
              <div class="box-stepNOactive">
                <asp:Label ID="Label6" runat="server" Text="VENTAS DECLARADAS CORRECTAMENTE"></asp:Label>
                <span class="glyphicon glyphicon-thumbs-up"></span>
              </div>
            </div>
          </div>
        </div>
        <div class="box-content-form">
          <div class="bx-lbl-title">
            <asp:Label ID="lblcontrato" runat="server" Text="Contrato:"></asp:Label>
          </div>
          <div class="bx-lbl-data">
            <asp:DropDownList ID="cmbox_contrato" CssClass="box-select" runat="server" OnSelectedIndexChanged="cmbox_contrato_SelectedIndexChanged" AutoPostBack="True">
              <asp:ListItem Text="<< Selecciones Contrato >>" Value="0"></asp:ListItem>
            </asp:DropDownList>
          </div>

          <div class="bx-lbl-title">
            <asp:Label ID="lblmesventa" runat="server" Text="Año de venta:"></asp:Label>
          </div>
          <div class="bx-lbl-data">
            <asp:Label ID="lblMesdeVenta" runat="server" Text=""></asp:Label>
            <asp:HiddenField ID="hddmesventa" runat="server" />
          </div>

          <div class="bx-lbl-title">
            <asp:Label ID="Label4" runat="server" Text="Mes de venta:"></asp:Label>
          </div>
          <div class="bx-lbl-data">
            <asp:DropDownList ID="ddlmesventa" CssClass="box-select" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlmesventa_SelectedIndexChanged">
            </asp:DropDownList>
          </div>

          <div class="bx-lbl-title">
            <asp:Label ID="lblarchivo" runat="server" Text="Cargar archivo de venta:"></asp:Label>
          </div>
          <div class="bx-lbl-data">
            <div class="bx-lbl-filtros">
              <div class="bx-lbl-data">
                <div class="box-file">
                  <telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
                  <telerik:RadAsyncUpload ID="RadAsyncUpload1" runat="server" MaxFileInputsCount="1"
                    PostbackTriggers="btnLoadExcel" Skin="Sitefinity" AllowedFileExtensions="csv"
                    TargetFolder="~/UploadTemp">
                  </telerik:RadAsyncUpload>
                  <telerik:RadProgressArea ID="RadProgressArea1" runat="server"></telerik:RadProgressArea>
                </div>
              </div>
            </div>
            <div class="bx-lbl-filtros">
              <div class="bx-lbl-data">
                <asp:ImageButton ID="ImgBtn" ImageUrl="~/images/image226.png" Width="30px" ToolTip="Descarga planilla de ventas" runat="server" OnClick="ImgBtn_Click" />
              </div>
            </div>
            <div class="bx-lbl-filtros">
              <div class="bx-lbl-title">
                <asp:Label ID="Label1" runat="server" Text="Descargar archivo"></asp:Label>
              </div>
            </div>
          </div>
          <div class="bx-lbl-data">
            <div class="bx-lbl-data">
              <telerik:RadProgressManager ID="RadProgressManager2" runat="server" />
              <asp:Button ID="btnLoadExcel" CssClass="btnGuardar" runat="server" Text="Cargar Archivo" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" OnClick="btnLoadExcel_Click" />
              <asp:Button ID="Button1" runat="server" CssClass="btnDeclaraNoMov" Text="Declarar mes sin movimientos" OnClientClick="doSinMovimientos();" />
              <telerik:RadProgressArea ID="RadProgressArea2" runat="server"></telerik:RadProgressArea>
            </div>
          </div>
        </div>
        <div class="box-pie">
          <asp:Button ID="btnInstructivo" runat="server" CssClass="btnPie" Text="Instructivo" OnClick="btnInstructivo_Click" />
          <asp:Button ID="btnContacto" runat="server" CssClass="btnPie" Text="Contacto" OnClick="btnContacto_Click" />
          <asp:Button ID="btnTCambio" runat="server" CssClass="btnPie" Text="Tipo Cambio" OnClientClick="doNothing();" />
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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
      <ProgressTemplate>
        <div id="IMGDIV" align="center" valign="middle" runat="server" style="position: absolute; left: 35%; top: 25%; visibility: visible; vertical-align: middle; border-style: inset; border-color: silver; background-color: White">
          <img src="/images/loading01.gif" /><br />
        </div>
      </ProgressTemplate>
    </asp:UpdateProgress>


    <div id="myModal" class="modal fade" role="dialog">
      <div class="modal-dialog">

        <!-- Modal content-->
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal">&times;</button>
            <div class="box-titulo-modal">
              <asp:Label ID="Label3" runat="server" Text="DECLARAR MES SIN MOVIMIENTOS"></asp:Label>
            </div>
          </div>
          <div class="box-content-form">
            <div class="bx-lbl-btn">
              <asp:CheckBox ID="chkbox_nodeclaracion" Text="Declaro sin movimiento el periodo seleccionado." runat="server" />
              <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Debe aceptar las condiciones y luego presionar el botón Autorizar." Text="" ClientValidationFunction="ValidateCheckBox" Display="Dynamic" ValidationGroup="ValidPage"></asp:CustomValidator>
            </div>
            <div class="bx-lbl-btn">
              <asp:Button ID="btnAutorizar" CssClass="btnGuardar" runat="server" Text="Declarar" ValidationGroup="ValidPage" OnClick="btnAutorizar_Click" />
              <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="ValidPage" />
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
          </div>
        </div>

      </div>
    </div>

    <div id="ModalError" class="modal fade" role="dialog">
      <div class="modal-dialog">

        <!-- Modal content-->
        <div class="modal-content">
          <div class="modal-header-error" style="padding: 35px 50px;">
            <button type="button" class="close" data-dismiss="modal">&times;</button>
            <h4><span class="glyphicon glyphicon-alert"></span>Alerta</h4>
          </div>
          <div class="modal-body-error" style="padding: 40px 50px;">
            <h4>Debe seleccionar el contrato y el mes para declarar el periodo.</h4>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
          </div>
        </div>

      </div>
    </div>


    <asp:HiddenField ID="hddCodReporteVenta" runat="server" />
    <script type="text/javascript">
      function ValidateCheckBox(sender, args) {
        if ((document.getElementById('<%=chkbox_nodeclaracion.ClientID %>').checked == false)) {
          args.IsValid = false;
        } else {
          args.IsValid = true;
        }
      }

      function doNothing() {
        var obj = document.getElementById("tipo_cambio")
        obj.click();
        return false;
      }

      function doSinMovimientos() {
        if (($('#cmbox_contrato option:selected').val() == "0") || ($('#ddlmesventa option:selected').val()) == "") {
          $("#ModalError").modal();
        } else {
          $("#myModal").modal();
        }

        return false;
      }

      function goConfirm() {
        document.forms[0].action = 'confirmacion_ingreso_ventas.aspx';
        document.forms[0].submit();
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
