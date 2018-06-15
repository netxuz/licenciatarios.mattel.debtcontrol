using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.IO;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;

namespace licenciatarios.mattel.debtcontrol
{
  /// <summary>
  /// Summary description for downloadreporting
  /// </summary>
  public class downloadreporting : IHttpHandler
  {
    Web oWeb = new Web();
    public void ProcessRequest(HttpContext context)
    {
      string sPath = string.Empty;
      string sFileName = string.Empty;
      string sCodUsuario = oWeb.GetData("CodUsuario");
      string pCodReporting = oWeb.GetData("pCodReporting");

      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cReportingRegional oReportingRegional = new cReportingRegional(ref oConn);
        oReportingRegional.CodReporting = pCodReporting;
        DataTable dtReportingRegional = oReportingRegional.Get();
        if (dtReportingRegional != null)
        {
          if (dtReportingRegional.Rows.Count > 0)
          {
            sFileName = dtReportingRegional.Rows[0]["filename_reporting"].ToString();

            cCliente oCliente = new cCliente(ref oConn);
            oCliente.NkeyCliente = sCodUsuario;
            DataTable dtCliente = oCliente.Get();
            if (dtCliente != null)
            {
              if (dtCliente.Rows.Count > 0)
              {
                sPath = dtCliente.Rows[0]["pathsdocscaneados"].ToString();
              }
            }
            dtReportingRegional = null;
          }
        }
      }
      oConn.Close();

      System.Web.HttpResponse oResponse = System.Web.HttpContext.Current.Response;

      sPath = sPath + "\\Mattel Europa\\Reporte_Regional\\" + sFileName;
      oResponse.AppendHeader("Content-Disposition", "attachment; filename=" + sFileName);

      // Write the file to the Response
      const int bufferLength = 10000;
      byte[] buffer = new Byte[bufferLength];
      int length = 0;
      Stream download = null;
      try
      {
        download = new FileStream(sPath, FileMode.Open, FileAccess.Read);
        do
        {
          if (oResponse.IsClientConnected)
          {
            length = download.Read(buffer, 0, bufferLength);
            oResponse.OutputStream.Write(buffer, 0, length);
            buffer = new Byte[bufferLength];
          }
          else
          {
            length = -1;
          }
        }
        while (length > 0);
        oResponse.Flush();
        oResponse.End();
      }
      finally
      {
        if (download != null)
          download.Close();
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