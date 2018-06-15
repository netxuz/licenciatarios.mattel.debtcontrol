using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;


namespace DebtControl.Model
{
  public class cDeudor
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pNKeyCliente;
    public string NKeyCliente { get { return pNKeyCliente; } set { pNKeyCliente = value; } }
    
    private string pNKeyDeudor;
    public string NKeyDeudor { get { return pNKeyDeudor; } set { pNKeyDeudor = value; } }

    private string pNRut;
    public string NRut { get { return pNRut; } set { pNRut = value; } }

    private string pSDigitoVerificador;
    public string SDigitoVerificador { get { return pSDigitoVerificador; } set { pSDigitoVerificador = value; } }

    private string pNCod;
    public string NCod { get { return pNCod; } set { pNCod = value; } }

    private string pSNombre;
    public string SNombre { get { return pSNombre; } set { pSNombre = value; } }

    private string pSNomFantasia;
    public string SNomFantasia { get { return pSNomFantasia; } set { pSNomFantasia = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cDeudor()
    {

    }

    public cDeudor(ref DBConn oConn)
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
        cSQL.Append("select nKey_Deudor, nRut, sDigitoVerificador, nCod, sNombre, snomfantasia ");
        cSQL.Append("from Deudor  ");

        if (!string.IsNullOrEmpty(pNKeyDeudor))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nKey_Deudor = @nKey_Deudor");
          oParam.AddParameters("@nKey_Deudor", pNKeyDeudor, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pSNombre))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" sNombre Like '%' + @snombre + '%'");
          oParam.AddParameters("@snombre", pSNombre, TypeSQL.Varchar);

        }

        cSQL.Append(" order by sNombre ");

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

    public DataTable GetByContrato()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select nKey_Deudor, nRut, sDigitoVerificador, nCod, sNombre, snomfantasia ");
        cSQL.Append("from Deudor where nKey_Deudor in (select nKey_Deudor from lic_contratos ) ");

        cSQL.Append(" order by sNombre ");

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

    public DataTable GetByReporte()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select nKey_Deudor, nRut, sDigitoVerificador, nCod, sNombre, snomfantasia ");
        cSQL.Append("from Deudor where nKey_Deudor in(select nKey_Deudor from lic_contratos ) ");

        if (!string.IsNullOrEmpty(pNKeyDeudor))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nKey_Deudor = @nKey_Deudor");
          oParam.AddParameters("@nKey_Deudor", pNKeyDeudor, TypeSQL.Numeric);

        }

        cSQL.Append(" order by sNombre ");

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

    public DataTable GetDatosDeudor()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select a.nKey_Deudor, a.nRut, a.sDigitoVerificador, a.nCod, a.sNombre, a.snomfantasia, b.semail ");
        cSQL.Append("from deudor a, contactosdeudor b ");
        cSQL.Append("where a.nKey_Deudor = b.nKey_Deudor and b.activo = 'S' ");

        if (!string.IsNullOrEmpty(pNKeyDeudor))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.nKey_Deudor = @nKey_Deudor ");
          oParam.AddParameters("@nKey_Deudor", pNKeyDeudor, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNKeyCliente))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" b.nkey_cliente = @nkey_cliente ");
          oParam.AddParameters("@nkey_cliente", pNKeyCliente, TypeSQL.Numeric);

        }

        cSQL.Append(" order by a.sNombre ");

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

  }
}
