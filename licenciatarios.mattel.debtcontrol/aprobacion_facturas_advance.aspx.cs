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
  public partial class aprobacion_facturas_advance : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
      if (!IsPostBack)
      {
        lblNoContrato.Text = oWeb.GetData("pNoContrato");
        hdd_no_contrato.Value = oWeb.GetData("pNoContrato");
        hdd_num_contrato.Value = oWeb.GetData("pNumContrato");
      }
      getDataInvoceAdvance();
    }

    protected void getDataInvoceAdvance()
    {
      FacturaAdvance oFactura = new FacturaAdvance();
      oFactura.getMakeTable();

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAdvanceContrato oAdvanceContrato = new cAdvanceContrato(ref oConn);
        oAdvanceContrato.NumContrato = hdd_num_contrato.Value;
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

          RadGrid oGridFractura = new RadGrid();
          oGridFractura.ID = "rdGridFactura";
          oGridFractura.ShowStatusBar = true;
          oGridFractura.ShowFooter = true;
          oGridFractura.AutoGenerateColumns = false;
          oGridFractura.Skin = "Sitefinity";
          oGridFractura.ItemDataBound += oGridFractura_ItemDataBound;
          oGridFractura.ItemCommand += oGridFractura_ItemCommand;

          oGridFractura.ExportSettings.HideStructureColumns = true;

          oGridFractura.MasterTableView.AutoGenerateColumns = false;
          oGridFractura.MasterTableView.ShowHeader = true;
          oGridFractura.MasterTableView.TableLayout = GridTableLayout.Fixed;
          oGridFractura.MasterTableView.ShowHeadersWhenNoRecords = true;
          oGridFractura.MasterTableView.ShowFooter = true;
          oGridFractura.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
          oGridFractura.MasterTableView.CommandItemSettings.ShowExportToExcelButton = true;
          oGridFractura.MasterTableView.CommandItemSettings.ShowRefreshButton = false;
          oGridFractura.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

          GridBoundColumn oGridBoundColumn;
          oGridBoundColumn = new GridBoundColumn();
          oGridBoundColumn.DataField = "Marca";
          oGridBoundColumn.HeaderText = "Marca";
          oGridBoundColumn.UniqueName = "Marca";
          oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

          oGridBoundColumn = new GridBoundColumn();
          oGridBoundColumn.DataField = "Categoria";
          oGridBoundColumn.HeaderText = "Categoria";
          oGridBoundColumn.UniqueName = "Categoria";
          oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

          oGridBoundColumn = new GridBoundColumn();
          oGridBoundColumn.DataField = "SubCategoria";
          oGridBoundColumn.HeaderText = "SubCategoria";
          oGridBoundColumn.UniqueName = "SubCategoria";
          oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

          oGridBoundColumn = new GridBoundColumn();
          oGridBoundColumn.DataField = "Advance USD";
          oGridBoundColumn.HeaderText = "Advance USD";
          oGridBoundColumn.UniqueName = "Advance USD";
          oGridBoundColumn.FooterText = "Total Advance USD";
          oGridBoundColumn.DataFormatString = "{0:N0}";
          oGridBoundColumn.FooterAggregateFormatString = "{0:N0}";
          oGridBoundColumn.Aggregate = GridAggregateFunction.Sum;
          oGridFractura.MasterTableView.Columns.Add(oGridBoundColumn);

          oGridFractura.DataSource = dtFactura;
          idGrilla.Visible = true;
          idGrilla.Controls.Add(oGridFractura);
          idBtnSave.Visible = true;

          ViewState["FacturaAdvance"] = dtFactura;

          RadAjaxManager oRadAjaxManager = new RadAjaxManager();
          oRadAjaxManager.ClientEvents.OnRequestStart = "onRequestStart";

          AjaxUpdatedControl oAjaxUpdatedControl = new AjaxUpdatedControl();
          oAjaxUpdatedControl.ControlID = "rdGridFactura";

          AjaxSetting oAjaxSetting = new AjaxSetting();
          oAjaxSetting.AjaxControlID = "rdGridFactura";
          oAjaxSetting.UpdatedControls.Add(oAjaxUpdatedControl);

          oRadAjaxManager.AjaxSettings.Add(oAjaxSetting);
          idGrilla.Controls.Add(oRadAjaxManager);
        }
      }
      oConn.Close();
    }

    void oGridFractura_ItemCommand(object source, GridCommandEventArgs e)
    {
      if (e.CommandName == RadGrid.ExportToExcelCommandName)
      {
        RadGrid rdGridFactura = (RadGrid)idGrilla.FindControl("rdGridFactura");
        rdGridFactura.ExportSettings.ExportOnlyData = true;
        rdGridFactura.ExportSettings.IgnorePaging = true;
        rdGridFactura.ExportSettings.OpenInNewWindow = true;
        rdGridFactura.ExportSettings.FileName = "reporte_facturas_advance" + DateTime.Now.ToString("yyyyMMdd");
        rdGridFactura.MasterTableView.ExportToExcel();
      }
    }

    void oGridFractura_ItemDataBound(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;

        item["Advance USD"].Text = double.Parse(row["Advance USD"].ToString()).ToString("#,##0.00");

      }

    }

    protected void btnAprobar_Click(object sender, EventArgs e)
    {
      string sNumInvoce = string.Empty;
      StringBuilder sHtml = new StringBuilder();
      sHtml.Append(File.ReadAllText(Server.MapPath("invoice.html")));
      StringBuilder sDataProductos = new StringBuilder();
      StringBuilder sDataValorProductos = new StringBuilder();
      double dTotalFacturaUsd = 0;
      bool bExito = false;

      if (ViewState["FacturaAdvance"] != null)
      {
        DataTable dtFactura = ViewState["FacturaAdvance"] as DataTable;
        if (dtFactura != null)
        {
          if (dtFactura.Rows.Count > 0)
          {
            DBConn oConn = new DBConn();
            if (oConn.Open())
            {
              cFactura oFactura = new cFactura(ref oConn);
              oFactura.NumContrato = hdd_num_contrato.Value;
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

                if (!string.IsNullOrEmpty(oRow["CodCategoria"].ToString())) { 
                  pCodCategoria = oRow["CodCategoria"].ToString();
                  sDataProductos.Append(" / " + oRow["Categoria"].ToString());
                }

                if (!string.IsNullOrEmpty(oRow["CodSubCategoria"].ToString())) { 
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
              oContrato.NumContrato = hdd_num_contrato.Value;
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
              oContratos.NumContrato = hdd_num_contrato.Value;
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
              sHtml.Replace("[#NUMCONTRATO]", hdd_no_contrato.Value);
              sHtml.Replace("[#PERIODOQ]", "Advance " + DateTime.Now.Year.ToString());
              sHtml.Replace("[#PROPERTY]", "");
              sHtml.Replace("[#TERRITORIO]", "CHILE");
              sHtml.Replace("[#DETALLEPRODUCTOS]", sDataProductos.ToString());
              sHtml.Replace("[#DETALLEVALORPRODUCTOS]", sDataValorProductos.ToString());
              sHtml.Replace("[#DUEDATE]", DateTime.Now.AddMonths(1).ToString("d-MMM-yy", CultureInfo.CreateSpecificCulture("en-US")));
              //sHtml.Replace("[#TOTAL]", double.Parse(dTotalFacturaUsd.ToString()).ToString("#,##0.00"));
              sHtml.Replace("[#TOTAL]", double.Parse(dTotalFacturaUsd.ToString()).ToString("N0"));

              if (!Directory.Exists(Server.MapPath("Facturas_Advance/") + hdd_no_contrato.Value + "/"))
                Directory.CreateDirectory(Server.MapPath("Facturas_Advance/") + hdd_no_contrato.Value + "/");

              string sFileHtml = Server.MapPath("Facturas_Advance/") + hdd_no_contrato.Value + "/" + sNumInvoce + ".html";
              File.WriteAllText(sFileHtml, sHtml.ToString(),Encoding.UTF8);

            }
            oConn.Close();
          }
        }
        dtFactura = null;

        if (bExito)
        {
          idGrilla.Visible = false;
          idBtnSave.Visible = false;

          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            string sNoContrato = string.Empty;
            cContratos oContratos = new cContratos(ref oConn);
            oContratos.NumContrato = hdd_num_contrato.Value;
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
            oLogEventos.AccionLog = "APROBAR POR DETALLE PRE-FACTURA ADVANCE";
            oLogEventos.CodCanal = "2";
            oLogEventos.CodFlujo = "4";
            oLogEventos.NomFlujo = "FACTURACION ADVANCE";
            oLogEventos.NumContrato = hdd_num_contrato.Value;
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
          js.Append(" window.radalert('La factura ha sido emitida con éxito.', 330, 210); ");
          js.Append(" Sys.Application.remove_load(LgRespuesta); ");
          js.Append("};");
          js.Append("Sys.Application.add_load(LgRespuesta);");
          Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", js.ToString(), true);
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
  }
}