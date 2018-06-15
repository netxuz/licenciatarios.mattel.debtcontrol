using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cTipoRoyalty
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodRoyalty;
    public string CodRoyalty { get { return pCodRoyalty; } set { pCodRoyalty = value; } }

    private string pDescripcion;
    public string Descripcion { get { return pDescripcion; } set { pDescripcion = value; } }

    private string pCodMoneda;
    public string CodMoneda { get { return pCodMoneda; } set { pCodMoneda = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cTipoRoyalty()
    {

    }

    public cTipoRoyalty(ref DBConn oConn)
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
        cSQL.Append("select cod_royalty, descripcion, cod_moneda ");
        cSQL.Append("from lic_tipo_royalty ");

        if (!string.IsNullOrEmpty(pCodRoyalty))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_royalty = @cod_royalty");
          oParam.AddParameters("@cod_royalty", pCodRoyalty, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pCodMoneda))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_moneda = @cod_moneda");
          oParam.AddParameters("@cod_moneda", pCodMoneda, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pDescripcion))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" descripcion = '").Append(pDescripcion).Append("'");

        }

        cSQL.Append(" order by descripcion");

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
