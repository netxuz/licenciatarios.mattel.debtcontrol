using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;
using Telerik.Web.UI;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class reporte_ventas : System.Web.UI.Page
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
        int sYear = 2015;
        int aYear = DateTime.Now.Year;
        while (sYear <= aYear)
        {
          ddlist_ano_ini.Items.Add(new ListItem(sYear.ToString(), sYear.ToString()));
          ddlist_ano_fin.Items.Add(new ListItem(sYear.ToString(), sYear.ToString()));
          sYear++;
        }

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

    protected void btnContacto_Click(object sender, EventArgs e)
    {
      Response.Redirect("contacto.aspx");
    }

    protected void rdGridReporteVenta_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
      if ((cmbox_contrato.SelectedValue != "0") && (!string.IsNullOrEmpty(cmbox_contrato.SelectedValue)))
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cDetalleVenta oDetalleVenta = new cDetalleVenta(ref oConn);
          oDetalleVenta.NumContrato = cmbox_contrato.SelectedValue;
          if ((ddlist_ano_ini.SelectedValue != "0") && (ddlist_mes_ini.SelectedValue != "0") && (ddlist_ano_fin.SelectedValue != "0") && (ddlist_mes_fin.SelectedValue != "0"))
          {
            string sMeses = string.Empty;
            if (int.Parse(ddlist_mes_ini.SelectedValue) < int.Parse(ddlist_mes_fin.SelectedValue))
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
          DataTable dtDetalleVenta = oDetalleVenta.GetByReporteVenta();
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

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      rdGridReporteVenta.Rebind();
    }

    protected void rdGridReporteVenta_ItemCommand(object source, GridCommandEventArgs e)
    {
      if (e.CommandName == RadGrid.ExportToExcelCommandName)
      {
        rdGridReporteVenta.ExportSettings.ExportOnlyData = true;
        rdGridReporteVenta.ExportSettings.IgnorePaging = true;
        rdGridReporteVenta.ExportSettings.OpenInNewWindow = true;
        rdGridReporteVenta.ExportSettings.FileName = "reporte_venta_" + DateTime.Now.ToString("yyyyMMdd");
        rdGridReporteVenta.MasterTableView.ExportToExcel();
      }
    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {
      Response.Redirect("downloadmanual.ashx");
    }
  }
}