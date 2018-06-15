using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cSubCategoria
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodCategoria;
    public string CodCategoria { get { return pCodCategoria; } set { pCodCategoria = value; } }

    private string pCodSubCategoria;
    public string CodSubCategoria { get { return pCodSubCategoria; } set { pCodSubCategoria = value; } }

    private string pCodContrato;
    public string CodContrato { get { return pCodContrato; } set { pCodContrato = value; } }

    private string pDescripcion;
    public string Descripcion { get { return pDescripcion; } set { pDescripcion = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cSubCategoria()
    {

    }

    public cSubCategoria(ref DBConn oConn)
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
        cSQL.Append("select cod_subcategoria, descripcion ");
        cSQL.Append("from lic_subcategoria ");

        if (!string.IsNullOrEmpty(pCodSubCategoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_subcategoria = @cod_subcategoria");
          oParam.AddParameters("@cod_subcategoria", pCodSubCategoria, TypeSQL.Numeric);

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
        cSQL.Append("select cod_subcategoria, descripcion ");
        cSQL.Append("from lic_subcategoria ");

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

    public DataTable GetByCategoria()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select cod_subcategoria, descripcion ");
        cSQL.Append("from lic_subcategoria ");

        if (!string.IsNullOrEmpty(pCodCategoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_subcategoria in (select cod_subcategoria from lic_categoria_subcategoria where cod_categoria = @cod_categoria ) ");
          oParam.AddParameters("@cod_categoria", pCodCategoria, TypeSQL.Numeric);

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

    public DataTable GetByContrato()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select cod_subcategoria, descripcion ");
        cSQL.Append("from lic_subcategoria ");

        cSQL.Append(" where cod_subcategoria in (select cod_subcategoria from lic_categoria_subcategoria where cod_categoria = @cod_categoria ");
        oParam.AddParameters("@cod_categoria", pCodCategoria, TypeSQL.Numeric);

        cSQL.Append(" and cod_subcategoria in(select cod_subcategoria from lic_productos_contrato where num_contrato = @cod_contrato ");
        oParam.AddParameters("@cod_contrato", pCodContrato, TypeSQL.Numeric);

        cSQL.Append(" and cod_categoria = @cod_categoria1)) ");
        oParam.AddParameters("@cod_categoria1", pCodCategoria, TypeSQL.Numeric);

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
