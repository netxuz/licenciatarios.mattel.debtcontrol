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


namespace licenciatarios.mattel.debtcontrol.adm
{
  public partial class _default : System.Web.UI.Page
  {
    Usuario oUsuario;
    Web oWeb = new Web();

    protected void Page_Load(object sender, EventArgs e)
    {
      oUsuario = oWeb.GetObjAdmUsuario();
    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
      bool bExito = false;
      Web oWeb = new Web();
      string sLogin = txt_rut.Text;
      string sPwd = oWeb.Crypt(txt_password.Text);

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
            bExito = true;
            oUsuario = oWeb.GetObjUsuario();
            oUsuario.CodUsuario = dtUser.Rows[0]["cod_user"].ToString();
            oUsuario.Nombres = (dtUser.Rows[0]["nom_user"].ToString() + " " + dtUser.Rows[0]["ape_user"].ToString()).Trim();
            oUsuario.Email = dtUser.Rows[0]["eml_user"].ToString();
            oUsuario.CodTipoUsuario = dtUser.Rows[0]["cod_tipo"].ToString();
            oUsuario.RutUsuario = sLogin;
          }
        }
        dtUser = null;
      }
      oConn.Close();
      if (bExito)
      {
        Session["Administrador"] = oUsuario;
        Response.Redirect("management.aspx");
      }
      else
      {
        StringBuilder js = new StringBuilder();
        js.Append("function LgRespuesta() {");
        js.Append(" window.radalert('Login o Password incorrecto.', 200, 100,'Atención'); ");
        js.Append(" Sys.Application.remove_load(LgRespuesta); ");
        js.Append("};");
        js.Append("Sys.Application.add_load(LgRespuesta);");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LgRespuesta", js.ToString(), true);
      }

    }
  }
}