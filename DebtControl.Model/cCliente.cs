using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cCliente
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pNkeyCliente;
    public string NkeyCliente { get { return pNkeyCliente; } set { pNkeyCliente = value; } }

    private string pNRut;
    public string NRut { get { return pNRut; } set { pNRut = value; } }

    private string pSDigitoVerificador;
    public string SDigitoVerificador { get { return pSDigitoVerificador; } set { pSDigitoVerificador = value; } }

    private string pNCod;
    public string NCod { get { return pNCod; } set { pNCod = value; } }

    private string pSNombre;
    public string SNombre { get { return pSNombre; } set { pSNombre = value; } }

    private string pPathsDocScaneados;
    public string PathsDocScaneados { get { return pPathsDocScaneados; } set { pPathsDocScaneados = value; } }

    private string pDirecArchivos;
    public string DirecArchivos { get { return pDirecArchivos; } set { pDirecArchivos = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cCliente()
    {

    }

    public cCliente(ref DBConn oConn)
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
        cSQL.Append("select nKey_cliente, nRut, sDigitoVerificador, nCod, sNombre, pathsdocscaneados, direcarchivos ");
        cSQL.Append("from Cliente  ");

        if (!string.IsNullOrEmpty(pNkeyCliente))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nKey_cliente = @nKey_cliente");
          oParam.AddParameters("@nKey_cliente", pNkeyCliente, TypeSQL.Numeric);

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
  }
}
