using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cMarcas
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodMarca;
    public string CodMarca { get { return pCodMarca; } set { pCodMarca = value; } }

    private string pCodContrato;
    public string CodContrato { get { return pCodContrato; } set { pCodContrato = value; } }

    private string pDescripcion;
    public string Descripcion { get { return pDescripcion; } set { pDescripcion = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cMarcas()
    {

    }

    public cMarcas(ref DBConn oConn)
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
        cSQL.Append("select cod_marca, descripcion ");
        cSQL.Append("from lic_marcas ");

        if (!string.IsNullOrEmpty(pCodMarca))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_marca = @cod_marca");
          oParam.AddParameters("@cod_marca", pCodMarca, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pDescripcion))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" descripcion like '%").Append(pDescripcion).Append("%'");

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

    public DataTable GetByDescripcion()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select cod_marca, descripcion ");
        cSQL.Append("from lic_marcas ");

        if (!string.IsNullOrEmpty(pDescripcion))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" upper(descripcion) = '").Append(pDescripcion.ToUpper()).Append("'");

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

    public DataTable GetByContrato()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select cod_marca, descripcion ");
        cSQL.Append("from lic_marcas where cod_marca in( select cod_marca from lic_productos_contrato where num_contrato = @cod_contrato ) ");
        oParam.AddParameters("@cod_contrato", pCodContrato, TypeSQL.Numeric);

        if (!string.IsNullOrEmpty(pCodMarca))
        {
          cSQL.Append(" and cod_marca = @cod_marca");
          oParam.AddParameters("@cod_marca", pCodMarca, TypeSQL.Numeric);
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
