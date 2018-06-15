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
  public partial class aprobar_facturas_short_fall_detalle : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();

      if (!IsPostBack) { 
        hdd_periodo.Value = oWeb.GetData("pPeriodo");
        hdd_no_contrato.Value = oWeb.GetData("pNoContrato");
        hdd_num_contrato.Value = oWeb.GetData("pNumContrato");

        lblNoContrato.Text = hdd_no_contrato.Value;
      }
      getShortFall();

    }

    public void getShortFall()
    {
      FacturaShortFall oFacturaShortFall = new FacturaShortFall();
      oFacturaShortFall.ViewClient = true;
      oFacturaShortFall.Periodo = hdd_periodo.Value;
      oFacturaShortFall.getMakeTable();

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        DateTime dFechaIni = DateTime.Parse(hdd_periodo.Value.Substring(0, 10));
        DateTime dFechaFin = DateTime.Parse(hdd_periodo.Value.Substring(13, 10));

        string pNKeyDeudor = null;
        cContratos oContrato = new cContratos(ref oConn);
        oContrato.NumContrato = hdd_num_contrato.Value;
        DataTable dtContrato = oContrato.Get();
        if (dtContrato != null)
        {
          pNKeyDeudor = dtContrato.Rows[0]["nkey_deudor"].ToString();
        }
        dtContrato = null;

        cMinimoContrato oMinimoContrato = new cMinimoContrato(ref oConn);
        oMinimoContrato.NkeyDeudor = pNKeyDeudor;
        oMinimoContrato.NumContrato = hdd_num_contrato.Value;
        oMinimoContrato.FechaInicio = dFechaIni.ToString("yyyyMMdd");
        oMinimoContrato.FechaFinal = dFechaFin.ToString("yyyyMMdd");
        DataTable dtMinimo = oMinimoContrato.Get();

        if (dtMinimo != null)
        {
          foreach (DataRow oRow in dtMinimo.Rows)
          {
            int iYearInicio = DateTime.Parse(hdd_periodo.Value.Substring(0, 10)).Year;
            int iYearFinal = DateTime.Parse(hdd_periodo.Value.Substring(13, 10)).Year;

            oFacturaShortFall.Licenciatario = oRow["licenciatario"].ToString();
            oFacturaShortFall.NoContrato = oRow["contrato"].ToString();
            oFacturaShortFall.Marca = oRow["marca"].ToString();
            oFacturaShortFall.Categoria = oRow["categoria"].ToString();
            oFacturaShortFall.SubCategoria = oRow["subcategoria"].ToString();
            oFacturaShortFall.MntMinimoGarantizado = oRow["minimo"].ToString();
            oFacturaShortFall.Periodo = "Short Fall/" + iYearFinal.ToString();

            oFacturaShortFall.MntFactAdvance = "0";
            oFacturaShortFall.MntPeriodoFactUno = "0";
            oFacturaShortFall.MntPeriodoFactDos = "0";
            oFacturaShortFall.MntPeriodoFactTres = "0";
            oFacturaShortFall.MntPeriodoFactCuatro = "0";
            oFacturaShortFall.MntPeriodoFactCinco = "0";

            cFactura oFactura = new cFactura(ref oConn);
            oFactura.NumContrato = (!string.IsNullOrEmpty(hdd_num_contrato.Value) ? hdd_num_contrato.Value : oRow["num_contrato"].ToString());
            oFactura.TipoFactura = "T";
            oFactura.PeriodoAno = (iYearInicio.ToString() == iYearFinal.ToString() ? "'" + iYearInicio.ToString() + "'" : "'" + iYearInicio.ToString() + "','" + iYearFinal.ToString() + "'");

            DataTable dtFactura = oFactura.GetForMinimo();
            if (dtFactura != null)
            {
              if (dtFactura.Rows.Count > 0)
              {
                double iTotal = 0;
                double iTotalCorregido = 0;
                double iTotalUsd = 0;
                foreach (DataRow oRowFact in dtFactura.Rows)
                {
                  cDetalleFacturaAdvance oDetalleFacturaAdvance = new cDetalleFacturaAdvance(ref oConn);
                  oDetalleFacturaAdvance.CodigoFactura = oRowFact["codigo_factura"].ToString();
                  oDetalleFacturaAdvance.CodMarca = oRow["cod_marca"].ToString();
                  oDetalleFacturaAdvance.CodCategoria = oRow["cod_categoria"].ToString();
                  oDetalleFacturaAdvance.CodSubCategoria = oRow["cod_subcategoria"].ToString();
                  DataTable dtDetalleFactAdvance = oDetalleFacturaAdvance.GetByMinimo();
                  if (dtDetalleFactAdvance != null)
                  {
                    if (dtDetalleFactAdvance.Rows.Count > 0)
                    {
                      oFacturaShortFall.MntFactAdvance = dtDetalleFactAdvance.Rows[0]["advance_usd"].ToString();
                    }
                  }
                  dtDetalleFactAdvance = null;

                  cDetalleFactura DetalleFactura = new cDetalleFactura(ref oConn);
                  DetalleFactura.CodigoFactura = oRowFact["codigo_factura"].ToString();
                  DetalleFactura.CodMarca = oRow["cod_marca"].ToString();
                  DetalleFactura.CodCategoria = oRow["cod_categoria"].ToString();
                  DetalleFactura.CodSubcategoria = oRow["cod_subcategoria"].ToString();
                  DataTable dtDetalleFactura = DetalleFactura.GetByMinimo();
                  if (dtDetalleFactura != null)
                  {
                    if (dtDetalleFactura.Rows.Count > 0)
                    {

                      Web oWeb = new Web();
                      DateTime fecha_inicial = DateTime.Parse(hdd_periodo.Value.Substring(0, 10));
                      DateTime fecha_final = DateTime.Parse(hdd_periodo.Value.Substring(13));

                      int lngCantMes = Math.Abs((fecha_final.Month - fecha_inicial.Month) + 12 * (fecha_final.Year - fecha_inicial.Year)) + 1;
                      int lngTrimestres = lngCantMes / 3;

                      if (lngTrimestres > 0)
                      {
                        DateTime dMonthForQ = fecha_inicial;
                        for (int i = 0; i < lngTrimestres; i++)
                        {
                          dMonthForQ = dMonthForQ.AddMonths(2);
                          if (oRowFact["periodo"].ToString() == (oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString()))
                          {
                            if (i == 0)
                              oFacturaShortFall.MntPeriodoFactUno = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); // dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                            else if (i == 1)
                              oFacturaShortFall.MntPeriodoFactDos = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); // dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                            else if (i == 2)
                              oFacturaShortFall.MntPeriodoFactTres = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); // dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                            else if (i == 3)
                              oFacturaShortFall.MntPeriodoFactCuatro = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); // dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                          }
                          dMonthForQ = dMonthForQ.AddMonths(1);
                        }
                      }
                      else
                      {
                        oFacturaShortFall.MntPeriodoFactUno = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); //dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                      }
                    }
                  }
                  dtDetalleFactura = null;
                }
                iTotal = double.Parse(oFacturaShortFall.MntMinimoGarantizado) - (!string.IsNullOrEmpty(oFacturaShortFall.MntFactAdvance) ? double.Parse(oFacturaShortFall.MntFactAdvance) : 0);
                iTotal = iTotal - (!string.IsNullOrEmpty(oFacturaShortFall.MntPeriodoFactUno) ? double.Parse(oFacturaShortFall.MntPeriodoFactUno) : 0);
                iTotal = iTotal - (!string.IsNullOrEmpty(oFacturaShortFall.MntPeriodoFactDos) ? double.Parse(oFacturaShortFall.MntPeriodoFactDos) : 0);
                iTotal = iTotal - (!string.IsNullOrEmpty(oFacturaShortFall.MntPeriodoFactTres) ? double.Parse(oFacturaShortFall.MntPeriodoFactTres) : 0);
                iTotal = iTotal - (!string.IsNullOrEmpty(oFacturaShortFall.MntPeriodoFactCuatro) ? double.Parse(oFacturaShortFall.MntPeriodoFactCuatro) : 0);

                if (iTotal < 0)
                  iTotalCorregido = 0;
                else
                  iTotalCorregido = iTotal;

                iTotalUsd = (!string.IsNullOrEmpty(oFacturaShortFall.MntFactAdvance) ? double.Parse(oFacturaShortFall.MntFactAdvance) : 0);
                iTotalUsd = iTotalUsd - (!string.IsNullOrEmpty(oFacturaShortFall.MntPeriodoFactUno) ? double.Parse(oFacturaShortFall.MntPeriodoFactUno) : 0);
                iTotalUsd = iTotalUsd - (!string.IsNullOrEmpty(oFacturaShortFall.MntPeriodoFactDos) ? double.Parse(oFacturaShortFall.MntPeriodoFactDos) : 0);
                iTotalUsd = iTotalUsd - (!string.IsNullOrEmpty(oFacturaShortFall.MntPeriodoFactTres) ? double.Parse(oFacturaShortFall.MntPeriodoFactTres) : 0);
                iTotalUsd = iTotalUsd - (!string.IsNullOrEmpty(oFacturaShortFall.MntPeriodoFactCuatro) ? double.Parse(oFacturaShortFall.MntPeriodoFactCuatro) : 0);

                //if (iTotalUsd < 0)
                //  iTotalUsd = 0;

                oFacturaShortFall.FacturaUsd = iTotal.ToString();
                oFacturaShortFall.MntPeriodoFactCinco = iTotalUsd.ToString();
                oFacturaShortFall.Descuento = null;
                oFacturaShortFall.FacturaUsdCorregida = iTotalCorregido.ToString();

                oFacturaShortFall.AddRow();
              }
            }
            dtFactura = null;
          }
        }
        dtMinimo = null;
      }
      oConn.Close();

      makeGrid(oFacturaShortFall.Get());

    }

    private class TemplateText : ITemplate
    {
      private string colname;
      public TemplateText()
      {

      }

      public TemplateText(string cName)
      {
        colname = cName;
      }

      public void InstantiateIn(System.Web.UI.Control container)
      {
        RadTextBox textBox = new RadTextBox();
        textBox.ID = "txt_descuento";
        textBox.DataBinding += TextBox_DataBinding;
        container.Controls.Add(textBox);
      }

      private void TextBox_DataBinding(object sender, EventArgs e)
      {
        RadTextBox RadText = (RadTextBox)sender;
        GridDataItem container = (GridDataItem)RadText.NamingContainer;
        RadText.Text = ((DataRowView)container.DataItem)[colname].ToString();
      }
    }

    private void makeGrid(DataTable dtFacturaShortFall)
    {
      RadGrid oGridMinimo = new RadGrid();
      oGridMinimo.ID = "idGridShortFall";
      oGridMinimo.ShowStatusBar = true;
      oGridMinimo.ShowFooter = true;
      oGridMinimo.AutoGenerateColumns = false;
      oGridMinimo.Skin = "Sitefinity";
      oGridMinimo.ItemDataBound += OGridMinimo_ItemDataBound;
      oGridMinimo.MasterTableView.AutoGenerateColumns = false;
      oGridMinimo.MasterTableView.ShowHeader = true;
      oGridMinimo.MasterTableView.ShowFooter = true;
      oGridMinimo.MasterTableView.TableLayout = GridTableLayout.Fixed;
      oGridMinimo.MasterTableView.EditMode = GridEditMode.EditForms;
      oGridMinimo.MasterTableView.ShowHeadersWhenNoRecords = true;

      GridBoundColumn oGridBoundColumn;
      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Licenciatario";
      oGridBoundColumn.HeaderText = "Licenciatario";
      oGridBoundColumn.UniqueName = "Licenciatario";
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Contrato";
      oGridBoundColumn.HeaderText = "Contrato";
      oGridBoundColumn.UniqueName = "Contrato";
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Marca";
      oGridBoundColumn.HeaderText = "Marca";
      oGridBoundColumn.UniqueName = "Marca";
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Categoría";
      oGridBoundColumn.HeaderText = "Categoría";
      oGridBoundColumn.UniqueName = "Categoría";
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Subcategoría";
      oGridBoundColumn.HeaderText = "Subcategoría";
      oGridBoundColumn.UniqueName = "Subcategoría";
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Mínimo Garantizado USD";
      oGridBoundColumn.HeaderText = "Mínimo Garantizado USD";
      oGridBoundColumn.UniqueName = "Mínimo Garantizado USD";
      //oGridBoundColumn.FooterText = "Total Mínimo Garantizado USD";
      oGridBoundColumn.DataFormatString = "{0:N0}";
      oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
      oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Periodo";
      oGridBoundColumn.HeaderText = "Periodo";
      oGridBoundColumn.UniqueName = "Periodo";
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Factura Advance";
      oGridBoundColumn.HeaderText = "Factura Advance";
      oGridBoundColumn.UniqueName = "Factura Advance";
      //oGridBoundColumn.FooterText = "Total Factura Advance";
      oGridBoundColumn.DataFormatString = "{0:N}";
      oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
      oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      DateTime fechainicial = DateTime.Parse(hdd_periodo.Value.Substring(0, 10));
      DateTime fechafinal = DateTime.Parse(hdd_periodo.Value.Substring(13));

      int iCantMes = Math.Abs((fechafinal.Month - fechainicial.Month) + 12 * (fechafinal.Year - fechainicial.Year)) + 1;
      int iTrimestres = iCantMes / 3;
      if (iTrimestres > 0)
      {
        DateTime dMonthForQ = fechainicial;
        for (int i = 0; i < iTrimestres; i++)
        {
          dMonthForQ = dMonthForQ.AddMonths(2);
          oGridBoundColumn = new GridBoundColumn();
          oGridBoundColumn.DataField = "Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString();
          oGridBoundColumn.HeaderText = "Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString();
          oGridBoundColumn.UniqueName = "Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString();
          //oGridBoundColumn.FooterText = "Total Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString();
          oGridBoundColumn.DataFormatString = "{0:N}";
          oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
          oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
          oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);
          dMonthForQ = dMonthForQ.AddMonths(1);
        }
      }
      else
      {
        DateTime iMonth = fechainicial.AddMonths(2);
        oGridBoundColumn = new GridBoundColumn();
        oGridBoundColumn.DataField = "Factura " + oWeb.getPeriodoByNumMonth(iMonth.Month) + "/" + iMonth.Year.ToString();
        oGridBoundColumn.HeaderText = "Factura " + oWeb.getPeriodoByNumMonth(iMonth.Month) + "/" + iMonth.Year.ToString();
        oGridBoundColumn.UniqueName = "Factura " + oWeb.getPeriodoByNumMonth(iMonth.Month) + "/" + iMonth.Year.ToString();
        //oGridBoundColumn.FooterText = "Total Factura " + oWeb.getPeriodoByNumMonth(iMonth.Month) + "/" + iMonth.Year.ToString();
        oGridBoundColumn.DataFormatString = "{0:N}";
        oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
        oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
        oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);
      }

      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Factura USD";
      oGridBoundColumn.HeaderText = "Factura USD";
      oGridBoundColumn.UniqueName = "Factura USD";
      //oGridBoundColumn.FooterText = "Total Factura USD";
      oGridBoundColumn.DataFormatString = "{0:N0}";
      oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
      oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Factura Prueba";
      oGridBoundColumn.HeaderText = "Factura Prueba";
      oGridBoundColumn.UniqueName = "Factura Prueba";
      //oGridBoundColumn.FooterText = "Total Factura Advance";
      oGridBoundColumn.DataFormatString = "{0:N0}";
      oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
      oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
      //oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      GridTemplateColumn oGridTemplateColumn = new GridTemplateColumn();
      oGridBoundColumn.DataField = "Descuento";
      oGridTemplateColumn.HeaderText = "Aplicar Descuento (%)";
      oGridTemplateColumn.UniqueName = "Descuento";
      oGridTemplateColumn.ItemTemplate = new TemplateText("Descuento");
      oGridMinimo.MasterTableView.Columns.Add(oGridTemplateColumn);

      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Factura USD Corregida";
      oGridBoundColumn.HeaderText = "Factura USD Corregida";
      oGridBoundColumn.UniqueName = "Factura USD Corregida";
      //oGridBoundColumn.FooterText = "Total Factura USD Corregida";
      oGridBoundColumn.DataFormatString = "{0:N0}";
      oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
      oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      oGridMinimo.DataSource = dtFacturaShortFall;
      oGridMinimo.Rebind();
      idGrilla.Visible = true;
      idGrilla.Controls.Add(oGridMinimo);
      if (ViewState["dtFactShortFall"] == null)
        ViewState["dtFactShortFall"] = dtFacturaShortFall;

    }

    private void OGridMinimo_ItemDataBound(object sender, GridItemEventArgs e)
    {
      //if (e.Item is GridDataItem)
      //{
      //  GridDataItem item = (GridDataItem)e.Item;
      //  DataRowView row = (DataRowView)e.Item.DataItem;

      //  //if (!string.IsNullOrEmpty(row["Mínimo Garantizado USD"].ToString()))
      //  //  item["Mínimo Garantizado USD"].Text = double.Parse(row["Mínimo Garantizado USD"].ToString()).ToString("#,##0.00");

      //  //if (!string.IsNullOrEmpty(row["Factura Advance"].ToString()))
      //  //  item["Factura Advance"].Text = double.Parse(row["Factura Advance"].ToString()).ToString("#,##0.00");

      //  //if (!string.IsNullOrEmpty(row["Factura USD"].ToString()))
      //  //  item["Factura USD"].Text = double.Parse(row["Factura USD"].ToString()).ToString("#,##0.00");
      //}
    }

    protected void btnAprobar_Click(object sender, EventArgs e)
    {
      int iCant = 0;
      StringBuilder sDataProductos1 = new StringBuilder();
      StringBuilder sDataProductos2 = new StringBuilder();
      
      StringBuilder sDataValorProductos1 = new StringBuilder();
      StringBuilder sDataValorProductos2 = new StringBuilder();
      string sPeriodo = string.Empty;
      //double dTotalFacturaUsd = 0;
      bool bExito = false;

      double iTotalFactura = 0;
      int iYearInicio = DateTime.Parse(hdd_periodo.Value.Substring(0, 10)).Year;
      int iYearFinal = DateTime.Parse(hdd_periodo.Value.Substring(13, 10)).Year;

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cFactura oFactura = new cFactura(ref oConn);
        oFactura.NumContrato = hdd_num_contrato.Value;
        oFactura.Territory = "CHILE";
        oFactura.DateInvoce = DateTime.Now.ToString("yyyy-MM-dd");
        oFactura.DueDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
        oFactura.TipoFactura = "S";
        oFactura.Accion = "CREAR";
        oFactura.Put();

        if (string.IsNullOrEmpty(oFactura.Error))
          bExito = true;

        string pCodFactura = oFactura.CodFactura;


        cDetFactShortfall oDetFactShortfall = new cDetFactShortfall(ref oConn);

        DataTable dtFActShortFall = ViewState["dtFactShortFall"] as DataTable;

        if (dtFActShortFall != null)
        {
          foreach (DataRow oRow in dtFActShortFall.Rows)
          {
            StringBuilder sDataProductos = new StringBuilder();
            StringBuilder sDataValorProductos = new StringBuilder();

            oDetFactShortfall.CodigoFactura = pCodFactura;
            oDetFactShortfall.NumContrato = hdd_num_contrato.Value;

            string pCodMarca = null;
            string sDescripcionMarca = string.Empty;
            DataTable dtMarca = getMarca(oRow["Marca"].ToString());
            if (dtMarca != null)
            {
              if (dtMarca.Rows.Count > 0)
              {
                pCodMarca = dtMarca.Rows[0]["cod_marca"].ToString();
                sDescripcionMarca = dtMarca.Rows[0]["descripcion"].ToString();
              }
            }
            dtMarca = null;
            oDetFactShortfall.CodMarca = pCodMarca;

            string pCodCategoria = null;
            string sDescripcionCategoria = string.Empty;
            if (!string.IsNullOrEmpty(oRow["Categoría"].ToString()))
            {
              DataTable dtCategoria = getCategoria(oRow["Categoría"].ToString());
              if (dtCategoria != null)
              {
                if (dtCategoria.Rows.Count > 0)
                {
                  pCodCategoria = dtCategoria.Rows[0]["cod_categoria"].ToString();
                  sDescripcionCategoria = dtCategoria.Rows[0]["descripcion"].ToString();
                }
              }
              dtCategoria = null;
            }
            oDetFactShortfall.CodCategoria = pCodCategoria;

            string pCodSubCategoria = null;
            string sDescripcionSubCategoria = string.Empty;
            if (!string.IsNullOrEmpty(oRow["Subcategoría"].ToString()))
            {
              DataTable dtSubCategoria = getSubCategoria(oRow["Subcategoría"].ToString());
              if (dtSubCategoria != null)
              {
                if (dtSubCategoria.Rows.Count > 0)
                {
                  pCodSubCategoria = dtSubCategoria.Rows[0]["cod_subcategoria"].ToString();
                  sDescripcionSubCategoria = dtSubCategoria.Rows[0]["descripcion"].ToString();
                }
              }
              dtSubCategoria = null;
            }
            oDetFactShortfall.CodSubcategoria = pCodSubCategoria;

            if (string.IsNullOrEmpty(oRow["Descuento"].ToString()))
              sDataProductos.Append("<div style=\"height:50px;padding-top:10px;padding-bottom:10px;float:left;width:85%\">");
            else
              sDataProductos.Append("<div style=\"height:100px;padding-top:10px;padding-bottom:10px;float:left;width:85%\">");
            sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;font-weight:bold;\">Minimo Garantizado - ").Append(sDescripcionMarca);

            if (!string.IsNullOrEmpty(sDescripcionCategoria))
            {
              sDataProductos.Append(" / " + sDescripcionCategoria);
            }
            if (!string.IsNullOrEmpty(sDescripcionSubCategoria))
            {
              sDataProductos.Append(" / " + sDescripcionSubCategoria);
            }

            sDataProductos.Append("</font></div>");

            sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">Royalty full year</font></div>");
            sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">Short fall calculado</font></div>");
            if (!string.IsNullOrEmpty(oRow["Descuento"].ToString())) {
              sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">Waiver ");
              sDataProductos.Append(oRow["Descuento"].ToString()).Append("%");
              sDataProductos.Append("</font></div>");
              sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">Total</font></div>");
            }
            sDataProductos.Append("</div>");
            if (string.IsNullOrEmpty(oRow["Descuento"].ToString()))
              sDataProductos.Append("<div style=\"height:50px;padding-top:10px; padding-bottom:10px; \">");
            else
              sDataProductos.Append("<div style=\"height:100px;padding-top:10px; padding-bottom:10px; \">");

            sDataProductos.Append("<div style=\"right\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;font-weight:bold;\">").Append(double.Parse(oRow["Mínimo Garantizado USD"].ToString()).ToString("#,##0.00")).Append("</font></div>");

            oDetFactShortfall.MntMinGarantizado = oRow["Mínimo Garantizado USD"].ToString();
            oDetFactShortfall.MntFactAdvance = oRow["Factura Advance"].ToString();

            DateTime fechainicial = DateTime.Parse(hdd_periodo.Value.Substring(0, 10));
            DateTime fechafinal = DateTime.Parse(hdd_periodo.Value.Substring(13));

            int iCantMes = Math.Abs((fechafinal.Month - fechainicial.Month) + 12 * (fechafinal.Year - fechainicial.Year)) + 1;
            int iTrimestres = iCantMes / 3;
            if (iTrimestres > 0)
            {
              DateTime dMonthForQ = fechainicial;
              for (int i = 0; i < iTrimestres; i++)
              {
                dMonthForQ = dMonthForQ.AddMonths(2);
                string sMonto = oRow["Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString()].ToString();
                if (i == 0)
                {
                  oDetFactShortfall.PeriodoFactUno = "Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString();
                  oDetFactShortfall.MntPeriodoFactUno = (sMonto != "&nbsp;" ? sMonto : null);
                }
                else if (i == 1)
                {
                  oDetFactShortfall.PeriodoFactDos = "Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString();
                  oDetFactShortfall.MntPeriodoFactDos = (sMonto != "&nbsp;" ? sMonto : null);
                }
                else if (i == 2)
                {
                  oDetFactShortfall.PeriodoFactTres = "Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString();
                  oDetFactShortfall.MntPeriodoFactTres = (sMonto != "&nbsp;" ? sMonto : null);
                }
                else if (i == 3)
                {
                  oDetFactShortfall.PeriodoFactCuatro = "Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString();
                  oDetFactShortfall.MntPeriodoFactCuatro = (sMonto != "&nbsp;" ? sMonto : null);
                }
                dMonthForQ = dMonthForQ.AddMonths(1);
              }
            }
            else
            {
              DateTime iMonth = fechainicial.AddMonths(2);
              string sMonto = oRow["Factura " + oWeb.getPeriodoByNumMonth(iMonth.Month) + "/" + iMonth.Year.ToString()].ToString();

              oDetFactShortfall.PeriodoFactUno = "Factura " + oWeb.getPeriodoByNumMonth(iMonth.Month) + "/" + iMonth.Year.ToString();
              oDetFactShortfall.MntPeriodoFactUno = (sMonto != "&nbsp;" ? sMonto : null);
            }

            oDetFactShortfall.FacturaUsd = oRow["Factura USD"].ToString();

            oDetFactShortfall.MntDescuento = (!string.IsNullOrEmpty(oRow["Descuento"].ToString()) ? oRow["Descuento"].ToString() : null);

            oDetFactShortfall.FacturaUsdDf = (!string.IsNullOrEmpty(oRow["Factura USD Corregida"].ToString()) ? oRow["Factura USD Corregida"].ToString() : oRow["Factura USD"].ToString());

            sDataProductos.Append("<div style=\"right\"><font style=\"font-family:Arial; color:#ff0000; font-size:10pt;\">(").Append(double.Parse(oRow["Factura Prueba"].ToString()).ToString("#,##0.00")).Append(")</font></div>");

            if (!string.IsNullOrEmpty(oRow["Descuento"].ToString()))
            {
              sDataProductos.Append("<div style=\"right\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">").Append(double.Parse(oDetFactShortfall.FacturaUsd).ToString("#,##0.00")).Append("</font></div>");
              sDataProductos.Append("<div style=\"right\"><font style=\"font-family:Arial; color:#ff0000; font-size:10pt;\">(").Append((double.Parse(oDetFactShortfall.FacturaUsd) - double.Parse(oDetFactShortfall.FacturaUsdDf)).ToString("#,##0.00")).Append(")</font></div>");
              sDataProductos.Append("<div style=\"right\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">").Append(double.Parse(oDetFactShortfall.FacturaUsdDf).ToString("#,##0.00")).Append("</font></div>");
            }
            else {
              sDataProductos.Append("<div style=\"right\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">").Append(double.Parse(oDetFactShortfall.FacturaUsdDf).ToString("#,##0.00")).Append("</font></div>");
            }

            sDataProductos.Append("</div>");

            if (iCant <= 8)
              sDataProductos1.Append(sDataProductos.ToString());
            else
              sDataProductos2.Append(sDataProductos.ToString());

            if (string.IsNullOrEmpty(oRow["Descuento"].ToString()))
              sDataValorProductos.Append("<div style=\"height:50px;padding-top:10px;padding-bottom:10px; padding-left:10px;\">");
            else
              sDataValorProductos.Append("<div style=\"height:100px;padding-top:10px;padding-bottom:10px; padding-left:10px;\">");
            sDataValorProductos.Append("<div style=\"float:left;width:30px;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">&nbsp;</font></div>");
            sDataValorProductos.Append("<div style=\"text-align:right;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">").Append(double.Parse(oDetFactShortfall.FacturaUsdDf).ToString("#,##0.00")).Append("</font></div>");
            sDataValorProductos.Append("<div style=\"float:left;width:30px;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">&nbsp;</font></div>");
            sDataValorProductos.Append("<div style=\"text-align:right;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">&nbsp;</font></div>");
            sDataValorProductos.Append("</div>");

            if (iCant <= 8)
              sDataValorProductos1.Append(sDataValorProductos.ToString());
            else
              sDataValorProductos2.Append(sDataValorProductos.ToString());

            iTotalFactura = iTotalFactura + (!string.IsNullOrEmpty(oDetFactShortfall.FacturaUsdDf) ? double.Parse(oDetFactShortfall.FacturaUsdDf) : 0);

            oDetFactShortfall.Accion = "CREAR";
            oDetFactShortfall.Put();

            if (string.IsNullOrEmpty(oDetFactShortfall.Error))
              bExito = false;

            iCant++;
          }
        }
        dtFActShortFall = null;

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
        string sNumInvoce = "FE" + pCodFactura;

        oFactura.Total = iTotalFactura.ToString();
        oFactura.Periodo = "Short Fall / " + hdd_periodo.Value;
        oFactura.NumInvoice = sNumInvoce;
        oFactura.Accion = "EDITAR";
        oFactura.Put();

        cMinimoContrato oMinimoContrato = new cMinimoContrato(ref oConn);
        oMinimoContrato.NumContrato = hdd_num_contrato.Value;
        oMinimoContrato.CodFactShortFall = pCodFactura;
        oMinimoContrato.FechaInicio = DateTime.Parse(hdd_periodo.Value.Substring(0, 10)).ToString("yyyyMMdd");
        oMinimoContrato.FechaFinal = DateTime.Parse(hdd_periodo.Value.Substring(13)).ToString("yyyyMMdd");
        oMinimoContrato.Accion = "EDITAR";
        oMinimoContrato.Put();

        StringBuilder sHtml = new StringBuilder();
        if (iCant <= 8)
          sHtml.Append(File.ReadAllText(Server.MapPath("invoice.html")));
        else
          sHtml.Append(File.ReadAllText(Server.MapPath("invoicetwopage.html")));
        sHtml.Replace("[#TITLE]", "INVOICE");
        sHtml.Replace("[#NUMFACTURA]", sNumInvoce);
        sHtml.Replace("[#NOMEMPRESA]", strNomDeudor);
        sHtml.Replace("[#NOMCONTACTO]", sNomContacto);
        sHtml.Replace("[#EMAILCONTACTO]", sEmailContacto);
        sHtml.Replace("[#DIRECCION]", sDireccion + (!string.IsNullOrEmpty(sComuna) ? ", " + sComuna : string.Empty));
        sHtml.Replace("[#CIUDAD]", sCiudad);
        sHtml.Replace("[#PAIS]", "");
        sHtml.Replace("[#FECHA]", DateTime.Now.ToString("d-MMM-yy", CultureInfo.CreateSpecificCulture("en-US")));
        sHtml.Replace("[#NUMCONTRATO]", hdd_no_contrato.Value);
        sHtml.Replace("[#PERIODOQ]", "Short Fall/" + hdd_periodo.Value);
        sHtml.Replace("[#PROPERTY]", "");
        sHtml.Replace("[#TERRITORIO]", "CHILE");
        sHtml.Replace("[#DETALLEPRODUCTOS]", sDataProductos1.ToString());
        sHtml.Replace("[#DETALLEPRODUCTOS2]", sDataProductos2.ToString());
        sHtml.Replace("[#DETALLEVALORPRODUCTOS]", sDataValorProductos1.ToString());
        sHtml.Replace("[#DETALLEVALORPRODUCTOS2]", sDataValorProductos2.ToString());
        sHtml.Replace("[#DUEDATE]", DateTime.Now.AddMonths(1).ToString("d-MMM-yy", CultureInfo.CreateSpecificCulture("en-US")));
        //sHtml.Replace("[#TOTAL]", double.Parse(dTotalFacturaUsd.ToString()).ToString("#,##0.00"));
        sHtml.Replace("[#TOTAL]", double.Parse(iTotalFactura.ToString()).ToString("N0"));

        if (!Directory.Exists(Server.MapPath("Facturas/") + hdd_no_contrato.Value + "/"))
          Directory.CreateDirectory(Server.MapPath("Facturas/") + hdd_no_contrato.Value + "/");

        string sFileHtml = Server.MapPath("Facturas/") + hdd_no_contrato.Value + "/" + sNumInvoce + ".html";
        File.WriteAllText(sFileHtml, sHtml.ToString(), Encoding.UTF8);

        cLogEventos oLogEventos = new cLogEventos(ref oConn);
        oLogEventos.AccionLog = "APROBAR POR DETALLE PRE-FACTURA SHORT FALL";
        oLogEventos.CodCanal = "2";
        oLogEventos.CodFlujo = "6";
        oLogEventos.NomFlujo = "FACTURACION SHORT FALL";
        oLogEventos.NumContrato = hdd_num_contrato.Value;
        oLogEventos.NoContrato = hdd_no_contrato.Value;
        oLogEventos.PeriodoLog = "Short Fall/" + hdd_periodo.Value;
        //oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
        oLogEventos.CodUser = oUsuario.CodUsuario;
        oLogEventos.RutUser = oUsuario.RutUsuario;
        oLogEventos.NomUser = oUsuario.Nombres;
        oLogEventos.ObsLog = "Se ha autorizado correctamente la Factura Short Fall " + sNumInvoce;
        oLogEventos.IpLog = oWeb.GetIpUsuario();
        oLogEventos.Accion = "CREAR";
        oLogEventos.Put();
      }
      oConn.Close();

      idGrilla.Visible = false;
      idBtnSave.Visible = false;
      StringBuilder js = new StringBuilder();
      js.Append("function LgRespuesta() {");
      js.Append(" window.radalert('La factura ha sido emitida con éxito.', 330, 210); ");
      js.Append(" Sys.Application.remove_load(LgRespuesta); ");
      js.Append("};");
      js.Append("Sys.Application.add_load(LgRespuesta);");
      Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", js.ToString(), true);

    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {
      Response.Redirect("downloadmanual.ashx");
    }

    protected void btnContacto_Click(object sender, EventArgs e)
    {
      Response.Redirect("contacto.aspx");
    }

    protected void btnCalcular_Click(object sender, EventArgs e)
    {
      int iYearInicio = DateTime.Parse(hdd_periodo.Value.Substring(0, 10)).Year;
      int iYearFinal = DateTime.Parse(hdd_periodo.Value.Substring(13, 10)).Year;

      FacturaShortFall oFacturaShortFall = new FacturaShortFall();
      oFacturaShortFall.ViewClient = true;
      oFacturaShortFall.Periodo = hdd_periodo.Value;
      oFacturaShortFall.getMakeTable();

      RadGrid grid = (this.FindControl("idGridShortFall") as RadGrid);
      GridDataItem item;
      for (int x = 0; x < grid.MasterTableView.Items.Count; x++)
      {
        if (grid.MasterTableView.Items[x] is GridDataItem)
        {
          double iTotalUsd = 0;
          item = (GridDataItem)grid.MasterTableView.Items[x];

          oFacturaShortFall.Licenciatario = item["Licenciatario"].Text;
          oFacturaShortFall.NoContrato = item["Contrato"].Text;
          oFacturaShortFall.Marca = item["Marca"].Text;
          oFacturaShortFall.Categoria = item["Categoría"].Text;
          oFacturaShortFall.SubCategoria = item["Subcategoría"].Text;
          oFacturaShortFall.MntMinimoGarantizado = item["Mínimo Garantizado USD"].Text;
          oFacturaShortFall.Periodo = item["Periodo"].Text;
          oFacturaShortFall.MntFactAdvance = item["Factura Advance"].Text;
          
          oFacturaShortFall.MntPeriodoFactUno = "0";
          oFacturaShortFall.MntPeriodoFactDos = "0";
          oFacturaShortFall.MntPeriodoFactTres = "0";
          oFacturaShortFall.MntPeriodoFactCuatro = "0";
          oFacturaShortFall.MntPeriodoFactCinco = "0";

          DateTime fechainicial = DateTime.Parse(hdd_periodo.Value.Substring(0, 10));
          DateTime fechafinal = DateTime.Parse(hdd_periodo.Value.Substring(13));

          int iCantMes = Math.Abs((fechafinal.Month - fechainicial.Month) + 12 * (fechafinal.Year - fechainicial.Year)) + 1;
          int iTrimestres = iCantMes / 3;
          if (iTrimestres > 0)
          {
            DateTime dMonthForQ = fechainicial;
            for (int i = 0; i < iTrimestres; i++)
            {
              dMonthForQ = dMonthForQ.AddMonths(2);
              string sMonto = item["Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString()].Text;
              if (i == 0)
                oFacturaShortFall.MntPeriodoFactUno = (sMonto != "&nbsp;" ? sMonto : null);
              else if (i == 1)
                oFacturaShortFall.MntPeriodoFactDos = (sMonto != "&nbsp;" ? sMonto : null);
              else if (i == 2)
                oFacturaShortFall.MntPeriodoFactTres = (sMonto != "&nbsp;" ? sMonto : null);
              else if (i == 3)
                oFacturaShortFall.MntPeriodoFactCuatro = (sMonto != "&nbsp;" ? sMonto : null);

              dMonthForQ = dMonthForQ.AddMonths(1);
            }
          }
          else
          {
            DateTime iMonth = fechainicial.AddMonths(2);
            string sMonto = item["Factura " + oWeb.getPeriodoByNumMonth(iMonth.Month) + "/" + iMonth.Year.ToString()].Text;
            oFacturaShortFall.MntPeriodoFactUno = (sMonto != "&nbsp;" ? sMonto : null);
          }

          iTotalUsd = (!string.IsNullOrEmpty(oFacturaShortFall.MntFactAdvance) ? double.Parse(oFacturaShortFall.MntFactAdvance) : 0);
          iTotalUsd = iTotalUsd - (!string.IsNullOrEmpty(oFacturaShortFall.MntPeriodoFactUno) ? double.Parse(oFacturaShortFall.MntPeriodoFactUno) : 0);
          iTotalUsd = iTotalUsd - (!string.IsNullOrEmpty(oFacturaShortFall.MntPeriodoFactDos) ? double.Parse(oFacturaShortFall.MntPeriodoFactDos) : 0);
          iTotalUsd = iTotalUsd - (!string.IsNullOrEmpty(oFacturaShortFall.MntPeriodoFactTres) ? double.Parse(oFacturaShortFall.MntPeriodoFactTres) : 0);
          iTotalUsd = iTotalUsd - (!string.IsNullOrEmpty(oFacturaShortFall.MntPeriodoFactCuatro) ? double.Parse(oFacturaShortFall.MntPeriodoFactCuatro) : 0);

          //if (iTotalUsd < 0)
          //  iTotalUsd = 0;

          oFacturaShortFall.FacturaUsd = item["Factura USD"].Text;

          string sTxtDescuento = null;

          if (double.Parse(item["Factura USD"].Text) > 0)
            sTxtDescuento = (!string.IsNullOrEmpty(((RadTextBox)item.FindControl("txt_descuento")).Text) ? ((RadTextBox)item.FindControl("txt_descuento")).Text : null);
          else
            ((RadTextBox)item.FindControl("txt_descuento")).Text = string.Empty;

          oFacturaShortFall.Descuento = sTxtDescuento;

          double dFacturaUsdCorregida = 0;
          if ((!string.IsNullOrEmpty(sTxtDescuento)) && (double.Parse(item["Factura USD"].Text) > 0))
          {
            double dPorcentaje = double.Parse(sTxtDescuento) / 100;
            double dDescuento = double.Parse(item["Factura USD"].Text) * dPorcentaje;
            dFacturaUsdCorregida = double.Parse(item["Factura USD"].Text) - dDescuento;
          }
          else if (double.Parse(item["Factura USD"].Text) > 0)
            dFacturaUsdCorregida = double.Parse(item["Factura USD"].Text);
          
          oFacturaShortFall.MntPeriodoFactCinco = iTotalUsd.ToString();
          oFacturaShortFall.FacturaUsdCorregida = dFacturaUsdCorregida.ToString();

          oFacturaShortFall.AddRow();
        }
      }

      DataTable dtFacturaShortFall = oFacturaShortFall.Get();
      grid.DataSource = dtFacturaShortFall;
      grid.Rebind();
      ViewState["dtFactShortFall"] = dtFacturaShortFall;

    }

    protected DataTable getMarca(string pCodMarca)
    {
      DataTable dtMarca = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cMarcas oMarcas = new cMarcas(ref oConn);
        oMarcas.Descripcion = pCodMarca;
        dtMarca = oMarcas.GetByDescripcion();

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
        oCategorias.Descripcion = pCodCategoria;
        dtCategoria = oCategorias.GetByDescripcion();

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
        oSubCategoria.Descripcion = pCodSubCategoria;
        dtSubCategoria = oSubCategoria.GetByDescripcion();
      }
      oConn.Close();
      return dtSubCategoria;
    }
  }
}