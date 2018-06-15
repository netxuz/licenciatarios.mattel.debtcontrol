using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;
using Telerik.Web.UI;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class confirmacion_ingreso_ventas : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();

      if (!IsPostBack)
      {
        hddCodReporteVenta.Value = oWeb.GetData("CodReporteVenta");
        hddMesReporte.Value = oWeb.GetData("MesReporte");
        hddAnoReporte.Value = oWeb.GetData("AnoReporte");
        lblmesventa.Text = "Mes de venta: " + oWeb.getMes(int.Parse(hddMesReporte.Value)).ToUpper() + " / " + hddAnoReporte.Value;

        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
          oReporteVenta.CodigoReporteVenta = hddCodReporteVenta.Value;
          DataTable tblReporteVenta = oReporteVenta.Get();
          if (tblReporteVenta != null)
          {
            if (tblReporteVenta.Rows.Count > 0)
            {
              cContratos oContratos = new cContratos(ref oConn);
              oContratos.NumContrato = tblReporteVenta.Rows[0]["num_contrato"].ToString();
              DataTable dtContrato = oContratos.Get();
              if (dtContrato != null)
              {
                if (dtContrato.Rows.Count > 0)
                {
                  lblcontrato.Text = "Contrato: " + dtContrato.Rows[0]["no_contrato"].ToString();
                  hddNumContrato.Value = tblReporteVenta.Rows[0]["num_contrato"].ToString();
                  hddNoContrato.Value = dtContrato.Rows[0]["no_contrato"].ToString();
                }
              }
              dtContrato = null;
            }
          }
          tblReporteVenta = null;
        }
        oConn.Close();


        if (!string.IsNullOrEmpty(oWeb.GetData("indVentaAnterio"))) {

          oConn = new DBConn();
          if (oConn.Open())
          {
            cDetalleVenta DetalleVenta = new cDetalleVenta(ref oConn);
            DetalleVenta.CodigoReporteVenta = hddCodReporteVenta.Value;
            DataTable td = DetalleVenta.Get();
            if (td != null) {
              if (td.Rows.Count == 0)
              {
                btnGuardar.Visible = false;
                lblAlerta.Text = "<strong>ATENCION!</strong> Se ha detectado un ingreso erroneo de ventas para el mes de <strong>" + oWeb.getMes(int.Parse(hddMesReporte.Value)).ToUpper() + "</strong>. Para corregirlo por favor presione el botón <strong>Cancelar</strong>, y luego vuelva a cargar las ventas.";
              }
              else {
                lblAlerta.Text = "<strong>ATENCION!</strong> Usted tiene ventas ingresadas para el mes de <strong>" + oWeb.getMes(int.Parse(hddMesReporte.Value)).ToUpper() + "</strong>, si esta de acuerdo con estas, presione el botón <strong>Autorizar Periodo</strong> para terminar con el proceso de lo contrario presione el botón <strong>Cancelar</strong>.";
              }
            }
            td = null;
          }
          oConn.Close();          
        }
        else
          lblAlerta.Text = "<strong>ATENCION!</strong> Para terminar el proceso, presione el botón <strong>Autorizar Periodo</strong>, de lo contrario presione el botón <strong>Cancelar</strong>.";
      }
    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {
      Response.Redirect("downloadmanual.ashx");
    }

    protected void btnContacto_Click(object sender, EventArgs e)
    {
      Response.Redirect("contacto.aspx");
    }

    protected void rdDetalleVenta_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cDetalleVenta oDetalleVenta = new cDetalleVenta(ref oConn);
        oDetalleVenta.CodigoReporteVenta = hddCodReporteVenta.Value;
        rdDetalleVenta.DataSource = oDetalleVenta.Get();
      }
      oConn.Close();
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
      string sPeriodo = oWeb.getPeriodoVenta(oWeb.getMes(int.Parse(hddMesReporte.Value)).ToUpper());
      if (Page.IsValid)
      {
        try
        {
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
            oReporteVenta.CodigoReporteVenta = hddCodReporteVenta.Value;
            oReporteVenta.FechaReporte = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            oReporteVenta.Facturado = "N";
            oReporteVenta.EstReporte = "L";
            oReporteVenta.Periodo = sPeriodo;
            oReporteVenta.DeclaraMovimiento = "S";
            oReporteVenta.Accion = "EDITAR";
            oReporteVenta.Put();

            cLogEventos oLogEventos = new cLogEventos(ref oConn);
            oLogEventos.AccionLog = "AUTORIZACIÓN OK DE REPORTE DE VENTAS";
            oLogEventos.CodCanal = "2";
            oLogEventos.CodFlujo = "2";
            oLogEventos.NomFlujo = "INGRESO DE VENTAS LICENCIATARIO";
            oLogEventos.NumContrato = hddNumContrato.Value;
            oLogEventos.NoContrato = hddNoContrato.Value;
            oLogEventos.PeriodoLog = sPeriodo + " / " + hddAnoReporte.Value;
            oLogEventos.NkeyDeudor = oUsuario.NKeyDeudor;
            oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
            oLogEventos.NomDeudor = oUsuario.Licenciatario;
            oLogEventos.CodUser = oUsuario.CodUsuario;
            oLogEventos.RutUser = oUsuario.RutUsuario;
            oLogEventos.NomUser = oUsuario.Nombres;
            oLogEventos.ObsLog = "Se a autorizado correctamente el inhgreso de ventas.";
            oLogEventos.IpLog = oWeb.GetIpUsuario();
            oLogEventos.Accion = "CREAR";
            oLogEventos.Put();
          }
          oConn.Close();

          StringBuilder sUrl = new StringBuilder();
          sUrl.Append("ventas_ingresadas.aspx?MesReporte=").Append(hddMesReporte.Value);
          sUrl.Append("&AnoReporte=").Append(hddAnoReporte.Value);
          sUrl.Append("&NoContrato=").Append(hddNoContrato.Value);
          Response.Redirect(sUrl.ToString(), false);

        }
        catch (Exception Ex)
        {
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            cLogEventos oLogEventos = new cLogEventos(ref oConn);
            oLogEventos.AccionLog = "ERROR EN AUTORIZACION DE INGRESO DE REPORTE DE VENTAS";
            oLogEventos.CodCanal = "2";
            oLogEventos.CodFlujo = "2";
            oLogEventos.NomFlujo = "INGRESO DE VENTAS LICENCIATARIO";
            oLogEventos.PeriodoLog = sPeriodo + " / " + hddAnoReporte.Value;
            oLogEventos.NkeyDeudor = oUsuario.NKeyDeudor;
            oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
            oLogEventos.NomDeudor = oUsuario.Licenciatario;
            oLogEventos.CodUser = oUsuario.CodUsuario;
            oLogEventos.RutUser = oUsuario.RutUsuario;
            oLogEventos.NomUser = oUsuario.Nombres;
            oLogEventos.ObsErrorLog = Ex.Message + " / " + Ex.Source;
            oLogEventos.IpLog = oWeb.GetIpUsuario();
            oLogEventos.Accion = "CREAR";
            oLogEventos.Put();

            cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
            oReporteVenta.CodigoReporteVenta = hddCodReporteVenta.Value;
            oReporteVenta.FechaReporte = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            oReporteVenta.Facturado = "N";
            oReporteVenta.EstReporte = "P";
            oReporteVenta.Periodo = sPeriodo;
            oReporteVenta.Accion = "EDITAR";
            oReporteVenta.Put();
          }
          oConn.Close();

          Response.Redirect("error_ingreso_venta.aspx", false);
        }
      }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cDetalleVenta oDetalleVenta = new cDetalleVenta(ref oConn);
        oDetalleVenta.CodigoReporteVenta = hddCodReporteVenta.Value;
        oDetalleVenta.Accion = "ELIMINAR";
        oDetalleVenta.Put();

        cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
        oReporteVenta.CodigoReporteVenta = hddCodReporteVenta.Value;
        oReporteVenta.Facturado = "N";
        oReporteVenta.EstReporte = "E";
        oReporteVenta.Accion = "EDITAR";
        oReporteVenta.Put();
      }
      oConn.Close();

      Session["Error"] = "Error en el proceso de autorización de ingreso de ventas. Por favor comuniquese con el administrador del sitio.";
      Response.Redirect("ingreso_ventas.aspx");
    }
  }
}