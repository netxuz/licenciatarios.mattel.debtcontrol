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

namespace licenciatarios.mattel.debtcontrol.adm
{
  public partial class madmcontratos : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void rdContrato_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cContratos oContratos = new cContratos(ref oConn);
        if (!string.IsNullOrEmpty(txt_contrato.Text)) {
          oContratos.NoContrato = txt_contrato.Text;
        }
        rdContrato.DataSource = oContratos.GetWithDeudor();

      }
      oConn.Close();
    }

    protected void rdContrato_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
      switch (e.CommandName)
      {
        case "cmdEdit":
          string[] cParam = new string[2];
          cParam[0] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["num_contrato"].ToString();
          Response.Redirect(String.Format("mcontrato.aspx?numcontrato={0}", cParam));
          break;
      }
    }

    protected void rdContrato_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem oItem = e.Item as GridDataItem;

        if ((!string.IsNullOrEmpty(oItem["tipo_contrato"].Text)) && (oItem["tipo_contrato"].Text != "&nbsp;"))
        {
          if (oItem["tipo_contrato"].Text == "F")
            oItem["tipo_contrato"].Text = "Full";
          else if (oItem["tipo_contrato"].Text == "D")
            oItem["tipo_contrato"].Text = "Draft";
        }

        if ((!string.IsNullOrEmpty(oItem["aprobado"].Text)) && (oItem["aprobado"].Text != "&nbsp;"))
        {
          if (oItem["aprobado"].Text == "S")
            oItem["aprobado"].Text = "Aprobado";
          else if (oItem["aprobado"].Text == "N")
            oItem["aprobado"].Text = "No Aprobado";
          else if (oItem["aprobado"].Text == "T")
            oItem["aprobado"].Text = "Terminado";
        }
      }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      rdContrato.Rebind();
    }
  }
}