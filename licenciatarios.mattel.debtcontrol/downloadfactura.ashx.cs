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
  /// Summary description for downloadfactura
  /// </summary>
  public class downloadfactura : IHttpHandler
  {
    Web oWeb = new Web();
    public void ProcessRequest(HttpContext context)
    {
      string sPath = string.Empty;
      string sNomFactura = oWeb.GetData("sNomFactura");
      string sNumContrato = oWeb.GetData("sNumContrato");
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cContratos oContratos = new cContratos(ref oConn);
        oContratos.NumContrato = sNumContrato;
        DataTable dtContrato = oContratos.Get();
        if (dtContrato != null)
        {
          if (dtContrato.Rows.Count > 0)
          {
            cCliente oCliente = new cCliente(ref oConn);
            oCliente.NkeyCliente = dtContrato.Rows[0]["nkey_cliente"].ToString();
            DataTable dtCliente = oCliente.Get();
            if (dtCliente != null)
            {
              if (dtCliente.Rows.Count > 0)
              {
                sPath = dtCliente.Rows[0]["pathsdocscaneados"].ToString() + "\\" + dtCliente.Rows[0]["direcarchivos"].ToString();
              }
            }
            dtContrato = null;
          }
        }
      }
      oConn.Close();

      System.Web.HttpResponse oResponse = System.Web.HttpContext.Current.Response;

      sPath = sPath + "\\" + sNomFactura + ".pdf";
      oResponse.ContentType = "application/pdf";
      oResponse.AppendHeader("Content-Disposition", "attachment; filename=" + sNomFactura + ".pdf");

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