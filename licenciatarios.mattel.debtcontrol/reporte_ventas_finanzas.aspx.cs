using System;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;
using Telerik.Web.UI;
using ClosedXML.Excel;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class reporte_ventas_finanzas : System.Web.UI.Page
  {
    CultureInfo oCulture;
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
      if (oUsuario.CodTipoUsuario == "3")
        oCulture = new CultureInfo("en-US");
      else
        oCulture = new CultureInfo("es-CL");

      if (!IsPostBack)
      {
        loadLicenciatarios();
        int sYear = 2015;
        int aYear = DateTime.Now.Year;
        while (sYear <= aYear)
        {
          ddlist_ano_ini.Items.Add(new ListItem(sYear.ToString(), sYear.ToString()));
          ddlist_ano_fin.Items.Add(new ListItem(sYear.ToString(), sYear.ToString()));
          sYear++;
        }
        cmbox_contrato.Items.Clear();
        cmbox_contrato.Items.Add(new ListItem("<< Todos los Contratos >>", "0"));
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
            cmbox_licenciatario.Items.Add(new ListItem("<< Todos >>", "0"));
            foreach (DataRow oRow in dtDeudor.Rows)
            {
              cmbox_licenciatario.Items.Add(new ListItem(oRow["snombre"].ToString(), oRow["nKey_Deudor"].ToString()));
            }
          }
          else
          {
            cmbox_licenciatario.Items.Clear();
            cmbox_licenciatario.Items.Add(new ListItem("<< No Existen Licenciatarios >>", "NE"));
          }
        }
        dtDeudor = null;

      }
      oConn.Close();
    }

    protected void cmbox_licenciatario_SelectedIndexChanged(object sender, EventArgs e)
    {
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
              cmbox_contrato.Items.Clear();
              cmbox_contrato.Items.Add(new ListItem("<< Todos los Contratos >>", "0"));
              foreach (DataRow oRow in dtContrato.Rows)
              {
                cmbox_contrato.Items.Add(new ListItem(oRow["no_contrato"].ToString(), oRow["num_contrato"].ToString()));
              }
            }
          }
          else
          {
            cmbox_contrato.Items.Clear();
            cmbox_contrato.Items.Add(new ListItem("<< No Existen Contratos >>", "NE"));
          }
        }
        dtContrato = null;
      }
      oConn.Close();
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      idGrilla.Visible = true;
      rdGridReporteVenta.Rebind();
    }

    protected void rdGridReporteVenta_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
      if (cmbox_licenciatario.SelectedValue != "NE")
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cDetalleVenta oDetalleVenta = new cDetalleVenta(ref oConn);
          oDetalleVenta.NkeyDeudor = cmbox_licenciatario.SelectedValue;
          oDetalleVenta.NumContrato = cmbox_contrato.SelectedValue;
          if ((ddlist_ano_ini.SelectedValue != "0") && (ddlist_mes_ini.SelectedValue != "0") && (ddlist_ano_fin.SelectedValue != "0") && (ddlist_mes_fin.SelectedValue != "0"))
          {
            string sMeses = string.Empty;
            if ((int.Parse(ddlist_mes_ini.SelectedValue) < int.Parse(ddlist_mes_fin.SelectedValue)) && (int.Parse(ddlist_ano_ini.SelectedValue) == int.Parse(ddlist_ano_fin.SelectedValue)))
            {
              for (int i = int.Parse(ddlist_mes_ini.SelectedValue); i <= int.Parse(ddlist_mes_fin.SelectedValue); i++)
              {
                sMeses = (string.IsNullOrEmpty(sMeses) ? i.ToString() : sMeses + "," + i.ToString());
              }
            }
            else
            {
              if ((int.Parse(ddlist_mes_ini.SelectedValue) == int.Parse(ddlist_mes_fin.SelectedValue)) && (int.Parse(ddlist_ano_ini.SelectedValue) == int.Parse(ddlist_ano_fin.SelectedValue)))
              {
                sMeses = ddlist_mes_ini.SelectedValue;
              }
              else if (int.Parse(ddlist_mes_ini.SelectedValue) == 12)
              {
                sMeses = "12";
                for (int i = 1; i <= int.Parse(ddlist_mes_fin.SelectedValue); i++)
                {
                  sMeses = (string.IsNullOrEmpty(sMeses) ? i.ToString() : sMeses + "," + i.ToString());
                }
              }
              else
              {
                for (int i = int.Parse(ddlist_mes_ini.SelectedValue); i <= 12; i++)
                {
                  sMeses = (string.IsNullOrEmpty(sMeses) ? i.ToString() : sMeses + "," + i.ToString());
                }

                for (int i = 1; i <= int.Parse(ddlist_mes_fin.SelectedValue); i++)
                {
                  sMeses = (string.IsNullOrEmpty(sMeses) ? i.ToString() : sMeses + "," + i.ToString());
                }

              }
            }
            string sAnos = string.Empty;
            if (int.Parse(ddlist_ano_ini.SelectedValue) == int.Parse(ddlist_ano_fin.SelectedValue))
            {
              sAnos = ddlist_ano_ini.SelectedValue;
            }
            else
            {
              for (int i = int.Parse(ddlist_ano_ini.SelectedValue); i <= int.Parse(ddlist_ano_fin.SelectedValue); i++)
              {
                sAnos = (string.IsNullOrEmpty(sAnos) ? i.ToString() : sAnos + "," + i.ToString());
              }
            }


            oDetalleVenta.FechaInicio = sMeses;
            oDetalleVenta.FechaFinal = sAnos;
          }
          DataTable dtDetalleVenta = oDetalleVenta.GetByReporteVentaFinanzas();
          rdGridReporteVenta.DataSource = dtDetalleVenta;
        }
        oConn.Close();
      }
    }

    protected void rdGridReporteVenta_ItemDataBound(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;

        item["Royalty"].Text = String.Format("{0:p2}", double.Parse(row["Royalty"].ToString()));
        item["BDI"].Text = String.Format("{0:p2}", double.Parse(row["BDI"].ToString()));

        item["precio_uni_venta_bruta"].Text = double.Parse(row["precio_uni_venta_bruta"].ToString()).ToString("#,##0.00");
        //item["total_local"].Text = (double.Parse(row["precio_uni_venta_bruta"].ToString()) * double.Parse(row["cantidad_venta_bruta"].ToString())).ToString("#,##0.00");

        item["precio_uni_devolucion"].Text = double.Parse(row["precio_uni_devolucion"].ToString()).ToString("#,##0.00");
        //item["total_local_devol"].Text = (double.Parse(row["precio_uni_devolucion"].ToString()) * double.Parse(row["cantidad_q_devolucion"].ToString())).ToString("#,##0.00");

        item["total_local"].Text = double.Parse(row["total_local"].ToString()).ToString("#,##0.00");
        item["total_local_devol"].Text = double.Parse(row["total_local_devol"].ToString()).ToString("#,##0.00");

        item["venta_neta"].Text = (double.Parse(item["total_local"].Text) - double.Parse(item["total_local_devol"].Text)).ToString("#,##0.00");

        item["tipo_cambio"].Text = double.Parse(row["tipo_cambio"].ToString()).ToString("#,##0.00");

        item["venta_neta_usd"].Text = (double.Parse(item["venta_neta"].Text) / double.Parse(row["tipo_cambio"].ToString())).ToString("#,##0.00");

        item["royalty_usd"].Text = (double.Parse(item["venta_neta_usd"].Text) * double.Parse(row["Royalty"].ToString())).ToString("#,##0.00");

        item["bdi_usd"].Text = (double.Parse(item["venta_neta_usd"].Text) * double.Parse(row["BDI"].ToString())).ToString("#,##0.00");

        /*item["Monto BDI USD"].Text = double.Parse(row["Monto BDI USD"].ToString()).ToString("#,##0.00");
        item["Saldo Advance USD"].Text = double.Parse(row["Saldo Advance USD"].ToString()).ToString("#,##0.00");
        item["Factura USD"].Text = double.Parse(row["Factura USD"].ToString()).ToString("#,##0.00");*/
      }
    }

    protected void btnContacto_Click(object sender, EventArgs e)
    {
      rdGridReporteVenta.Rebind();
    }

    protected void rdGridReporteVenta_ItemCommand(object source, GridCommandEventArgs e)
    {
      if (e.CommandName == RadGrid.ExportToExcelCommandName)
      {
        //DataTable dt = new DataTable();

        //foreach (GridHeaderItem itm in rdGridReporteVenta.MasterTableView.GetItems(GridItemType.Header)) {
        //  foreach (GridColumn col in rdGridReporteVenta.MasterTableView.Columns)
        //  {
        //    dt.Columns.Add(new DataColumn(col.HeaderText, typeof(string)));
        //  }
        //}          

        //foreach (GridDataItem item in rdGridReporteVenta.MasterTableView.Items) {
        //  DataRow newRow = dt.NewRow();
        //  foreach (GridColumn col in rdGridReporteVenta.MasterTableView.RenderColumns)
        //  {
        //    if (col.ColumnType == "GridBoundColumn")
        //    {
        //      newRow[col.HeaderText] = item[col.UniqueName].Text;
        //    }
        //  }
        //  dt.Rows.Add(newRow);
        //}
        DataTable dt = dtReporteVenta();

        XLWorkbook wb = new XLWorkbook();
        wb.Worksheets.Add(dt, "ReporteVentas");

        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("content-disposition", "attachment;filename=ReporteVenta_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
        using (MemoryStream MyMemoryStream = new MemoryStream())
        {
          wb.SaveAs(MyMemoryStream);
          MyMemoryStream.WriteTo(Response.OutputStream);
          Response.Flush();
          Response.End();
        }

      }
    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {
      Response.Redirect("downloadmanual.ashx");
    }

    public DataTable dtReporteVenta()
    {
      DataTable table = new DataTable("ReporteVenta");
      table.Columns.Add(new DataColumn("Mes", typeof(string)));
      table.Columns.Add(new DataColumn("Año", typeof(string)));
      table.Columns.Add(new DataColumn("Licenciatario", typeof(string)));
      table.Columns.Add(new DataColumn("Contrato", typeof(string)));
      table.Columns.Add(new DataColumn("Marca", typeof(string)));
      table.Columns.Add(new DataColumn("Categoría", typeof(string)));
      table.Columns.Add(new DataColumn("Sub-Categoría", typeof(string)));
      table.Columns.Add(new DataColumn("Producto", typeof(string)));
      table.Columns.Add(new DataColumn("Royalty (%)", typeof(string)));
      table.Columns.Add(new DataColumn("BDI (%)", typeof(string)));
      table.Columns.Add(new DataColumn("Cliente", typeof(string)));
      table.Columns.Add(new DataColumn("SKU", typeof(string)));
      table.Columns.Add(new DataColumn("Descripcion Producto", typeof(string)));
      table.Columns.Add(new DataColumn("Precio Unit. Venta", typeof(string)));
      table.Columns.Add(new DataColumn("Unid.", typeof(string)));
      table.Columns.Add(new DataColumn("Total Local $.", typeof(string)));
      table.Columns.Add(new DataColumn("Precio Unit. Dev", typeof(string)));
      table.Columns.Add(new DataColumn("Q.", typeof(string)));
      table.Columns.Add(new DataColumn("Total Local $", typeof(string)));
      table.Columns.Add(new DataColumn("Venta Neta Local $", typeof(string)));
      table.Columns.Add(new DataColumn("Tipo Cambio", typeof(string)));
      table.Columns.Add(new DataColumn("Venta Neta USD", typeof(string)));
      table.Columns.Add(new DataColumn("Royalty USD", typeof(string)));
      table.Columns.Add(new DataColumn("BDI USD", typeof(string)));

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cDetalleVenta oDetalleVenta = new cDetalleVenta(ref oConn);
        oDetalleVenta.NkeyDeudor = cmbox_licenciatario.SelectedValue;
        oDetalleVenta.NumContrato = cmbox_contrato.SelectedValue;
        if ((ddlist_ano_ini.SelectedValue != "0") && (ddlist_mes_ini.SelectedValue != "0") && (ddlist_ano_fin.SelectedValue != "0") && (ddlist_mes_fin.SelectedValue != "0"))
        {
          string sMeses = string.Empty;
          if ((int.Parse(ddlist_mes_ini.SelectedValue) < int.Parse(ddlist_mes_fin.SelectedValue)) && (int.Parse(ddlist_ano_ini.SelectedValue) == int.Parse(ddlist_ano_fin.SelectedValue)))
          {
            for (int i = int.Parse(ddlist_mes_ini.SelectedValue); i <= int.Parse(ddlist_mes_fin.SelectedValue); i++)
            {
              sMeses = (string.IsNullOrEmpty(sMeses) ? i.ToString() : sMeses + "," + i.ToString());
            }
          }
          else
          {
            if ((int.Parse(ddlist_mes_ini.SelectedValue) == int.Parse(ddlist_mes_fin.SelectedValue)) && (int.Parse(ddlist_ano_ini.SelectedValue) == int.Parse(ddlist_ano_fin.SelectedValue)))
            {
              sMeses = ddlist_mes_ini.SelectedValue;
            }
            else if (int.Parse(ddlist_mes_ini.SelectedValue) == 12)
            {
              sMeses = "12";
              for (int i = 1; i <= int.Parse(ddlist_mes_fin.SelectedValue); i++)
              {
                sMeses = (string.IsNullOrEmpty(sMeses) ? i.ToString() : sMeses + "," + i.ToString());
              }
            }
            else
            {
              for (int i = int.Parse(ddlist_mes_ini.SelectedValue); i <= 12; i++)
              {
                sMeses = (string.IsNullOrEmpty(sMeses) ? i.ToString() : sMeses + "," + i.ToString());
              }

              for (int i = 1; i <= int.Parse(ddlist_mes_fin.SelectedValue); i++)
              {
                sMeses = (string.IsNullOrEmpty(sMeses) ? i.ToString() : sMeses + "," + i.ToString());
              }

            }
          }
          string sAnos = string.Empty;
          if (int.Parse(ddlist_ano_ini.SelectedValue) == int.Parse(ddlist_ano_fin.SelectedValue))
          {
            sAnos = ddlist_ano_ini.SelectedValue;
          }
          else
          {
            for (int i = int.Parse(ddlist_ano_ini.SelectedValue); i <= int.Parse(ddlist_ano_fin.SelectedValue); i++)
            {
              sAnos = (string.IsNullOrEmpty(sAnos) ? i.ToString() : sAnos + "," + i.ToString());
            }
          }


          oDetalleVenta.FechaInicio = sMeses;
          oDetalleVenta.FechaFinal = sAnos;
        }

        DataTable dt = oDetalleVenta.GetByReporteVentaFinanzas();
        foreach (DataRow oRow in dt.Rows)
        {
          DataRow newRow = table.NewRow();
          newRow["Mes"] = oRow["mes"].ToString();
          newRow["Año"] = oRow["ano"].ToString();
          newRow["Licenciatario"] = oRow["licenciatario"].ToString();
          newRow["Contrato"] = oRow["no_contrato"].ToString();
          newRow["Marca"] = oRow["nom_marca"].ToString();
          newRow["Categoría"] = oRow["nom_categoria"].ToString();
          newRow["Sub-Categoría"] = oRow["nom_subcategoria"].ToString();
          newRow["Producto"] = oRow["producto"].ToString();
          newRow["Royalty (%)"] = String.Format("{0:p2}", double.Parse(oRow["royalty"].ToString())); //oRow["royalty"].ToString();
          newRow["BDI (%)"] = String.Format("{0:p2}", double.Parse(oRow["bdi"].ToString()));  //oRow["bdi"].ToString();
          newRow["Cliente"] = oRow["cliente"].ToString();
          newRow["SKU"] = oRow["sku"].ToString();
          newRow["Descripcion Producto"] = oRow["descripcion_producto"].ToString();
          newRow["Precio Unit. Venta"] = double.Parse(oRow["precio_uni_venta_bruta"].ToString()).ToString("#,0.00", oCulture); //oRow["precio_uni_venta_bruta"].ToString();
          newRow["Unid."] = oRow["cantidad_venta_bruta"].ToString();
          newRow["Total Local $."] = double.Parse(oRow["total_local"].ToString()).ToString("#,0.00", oCulture);  //oRow["total_local"].ToString();
          newRow["Precio Unit. Dev"] = double.Parse(oRow["precio_uni_devolucion"].ToString()).ToString("#,0.00", oCulture);  //oRow["precio_uni_devolucion"].ToString();
          newRow["Q."] = oRow["cantidad_q_devolucion"].ToString();
          newRow["Total Local $"] = double.Parse(oRow["total_local_devol"].ToString()).ToString("#,0.00", oCulture);  //oRow["total_local_devol"].ToString();
          newRow["Venta Neta Local $"] = (double.Parse(oRow["total_local"].ToString()) - double.Parse(oRow["total_local_devol"].ToString())).ToString("#,0.00", oCulture);  //oRow["venta_neta"].ToString();
          newRow["Tipo Cambio"] = double.Parse(oRow["tipo_cambio"].ToString()).ToString("#,0.00", oCulture);  //oRow["tipo_cambio"].ToString();
          newRow["Venta Neta USD"] = ((double.Parse(oRow["total_local"].ToString()) - double.Parse(oRow["total_local_devol"].ToString())) / double.Parse(oRow["tipo_cambio"].ToString())).ToString("#,0.00", oCulture);  //oRow["venta_neta_usd"].ToString();
          newRow["Royalty USD"] = (((double.Parse(oRow["total_local"].ToString()) - double.Parse(oRow["total_local_devol"].ToString())) / double.Parse(oRow["tipo_cambio"].ToString())) * double.Parse(oRow["royalty"].ToString())).ToString("#,0.00", oCulture);
          newRow["BDI USD"] = (((double.Parse(oRow["total_local"].ToString()) - double.Parse(oRow["total_local_devol"].ToString())) / double.Parse(oRow["tipo_cambio"].ToString())) * double.Parse(oRow["bdi"].ToString())).ToString("#,0.00", oCulture);
          table.Rows.Add(newRow);
        }
      }
      oConn.Close();

      return table;

    }

  }
}