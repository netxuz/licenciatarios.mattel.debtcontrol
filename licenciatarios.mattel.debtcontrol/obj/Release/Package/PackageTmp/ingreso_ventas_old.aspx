<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ingreso_ventas_old.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.ingreso_ventas" %>

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
</head>
<body class="body-form">
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Telerik" runat="server"></telerik:RadWindowManager>
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
      function GotoDownloadPage(lngContrato)
      {
        window.location = 'DownloadGrid.ashx?pCodContrato=' + lngContrato;
      }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
      <ContentTemplate>
        <div class="box-titulo">
          <asp:Label ID="lbltitulo" runat="server" Text="INGRESO DE VENTAS"></asp:Label>
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
            <asp:Label ID="lblmesventa" runat="server" Text="Mes de venta:"></asp:Label>
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
            <div class="bx-lbl-filtros">
              <div class="bx-lbl-data">
                <telerik:RadProgressManager ID="RadProgressManager2" runat="server" />
                <asp:Button ID="btnLoadExcel" CssClass="btnGuardar" runat="server" Text="Cargar Archivo" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" OnClick="btnLoadExcel_Click" />
                <telerik:RadProgressArea ID="RadProgressArea2" runat="server"></telerik:RadProgressArea>
              </div>
            </div>
          </div>
          <div class="bx-lbl-data">
            <!-- AllowPaging="True" PageSize="300" -->
            <telerik:RadGrid ID="rdDetalleVenta" runat="server"
              OnNeedDataSource="rdDetalleVenta_NeedDataSource"
              AllowSorting="true" ShowStatusBar="True" GridLines="None"
              AllowAutomaticUpdates="true" AllowAutomaticInserts="true" AllowAutomaticDeletes="true"
              OnItemCreated="rdDetalleVenta_ItemCreated"
              OnInsertCommand="rdDetalleVenta_InsertCommand"
              OnUpdateCommand="rdDetalleVenta_UpdateCommand"
              OnDeleteCommand="rdDetalleVenta_DeleteCommand"
              OnItemDataBound="rdDetalleVenta_ItemDataBound" OnPreRender="rdDetalleVenta_PreRender"
              Skin="Sitefinity">

              <PagerStyle Mode="NextPrevAndNumeric" />
              <MasterTableView DataKeyNames="codigo_detalle" AutoGenerateColumns="false"
                CommandItemDisplay="Top" EditMode="EditForms" ShowHeader="true"
                TableLayout="Fixed" ShowHeadersWhenNoRecords="true">
                <CommandItemSettings AddNewRecordText="Agregar Venta"></CommandItemSettings>
                <Columns>
                  <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                    <ItemStyle CssClass="MyImageButton" />
                  </telerik:GridEditCommandColumn>

                  <telerik:GridTemplateColumn DataField="marca" HeaderText="Marca"
                    UniqueName="marca">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                    <ItemTemplate>
                      <asp:Label ID="lblMarca" runat="server" Text='<%# Eval("marca") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                      <telerik:RadComboBox ID="cmb_marca" runat="server" EnableLoadOnDemand="true"
                        OnItemsRequested="cmb_marca_ItemsRequested" EmptyMessage="<< Seleccione Marca >>">
                      </telerik:RadComboBox>
                    </EditItemTemplate>
                  </telerik:GridTemplateColumn>

                  <telerik:GridTemplateColumn DataField="categoria" HeaderText="categoria"
                    UniqueName="categoria">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                    <ItemTemplate>
                      <asp:Label ID="lblCategoria" runat="server" Text='<%# Eval("categoria") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                      <telerik:RadComboBox ID="cmb_categoria" runat="server" EnableLoadOnDemand="true" EmptyMessage="<< Todas las Categorias >>"
                        OnItemsRequested="cmb_categoria_ItemsRequested" OnClientSelectedIndexChanged="LoadOrderID">
                      </telerik:RadComboBox>
                    </EditItemTemplate>
                  </telerik:GridTemplateColumn>

                  <telerik:GridTemplateColumn DataField="subcategoria" HeaderText="subcategoria"
                    UniqueName="subcategoria">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                    <ItemTemplate>
                      <asp:Label ID="lblSubCategoria" runat="server" Text='<%# Eval("subcategoria") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                      <telerik:RadComboBox ID="cmb_subcategoria" runat="server" EnableLoadOnDemand="true" EmptyMessage="<< Todas las SubCategorias >>"
                        OnItemsRequested="cmb_subcategoria_ItemsRequested" OnClientItemsRequesting="ItemsLoaded">
                      </telerik:RadComboBox>
                    </EditItemTemplate>
                  </telerik:GridTemplateColumn>

                  <telerik:GridBoundColumn DataField="producto" HeaderText="producto"
                    UniqueName="producto" ColumnEditorID="GridTextBoxColumnEditor0">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  </telerik:GridBoundColumn>

                  <telerik:GridBoundColumn DataField="nom_royalty" ReadOnly="true" EditFormHeaderTextFormat="" HeaderText="Desc.Royalty"
                    UniqueName="descroyalty" ColumnEditorID="GridTextBoxColumnEditor1">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  </telerik:GridBoundColumn>

                  <telerik:GridBoundColumn DataField="royalty" ReadOnly="true" EditFormHeaderTextFormat="" HeaderText="Royalty"
                    UniqueName="royalty" ColumnEditorID="GridTextBoxColumnEditor2">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  </telerik:GridBoundColumn>

                  <telerik:GridBoundColumn DataField="bdi" ReadOnly="true" EditFormHeaderTextFormat="" HeaderText="BDI"
                    UniqueName="bdi" ColumnEditorID="GridTextBoxColumnEditor3">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  </telerik:GridBoundColumn>

                  <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente"
                    UniqueName="cliente" ColumnEditorID="GridTextBoxColumnEditor4">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  </telerik:GridBoundColumn>

                  <telerik:GridBoundColumn DataField="sku" HeaderText="SKU"
                    UniqueName="sku" ColumnEditorID="GridTextBoxColumnEditor5">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  </telerik:GridBoundColumn>

                  <telerik:GridBoundColumn DataField="descripcion_producto" HeaderText="Descripcion Producto"
                    UniqueName="descripcion_producto" ColumnEditorID="GridTextBoxColumnEditor6">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  </telerik:GridBoundColumn>

                  <telerik:GridTemplateColumn DataField="precio_uni_venta_bruta" HeaderText="Precio Unitario Venta Bruta"
                    UniqueName="precio_uni_venta_bruta">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                    <ItemTemplate>
                      <telerik:RadTextBox ID="txt_lbl_PrecioUniBruto" runat="server" Text='<%# Eval("precio_uni_venta_bruta") %>'></telerik:RadTextBox>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red" ErrorMessage="Falta completar campos con * que son obligatorios" Text="*" ControlToValidate="txt_lbl_PrecioUniBruto" Display="Dynamic" ValidationGroup="ValidPage"></asp:RequiredFieldValidator>
                      <asp:CompareValidator ID="CompareValidator4" ControlToValidate="txt_lbl_PrecioUniBruto" ForeColor="Red" Type="Double" ValidationGroup="ValidPage" runat="server" ErrorMessage="Solo se permiten numeros" Text="*" Operator="datatypecheck"></asp:CompareValidator>
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                        ControlToValidate="txt_lbl_PrecioUniBruto"
                        ErrorMessage="Solo numeros positivos." ForeColor="Red"
                        ValidationExpression="\d*\,?\d*" ValidationGroup="ValidPage" Text="*">
                      </asp:RegularExpressionValidator>
                    </ItemTemplate>
                    <EditItemTemplate>
                      <telerik:RadTextBox ID="txt1" runat="server" Text=''></telerik:RadTextBox>

                    </EditItemTemplate>
                  </telerik:GridTemplateColumn>

                  <telerik:GridTemplateColumn DataField="cantidad_venta_bruta" HeaderText="Q Venta Bruta" UniqueName="cantidad_venta_bruta">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                    <ItemTemplate>
                      <telerik:RadTextBox ID="txt_lblCantidadVentaBruta" runat="server" Text='<%# Eval("cantidad_venta_bruta") %>'></telerik:RadTextBox>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red" ErrorMessage="Falta completar campos con * que son obligatorios" Text="*" ControlToValidate="txt_lblCantidadVentaBruta" Display="Dynamic" ValidationGroup="ValidPage"></asp:RequiredFieldValidator>
                      <asp:CompareValidator ID="CompareValidator3" ControlToValidate="txt_lblCantidadVentaBruta" ForeColor="Red" Type="Double" ValidationGroup="ValidPage" runat="server" ErrorMessage="Solo se permiten numeros" Text="*" Operator="datatypecheck"></asp:CompareValidator>
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                        ControlToValidate="txt_lblCantidadVentaBruta"
                        ErrorMessage="Solo numeros positivos." ForeColor="Red"
                        ValidationExpression="\d*\,?\d*" ValidationGroup="ValidPage" Text="*">
                      </asp:RegularExpressionValidator>
                    </ItemTemplate>
                    <EditItemTemplate>
                      <telerik:RadTextBox ID="txt2" runat="server" Text=''></telerik:RadTextBox>

                    </EditItemTemplate>
                  </telerik:GridTemplateColumn>

                  <telerik:GridTemplateColumn DataField="precio_unit_descue_devol" HeaderText="Precio Unitario Descuento / Devolución"
                    UniqueName="precio_unit_descue_devol">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                    <ItemTemplate>
                      <telerik:RadTextBox ID="txt_lbl_PrecioUnitDescueDevol" runat="server" Text='<%# Eval("precio_unit_descue_devol") %>'></telerik:RadTextBox>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red" ErrorMessage="Falta completar campos con * que son obligatorios" Text="*" ControlToValidate="txt_lbl_PrecioUnitDescueDevol" Display="Dynamic" ValidationGroup="ValidPage"></asp:RequiredFieldValidator>
                      <asp:CompareValidator ID="CompareValidator2" ControlToValidate="txt_lbl_PrecioUnitDescueDevol" ForeColor="Red" Type="Double" ValidationGroup="ValidPage" runat="server" ErrorMessage="Solo se permiten numeros" Text="*" Operator="datatypecheck"></asp:CompareValidator>
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                        ControlToValidate="txt_lbl_PrecioUnitDescueDevol"
                        ErrorMessage="Solo numeros positivos." ForeColor="Red"
                        ValidationExpression="\d*\,?\d*" ValidationGroup="ValidPage" Text="*">
                      </asp:RegularExpressionValidator>
                    </ItemTemplate>
                    <EditItemTemplate>
                      <telerik:RadTextBox ID="txt3" runat="server" Text=''></telerik:RadTextBox>

                    </EditItemTemplate>
                  </telerik:GridTemplateColumn>

                  <telerik:GridTemplateColumn DataField="cantidad_descue_devol" HeaderText="Q Descuento / Devolución" UniqueName="cantidad_descue_devol">
                    <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                    <ItemTemplate>
                      <telerik:RadTextBox ID="txt_lblCantidadDescueDevol" runat="server" Text='<%# Eval("cantidad_descue_devol") %>'></telerik:RadTextBox>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red" ErrorMessage="Falta completar campos con * que son obligatorios" Text="*" ControlToValidate="txt_lblCantidadDescueDevol" Display="Dynamic" ValidationGroup="ValidPage"></asp:RequiredFieldValidator>
                      <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txt_lblCantidadDescueDevol" ForeColor="Red" Type="Double" ValidationGroup="ValidPage" runat="server" ErrorMessage="Solo se permiten numeros" Text="*" Operator="datatypecheck"></asp:CompareValidator>
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ControlToValidate="txt_lblCantidadDescueDevol"
                        ErrorMessage="Solo numeros positivos." ForeColor="Red"
                        ValidationExpression="\d*\,?\d*" ValidationGroup="ValidPage" Text="*">
                      </asp:RegularExpressionValidator>
                    </ItemTemplate>
                    <EditItemTemplate>
                      <telerik:RadTextBox ID="txt4" runat="server" Text=''></telerik:RadTextBox>

                    </EditItemTemplate>
                  </telerik:GridTemplateColumn>

                  <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Are you sure you want to delete this row?"
                    Text="Delete" UniqueName="column" HeaderStyle-Width="35px" ButtonType="ImageButton" ButtonCssClass="MyImageButton">
                  </telerik:GridButtonColumn>
                </Columns>
              </MasterTableView>
            </telerik:RadGrid>
            <telerik:GridDropDownListColumnEditor ID="GridDropDownColumnEditor1" runat="server" DataTextField="cod_marca" DataValueField="descripcion" />
            <telerik:GridDropDownListColumnEditor ID="GridDropDownColumnEditor2" runat="server" DataTextField="cod_categoria" DataValueField="descripcion" />
            <telerik:GridDropDownListColumnEditor ID="GridDropDownColumnEditor3" runat="server" DataTextField="cod_subcategoria" DataValueField="descripcion" />
            <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor0" runat="server" />
            <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor1" runat="server" />
            <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor2" runat="server" />
            <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor3" runat="server" />
            <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor4" runat="server" />
            <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor5" runat="server" />
            <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor6" runat="server" />
            <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor7" runat="server" />
            <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor8" runat="server" />
            <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor9" runat="server" />
            <telerik:GridTextBoxColumnEditor ID="GridTextBoxColumnEditor10" runat="server" />

          </div>
          <div class="bx-lbl-title">
            <asp:CheckBox ID="chkbox_declaracion" Text="Declaro que el archivo cargado corresponde a las ventas de los productos licenciados de Mattel para el periodo señalado."
              runat="server" />
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Debe seleccionar si declarar o no el periodo." Text="" ClientValidationFunction="ValidateCheckBox" Display="Dynamic" ValidationGroup="ValidPage"></asp:CustomValidator>
          </div>
          <div class="bx-lbl-title">
            <asp:CheckBox ID="chkbox_nodeclaracion" Text="Declaro sin movimiento el periodo seleccionado." runat="server" />
          </div>
          <div class="bx-lbl-btn">
            <asp:Button ID="btnGuardar" CssClass="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" ValidationGroup="ValidPage" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="ValidPage" />
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
    <asp:HiddenField ID="hddCodReporteVenta" runat="server" />
    <script type="text/javascript">
      function ValidateCheckBox(sender, args) {
        if ((document.getElementById('<%=chkbox_declaracion.ClientID %>').checked == true) && (document.getElementById('<%=chkbox_nodeclaracion.ClientID %>').checked == true)) {
          args.IsValid = false;
        } else if ((document.getElementById('<%=chkbox_declaracion.ClientID %>').checked == false) && (document.getElementById('<%=chkbox_nodeclaracion.ClientID %>').checked == false)) {
          args.IsValid = false;
        } else {
          args.IsValid = true;
        }
      }
      var codcategoria;
      function LoadOrderID(sender, eventArgs) {
        var item = eventArgs.get_item();
        codcategoria = item.get_value();
      }
      function ItemsLoaded(sender, eventArgs) {
        eventArgs.get_context().Text = codcategoria;
      }
      function doNothing() {
        var obj = document.getElementById("tipo_cambio")
        obj.click();
        return false;
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
