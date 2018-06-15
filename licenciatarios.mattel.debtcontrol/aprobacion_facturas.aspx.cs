using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;
using Telerik.Web.UI;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class aprobacion_facturas : System.Web.UI.Page
  {
    string[] arrayMes;
    string[] arrayReporteVenta;
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
      if (!IsPostBack)
      {
        lblNoContrato.Text = oWeb.GetData("pNoContrato");
        hdd_no_contrato.Value = oWeb.GetData("pNoContrato");
        hdd_num_contrato.Value = oWeb.GetData("pNumContrato");
        hdd_periodo.Value = oWeb.GetData("pPeriodo");
        hdd_ano_periodo.Value = oWeb.GetData("pAnoPeriodo");

      }
      getDataQInvoce(hdd_periodo.Value, hdd_ano_periodo.Value);
    }

    protected void getDataQInvoce(string sQ, string sYear)
    {
      detFactura oFactura = new detFactura();
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        string arrMes = string.Empty;
        string arrReporteVenta = string.Empty;
        cReporteVenta oReportForInvoce = new cReporteVenta(ref oConn);
        oReportForInvoce.NumContrato = hdd_num_contrato.Value;
        oReportForInvoce.Periodo = sQ;
        oReportForInvoce.AnoReporte = sYear;
        DataTable dtReporteVenta = oReportForInvoce.GetReportForInvoice();
        foreach (DataRow oRowReporte in dtReporteVenta.Rows)
        {
          arrReporteVenta = (string.IsNullOrEmpty(arrReporteVenta) ? oRowReporte["cod_reporte_venta"].ToString() : arrReporteVenta + ";" + oRowReporte["cod_reporte_venta"].ToString());
          arrMes = (string.IsNullOrEmpty(arrMes) ? oRowReporte["mes_reporte"].ToString() : arrMes + ";" + oRowReporte["mes_reporte"].ToString());
        }
        arrayReporteVenta = arrReporteVenta.Split(';');
        arrayMes = arrMes.Split(';');

        string sNomMesUno = oWeb.getMes(int.Parse(arrayMes[0].ToString())).ToString();
        string sNomMesDos = oWeb.getMes(int.Parse(arrayMes[1].ToString())).ToString();
        string sNomMesTres = oWeb.getMes(int.Parse(arrayMes[2].ToString())).ToString();

        oFactura.MesNomUno = sNomMesUno;
        oFactura.MesNomDos = sNomMesDos;
        oFactura.MesNomTres = sNomMesTres;
        oFactura.getMakeTable();

        cDetalleVenta oDetalleVenta;

        //cProductosContrato oProductosContrato = new cProductosContrato(ref oConn);
        //oProductosContrato.NumContrato = hdd_num_contrato.Value;

        cRoyaltyContrato oRoyaltyContrato = new cRoyaltyContrato(ref oConn);
        oRoyaltyContrato.NumContrato = hdd_num_contrato.Value;

        DataTable dtProdCont = oRoyaltyContrato.GetByInvoce();
        if (dtProdCont != null)
        {
          foreach (DataRow oRow in dtProdCont.Rows)
          {
            bool bVenta = false;
            string pCodRotalty = string.Empty;
            double sRoyalty = 0;
            double sBdi = 0;
            string sPeriodo = string.Empty;
            string arrMntMes = string.Empty;

            foreach (DataRow oRowReporte in dtReporteVenta.Rows)
            {

              sPeriodo = oRowReporte["periodo_q"].ToString() + "/" + oRowReporte["ano_reporte"].ToString();

              oDetalleVenta = new cDetalleVenta(ref oConn);
              oDetalleVenta.CodigoReporteVenta = oRowReporte["cod_reporte_venta"].ToString();
              oDetalleVenta.Marca = oRow["cod_marca"].ToString();
              oDetalleVenta.Categoria = oRow["cod_categoria"].ToString();
              oDetalleVenta.SubCategoria = oRow["cod_subcategoria"].ToString();
              oDetalleVenta.CodRoyalty = oRow["cod_royalty"].ToString();
              DataTable detVenta = oDetalleVenta.GetByFactura();
              if (detVenta != null)
              {
                if (detVenta.Rows.Count > 0)
                {
                  //int intPrecioUni = int.Parse(detVenta.Rows[0]["precio_uni"].ToString());
                  //int intCantidadVenta = int.Parse(detVenta.Rows[0]["cantidad_venta"].ToString());
                  //int intPrecioUniDevol = int.Parse(detVenta.Rows[0]["precio_uni_devol"].ToString());
                  //int intCantDescueDevol = int.Parse(detVenta.Rows[0]["cant_descue_devol"].ToString());
                  bVenta = true;
                  pCodRotalty = detVenta.Rows[0]["cod_royalty"].ToString();
                  sRoyalty = double.Parse(detVenta.Rows[0]["royalty"].ToString());
                  sBdi = double.Parse(detVenta.Rows[0]["bdi"].ToString());

                  //int intTotalMes = ((intPrecioUni * intCantidadVenta) - (intPrecioUniDevol * intCantDescueDevol));
                  double intTotalMes = double.Parse(detVenta.Rows[0]["totalmes"].ToString());

                  arrMntMes = (string.IsNullOrEmpty(arrMntMes) ? intTotalMes.ToString() : arrMntMes + ";" + intTotalMes.ToString());
                }
                else
                {
                  arrMntMes = (string.IsNullOrEmpty(arrMntMes) ? "0" : arrMntMes + ";0");
                }
              }
              detVenta = null;
            }

            if (bVenta)
            {
              string[] arrayMntMes = arrMntMes.Split(';');


              oFactura.Marca = oRow["cod_marca"].ToString();
              oFactura.Categoria = oRow["cod_categoria"].ToString();
              oFactura.SubCategoria = oRow["cod_subcategoria"].ToString();
              oFactura.MesUno = arrayMntMes[0].ToString();
              oFactura.MesDos = arrayMntMes[1].ToString();
              oFactura.MesTres = arrayMntMes[2].ToString();
              oFactura.CodRoyalty = pCodRotalty;
              oFactura.Royalty = sRoyalty.ToString();
              oFactura.Bdi = sBdi.ToString();
              oFactura.Periodo = sPeriodo;

              double MntRoyUno = double.Parse(arrayMntMes[0].ToString()) / getValueUsd(arrayMes[0].ToString(), sYear, pCodRotalty);
              double MntRoyDos = double.Parse(arrayMntMes[1].ToString()) / getValueUsd(arrayMes[1].ToString(), sYear, pCodRotalty);
              double MntRoyTres = double.Parse(arrayMntMes[2].ToString()) / getValueUsd(arrayMes[2].ToString(), sYear, pCodRotalty);

              double MntTotal = MntRoyUno + MntRoyDos + MntRoyTres;

              oFactura.MontoRoyaltyUsd = (MntTotal * sRoyalty).ToString();

              double MntDbiUno = ((double.Parse(arrayMntMes[0].ToString()) / getValueUsd(arrayMes[0].ToString(), sYear, pCodRotalty)) * (sBdi));
              double MntDbiDos = ((double.Parse(arrayMntMes[1].ToString()) / getValueUsd(arrayMes[1].ToString(), sYear, pCodRotalty)) * (sBdi));
              double MntDbiTres = ((double.Parse(arrayMntMes[2].ToString()) / getValueUsd(arrayMes[2].ToString(), sYear, pCodRotalty)) * (sBdi));

              oFactura.MontoDbiUsd = (MntDbiUno + MntDbiDos + MntDbiTres).ToString();

              string iSaldo = string.Empty;
              oFactura.SaldoAdvanceUsd = "0";
              oFactura.Saldo = "0";
              cAdvanceContrato oAdvanceContrato = new cAdvanceContrato(ref oConn);
              oAdvanceContrato.NumContrato = hdd_num_contrato.Value;
              oAdvanceContrato.CodMarca = oRow["cod_marca"].ToString();
              oAdvanceContrato.CodCategoria = oRow["cod_categoria"].ToString();
              oAdvanceContrato.CodSubCategoria = oRow["cod_subcategoria"].ToString();
              oAdvanceContrato.Facturado = true;
              DataTable dtAdvanceSaldo = oAdvanceContrato.Get();
              if (dtAdvanceSaldo != null)
              {
                if (dtAdvanceSaldo.Rows.Count > 0)
                {
                  if (string.IsNullOrEmpty(dtAdvanceSaldo.Rows[0]["saldo"].ToString()))
                  {
                    if (double.Parse(oFactura.MontoRoyaltyUsd) > double.Parse(dtAdvanceSaldo.Rows[0]["valor_original"].ToString()))
                    {
                      oFactura.SaldoAdvanceUsd = double.Parse(dtAdvanceSaldo.Rows[0]["valor_original"].ToString()).ToString();
                      oFactura.Saldo = "0";
                    }
                    else
                    {
                      if (double.Parse(oFactura.MontoRoyaltyUsd) > 0)
                      {
                        oFactura.SaldoAdvanceUsd = double.Parse(oFactura.MontoRoyaltyUsd).ToString();
                        oFactura.Saldo = (double.Parse(dtAdvanceSaldo.Rows[0]["valor_original"].ToString()) - double.Parse(oFactura.MontoRoyaltyUsd)).ToString();
                      }
                      else {
                        oFactura.SaldoAdvanceUsd = "0";
                        oFactura.Saldo = "0";
                      }
                    }
                  }
                  else if (double.Parse(oFactura.MontoRoyaltyUsd) > double.Parse(dtAdvanceSaldo.Rows[0]["saldo"].ToString()))
                  {
                    oFactura.SaldoAdvanceUsd = (double.Parse(dtAdvanceSaldo.Rows[0]["saldo"].ToString())).ToString();
                    oFactura.Saldo = "0";
                  }
                  else
                  {
                    if (double.Parse(oFactura.MontoRoyaltyUsd) > 0)
                    {
                      oFactura.SaldoAdvanceUsd = double.Parse(oFactura.MontoRoyaltyUsd).ToString();
                      oFactura.Saldo = (double.Parse(dtAdvanceSaldo.Rows[0]["saldo"].ToString()) - double.Parse(oFactura.MontoRoyaltyUsd)).ToString();
                    }
                    else {
                      oFactura.SaldoAdvanceUsd = "0";
                      oFactura.Saldo = "0";
                    }
                  }
                }
              }
              dtAdvanceSaldo = null;

              oFactura.FacturaUsd = (double.Parse(oFactura.MontoRoyaltyUsd) + double.Parse(oFactura.MontoDbiUsd) - double.Parse(oFactura.SaldoAdvanceUsd)).ToString();

              oFactura.AddRow();
            }
          }
        }
        dtReporteVenta = null;
        dtProdCont = null;

        DataTable dtFactura = oFactura.Get();

        RadGrid oGridFractura = new RadGrid();
        oGridFractura.ID = "rdGridFactura";
        oGridFractura.ShowStatusBar = true;
        oGridFractura.ShowFooter = true;
        oGridFractura.AutoGenerateColumns = false;
        oGridFractura.Skin = "Sitefinity";
        oGridFractura.ItemDataBound += oGridFractura_ItemDataBound;
        oGridFractura.ItemCommand += oGridFractura_ItemCommand;


        oGridFractura.ExportSettings.HideStructureColumns = true;

        oGridFractura.MasterTableView.AutoGenerateColumns = false;
        oGridFractura.MasterTableView.ShowHeader = true;
        oGridFractura.MasterTableView.TableLayout = GridTableLayout.Fixed;
        oGridFractura.MasterTableView.ShowHeadersWhenNoRecords = true;
        oGridFractura.MasterTableView.ShowFooter = true;
        oGridFractura.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
        oGridFractura.MasterTableView.CommandItemSettings.ShowExportToExcelButton = true;
        oGridFractura.MasterTableView.CommandItemSettings.ShowRefreshButton = false;
        oGridFractura.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

        GridBoundColumn oGridBoundColumn;
        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = "Marca";
        oGridBoundColumn.HeaderText = "Marca";
        oGridBoundColumn.UniqueName = "Marca";
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = "Categoría";
        oGridBoundColumn.HeaderText = "Categoría";
        oGridBoundColumn.UniqueName = "Categoría";
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = "Subcategoría";
        oGridBoundColumn.HeaderText = "Subcategoría";
        oGridBoundColumn.UniqueName = "Subcategoría";
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = sNomMesUno;
        oGridBoundColumn.HeaderText = sNomMesUno;
        oGridBoundColumn.UniqueName = sNomMesUno;
        oGridBoundColumn.FooterText = "Total " + sNomMesUno;
        oGridBoundColumn.DataFormatString = "{0:N0}";
        oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
        oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = sNomMesDos;
        oGridBoundColumn.HeaderText = sNomMesDos;
        oGridBoundColumn.UniqueName = sNomMesDos;
        oGridBoundColumn.FooterText = "Total " + sNomMesDos;
        oGridBoundColumn.DataFormatString = "{0:N0}";
        oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
        oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = sNomMesTres;
        oGridBoundColumn.HeaderText = sNomMesTres;
        oGridBoundColumn.UniqueName = sNomMesTres;
        oGridBoundColumn.FooterText = "Total " + sNomMesTres;
        oGridBoundColumn.DataFormatString = "{0:N0}";
        oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
        oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = "Royalty (%)";
        oGridBoundColumn.HeaderText = "Royalty (%)";
        oGridBoundColumn.UniqueName = "Royalty (%)";
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = "BDI (%)";
        oGridBoundColumn.HeaderText = "BDI (%)";
        oGridBoundColumn.UniqueName = "BDI (%)";
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = "Periodo";
        oGridBoundColumn.HeaderText = "Periodo";
        oGridBoundColumn.UniqueName = "Periodo";
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = "Monto Royalty USD";
        oGridBoundColumn.HeaderText = "Monto Royalty USD";
        oGridBoundColumn.UniqueName = "Monto Royalty USD";
        oGridBoundColumn.FooterText = "Total Royalty USD";
        oGridBoundColumn.DataFormatString = "{0:N0}";
        oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
        oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = "Monto BDI USD";
        oGridBoundColumn.HeaderText = "Monto BDI USD";
        oGridBoundColumn.UniqueName = "Monto BDI USD";
        oGridBoundColumn.FooterText = "Total BDI USD";
        oGridBoundColumn.DataFormatString = "{0:N0}";
        oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
        oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        /*oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = "Saldo Advance USD";
        oGridBoundColumn.HeaderText = "Saldo Advance USD";
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);*/

        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = "Saldo Advance USD";
        oGridBoundColumn.HeaderText = "Saldo Advance USD";
        oGridBoundColumn.UniqueName = "Saldo Advance USD";
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = "Factura USD";
        oGridBoundColumn.HeaderText = "Factura USD";
        oGridBoundColumn.UniqueName = "Factura USD";
        oGridBoundColumn.FooterText = "Total Factura USD";
        //oGridBoundColumn.DataFormatString = "{0:N0}";
        oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
        oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
        oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

        oGridFractura.DataSource = dtFactura;
        idGrilla.Visible = true;
        idGrilla.Controls.Add(oGridFractura);
        idBtnSave.Visible = true;

        ViewState["dtFactura"] = dtFactura;
        ViewState["arrayMes"] = arrayMes;
        ViewState["arrayReporteVenta"] = arrayReporteVenta;

        RadAjaxManager oRadAjaxManager = new RadAjaxManager();
        oRadAjaxManager.ClientEvents.OnRequestStart = "onRequestStart";

        AjaxUpdatedControl oAjaxUpdatedControl = new AjaxUpdatedControl();
        oAjaxUpdatedControl.ControlID = "rdGridFactura";

        AjaxSetting oAjaxSetting = new AjaxSetting();
        oAjaxSetting.AjaxControlID = "rdGridFactura";
        oAjaxSetting.UpdatedControls.Add(oAjaxUpdatedControl);

        oRadAjaxManager.AjaxSettings.Add(oAjaxSetting);
        idGrilla.Controls.Add(oRadAjaxManager);

      }
      oConn.Close();

    }

    void oGridFractura_ItemCommand(object source, GridCommandEventArgs e)
    {
      if (e.CommandName == RadGrid.ExportToExcelCommandName)
      {
        RadGrid rdGridFactura = (RadGrid)idGrilla.FindControl("rdGridFactura");
        rdGridFactura.ExportSettings.ExportOnlyData = true;
        rdGridFactura.ExportSettings.IgnorePaging = true;
        rdGridFactura.ExportSettings.OpenInNewWindow = true;
        rdGridFactura.ExportSettings.FileName = "reporte_facturas_" + DateTime.Now.ToString("yyyyMMdd");
        rdGridFactura.MasterTableView.ExportToExcel();
      }
    }

    void oGridFractura_ItemDataBound(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;

        string sMarca = string.Empty;
        if (!string.IsNullOrEmpty(row["Marca"].ToString()))
        {
          DataTable dtMarca = getMarca(row["Marca"].ToString());
          if (dtMarca != null)
          {
            if (dtMarca.Rows.Count > 0)
            {
              sMarca = dtMarca.Rows[0]["descripcion"].ToString();
            }
          }
          dtMarca = null;
        }
        string sCategoria = string.Empty;
        if (!string.IsNullOrEmpty(row["Categoría"].ToString()))
        {
          DataTable dtCategoria = getCategoria(row["Categoría"].ToString());
          if (dtCategoria != null)
          {
            if (dtCategoria.Rows.Count > 0)
            {
              sCategoria = dtCategoria.Rows[0]["descripcion"].ToString();
            }
          }
          dtCategoria = null;
        }
        string sSubCategoria = string.Empty;
        if (!string.IsNullOrEmpty(row["Subcategoría"].ToString()))
        {
          DataTable dtSubCategoria = getSubCategoria(row["Subcategoría"].ToString());
          if (dtSubCategoria != null)
          {
            if (dtSubCategoria.Rows.Count > 0)
            {
              sSubCategoria = dtSubCategoria.Rows[0]["descripcion"].ToString();
            }
          }
          dtSubCategoria = null;
        }

        item["Marca"].Text = sMarca;
        item["Categoría"].Text = sCategoria;
        item["Subcategoría"].Text = sSubCategoria;

        string sNomMesUno = oWeb.getMes(int.Parse(arrayMes[0].ToString())).ToString();
        string sNomMesDos = oWeb.getMes(int.Parse(arrayMes[1].ToString())).ToString();
        string sNomMesTres = oWeb.getMes(int.Parse(arrayMes[2].ToString())).ToString();

        item[sNomMesUno].Text = double.Parse(row[sNomMesUno].ToString()).ToString("#,##0.00");
        item[sNomMesDos].Text = double.Parse(row[sNomMesDos].ToString()).ToString("#,##0.00");
        item[sNomMesTres].Text = double.Parse(row[sNomMesTres].ToString()).ToString("#,##0.00");

        item["Royalty (%)"].Text = String.Format("{0:p0}", double.Parse(row["Royalty (%)"].ToString()));
        item["BDI (%)"].Text = String.Format("{0:p0}", double.Parse(row["BDI (%)"].ToString()));
        item["Monto Royalty USD"].Text = double.Parse(row["Monto Royalty USD"].ToString()).ToString("#,##0.00");
        item["Monto BDI USD"].Text = double.Parse(row["Monto BDI USD"].ToString()).ToString("#,##0.00");
        item["Saldo Advance USD"].Text = double.Parse(row["Saldo Advance USD"].ToString()).ToString("#,##0.00");
        item["Factura USD"].Text = double.Parse(row["Factura USD"].ToString()).ToString("#,##0.00");
      }
    }

    protected DataTable getMarca(string pCodMarca)
    {
      DataTable dtMarca = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cMarcas oMarcas = new cMarcas(ref oConn);
        oMarcas.CodMarca = pCodMarca;
        dtMarca = oMarcas.Get();

      }
      oConn.Close();
      return dtMarca;
    }

    protected DataTable getCategoria(string pCodCategoria)
    {
      DataTable dtCategoria = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cCategorias oCategorias = new cCategorias(ref oConn);
        oCategorias.CodCategoria = pCodCategoria;
        dtCategoria = oCategorias.Get();

      }
      oConn.Close();
      return dtCategoria;
    }

    protected DataTable getSubCategoria(string pCodSubCategoria)
    {
      DataTable dtSubCategoria = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cSubCategoria oSubCategoria = new cSubCategoria(ref oConn);
        oSubCategoria.CodSubCategoria = pCodSubCategoria;
        dtSubCategoria = oSubCategoria.Get();
      }
      oConn.Close();
      return dtSubCategoria;
    }

    protected float getValueUsd(string intMes, string intAno, string pCodRoyalty)
    {
      float valueUsd = 0;
      if (pCodRoyalty != "3")
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cTipoRoyalty oTipoRoyalty = new cTipoRoyalty(ref oConn);
          oTipoRoyalty.CodRoyalty = pCodRoyalty;
          DataTable dtRoyalty = oTipoRoyalty.Get();
          if (dtRoyalty != null)
          {
            if (dtRoyalty.Rows.Count > 0)
            {
              cMonedas oMonedas = new cMonedas(ref oConn);
              oMonedas.Mes = intMes;
              oMonedas.Ano = intAno;
              oMonedas.CodMoneda = dtRoyalty.Rows[0]["cod_moneda"].ToString();
              valueUsd = oMonedas.GetValueUSD();
            }
          }
          dtRoyalty = null;
        }
        oConn.Close();
      }
      else
      {
        valueUsd = 1;
      }
      return valueUsd;
    }

    protected void btnAprobar_Click(object sender, EventArgs e)
    {
      string sNumInvoce = string.Empty;
      string sPeriodo = string.Empty;
      StringBuilder sHtml = new StringBuilder();
      sHtml.Append(File.ReadAllText(Server.MapPath("invoice.html")));
      StringBuilder sDataProductos = new StringBuilder();
      StringBuilder sDataValorProductos = new StringBuilder();
      double dTotalFacturaUsd = 0;
      bool bExito = false;

      if (ViewState["dtFactura"] != null)
      {
        arrayMes = ViewState["arrayMes"] as string[];
        DataTable dtFactura = ViewState["dtFactura"] as DataTable;
        if (dtFactura != null)
        {
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            cFactura oFactura = new cFactura(ref oConn);
            oFactura.NumContrato = hdd_num_contrato.Value;
            oFactura.Territory = "CHILE";
            oFactura.DateInvoce = DateTime.Now.ToString("yyyy-MM-dd");
            oFactura.DueDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
            oFactura.TipoFactura = "Q";
            oFactura.Accion = "CREAR";
            oFactura.Put();

            if (string.IsNullOrEmpty(oFactura.Error))
              bExito = true;

            string pCodFactura = oFactura.CodFactura;

            cDetalleFactura oDetalleFactura = new cDetalleFactura(ref oConn);
            oDetalleFactura.CodigoFactura = pCodFactura;

            foreach (DataRow oRow in dtFactura.Rows)
            {
              string sDescripcionMarca = string.Empty;
              DataTable dtMarca = getMarca(oRow["Marca"].ToString());
              if (dtMarca != null)
              {
                if (dtMarca.Rows.Count > 0)
                {
                  sDescripcionMarca = dtMarca.Rows[0]["descripcion"].ToString();
                }
              }
              dtMarca = null;

              string pCodCategoria = null;
              string sDescripcionCategoria = string.Empty;

              string pCodSubCategoria = null;
              string sDescripcionSubCategoria = string.Empty;

              if (!string.IsNullOrEmpty(oRow["Categoría"].ToString()))
              {
                pCodCategoria = oRow["Categoría"].ToString();
                DataTable dtCategoria = getCategoria(oRow["Categoría"].ToString());
                if (dtCategoria != null)
                {
                  if (dtCategoria.Rows.Count > 0)
                  {
                    sDescripcionCategoria = dtCategoria.Rows[0]["descripcion"].ToString();
                  }
                }
                dtCategoria = null;
              }
              if (!string.IsNullOrEmpty(oRow["Subcategoría"].ToString())) { 
                pCodSubCategoria = oRow["Subcategoría"].ToString();
                DataTable dtSubCategoria = getSubCategoria(oRow["Subcategoría"].ToString());
                if (dtSubCategoria != null) {
                  if (dtSubCategoria.Rows.Count > 0) {
                    sDescripcionSubCategoria = dtSubCategoria.Rows[0]["descripcion"].ToString();
                  }
                }
                dtSubCategoria = null;
              }

              sDataProductos.Append("<div style=\"padding-top:10px;padding-bottom:10px;\">");
              sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;font-weight:bold;\">");
              sDataProductos.Append(sDescripcionMarca);

              if (!string.IsNullOrEmpty(sDescripcionCategoria)) {
                sDataProductos.Append(" / " + sDescripcionCategoria);
              }
              if (!string.IsNullOrEmpty(sDescripcionSubCategoria))
              {
                sDataProductos.Append(" / " + sDescripcionSubCategoria);
              }

              sDataProductos.Append(" ").Append(String.Format("{0:P1}", double.Parse(oRow["Royalty (%)"].ToString()))).Append("</font></div>");
              //sDataProductos.Append(" ").Append(oRow["Royalty (%)"].ToString()).Append("</font></div>");
              sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">Royalties</font></div>");
              sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">BDI</font></div>");
              if (double.Parse(oRow["Saldo Advance USD"].ToString()) > 0)
              sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">Advance</font></div>");
              sDataProductos.Append("</div>");

              oDetalleFactura.CodMarca = oRow["Marca"].ToString();
              oDetalleFactura.CodCategoria = pCodCategoria;
              oDetalleFactura.CodSubcategoria = pCodSubCategoria;
              oDetalleFactura.MesNomUno = arrayMes[0].ToString();
              oDetalleFactura.MesMntUno = oRow[oWeb.getMes(int.Parse(arrayMes[0].ToString())).ToString()].ToString();

              oDetalleFactura.MesNomDos = arrayMes[1].ToString();
              oDetalleFactura.MesMntDos = oRow[oWeb.getMes(int.Parse(arrayMes[1].ToString())).ToString()].ToString();

              oDetalleFactura.MesNomTres = arrayMes[2].ToString();
              oDetalleFactura.MesMntTres = oRow[oWeb.getMes(int.Parse(arrayMes[2].ToString())).ToString()].ToString();

              oDetalleFactura.CodRoyalty = oRow["CodRoyalty"].ToString();
              oDetalleFactura.Royalty = oRow["Royalty (%)"].ToString();
              oDetalleFactura.Bdi = oRow["BDI (%)"].ToString();
              oDetalleFactura.Periodo = oRow["Periodo"].ToString();
              oDetalleFactura.MontoRoyaltyUsd = double.Parse(oRow["Monto Royalty USD"].ToString()).ToString();
              oDetalleFactura.MontoBdiUsd = double.Parse(oRow["Monto BDI USD"].ToString()).ToString();
              oDetalleFactura.SaldoAdvanceUsd = double.Parse(oRow["Saldo Advance USD"].ToString()).ToString();
              oDetalleFactura.FacturaUsd = double.Parse(oRow["Factura USD"].ToString()).ToString();
              oDetalleFactura.Accion = "CREAR";
              oDetalleFactura.Put();

              if (!string.IsNullOrEmpty(oDetalleFactura.Error))
              {
                bExito = false;
              }

              cAdvanceContrato oAdvanceContrato = new cAdvanceContrato(ref oConn);
              oAdvanceContrato.NumContrato = hdd_num_contrato.Value;
              oAdvanceContrato.CodMarca = oRow["Marca"].ToString();
              oAdvanceContrato.CodCategoria = pCodCategoria;
              oAdvanceContrato.CodSubCategoria = pCodSubCategoria;
              if (double.Parse(oRow["Factura USD"].ToString()) > 0)
                oAdvanceContrato.Saldo = oRow["Saldo"].ToString();
              oAdvanceContrato.Accion = "EDITAR";
              oAdvanceContrato.Put();

              sDataValorProductos.Append("<div style=\"padding-top:10px;padding-bottom:10px; padding-left:10px;\">");
              sDataValorProductos.Append("<div style=\"float:left;width:30px;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">&nbsp;</font></div>");
              sDataValorProductos.Append("<div style=\"text-align:right;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">&nbsp;</font></div>");
              sDataValorProductos.Append("<div style=\"float:left;width:30px;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">$</font></div>");
              sDataValorProductos.Append("<div style=\"text-align:right;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">").Append(double.Parse(oRow["Monto Royalty USD"].ToString()).ToString("#,##0.00")).Append("</font></div>");
              sDataValorProductos.Append("<div style=\"float:left;width:30px;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">$</font></div>");
              sDataValorProductos.Append("<div style=\"text-align:right;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">").Append(double.Parse(oRow["Monto BDI USD"].ToString()).ToString("#,##0.00")).Append("</font></div>");
              sDataValorProductos.Append("<div style=\"float:left;width:30px;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">&nbsp;</font></div>");
              if (double.Parse(oRow["Saldo Advance USD"].ToString()) > 0)
              sDataValorProductos.Append("<div style=\"text-align:right;\"><font style=\"font-family:Arial; color:#ff0000; font-size:10pt;\">(").Append(double.Parse(oRow["Saldo Advance USD"].ToString()).ToString("#,##0.00")).Append(")</font></div>");
              sDataValorProductos.Append("</div>");

              dTotalFacturaUsd = dTotalFacturaUsd + double.Parse(oRow["Factura USD"].ToString());
            }

            string strNomDeudor = string.Empty;
            string sNomContacto = string.Empty;
            string sEmailContacto = string.Empty;
            string sDireccion = string.Empty;
            string sComuna = string.Empty;
            string sCiudad = string.Empty;

            cContratos oContrato = new cContratos(ref oConn);
            oContrato.NumContrato = hdd_num_contrato.Value;
            DataTable dtContrato = oContrato.Get();
            if (dtContrato != null)
            {
              if (dtContrato.Rows.Count > 0)
              {
                cDeudor oDeudor = new cDeudor(ref oConn);
                oDeudor.NKeyDeudor = dtContrato.Rows[0]["nKey_Deudor"].ToString();
                DataTable dtDeudor = oDeudor.Get();
                if (dtDeudor != null)
                {
                  if (dtDeudor.Rows.Count > 0)
                  {
                    strNomDeudor = dtDeudor.Rows[0]["snombre"].ToString();
                  }
                }
                dtDeudor = null;

                cContactosDeudor oContactosDeudor = new cContactosDeudor(ref oConn);
                oContactosDeudor.NKeyDeudor = dtContrato.Rows[0]["nKey_Deudor"].ToString();
                oContactosDeudor.NkeyCliente = oUsuario.NKeyUsuario;
                DataTable dtContacto = oContactosDeudor.Get();
                if (dtContacto != null)
                {
                  if (dtContacto.Rows.Count > 0)
                  {
                    sNomContacto = dtContacto.Rows[0]["sNombre"].ToString();
                    sEmailContacto = dtContacto.Rows[0]["sEmail"].ToString();
                  }
                }
                dtContacto = null;

                cDirecciones oDirecciones = new cDirecciones(ref oConn);
                oDirecciones.NKeyDeudor = dtContrato.Rows[0]["nKey_Deudor"].ToString();
                oDirecciones.NkeyCliente = oUsuario.NKeyUsuario;
                DataTable dtDireccion = oDirecciones.Get();
                if (dtDireccion != null)
                {
                  if (dtDireccion.Rows.Count > 0)
                  {
                    sDireccion = dtDireccion.Rows[0]["sDireccion"].ToString();
                    sComuna = dtDireccion.Rows[0]["sComuna"].ToString();
                    sCiudad = dtDireccion.Rows[0]["sCiudad"].ToString();
                  }
                }
                dtDireccion = null;
              }
            }
            dtContrato = null;

            //string sNumInvoce = strNomDeudor.Substring(0, 3).ToUpper();
            //sNumInvoce = sNumInvoce.ToUpper();
            //string sNumInvoce = "FE" + int.Parse(pCodFactura).ToString("D10");
            sNumInvoce = "FE" + pCodFactura;

            oFactura.Total = dTotalFacturaUsd.ToString();
            oFactura.Periodo = hdd_periodo.Value + "/" + hdd_ano_periodo.Value;
            oFactura.NumInvoice = sNumInvoce;
            oFactura.Accion = "EDITAR";
            oFactura.Put();

            cReporteVenta cReporteVenta;
            arrayReporteVenta = ViewState["arrayReporteVenta"] as string[];
            foreach (string pCodReporte in arrayReporteVenta)
            {
              cReporteVenta = new cReporteVenta(ref oConn);
              cReporteVenta.CodigoReporteVenta = pCodReporte;
              cReporteVenta.Facturado = "F";
              cReporteVenta.CodigoFactura = pCodFactura;
              cReporteVenta.Accion = "EDITAR";
              cReporteVenta.Put();

            }

            if (double.Parse(dTotalFacturaUsd.ToString()) > 0)
              sHtml.Replace("[#TITLE]", "INVOICE");
            else
              sHtml.Replace("[#TITLE]", "CREDIT NOTE");

            sHtml.Replace("[#NUMFACTURA]", sNumInvoce);
            sHtml.Replace("[#NOMEMPRESA]", strNomDeudor);
            sHtml.Replace("[#NOMCONTACTO]", sNomContacto);
            sHtml.Replace("[#EMAILCONTACTO]", sEmailContacto);
            sHtml.Replace("[#DIRECCION]", sDireccion + (!string.IsNullOrEmpty(sComuna) ? ", " + sComuna : string.Empty));
            sHtml.Replace("[#CIUDAD]", sCiudad);
            sHtml.Replace("[#PAIS]", "");
            sHtml.Replace("[#FECHA]", DateTime.Now.ToString("d-MMM-yy", CultureInfo.CreateSpecificCulture("en-US")));
            sHtml.Replace("[#NUMCONTRATO]", hdd_no_contrato.Value);
            sHtml.Replace("[#PERIODOQ]", oDetalleFactura.Periodo);
            sPeriodo = oDetalleFactura.Periodo;
            sHtml.Replace("[#PROPERTY]", "");
            sHtml.Replace("[#TERRITORIO]", "CHILE");
            sHtml.Replace("[#DETALLEPRODUCTOS]", sDataProductos.ToString());
            sHtml.Replace("[#DETALLEVALORPRODUCTOS]", sDataValorProductos.ToString());
            sHtml.Replace("[#DUEDATE]", DateTime.Now.AddMonths(1).ToString("d-MMM-yy", CultureInfo.CreateSpecificCulture("en-US")));
            //sHtml.Replace("[#TOTAL]", double.Parse(dTotalFacturaUsd.ToString()).ToString("#,##0.00"));
            sHtml.Replace("[#TOTAL]", double.Parse(dTotalFacturaUsd.ToString()).ToString("N0"));

            if (!Directory.Exists(Server.MapPath("Facturas/") + hdd_no_contrato.Value + "/"))
              Directory.CreateDirectory(Server.MapPath("Facturas/") + hdd_no_contrato.Value + "/");

            string sFileHtml = Server.MapPath("Facturas/") + hdd_no_contrato.Value + "/" + sNumInvoce + ".html";
            File.WriteAllText(sFileHtml, sHtml.ToString(),Encoding.UTF8);



          }
          oConn.Close();
        }
        dtFactura = null;
      }

      if (bExito)
      {
        //cmbox_contrato.SelectedValue = "0";
        idGrilla.Visible = false;
        idBtnSave.Visible = false;

        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cLogEventos oLogEventos = new cLogEventos(ref oConn);
          oLogEventos.AccionLog = "APROBAR POR DETALLE PRE-FACTURA Q";
          oLogEventos.CodCanal = "2";
          oLogEventos.CodFlujo = "5";
          oLogEventos.NomFlujo = "FACTURACION Q";
          oLogEventos.NumContrato = hdd_num_contrato.Value;
          oLogEventos.NoContrato = hdd_no_contrato.Value;
          oLogEventos.PeriodoLog = sPeriodo;
          //oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
          oLogEventos.CodUser = oUsuario.CodUsuario;
          oLogEventos.RutUser = oUsuario.RutUsuario;
          oLogEventos.NomUser = oUsuario.Nombres;
          oLogEventos.ObsLog = "Se ha autorizado correctamente la Factura Q " + sNumInvoce;
          oLogEventos.IpLog = oWeb.GetIpUsuario();
          oLogEventos.Accion = "CREAR";
          oLogEventos.Put();
        }
        oConn.Close();

        StringBuilder js = new StringBuilder();
        js.Append("function LgRespuesta() {");
        js.Append(" window.radalert('La factura ha sido emitida con éxito.', 330, 210); ");
        js.Append(" Sys.Application.remove_load(LgRespuesta); ");
        js.Append("};");
        js.Append("Sys.Application.add_load(LgRespuesta);");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", js.ToString(), true);
      }
    }

    protected void btnContacto_Click(object sender, EventArgs e)
    {
      Response.Redirect("contacto.aspx");
    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {
      Response.Redirect("downloadmanual.ashx");
    }
  }
}
