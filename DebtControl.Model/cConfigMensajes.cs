using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cConfigMensajes
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodConfigMsn;
    public string CodConfigMsn { get { return pCodConfigMsn; } set { pCodConfigMsn = value; } }

    private string pDescripcionConfigMsn;
    public string DescripcionConfigMsn { get { return pDescripcionConfigMsn; } set { pDescripcionConfigMsn = value; } }

    private string pTipoEmail;
    public string TipoEmail { get { return pTipoEmail; } set { pTipoEmail = value; } }

    private string pDiaConfigMsn;
    public string DiaConfigMsn { get { return pDiaConfigMsn; } set { pDiaConfigMsn = value; } }

    private string pCantDiasConfigMsn;
    public string CantDiasConfigMsn { get { return pCantDiasConfigMsn; } set { pCantDiasConfigMsn = value; } }

    private string pEstConfigMsn;
    public string EstConfigMsn { get { return pEstConfigMsn; } set { pEstConfigMsn = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cConfigMensajes() { 

    }

    public cConfigMensajes(ref DBConn oConn)
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
        cSQL.Append("select cod_config_msn, descripcion_config_msn, tipo_email, dia_config_msn, cant_dias_config_msn, est_config_msn ");
        cSQL.Append("from lic_config_mensajes ");

        if (!string.IsNullOrEmpty(pCodConfigMsn))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_config_msn = @cod_config_msn ");
          oParam.AddParameters("@cod_config_msn", pCodConfigMsn, TypeSQL.Numeric);
        }

        if (!string.IsNullOrEmpty(pTipoEmail))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" tipo_email = @tipo_email ");
          oParam.AddParameters("@tipo_email", pTipoEmail, TypeSQL.Char);
        }

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
      oParam = new DBConn.SQLParameters(10);
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

              pCodConfigMsn = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
              cSQL.Append("insert into lic_config_mensajes(cod_config_msn, descripcion_config_msn, tipo_email, dia_config_msn, cant_dias_config_msn, est_config_msn) values(");
              cSQL.Append("@cod_config_msn, @descripcion_config_msn, @tipo_email, @dia_config_msn, @cant_dias_config_msn, @est_config_msn)");
              oParam.AddParameters("@cod_config_msn", pCodConfigMsn, TypeSQL.Numeric);
              oParam.AddParameters("@descripcion_config_msn", pDescripcionConfigMsn, TypeSQL.Varchar);
              oParam.AddParameters("@tipo_email", pTipoEmail, TypeSQL.Char);
              oParam.AddParameters("@dia_config_msn", pDiaConfigMsn, TypeSQL.Int);
              oParam.AddParameters("@cant_dias_config_msn", pCantDiasConfigMsn, TypeSQL.Int);
              oParam.AddParameters("@est_config_msn", pEstConfigMsn, TypeSQL.Char);
              oConn.Insert(cSQL.ToString(), oParam);

              break;
            case "EDITAR":
              cSQL = new StringBuilder();
              cSQL.Append("update lic_config_mensajes set ");
              if (!string.IsNullOrEmpty(pDiaConfigMsn))
              {
                cSQL.Append(sComa);
                cSQL.Append(" dia_config_msn = @dia_config_msn");
                oParam.AddParameters("@dia_config_msn", pDiaConfigMsn, TypeSQL.Int);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pCantDiasConfigMsn))
              {
                cSQL.Append(sComa);
                cSQL.Append(" cant_dias_config_msn = @cant_dias_config_msn");
                oParam.AddParameters("@cant_dias_config_msn", pCantDiasConfigMsn, TypeSQL.Int);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pEstConfigMsn))
              {
                cSQL.Append(sComa);
                cSQL.Append(" est_config_msn = @est_config_msn");
                oParam.AddParameters("@est_config_msn", pEstConfigMsn, TypeSQL.Char);
                sComa = ", ";
              }

              cSQL.Append(" where tipo_email = @tipo_email ");
              oParam.AddParameters("@tipo_email", pTipoEmail, TypeSQL.Char);
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
