using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace licenciatarios.mattel.debtcontrol
{
  public partial class comprobante_pago_impuesto : System.Web.UI.Page
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

    protected void cmbox_contrato_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cmbox_contrato.SelectedValue != "0")
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cFactura oFactura = new cFactura(ref oConn);
          oFactura.NumContrato = cmbox_contrato.SelectedValue;
          oFactura.isNullComprobante = true;
          DataTable dtComprobanteImpuesto = oFactura.Get();

          if (dtComprobanteImpuesto != null)
          {
            if (dtComprobanteImpuesto.Rows.Count > 0)
            {
              ddlmesventa.Items.Clear();
              ddlmesventa.Items.Add(new ListItem("<< Seleccione Periodo >>", "0"));
              foreach (DataRow oRow in dtComprobanteImpuesto.Rows)
              {
                ddlmesventa.Items.Add(new ListItem(oRow["periodo"].ToString(), oRow["codigo_factura"].ToString()));
              }
            }
            else
            {
              ddlmesventa.Items.Clear();
              ddlmesventa.Items.Add(new ListItem("<< No existen meses a declarar >>", "0"));
            }
          }
          dtComprobanteImpuesto = null;
        }
        oConn.Close();
      }
      else
      {
        ddlmesventa.Items.Clear();
        ddlmesventa.Items.Add(new ListItem("<< No existen meses a declarar >>", "0"));
      }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
      string FileName = string.Empty;
      string FilePath = string.Empty;
      string sNoContrato = string.Empty;
      string sPeriodo = ddlmesventa.Items[ddlmesventa.SelectedIndex].Text.ToUpper();
      string pCodComprobante = string.Empty;

      try
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cContratos oContrato = new cContratos(ref oConn);
          oContrato.NumContrato = cmbox_contrato.SelectedValue;
          DataTable tbContrato = oContrato.Get();
          if (tbContrato != null)
          {
            if (tbContrato.Rows.Count > 0)
            {
              sNoContrato = tbContrato.Rows[0]["no_contrato"].ToString();
            }
          }
          tbContrato = null;
        }
        oConn.Close();

        if (chkbox_declaracion.Checked)
        {
          bool bExito = false;
          if (RadUpload1.UploadedFiles.Count > 0)
          {
            oConn = new DBConn();
            if (oConn.Open())
            {
              foreach (UploadedFile ofile in RadUpload1.UploadedFiles)
              {
                //string FileName = ofile.GetName();
                FileName = cmbox_contrato.SelectedValue + "_" + ddlmesventa.Items[ddlmesventa.SelectedIndex].Text.Replace("/", "_") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                //FilePath = FilePath + FileName;
                //FilePath = Server.MapPath("rps_licenciatariosmattel/") + cmbox_contrato.Items[cmbox_contrato.SelectedIndex].Text + "\\" + FileName;
                //if (!Directory.Exists(Server.MapPath("rps_licenciatariosmattel/") + cmbox_contrato.Items[cmbox_contrato.SelectedIndex].Text + "\\"))
                //  Directory.CreateDirectory(Server.MapPath("rps_licenciatariosmattel/") + cmbox_contrato.Items[cmbox_contrato.SelectedIndex].Text + "\\");

                FilePath = Server.MapPath("rps_licenciatariosmattel/") + "\\" + FileName;
                if (!Directory.Exists(Server.MapPath("rps_licenciatariosmattel/") + "\\"))
                  Directory.CreateDirectory(Server.MapPath("rps_licenciatariosmattel/") + "\\");

                ofile.SaveAs(FilePath);

                cComprobanteImpuesto oComprobanteImpuesto = new cComprobanteImpuesto(ref oConn);
                oComprobanteImpuesto.NumContrato = cmbox_contrato.SelectedValue;
                oComprobanteImpuesto.Periodo = ddlmesventa.Items[ddlmesventa.SelectedIndex].Text;
                oComprobanteImpuesto.NomComprobante = FileName;
                oComprobanteImpuesto.DeclaraMovimiento = "S";
                oComprobanteImpuesto.RepositorioArchivo = "L";
                oComprobanteImpuesto.Accion = "CREAR";
                oComprobanteImpuesto.Put();

                cFactura oFactura = new cFactura(ref oConn);
                oFactura.CodFactura = ddlmesventa.SelectedValue;
                oFactura.CodComprobante = oComprobanteImpuesto.CodComprobante;
                oFactura.Accion = "EDITAR";
                oFactura.Put();

                if (string.IsNullOrEmpty(oComprobanteImpuesto.Error))
                {
                  pCodComprobante = oComprobanteImpuesto.CodComprobante;
                  bExito = true;
                }
              }
              oConn.Close();
            }
          }

          if (bExito)
          {
            oConn = new DBConn();
            if (oConn.Open())
            {
              cLogEventos oLogEventos = new cLogEventos(ref oConn);
              oLogEventos.AccionLog = "SUBIR ARCHIVO COMPROBANTE DE IMPUESTO";
              oLogEventos.CodCanal = "2";
              oLogEventos.CodFlujo = "3";
              oLogEventos.NomFlujo = "COMPROBANTE DE IMPUESTO";
              oLogEventos.ArchLog = FileName;
              oLogEventos.NumContrato = cmbox_contrato.SelectedValue;
              oLogEventos.NoContrato = sNoContrato;
              oLogEventos.PeriodoLog = sPeriodo;
              oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
              oLogEventos.CodUser = oUsuario.CodUsuario;
              oLogEventos.RutUser = oUsuario.RutUsuario;
              oLogEventos.NomUser = oUsuario.Nombres;
              oLogEventos.ObsLog = "Se a cargado el archivo de comprobante de impuesto con éxito.";
              oLogEventos.IpLog = oWeb.GetIpUsuario();
              oLogEventos.IndReporsitorioArch = "L";
              oLogEventos.Accion = "CREAR";
              oLogEventos.Put();
            }
            oConn.Close();

            StringBuilder js = new StringBuilder();
            js.Append("function LgRespuesta() {");
            js.Append(" window.radalert('El comprobante pago de impuesto a sido cargado con éxito.', 330, 210); ");
            js.Append(" Sys.Application.remove_load(LgRespuesta); ");
            js.Append("};");
            js.Append("Sys.Application.add_load(LgRespuesta);");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", js.ToString(), true);

            chkbox_declaracion.Checked = false;
            ddlmesventa.Items.Clear();
            cmbox_contrato.Items.Clear();
            loadContrato();
          }
        }
        else
        {
          oConn = new DBConn();
          if (oConn.Open())
          {
            cComprobanteImpuesto oComprobanteImpuesto = new cComprobanteImpuesto(ref oConn);
            oComprobanteImpuesto.NumContrato = cmbox_contrato.SelectedValue;
            oComprobanteImpuesto.Periodo = ddlmesventa.Items[ddlmesventa.SelectedIndex].Text;
            oComprobanteImpuesto.NomComprobante = string.Empty;
            oComprobanteImpuesto.DeclaraMovimiento = "N";
            oComprobanteImpuesto.Accion = "CREAR";
            oComprobanteImpuesto.Put();

            cFactura oFactura = new cFactura(ref oConn);
            oFactura.CodFactura = ddlmesventa.SelectedValue;
            oFactura.CodComprobante = oComprobanteImpuesto.CodComprobante;
            oFactura.Accion = "EDITAR";
            oFactura.Put();

            chkbox_declaracion.Checked = false;
            ddlmesventa.Items.Clear();
            cmbox_contrato.Items.Clear();
            loadContrato();

            oConn = new DBConn();
            if (oConn.Open())
            {
              cLogEventos oLogEventos = new cLogEventos(ref oConn);
              oLogEventos.AccionLog = "SE DECLARA EL PERIODO SIN MOVIMIENTOS";
              oLogEventos.CodCanal = "2";
              oLogEventos.CodFlujo = "3";
              oLogEventos.NomFlujo = "COMPROBANTE DE IMPUESTO";
              oLogEventos.NumContrato = cmbox_contrato.SelectedValue;
              oLogEventos.NoContrato = sNoContrato;
              oLogEventos.PeriodoLog = sPeriodo + " / " + ViewState["ano_reporte"].ToString();
              oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
              oLogEventos.CodUser = oUsuario.CodUsuario;
              oLogEventos.RutUser = oUsuario.RutUsuario;
              oLogEventos.NomUser = oUsuario.Nombres;
              oLogEventos.ObsLog = "Se a declarado el periodo sin movimientos con éxito.";
              oLogEventos.IpLog = oWeb.GetIpUsuario();
              oLogEventos.Accion = "CREAR";
              oLogEventos.Put();
            }
            oConn.Close();

            StringBuilder js = new StringBuilder();
            js.Append("function LgRespuesta() {");
            js.Append(" window.radalert('Se ha declarado el periodo sin movimientos con éxito.', 330, 210); ");
            js.Append(" Sys.Application.remove_load(LgRespuesta); ");
            js.Append("};");
            js.Append("Sys.Application.add_load(LgRespuesta);");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", js.ToString(), true);
          }
          oConn.Close();
        }
      }
      catch (Exception Ex)
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cLogEventos oLogEventos = new cLogEventos(ref oConn);
          oLogEventos.AccionLog = "ERROR SUBIR ARCHIVO COMPROBANTE DE IMPUESTO";
          oLogEventos.CodCanal = "2";
          oLogEventos.CodFlujo = "3";
          oLogEventos.NomFlujo = "COMPROBANTE DE IMPUESTO";
          oLogEventos.PeriodoLog = sPeriodo;
          oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
          oLogEventos.CodUser = oUsuario.CodUsuario;
          oLogEventos.RutUser = oUsuario.RutUsuario;
          oLogEventos.NomUser = oUsuario.Nombres;
          oLogEventos.ObsErrorLog = Ex.Message + " / " + Ex.Source;
          oLogEventos.IpLog = oWeb.GetIpUsuario();
          oLogEventos.Accion = "CREAR";
          oLogEventos.Put();
        }
        oConn.Close();

        Session["Error"] = "Error en la carga de archivo de comprobante de impuesto. Por favor comuniquese con el administrador.";
        Response.Redirect("error_ingreso_venta.aspx?sTypeError=2", true);
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