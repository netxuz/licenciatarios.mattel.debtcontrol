using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cSysUserCliente
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodUser;
    public string CodUser { get { return pCodUser; } set { pCodUser = value; } }

    private string pNKeyCliente;
    public string NKeyCliente { get { return pNKeyCliente; } set { pNKeyCliente = value; } }

    private string pTipoCliente;
    public string TipoCliente { get { return pTipoCliente; } set { pTipoCliente = value; } }

    private string pRut;
    public string Rut { get { return pRut; } set { pRut = value; } }

    private string pDv;
    public string Dv { get { return pDv; } set { pDv = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cSysUserCliente()
    {

    }

    public cSysUserCliente(ref DBConn oConn)
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
        cSQL.Append("select cod_user, nkey_cliente, tipo_cliente from sys_user_cliente ");

        if (!string.IsNullOrEmpty(pCodUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_user = @cod_user");
          oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNKeyCliente))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nkey_cliente = @nkey_cliente");
          oParam.AddParameters("@nkey_cliente", pNKeyCliente, TypeSQL.Numeric);

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
        cSQL.Append("select cod_user, nkey_cliente, tipo_cliente from sys_user_cliente ");
        cSQL.Append(" where nkey_cliente in(select nkey_cliente from cliente where nRut = @nRut and sDigitoVerificador = @dv ) ");
        oParam.AddParameters("@nRut", pRut, TypeSQL.Numeric);
        oParam.AddParameters("@dv", pDv, TypeSQL.Varchar);

        if (!string.IsNullOrEmpty(pCodUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_user = @cod_user");
          oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNKeyCliente))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nkey_cliente = @nkey_cliente");
          oParam.AddParameters("@nkey_cliente", pNKeyCliente, TypeSQL.Numeric);

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
      oParam = new DBConn.SQLParameters(3);
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
              cSQL.Append("insert into sys_user_cliente(cod_user, nkey_cliente, tipo_cliente) values(");
              cSQL.Append("@cod_user, @nkey_cliente, @tipo_cliente)");
              oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);
              oParam.AddParameters("@nkey_cliente", pNKeyCliente, TypeSQL.Varchar);
              oParam.AddParameters("@tipo_cliente", pTipoCliente, TypeSQL.Char);
              oConn.Insert(cSQL.ToString(), oParam);

              break;
            case "ELIMINAR":
              string Condicion = " where ";
              cSQL = new StringBuilder();
              cSQL.Append("delete from sys_user_cliente ");

              if (!string.IsNullOrEmpty(pCodUser))
              {
                cSQL.Append(Condicion);
                Condicion = " and ";
                cSQL.Append(" cod_user = @cod_user");
                oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);

              }

              if (!string.IsNullOrEmpty(pNKeyCliente))
              {
                cSQL.Append(Condicion);
                Condicion = " and ";
                cSQL.Append(" nkey_cliente = @nkey_cliente");
                oParam.AddParameters("@nkey_cliente", pNKeyCliente, TypeSQL.Numeric);

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
