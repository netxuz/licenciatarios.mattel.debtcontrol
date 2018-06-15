using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.IO;

namespace licenciatarios.mattel.debtcontrol
{
  /// <summary>
  /// Summary description for download
  /// </summary>
  public class download : IHttpHandler
  {

    public void ProcessRequest(HttpContext context)
    {
      System.Web.HttpResponse oResponse = System.Web.HttpContext.Current.Response;
      string sPath = string.Empty;
      sPath = sPath + "\\\\srvdebt03\\Comun\\Mattel Europa\\Base.bak";
      oResponse.ContentType = "application/pdf";
      oResponse.AppendHeader("Content-Disposition", "attachment; filename=Base.bak");

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