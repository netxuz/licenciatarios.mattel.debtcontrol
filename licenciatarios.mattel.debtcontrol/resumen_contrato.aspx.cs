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
  public partial class resumen_contrato : System.Web.UI.Page
  {
    string[] sPeriodo;
    string sMeses = string.Empty;
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
      if (!IsPostBack)
      {
        int sYear = 2015;
        int aYear = DateTime.Now.Year;
        while (sYear <= aYear)
        {
          cmbox_ano.Items.Add(new ListItem(sYear.ToString(), sYear.ToString()));
          sYear++;
        }
      }
    }

    protected void btnContacto_Click(object sender, EventArgs e)
    {
      Response.Redirect("contacto.aspx");
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      idGrilla.Visible = true;
      rdGridReporteVenta.Rebind();
    }

    protected DataTable getResumenContratos()
    {
      ResumenContrato oResumenContrato = new ResumenContrato();
      oResumenContrato.getMakeTable();

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cContratos oContratos = new cContratos(ref oConn);
        oContratos.Aprobado = true;
        //oContratos.AnoTermino = cmbox_ano.SelectedValue;
        DataTable dtContrato = oContratos.GetForResumen();
        if (dtContrato != null)
        {
          foreach (DataRow oRow in dtContrato.Rows)
          {
            oResumenContrato.Licenciatario = oRow["licenciatario"].ToString();
            oResumenContrato.NoContrato = oRow["no_contrato"].ToString();
            oResumenContrato.Inicio = DateTime.Parse(oRow["fech_inicio"].ToString()).ToString("dd/MM/yyyy");
            oResumenContrato.Final = DateTime.Parse(oRow["fech_termino"].ToString()).ToString("dd/MM/yyyy");
            oResumenContrato.PorVencer = string.Empty;

            string sMesPeriodo = string.Empty;
            string sFechaPeriodo = string.Empty;

            string sNumFactura = string.Empty;
            string sFechFactura = string.Empty;
            string sFechComprobante = string.Empty;

            cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
            oReporteVenta.InMesReporte = sMeses;
            oReporteVenta.AnoReporte = cmbox_ano.SelectedValue;
            oReporteVenta.NumContrato = oRow["num_contrato"].ToString();
            oReporteVenta.OrderMes = true;
            DataTable dtReporteVenta = oReporteVenta.GetResumenContrato();
            if (dtReporteVenta != null)
            {
              sNumFactura = string.Empty;
              sFechFactura = string.Empty;
              sFechComprobante = string.Empty;

              oResumenContrato.MesFechaUno = null;
              oResumenContrato.MesFechaDos = null;
              oResumenContrato.MesFechaTres = null;
              if (dtReporteVenta.Rows.Count > 0)
              {
                foreach (DataRow oRowVenta in dtReporteVenta.Rows)
                {
                  sMesPeriodo = (string.IsNullOrEmpty(sMesPeriodo) ? oRowVenta["mes_reporte"].ToString() : sMesPeriodo + ',' + oRowVenta["mes_reporte"].ToString());
                  sFechaPeriodo = (string.IsNullOrEmpty(sFechaPeriodo) ? oRowVenta["fecha_reporte"].ToString() : sFechaPeriodo + ',' + oRowVenta["fecha_reporte"].ToString());
                }

                string[] sMes = sMeses.Split(',');
                string[] sMesArrPeriodo = sMesPeriodo.Split(',');
                string[] sFechPeriodo = sFechaPeriodo.Split(',');

                oResumenContrato.MesFechaUno = string.Empty;
                oResumenContrato.MesFechaDos = string.Empty;
                oResumenContrato.MesFechaTres = string.Empty;

                for (int i = 0; i < sMesArrPeriodo.Length; i++)
                {
                  if (sMes[0].ToString() == sMesArrPeriodo[i].ToString())
                    oResumenContrato.MesFechaUno = DateTime.Parse(sFechPeriodo[i]).ToString("dd/MM/yyyy");
                  if (sMes[1].ToString() == sMesArrPeriodo[i].ToString())
                    oResumenContrato.MesFechaDos = DateTime.Parse(sFechPeriodo[i]).ToString("dd/MM/yyyy");
                  if (sMes[2].ToString() == sMesArrPeriodo[i].ToString())
                    oResumenContrato.MesFechaTres = DateTime.Parse(sFechPeriodo[i]).ToString("dd/MM/yyyy");
                }

                cFactura oFactura = new cFactura(ref oConn);
                oFactura.NumContrato = oRow["num_contrato"].ToString();
                oFactura.Periodo = cmbox_periodo.SelectedValue + "/" + cmbox_ano.SelectedValue;
                DataTable dtFactura = oFactura.Get();
                if (dtFactura != null)
                {
                  if (dtFactura.Rows.Count > 0)
                  {
                    sNumFactura = dtFactura.Rows[0]["num_invoice"].ToString();
                    sFechFactura = DateTime.Parse(dtFactura.Rows[0]["date_invoce"].ToString()).ToString("dd/MM/yyyy");

                    if (!string.IsNullOrEmpty(dtFactura.Rows[0]["cod_comprobante"].ToString()))
                    {
                      cComprobanteImpuesto oComprobanteImpuesto = new cComprobanteImpuesto(ref oConn);
                      oComprobanteImpuesto.CodComprobante = dtFactura.Rows[0]["cod_comprobante"].ToString();
                      DataTable dtComprobante = oComprobanteImpuesto.Get();
                      if (dtComprobante != null)
                      {
                        if (dtComprobante.Rows.Count > 0)
                        {
                          sFechComprobante = DateTime.Parse(dtComprobante.Rows[0]["fecha_declaracion"].ToString()).ToString("dd/MM/yyyy");
                        }
                      }
                      dtComprobante = null;
                    }
                  }
                }
                dtFactura = null;
              }
              dtReporteVenta = null;
            }

            oResumenContrato.NumInvoce = sNumFactura;
            oResumenContrato.FechFactura = sFechFactura;
            oResumenContrato.FechComprobante = sFechComprobante;

            oResumenContrato.AddRow();
          }

        }
        dtContrato = null;
      }
      oConn.Close();
      DataTable dtResumenContrato = oResumenContrato.Get();
      return dtResumenContrato;
    }

    protected void rdGridReporteVenta_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
      sMeses = oWeb.getPeriodoMesesQ(cmbox_periodo.SelectedValue);
      sPeriodo = sMeses.Split(',');
      rdGridReporteVenta.DataSource = getResumenContratos();
    }

    protected void rdGridReporteVenta_ItemCommand(object source, GridCommandEventArgs e)
    {
      if (e.CommandName == RadGrid.ExportToExcelCommandName)
      {
        rdGridReporteVenta.ExportSettings.ExportOnlyData = true;
        rdGridReporteVenta.ExportSettings.IgnorePaging = true;
        rdGridReporteVenta.ExportSettings.OpenInNewWindow = true;
        rdGridReporteVenta.ExportSettings.FileName = "resumen_contrato_" + DateTime.Now.ToString("yyyyMMdd");
        rdGridReporteVenta.MasterTableView.ExportToExcel();
        //rdGridReporteVenta.AllowPaging = true;
        //sMeses = oWeb.getPeriodoMesesQ(cmbox_periodo.SelectedValue);
        //sPeriodo = sMeses.Split(',');
        //rdGridReporteVenta.Rebind();
      }

      if (e.CommandName == "BajarComprobante")
      {
        GridDataItem item = (GridDataItem)e.Item;
        if ((!string.IsNullOrEmpty(item["FechaComprobante"].Text)) && (item["FechaComprobante"].Text != "&nbsp;"))
        {
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            cFactura oFactura = new cFactura(ref oConn);
            oFactura.NumInvoice = item["num_factura"].Text;
            DataTable dtFactura = oFactura.Get();
            if (dtFactura != null)
            {
              if (dtFactura.Rows.Count > 0)
              {
                Response.Redirect("downloadcomprobantesii.ashx?pCodComprobante=" + dtFactura.Rows[0]["cod_comprobante"].ToString() + "&NumContrato=" + item["Contrato"].Text);
              }
            }
            dtFactura = null;
          }
          oConn.Close();

        }
        else
        {
          StringBuilder scriptstring = new StringBuilder();
          scriptstring.Append(" window.radalert('No existe comprobante para este Q.', 330, 210); ");
          ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring.ToString(), true);
        }
      }
    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {
      Response.Redirect("downloadmanual.ashx");
    }

    protected void rdGridReporteVenta_ItemDataBound(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;

        DateTime dTermino = DateTime.Parse(row["Termino"].ToString());
        TimeSpan ts = dTermino - DateTime.Now;
        int diffDays = ts.Days;

        item["porvencer"].Text = diffDays.ToString() + " días";
      }
      foreach (GridColumn col in rdGridReporteVenta.MasterTableView.Columns)
      {
        if (col.UniqueName == "mesuno")
        {
          col.HeaderText = oWeb.getMes(int.Parse(sPeriodo[0].ToString()));
        }
        else if (col.UniqueName == "mesdos")
        {
          col.HeaderText = oWeb.getMes(int.Parse(sPeriodo[1].ToString()));
        }
        else if (col.UniqueName == "mestres")
        {
          col.HeaderText = oWeb.getMes(int.Parse(sPeriodo[2].ToString()));
        }
      }
    }
  }
}