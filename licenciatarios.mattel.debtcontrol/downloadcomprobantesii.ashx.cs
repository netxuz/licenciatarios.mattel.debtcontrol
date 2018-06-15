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
  /// Summary description for downloadcomprobantesii
  /// </summary>
  public class downloadcomprobantesii : IHttpHandler
  {
    Web oWeb = new Web();
    public void ProcessRequest(HttpContext context)
    {
      string sPath = string.Empty;
      string sNoContrato = oWeb.GetData("NumContrato");
      string sFileName = string.Empty;
      string pCodComprobante = oWeb.GetData("pCodComprobante");
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cComprobanteImpuesto oComprobanteImpuesto = new cComprobanteImpuesto(ref oConn);
        oComprobanteImpuesto.CodComprobante = pCodComprobante;
        DataTable dtComprobante = oComprobanteImpuesto.Get();

        if (dtComprobante != null){
          if (dtComprobante.Rows.Count > 0){
            sFileName = dtComprobante.Rows[0]["nom_comprobante"].ToString();
          }
        }
        dtComprobante = null;
      }

      System.Web.HttpResponse oResponse = System.Web.HttpContext.Current.Response;

      //sPath = System.Web.HttpContext.Current.Server.MapPath("ComprobantesSII/") + sNoContrato + "/" + sFileName;
      sPath = System.Web.HttpContext.Current.Server.MapPath("rps_licenciatariosmattel/") + sFileName;
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