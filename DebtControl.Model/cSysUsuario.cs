using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cSysUsuario
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodUser;
    public string CodUser { get { return pCodUser; } set { pCodUser = value; } }

    private string pNomUser;
    public string NomUser { get { return pNomUser; } set { pNomUser = value; } }

    private string pApeUser;
    public string ApeUser { get { return pApeUser; } set { pApeUser = value; } }

    private string pEmlUser;
    public string EmlUser { get { return pEmlUser; } set { pEmlUser = value; } }

    private string pLoginUser;
    public string LoginUser { get { return pLoginUser; } set { pLoginUser = value; } }

    private string pPwdUser;
    public string PwdUser { get { return pPwdUser; } set { pPwdUser = value; } }

    private string pEstUser;
    public string EstUser { get { return pEstUser; } set { pEstUser = value; } }

    private string pTipoUser;
    public string TipoUser { get { return pTipoUser; } set { pTipoUser = value; } }

    private string pCodTipoUsuario;
    public string CodTipoUsuario { get { return pCodTipoUsuario; } set { pCodTipoUsuario = value; } }

    private string pNkeyUser;
    public string NkeyUser { get { return pNkeyUser; } set { pNkeyUser = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cSysUsuario()
    {

    }

    public cSysUsuario(ref DBConn oConn)
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
        cSQL.Append("select cod_user, nom_user, ape_user, eml_user, login_user, pwd_user, est_user, tipo_user, nkey_user, cod_tipo ");
        cSQL.Append("from sys_usuario ");

        if (!string.IsNullOrEmpty(pCodUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_user = @cod_user");
          oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNomUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nom_user like '%").Append(pNomUser).Append("%'");
        }

        if (!string.IsNullOrEmpty(pLoginUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" login_user = @login_user");
          oParam.AddParameters("@login_user", pLoginUser, TypeSQL.Varchar);

        }

        if (!string.IsNullOrEmpty(pPwdUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" pwd_user = @pwd_user");
          oParam.AddParameters("@pwd_user", pPwdUser, TypeSQL.Varchar);

        }

        if (!string.IsNullOrEmpty(pEstUser))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" est_user = @est_user");
          oParam.AddParameters("@est_user", pEstUser, TypeSQL.Varchar);

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
              pCodUser = getCodeUsuario().ToString();
              cSQL = new StringBuilder();
              cSQL.Append("insert into sys_usuario(cod_user, nom_user, ape_user, eml_user, login_user, pwd_user, est_user, tipo_user, nkey_user, cod_tipo) values(");
              cSQL.Append("@cod_user, @nom_user, @ape_user, @eml_user, @login_user, @pwd_user, @est_user, @tipo_user, @nkey_user, @cod_tipo)");
              oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);
              oParam.AddParameters("@nom_user", pNomUser, TypeSQL.Varchar);
              oParam.AddParameters("@ape_user", pApeUser, TypeSQL.Varchar);
              oParam.AddParameters("@eml_user", pEmlUser, TypeSQL.Varchar);
              oParam.AddParameters("@login_user", pLoginUser, TypeSQL.Varchar);
              oParam.AddParameters("@pwd_user", pPwdUser, TypeSQL.Varchar);
              oParam.AddParameters("@est_user", pEstUser, TypeSQL.Char);
              oParam.AddParameters("@tipo_user", pTipoUser, TypeSQL.Varchar);
              oParam.AddParameters("@nkey_user", pNkeyUser, TypeSQL.Char);
              oParam.AddParameters("@cod_tipo", pCodTipoUsuario, TypeSQL.Numeric);
              oConn.Insert(cSQL.ToString(), oParam);

              break;
            case "EDITAR":
              cSQL = new StringBuilder();
              cSQL.Append("update sys_usuario set ");

              if (!string.IsNullOrEmpty(pNomUser))
              {
                cSQL.Append(sComa);
                cSQL.Append(" nom_user = @nom_user");
                oParam.AddParameters("@nom_user", pNomUser, TypeSQL.Varchar);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pApeUser))
              {
                cSQL.Append(sComa);
                cSQL.Append(" ape_user = @ape_user");
                oParam.AddParameters("@ape_user", pApeUser, TypeSQL.Varchar);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pEmlUser))
              {
                cSQL.Append(sComa);
                cSQL.Append(" eml_user = @eml_user");
                oParam.AddParameters("@eml_user", pEmlUser, TypeSQL.Varchar);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pLoginUser))
              {
                cSQL.Append(sComa);
                cSQL.Append(" login_user = @login_user");
                oParam.AddParameters("@login_user", pLoginUser, TypeSQL.Varchar);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pPwdUser))
              {
                cSQL.Append(sComa);
                cSQL.Append(" pwd_user = @pwd_user");
                oParam.AddParameters("@pwd_user", pPwdUser, TypeSQL.Varchar);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pEstUser))
              {
                cSQL.Append(sComa);
                cSQL.Append(" est_user = @est_user");
                oParam.AddParameters("@est_user", pEstUser, TypeSQL.Char);
              }
              if (!string.IsNullOrEmpty(pTipoUser))
              {
                cSQL.Append(sComa);
                cSQL.Append(" tipo_user = @tipo_user");
                oParam.AddParameters("@tipo_user", pTipoUser, TypeSQL.Numeric);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pNkeyUser))
              {
                cSQL.Append(sComa);
                cSQL.Append(" nkey_user = @nkey_user ");
                oParam.AddParameters("@nkey_user", pNkeyUser, TypeSQL.Numeric);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pCodTipoUsuario))
              {
                cSQL.Append(sComa);
                cSQL.Append(" cod_tipo = @cod_tipo ");
                oParam.AddParameters("@cod_tipo", pCodTipoUsuario, TypeSQL.Numeric);
                sComa = ", ";
              }

              cSQL.Append(" where cod_user = @cod_user ");
              oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);
              oConn.Update(cSQL.ToString(), oParam);

              break;
            case "ELIMINAR":
              cSQL = new StringBuilder();
              cSQL.Append("delete from sys_usuario where cod_user = @cod_user");
              oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);
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

    int getCodeUsuario()
    {
      DataTable dtData;
      string cod_usuario = string.Empty;
      StringBuilder cSQL;

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select Max(cod_user) from sys_usuario");
        dtData = oConn.Select(cSQL.ToString());
        pError = oConn.Error;
        return int.Parse(dtData.Rows[0][0].ToString()) + 1;
      }
      else
      {
        pError = "Conexion Cerrada";
        return 0;
      }
    }
  }
}