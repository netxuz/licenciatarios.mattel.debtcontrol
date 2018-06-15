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
  public partial class contratos_afacturar_advance : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
    }

    protected void btnContacto_Click(object sender, EventArgs e)
    {
      Response.Redirect("contacto.aspx");
    }

    protected void rdGridLicen_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cContratos oContratos = new cContratos(ref oConn);
        oContratos.IsNullFacturadoAdvance = true;
        oContratos.Aprobado = true;
        DataTable dtContratos = oContratos.GetForInvoceAdvance();
        rdGridLicen.DataSource = dtContratos;
      }
      oConn.Close();
    }

    protected void rdGridLicen_ItemCommand(object source, GridCommandEventArgs e)
    {
      if (e.CommandName == "AprobarFactura")
      {
        GridDataItem item = (GridDataItem)e.Item;
        string pNumContrato = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["num_contrato"].ToString();
        string pNoContrato = item["no_contrato"].Text;

        StringBuilder sUrl = new StringBuilder();
        sUrl.Append("aprobacion_facturas_advance.aspx?pNumContrato=").Append(pNumContrato);
        sUrl.Append("&pNoContrato=").Append(pNoContrato);
        Response.Redirect(sUrl.ToString());
      }
      else if (e.CommandName == "Aprobar")
      {
        GridDataItem item = (GridDataItem)e.Item;
        string pNumContrato = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["num_contrato"].ToString();
        string pNoContrato = item["no_contrato"].Text;
        putAprobarAdvance(pNumContrato, pNoContrato);
      }
    }

    protected void rdGridLicen_ItemDataBound(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;

        item["periodo"].Text = "Advance " + row["periodo"].ToString();

        string pNumContrato = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["num_contrato"].ToString();

        getDataInvoceAdvance(pNumContrato);

        item["monto"].Text = int.Parse(row["monto"].ToString()).ToString("N0");
        if (!string.IsNullOrEmpty(row["fecha_full"].ToString()))
          item["fecha_full"].Text = DateTime.Parse(row["fecha_full"].ToString()).ToString("dd-MM-yyyy");
        else
          item["fecha_full"].Text = string.Empty;
      }
    }

    protected void getDataInvoceAdvance(string pNumContrato)
    {
      FacturaAdvance oFactura = new FacturaAdvance();
      oFactura.getMakeTable();

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAdvanceContrato oAdvanceContrato = new cAdvanceContrato(ref oConn);
        oAdvanceContrato.NumContrato = pNumContrato;
        DataTable dt = oAdvanceContrato.GetForInvoce();
        if (dt != null)
        {
          foreach (DataRow oRow in dt.Rows)
          {
            oFactura.CodMarca = oRow["cod_marca"].ToString();
            oFactura.Marca = oRow["marca"].ToString();
            oFactura.CodCategoria = oRow["cod_categoria"].ToString();
            oFactura.Categoria = oRow["categoria"].ToString();
            oFactura.CodSubCategoria = oRow["cod_subcategoria"].ToString();
            oFactura.SubCategoria = oRow["subcategoria"].ToString();
            oFactura.AdvanceUsd = oRow["valor_original"].ToString();
            oFactura.AddRow();
          }

          DataTable dtFactura = oFactura.Get();

          ViewState["FacturaAdvance_" + pNumContrato] = dtFactura;
        }
      }
      oConn.Close();

    }

    protected void putAprobarAdvance(string pNumContrato, string pNoContrato)
    {
      string sNumInvoce = string.Empty;
      StringBuilder sHtml = new StringBuilder();
      sHtml.Append(File.ReadAllText(Server.MapPath("invoice.html")));
      StringBuilder sDataProductos = new StringBuilder();
      StringBuilder sDataValorProductos = new StringBuilder();
      double dTotalFacturaUsd = 0;
      bool bExito = false;

      if (ViewState["FacturaAdvance_" + pNumContrato] != null)
      {
        DataTable dtFactura = ViewState["FacturaAdvance_" + pNumContrato] as DataTable;
        if (dtFactura != null)
        {
          if (dtFactura.Rows.Count > 0)
          {
            DBConn oConn = new DBConn();
            if (oConn.Open())
            {
              cFactura oFactura = new cFactura(ref oConn);
              oFactura.NumContrato = pNumContrato;
              oFactura.Territory = "CHILE";
              oFactura.DateInvoce = DateTime.Now.ToString("yyyy-MM-dd");
              oFactura.DueDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
              oFactura.TipoFactura = "A";
              oFactura.Accion = "CREAR";
              oFactura.Put();

              if (string.IsNullOrEmpty(oFactura.Error))
                bExito = true;

              string pCodFactura = oFactura.CodFactura;

              foreach (DataRow oRow in dtFactura.Rows)
              {
                string sDescripcionMarca = oRow["Marca"].ToString();
                sDataProductos.Append("<div style=\"padding-top:10px;padding-bottom:10px;\">");
                sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;font-weight:bold;\">");
                sDataProductos.Append(oRow["Marca"].ToString());

                string pCodCategoria = null;
                string pCodSubCategoria = null;

                if (!string.IsNullOrEmpty(oRow["CodCategoria"].ToString()))
                {
                  pCodCategoria = oRow["CodCategoria"].ToString();
                  sDataProductos.Append(" / " + oRow["Categoria"].ToString());
                }

                if (!string.IsNullOrEmpty(oRow["CodSubCategoria"].ToString()))
                {
                  pCodSubCategoria = oRow["CodSubCategoria"].ToString();
                  sDataProductos.Append(" / " + oRow["SubCategoria"].ToString());
                }

                sDataProductos.Append("</font></div>");
                sDataProductos.Append("</div>");

                cDetalleFacturaAdvance oDetalleFacturaAdvance = new cDetalleFacturaAdvance(ref oConn);
                oDetalleFacturaAdvance.CodigoFactura = pCodFactura;
                oDetalleFacturaAdvance.CodMarca = oRow["CodMarca"].ToString();
                oDetalleFacturaAdvance.CodCategoria = pCodCategoria;
                oDetalleFacturaAdvance.CodSubCategoria = pCodSubCategoria;
                oDetalleFacturaAdvance.AdvanceUsd = double.Parse(oRow["Advance USD"].ToString()).ToString();
                oDetalleFacturaAdvance.Accion = "CREAR";
                oDetalleFacturaAdvance.Put();

                if (!string.IsNullOrEmpty(oDetalleFacturaAdvance.Error))
                {
                  bExito = false;
                }

                sDataValorProductos.Append("<div style=\"padding-top:10px;padding-bottom:10px; padding-left:10px;\">");
                sDataValorProductos.Append("<div style=\"float:left;width:30px;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">$</font></div>");
                sDataValorProductos.Append("<div style=\"text-align:right;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">").Append(double.Parse(oRow["Advance USD"].ToString()).ToString("#,##0.00")).Append("</font></div>");
                sDataValorProductos.Append("</div>");

                dTotalFacturaUsd = dTotalFacturaUsd + double.Parse(oRow["Advance USD"].ToString());

              }

              string strNomDeudor = string.Empty;
              string sNomContacto = string.Empty;
              string sEmailContacto = string.Empty;
              string sDireccion = string.Empty;
              string sComuna = string.Empty;
              string sCiudad = string.Empty;

              cContratos oContrato = new cContratos(ref oConn);
              oContrato.NumContrato = pNumContrato;
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
              sNumInvoce = "FE" + pCodFactura;

              oFactura.Total = dTotalFacturaUsd.ToString();
              oFactura.Periodo = "Advance " + DateTime.Now.Year.ToString();
              oFactura.NumInvoice = sNumInvoce;
              oFactura.Accion = "EDITAR";
              oFactura.Put();

              cContratos oContratos = new cContratos(ref oConn);
              oContratos.NumContrato = pNumContrato;
              oContratos.Accion = "EDITAR";
              oContratos.Put();

              sHtml.Replace("[#NUMFACTURA]", sNumInvoce);
              sHtml.Replace("[#NOMEMPRESA]", strNomDeudor);
              sHtml.Replace("[#NOMCONTACTO]", sNomContacto);
              sHtml.Replace("[#EMAILCONTACTO]", sEmailContacto);
              sHtml.Replace("[#DIRECCION]", sDireccion + (!string.IsNullOrEmpty(sComuna) ? ", " + sComuna : string.Empty));
              sHtml.Replace("[#CIUDAD]", sCiudad);
              sHtml.Replace("[#PAIS]", "");
              sHtml.Replace("[#FECHA]", DateTime.Now.ToString("d-MMM-yy", CultureInfo.CreateSpecificCulture("en-US")));
              sHtml.Replace("[#NUMCONTRATO]", pNoContrato);
              sHtml.Replace("[#PERIODOQ]", "Advance " + DateTime.Now.Year.ToString());
              sHtml.Replace("[#PROPERTY]", "");
              sHtml.Replace("[#TERRITORIO]", "CHILE");
              sHtml.Replace("[#DETALLEPRODUCTOS]", sDataProductos.ToString());
              sHtml.Replace("[#DETALLEVALORPRODUCTOS]", sDataValorProductos.ToString());
              sHtml.Replace("[#DUEDATE]", DateTime.Now.AddMonths(1).ToString("d-MMM-yy", CultureInfo.CreateSpecificCulture("en-US")));
              //sHtml.Replace("[#TOTAL]", double.Parse(dTotalFacturaUsd.ToString()).ToString("#,##0.00"));
              sHtml.Replace("[#TOTAL]", double.Parse(dTotalFacturaUsd.ToString()).ToString("N0"));

              if (!Directory.Exists(Server.MapPath("Facturas_Advance/") + pNoContrato + "/"))
                Directory.CreateDirectory(Server.MapPath("Facturas_Advance/") + pNoContrato + "/");

              string sFileHtml = Server.MapPath("Facturas_Advance/") + pNoContrato + "/" + sNumInvoce + ".html";
              File.WriteAllText(sFileHtml, sHtml.ToString(), Encoding.UTF8);

            }
            oConn.Close();
          }
        }
        dtFactura = null;

        if (bExito)
        {
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            string sNoContrato = string.Empty;
            cContratos oContratos = new cContratos(ref oConn);
            oContratos.NumContrato = pNumContrato;
            DataTable dtContrato = oContratos.Get();
            if (dtContrato != null)
            {
              if (dtContrato.Rows.Count > 0)
              {
                sNoContrato = dtContrato.Rows[0]["no_contrato"].ToString();
              }
            }
            dtContrato = null;

            cLogEventos oLogEventos = new cLogEventos(ref oConn);
            oLogEventos.AccionLog = "APROBAR PRE-FACTURA ADVANCE";
            oLogEventos.CodCanal = "2";
            oLogEventos.CodFlujo = "4";
            oLogEventos.NomFlujo = "FACTURACION ADVANCE";
            oLogEventos.NumContrato = pNumContrato;
            oLogEventos.NoContrato = sNoContrato;
            oLogEventos.PeriodoLog = "Advance " + DateTime.Now.Year.ToString();
            //oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
            oLogEventos.CodUser = oUsuario.CodUsuario;
            oLogEventos.RutUser = oUsuario.RutUsuario;
            oLogEventos.NomUser = oUsuario.Nombres;
            oLogEventos.ObsLog = "Se ha autorizado correctamente la Factura Advance " + sNumInvoce;
            oLogEventos.IpLog = oWeb.GetIpUsuario();
            oLogEventos.Accion = "CREAR";
            oLogEventos.Put();
          }
          oConn.Close();

          StringBuilder js = new StringBuilder();
          js.Append("function LgRespuesta() {");
          js.Append(" window.radalert('La factura del contrato " + pNoContrato + " ha sido emitida con éxito.', 330, 210); ");
          js.Append(" Sys.Application.remove_load(LgRespuesta); ");
          js.Append("};");
          js.Append("Sys.Application.add_load(LgRespuesta);");
          Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", js.ToString(), true);
          rdGridLicen.Rebind();
        }
      }
    }

    protected void putAprobarAdvanceAll(string pNumContrato, string pNoContrato)
    {
      StringBuilder sHtml = new StringBuilder();
      sHtml.Append(File.ReadAllText(Server.MapPath("invoice.html")));
      StringBuilder sDataProductos = new StringBuilder();
      StringBuilder sDataValorProductos = new StringBuilder();
      double dTotalFacturaUsd = 0;

      if (ViewState["FacturaAdvance_" + pNumContrato] != null)
      {
        DataTable dtFactura = ViewState["FacturaAdvance_" + pNumContrato] as DataTable;
        if (dtFactura != null)
        {
          if (dtFactura.Rows.Count > 0)
          {
            DBConn oConn = new DBConn();
            if (oConn.Open())
            {
              cFactura oFactura = new cFactura(ref oConn);
              oFactura.NumContrato = pNumContrato;
              oFactura.Territory = "CHILE";
              oFactura.DateInvoce = DateTime.Now.ToString("yyyy-MM-dd");
              oFactura.DueDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
              oFactura.TipoFactura = "A";
              oFactura.Accion = "CREAR";
              oFactura.Put();

              string pCodFactura = oFactura.CodFactura;

              foreach (DataRow oRow in dtFactura.Rows)
              {
                string sDescripcionMarca = oRow["Marca"].ToString();
                sDataProductos.Append("<div style=\"padding-top:10px;padding-bottom:10px;\">");
                sDataProductos.Append("<div><font style=\"font-family:Arial; color:#000000; font-size:10pt;font-weight:bold;\">");
                sDataProductos.Append(oRow["Marca"].ToString());

                string pCodCategoria = null;
                string pCodSubCategoria = null;

                if (!string.IsNullOrEmpty(oRow["CodCategoria"].ToString()))
                {
                  pCodCategoria = oRow["CodCategoria"].ToString();
                  sDataProductos.Append(" / " + oRow["Categoria"].ToString());
                }

                if (!string.IsNullOrEmpty(oRow["CodSubCategoria"].ToString()))
                {
                  pCodSubCategoria = oRow["CodSubCategoria"].ToString();
                  sDataProductos.Append(" / " + oRow["SubCategoria"].ToString());
                }

                sDataProductos.Append("</font></div>");
                sDataProductos.Append("</div>");

                cDetalleFacturaAdvance oDetalleFacturaAdvance = new cDetalleFacturaAdvance(ref oConn);
                oDetalleFacturaAdvance.CodigoFactura = pCodFactura;
                oDetalleFacturaAdvance.CodMarca = oRow["CodMarca"].ToString();
                oDetalleFacturaAdvance.CodCategoria = pCodCategoria;
                oDetalleFacturaAdvance.CodSubCategoria = pCodSubCategoria;
                oDetalleFacturaAdvance.AdvanceUsd = double.Parse(oRow["Advance USD"].ToString()).ToString();
                oDetalleFacturaAdvance.Accion = "CREAR";
                oDetalleFacturaAdvance.Put();

                sDataValorProductos.Append("<div style=\"padding-top:10px;padding-bottom:10px; padding-left:10px;\">");
                sDataValorProductos.Append("<div style=\"float:left;width:30px;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">$</font></div>");
                sDataValorProductos.Append("<div style=\"text-align:right;\"><font style=\"font-family:Arial; color:#000000; font-size:10pt;\">").Append(double.Parse(oRow["Advance USD"].ToString()).ToString("#,##0.00")).Append("</font></div>");
                sDataValorProductos.Append("</div>");

                dTotalFacturaUsd = dTotalFacturaUsd + double.Parse(oRow["Advance USD"].ToString());

              }

              string strNomDeudor = string.Empty;
              string sNomContacto = string.Empty;
              string sEmailContacto = string.Empty;
              string sDireccion = string.Empty;
              string sComuna = string.Empty;
              string sCiudad = string.Empty;

              cContratos oContrato = new cContratos(ref oConn);
              oContrato.NumContrato = pNumContrato;
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

              oFactura.Total = dTotalFacturaUsd.ToString();
              oFactura.Periodo = "Advance " + DateTime.Now.Year.ToString();
              oFactura.NumInvoice = sNumInvoce;
              oFactura.Accion = "EDITAR";
              oFactura.Put();

              cContratos oContratos = new cContratos(ref oConn);
              oContratos.NumContrato = pNumContrato;
              oContratos.Accion = "EDITAR";
              oContratos.Put();

              sHtml.Replace("[#NUMFACTURA]", sNumInvoce);
              sHtml.Replace("[#NOMEMPRESA]", strNomDeudor);
              sHtml.Replace("[#NOMCONTACTO]", sNomContacto);
              sHtml.Replace("[#EMAILCONTACTO]", sEmailContacto);
              sHtml.Replace("[#DIRECCION]", sDireccion + (!string.IsNullOrEmpty(sComuna) ? ", " + sComuna : string.Empty));
              sHtml.Replace("[#CIUDAD]", sCiudad);
              sHtml.Replace("[#PAIS]", "");
              sHtml.Replace("[#FECHA]", DateTime.Now.ToString("d-MMM-yy", CultureInfo.CreateSpecificCulture("en-US")));
              sHtml.Replace("[#NUMCONTRATO]", pNoContrato);
              sHtml.Replace("[#PERIODOQ]", "Advance " + DateTime.Now.Year.ToString());
              sHtml.Replace("[#PROPERTY]", "");
              sHtml.Replace("[#TERRITORIO]", "CHILE");
              sHtml.Replace("[#DETALLEPRODUCTOS]", sDataProductos.ToString());
              sHtml.Replace("[#DETALLEVALORPRODUCTOS]", sDataValorProductos.ToString());
              sHtml.Replace("[#DUEDATE]", DateTime.Now.AddMonths(1).ToString("d-MMM-yy", CultureInfo.CreateSpecificCulture("en-US")));
              //sHtml.Replace("[#TOTAL]", double.Parse(dTotalFacturaUsd.ToString()).ToString("#,##0.00"));
              sHtml.Replace("[#TOTAL]", double.Parse(dTotalFacturaUsd.ToString()).ToString("N0"));

              if (!Directory.Exists(Server.MapPath("Facturas_Advance/") + pNoContrato + "/"))
                Directory.CreateDirectory(Server.MapPath("Facturas_Advance/") + pNoContrato + "/");

              string sFileHtml = Server.MapPath("Facturas_Advance/") + pNoContrato + "/" + sNumInvoce + ".html";
              File.WriteAllText(sFileHtml, sHtml.ToString(), Encoding.UTF8);

              cLogEventos oLogEventos = new cLogEventos(ref oConn);
              oLogEventos.AccionLog = "APROBACION MASIVA PRE-FACTURA ADVANCE";
              oLogEventos.CodCanal = "2";
              oLogEventos.CodFlujo = "4";
              oLogEventos.NomFlujo = "FACTURACION ADVANCE";
              oLogEventos.NumContrato = pNumContrato;
              oLogEventos.NoContrato = pNoContrato;
              oLogEventos.PeriodoLog = "Advance " + DateTime.Now.Year.ToString();
              //oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
              oLogEventos.CodUser = oUsuario.CodUsuario;
              oLogEventos.RutUser = oUsuario.RutUsuario;
              oLogEventos.NomUser = oUsuario.Nombres;
              oLogEventos.ObsLog = "Se ha autorizado correctamente la Factura Advance " + sNumInvoce;
              oLogEventos.IpLog = oWeb.GetIpUsuario();
              oLogEventos.Accion = "CREAR";
              oLogEventos.Put();

            }
            oConn.Close();
          }
        }
        dtFactura = null;

      }
    }

    protected void btnAprobar_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cContratos oContratos = new cContratos(ref oConn);
        oContratos.IsNullFacturadoAdvance = true;
        DataTable dtContratos = oContratos.GetForInvoceAdvance();
        if (dtContratos != null)
        {
          foreach (DataRow oRow in dtContratos.Rows)
          {
            putAprobarAdvanceAll(oRow["num_contrato"].ToString(), oRow["no_contrato"].ToString());
          }

          StringBuilder js = new StringBuilder();
          js.Append("function LgRespuesta() {");
          js.Append(" window.radalert('Todas las facturas han sido emitidas con éxito.', 330, 210); ");
          js.Append(" Sys.Application.remove_load(LgRespuesta); ");
          js.Append("};");
          js.Append("Sys.Application.add_load(LgRespuesta);");
          Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", js.ToString(), true);
          rdGridLicen.Rebind();
        }
        dtContratos = null;
      }
      oConn.Close();
    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {
      Response.Redirect("downloadmanual.ashx");
    }
  }
}