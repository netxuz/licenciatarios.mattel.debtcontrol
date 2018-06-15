using System;
using System.Web.UI;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;
using Telerik.Web.UI;
using ClosedXML.Excel;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class status_facturas_finanzas : System.Web.UI.Page
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
          sYear++;
        }
        cmbox_contrato.Items.Clear();
        cmbox_contrato.Items.Add(new ListItem("<< Todos los Contratos >>", ""));
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
              cmbox_contrato.Items.Add(new ListItem("<< Todos los Contratos >>", ""));
              foreach (DataRow oRow in dtContrato.Rows)
              {
                cmbox_contrato.Items.Add(new ListItem(oRow["no_contrato"].ToString(), oRow["num_contrato"].ToString()));
              }
            }
          }
          else
          {
            cmbox_contrato.Items.Clear();
            cmbox_contrato.Items.Add(new ListItem("<< No Existen Contratos >>", "0"));
          }
        }
        dtContrato = null;
      }
      oConn.Close();
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      idGrilla.Visible = true;
      rdGridFactura.Rebind();
    }

    protected void rdGridFactura_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cFactura oFactura = new cFactura(ref oConn);
        oFactura.NkeyDeudor = cmbox_licenciatario.SelectedValue;
        oFactura.NumContrato = cmbox_contrato.SelectedValue;
        if ((ddlist_ano_ini.SelectedValue != "") && (ddlist_mes_ini.SelectedValue != ""))
        {
          if (ddlist_mes_ini.SelectedValue != "ShortFall")
            oFactura.Periodo = ddlist_mes_ini.SelectedValue + (ddlist_mes_ini.SelectedValue == "Advance" ? " " : "/") + ddlist_ano_ini.SelectedValue;
          else
          {
            oFactura.Periodo = ddlist_ano_ini.SelectedValue;
            oFactura.TipoFactura = "S";
          }
        }
        DataTable dtFacura = oFactura.GetStatusInvoceFinanzas();
        rdGridFactura.DataSource = dtFacura;
      }
      oConn.Close();
    }

    protected void rdGridFactura_ItemDataBound(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;

        string sSaldo = string.Empty;
        string sFechaPago = string.Empty;

        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cFactura oFactura = new cFactura(ref oConn);
          oFactura.NumContrato = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["num_contrato"].ToString();
          oFactura.CodFactura = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codigo_factura"].ToString();
          DataTable dtFactura = oFactura.GetSaldoFechaPago();
          if (dtFactura != null)
          {
            if (dtFactura.Rows.Count > 0)
            {
              sSaldo = (!string.IsNullOrEmpty(dtFactura.Rows[0]["saldo"].ToString()) ? dtFactura.Rows[0]["saldo"].ToString() : string.Empty);
              sFechaPago = (!string.IsNullOrEmpty(dtFactura.Rows[0]["fecha_pago"].ToString()) ? dtFactura.Rows[0]["fecha_pago"].ToString() : string.Empty);
            }
          }
          dtFactura = null;
        }
        oConn.Close();

        item["date_invoce"].Text = DateTime.Parse(row["date_invoce"].ToString()).ToString("dd-MM-yyyy");
        item["due_date"].Text = DateTime.Parse(row["due_date"].ToString()).ToString("dd-MM-yyyy");
        item["total"].Text = double.Parse(row["total"].ToString()).ToString("N0");
        item["saldo"].Text = string.Empty;
        item["fecha_pago"].Text = string.Empty;

        item["saldo"].Text = (!string.IsNullOrEmpty(sSaldo) ? double.Parse(sSaldo).ToString("N0") : string.Empty);
        item["fecha_pago"].Text = (!string.IsNullOrEmpty(sFechaPago) ? DateTime.Parse(sFechaPago).ToString("dd-MM-yyyy") : string.Empty);
      }
    }

    protected void rdGridFactura_ItemCommand(object source, GridCommandEventArgs e)
    {
      if (e.CommandName == "VerFactura")
      {
        string sPdfGenerado = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pdf_generado"].ToString();
        if (!string.IsNullOrEmpty(sPdfGenerado))
        {
          GridDataItem item = (GridDataItem)e.Item;
          string sNomFactura = item["num_invoice"].Text;
          string sNumContrato = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["num_contrato"].ToString();

          Response.Redirect("downloadfactura.ashx?sNomFactura=" + sNomFactura + "&sNumContrato=" + sNumContrato);
        }
        else
        {
          StringBuilder scriptstring = new StringBuilder();
          scriptstring.Append(" window.radalert('La factura aún no esta disponible.', 330, 210); ");
          ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring.ToString(), true);
        }
      }
      else if (e.CommandName == RadGrid.ExportToExcelCommandName)
      {
        //rdGridFactura.ExportSettings.ExportOnlyData = true;
        //rdGridFactura.ExportSettings.IgnorePaging = true;
        //rdGridFactura.ExportSettings.OpenInNewWindow = true;
        //rdGridFactura.ExportSettings.FileName = "status_factura_" + DateTime.Now.ToString("yyyyMMdd");
        //rdGridFactura.MasterTableView.ExportToExcel();

        //DataTable dt = new DataTable();

        //foreach (GridHeaderItem itm in rdGridFactura.MasterTableView.GetItems(GridItemType.Header))
        //{
        //  foreach (GridColumn col in rdGridFactura.MasterTableView.Columns)
        //  {
        //    dt.Columns.Add(new DataColumn(col.HeaderText, typeof(string)));
        //  }
        //}

        //foreach (GridDataItem item in rdGridFactura.MasterTableView.Items)
        //{
        //  DataRow newRow = dt.NewRow();
        //  foreach (GridColumn col in rdGridFactura.MasterTableView.RenderColumns)
        //  {
        //    if (col.ColumnType == "GridBoundColumn")
        //    {
        //      newRow[col.HeaderText] = item[col.UniqueName].Text;
        //    }
        //  }
        //  dt.Rows.Add(newRow);
        //}

        DataTable dt = dtStatusdeFactura();

        XLWorkbook wb = new XLWorkbook();
        wb.Worksheets.Add(dt, "StatusdeFactura");

        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("content-disposition", "attachment;filename=StatusdeFactura_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
        using (MemoryStream MyMemoryStream = new MemoryStream())
        {
          wb.SaveAs(MyMemoryStream);
          MyMemoryStream.WriteTo(Response.OutputStream);
          Response.Flush();
          Response.End();
        }


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

    public DataTable dtStatusdeFactura()
    {
      DataTable table = new DataTable("StatusdeFactura");
      table.Columns.Add(new DataColumn("Licenciatario", typeof(string)));
      table.Columns.Add(new DataColumn("Contrato", typeof(string)));
      table.Columns.Add(new DataColumn("Número Invoice", typeof(string)));
      table.Columns.Add(new DataColumn("Periodo", typeof(string)));
      table.Columns.Add(new DataColumn("Emisión", typeof(string)));
      table.Columns.Add(new DataColumn("Vencimiento", typeof(string)));
      table.Columns.Add(new DataColumn("Monto", typeof(string)));
      table.Columns.Add(new DataColumn("Saldo", typeof(string)));
      table.Columns.Add(new DataColumn("Fecha de Pago", typeof(string)));

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cFactura oFactura = new cFactura(ref oConn);
        oFactura.NkeyDeudor = cmbox_licenciatario.SelectedValue;
        oFactura.NumContrato = cmbox_contrato.SelectedValue;
        if ((ddlist_ano_ini.SelectedValue != "") && (ddlist_mes_ini.SelectedValue != ""))
        {
          if (ddlist_mes_ini.SelectedValue != "ShortFall")
            oFactura.Periodo = ddlist_mes_ini.SelectedValue + (ddlist_mes_ini.SelectedValue == "Advance" ? " " : "/") + ddlist_ano_ini.SelectedValue;
          else
          {
            oFactura.Periodo = ddlist_ano_ini.SelectedValue;
            oFactura.TipoFactura = "S";
          }
        }
        DataTable dt = oFactura.GetStatusInvoceFinanzas();
        foreach (DataRow oRow in dt.Rows)
        {
          string sSaldo = string.Empty;
          string sFechaPago = string.Empty;
          cFactura GetSaldoFechaPago = new cFactura(ref oConn);
          GetSaldoFechaPago.NumContrato = oRow["num_contrato"].ToString();
          GetSaldoFechaPago.CodFactura = oRow["codigo_factura"].ToString();
          DataTable dtFactura = GetSaldoFechaPago.GetSaldoFechaPago();
          if (dtFactura != null)
          {
            if (dtFactura.Rows.Count > 0)
            {
              sSaldo = (!string.IsNullOrEmpty(dtFactura.Rows[0]["saldo"].ToString()) ? dtFactura.Rows[0]["saldo"].ToString() : string.Empty);
              sFechaPago = (!string.IsNullOrEmpty(dtFactura.Rows[0]["fecha_pago"].ToString()) ? dtFactura.Rows[0]["fecha_pago"].ToString() : string.Empty);
            }
          }
          GetSaldoFechaPago = null;

          DataRow newRow = table.NewRow();
          newRow["Licenciatario"] = oRow["licenciatario"].ToString();
          newRow["Contrato"] = oRow["no_contrato"].ToString();
          newRow["Número Invoice"] = oRow["num_invoice"].ToString();
          newRow["Periodo"] = oRow["periodo"].ToString();
          newRow["Emisión"] = DateTime.Parse(oRow["date_invoce"].ToString()).ToString("dd-MM-yyyy"); // oRow["date_invoce"].ToString();
          newRow["Vencimiento"] = DateTime.Parse(oRow["due_date"].ToString()).ToString("dd-MM-yyyy"); //oRow["due_date"].ToString();
          newRow["Monto"] = double.Parse(oRow["total"].ToString()).ToString("N0", oCulture); //oRow["total"].ToString();
          newRow["Saldo"] = (!string.IsNullOrEmpty(sSaldo) ? double.Parse(sSaldo).ToString("N0", oCulture) : string.Empty); //oRow["saldo"].ToString();
          newRow["Fecha de Pago"] = (!string.IsNullOrEmpty(sFechaPago) ? DateTime.Parse(sFechaPago).ToString("dd-MM-yyyy") : string.Empty); //oRow["fecha_pago"].ToString();
          table.Rows.Add(newRow);
        }
      }
      oConn.Close();

      return table;
    }
  }
}