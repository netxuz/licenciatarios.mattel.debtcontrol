using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DebtControl.Method;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class error_ingreso_venta : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        string sErrorType = oWeb.GetData("sTypeError");
        string sLine = oWeb.GetData("iLine");
        string sColumn = oWeb.GetData("iColumn");
        string sDataError = Session["Error"].ToString(); // oWeb.GetData("sDataError");
        string sMessageSys = oWeb.GetData("sMessageSys");

        if (sErrorType == "1"){
          lblerror0.Text = "Se ha producido un error en la carga, por datos que no concuerdan según el contrato.";
          lblerror1.Text = sDataError.Replace("[br]", "<br>"); 
        }
        else { 
          lblerror0.Text = "Se ha producido un error en la carga, el archivo tiene problemas en su configuración.";
          lblerror1.Text = sDataError.Replace("[br]", "<br>");
          //lblerror2.Text = "Error: " + sMessageSys;
        }
        Session["Error"] = string.Empty;
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
  }
}