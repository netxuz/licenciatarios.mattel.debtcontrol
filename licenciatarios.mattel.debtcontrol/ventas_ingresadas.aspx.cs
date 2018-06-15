using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DebtControl.Method;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class ventas_ingresadas : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
      lblperiodo.Text = oWeb.getMes(int.Parse(oWeb.GetData("MesReporte"))).ToUpper() + " / " + oWeb.GetData("AnoReporte") + ", del contrato " + oWeb.GetData("NoContrato");
    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {

    }

    protected void btnContacto_Click(object sender, EventArgs e)
    {

    }
  }
}