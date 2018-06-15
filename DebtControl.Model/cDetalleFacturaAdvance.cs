using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cDetalleFacturaAdvance
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodDetFactura;
    public string CodDetFactura { get { return pCodDetFactura; } set { pCodDetFactura = value; } }

    private string pCodigoFactura;
    public string CodigoFactura { get { return pCodigoFactura; } set { pCodigoFactura = value; } }

    private string pCodMarca;
    public string CodMarca { get { return pCodMarca; } set { pCodMarca = value; } }

    private string pCodCategoria;
    public string CodCategoria { get { return pCodCategoria; } set { pCodCategoria = value; } }

    private string pCodSubCategoria;
    public string CodSubCategoria { get { return pCodSubCategoria; } set { pCodSubCategoria = value; } }

    private string pAdvanceUsd;
    public string AdvanceUsd { get { return pAdvanceUsd; } set { pAdvanceUsd = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cDetalleFacturaAdvance()
    {

    }

    public cDetalleFacturaAdvance(ref DBConn oConn)
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
        cSQL.Append("select cod_det_factura, codigo_factura, cod_marca, advance_usd ");
        cSQL.Append("from detalle_factura_advance ");

        if (!string.IsNullOrEmpty(pCodigoFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" codigo_factura = @codigo_factura");
          oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);

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

    public DataTable GetByMinimo()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select cod_det_factura, codigo_factura, cod_marca, cod_categoria, cod_subcategoria, advance_usd ");
        cSQL.Append("from detalle_factura_advance ");

        if (!string.IsNullOrEmpty(pCodigoFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" codigo_factura = @codigo_factura");
          oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pCodMarca))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_marca = @cod_marca ");
          oParam.AddParameters("@cod_marca", pCodMarca, TypeSQL.Numeric);
        }
        else {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_marca is null ");
        }

        if (!string.IsNullOrEmpty(pCodCategoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_categoria = @cod_categoria ");
          oParam.AddParameters("@cod_categoria", pCodCategoria, TypeSQL.Numeric);
        }
        else
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_categoria is null ");
        }

        if (!string.IsNullOrEmpty(pCodSubCategoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_subcategoria = @cod_subcategoria ");
          oParam.AddParameters("@cod_subcategoria", pCodSubCategoria, TypeSQL.Numeric);
        }
        else
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_subcategoria is null ");
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
      DataTable dtData;
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
              cSQL.Append("insert into detalle_factura_advance(codigo_factura, cod_marca, cod_categoria, cod_subcategoria, advance_usd) values(");
              cSQL.Append("@codigo_factura, @cod_marca, @cod_categoria, @cod_subcategoria, @advance_usd) ");
              oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);
              oParam.AddParameters("@cod_marca", pCodMarca, TypeSQL.Numeric);
              oParam.AddParameters("@cod_categoria", pCodCategoria, TypeSQL.Numeric);
              oParam.AddParameters("@cod_subcategoria", pCodSubCategoria, TypeSQL.Numeric);
              oParam.AddParameters("@advance_usd", pAdvanceUsd, TypeSQL.Float);
              oConn.Insert(cSQL.ToString(), oParam);

              cSQL = new StringBuilder();
              cSQL.Append("select @@IDENTITY");
              dtData = oConn.Select(cSQL.ToString());
              if (dtData != null)
                if (dtData.Rows.Count > 0)
                  pCodDetFactura = dtData.Rows[0][0].ToString();
              dtData = null;

              break;
            case "EDITAR":
              break;
            case "ELIMINAR":
              cSQL = new StringBuilder();
              cSQL.Append("delete from detalle_factura_advance where codigo_factura = @codigo_factura ");
              oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);
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
