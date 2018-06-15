<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mcontrato.aspx.cs" Inherits="licenciatarios.mattel.debtcontrol.adm.mcontrato" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <link href="/style/admstyle.css" rel="stylesheet" />
  <link rel="stylesheet" href="/bootstrap/css/bootstrap.min.css" />
  <script src="/bootstrap/js/bootstrap.min.js"></script>
</head>
<body class="bodyadm">
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
    <div class="entorno">
      <div class="panel">
        <div id="divAprobar" runat="server">
          <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" OnClick="btnAprobar_Click" />
        </div>
        <div id="divTerminar" runat="server">
          <asp:Button ID="btnTerminar" runat="server" Text="Terminar Contrato" OnClick="btnTerminar_Click" />
        </div>
        <div id="divAbrir" runat="server">
          <asp:Button ID="btnAbrir" runat="server" Text="Abrir Contrato" OnClick="btnAbrir_Click"/>
        </div>
        <div>
          <asp:Button ID="btnVolver" runat="server" Text="Volver" OnClick="btnVolver_Click" />
        </div>
      </div>
      <div id="divAlert" runat="server" class="box-content-form" visible="false">
        <div class="alert alert-success">
          <strong>ATENCION!!</strong> Usted ha aprobado correctamente el contrato.
        </div>
      </div>
      <div id="divAlertTermino" runat="server" class="box-content-form" visible="false">
        <div class="alert alert-success">
          <strong>ATENCION!!</strong> Usted ha puesto termino al contrato correctamente.
        </div>
      </div>
      <div id="divAlertAbrir" runat="server" class="box-content-form" visible="false">
        <div class="alert alert-success">
          <strong>ATENCION!!</strong> Usted ha abierto el contrato nuevamente.
        </div>
      </div>
      <div class="panel">
        <div class="blq-left-top">
          <div class="blq-left"><span>LICENCIATARIO:</span></div>
          <div>
            <asp:Label ID="lblLicenciatario" runat="server"></asp:Label>
          </div>
        </div>
        <div class="blq-left-top">
          <div class="blq-left"><span>NUMERO DE CONTRATO:</span></div>
          <div>
            <asp:Label ID="lblNoContrato" runat="server"></asp:Label>
          </div>
        </div>
        <div class="blq-left-top">
          <div class="blq-left"><span>TIPO DE CONTRATO:</span></div>
          <div>
            <asp:Label ID="lblTipoContrato" runat="server"></asp:Label>
          </div>
        </div>
        <div>
          <div class="blq-left"><span>CONTRATO:</span></div>
          <div id="PdfContrato" runat="server" visible="false">
            <asp:LinkButton ID="lnkButton" runat="server" Text="Contrato" OnClick="lnkButton_Click"></asp:LinkButton>
          </div>
          <div id="NoPdfContrato" runat="server" visible="false"><span>No existe archivo de contrato</span></div>
        </div>
      </div>
      <div class="panel">
        <div class="blq-left-top">
          <div class="blq-left"><span>FECHA INICIO:</span></div>
          <div>
            <asp:Label ID="lblFechaInicio" runat="server"></asp:Label>
          </div>
        </div>
        <div class="blq-left-top">
          <div class="blq-left"><span>FECHA TERMINO:</span></div>
          <div>
            <asp:Label ID="lblFechaTermino" runat="server"></asp:Label>
          </div>
        </div>
        <div>
          <div class="blq-left"><span>ESTADO:</span></div>
          <div>
            <asp:Label ID="lblEstado" runat="server"></asp:Label>
          </div>
        </div>
      </div>
      <div class="panel">
        <div><span>ADVANCE</span></div>
        <div>
          <telerik:RadGrid ID="rdAdvance" runat="server" ShowStatusBar="True" AutoGenerateColumns="false"
            AllowSorting="True" PageSize="15" AllowPaging="True" GridLines="None" Skin="Windows7" OnNeedDataSource="rdAdvance_NeedDataSource">
            <MasterTableView AutoGenerateColumns="false" ShowFooter="true"
              ShowHeader="true" TableLayout="Fixed" ShowHeadersWhenNoRecords="true">
              <Columns>
                <telerik:GridBoundColumn UniqueName="marca" DataField="marca" HeaderText="Marca">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="categoria" DataField="categoria" HeaderText="Categoria">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="subcategoria" DataField="subcategoria" HeaderText="SubCategoria">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="valor_original" DataField="valor_original" Aggregate="Sum" HeaderText="Valor Saldo Original" DataFormatString="{0:N0}" FooterAggregateFormatString="{0:N0}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Right" />
                  <FooterStyle HorizontalAlign="Right" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="saldo" DataField="saldo" Aggregate="Sum" HeaderText="Saldo" DataFormatString="{0:N0}" FooterAggregateFormatString="{0:N0}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Right" />
                  <FooterStyle HorizontalAlign="Right" />
                </telerik:GridBoundColumn>
              </Columns>
            </MasterTableView>
          </telerik:RadGrid>
        </div>
      </div>
      <div class="panel">
        <div><span>ROYALTY & BDI</span></div>
        <div>
          <telerik:RadGrid ID="rdRoyaltyBDI" runat="server" ShowStatusBar="True" AutoGenerateColumns="false"
            AllowSorting="True" PageSize="15" AllowPaging="True" GridLines="None" Skin="Windows7" OnNeedDataSource="rdRoyaltyBDI_NeedDataSource">
            <MasterTableView AutoGenerateColumns="false"
              ShowHeader="true" TableLayout="Fixed" ShowHeadersWhenNoRecords="true">
              <Columns>
                <telerik:GridBoundColumn UniqueName="marca" DataField="marca" HeaderText="Marca">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="categoria" DataField="categoria" HeaderText="Categoria">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="subcategoria" DataField="subcategoria" HeaderText="SubCategoria">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="Moneda" DataField="Moneda" HeaderText="Moneda">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="Desc. Royalty" DataField="Desc. Royalty" HeaderText="Desc. Royalty">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="Royalty" DataField="Royalty" HeaderText="Royalty" DataFormatString="{0:P}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Right" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="BDI" DataField="BDI" HeaderText="BDI"  DataFormatString="{0:P}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Right" />
                </telerik:GridBoundColumn>
              </Columns>
            </MasterTableView>
          </telerik:RadGrid>
        </div>
      </div>
      <div class="panel">
        <div><span>MINIMO GARANTIZADO</span></div>
        <div>
          <telerik:RadGrid ID="rdMinimo" runat="server" ShowStatusBar="True" AutoGenerateColumns="false"
            AllowSorting="True" PageSize="15" AllowPaging="True" GridLines="None" Skin="Windows7" OnNeedDataSource="rdMinimo_NeedDataSource">
            <MasterTableView AutoGenerateColumns="false"
              ShowHeader="true" TableLayout="Fixed" ShowHeadersWhenNoRecords="true">
              <Columns>
                <telerik:GridBoundColumn UniqueName="marca" DataField="marca" HeaderText="Marca">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="categoria" DataField="categoria" HeaderText="Categoria">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="subcategoria" DataField="subcategoria" HeaderText="SubCategoria">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="fecha_inicio" DataField="fecha_inicio" HeaderText="Fecha Inicio" DataType="System.DateTime" DataFormatString="{0:d/M/yyyy}">
                  <HeaderStyle Font-Size="Smaller" Width="100px" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="fecha_final" DataField="fecha_final" HeaderText="Fecha Final" DataType="System.DateTime" DataFormatString="{0:d/M/yyyy}">
                  <HeaderStyle Font-Size="Smaller" Width="100px" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="minimo" DataField="minimo" HeaderText="Minimo" DataFormatString="{0:N0}">
                  <HeaderStyle Font-Size="Smaller" HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Right" />
                </telerik:GridBoundColumn>
              </Columns>
            </MasterTableView>
          </telerik:RadGrid>
        </div>
      </div>
    </div>
    <div>
    </div>
    <asp:HiddenField ID="num_contrato" runat="server" />
    <asp:HiddenField ID="no_contrato" runat="server" />
    <asp:HiddenField ID="hddpdfcontrato" runat="server" />
  </form>
</body>
</html>
