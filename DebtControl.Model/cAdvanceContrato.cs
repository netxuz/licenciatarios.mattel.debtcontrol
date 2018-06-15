using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cAdvanceContrato
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

    private string pValorOriginal;
    public string ValorOriginal { get { return pValorOriginal; } set { pValorOriginal = value; } }

    private string pSaldo;
    public string Saldo { get { return pSaldo; } set { pSaldo = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private bool bFacturado;
    public bool Facturado { get { return bFacturado; } set { bFacturado = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cAdvanceContrato()
    {

    }

    public cAdvanceContrato(ref DBConn oConn)
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
        cSQL.Append("select num_contrato, no_contrato, cod_marca, cod_categoria, cod_subcategoria, valor_original, saldo ");
        cSQL.Append("from lic_advance_contrato ");

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato = @num_contrato ");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNoContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" no_contrato = @no_contrato ");
          oParam.AddParameters("@no_contrato", pNoContrato, TypeSQL.Varchar);

        }

        if (!string.IsNullOrEmpty(pCodMarca))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_marca = @cod_marca ");
          oParam.AddParameters("@cod_marca", pCodMarca, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pCodCategoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_categoria = @cod_categoria ");
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

        if (!string.IsNullOrEmpty(pValorOriginal))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" valor_original = @valor_original");
          oParam.AddParameters("@valor_original", pValorOriginal, TypeSQL.Float);

        }

        if (!string.IsNullOrEmpty(pSaldo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" saldo = @saldo");
          oParam.AddParameters("@saldo", pSaldo, TypeSQL.Float);

        }

        if (bFacturado) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato in (select num_contrato from lic_contratos where facturado_advance = 'V')  ");
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

    public DataTable GetForInvoce()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select a.cod_marca, (select descripcion from lic_marcas where cod_marca = a.cod_marca) marca, ");
        cSQL.Append(" a.cod_categoria, (select descripcion from lic_categorias where cod_categoria = a.cod_categoria) categoria, ");
        cSQL.Append(" a.cod_subcategoria, (select descripcion from lic_subcategoria where cod_subcategoria = a.cod_subcategoria) subcategoria, ");
        cSQL.Append(" a.valor_original, a.saldo ");
        cSQL.Append(" from lic_advance_contrato a ");

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
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
              cSQL = new StringBuilder();
              cSQL.Append("insert into lic_advance_contrato(num_contrato, no_contrato, cod_marca, cod_categoria, cod_subcategoria, valor_original, saldo) values(");
              cSQL.Append("num_contrato, no_contrato, cod_marca, cod_categoria, cod_subcategoria, valor_original, saldo)");
              oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
              oParam.AddParameters("@no_contrato", pNoContrato, TypeSQL.Varchar);
              oParam.AddParameters("@cod_marca", pCodMarca, TypeSQL.Numeric);
              oParam.AddParameters("@cod_categoria", pCodCategoria, TypeSQL.Numeric);
              oParam.AddParameters("@cod_subcategoria", pCodSubCategoria, TypeSQL.Numeric);
              oParam.AddParameters("@valor_original", pValorOriginal, TypeSQL.Numeric);
              oParam.AddParameters("@saldo", pSaldo, TypeSQL.Numeric);
              oConn.Insert(cSQL.ToString(), oParam);

              break;
            case "EDITAR":
              cSQL = new StringBuilder();
              cSQL.Append("update lic_advance_contrato set ");

              if (!string.IsNullOrEmpty(pSaldo))
              {
                cSQL.Append(sComa);
                cSQL.Append(" saldo = @saldo");
                oParam.AddParameters("@saldo", pSaldo, TypeSQL.Float);
                sComa = ", ";
              }

              cSQL.Append(" where num_contrato = @num_contrato ");
              oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

              cSQL.Append(" and cod_marca = @cod_marca ");
              oParam.AddParameters("@cod_marca", pCodMarca, TypeSQL.Numeric);

              if (!string.IsNullOrEmpty(pCodCategoria))
              {
                cSQL.Append(" and cod_categoria = @cod_categoria ");
                oParam.AddParameters("@cod_categoria", pCodCategoria, TypeSQL.Numeric);
              }
              else {
                cSQL.Append(" and cod_categoria is null ");
              }

              if (!string.IsNullOrEmpty(pCodSubCategoria))
              {
                cSQL.Append(" and cod_subcategoria = @cod_subcategoria ");
                oParam.AddParameters("@cod_subcategoria", pCodSubCategoria, TypeSQL.Numeric);
              }
              else
              {
                cSQL.Append(" and cod_subcategoria is null ");
              }

              oConn.Update(cSQL.ToString(), oParam);

              break;
            case "ELIMINAR":
              //cSQL = new StringBuilder();
              //cSQL.Append("delete from sys_usuario where cod_user = @cod_user");
              //oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);
              //oConn.Delete(cSQL.ToString(), oParam);

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
