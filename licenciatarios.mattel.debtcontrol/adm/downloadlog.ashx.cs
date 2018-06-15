using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;
using Telerik.Web.UI;
using ClosedXML.Excel;

namespace licenciatarios.mattel.debtcontrol.adm
{
  /// <summary>
  /// Summary description for downloadlog
  /// </summary>
  public class downloadlog : IHttpHandler
  {
    Web oWeb = new Web();
    public void ProcessRequest(HttpContext context)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cLogEventos oLogEventos = new cLogEventos(ref oConn);
        oLogEventos.NkeyDeudor = oWeb.GetData("sNkeyDeudor");
        oLogEventos.NoContrato = oWeb.GetData("sNoContrato");
        oLogEventos.CodFlujo = oWeb.GetData("sCodFlujo");

        if ((!string.IsNullOrEmpty(oWeb.GetData("sRadDatePicker1"))) && (!string.IsNullOrEmpty(oWeb.GetData("sRadDatePicker2"))))
        {
          oLogEventos.FchDesdeLog = oWeb.GetData("sRadDatePicker1");
          oLogEventos.FchHastaLog = oWeb.GetData("sRadDatePicker2");
        }
        DataTable dt = oLogEventos.Get();

        XLWorkbook wb = new XLWorkbook();
        wb.Worksheets.Add(dt, "LogEventos");
        oConn.Close();

        context.Response.Clear();
        context.Response.Buffer = true;
        context.Response.Charset = "";
        context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        context.Response.AddHeader("content-disposition", "attachment;filename=LogEventos.xlsx");
        using (MemoryStream MyMemoryStream = new MemoryStream())
        {
          wb.SaveAs(MyMemoryStream);
          MyMemoryStream.WriteTo(context.Response.OutputStream);
          context.Response.Flush();
          context.Response.End();
        }
      }
    }

    public bool IsReusable
    {
      get
      {
        return false;
      }
    }
  }
}