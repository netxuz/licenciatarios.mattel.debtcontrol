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

namespace licenciatarios.mattel.debtcontrol
{
  public partial class WebSQL : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        StringBuilder sTextQuery = new StringBuilder();
        string sQuery = txt_query.Text;
        DataTable dtQuery = oConn.Select(sQuery);
        if (dtQuery != null)
        {
          if (dtQuery.Rows.Count > 0)
          {
            sTextQuery.Append("<table border=\"1\" style=\"color:#000\">");
            sTextQuery.Append("<tr>");
            foreach (DataColumn oColumn in dtQuery.Columns)
            {
              sTextQuery.Append("<td>");
              sTextQuery.Append(oColumn.ColumnName);
              sTextQuery.Append("</td>");
            }
            sTextQuery.Append("</tr>");

            foreach (DataRow oRow in dtQuery.Rows)
            {
              sTextQuery.Append("<tr>");
              foreach (DataColumn column in dtQuery.Columns)
              {
                sTextQuery.Append("<td>");
                sTextQuery.Append(oRow[column].ToString());
                sTextQuery.Append("</td>");
              }
              sTextQuery.Append("</tr>");
            }
            sTextQuery.Append("</table>");
            this.Controls.Add(new LiteralControl(sTextQuery.ToString()));
          }
        }
        else {
          this.Controls.Add(new LiteralControl("Done!"));
        }
        dtQuery = null;
        
      }
      oConn.Close();
    }
  }
}