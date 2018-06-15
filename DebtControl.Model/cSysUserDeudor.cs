using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cSysUserDeudor
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodUser;
    public string CodUser { get { return pCodUser; } set { pCodUser = value; } }

    private string pNKeyDeudor;
    public string NKeyDeudor { get { return pNKeyDeudor; } set { pNKeyDeudor = value; } }

    private string pRut;
    public string Rut { get { return pRut; } set { pRut = value; } }

    private string pDv;
    public string Dv { get { return pDv; } set { pDv = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cSysUserDeudor()
    {

    }

    public cSysUserDeudor(ref DBConn oConn)
    {
      this.oConn = oConn;
    }

    public DataTable Get()
    {
      oParam = new DBConn.SQLParameters(2);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select cod_user, nkey_deudor from sys_user_deudor ");

        if (!string.IsNullOrEmpty(pCodUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_user = @cod_user");
          oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNKeyDeudor))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nkey_deudor = @nkey_deudor");
          oParam.AddParameters("@nkey_deudor", pNKeyDeudor, TypeSQL.Numeric);

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

    public DataTable GetByLogin()
    {
      oParam = new DBConn.SQLParameters(4);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select a.cod_user, a.nkey_deudor, (select snombre from deudor where nkey_deudor = a.nkey_deudor) nombre_deudor from sys_user_deudor a ");
        cSQL.Append(" where a.nkey_deudor in(select nkey_deudor from deudor where nRut = @nRut and sDigitoVerificador = @dv ) ");
        oParam.AddParameters("@nRut", pRut, TypeSQL.Numeric);
        oParam.AddParameters("@dv", pDv, TypeSQL.Varchar);

        if (!string.IsNullOrEmpty(pCodUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_user = @cod_user");
          oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNKeyDeudor))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nkey_deudor = @nkey_deudor");
          oParam.AddParameters("@nkey_deudor", pNKeyDeudor, TypeSQL.Numeric);

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
      oParam = new DBConn.SQLParameters(2);
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
              cSQL.Append("insert into sys_user_deudor(cod_user, nkey_deudor) values(");
              cSQL.Append("@cod_user, @nkey_deudor)");
              oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);
              oParam.AddParameters("@nkey_deudor", pNKeyDeudor, TypeSQL.Varchar);
              oConn.Insert(cSQL.ToString(), oParam);

              break;
            case "ELIMINAR":
              string Condicion = " where ";
              cSQL = new StringBuilder();
              cSQL.Append("delete from sys_user_deudor ");

              if (!string.IsNullOrEmpty(pCodUser))
              {
                cSQL.Append(Condicion);
                Condicion = " and ";
                cSQL.Append(" cod_user = @cod_user");
                oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);

              }

              if (!string.IsNullOrEmpty(pNKeyDeudor))
              {
                cSQL.Append(Condicion);
                Condicion = " and ";
                cSQL.Append(" nkey_deudor = @nkey_deudor");
                oParam.AddParameters("@nkey_deudor", pNKeyDeudor, TypeSQL.Numeric);

              }

              oConn.Delete(cSQL.ToString(), oParam);

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
