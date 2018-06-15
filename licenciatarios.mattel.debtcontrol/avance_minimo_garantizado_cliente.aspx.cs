using System;
using System.Collections.Generic;
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
  public partial class avance_minimo_garantizado_cliente : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
      if (!IsPostBack)
      {
        loadLicenciatarios();
        //int sYear = 2015;
        //int aYear = DateTime.Now.Year;
        //while (sYear <= aYear)
        //{
        //  ddlist_ano_ini.Items.Add(new ListItem(sYear.ToString(), sYear.ToString()));
        //  sYear++;
        //}
        cmbox_contrato.Items.Clear();
        cmbox_contrato.Items.Add(new ListItem("<< Todos los Contratos >>", ""));
        loadPeriodos();
      }
    }

    protected void loadLicenciatarios()
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cDeudor oDeudor = new cDeudor(ref oConn);
        DataTable dtDeudor = oDeudor.GetByReporte();
        if (dtDeudor != null)
        {
          if (dtDeudor.Rows.Count > 0)
          {
            cmbox_licenciatario.Items.Clear();
            cmbox_licenciatario.Items.Add(new ListItem("<< Todos >>", ""));
            foreach (DataRow oRow in dtDeudor.Rows)
            {
              cmbox_licenciatario.Items.Add(new ListItem(oRow["snombre"].ToString(), oRow["nKey_Deudor"].ToString()));
            }
          }
          else
          {
            cmbox_licenciatario.Items.Clear();
            cmbox_licenciatario.Items.Add(new ListItem("<< No Existen Licenciatarios >>", "0"));
          }
        }
        dtDeudor = null;

      }
      oConn.Close();
    }

    protected void loadPeriodos()
    {
      cmbox_periodo_minimo.Items.Clear();
      cmbox_periodo_minimo.Items.Add(new ListItem("<< Seleccione Periodo >>", ""));
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cMinimoContrato oMinimoContrato = new cMinimoContrato(ref oConn);
        oMinimoContrato.Ano = "2015";
        oMinimoContrato.NumContrato = cmbox_contrato.SelectedValue;
        oMinimoContrato.NkeyDeudor = cmbox_licenciatario.SelectedValue;
        DataTable dtMinimo = oMinimoContrato.GetPeriodo();
        if (dtMinimo != null)
        {
          foreach (DataRow oRow in dtMinimo.Rows)
          {
            cmbox_periodo_minimo.Items.Add(new ListItem(oRow["periodo"].ToString(), oRow["periodo"].ToString()));
          }
        }
        dtMinimo = null;
      }
      oConn.Close();
    }

    protected void cmbox_licenciatario_SelectedIndexChanged(object sender, EventArgs e)
    {
      cmbox_contrato.Items.Clear();
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cContratos oContratos = new cContratos(ref oConn);
        oContratos.NkeyDeudor = cmbox_licenciatario.SelectedValue;
        oContratos.NoAprobado = true;
        DataTable dtContrato = oContratos.Get();
        if (dtContrato != null)
        {
          if (dtContrato.Rows.Count > 0)
          {
            if (dtContrato.Rows.Count > 0)
            {
              cmbox_contrato.Items.Add(new ListItem("<< Todos los Contratos >>", ""));
              foreach (DataRow oRow in dtContrato.Rows)
              {
                cmbox_contrato.Items.Add(new ListItem(oRow["no_contrato"].ToString(), oRow["num_contrato"].ToString()));
              }
            }
          }
          else
          {
            cmbox_contrato.Items.Add(new ListItem("<< No Existen Contratos >>", "0"));
          }
        }
        dtContrato = null;

        loadPeriodos();
      }
      oConn.Close();
    }

    protected void btnContacto_Click(object sender, EventArgs e)
    {
      Response.Redirect("contacto.aspx");
    }

    protected void cmbox_contrato_SelectedIndexChanged(object sender, EventArgs e)
    {
      loadPeriodos();
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      getDataMinimiGaranty();
    }

    protected void getDataMinimiGaranty()
    {

      MinimoGarantizado oMinimoGarantizado = new MinimoGarantizado();
      oMinimoGarantizado.ViewClient = true;
      oMinimoGarantizado.Periodo = cmbox_periodo_minimo.SelectedValue;
      oMinimoGarantizado.getMakeTable();
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        DateTime dFechaIni = DateTime.Parse(cmbox_periodo_minimo.SelectedValue.Substring(0, 10));
        DateTime dFechaFin = DateTime.Parse(cmbox_periodo_minimo.SelectedValue.Substring(13, 10));

        cMinimoContrato oMinimoContrato = new cMinimoContrato(ref oConn);
        oMinimoContrato.NkeyDeudor = cmbox_licenciatario.SelectedValue;
        oMinimoContrato.NumContrato = cmbox_contrato.SelectedValue;
        oMinimoContrato.FechaInicio = dFechaIni.ToString("yyyyMMdd");
        oMinimoContrato.FechaFinal = dFechaFin.ToString("yyyyMMdd");

        DataTable dtMinimo = oMinimoContrato.Get();
        if (dtMinimo != null)
        {
          foreach (DataRow oRow in dtMinimo.Rows)
          {
            int iYearInicio = DateTime.Parse(cmbox_periodo_minimo.SelectedValue.Substring(0, 10)).Year;
            int iYearFinal = DateTime.Parse(cmbox_periodo_minimo.SelectedValue.Substring(13, 10)).Year;

            cFactura oFactura = new cFactura(ref oConn);
            oFactura.NumContrato = (!string.IsNullOrEmpty(cmbox_contrato.SelectedValue) ? cmbox_contrato.SelectedValue : oRow["num_contrato"].ToString());
            oFactura.TipoFactura = "T";
            oFactura.PeriodoAno = (iYearInicio.ToString() == iYearFinal.ToString() ? "'" + iYearInicio.ToString() + "'" : "'" + iYearInicio.ToString() + "','" + iYearFinal.ToString() + "'");

            DataTable dtFactura = oFactura.GetForMinimo();
            if (dtFactura != null)
            {
              if (dtFactura.Rows.Count > 0)
              {
                double iTotal = 0;

                oMinimoGarantizado.Licenciatario = oRow["licenciatario"].ToString();
                oMinimoGarantizado.NoContrato = oRow["contrato"].ToString();
                oMinimoGarantizado.Marca = oRow["marca"].ToString();
                oMinimoGarantizado.Categoria = oRow["categoria"].ToString();
                oMinimoGarantizado.SubCategoria = oRow["subcategoria"].ToString();
                oMinimoGarantizado.MinimoUsd = oRow["minimo"].ToString();

                oMinimoGarantizado.Advance = "0";
                oMinimoGarantizado.PeriodoUno = "0";
                oMinimoGarantizado.PeriodoDos = "0";
                oMinimoGarantizado.PeriodoTres = "0";
                oMinimoGarantizado.PeriodoCuatro = "0";

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
                      oMinimoGarantizado.Advance = dtDetalleFactAdvance.Rows[0]["advance_usd"].ToString();
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
                      DateTime fecha_inicial = DateTime.Parse(cmbox_periodo_minimo.SelectedValue.Substring(0, 10));
                      DateTime fecha_final = DateTime.Parse(cmbox_periodo_minimo.SelectedValue.Substring(13));

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
                              oMinimoGarantizado.PeriodoUno = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString();
                            //oMinimoGarantizado.PeriodoUno = dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                            else if (i == 1)
                              oMinimoGarantizado.PeriodoDos = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString();
                            else if (i == 2)
                              oMinimoGarantizado.PeriodoTres = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString();
                            else if (i == 3)
                              oMinimoGarantizado.PeriodoCuatro = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString();
                          }
                          dMonthForQ = dMonthForQ.AddMonths(1);
                        }
                      }
                      else
                      {
                        oMinimoGarantizado.PeriodoUno = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString();
                      }                      
                    }
                  }
                  dtDetalleFactura = null;
                }

                iTotal = double.Parse(oMinimoGarantizado.MinimoUsd) - (!string.IsNullOrEmpty(oMinimoGarantizado.Advance) ? double.Parse(oMinimoGarantizado.Advance) : 0);
                iTotal = iTotal - (!string.IsNullOrEmpty(oMinimoGarantizado.PeriodoUno) ? double.Parse(oMinimoGarantizado.PeriodoUno) : 0);
                iTotal = iTotal - (!string.IsNullOrEmpty(oMinimoGarantizado.PeriodoDos) ? double.Parse(oMinimoGarantizado.PeriodoDos) : 0);
                iTotal = iTotal - (!string.IsNullOrEmpty(oMinimoGarantizado.PeriodoTres) ? double.Parse(oMinimoGarantizado.PeriodoTres) : 0);
                iTotal = iTotal - (!string.IsNullOrEmpty(oMinimoGarantizado.PeriodoCuatro) ? double.Parse(oMinimoGarantizado.PeriodoCuatro) : 0);

                if (iTotal < 0)
                  iTotal = 0;

                oMinimoGarantizado.Saldo = iTotal.ToString();
                oMinimoGarantizado.AddRow();
              }
            }
            dtFactura = null;
          }
        }
        dtMinimo = null;
      }
      oConn.Close();

      DataTable dtMinimoGarantizado = oMinimoGarantizado.Get();


      RadGrid oGridMinimo = new RadGrid();
      oGridMinimo.ShowStatusBar = true;
      oGridMinimo.ShowFooter = true;
      oGridMinimo.AutoGenerateColumns = false;
      oGridMinimo.Skin = "Sitefinity";
      //oGridMinimo.ItemDataBound += oGridMinimo_ItemDataBound;
      oGridMinimo.MasterTableView.AutoGenerateColumns = false;
      oGridMinimo.MasterTableView.ShowHeader = true;
      oGridMinimo.MasterTableView.ShowFooter = true;
      oGridMinimo.MasterTableView.TableLayout = GridTableLayout.Fixed;
      //oGridMinimo.MasterTableView.EditMode = GridEditMode.EditForms;
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
      oGridBoundColumn.DataFormatString = "{0:N0}";
      oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
      oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Factura Advance";
      oGridBoundColumn.HeaderText = "Factura Advance";
      oGridBoundColumn.UniqueName = "Factura Advance";
      oGridBoundColumn.DataFormatString = "{0:N}";
      oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
      oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      DateTime fechainicial = DateTime.Parse(cmbox_periodo_minimo.SelectedValue.Substring(0, 10));
      DateTime fechafinal = DateTime.Parse(cmbox_periodo_minimo.SelectedValue.Substring(13));

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
        oGridBoundColumn.DataFormatString = "{0:N}";
        oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
        oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
        oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);
      }

      oGridBoundColumn = new GridBoundColumn();
      oGridBoundColumn.DataField = "Saldo";
      oGridBoundColumn.HeaderText = "Saldo";
      oGridBoundColumn.UniqueName = "Saldo";
      oGridBoundColumn.DataFormatString = "{0:N0}";
      oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
      oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
      oGridMinimo.MasterTableView.Columns.Add(oGridBoundColumn);

      oGridMinimo.DataSource = dtMinimoGarantizado;
      oGridMinimo.Rebind();
      idGrilla.Visible = true;
      idGrilla.Controls.Add(oGridMinimo);

    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {
      Response.Redirect("downloadmanual.ashx");
    }

  }
}