using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cDirecciones
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pNkeyDeudor;
    public string NKeyDeudor { get { return pNkeyDeudor; } set { pNkeyDeudor = value; } }

    private string pNkeyCliente;
    public string NkeyCliente { get { return pNkeyCliente; } set { pNkeyCliente = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cDirecciones() { 
    }

    public cDirecciones(ref DBConn oConn)
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
        cSQL.Append("select * from direcciones ");

        if (!string.IsNullOrEmpty(pNkeyDeudor))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nkey_deudor = @nkey_deudor");
          oParam.AddParameters("@nkey_deudor", pNkeyDeudor, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNkeyCliente))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nkey_cliente = @nkey_cliente");
          oParam.AddParameters("@nkey_cliente", pNkeyCliente, TypeSQL.Numeric);

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
  }
}
