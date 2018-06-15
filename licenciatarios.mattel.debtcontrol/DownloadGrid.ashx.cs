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

namespace licenciatarios.mattel.debtcontrol
{
  /// <summary>
  /// Summary description for DownloadGrid
  /// </summary>
  public class DownloadGrid : IHttpHandler
  {

    public void ProcessRequest(HttpContext context)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        Web oWeb = new Web();

        //cProductosContrato oProductosContrato = new cProductosContrato(ref oConn);
        //oProductosContrato.NumContrato = oWeb.GetData("pCodContrato");
        //DataTable dtProdCont = oProductosContrato.GetByExcel();

        cRoyaltyContrato oRoyaltyContrato = new cRoyaltyContrato(ref oConn);
        oRoyaltyContrato.NumContrato =  oWeb.GetData("pCodContrato");
        DataTable dtProdCont = oRoyaltyContrato.GetByExcel();
        oConn.Close();

        System.Web.HttpResponse oResponse = System.Web.HttpContext.Current.Response;

        oResponse.Clear();
        oResponse.AddHeader("content-disposition", "attachment;filename=FileName.csv");
        oResponse.Charset = "";
        oResponse.Cache.SetCacheability(HttpCacheability.NoCache);
        oResponse.ContentType = "application/vnd.ms-excel";

        oResponse.Output.Write(ToCSV(dtProdCont));
        oResponse.Flush();
        oResponse.End();
      }
    }

    public bool IsReusable
    {
      get
      {
        return false;
      }
    }

    public string ToCSV(DataTable dtDataTable)
    {
      System.IO.StringWriter sw = new System.IO.StringWriter();
      for (int i = 0; i < dtDataTable.Columns.Count; i++)
      {
        sw.Write(dtDataTable.Columns[i]);
        if (i < dtDataTable.Columns.Count - 1)
        {
          sw.Write(";");
        }
      }
      sw.Write(sw.NewLine);
      foreach (DataRow dr in dtDataTable.Rows)
      {
        for (int i = 0; i < dtDataTable.Columns.Count; i++)
        {
          if (!Convert.IsDBNull(dr[i]))
          {
            string value = dr[i].ToString();
            if (value.Contains(';'))
            {
              value = String.Format("\"{0}\"", value);
              sw.Write(value);
            }
            else
            {
              sw.Write(dr[i].ToString());
            }
          }
          if (i < dtDataTable.Columns.Count - 1)
          {
            sw.Write(";");
          }
        }
        sw.Write(sw.NewLine);
      }
      return sw.ToString();
    }

  }
}