using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using ClosedXML.Excel;
using DebtControl.Conn;
using DebtControl.Model;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class WebForm1 : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cLogEventos oLogEventos = new cLogEventos(ref oConn);
        DataTable dt = oLogEventos.Get();

        XLWorkbook wb = new XLWorkbook();
        wb.Worksheets.Add(dt, "LogEventos");
        oConn.Close();

        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xlsx");
        using (MemoryStream MyMemoryStream = new MemoryStream())
        {
          wb.SaveAs(MyMemoryStream);
          MyMemoryStream.WriteTo(Response.OutputStream);
          Response.Flush();
          Response.End();
        }
      }
    }
  }
}