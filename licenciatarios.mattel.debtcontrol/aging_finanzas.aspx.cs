using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;
using Telerik.Web.UI;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class aging_finanzas : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
    }

    protected void rdGridAging_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        DBConn oConn = new DBConn();
        if (oConn.Open()) {
          cAntiguedadDeuda oAntiguedadDeuda = new cAntiguedadDeuda(ref oConn);
          oAntiguedadDeuda.NKeyCliente = oUsuario.NKeyUsuario;
          rdGridAging.DataSource = oAntiguedadDeuda.GetFinanzas();
        }
        oConn.Close();
    }

    protected void rdGridAging_ItemCommand(object source, GridCommandEventArgs e)
    {
      if (e.CommandName == RadGrid.ExportToExcelCommandName)
      {
        rdGridAging.ExportSettings.ExportOnlyData = true;
        rdGridAging.ExportSettings.IgnorePaging = true;
        rdGridAging.ExportSettings.OpenInNewWindow = true;
        rdGridAging.ExportSettings.FileName = "reporte_aging_" + DateTime.Now.ToString("yyyyMMdd");
        rdGridAging.MasterTableView.ExportToExcel();
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