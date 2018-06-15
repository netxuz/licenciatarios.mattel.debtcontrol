using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cBdiContrato
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pNumContrato;
    public string NumContrato { get { return pNumContrato; } set { pNumContrato = value; } }

    private string pNoContrato;
    public string NoContrato { get { return pNoContrato; } set { pNoContrato = value; } }

    private string pCodMarca;
    public string CodMarca { get { return pCodMarca; } set { pCodMarca = value; } }

    private string pCodCategoria;
    public string CodCategoria { get { return pCodCategoria; } set { pCodCategoria = value; } }

    private string pCodSubCategoria;
    public string CodSubCategoria { get { return pCodSubCategoria; } set { pCodSubCategoria = value; } }

    private string pPorcentaje;
    public string Porcentaje { get { return pPorcentaje; } set { pPorcentaje = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cBdiContrato()
    {

    }

    public cBdiContrato(ref DBConn oConn)
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
        cSQL.Append("select num_contrato, no_contrato, cod_marca, cod_categoria, cod_subcategoria, porcentaje ");
        cSQL.Append("from lic_bdi_contrato ");

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNoContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" no_contrato = @no_contrato");
          oParam.AddParameters("@no_contrato", pNoContrato, TypeSQL.Varchar);

        }

        if (!string.IsNullOrEmpty(pCodMarca))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_marca = @cod_marca");
          oParam.AddParameters("@cod_marca", pCodMarca, TypeSQL.Numeric);

        }
        else
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_marca is null ");
        }

        if (!string.IsNullOrEmpty(pCodCategoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_categoria = @cod_categoria");
          oParam.AddParameters("@cod_categoria", pCodCategoria, TypeSQL.Numeric);

        }
        else {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_categoria is null ");
        }

        if (!string.IsNullOrEmpty(pCodSubCategoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_subcategoria = @cod_subcategoria");
          oParam.AddParameters("@cod_subcategoria", pCodSubCategoria, TypeSQL.Numeric);

        }
        else {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_subcategoria is null ");
        }

        if (!string.IsNullOrEmpty(pPorcentaje))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" porcentaje = @porcentaje");
          oParam.AddParameters("@porcentaje", pPorcentaje, TypeSQL.Float);

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
