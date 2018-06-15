using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;
using Telerik.Web.UI;
using SelectPdf;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class status_facturas : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
      if (!IsPostBack)
      {
        loadContrato();
      }
    }

    protected void loadContrato()
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cContratos oContratos = new cContratos(ref oConn);
        oContratos.NkeyDeudor = oUsuario.NKeyUsuario;
        oContratos.NoAprobado = true;
        DataTable dtContrato = oContratos.Get();
        if (dtContrato != null)
        {
          if (dtContrato.Rows.Count > 0)
          {
            cmbox_contrato.Items.Clear();
            cmbox_contrato.Items.Add(new ListItem("<< Selecciona Contrato >>", "0"));
            foreach (DataRow oRow in dtContrato.Rows)
            {
              cmbox_contrato.Items.Add(new ListItem(oRow["no_contrato"].ToString(), oRow["num_contrato"].ToString()));
            }
          }
        }
        dtContrato = null;
      }
      oConn.Close();
    }

    protected void rdGridFactura_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      if ((cmbox_contrato.SelectedValue != "0") && (!string.IsNullOrEmpty(cmbox_contrato.SelectedValue)))
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cFactura oFactura = new cFactura(ref oConn);
          oFactura.NumContrato = cmbox_contrato.SelectedValue;
          DataTable dtFacura = oFactura.GetStatusInvoce();
          rdGridFactura.DataSource = dtFacura;
        }
        oConn.Close();
      }
    }

    protected void rdGridFactura_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;

        string sSaldo = string.Empty;
        string sFechaPago = string.Empty;

        DBConn oConn = new DBConn();
        if (oConn.Open()) {
          cFactura oFactura = new cFactura(ref oConn);
          oFactura.NumContrato = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["num_contrato"].ToString();
          oFactura.CodFactura = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codigo_factura"].ToString();
          DataTable dtFactura = oFactura.GetSaldoFechaPago();
          if (dtFactura != null) {
            if (dtFactura.Rows.Count > 0) {
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
        item["saldo"].Text = (!string.IsNullOrEmpty(sSaldo) ? double.Parse(sSaldo).ToString("N0") : string.Empty);
        item["fecha_pago"].Text = (!string.IsNullOrEmpty(sFechaPago) ? DateTime.Parse(sFechaPago).ToString("dd-MM-yyyy") : string.Empty);
      }
    }

    protected void cmbox_contrato_SelectedIndexChanged(object sender, EventArgs e)
    {
      rdGridFactura.Rebind();
    }

    protected void btnContacto_Click(object sender, EventArgs e)
    {
      Response.Redirect("contacto.aspx");
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
        rdGridFactura.ExportSettings.ExportOnlyData = true;
        rdGridFactura.ExportSettings.IgnorePaging = true;
        rdGridFactura.ExportSettings.OpenInNewWindow = true;
        rdGridFactura.ExportSettings.FileName = "status_factura_" + DateTime.Now.ToString("yyyyMMdd");
        rdGridFactura.MasterTableView.ExportToExcel();
      }
    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {
      Response.Redirect("downloadmanual.ashx");
    }
  }
}