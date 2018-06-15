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

namespace licenciatarios.mattel.debtcontrol.adm
{
  public partial class mserviciomensajeria : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Web oWeb = new Web();
      if (!IsPostBack) {
        getDataConfigMail();
      }
    }

    protected void getDataConfigMail() {
      DataTable dtConfig;
      DBConn oConn = new DBConn();
      if (oConn.Open()) {
        cConfigMensajes oConfigMensajes = new cConfigMensajes(ref oConn);
        oConfigMensajes.TipoEmail = "R";
        dtConfig = oConfigMensajes.Get();
        if (dtConfig != null) {
          if (dtConfig.Rows.Count > 0) {
            txt_dia_tope_ingventa1.Text = dtConfig.Rows[0]["dia_config_msn"].ToString();
            txt_cant_ant_tope_ingventa.Text = dtConfig.Rows[0]["cant_dias_config_msn"].ToString();
          }
        }
        dtConfig = null;

        oConfigMensajes.TipoEmail = "N";
        dtConfig = oConfigMensajes.Get();
        if (dtConfig != null)
        {
          if (dtConfig.Rows.Count > 0)
          {
            txt_dia_tope_ingventa2.Text = dtConfig.Rows[0]["dia_config_msn"].ToString();
            txt_cant_desp_tope_ingventa.Text = dtConfig.Rows[0]["cant_dias_config_msn"].ToString();
          }
        }
        dtConfig = null;

        oConfigMensajes.TipoEmail = "P";
        dtConfig = oConfigMensajes.Get();
        if (dtConfig != null)
        {
          if (dtConfig.Rows.Count > 0)
          {
            //txt_dia_tope_ingventa2.Text = dtConfig.Rows[0]["dia_config_msn"].ToString();
            txt_cant_desp_q.Text = dtConfig.Rows[0]["cant_dias_config_msn"].ToString();
          }
        }
        dtConfig = null;

        oConfigMensajes.TipoEmail = "E";
        dtConfig = oConfigMensajes.Get();
        if (dtConfig != null)
        {
          if (dtConfig.Rows.Count > 0)
          {
            txt_dia_tope_ingventa3.Text = dtConfig.Rows[0]["dia_config_msn"].ToString();
            txt_cant_desp_tope_ingventa2.Text = dtConfig.Rows[0]["cant_dias_config_msn"].ToString();
          }
        }
        dtConfig = null;

        oConfigMensajes.TipoEmail = "F";
        dtConfig = oConfigMensajes.Get();
        if (dtConfig != null)
        {
          if (dtConfig.Rows.Count > 0)
          {
            txt_horario_factura.Text = dtConfig.Rows[0]["dia_config_msn"].ToString();
          }
        }
        dtConfig = null;

        oConfigMensajes.TipoEmail = "L";
        dtConfig = oConfigMensajes.Get();
        if (dtConfig != null)
        {
          if (dtConfig.Rows.Count > 0)
          {
            txt_horario_fact_generadas.Text = dtConfig.Rows[0]["dia_config_msn"].ToString();
          }
        }
        dtConfig = null;

        oConfigMensajes.TipoEmail = "A";
        dtConfig = oConfigMensajes.Get();
        if (dtConfig != null)
        {
          if (dtConfig.Rows.Count > 0)
          {
            txt_dias_espera.Text = dtConfig.Rows[0]["dia_config_msn"].ToString();
          }
        }
        dtConfig = null;

        oConfigMensajes.TipoEmail = "Q";
        dtConfig = oConfigMensajes.Get();
        if (dtConfig != null)
        {
          if (dtConfig.Rows.Count > 0)
          {
            txt_cant_hrs_cierre.Text = dtConfig.Rows[0]["dia_config_msn"].ToString();
            txt_horario_cierre.Text = dtConfig.Rows[0]["cant_dias_config_msn"].ToString();
          }
        }
        dtConfig = null;
        
      }
      oConn.Close();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open()) {
        cConfigMensajes oConfigMensajes = new cConfigMensajes(ref oConn);
        oConfigMensajes.TipoEmail = "R";
        oConfigMensajes.DiaConfigMsn = txt_dia_tope_ingventa1.Text;
        oConfigMensajes.CantDiasConfigMsn = txt_cant_ant_tope_ingventa.Text;
        oConfigMensajes.Accion = "EDITAR";
        oConfigMensajes.Put();

        oConfigMensajes.TipoEmail = "N";
        oConfigMensajes.DiaConfigMsn = txt_dia_tope_ingventa2.Text;
        oConfigMensajes.CantDiasConfigMsn = txt_cant_desp_tope_ingventa.Text;
        oConfigMensajes.Accion = "EDITAR";
        oConfigMensajes.Put();

        oConfigMensajes.TipoEmail = "P";
        oConfigMensajes.DiaConfigMsn = null;
        oConfigMensajes.CantDiasConfigMsn = txt_cant_desp_q.Text;
        oConfigMensajes.Accion = "EDITAR";
        oConfigMensajes.Put();

        oConfigMensajes.TipoEmail = "E";
        oConfigMensajes.DiaConfigMsn = txt_dia_tope_ingventa3.Text;
        oConfigMensajes.CantDiasConfigMsn = txt_cant_desp_tope_ingventa2.Text;
        oConfigMensajes.Accion = "EDITAR";
        oConfigMensajes.Put();

        oConfigMensajes.TipoEmail = "F";
        oConfigMensajes.DiaConfigMsn = txt_horario_factura.Text;
        oConfigMensajes.CantDiasConfigMsn = null;
        oConfigMensajes.Accion = "EDITAR";
        oConfigMensajes.Put();

        oConfigMensajes.TipoEmail = "L";
        oConfigMensajes.DiaConfigMsn = txt_horario_fact_generadas.Text;
        oConfigMensajes.CantDiasConfigMsn = null;
        oConfigMensajes.Accion = "EDITAR";
        oConfigMensajes.Put();

        oConfigMensajes.TipoEmail = "A";
        oConfigMensajes.DiaConfigMsn = txt_dias_espera.Text;
        oConfigMensajes.CantDiasConfigMsn = null;
        oConfigMensajes.Accion = "EDITAR";
        oConfigMensajes.Put();

        oConfigMensajes.TipoEmail = "Q";
        oConfigMensajes.DiaConfigMsn = txt_cant_hrs_cierre.Text;
        oConfigMensajes.CantDiasConfigMsn = txt_horario_cierre.Text;
        oConfigMensajes.Accion = "EDITAR";
        oConfigMensajes.Put();
      }
      oConn.Close();

      StringBuilder js = new StringBuilder();
      js.Append("function LgRespuesta() {");
      js.Append(" window.radalert('Los datos para la configuración han sido grabados correctamente.', 330, 210); ");
      js.Append(" Sys.Application.remove_load(LgRespuesta); ");
      js.Append("};");
      js.Append("Sys.Application.add_load(LgRespuesta);");
      Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", js.ToString(), true);
    }
  }
}