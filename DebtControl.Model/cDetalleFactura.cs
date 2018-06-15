using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cDetalleFactura
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodDetFactura;
    public string CodDetFactura { get { return pCodDetFactura; } set { pCodDetFactura = value; } }

    private string pCodigoFactura;
    public string CodigoFactura { get { return pCodigoFactura; } set { pCodigoFactura = value; } }

    private string pNumContrato;
    public string NumContrato { get { return pNumContrato; } set { pNumContrato = value; } }

    private string pCodMarca;
    public string CodMarca { get { return pCodMarca; } set { pCodMarca = value; } }

    private string pCodCategoria;
    public string CodCategoria { get { return pCodCategoria; } set { pCodCategoria = value; } }

    private string pCodSubcategoria;
    public string CodSubcategoria { get { return pCodSubcategoria; } set { pCodSubcategoria = value; } }

    private string pMesNomUno;
    public string MesNomUno { get { return pMesNomUno; } set { pMesNomUno = value; } }

    private string pMesMntUno;
    public string MesMntUno { get { return pMesMntUno; } set { pMesMntUno = value; } }

    private string pMesNomDos;
    public string MesNomDos { get { return pMesNomDos; } set { pMesNomDos = value; } }

    private string pMesMntDos;
    public string MesMntDos { get { return pMesMntDos; } set { pMesMntDos = value; } }

    private string pMesNomTres;
    public string MesNomTres { get { return pMesNomTres; } set { pMesNomTres = value; } }

    private string pMesMntTres;
    public string MesMntTres { get { return pMesMntTres; } set { pMesMntTres = value; } }

    private string pCodRoyalty;
    public string CodRoyalty { get { return pCodRoyalty; } set { pCodRoyalty = value; } }

    private string pRoyalty;
    public string Royalty { get { return pRoyalty; } set { pRoyalty = value; } }

    private string pBdi;
    public string Bdi { get { return pBdi; } set { pBdi = value; } }

    private string pPeriodo;
    public string Periodo { get { return pPeriodo; } set { pPeriodo = value; } }

    private string pMontoRoyaltyUsd;
    public string MontoRoyaltyUsd { get { return pMontoRoyaltyUsd; } set { pMontoRoyaltyUsd = value; } }

    private string pMontoBdiUsd;
    public string MontoBdiUsd { get { return pMontoBdiUsd; } set { pMontoBdiUsd = value; } }

    private string pSaldoAdvanceUsd;
    public string SaldoAdvanceUsd { get { return pSaldoAdvanceUsd; } set { pSaldoAdvanceUsd = value; } }

    private string pFacturaUsd;
    public string FacturaUsd { get { return pFacturaUsd; } set { pFacturaUsd = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cDetalleFactura()
    {

    }

    public cDetalleFactura(ref DBConn oConn)
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
        cSQL.Append("select cod_det_factura, codigo_factura, num_contrato, cod_marca, cod_categoria, cod_subcategoria, mesnomuno, mesmntuno, mesnomdos, mesmntdos, mesnomtres, mesmnttres, royalty, bdi, periodo, monto_royalty_usd, monto_bdi_usd, saldo_advance_usd, factura_usd, cod_royalty ");
        cSQL.Append("from detalle_factura ");

        if (!string.IsNullOrEmpty(pCodDetFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_det_factura = @cod_det_factura");
          oParam.AddParameters("@cod_det_factura", pCodDetFactura, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pCodigoFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" codigo_factura = @codigo_factura");
          oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato = @num_contrato");
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

    public DataTable GetByMinimo()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select  ");

        cSQL.Append(" (select descripcion from lic_marcas where cod_marca = a.cod_marca) marca, ");
        cSQL.Append("(select descripcion from lic_categorias where cod_categoria = a.cod_categoria) categoria, ");
        cSQL.Append("(select descripcion from lic_subcategoria where cod_subcategoria = a.cod_subcategoria) subcategoria, ");
        cSQL.Append(" sum(factura_usd) factura_usd, sum(monto_royalty_usd) monto_royalty_usd, sum(saldo_advance_usd) saldo_advance_usd ");
        cSQL.Append("from detalle_factura a ");

        if (!string.IsNullOrEmpty(pCodDetFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.cod_det_factura = @cod_det_factura");
          oParam.AddParameters("@cod_det_factura", pCodDetFactura, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pCodigoFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.codigo_factura = @codigo_factura");
          oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pCodMarca))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_marca = @cod_marca");
          oParam.AddParameters("@cod_marca", pCodMarca, TypeSQL.Numeric);

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

        if (!string.IsNullOrEmpty(pCodSubcategoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_subcategoria = @cod_subcategoria ");
          oParam.AddParameters("@cod_subcategoria", pCodSubcategoria, TypeSQL.Numeric);

        }
        else
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_subcategoria is null ");
        }

        cSQL.Append("group by  a.cod_marca, a.cod_categoria, a.cod_subcategoria ");

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
              cSQL.Append("insert into detalle_factura(codigo_factura, num_contrato, cod_marca, cod_categoria, cod_subcategoria, mesnomuno, mesmntuno, mesnomdos, mesmntdos, mesnomtres, mesmnttres, cod_royalty, royalty, bdi, periodo, monto_royalty_usd, monto_bdi_usd, saldo_advance_usd, factura_usd) values(");
              cSQL.Append("@codigo_factura, @num_contrato, @cod_marca, @cod_categoria, @cod_subcategoria, @mesnomuno, @mesmntuno, @mesnomdos, @mesmntdos, @mesnomtres, @mesmnttres, @cod_royalty, @royalty, @bdi, @periodo, @monto_royalty_usd, @monto_bdi_usd, @saldo_advance_usd, @factura_usd) ");
              oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);
              oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
              oParam.AddParameters("@cod_marca", pCodMarca, TypeSQL.Numeric);
              oParam.AddParameters("@cod_categoria", pCodCategoria, TypeSQL.Numeric);
              oParam.AddParameters("@cod_subcategoria", pCodSubcategoria, TypeSQL.Numeric);
              oParam.AddParameters("@mesnomuno", pMesNomUno, TypeSQL.Varchar);
              oParam.AddParameters("@mesmntuno", pMesMntUno, TypeSQL.Numeric);
              oParam.AddParameters("@mesnomdos", pMesNomDos, TypeSQL.Varchar);
              oParam.AddParameters("@mesmntdos", pMesMntDos, TypeSQL.Numeric);
              oParam.AddParameters("@mesnomtres", pMesNomTres, TypeSQL.Varchar);
              oParam.AddParameters("@mesmnttres", pMesMntTres, TypeSQL.Numeric);
              oParam.AddParameters("@cod_royalty", pCodRoyalty, TypeSQL.Numeric);
              oParam.AddParameters("@royalty", pRoyalty, TypeSQL.Float);
              oParam.AddParameters("@bdi", pBdi, TypeSQL.Float);
              oParam.AddParameters("@periodo", pPeriodo, TypeSQL.Varchar);
              oParam.AddParameters("@monto_royalty_usd", pMontoRoyaltyUsd, TypeSQL.Float);
              oParam.AddParameters("@monto_bdi_usd", pMontoBdiUsd, TypeSQL.Float);
              oParam.AddParameters("@saldo_advance_usd", pSaldoAdvanceUsd, TypeSQL.Float);
              oParam.AddParameters("@factura_usd", pFacturaUsd, TypeSQL.Float);
              oConn.Insert(cSQL.ToString(), oParam);

              cSQL = new StringBuilder();
              cSQL.Append("select @@IDENTITY");
              dtData = oConn.Select(cSQL.ToString());
              if (dtData != null)
                if (dtData.Rows.Count > 0)
                  pCodDetFactura = dtData.Rows[0][0].ToString();
              dtData = null;

              break;
            case "ELIMINAR":
              cSQL = new StringBuilder();
              cSQL.Append("delete from detalle_factura where codigo_factura = @codigo_factura");
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

