using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;

namespace licenciatarios.mattel.debtcontrol
{
  /// <summary>
  /// Summary description for downloadmanual
  /// </summary>
  public class downloadmanual : IHttpHandler
  {

    public void ProcessRequest(HttpContext context)
    {
      HttpResponse oResponse = HttpContext.Current.Response;
      HttpServerUtility oServer = HttpContext.Current.Server;

      string sPath = oServer.MapPath("/Resources") + "\\Mattel Manual Licenciatarios.pdf";
      oResponse.ContentType = "application/pdf";
      oResponse.AppendHeader("Content-Disposition", "attachment; filename=Mattel Manual Licenciatarios.pdf");

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