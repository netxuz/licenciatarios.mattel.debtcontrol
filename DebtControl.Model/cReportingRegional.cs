using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cReportingRegional
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodReporting;
    public string CodReporting { get { return pCodReporting; } set { pCodReporting = value; } }

    private string pNomReporting;
    public string NomReporting { get { return pNomReporting; } set { pNomReporting = value; } }

    private string pFechReporting;
    public string FechReporting { get { return pFechReporting; } set { pFechReporting = value; } }

    private string pEstReporting;
    public string EstReporting { get { return pEstReporting; } set { pEstReporting = value; } }

    private string pAnoReporting;
    public string AnoReporting { get { return pAnoReporting; } set { pAnoReporting = value; } }

    private string pCodTipo;
    public string CodTipo { get { return pCodTipo; } set { pCodTipo = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cReportingRegional()
    {

    }

    public cReportingRegional(ref DBConn oConn)
    {
      this.oConn = oConn;
    }

    public DataTable Get()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select cod_reporting, nom_reporting, fech_reporting, est_reporting, filename_reporting, ano_reporting, cod_tipo ");
        cSQL.Append("from lic_reporting_regional ");

        if (!string.IsNullOrEmpty(pCodReporting))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_reporting = @cod_reporting");
          oParam.AddParameters("@cod_reporting", pCodReporting, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pEstReporting))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" est_reporting = @est_reporting");
          oParam.AddParameters("@est_reporting", pEstReporting, TypeSQL.Char);

        }

        if (!string.IsNullOrEmpty(pCodTipo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_tipo = @cod_tipo");
          oParam.AddParameters("@cod_tipo", pCodTipo, TypeSQL.Int);

        }

        cSQL.Append(" order by fech_reporting desc ");

        dtData = oConn.Select(cSQL.ToString(), oParam);
        pError = oConn.Error;
        return dtData;
      }
      else
      {
        pError = "Conexion Cerrada";
        return null;
      }

    }

    public void Put()
    {
      DataTable dtData;
      oParam = new DBConn.SQLParameters(20);
      StringBuilder cSQL;
      string sComa = string.Empty;

      if (oConn.bIsOpen)
      {
        try
        {
          switch (pAccion)
          {
            case "CREAR":
              cSQL = new StringBuilder();
              cSQL.Append("insert into lic_reporting_regional(nom_reporting, fech_reporting, est_reporting, ano_reporting, cod_tipo) values(");
              cSQL.Append("@nom_reporting, @fech_reporting, @est_reporting, @ano_reporting, @cod_tipo) ");
              oParam.AddParameters("@nom_reporting", pNomReporting, TypeSQL.Varchar);
              oParam.AddParameters("@fech_reporting", pFechReporting, TypeSQL.Varchar);
              oParam.AddParameters("@est_reporting", pEstReporting, TypeSQL.Char);
              oParam.AddParameters("@ano_reporting", pAnoReporting, TypeSQL.Int);
              oParam.AddParameters("@cod_tipo", pCodTipo, TypeSQL.Int);
              oConn.Insert(cSQL.ToString(), oParam);

              cSQL = new StringBuilder();
              cSQL.Append("select @@IDENTITY");
              dtData = oConn.Select(cSQL.ToString());
              if (dtData != null)
                if (dtData.Rows.Count > 0)
                  pCodReporting = dtData.Rows[0][0].ToString();
              dtData = null;

              break;
            case "EDITAR":
              cSQL = new StringBuilder();
              cSQL.Append("update lic_reporting_regional set ");

              if (!string.IsNullOrEmpty(pNomReporting))
              {
                cSQL.Append(sComa);
                cSQL.Append(" nom_reporting = @nom_reporting ");
                oParam.AddParameters("@nom_reporting", pNomReporting, TypeSQL.Varchar);
                sComa = ", ";
              }

              if (!string.IsNullOrEmpty(pEstReporting))
              {
                cSQL.Append(sComa);
                cSQL.Append(" est_reporting = @est_reporting ");
                oParam.AddParameters("@est_reporting", pEstReporting, TypeSQL.Char);
                sComa = ", ";
              }

              if (!string.IsNullOrEmpty(pCodTipo))
              {
                cSQL.Append(sComa);
                cSQL.Append(" cod_tipo = @cod_tipo ");
                oParam.AddParameters("@cod_tipo", pCodTipo, TypeSQL.Int);
                sComa = ", ";
              }

              cSQL.Append(" where cod_reporting = @cod_reporting ");
              oParam.AddParameters("@cod_reporting", pCodReporting, TypeSQL.Numeric);
              oConn.Update(cSQL.ToString(), oParam);

              break;
            case "ELIMINAR":
              break;
          }
        }
        catch (Exception Ex)
        {
          pError = Ex.Message;
        }
      }
    }

  }
}
