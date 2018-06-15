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

namespace licenciatarios.mattel.debtcontrol
{
  public partial class _default : System.Web.UI.Page
  {
    Usuario oUsuario;
    Web oWeb = new Web();

    protected void Page_Load(object sender, EventArgs e)
    {
      oUsuario = oWeb.GetObjUsuario();
    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
      //Session["bLicenciatario"] = string.Empty;
      string sRutEmpresa = txt_box_empresa.Text;
      string sLogin = txt_rut.Text;

      if (Page.IsValid)
      {
        try
        {
          bool bExito = false;
          string sError = string.Empty;
          string sPwd = oWeb.Crypt(txt_password.Text);

          if (sRutEmpresa.IndexOf("-") >= 0)
          {
            string sRut = sRutEmpresa.Substring(0, sRutEmpresa.IndexOf("-"));
            string sDv = sRutEmpresa.Substring(sRutEmpresa.IndexOf("-") + 1);

            DBConn oConn = new DBConn();
            if (oConn.Open())
            {
              cSysUsuario oSysUsuario = new cSysUsuario(ref oConn);
              oSysUsuario.LoginUser = sLogin;
              oSysUsuario.PwdUser = sPwd;
              DataTable dtUser = oSysUsuario.Get();
              if (dtUser != null)
              {
                if (dtUser.Rows.Count > 0)
                {
                  oUsuario = oWeb.GetObjUsuario();
                  oUsuario.CodUsuario = dtUser.Rows[0]["cod_user"].ToString();
                  oUsuario.Nombres = (dtUser.Rows[0]["nom_user"].ToString() + " " + dtUser.Rows[0]["ape_user"].ToString()).Trim();
                  oUsuario.Pais = cmbox_pais.SelectedValue;
                  oUsuario.Email = dtUser.Rows[0]["eml_user"].ToString();
                  oUsuario.CodTipoUsuario = dtUser.Rows[0]["cod_tipo"].ToString();
                  oUsuario.RutLicenciatario = sRutEmpresa;
                  oUsuario.RutUsuario = sLogin;


                  cSysPerfilesUsuarios oSysPerfilesUsuarios = new cSysPerfilesUsuarios(ref oConn);
                  oSysPerfilesUsuarios.CodUser = dtUser.Rows[0]["cod_user"].ToString();
                  DataTable dtUserPerfil = oSysPerfilesUsuarios.Get();
                  if (dtUserPerfil != null)
                  {
                    foreach (DataRow oRow in dtUserPerfil.Rows)
                    {
                      if (oRow["cod_perfil"].ToString() == "4")
                      {
                        cSysUserCliente oSysUserCliente = new cSysUserCliente(ref oConn);
                        oSysUserCliente.CodUser = dtUser.Rows[0]["cod_user"].ToString();
                        oSysUserCliente.Rut = sRut;
                        oSysUserCliente.Dv = sDv;
                        DataTable dtUserCliente = oSysUserCliente.GetByLogin();
                        if (dtUserCliente != null)
                        {
                          if (dtUserCliente.Rows.Count > 0)
                          {
                            bExito = true;
                            oUsuario.NKeyUsuario = dtUserCliente.Rows[0]["nkey_cliente"].ToString();
                            oUsuario.VistaMenu = dtUserCliente.Rows[0]["tipo_cliente"].ToString();
                            oUsuario.Tipo = string.Empty;
                          }
                        }
                        dtUserCliente = null;
                      }
                      else if (oRow["cod_perfil"].ToString() == "5")
                      {
                        cSysUserDeudor oSysUserDeudor = new cSysUserDeudor(ref oConn);
                        oSysUserDeudor.CodUser = dtUser.Rows[0]["cod_user"].ToString();
                        oSysUserDeudor.Rut = sRut;
                        oSysUserDeudor.Dv = sDv;
                        DataTable dtUserDeudor = oSysUserDeudor.GetByLogin();
                        if (dtUserDeudor != null)
                        {
                          if (dtUserDeudor.Rows.Count > 0)
                          {
                            bExito = true;
                            oUsuario.NKeyUsuario = dtUserDeudor.Rows[0]["nkey_deudor"].ToString();
                            oUsuario.NKeyDeudor = dtUserDeudor.Rows[0]["nkey_deudor"].ToString();
                            oUsuario.Licenciatario = dtUserDeudor.Rows[0]["nombre_deudor"].ToString();
                            oUsuario.VistaMenu = string.Empty;
                            oUsuario.Tipo = "1";
                            //Session["bLicenciatario"] = 1;
                          }
                        }
                        dtUserDeudor = null;
                      }
                    }
                  }
                  dtUserPerfil = null;
                }
                else {
                  sError = "Login o Password incorrecto.";
                }
              }
              dtUser = null;
            }
            oConn.Close();
          }
          if (bExito)
          {
            Session["USUARIO"] = oUsuario;

            DBConn oConn = new DBConn();
            if (oConn.Open())
            {
              cLogEventos oLogEventos = new cLogEventos(ref oConn);
              oLogEventos.AccionLog = "LOGIN";
              oLogEventos.CodCanal = "2";
              oLogEventos.CodFlujo = "1";
              oLogEventos.NomFlujo = "LOGIN";
              oLogEventos.NkeyDeudor = (!string.IsNullOrEmpty(oUsuario.NKeyDeudor)? oUsuario.NKeyDeudor: string.Empty);
              oLogEventos.RutDeudor = (!string.IsNullOrEmpty(oUsuario.RutLicenciatario) ? oUsuario.RutLicenciatario : string.Empty);
              oLogEventos.NomDeudor = (!string.IsNullOrEmpty(oUsuario.Licenciatario) ? oUsuario.Licenciatario : string.Empty);
              oLogEventos.CodUser = (!string.IsNullOrEmpty(oUsuario.CodUsuario) ? oUsuario.CodUsuario : string.Empty);
              oLogEventos.RutUser = (!string.IsNullOrEmpty(oUsuario.RutUsuario) ? oUsuario.RutUsuario : string.Empty);
              oLogEventos.NomUser = (!string.IsNullOrEmpty(oUsuario.Nombres) ? oUsuario.Nombres : string.Empty);
              oLogEventos.ObsLog = "El usuario se ha logeado con exito.";
              oLogEventos.IpLog = (!string.IsNullOrEmpty(oWeb.GetIpUsuario()) ? oWeb.GetIpUsuario() : string.Empty);
              oLogEventos.Accion = "CREAR";
              oLogEventos.Put();

              if (!string.IsNullOrEmpty(oLogEventos.Error)) {
                Response.Write("error 1:" + oLogEventos.Error);

                Response.Write("NKeyDeudor  " + oUsuario.NKeyDeudor + "<br>");
                Response.Write("RutLicenciatario  " + oUsuario.RutLicenciatario + "<br>");
                Response.Write("Licenciatario  " + oUsuario.Licenciatario + "<br>");
                Response.Write("CodUsuario  " + oUsuario.CodUsuario + "<br>");
                Response.Write("RutUsuario  " + oUsuario.RutUsuario + "<br>");
                Response.Write("Nombres  " + oUsuario.Nombres + "<br>");
                Response.Write("GetIpUsuario  " + oWeb.GetIpUsuario() + "<br>");

                Response.End();
              }

            }
            oConn.Close();

            Response.Redirect("licenciatarios.aspx", false);
          }
          else
          {
            DBConn oConn = new DBConn();
            if (oConn.Open())
            {

              sError = (!string.IsNullOrEmpty(sError) ? sError : "Usted no tiene los roles necesarios para acceder a este sistema, por favor contacte al administrador del sitio para que le otorgen el acceso.");

              cLogEventos oLogEventos = new cLogEventos(ref oConn);
              oLogEventos.AccionLog = "LOGIN";
              oLogEventos.CodCanal = "2";
              oLogEventos.CodFlujo = "1";
              oLogEventos.NomFlujo = "LOGIN";
              oLogEventos.NkeyDeudor = oUsuario.NKeyDeudor;
              oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
              oLogEventos.NomDeudor = oUsuario.Licenciatario;
              oLogEventos.CodUser = oUsuario.CodUsuario;
              oLogEventos.RutUser = oUsuario.RutUsuario;
              oLogEventos.NomUser = oUsuario.Nombres;
              oLogEventos.ObsLog = sError;
              oLogEventos.IpLog = oWeb.GetIpUsuario();
              oLogEventos.Accion = "CREAR";
              oLogEventos.Put();

              //if (!string.IsNullOrEmpty(oLogEventos.Error))
              //{
              //  Response.Write("error 2:" + oLogEventos.Error);
              //  Response.Write("NKeyDeudor  " + oUsuario.NKeyDeudor + "<br>");
              //  Response.Write("RutLicenciatario  " + oUsuario.RutLicenciatario + "<br>");
              //  Response.Write("Licenciatario  " + oUsuario.Licenciatario + "<br>");
              //  Response.Write("CodUsuario  " + oUsuario.CodUsuario + "<br>");
              //  Response.Write("RutUsuario  " + oUsuario.RutUsuario + "<br>");
              //  Response.Write("Nombres  " + oUsuario.Nombres + "<br>");
              //  Response.Write("GetIpUsuario  " + oWeb.GetIpUsuario() + "<br>");
              //  Response.End();
              //}
            }
            oConn.Close();


            StringBuilder js = new StringBuilder();
            js.Append("function LgRespuesta() {");
            js.Append(" window.radalert('" + sError + ".', 400, 200,'Atención'); ");
            js.Append(" Sys.Application.remove_load(LgRespuesta); ");
            js.Append("};");
            js.Append("Sys.Application.add_load(LgRespuesta);");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "LgRespuesta", js.ToString(), true);
          }
        }
        catch (Exception Ex) {
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            string sError = "El usuario " + sLogin + ", de la empresa " + sRutEmpresa + ", no se ha podido logear.";

            cLogEventos oLogEventos = new cLogEventos(ref oConn);
            oLogEventos.AccionLog = "LOGIN";
            oLogEventos.CodCanal = "2";
            oLogEventos.CodFlujo = "1";
            oLogEventos.NomFlujo = "LOGIN";
            oLogEventos.NkeyDeudor = oUsuario.NKeyDeudor;
            oLogEventos.RutDeudor = oUsuario.RutLicenciatario;
            oLogEventos.NomDeudor = oUsuario.Licenciatario;
            oLogEventos.NomUser = oUsuario.Nombres;
            oLogEventos.RutUser = oUsuario.RutUsuario;
            oLogEventos.ObsLog = sError;
            oLogEventos.ObsErrorLog = Ex.Message + " / " + Ex.Source;
            oLogEventos.IpLog = oWeb.GetIpUsuario();
            oLogEventos.Accion = "CREAR";
            oLogEventos.Put();

            //if (!string.IsNullOrEmpty(oLogEventos.Error))
            //{
            //  Response.Write("error 3:" + oLogEventos.Error);
            //  Response.Write("NKeyDeudor  " + oUsuario.NKeyDeudor + "<br>");
            //    Response.Write("RutLicenciatario  " + oUsuario.RutLicenciatario + "<br>");
            //    Response.Write("Licenciatario  " + oUsuario.Licenciatario + "<br>");
            //    Response.Write("CodUsuario  " + oUsuario.CodUsuario + "<br>");
            //    Response.Write("RutUsuario  " + oUsuario.RutUsuario + "<br>");
            //    Response.Write("Nombres  " + oUsuario.Nombres + "<br>");
            //    Response.Write("GetIpUsuario  " + oWeb.GetIpUsuario() + "<br>");
            //  Response.End();
            //}

            StringBuilder js = new StringBuilder();
            js.Append("function LgRespuesta() {");
            js.Append(" window.radalert('" + sError + ".', 400, 200,'Atención'); ");
            js.Append(" Sys.Application.remove_load(LgRespuesta); ");
            js.Append("};");
            js.Append("Sys.Application.add_load(LgRespuesta);");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "LgRespuesta", js.ToString(), true);


          }

        }
      }
    }
  }
}