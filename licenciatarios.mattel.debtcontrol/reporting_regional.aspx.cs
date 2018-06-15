using System;
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
  public partial class reporting_regional : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();

      if (!IsPostBack)
      {
        int sYear = 2015;
        int aYear = DateTime.Now.Year;
        while (sYear <= aYear)
        {
          cmbox_ano.Items.Add(new ListItem(sYear.ToString(), sYear.ToString()));
          sYear++;
        }
      }
    }

    protected void rdGridReporting_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open()) {
        cReportingRegional oReportingRegional = new cReportingRegional(ref oConn);
        oReportingRegional.CodTipo = oUsuario.CodTipoUsuario;
        rdGridReporting.DataSource = oReportingRegional.Get();
      }
      oConn.Close();
    }

    protected void rdGridReporting_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
      if (e.CommandName == "BajarReporting")
      {
        GridDataItem item = (GridDataItem)e.Item;
        string pCodReporting = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["cod_reporting"].ToString();

        Response.Redirect("downloadreporting.ashx?pCodReporting=" + pCodReporting + "&CodUsuario=" + oUsuario.NKeyUsuario);
      }
    }

    protected void rdGridReporting_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;

        item["fech_reporting"].Text = DateTime.Parse(row["fech_reporting"].ToString()).ToString("dd-MM-yyyy");
        item["est_reporting"].Text = (row["est_reporting"].ToString() == "S" ? "SOLICITADO" : "GENERADO");
      }
    }

    protected void btnGenerar_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cReportingRegional oReportingRegional = new cReportingRegional(ref oConn);
        oReportingRegional.NomReporting = "Reporte solicitado el " + DateTime.Now.ToString("dd-MM-yyyy");
        oReportingRegional.EstReporting = "S";
        oReportingRegional.FechReporting = DateTime.Now.ToString("yyyyMMdd");
        oReportingRegional.AnoReporting = cmbox_ano.SelectedValue;
        oReportingRegional.CodTipo = oUsuario.CodTipoUsuario;
        oReportingRegional.Accion = "CREAR";
        oReportingRegional.Put();

        StringBuilder js = new StringBuilder();
        js.Append("function LgRespuesta() {");
        js.Append(" window.radalert('Se realizo la solicitud de generación del Reporting Regional, espere uno 5 minutos y vuelva a ingresar a la funcionalidad para obtener el reporte.', 330, 210); ");
        js.Append(" Sys.Application.remove_load(LgRespuesta); ");
        js.Append("};");
        js.Append("Sys.Application.add_load(LgRespuesta);");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", js.ToString(), true);

        rdGridReporting.Rebind();
        
      }
      oConn.Close();
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