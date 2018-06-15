using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;
using Telerik.Web.UI;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class ingreso_ventas1 : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
      if (!IsPostBack)
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cContratos oContratos = new cContratos(ref oConn);
          oContratos.NkeyDeudor = oUsuario.NKeyUsuario;
          oContratos.Aprobado = true;
          DataTable dtContrato = oContratos.Get();
          if (dtContrato != null)
          {
            if (dtContrato.Rows.Count > 0)
            {
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
    }

    protected void cmbox_contrato_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool bDirect = false;
      if (cmbox_contrato.SelectedValue != "0")
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
          oReporteVenta.NumContrato = cmbox_contrato.SelectedValue;
          oReporteVenta.Facturado = "N";
          oReporteVenta.EstReporte = "P";
          oReporteVenta.OrderMes = true;
          DataTable dtReporteVenta = oReporteVenta.Get();

          if (dtReporteVenta != null)
          {
            if (dtReporteVenta.Rows.Count > 0)
            {
              lblMesdeVenta.Text = dtReporteVenta.Rows[0]["ano_reporte"].ToString();
              ddlmesventa.Items.Clear();
              hddmesventa.Value = dtReporteVenta.Rows[0]["mes_reporte"].ToString();
              oWeb.getMesesDdlist(ddlmesventa, int.Parse(dtReporteVenta.Rows[0]["mes_reporte"].ToString()));


              ViewState["ano_reporte"] = dtReporteVenta.Rows[0]["ano_reporte"].ToString();
              hddCodReporteVenta.Value = dtReporteVenta.Rows[0]["cod_reporte_venta"].ToString();
              bDirect = true;
            }
            else
            {
              oReporteVenta.NumContrato = cmbox_contrato.SelectedValue;
              oReporteVenta.Facturado = "N";
              oReporteVenta.EstReporte = "E";
              oReporteVenta.OrderMes = true;
              DataTable dtRepEliminado = oReporteVenta.Get();

              if (dtRepEliminado != null) {
                if (dtRepEliminado.Rows.Count > 0)
                {
                  lblMesdeVenta.Text = dtRepEliminado.Rows[0]["ano_reporte"].ToString();
                  ddlmesventa.Items.Clear();
                  oWeb.getMesesDdlist(ddlmesventa, int.Parse(dtRepEliminado.Rows[0]["mes_reporte"].ToString()));
                  ViewState["ano_reporte"] = dtRepEliminado.Rows[0]["ano_reporte"].ToString();
                }
                else {
                  oReporteVenta.Facturado = string.Empty;
                  oReporteVenta.EstReporte = string.Empty;
                  oReporteVenta.OrderMes = false;
                  oReporteVenta.OrderDesc = true;
                  DataTable dtRepVenta = oReporteVenta.Get();
                  if (dtRepVenta != null)
                  {
                    if (dtRepVenta.Rows.Count > 0)
                    {
                      bool bExistePeriodoCompleto = true;
                      string sQ = oWeb.getPeriodoByNumMonth(int.Parse(dtRepVenta.Rows[0]["mes_reporte"].ToString()));
                      string[] sMeses = oWeb.getPeriodoMesesQ(sQ).Split(',');

                      cReporteVenta oValidaReporte = new cReporteVenta(ref oConn);
                      foreach (string element in sMeses)
                      {
                        oValidaReporte.MesReporte = element;
                        oValidaReporte.NumContrato = cmbox_contrato.SelectedValue;
                        oValidaReporte.AnoReporte = dtRepVenta.Rows[0]["ano_reporte"].ToString();
                        DataTable dtVal = oValidaReporte.Get();
                        if (dtVal != null)
                        {
                          if (dtVal.Rows.Count == 0)
                          {
                            bExistePeriodoCompleto = false;
                          }
                        }
                        dtVal = null;

                      }

                      DateTime dFechFact;
                      DateTime dFechNext;
                      if (!bExistePeriodoCompleto)
                      {
                        lblMesdeVenta.Text = dtRepVenta.Rows[0]["ano_reporte"].ToString();
                        ddlmesventa.Items.Clear();
                        oWeb.getMesesDdlist(ddlmesventa, int.Parse(dtRepVenta.Rows[0]["mes_reporte"].ToString()));
                        ViewState["ano_reporte"] = dtRepVenta.Rows[0]["ano_reporte"].ToString();
                      }
                      else
                      {
                        dFechFact = DateTime.Parse("01/" + dtRepVenta.Rows[0]["mes_reporte"].ToString() + "/" + dtRepVenta.Rows[0]["ano_reporte"].ToString());
                        dFechNext = dFechFact.AddMonths(1);

                        lblMesdeVenta.Text = dFechNext.Year.ToString();
                        ddlmesventa.Items.Clear();
                        oWeb.getMesesDdlist(ddlmesventa, dFechNext.Month);
                        ViewState["ano_reporte"] = dFechNext.Year.ToString();
                      }
                    }
                    else
                    {
                      cContratos oContratos = new cContratos(ref oConn);
                      oContratos.NumContrato = cmbox_contrato.SelectedValue;
                      DataTable dtContrato = oContratos.Get();
                      if (dtContrato != null)
                      {
                        if (dtContrato.Rows.Count > 0)
                        {
                          lblMesdeVenta.Text = DateTime.Parse(dtContrato.Rows[0]["fech_inicio"].ToString()).Year.ToString();
                          ddlmesventa.Items.Clear();
                          oWeb.getMesesDdlist(ddlmesventa, DateTime.Parse(dtContrato.Rows[0]["fech_inicio"].ToString()).Month);
                          ViewState["ano_reporte"] = DateTime.Parse(dtContrato.Rows[0]["fech_inicio"].ToString()).Year.ToString();
                        }
                      }
                      dtContrato = null;
                    }
                  }
                  dtRepVenta = null;
                }
              }
              dtRepEliminado = null;
                            
            }
          }
          dtReporteVenta = null;
        }
        oConn.Close();
      }
      else
      {
        ddlmesventa.Items.Clear();
        ddlmesventa.Items.Add(new ListItem("", "0"));
        ViewState["ano_reporte"] = string.Empty;
      }

      if ((!string.IsNullOrEmpty(hddCodReporteVenta.Value))&&(bDirect))
      {
        StringBuilder sUrl = new StringBuilder();
        sUrl.Append("confirmacion_ingreso_ventas.aspx?CodReporteVenta=").Append(hddCodReporteVenta.Value);
        sUrl.Append("&MesReporte=").Append(hddmesventa.Value);
        sUrl.Append("&AnoReporte=").Append(ViewState["ano_reporte"].ToString());
        sUrl.Append("&indVentaAnterio=1");
        Response.Redirect(sUrl.ToString());
      }
    }

    protected void ImgBtn_Click(object sender, ImageClickEventArgs e)
    {
      ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Download", "GotoDownloadPage(" + cmbox_contrato.SelectedValue + ");", true);
    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {
      Response.Redirect("downloadmanual.ashx");
    }

    protected void btnContacto_Click(object sender, EventArgs e)
    {
      Response.Redirect("contacto.aspx");
    }

    protected void btnLoadExcel_Click(object sender, EventArgs e)
    {
      try
      {
        System.Threading.Thread.Sleep(2000);
        string FileName = string.Empty;
        string FilePath = string.Empty;
        string Extension = string.Empty;
        if (RadAsyncUpload1.UploadedFiles.Count > 0)
        {
          foreach (UploadedFile ofile in RadAsyncUpload1.UploadedFiles)
          {
            FileName = ofile.GetName();
            Extension = ofile.GetExtension();
            FilePath = Server.MapPath("UploadTemp/") + FileName;
          }
        }
        Import_To_Grid(FilePath, Extension);
      }
      catch
      {
        StringBuilder scriptstring = new StringBuilder();
        scriptstring.Append(" window.radalert('Ha habido un problema en la carga, por favor verifique el archivo y vuelva a intentarlo.', 330, 210); ");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring.ToString(), true);
      }
    }

    private void Import_To_Grid(string FilePath, string Extension)
    {
      int iLine = 0;
      int iColumn = 0;
      string sDataError = string.Empty;
      string sPeriodo = oWeb.getPeriodoVenta(oWeb.getMes(int.Parse(ddlmesventa.SelectedValue)).ToUpper());
      try
      {
        bool bExito = true;
        DataTable dt = new DataTable();
        using (StreamReader sr = new StreamReader(FilePath))
        {
          sDataError = "Error en campos de encabezado del archivo.[br]";
          string[] headers = sr.ReadLine().Split(';');
          foreach (string header in headers)
          {
            iLine = 1;
            dt.Columns.Add(header.Trim());
            iLine++;
          }
          sDataError = "Error en campos de datos del archivo.[br]";
          while (!sr.EndOfStream)
          {
            iLine = 1;
            string[] rows = sr.ReadLine().Split(';');
            DataRow dr = dt.NewRow();
            for (int i = 0; i < headers.Length; i++)
            {
              iColumn = 1;
              dr[i] = rows[i].Trim();
              iColumn++;
            }
            dt.Rows.Add(dr);
            iLine++;
          }

        }
        sDataError = string.Empty;
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cDetalleVenta oDetalleVenta;
          if ((ViewState["cod_reporte_venta"] == null) || (string.IsNullOrEmpty(ViewState["cod_reporte_venta"].ToString())))
          {
            cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
            oReporteVenta.NumContrato = cmbox_contrato.SelectedValue;
            oReporteVenta.MesReporte = ddlmesventa.SelectedValue;
            oReporteVenta.AnoReporte = ViewState["ano_reporte"].ToString();
            oReporteVenta.FechaReporte = DateTime.Now.ToString("yyyy-MM-dd");
            oReporteVenta.Facturado = "N";
            oReporteVenta.Periodo = sPeriodo;
            oReporteVenta.EstReporte = "P";
            oReporteVenta.Accion = "CREAR";
            oReporteVenta.Put();

            if (!string.IsNullOrEmpty(oReporteVenta.Error))
              bExito = false;
            else
              ViewState["cod_reporte_venta"] = oReporteVenta.CodigoReporteVenta;
          }
          else
          {

            cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
            oReporteVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
            oReporteVenta.AnoReporte = ViewState["ano_reporte"].ToString();
            oReporteVenta.FechaReporte = DateTime.Now.ToString("yyyy-MM-dd");
            oReporteVenta.Facturado = "N";
            oReporteVenta.Periodo = sPeriodo;
            oReporteVenta.EstReporte = "P";
            oReporteVenta.Accion = "EDITAR";
            oReporteVenta.Put();

            oDetalleVenta = new cDetalleVenta(ref oConn);
            oDetalleVenta.Accion = "ELIMINAR";
            oDetalleVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
            oDetalleVenta.Put();
          }

          if (bExito)
          {
            iLine = 2;
            //sDataError = string.Empty;
            foreach (DataRow odtRow in dt.Rows)
            {
              //if (bExito)
              string pCodMarca = string.Empty;
              string pCodCategoria = string.Empty;
              string pCodSubCategoria = string.Empty;

              //if (bExito)
              //  sDataError = "columna Marca";
              if (!string.IsNullOrEmpty(odtRow["Marca"].ToString()))
              {
                DataTable dtMarca = getMarca(odtRow["Marca"].ToString());
                if (dtMarca != null)
                {
                  if (dtMarca.Rows.Count > 0)
                  {
                    pCodMarca = dtMarca.Rows[0]["cod_marca"].ToString();
                  }
                }
                dtMarca = null;
              }

              //if (bExito)
              //  sDataError = "columna Categoria";
              if (!string.IsNullOrEmpty(odtRow["Categoria"].ToString()))
              {
                DataTable dtCategoria = getCategoria(odtRow["Categoria"].ToString());
                if (dtCategoria != null)
                {
                  if (dtCategoria.Rows.Count > 0)
                  {
                    pCodCategoria = dtCategoria.Rows[0]["cod_categoria"].ToString();
                  }
                }
                dtCategoria = null;
              }

              //if (bExito)
              //  sDataError = "columna SubCategoria";
              if (!string.IsNullOrEmpty(odtRow["SubCategoria"].ToString()))
              {
                DataTable dtSubCategoria = getSubCategoria(odtRow["SubCategoria"].ToString());
                if (dtSubCategoria != null)
                {
                  if (dtSubCategoria.Rows.Count > 0)
                  {
                    pCodSubCategoria = dtSubCategoria.Rows[0]["cod_subcategoria"].ToString();
                  }
                }
                dtSubCategoria = null;
              }

              oDetalleVenta = new cDetalleVenta(ref oConn);
              oDetalleVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
              oDetalleVenta.Marca = pCodMarca;
              oDetalleVenta.Categoria = (!string.IsNullOrEmpty(pCodCategoria) ? pCodCategoria : null);
              oDetalleVenta.SubCategoria = (!string.IsNullOrEmpty(pCodSubCategoria) ? pCodSubCategoria : null);
              //if (bExito)
              //  sDataError = "columna Producto";
              oDetalleVenta.Producto = odtRow["Producto"].ToString();

              cRoyaltyContrato oRoyaltyContrato = new cRoyaltyContrato(ref oConn);
              oRoyaltyContrato.NumContrato = cmbox_contrato.SelectedValue;
              oRoyaltyContrato.CodMarca = pCodMarca;
              oRoyaltyContrato.CodCategoria = pCodCategoria;
              oRoyaltyContrato.CodSubCategoria = pCodSubCategoria;
              //if (odtRow["Moneda"].ToString() == "USD")
              //  oRoyaltyContrato.CodRoyalty = "3";
              //else
              //  oRoyaltyContrato.NotUSD = true;
              //if (bExito)
              //  sDataError = "columna Desc. Royalty";
              if (odtRow["Desc. Royalty"].ToString().ToUpper() == "STANDARD")
                oRoyaltyContrato.CodRoyalty = "1";
              else if (odtRow["Desc. Royalty"].ToString().ToUpper() == "DIRECT TO CONSUMER")
                oRoyaltyContrato.CodRoyalty = "2";
              else if (odtRow["Desc. Royalty"].ToString().ToUpper() == "FOB")
                oRoyaltyContrato.CodRoyalty = "3";
              else if (odtRow["Desc. Royalty"].ToString().ToUpper() == "DISTRIBUITORS")
                oRoyaltyContrato.CodRoyalty = "4";

              //if (bExito)
              //  sDataError = "oRoyaltyContrato.Get() " + odtRow["Desc. Royalty"].ToString();
              DataTable dtRoyaltyContrato = oRoyaltyContrato.Get();
              if (dtRoyaltyContrato != null)
              {
                if (dtRoyaltyContrato.Rows.Count == 0)
                {
                  bExito = false;
                  oRoyaltyContrato.Marca = odtRow["Marca"].ToString();
                  oRoyaltyContrato.AsqMarcaNull = true;
                  DataTable dtValMarca = oRoyaltyContrato.GetValData();
                  if (dtValMarca != null)
                  {
                    if (dtValMarca.Rows.Count == 0)
                    {
                      if (!string.IsNullOrEmpty(odtRow["Marca"].ToString()))
                        sDataError = sDataError + "Error en la linea " + iLine + ".La marca no corresponde al contrato.[br]";
                      else
                        sDataError = sDataError + "Error en la linea " + iLine + ".La marca esta en blanco.[br]";
                    }
                    else
                    {
                      oRoyaltyContrato.Categoria = odtRow["Categoria"].ToString();
                      oRoyaltyContrato.AsqCategoriaNull = true;
                      DataTable dtValCategoria = oRoyaltyContrato.GetValData();
                      if (dtValCategoria != null)
                      {
                        if (dtValCategoria.Rows.Count == 0)
                        {
                          if (!string.IsNullOrEmpty(odtRow["Categoria"].ToString()))
                            sDataError = sDataError + "Error en la linea " + iLine + ".La categoria no corresponde al contrato.[br]";
                          else
                            sDataError = sDataError + "Error en la linea " + iLine + ".La categoria esta en blanco.[br]";
                        }
                        else
                        {
                          oRoyaltyContrato.SubCategoria = odtRow["SubCategoria"].ToString();
                          oRoyaltyContrato.AsqSubCategoriaNull = true;
                          DataTable dtValSubCategoria = oRoyaltyContrato.Get();
                          if (dtValSubCategoria != null)
                          {
                            if (dtValSubCategoria.Rows.Count == 0)
                            {
                              if (!string.IsNullOrEmpty(odtRow["SubCategoria"].ToString()))
                                sDataError = sDataError + "Error en la linea " + iLine + ".La subcategoria no corresponde al contrato.[br]";
                              else
                                sDataError = sDataError + "Error en la linea " + iLine + ".La subcategoria esta en blanco.[br]";
                            }
                          }
                          dtValSubCategoria = null;
                        }
                      }
                      dtValCategoria = null;
                    }
                  }
                  dtValMarca = null;

                  //sDataError = "Error al tratar de asignar el royalty según los datos ingresados. El error se encuentra en los campos asociados a los datos de Marca, Categoría, Subcategoría o Desc. Royalty.";
                }
                else
                {
                  oDetalleVenta.CodRoyalty = dtRoyaltyContrato.Rows[0]["cod_royalty"].ToString();
                  oDetalleVenta.Royalty = dtRoyaltyContrato.Rows[0]["porcentaje"].ToString();

                  cBdiContrato oBdiContrato = new cBdiContrato(ref oConn);
                  oBdiContrato.NumContrato = cmbox_contrato.SelectedValue;
                  oBdiContrato.CodMarca = pCodMarca;
                  oBdiContrato.CodCategoria = pCodCategoria;
                  oBdiContrato.CodSubCategoria = pCodSubCategoria;
                  DataTable dtBdiContrato = oBdiContrato.Get();
                  if (dtBdiContrato != null)
                  {
                    if (dtBdiContrato.Rows.Count == 0)
                    {
                      bExito = false;
                      sDataError = sDataError + "Error en la linea " + iLine + ".No existe BDI asociado a los datos asociados a numero de contrato, marca, categoría y subcategoría.[br]";
                    }
                    else
                    {
                      oDetalleVenta.Bdi = dtBdiContrato.Rows[0]["porcentaje"].ToString();
                    }
                  }
                  dtBdiContrato = null;
                }
              }
              dtRoyaltyContrato = null;

              //if (bExito)
              //  sDataError = "oBdiContrato.Get()";
              

              //sDataError = "columna Cliente";
              oDetalleVenta.Cliente = odtRow["Cliente"].ToString();

              //sDataError = "columna SKU";
              oDetalleVenta.Sku = odtRow["SKU"].ToString();

              //sDataError = "error  Descripcion Producto";
              oDetalleVenta.DescripcionProducto = odtRow["Descripcion Producto"].ToString();

              if (string.IsNullOrEmpty(odtRow["Precio Unitario Venta Neta"].ToString()))
              {
                bExito = false;
                sDataError = sDataError + "Error en la linea " + iLine + ".Precio Unitario Venta Neta, viene vacio desde la planilla.[br]";
              }
              else
              {
                if (!IsNumber(odtRow["Precio Unitario Venta Neta"].ToString()))
                {
                  bExito = false;
                  sDataError = sDataError + "Error en la linea " + iLine + ".Precio Unitario Venta Neta, No es un numero.[br]";
                }
                else
                {
                  if (double.Parse(odtRow["Precio Unitario Venta Neta"].ToString()) < 0)
                  {
                    bExito = false;
                    sDataError = sDataError + "Error en la linea " + iLine + ".Precio Unitario Venta Neta, es un numero negativo.[br]";
                  }
                  else {
                    oDetalleVenta.PrecioUnitarioVentaBruta = odtRow["Precio Unitario Venta Neta"].ToString();
                  }
                  
                }
              }

              if (string.IsNullOrEmpty(odtRow["Q Venta Neta"].ToString()))
              {
                bExito = false;
                sDataError = sDataError + "Error en la linea " + iLine + ".Q Venta Neta, viene vacio desde la planilla.[br]";
              }
              else
              {
                if (!IsNumber(odtRow["Q Venta Neta"].ToString()))
                {
                  bExito = false;
                  sDataError = sDataError + "Error en la linea " + iLine + ".Q Venta Neta, No es un numero.[br]";
                }
                else
                {
                  if (double.Parse(odtRow["Q Venta Neta"].ToString()) < 0)
                  {
                    bExito = false;
                    sDataError = sDataError + "Error en la linea " + iLine + ".Q Venta Neta, es un numero negativo.[br]";
                  }
                  else {
                    oDetalleVenta.CantidadVentaBruta = odtRow["Q Venta Neta"].ToString();
                  }
                }
              }

              if (string.IsNullOrEmpty(odtRow["Precio Unitario Devolucion / Descuento"].ToString()))
              {
                bExito = false;
                sDataError = sDataError + "Error en la linea " + iLine + ".Precio Unitario Devolucion / Descuento, viene vacio desde la planilla.[br]";
              }
              else
              {
                if (!IsNumber(odtRow["Precio Unitario Devolucion / Descuento"].ToString()))
                {
                  bExito = false;
                  sDataError = sDataError + "Error en la linea " + iLine + ".Precio Unitario Devolucion / Descuento, No es un numero.[br]";
                }
                else
                {
                  if (double.Parse(odtRow["Precio Unitario Devolucion / Descuento"].ToString()) < 0)
                  {
                    bExito = false;
                    sDataError = sDataError + "Error en la linea " + iLine + ".Precio Unitario Devolucion / Descuento, es un numero negativo.[br]";
                  }
                  else {
                    oDetalleVenta.PrecioUnitDescueDevol = odtRow["Precio Unitario Devolucion / Descuento"].ToString();
                  }
                }
              }

              if (string.IsNullOrEmpty(odtRow["Q Devolucion"].ToString()))
              {
                bExito = false;
                sDataError = sDataError + "Error en la linea " + iLine + ".Q Devolucion, viene vacio desde la planilla.[br]";
              }
              else
              {
                if (!IsNumber(odtRow["Q Devolucion"].ToString()))
                {
                  bExito = false;
                  sDataError = sDataError + "Error en la linea " + iLine + ".Q Devolucion, No es un numero.[br]";
                }
                else
                {
                  if (double.Parse(odtRow["Q Devolucion"].ToString()) < 0)
                  {
                    bExito = false;
                    sDataError = sDataError + "Error en la linea " + iLine + ".Q Devolucion, es un numero negativo.[br]";
                  }
                  else {
                    oDetalleVenta.CantidadDescueDevol = odtRow["Q Devolucion"].ToString();
                  }
                }
              }

              oDetalleVenta.Accion = "CREAR";
              oDetalleVenta.Put();

              if (!string.IsNullOrEmpty(oDetalleVenta.Error))
              {
                bExito = false;
                sDataError = sDataError + oDetalleVenta.Error + "[br]";
              }

              oDetalleVenta = null;
              iLine++;
            }
          }
        }

        string sNoContrato = string.Empty;
        cContratos oContratos = new cContratos(ref oConn);
        oContratos.NumContrato = cmbox_contrato.SelectedValue;
        DataTable dtContrato = oContratos.Get();
        if (dtContrato != null)
        {
          if (dtContrato.Rows.Count > 0)
          {
            sNoContrato = dtContrato.Rows[0]["no_contrato"].ToString();
          }
        }
        dtContrato = null;
        oConn.Close();


        if (bExito)
        {
          if (!Directory.Exists(Server.MapPath("rps_licenciatariosmattel/") + "\\"))
            Directory.CreateDirectory(Server.MapPath("rps_licenciatariosmattel/") + "\\");

          string sNameFile =  sNoContrato + "_" + sPeriodo + ViewState["ano_reporte"].ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
          if (File.Exists(Server.MapPath("rps_licenciatariosmattel/" + sNameFile + Extension)))
            File.Delete(Server.MapPath("rps_licenciatariosmattel/" + sNameFile + Extension));

          File.Move(FilePath, Server.MapPath("rps_licenciatariosmattel/" + sNameFile + Extension));

          oConn = new DBConn();
          if (oConn.Open())
          {
            cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
            oReporteVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
            oReporteVenta.ArchivoReporte = sNameFile + Extension;
            oReporteVenta.RepositorioArchivo = "L";
            oReporteVenta.Accion = "EDITAR";
            oReporteVenta.Put();

            cLogEventos oLogEventos = new cLogEventos(ref oConn);
            oLogEventos.AccionLog = "SUBIR ARCHIVO DE VENTAS";
            oLogEventos.CodCanal = "2";
            oLogEventos.CodFlujo = "2";
            oLogEventos.NomFlujo = "INGRESO DE VENTAS LICENCIATARIO";
            oLogEventos.ArchLog = sNameFile + Extension;
            oLogEventos.NumContrato = cmbox_contrato.SelectedValue;
            oLogEventos.NoContrato = sNoContrato;
            oLogEventos.PeriodoLog = sPeriodo + " / " + ViewState["ano_reporte"].ToString();
            oLogEventos.NkeyDeudor = oUsuario.NKeyDeudor;
            oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
            oLogEventos.NomDeudor = oUsuario.Licenciatario;
            oLogEventos.CodUser = oUsuario.CodUsuario;
            oLogEventos.RutUser = oUsuario.RutUsuario;
            oLogEventos.NomUser = oUsuario.Nombres;
            oLogEventos.ObsLog = "Se a cargado el archivo de ventas con exito.";
            oLogEventos.IpLog = oWeb.GetIpUsuario();
            oLogEventos.IndReporsitorioArch = "L";
            oLogEventos.Accion = "CREAR";
            oLogEventos.Put();
          }
          oConn.Close();


          StringBuilder sUrl = new StringBuilder();
          sUrl.Append("confirmacion_ingreso_ventas.aspx?CodReporteVenta=").Append(ViewState["cod_reporte_venta"].ToString());
          sUrl.Append("&MesReporte=").Append(ddlmesventa.SelectedValue);
          sUrl.Append("&AnoReporte=").Append(ViewState["ano_reporte"].ToString());
          Response.Redirect(sUrl.ToString(), false);
        }
        else
        {
          if (!string.IsNullOrEmpty(ViewState["cod_reporte_venta"].ToString()))
          {
            if (oConn.Open())
            {
              cLogEventos oLogEventos = new cLogEventos(ref oConn);
              oLogEventos.AccionLog = "ERROR EN SUBIR ARCHIVO DE VENTAS";
              oLogEventos.CodCanal = "2";
              oLogEventos.CodFlujo = "2";
              oLogEventos.NomFlujo = "INGRESO DE VENTAS LICENCIATARIO";
              oLogEventos.NumContrato = cmbox_contrato.SelectedValue;
              oLogEventos.NoContrato = sNoContrato;
              oLogEventos.PeriodoLog = sPeriodo + " / " + ViewState["ano_reporte"].ToString();
              oLogEventos.NkeyDeudor = oUsuario.NKeyDeudor;
              oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
              oLogEventos.NomDeudor = oUsuario.Licenciatario;
              oLogEventos.CodUser = oUsuario.CodUsuario;
              oLogEventos.RutUser = oUsuario.RutUsuario;
              oLogEventos.NomUser = oUsuario.Nombres;
              oLogEventos.ObsLog = sDataError.Replace("[br]", " ");
              oLogEventos.IpLog = oWeb.GetIpUsuario();
              oLogEventos.Accion = "CREAR";
              oLogEventos.Put();

              cDetalleVenta oDetalleVenta = new cDetalleVenta(ref oConn);
              oDetalleVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
              oDetalleVenta.Accion = "ELIMINAR";
              oDetalleVenta.Put();

              cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
              oReporteVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
              oReporteVenta.Accion = "ELIMINAR";
              oReporteVenta.Put();
            }
            oConn.Close();
          }
          Session["Error"] = sDataError;
          Response.Redirect("error_ingreso_venta.aspx?sTypeError=1", true);

        }
      }
      catch (Exception Ex)
      {
        if (!string.IsNullOrEmpty(hddCodReporteVenta.Value))
        {
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            cLogEventos oLogEventos = new cLogEventos(ref oConn);
            oLogEventos.AccionLog = "ERROR EN SUBIR ARCHIVO DE VENTAS";
            oLogEventos.CodCanal = "2";
            oLogEventos.CodFlujo = "2";
            oLogEventos.NomFlujo = "INGRESO DE VENTAS LICENCIATARIO";
            oLogEventos.PeriodoLog = sPeriodo + " / " + ViewState["ano_reporte"].ToString();
            oLogEventos.NkeyDeudor = oUsuario.NKeyDeudor;
            oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
            oLogEventos.NomDeudor = oUsuario.Licenciatario;
            oLogEventos.CodUser = oUsuario.CodUsuario;
            oLogEventos.RutUser = oUsuario.RutUsuario;
            oLogEventos.NomUser = oUsuario.Nombres;
            oLogEventos.ObsLog = sDataError.Replace("[br]", " ");
            oLogEventos.ObsErrorLog = Ex.Message + " / " + Ex.Source;
            oLogEventos.IpLog = oWeb.GetIpUsuario();
            oLogEventos.Accion = "CREAR";
            oLogEventos.Put();

            cDetalleVenta oDetalleVenta = new cDetalleVenta(ref oConn);
            oDetalleVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
            oDetalleVenta.Accion = "ELIMINAR";
            oDetalleVenta.Put();

            cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
            oReporteVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
            oReporteVenta.Accion = "ELIMINAR";
            oReporteVenta.Put();
          }
          oConn.Close();
        }
        Session["Error"] = sDataError;
        Response.Redirect("error_ingreso_venta.aspx?sTypeError=2", true);

      }
      //rdDetalleVenta.Rebind();
    }

    bool IsNumber(string text)
    {
      Regex regex = new Regex(@"^[-+]?[0-9]*\,?[0-9]+$");
      return regex.IsMatch(text);
    }

    protected DataTable getMarca(string pCodMarca, string pCodContrato)
    {
      DataTable dtMarca = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cMarcas oMarcas = new cMarcas(ref oConn);
        oMarcas.CodMarca = pCodMarca;
        oMarcas.CodContrato = pCodContrato;
        dtMarca = oMarcas.GetByContrato();

      }
      oConn.Close();
      return dtMarca;
    }

    protected DataTable getMarca(string sDesripcion)
    {
      DataTable dtMarca = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cMarcas oMarcas = new cMarcas(ref oConn);
        oMarcas.Descripcion = sDesripcion;
        dtMarca = oMarcas.GetByDescripcion();

      }
      oConn.Close();
      return dtMarca;
    }

    protected DataTable getCategoria(string pCodCategoria, string pCodContrato)
    {
      DataTable dtCategoria = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cCategorias oCategorias = new cCategorias(ref oConn);
        oCategorias.CodContrato = pCodContrato;
        oCategorias.CodCategoria = pCodCategoria;
        dtCategoria = oCategorias.GetByCodContrato();

      }
      oConn.Close();
      return dtCategoria;
    }

    protected DataTable getCategoria(string sDescripcion)
    {
      DataTable dtCategoria = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cCategorias oCategorias = new cCategorias(ref oConn);
        oCategorias.Descripcion = sDescripcion;
        dtCategoria = oCategorias.GetByDescripcion();

      }
      oConn.Close();
      return dtCategoria;
    }

    protected DataTable getSubCategoria(string pCodCategoria, string pCodSubCategoria, string pCodContrato)
    {
      DataTable dtSubCategoria = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cSubCategoria oSubCategoria = new cSubCategoria(ref oConn);
        if (!string.IsNullOrEmpty(pCodCategoria))
        {
          oSubCategoria.CodCategoria = pCodCategoria;
          oSubCategoria.CodContrato = pCodContrato;
          dtSubCategoria = oSubCategoria.GetByContrato();
        }
        if (!string.IsNullOrEmpty(pCodSubCategoria))
        {
          oSubCategoria.CodSubCategoria = pCodSubCategoria;
          dtSubCategoria = oSubCategoria.Get();
        }
      }
      oConn.Close();
      return dtSubCategoria;
    }

    protected DataTable getSubCategoria(string sDescripcion)
    {
      DataTable dtSubCategoria = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cSubCategoria oSubCategoria = new cSubCategoria(ref oConn);
        oSubCategoria.Descripcion = sDescripcion;
        dtSubCategoria = oSubCategoria.Get();
      }
      oConn.Close();
      return dtSubCategoria;
    }

    protected DataTable getRoyalty(string pCodRoyalty, string sDescripcion)
    {
      DataTable dtRoyalty = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cTipoRoyalty oTipoRoyalty = new cTipoRoyalty(ref oConn);
        oTipoRoyalty.CodRoyalty = pCodRoyalty;
        oTipoRoyalty.Descripcion = sDescripcion;
        dtRoyalty = oTipoRoyalty.Get();

      }
      oConn.Close();
      return dtRoyalty;
    }

    protected void btnAutorizar_Click(object sender, EventArgs e)
    {
      string sNoContrato = string.Empty;
      string sPeriodo = oWeb.getPeriodoVenta(oWeb.getMes(int.Parse(ddlmesventa.SelectedValue)).ToUpper());
      cDetalleVenta oDetalleVenta;

      try
      {

        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
          oReporteVenta.CodigoReporteVenta = (((ViewState["cod_reporte_venta"] != null) && (!string.IsNullOrEmpty(ViewState["cod_reporte_venta"].ToString()))) ? ViewState["cod_reporte_venta"].ToString() : string.Empty);
          oReporteVenta.NumContrato = cmbox_contrato.SelectedValue;
          oReporteVenta.MesReporte = ddlmesventa.SelectedValue;
          oReporteVenta.AnoReporte = ViewState["ano_reporte"].ToString();
          oReporteVenta.FechaReporte = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
          oReporteVenta.Facturado = "N";
          oReporteVenta.Periodo = sPeriodo;
          oReporteVenta.EstReporte = "L";
          oReporteVenta.DeclaraMovimiento = "N";
          oReporteVenta.ArchivoReporte = null;
          oReporteVenta.Accion = (((ViewState["cod_reporte_venta"] == null) || (string.IsNullOrEmpty(ViewState["cod_reporte_venta"].ToString()))) ? "CREAR" : "EDITAR");
          oReporteVenta.Put();

          cRoyaltyContrato oRoyaltyContrato = new cRoyaltyContrato(ref oConn);
          oRoyaltyContrato.NumContrato = cmbox_contrato.SelectedValue;
          DataTable dtProdCont = oRoyaltyContrato.GetByInvoce();

          if (dtProdCont != null)
          {
            if (dtProdCont.Rows.Count > 0)
            {
              foreach (DataRow oRow in dtProdCont.Rows)
              {
                oDetalleVenta = new cDetalleVenta(ref oConn);
                oDetalleVenta.CodigoReporteVenta = oReporteVenta.CodigoReporteVenta;
                oDetalleVenta.Marca = (!string.IsNullOrEmpty(oRow["cod_marca"].ToString()) ? oRow["cod_marca"].ToString() : null);
                oDetalleVenta.Categoria = (!string.IsNullOrEmpty(oRow["cod_categoria"].ToString()) ? oRow["cod_categoria"].ToString() : null);
                oDetalleVenta.SubCategoria = (!string.IsNullOrEmpty(oRow["cod_subcategoria"].ToString()) ? oRow["cod_subcategoria"].ToString() : null);
                oDetalleVenta.Producto = string.Empty;
                oDetalleVenta.CodRoyalty = oRow["cod_royalty"].ToString();
                oDetalleVenta.Royalty = oRow["porcentaje"].ToString();


                cBdiContrato oBdiContrato = new cBdiContrato(ref oConn);
                oBdiContrato.NumContrato = cmbox_contrato.SelectedValue;
                oBdiContrato.CodMarca = oRow["cod_marca"].ToString();
                oBdiContrato.CodCategoria = oRow["cod_categoria"].ToString();
                oBdiContrato.CodSubCategoria = oRow["cod_subcategoria"].ToString();
                DataTable dtDBI = oBdiContrato.Get();
                if (dtDBI != null)
                {
                  if (dtDBI.Rows.Count > 0)
                  {
                    oDetalleVenta.Bdi = dtDBI.Rows[0]["porcentaje"].ToString();
                  }
                  else
                  {
                    oDetalleVenta.Bdi = "0.0%";
                  }
                }
                dtDBI = null;

                oDetalleVenta.Cliente = string.Empty;
                oDetalleVenta.Sku = string.Empty;
                oDetalleVenta.DescripcionProducto = string.Empty;
                oDetalleVenta.PrecioUnitarioVentaBruta = "0";
                oDetalleVenta.CantidadVentaBruta = "0";
                oDetalleVenta.PrecioUnitDescueDevol = "0";
                oDetalleVenta.CantidadDescueDevol = "0";
                oDetalleVenta.Accion = "CREAR";
                oDetalleVenta.Put();
              }
            }
          }
          dtProdCont = null;

          cContratos oContratos = new cContratos(ref oConn);
          oContratos.NumContrato = cmbox_contrato.SelectedValue;
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
          oLogEventos.AccionLog = "AUTORIZAR MES SIN MOVIMIENTOS";
          oLogEventos.CodCanal = "2";
          oLogEventos.CodFlujo = "2";
          oLogEventos.NomFlujo = "INGRESO DE VENTAS LICENCIATARIO";
          oLogEventos.NumContrato = cmbox_contrato.SelectedValue;
          oLogEventos.NoContrato = sNoContrato;
          oLogEventos.NkeyDeudor = oUsuario.NKeyDeudor;
          oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
          oLogEventos.NomDeudor = oUsuario.Licenciatario;
          oLogEventos.CodUser = oUsuario.CodUsuario;
          oLogEventos.RutUser = oUsuario.RutUsuario;
          oLogEventos.NomUser = oUsuario.Nombres;
          oLogEventos.PeriodoLog = sPeriodo + " / " + ViewState["ano_reporte"].ToString();
          oLogEventos.IpLog = oWeb.GetIpUsuario();
          oLogEventos.Accion = "CREAR";
          oLogEventos.Put();
        }
        oConn.Close();

        StringBuilder sUrl = new StringBuilder();
        sUrl.Append("ventas_ingresadas.aspx?MesReporte=").Append(ddlmesventa.SelectedValue);
        sUrl.Append("&AnoReporte=").Append(ViewState["ano_reporte"].ToString());
        sUrl.Append("&NoContrato=").Append(sNoContrato);
        Response.Redirect(sUrl.ToString(), false);

      }
      catch (Exception Ex) {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cLogEventos oLogEventos = new cLogEventos(ref oConn);
          oLogEventos.AccionLog = "ERROR EN AUTORIZAR MES SIN MOVIMIENTOS";
          oLogEventos.CodCanal = "2";
          oLogEventos.CodFlujo = "2";
          oLogEventos.NomFlujo = "INGRESO DE VENTAS LICENCIATARIO";
          oLogEventos.NumContrato = cmbox_contrato.SelectedValue;
          oLogEventos.NkeyDeudor = oUsuario.NKeyDeudor;
          oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
          oLogEventos.NomDeudor = oUsuario.Licenciatario;
          oLogEventos.CodUser = oUsuario.CodUsuario;
          oLogEventos.RutUser = oUsuario.RutUsuario;
          oLogEventos.NomUser = oUsuario.Nombres;
          oLogEventos.PeriodoLog = sPeriodo + " / " + ViewState["ano_reporte"].ToString();
          oLogEventos.ObsErrorLog = Ex.Message + " / " + Ex.Source;
          oLogEventos.IpLog = oWeb.GetIpUsuario();
          oLogEventos.Accion = "CREAR";
          oLogEventos.Put();
        }
        oConn.Close();

      }
            
    }

    protected void ddlmesventa_SelectedIndexChanged(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open()) {
        cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
        oReporteVenta.NumContrato = cmbox_contrato.SelectedValue;
        oReporteVenta.MesReporte = ddlmesventa.SelectedValue;
        oReporteVenta.AnoReporte = ViewState["ano_reporte"].ToString();
        DataTable dtReporteVenta = oReporteVenta.Get();
        if (dtReporteVenta != null) {
          if (dtReporteVenta.Rows.Count > 0)
          {
            ViewState["cod_reporte_venta"] = dtReporteVenta.Rows[0]["cod_reporte_venta"].ToString();
          }
          else {
            ViewState["cod_reporte_venta"] = string.Empty;
          }
        }
        dtReporteVenta = null;
      }
      oConn.Close(); 
    }
  }
}