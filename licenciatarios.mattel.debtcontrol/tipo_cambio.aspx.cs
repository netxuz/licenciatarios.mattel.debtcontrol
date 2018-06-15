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
  public partial class tipo_cambio : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void rdGridTipoCambio_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cMonedas oMonedas = new cMonedas(ref oConn);
        oMonedas.CodMoneda = "1";
        DataTable dtMoneda = oMonedas.GetByCambio();
        if (dtMoneda != null)
          if (dtMoneda.Rows.Count > 0)
            rdGridTipoCambio.DataSource = dtMoneda;
        dtMoneda = null;
        oConn.Close();
      }
    }

    protected void rdGridTipoCambio_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;
        item["mes"].Text = oWeb.getMes(int.Parse(row["mes"].ToString())).ToUpper();
      }
    }

  }
}