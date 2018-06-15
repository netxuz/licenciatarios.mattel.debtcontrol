using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using DebtControl.Method;

namespace licenciatarios.mattel.debtcontrol.adm
{
  /// <summary>
  /// Summary description for downloadfile
  /// </summary>
  public class downloadfile : IHttpHandler
  {
    Web oWeb = new Web();
    public void ProcessRequest(HttpContext context)
    {
      string sPath = string.Empty;
      string sNomArchivo = oWeb.GetData("sNomArchivo");
      string sNumContrato = oWeb.GetData("sNumContrato");

      System.Web.HttpResponse oResponse = System.Web.HttpContext.Current.Response;

      sPath = HttpContext.Current.Server.MapPath("..\\rps_licenciatariosmattel") + "\\" + sNomArchivo;
      oResponse.ContentType = "application/octet-stream";
      oResponse.AppendHeader("Content-Disposition", "attachment; filename=" + sNomArchivo);

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