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
  public partial class aprobar_facturas_short_fall : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
    }

    protected DataTable getShortFallForApprove()
    {
      DataTable dtApproveSF = null;
      viewFacturaShortFall oViewFacturaShorFall = new viewFacturaShortFall();
      oViewFacturaShorFall.getMakeTable();

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cMinimoContrato oMinimoContrato = new cMinimoContrato(ref oConn);
        oMinimoContrato.bIsNullFact = true;
        DataTable dtMinimoContrato = oMinimoContrato.GetMinimoForShortFall();
        if (dtMinimoContrato != null)
        {
          if (dtMinimoContrato.Rows.Count > 0)
          {
            foreach (DataRow oRow in dtMinimoContrato.Rows)
            {
              DateTime fecha_inicial = DateTime.Parse(oRow["dinicio"].ToString());
              DateTime fecha_final = DateTime.Parse(oRow["dfinal"].ToString());

              string sPeriodo = string.Empty;
              int lngCantMes = Math.Abs((fecha_final.Month - fecha_inicial.Month) + 12 * (fecha_final.Year - fecha_inicial.Year)) + 1;
              int lngTrimestres = lngCantMes / 3;

              DateTime dMonthForQ = fecha_inicial;
              if (lngTrimestres > 0)
              {
                int i = 0;
                for (i = 0; i < lngTrimestres; i++)
                {
                  dMonthForQ = dMonthForQ.AddMonths(2);
                  sPeriodo = (!string.IsNullOrEmpty(sPeriodo) ? sPeriodo + "," : string.Empty) + "'" + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString() + "'";
                  dMonthForQ = dMonthForQ.AddMonths(1);
                }
              }
              else
              {
                sPeriodo = "'" + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString() + "'";
              }

              cFactura oFactura = new cFactura(ref oConn);
              oFactura.NumContrato = oRow["num_contrato"].ToString();
              oFactura.Periodo = sPeriodo;
              DataTable dtFactura = oFactura.GetByPeriodo();

              if (dtFactura != null)
              {
                if ((dtFactura.Rows.Count > 0) && (int.Parse(dtFactura.Rows[0]["cantidad"].ToString()) == lngTrimestres))
                {
                  oViewFacturaShorFall.Licenciatario = oRow["licenciatario"].ToString();
                  oViewFacturaShorFall.NumContrato = oRow["num_contrato"].ToString();
                  oViewFacturaShorFall.NoContrato = oRow["no_contrato"].ToString();
                  oViewFacturaShorFall.Inicio = oRow["dinicio"].ToString();
                  oViewFacturaShorFall.Final = oRow["dfinal"].ToString();
                  oViewFacturaShorFall.AddRow();
                }
              }
              dtFactura = null;
            }
          }
        }
        dtMinimoContrato = null;
      }
      oConn.Close();

      dtApproveSF = oViewFacturaShorFall.Get();
      return dtApproveSF;
    }

    protected void btnAprobar_Click(object sender, EventArgs e)
    {
      DataTable dtReporteVenta = getShortFallForApprove();
      if (dtReporteVenta != null)
      {
        foreach (DataRow oRow in dtReporteVenta.Rows)
        {
          putAprobar(oRow["num_contrato"].ToString(), oRow["dinicio"].ToString() + " - " + oRow["dfinal"].ToString(), oRow["contrato"].ToString());
        }
        StringBuilder js = new StringBuilder();
        js.Append("function LgRespuesta() {");
        js.Append(" window.radalert('Las facturas de todos los periodos han sido emitidas con éxito.', 330, 210); ");
        js.Append(" Sys.Application.remove_load(LgRespuesta); ");
        js.Append("};");
        js.Append("Sys.Application.add_load(LgRespuesta);");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", js.ToString(), true);
        rdGridShortFall.Rebind();
      }
      dtReporteVenta = null;
    }

    protected void rdGridShortFall_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      rdGridShortFall.DataSource = getShortFallForApprove();
    }

    protected void rdGridShortFall_ItemCommand(object source, GridCommandEventArgs e)
    {
      if (e.CommandName == "AprobarFactura")
      {
        GridDataItem item = (GridDataItem)e.Item;
        string pNumContrato = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["num_contrato"].ToString();
        string pPeriodo = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["dinicio"].ToString() + " - " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["dfinal"].ToString();
        string pNoContrato = item["no_contrato"].Text;

        StringBuilder sUrl = new StringBuilder();
        sUrl.Append("aprobar_facturas_short_fall_detalle.aspx?pNumContrato=").Append(pNumContrato);
        sUrl.Append("&pNoContrato=").Append(pNoContrato);
        sUrl.Append("&pPeriodo=").Append(pPeriodo);
        Response.Redirect(sUrl.ToString());
      }
      else if (e.CommandName == "Aprobar")
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;
        string pNumContrato = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["num_contrato"].ToString();
        string pPeriodo = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["dinicio"].ToString() + " - " + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["dfinal"].ToString();

        putAprobar(pNumContrato, pPeriodo, item["no_contrato"].Text);
        rdGridShortFall.Rebind();
      }
    }

    protected void rdGridShortFall_ItemDataBound(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;

        string pNumContrato = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["num_contrato"].ToString();

        item["dinicio"].Text = DateTime.Parse(row["dinicio"].ToString()).ToString("dd/MM/yyyy");
        item["dfinal"].Text = DateTime.Parse(row["dfinal"].ToString()).ToString("dd/MM/yyyy");

        double pMonto = getShortFall(pNumContrato, row["dinicio"].ToString() + " - " + row["dfinal"].ToString());
        //if (pMonto < 0) {
        //  pMonto = 0;
        //  (item["BtnAprobarFactura"].FindControl("BtnAprobarFactura") as ImageButton).Visible = false;
        //  (item["BtnAprobar"].FindControl("btnGridAprobar") as Button).Visible = false;
        //  (item["BtnRechazar"].FindControl("btnGridRechazar") as Button).Visible = false;
        //}

        item["monto"].Text = pMonto.ToString("N0");
      }
    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {
      Response.Redirect("downloadmanual.ashx");
    }

    protected void btnContacto_Click(object sender, EventArgs e)
    {
      Response.Redirect("contacto.aspx");
    }

    protected double getShortFall(string pNumContrato, string pPeriodo)
    {
      double dMonto = 0;
      bool bComplete = false;
      FacturaShortFall oFacturaShortFall = new FacturaShortFall();
      oFacturaShortFall.ViewClient = true;
      oFacturaShortFall.Periodo = pPeriodo;
      oFacturaShortFall.getMakeTable();

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        DateTime dFechaIni = DateTime.Parse(pPeriodo.Substring(0, 10));
        DateTime dFechaFin = DateTime.Parse(pPeriodo.Substring(13, 10));

        string pNKeyDeudor = null;
        cContratos oContrato = new cContratos(ref oConn);
        oContrato.NumContrato = pNumContrato;
        DataTable dtContrato = oContrato.Get();
        if (dtContrato != null)
        {
          if (dtContrato.Rows.Count > 0)
          {
            pNKeyDeudor = dtContrato.Rows[0]["nkey_deudor"].ToString();
          }
        }
        dtContrato = null;

        cMinimoContrato oMinimoContrato = new cMinimoContrato(ref oConn);
        oMinimoContrato.NkeyDeudor = pNKeyDeudor;
        oMinimoContrato.NumContrato = pNumContrato;
        oMinimoContrato.FechaInicio = dFechaIni.ToString("yyyyMMdd");
        oMinimoContrato.FechaFinal = dFechaFin.ToString("yyyyMMdd");
        DataTable dtMinimo = oMinimoContrato.Get();

        if (dtMinimo != null)
        {
          foreach (DataRow oRow in dtMinimo.Rows)
          {
            int iYearInicio = DateTime.Parse(pPeriodo.Substring(0, 10)).Year;
            int iYearFinal = DateTime.Parse(pPeriodo.Substring(13, 10)).Year;

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
            oFactura.NumContrato = (!string.IsNullOrEmpty(pNumContrato) ? pNumContrato : oRow["num_contrato"].ToString());
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
                      DateTime fecha_inicial = DateTime.Parse(pPeriodo.Substring(0, 10));
                      DateTime fecha_final = DateTime.Parse(pPeriodo.Substring(13));

                      int lngCantMes = Math.Abs((fecha_final.Month - fecha_inicial.Month) + 12 * (fecha_final.Year - fecha_inicial.Year)) + 1;
                      int lngTrimestres = lngCantMes / 3;

                      if (lngTrimestres > 0)
                      {
                        DateTime dMonthForQ = fecha_inicial;
                        int i = 0;
                        for (i = 0; i < lngTrimestres; i++)
                        {
                          dMonthForQ = dMonthForQ.AddMonths(2);
                          if (oRowFact["periodo"].ToString() == (oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString()))
                          {
                            if (i == 0)
                              oFacturaShortFall.MntPeriodoFactUno = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); //dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                            else if (i == 1)
                              oFacturaShortFall.MntPeriodoFactDos = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); //dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                            else if (i == 2)
                              oFacturaShortFall.MntPeriodoFactTres = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); //dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                            else if (i == 3)
                              oFacturaShortFall.MntPeriodoFactCuatro = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); //dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                          }
                          dMonthForQ = dMonthForQ.AddMonths(1);
                        }
                        if (lngTrimestres == i)
                          bComplete = true;
                      }
                      else
                      {
                        oFacturaShortFall.MntPeriodoFactUno = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); //dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                        bComplete = true;
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

                if (iTotal > 0)
                  dMonto = dMonto + iTotal;

                oFacturaShortFall.AddRow();
              }
            }
            dtFactura = null;
          }
        }
        dtMinimo = null;
      }
      oConn.Close();

      //DataTable dtFactShortFall = oFacturaShortFall.Get();
      if (!bComplete)
        dMonto = -1;
      return dMonto;
    }

    protected void putAprobar(string pNumContrato, string pPeriodo, string sNomContrato)
    {
      int iCant = 0;
      StringBuilder sDataProductos1 = new StringBuilder();
      StringBuilder sDataProductos2 = new StringBuilder();

      StringBuilder sDataValorProductos1 = new StringBuilder();
      StringBuilder sDataValorProductos2 = new StringBuilder();
      string sPeriodo = string.Empty;
      string sNumInvoce = string.Empty;

      //FacturaShortFall oFacturaShortFall = new FacturaShortFall();
      //oFacturaShortFall.ViewClient = true;
      //oFacturaShortFall.Periodo = pPeriodo;
      //oFacturaShortFall.getMakeTable();

      double iTotalFactura = 0;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        DateTime dFechaIni = DateTime.Parse(pPeriodo.Substring(0, 10));
        DateTime dFechaFin = DateTime.Parse(pPeriodo.Substring(13, 10));

        cFactura oFacturaSFall = new cFactura(ref oConn);
        oFacturaSFall.NumContrato = pNumContrato;
        oFacturaSFall.Territory = "CHILE";
        oFacturaSFall.DateInvoce = DateTime.Now.ToString("yyyy-MM-dd");
        oFacturaSFall.DueDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
        oFacturaSFall.TipoFactura = "S";
        oFacturaSFall.Accion = "CREAR";
        oFacturaSFall.Put();

        //if (string.IsNullOrEmpty(oFacturaSFall.Error))
        //bExito = true;

        string pCodFactura = oFacturaSFall.CodFactura;
        cDetFactShortfall oDetFactShortfall = new cDetFactShortfall(ref oConn);

        string strNomDeudor = string.Empty;
        string sNomContacto = string.Empty;
        string sEmailContacto = string.Empty;
        string sDireccion = string.Empty;
        string sComuna = string.Empty;
        string sCiudad = string.Empty;
        string pNKeyDeudor = null;

        cContratos oContrato = new cContratos(ref oConn);
        oContrato.NumContrato = pNumContrato;
        DataTable dtContrato = oContrato.Get();
        if (dtContrato != null)
        {
          if (dtContrato.Rows.Count > 0)
          {
            pNKeyDeudor = dtContrato.Rows[0]["nkey_deudor"].ToString();

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

        cMinimoContrato oMinimoContrato = new cMinimoContrato(ref oConn);
        oMinimoContrato.NkeyDeudor = pNKeyDeudor;
        oMinimoContrato.NumContrato = pNumContrato;
        oMinimoContrato.FechaInicio = dFechaIni.ToString("yyyyMMdd");
        oMinimoContrato.FechaFinal = dFechaFin.ToString("yyyyMMdd");
        DataTable dtMinimo = oMinimoContrato.Get();

        if (dtMinimo != null)
        {
          foreach (DataRow oRow in dtMinimo.Rows)
          {
            StringBuilder sDataProductos = new StringBuilder();
            StringBuilder sDataValorProductos = new StringBuilder();

            int iYearInicio = DateTime.Parse(pPeriodo.Substring(0, 10)).Year;
            int iYearFinal = DateTime.Parse(pPeriodo.Substring(13, 10)).Year;

            oDetFactShortfall.CodigoFactura = pCodFactura;
            oDetFactShortfall.NumContrato = pNumContrato;

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
            if (!string.IsNullOrEmpty(oRow["categoria"].ToString()))
            {
              DataTable dtCategoria = getCategoria(oRow["categoria"].ToString());
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
            if (!string.IsNullOrEmpty(oRow["subcategoria"].ToString()))
            {
              DataTable dtSubCategoria = getSubCategoria(oRow["subcategoria"].ToString());
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

            sDataProductos.Append("<div style=\"height:50px;padding-top:10px;padding-bottom:10px;float:left;width:85%\">");
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
            sDataProductos.Append("</div>");
            sDataProductos.Append("<div style=\"height:50px;padding-top:10px; padding-bottom:10px; \">");
            sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;font-weight:bold;\">").Append(double.Parse(oRow["minimo"].ToString()).ToString("#,##0.00")).Append("</font></div>");

            oDetFactShortfall.MntMinGarantizado = oRow["minimo"].ToString();
            oDetFactShortfall.MntFactAdvance = null;

            //oFacturaShortFall.Licenciatario = oRow["licenciatario"].ToString();
            //oFacturaShortFall.NoContrato = oRow["contrato"].ToString();
            //oFacturaShortFall.Marca = oRow["marca"].ToString();
            //oFacturaShortFall.Categoria = oRow["categoria"].ToString();
            //oFacturaShortFall.SubCategoria = oRow["subcategoria"].ToString();
            //oFacturaShortFall.MntMinimoGarantizado = oRow["minimo"].ToString();
            //oFacturaShortFall.Periodo = "Short Fall/" + iYearFinal.ToString();

            cFactura oFactura = new cFactura(ref oConn);
            oFactura.NumContrato = (!string.IsNullOrEmpty(pNumContrato) ? pNumContrato : oRow["num_contrato"].ToString());
            oFactura.TipoFactura = "T";
            oFactura.PeriodoAno = (iYearInicio.ToString() == iYearFinal.ToString() ? "'" + iYearInicio.ToString() + "'" : "'" + iYearInicio.ToString() + "','" + iYearFinal.ToString() + "'");

            DataTable dtFactura = oFactura.GetForMinimo();
            if (dtFactura != null)
            {
              if (dtFactura.Rows.Count > 0)
              {
                double iTotal = 0;
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
                      oDetFactShortfall.MntFactAdvance = dtDetalleFactAdvance.Rows[0]["advance_usd"].ToString();
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
                      DateTime fecha_inicial = DateTime.Parse(pPeriodo.Substring(0, 10));
                      DateTime fecha_final = DateTime.Parse(pPeriodo.Substring(13));

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
                            {
                              oDetFactShortfall.PeriodoFactUno = "Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString();
                              oDetFactShortfall.MntPeriodoFactUno = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); //dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                            }
                            else if (i == 1)
                            {
                              oDetFactShortfall.PeriodoFactDos = "Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString();
                              oDetFactShortfall.MntPeriodoFactDos = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); //dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                            }
                            else if (i == 2)
                            {
                              oDetFactShortfall.PeriodoFactTres = "Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString();
                              oDetFactShortfall.MntPeriodoFactTres = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); //dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                            }
                            else if (i == 3)
                            {
                              oDetFactShortfall.PeriodoFactCuatro = "Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString();
                              oDetFactShortfall.MntPeriodoFactCuatro = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); //dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                            }
                          }
                          dMonthForQ = dMonthForQ.AddMonths(1);
                        }
                      }
                      else
                      {
                        DateTime iMonth = fecha_inicial.AddMonths(2);
                        string sMonto = oRow["Factura " + oWeb.getPeriodoByNumMonth(iMonth.Month) + "/" + iMonth.Year.ToString()].ToString();

                        oDetFactShortfall.PeriodoFactUno = "Factura " + oWeb.getPeriodoByNumMonth(iMonth.Month) + "/" + iMonth.Year.ToString();
                        oDetFactShortfall.MntPeriodoFactUno = (double.Parse(dtDetalleFactura.Rows[0]["monto_royalty_usd"].ToString()) - double.Parse(dtDetalleFactura.Rows[0]["saldo_advance_usd"].ToString())).ToString(); //dtDetalleFactura.Rows[0]["factura_usd"].ToString();
                      }
                    }
                  }
                  dtDetalleFactura = null;
                }
                iTotal = double.Parse(oDetFactShortfall.MntMinGarantizado) - (!string.IsNullOrEmpty(oDetFactShortfall.MntFactAdvance) ? double.Parse(oDetFactShortfall.MntFactAdvance) : 0);
                iTotal = iTotal - (!string.IsNullOrEmpty(oDetFactShortfall.MntPeriodoFactUno) ? double.Parse(oDetFactShortfall.MntPeriodoFactUno) : 0);
                iTotal = iTotal - (!string.IsNullOrEmpty(oDetFactShortfall.MntPeriodoFactDos) ? double.Parse(oDetFactShortfall.MntPeriodoFactDos) : 0);
                iTotal = iTotal - (!string.IsNullOrEmpty(oDetFactShortfall.MntPeriodoFactTres) ? double.Parse(oDetFactShortfall.MntPeriodoFactTres) : 0);
                iTotal = iTotal - (!string.IsNullOrEmpty(oDetFactShortfall.MntPeriodoFactCuatro) ? double.Parse(oDetFactShortfall.MntPeriodoFactCuatro) : 0);

                //if (iTotal < 0)
                //  iTotal = 0;

                iTotalUsd = (!string.IsNullOrEmpty(oDetFactShortfall.MntFactAdvance) ? double.Parse(oDetFactShortfall.MntFactAdvance) : 0);
                iTotalUsd = iTotalUsd - (!string.IsNullOrEmpty(oDetFactShortfall.MntPeriodoFactUno) ? double.Parse(oDetFactShortfall.MntPeriodoFactUno) : 0);
                iTotalUsd = iTotalUsd - (!string.IsNullOrEmpty(oDetFactShortfall.MntPeriodoFactDos) ? double.Parse(oDetFactShortfall.MntPeriodoFactDos) : 0);
                iTotalUsd = iTotalUsd - (!string.IsNullOrEmpty(oDetFactShortfall.MntPeriodoFactTres) ? double.Parse(oDetFactShortfall.MntPeriodoFactTres) : 0);
                iTotalUsd = iTotalUsd - (!string.IsNullOrEmpty(oDetFactShortfall.MntPeriodoFactCuatro) ? double.Parse(oDetFactShortfall.MntPeriodoFactCuatro) : 0);

                //if (iTotalUsd < 0)
                //  iTotalUsd = 0;

                //oFacturaShortFall.FacturaUsd = iTotal.ToString();
                //oFacturaShortFall.MntPeriodoFactCinco = iTotalUsd.ToString();
                //oFacturaShortFall.Descuento = null;
                //oFacturaShortFall.FacturaUsdCorregida = iTotal.ToString();

                //oFacturaShortFall.AddRow();

                oDetFactShortfall.FacturaUsd = iTotal.ToString();

                oDetFactShortfall.MntDescuento = null;

                oDetFactShortfall.FacturaUsdDf = (iTotal < 0 ? "0" : iTotal.ToString());

                sDataProductos.Append("<div><font style=\"font-family:Arial; color:#ff0000; font-size:10pt;\">(").Append(double.Parse(iTotalUsd.ToString()).ToString("#,##0.00")).Append(")</font></div>");
                sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">").Append(double.Parse(oDetFactShortfall.FacturaUsdDf).ToString("#,##0.00")).Append("</font></div>");
                sDataProductos.Append("</div>");

                if (iCant <= 8)
                  sDataProductos1.Append(sDataProductos.ToString());
                else
                  sDataProductos2.Append(sDataProductos.ToString());

                sDataValorProductos.Append("<div style=\"height:50px;padding-top:10px;padding-bottom:10px; padding-left:10px;\">");
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

                //if (string.IsNullOrEmpty(oDetFactShortfall.Error))
                //  bExito = false;

              }
            }
            dtFactura = null;
          }
        }
        dtMinimo = null;

        //string sNumInvoce = strNomDeudor.Substring(0, 3).ToUpper();
        //sNumInvoce = sNumInvoce.ToUpper();
        //string sNumInvoce = "FE" + int.Parse(pCodFactura).ToString("D10");
        sNumInvoce = "FE" + pCodFactura;

        oFacturaSFall.Total = iTotalFactura.ToString();
        oFacturaSFall.Periodo = "Short Fall / " + pPeriodo;
        oFacturaSFall.NumInvoice = sNumInvoce;
        oFacturaSFall.Accion = "EDITAR";
        oFacturaSFall.Put();

        oMinimoContrato = new cMinimoContrato(ref oConn);
        oMinimoContrato.NumContrato = pNumContrato;
        oMinimoContrato.CodFactShortFall = pCodFactura;
        oMinimoContrato.FechaInicio = DateTime.Parse(pPeriodo.Substring(0, 10)).ToString("yyyyMMdd");
        oMinimoContrato.FechaFinal = DateTime.Parse(pPeriodo.Substring(13)).ToString("yyyyMMdd");
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
        sHtml.Replace("[#NUMCONTRATO]", sNomContrato);
        sHtml.Replace("[#PERIODOQ]", "Short Fall/" + pPeriodo);
        sPeriodo = "Short Fall/" + pPeriodo;
        sHtml.Replace("[#PROPERTY]", "");
        sHtml.Replace("[#TERRITORIO]", "CHILE");
        sHtml.Replace("[#DETALLEPRODUCTOS]", sDataProductos1.ToString());
        sHtml.Replace("[#DETALLEPRODUCTOS2]", sDataProductos2.ToString());
        sHtml.Replace("[#DETALLEVALORPRODUCTOS]", sDataValorProductos1.ToString());
        sHtml.Replace("[#DETALLEVALORPRODUCTOS2]", sDataValorProductos2.ToString());
        sHtml.Replace("[#DUEDATE]", DateTime.Now.AddMonths(1).ToString("d-MMM-yy", CultureInfo.CreateSpecificCulture("en-US")));
        //sHtml.Replace("[#TOTAL]", double.Parse(dTotalFacturaUsd.ToString()).ToString("#,##0.00"));
        sHtml.Replace("[#TOTAL]", double.Parse(iTotalFactura.ToString()).ToString("N0"));

        if (!Directory.Exists(Server.MapPath("Facturas/") + sNomContrato + "/"))
          Directory.CreateDirectory(Server.MapPath("Facturas/") + sNomContrato + "/");

        string sFileHtml = Server.MapPath("Facturas/") + sNomContrato + "/" + sNumInvoce + ".html";
        File.WriteAllText(sFileHtml, sHtml.ToString(), Encoding.UTF8);

        cLogEventos oLogEventos = new cLogEventos(ref oConn);
        oLogEventos.AccionLog = "APROBAR PRE-FACTURA SHORT FALL";
        oLogEventos.CodCanal = "2";
        oLogEventos.CodFlujo = "6";
        oLogEventos.NomFlujo = "FACTURACION SHORT FALL";
        oLogEventos.NumContrato = pNumContrato;
        oLogEventos.NoContrato = sNomContrato;
        oLogEventos.PeriodoLog = sPeriodo;
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

      StringBuilder js = new StringBuilder();
      js.Append("function LgRespuesta() {");
      js.Append(" window.radalert('La factura ha sido emitida con éxito.', 330, 210); ");
      js.Append(" Sys.Application.remove_load(LgRespuesta); ");
      js.Append("};");
      js.Append("Sys.Application.add_load(LgRespuesta);");
      Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", js.ToString(), true);

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