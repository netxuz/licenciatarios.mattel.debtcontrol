using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using DebtControl.Method;
using DebtControl.Model;
using DebtControl.Conn;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class rechazo_factura_q : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        numcontrato.Value = oWeb.GetData("numcontrato");
        periodo.Value = oWeb.GetData("periodo_q");
        ano_reporte.Value = oWeb.GetData("ano_reporte");
      }
    }

    protected void BtngEnviar_Click(object sender, EventArgs e)
    {
      string sLicenciatario = string.Empty;
      string sNoContrato = string.Empty;
      string sTxtComentarios = txtcomentarios.Text;

      if (!string.IsNullOrEmpty(numcontrato.Value))
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
          oReporteVenta.NumContrato = numcontrato.Value;
          oReporteVenta.Periodo = periodo.Value;
          oReporteVenta.AnoReporte = ano_reporte.Value;
          oReporteVenta.EstReporte = "C";
          DataTable dtReporteVenta = oReporteVenta.Get();
          if (dtReporteVenta != null)
          {
            if (dtReporteVenta.Rows.Count > 0)
            {
              foreach (DataRow oRow in dtReporteVenta.Rows)
              {
                oReporteVenta.CodigoReporteVenta = oRow["cod_reporte_venta"].ToString();
                oReporteVenta.EstReporte = "P";
                oReporteVenta.Accion = "EDITAR";
                oReporteVenta.Put();
              }
            }
          }
          dtReporteVenta = null;

          cContratos oContratos = new cContratos(ref oConn);
          oContratos.NumContrato = numcontrato.Value;
          DataTable dtContrato = oContratos.Get();
          if (dtContrato != null)
          {
            if (dtContrato.Rows.Count > 0)
            {
              sNoContrato = dtContrato.Rows[0]["no_contrato"].ToString();

              cDeudor oDeudor = new cDeudor(ref oConn);
              oDeudor.NKeyDeudor = dtContrato.Rows[0]["nkey_deudor"].ToString();
              DataTable dtDeudor = oDeudor.Get();
              if (dtDeudor != null)
              {
                if (dtDeudor.Rows.Count > 0)
                {
                  sLicenciatario = dtDeudor.Rows[0]["snombre"].ToString();
                }
              }
              dtDeudor = null;
            }
          }
          dtContrato = null;
        }
        oConn.Close();
      }
      else {
        DBConn oConn = new DBConn();
        if (oConn.Open()) {
          cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
          oReporteVenta.EstReporte = "C";
          DataTable dtReporteVenta = oReporteVenta.GettingForInvoice();
          if (dtReporteVenta != null)
          {
            if (dtReporteVenta.Rows.Count > 0)
            {
              foreach (DataRow oRow in dtReporteVenta.Rows)
              {
                oReporteVenta.NumContrato = oRow["num_contrato"].ToString();
                oReporteVenta.Periodo = oRow["periodo_q"].ToString();
                oReporteVenta.AnoReporte = oRow["ano_reporte"].ToString();
                oReporteVenta.EstReporte = "C";
                DataTable dtVenta = oReporteVenta.Get();
                if (dtVenta != null) {
                  if (dtVenta.Rows.Count > 0) {
                    foreach (DataRow oRowVenta in dtVenta.Rows)
                    {
                      oReporteVenta.CodigoReporteVenta = oRowVenta["cod_reporte_venta"].ToString();
                      oReporteVenta.EstReporte = "P";
                      oReporteVenta.Accion = "EDITAR";
                      oReporteVenta.Put();
                    }
                  }
                }
                dtVenta = null; 
              }
            }
          }
          dtReporteVenta = null;
        }
        oConn.Close();
      }

      StringBuilder sMensaje = new StringBuilder();
      sMensaje.Append("<html>");
      sMensaje.Append("<body>");
      if (!string.IsNullOrEmpty(numcontrato.Value))
      {
        sMensaje.Append("Licenciatario : ").Append(sLicenciatario).Append("<br>");
        sMensaje.Append("Contrato : ").Append(sNoContrato).Append("<br>");
        sMensaje.Append("Periodo : ").Append(periodo.Value).Append("<br>");
      }
      sMensaje.Append("Motivo Rechazo : ").Append(sTxtComentarios).Append("<br>");
      sMensaje.Append("</body>");
      sMensaje.Append("</html>");

      Emailing oEmailing = new Emailing();
      oEmailing.FromName = Application["NameSender"].ToString();
      oEmailing.From = Application["EmailSender"].ToString();
      oEmailing.Address = Application["EmailSender"].ToString();
      if (!string.IsNullOrEmpty(numcontrato.Value))
        oEmailing.Subject = "Rechazo de factura periodo " + periodo.Value + ", contrato " + sNoContrato + " de Licenciatario " + sLicenciatario;
      else
        oEmailing.Subject = "Rechazo de todas las facturas de periodos del sistema Licenciatario ";
      oEmailing.Body = sMensaje;

      StringBuilder js = new StringBuilder();
      js.Append("function LgRespuesta() {");
      if (oEmailing.EmailSend())
      {
        js.Append(" window.radalert('El rechazo fue enviado exitosamente.', 400, 100,''); ");
      }
      else
      {
        js.Append(" window.radalert('El rechazo no pudo ser enviado, intente más tarde.', 400, 100,''); ");
      }
      js.Append(" Sys.Application.remove_load(LgRespuesta); ");
      js.Append("};");
      js.Append("Sys.Application.add_load(LgRespuesta);");
      Page.ClientScript.RegisterStartupScript(Page.GetType(), "LgRespuesta", js.ToString(), true);

      txtcomentarios.Text = string.Empty;
      bx_msRechazo.Visible = false;
      bx_msRealizado.Visible = true;
    }
  }
}