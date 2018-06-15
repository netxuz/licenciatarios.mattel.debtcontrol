using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;
using Telerik.Web.UI;
using System.Text;
using System.IO;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class meliminafacturas : System.Web.UI.Page
  {
    Usuario oUsuario;
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      oUsuario = oWeb.GetObjAdmUsuario();
    }

    protected void rdLog_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cDeudor oDeudor = new cDeudor(ref oConn);
        if (!string.IsNullOrEmpty(txtBuscarUsuario.Text))
          oDeudor.SNombre = txtBuscarUsuario.Text;
        rdLog.DataSource = oDeudor.Get();
      }
      oConn.Close();
    }

    protected void rdLog_ItemCreated(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        LinkButton lb = (LinkButton)item["SelectRecord"].Controls[0];
        lb.Attributes.Add("onclick", "onSelectClient('" + item.ItemIndex + "');");
      }
    }

    protected void BtnBuscarLicenciatarios_Click(object sender, EventArgs e)
    {
      rdLog.Rebind();
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      obgrilla.Visible = true;
      RadGrid1.Rebind();
    }

    protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cFactura oFactura = new cFactura(ref oConn);
        oFactura.NkeyDeudor = hdd_nkey_deudor.Value;
        oFactura.CodFactura = (!string.IsNullOrEmpty(txt_factura.Text) ? txt_factura.Text.Substring(2) : string.Empty);
        DataTable dtFacura = oFactura.GetStatusInvoceFinanzas();
        RadGrid1.DataSource = dtFacura;
      }
      oConn.Close();
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;

        item["date_invoce"].Text = DateTime.Parse(row["date_invoce"].ToString()).ToString("dd-MM-yyyy");
        item["due_date"].Text = DateTime.Parse(row["due_date"].ToString()).ToString("dd-MM-yyyy");
        item["total"].Text = double.Parse(row["total"].ToString()).ToString("N0");

      }
    }

    protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
      switch (e.CommandName)
      {
        case "cmdDelete":
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            string pCodFactura = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codigo_factura"].ToString();
            string pNumContrato = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["num_contrato"].ToString();
            string pTipoFactura = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["tipo_factura"].ToString();

            if (pTipoFactura == "A")
            {
              cDetalleFacturaAdvance oDetalleFacturaAdvance = new cDetalleFacturaAdvance(ref oConn);
              oDetalleFacturaAdvance.CodigoFactura = pCodFactura;
              oDetalleFacturaAdvance.Accion = "ELIMINAR";
              oDetalleFacturaAdvance.Put();

              cContratos oContratos = new cContratos(ref oConn);
              oContratos.NumContrato = pNumContrato;
              oContratos.Accion = "ELIMINAADVANCE";
              oContratos.Put();

            }
            else if (pTipoFactura == "Q")
            {
              cDetalleFactura oDetalleFactura = new cDetalleFactura(ref oConn);
              oDetalleFactura.CodigoFactura = pCodFactura;
              DataTable dt = oDetalleFactura.Get();
              if (dt != null)
              {
                if (dt.Rows.Count > 0)
                {
                  foreach (DataRow oRow in dt.Rows)
                  {
                    if (!string.IsNullOrEmpty(oRow["saldo_advance_usd"].ToString()))
                    {
                      cAdvanceContrato oAdvanceContrato = new cAdvanceContrato(ref oConn);
                      oAdvanceContrato.NumContrato = pNumContrato;
                      oAdvanceContrato.CodMarca = oRow["cod_marca"].ToString();
                      oAdvanceContrato.CodCategoria = oRow["cod_categoria"].ToString();
                      oAdvanceContrato.CodSubCategoria = oRow["cod_subcategoria"].ToString();
                      DataTable dtAdvance = oAdvanceContrato.Get();
                      if (dtAdvance != null)
                      {
                        if (dtAdvance.Rows.Count > 0)
                        {
                          oAdvanceContrato.Saldo = (!string.IsNullOrEmpty(dtAdvance.Rows[0]["saldo"].ToString()) ? (double.Parse(dtAdvance.Rows[0]["saldo"].ToString()) + double.Parse(oRow["saldo_advance_usd"].ToString())).ToString() : oRow["saldo_advance_usd"].ToString());
                          oAdvanceContrato.Accion = "EDITAR";
                          oAdvanceContrato.Put();
                        }
                      }
                      dtAdvance = null;
                    }
                  }
                }
              }
              dt = null;
              
              oDetalleFactura.Accion = "ELIMINAR";
              oDetalleFactura.Put();



              cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
              oReporteVenta.CodigoFactura = pCodFactura;
              oReporteVenta.Accion = "ELIMINAFACTURA";
              oReporteVenta.Put();

            }
            else if (pTipoFactura == "S")
            {
              cDetFactShortfall oDetFactShortfall = new cDetFactShortfall(ref oConn);
              oDetFactShortfall.CodigoFactura = pCodFactura;
              oDetFactShortfall.Accion = "ELIMINAR";
              oDetFactShortfall.Put();

              cMinimoContrato oMinimoContrato = new cMinimoContrato(ref oConn);
              oMinimoContrato.CodFactShortFall = pCodFactura;
              oMinimoContrato.Accion = "ELIMINAFACTURA";
              oMinimoContrato.Put();

            }

            cFactura oFactura = new cFactura(ref oConn);
            oFactura.CodFactura = pCodFactura;
            oFactura.Accion = "ELIMINAR";
            oFactura.Put();

            cLogEventos oLogEventos = new cLogEventos(ref oConn);
            oLogEventos.AccionLog = "ELIMINACION DE FACTURA DE " + (pTipoFactura == "A" ? "ADVANCE" : (pTipoFactura == "Q" ? "PERIODO" : "SHORTFALL")) + " FE" + pCodFactura;
            oLogEventos.CodCanal = "2";
            oLogEventos.CodFlujo = "9";
            oLogEventos.NomFlujo = "ELIMINACION DE FACTURA";
            oLogEventos.NumContrato = pNumContrato;
            oLogEventos.CodUser = oUsuario.CodUsuario;
            oLogEventos.RutUser = oUsuario.RutUsuario;
            oLogEventos.NomUser = oUsuario.Nombres;
            oLogEventos.IpLog = oWeb.GetIpUsuario();
            oLogEventos.Accion = "CREAR";
            oLogEventos.Put();

          }
          oConn.Close();
          break;
      }

      RadGrid1.Rebind();
    }
  }
}