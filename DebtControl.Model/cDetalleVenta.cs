using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cDetalleVenta
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pNkeyDeudor;
    public string NkeyDeudor { get { return pNkeyDeudor; } set { pNkeyDeudor = value; } }

    private string pNumContrato;
    public string NumContrato { get { return pNumContrato; } set { pNumContrato = value; } }

    private string pFechaInicio;
    public string FechaInicio { get { return pFechaInicio; } set { pFechaInicio = value; } }

    private string pFechaFinal;
    public string FechaFinal { get { return pFechaFinal; } set { pFechaFinal = value; } }

    private string pCodigoDetalle;
    public string CodigoDetalle { get { return pCodigoDetalle; } set { pCodigoDetalle = value; } }

    private string pCodigoReporteVenta;
    public string CodigoReporteVenta { get { return pCodigoReporteVenta; } set { pCodigoReporteVenta = value; } }

    private string pMarca;
    public string Marca { get { return pMarca; } set { pMarca = value; } }

    private string pCategoria;
    public string Categoria { get { return pCategoria; } set { pCategoria = value; } }

    private string pSubCategoria;
    public string SubCategoria { get { return pSubCategoria; } set { pSubCategoria = value; } }

    private string pProducto;
    public string Producto { get { return pProducto; } set { pProducto = value; } }

    private string pCodRoyalty;
    public string CodRoyalty { get { return pCodRoyalty; } set { pCodRoyalty = value; } }

    private string pRoyalty;
    public string Royalty { get { return pRoyalty; } set { pRoyalty = value; } }

    private string pBdi;
    public string Bdi { get { return pBdi; } set { pBdi = value; } }

    private string pCliente;
    public string Cliente { get { return pCliente; } set { pCliente = value; } }

    private string pSku;
    public string Sku { get { return pSku; } set { pSku = value; } }

    private string pDescripcionProducto;
    public string DescripcionProducto { get { return pDescripcionProducto; } set { pDescripcionProducto = value; } }

    private string pPrecioUnitarioVentaBruta;
    public string PrecioUnitarioVentaBruta { get { return pPrecioUnitarioVentaBruta; } set { pPrecioUnitarioVentaBruta = value; } }

    private string pCantidadVentaBruta;
    public string CantidadVentaBruta { get { return pCantidadVentaBruta; } set { pCantidadVentaBruta = value; } }

    private string pPrecioUnitDescueDevol;
    public string PrecioUnitDescueDevol { get { return pPrecioUnitDescueDevol; } set { pPrecioUnitDescueDevol = value; } }

    private string pCantidadDescueDevol;
    public string CantidadDescueDevol { get { return pCantidadDescueDevol; } set { pCantidadDescueDevol = value; } }

    private bool pIndLoadDatMonthAnt;
    public bool IndLoadDatMonthAnt { get { return pIndLoadDatMonthAnt; } set { pIndLoadDatMonthAnt = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cDetalleVenta()
    {

    }

    public cDetalleVenta(ref DBConn oConn)
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
        cSQL.Append("select a.codigo_detalle, a.cod_reporte_venta,");
        cSQL.Append("a.cod_marca, (select descripcion from lic_marcas where cod_marca = a.cod_marca) marca, ");
        cSQL.Append("a.cod_categoria, (select descripcion from lic_categorias where cod_categoria = a.cod_categoria) categoria, ");
        cSQL.Append("a.cod_subcategoria, (select descripcion from lic_subcategoria where cod_subcategoria = a.cod_subcategoria) subcategoria, ");
        cSQL.Append("a.producto, ");
        cSQL.Append("a.cod_royalty, (select descripcion from lic_tipo_royalty where cod_royalty = a.cod_royalty) nom_royalty, ");
        cSQL.Append("a.royalty, a.bdi, a.cliente, a.sku, a.descripcion_producto, a.precio_uni_venta_bruta ");

        if (pIndLoadDatMonthAnt)
        {
          cSQL.Append(", '0' as 'cantidad_venta_bruta', '0' as 'precio_unit_descue_devol', '0' as 'cantidad_descue_devol' ");
        }
        else
        {
          cSQL.Append(", a.cantidad_venta_bruta, a.precio_unit_descue_devol, a.cantidad_descue_devol ");
        }

        cSQL.Append("from lic_detalle_venta a ");

        if (!string.IsNullOrEmpty(pCodigoDetalle))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.codigo_detalle = @codigo_detalle");
          oParam.AddParameters("@codigo_detalle", pCodigoDetalle, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pCodigoReporteVenta))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.cod_reporte_venta = @cod_reporte_venta");
          oParam.AddParameters("@cod_reporte_venta", pCodigoReporteVenta, TypeSQL.Numeric);

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

    public DataTable GetByFactura()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        //cSQL.Append("select cod_marca, cod_categoria, cod_subcategoria, cod_royalty, royalty, bdi, sum(precio_uni_venta_bruta) precio_uni, sum(cantidad_venta_bruta) cantidad_venta, sum(precio_unit_descue_devol) precio_uni_devol, sum(cantidad_descue_devol) cant_descue_devol ");
        cSQL.Append("select cod_marca, cod_categoria, cod_subcategoria, cod_royalty, royalty, bdi, ");
        cSQL.Append("(sum(precio_uni_venta_bruta * cantidad_venta_bruta) - sum(precio_unit_descue_devol * cantidad_descue_devol)) 'totalmes' ");
        cSQL.Append("from lic_detalle_venta ");

        if (!string.IsNullOrEmpty(pCodigoReporteVenta))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_reporte_venta = @cod_reporte_venta");
          oParam.AddParameters("@cod_reporte_venta", pCodigoReporteVenta, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(Marca))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_marca = @cod_marca");
          oParam.AddParameters("@cod_marca", Marca, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(Categoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_categoria = @cod_categoria ");
          oParam.AddParameters("@cod_categoria", Categoria, TypeSQL.Numeric);

        }
        else
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_categoria is null ");
        }

        if (!string.IsNullOrEmpty(SubCategoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_subcategoria = @cod_subcategoria ");
          oParam.AddParameters("@cod_subcategoria", SubCategoria, TypeSQL.Numeric);

        }
        else
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_subcategoria is null ");
        }

        if (!string.IsNullOrEmpty(pCodRoyalty))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_royalty = @cod_royalty");
          oParam.AddParameters("@cod_royalty", pCodRoyalty, TypeSQL.Numeric);

        }

        cSQL.Append(" group by cod_marca, cod_categoria, cod_subcategoria, cod_royalty, royalty, bdi ");

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

    public DataTable GetByReporteVenta()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select a.cod_marca, (select descripcion from lic_marcas where cod_marca = a.cod_marca) nom_marca, ");
        cSQL.Append("a.cod_categoria, (select descripcion from lic_categorias where cod_categoria = a.cod_categoria) nom_categoria, ");
        cSQL.Append("a.cod_subcategoria, (select descripcion from lic_subcategoria where cod_subcategoria = a.cod_subcategoria) nom_subcategoria, ");
        cSQL.Append("(select mes_reporte from lic_reporte_venta where cod_reporte_venta = a.cod_reporte_venta) mes, ");
        cSQL.Append("(select ano_reporte from lic_reporte_venta where cod_reporte_venta = a.cod_reporte_venta) ano, ");
        cSQL.Append("a.cod_royalty, (select descripcion from lic_tipo_royalty where cod_royalty = a.cod_royalty) nom_rotalty, ");
        cSQL.Append("a.royalty, a.bdi, a.producto, a.descripcion_producto, a.cliente, a.sku,");
        cSQL.Append(" (select valor_moneda from lic_monedas where cod_moneda in (select cod_moneda from lic_tipo_royalty where cod_royalty = a.cod_royalty) and convert(varchar,fecha_moneda,112) in((select  convert(varchar,ano_reporte)+right('00' + convert(varchar,mes_reporte), 2)+'01' from lic_reporte_venta where cod_reporte_venta = a.cod_reporte_venta ))) tipo_cambio , ");
        cSQL.Append("sum(a.precio_uni_venta_bruta) precio_uni_venta_bruta, sum(a.cantidad_venta_bruta) cantidad_venta_bruta, ");
        cSQL.Append("(sum(a.precio_uni_venta_bruta * a.cantidad_venta_bruta)) total_local, ");
        cSQL.Append("sum(a.precio_unit_descue_devol) precio_uni_devolucion, sum(a.cantidad_descue_devol) cantidad_q_devolucion,  ");
        cSQL.Append("(sum(a.precio_unit_descue_devol * a.cantidad_descue_devol)) total_local_devol  ");
        cSQL.Append("from lic_detalle_venta a ");

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_reporte_venta in(select cod_reporte_venta from lic_reporte_venta where num_contrato = @num_contrato ");

          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

          if ((!string.IsNullOrEmpty(pFechaInicio)) && (!string.IsNullOrEmpty(pFechaFinal)))
          {
            cSQL.Append(" and mes_reporte in (" + pFechaInicio + ") ");
            cSQL.Append(" and ano_reporte in (" + pFechaFinal + ") ");
          }

          cSQL.Append(") ");
        }

        cSQL.Append(" group by a.cod_reporte_venta, a.cod_marca, a.cod_categoria, a.cod_subcategoria, a.cod_royalty, a.royalty, a.bdi, producto, a.descripcion_producto, a.cliente, a.sku ");
        cSQL.Append(" order by ano, mes ");

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

    public DataTable GetByReporteVentaFinanzas()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";
      string CondicionTwo = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select (select snombre from deudor where nKey_Deudor in (select nKey_Deudor from lic_contratos where num_contrato in (select num_contrato from lic_reporte_venta where cod_reporte_venta = a.cod_reporte_venta))) licenciatario, ");
        cSQL.Append("(select no_contrato from lic_contratos where num_contrato in (select num_contrato from lic_reporte_venta where cod_reporte_venta = a.cod_reporte_venta)) no_contrato, ");
        cSQL.Append("a.cod_marca, (select descripcion from lic_marcas where cod_marca = a.cod_marca) nom_marca, ");
        cSQL.Append("a.cod_categoria, (select descripcion from lic_categorias where cod_categoria = a.cod_categoria) nom_categoria, ");
        cSQL.Append("a.cod_subcategoria, (select descripcion from lic_subcategoria where cod_subcategoria = a.cod_subcategoria) nom_subcategoria, ");
        cSQL.Append("(select mes_reporte from lic_reporte_venta where cod_reporte_venta = a.cod_reporte_venta) mes, ");
        cSQL.Append("(select ano_reporte from lic_reporte_venta where cod_reporte_venta = a.cod_reporte_venta) ano, ");
        cSQL.Append("a.cod_royalty, (select descripcion from lic_tipo_royalty where cod_royalty = a.cod_royalty) nom_rotalty, ");
        cSQL.Append("a.royalty, a.bdi, a.producto, a.descripcion_producto, a.cliente, a.sku,");
        cSQL.Append(" (select valor_moneda from lic_monedas where cod_moneda in (select cod_moneda from lic_tipo_royalty where cod_royalty = a.cod_royalty) and fecha_moneda in((select  convert(varchar,ano_reporte)+right('00' + convert(varchar,mes_reporte), 2)+'01' from lic_reporte_venta where cod_reporte_venta = a.cod_reporte_venta ))) tipo_cambio , ");
        cSQL.Append("sum(a.precio_uni_venta_bruta) precio_uni_venta_bruta, sum(a.cantidad_venta_bruta) cantidad_venta_bruta, ");
        cSQL.Append("(sum(a.precio_uni_venta_bruta * a.cantidad_venta_bruta)) total_local, ");
        cSQL.Append("sum(a.precio_unit_descue_devol) precio_uni_devolucion, sum(a.cantidad_descue_devol) cantidad_q_devolucion,  ");
        cSQL.Append("(sum(a.precio_unit_descue_devol * a.cantidad_descue_devol)) total_local_devol  ");
        cSQL.Append("from lic_detalle_venta a ");

        if ((!string.IsNullOrEmpty(pNumContrato)) || (!string.IsNullOrEmpty(pFechaInicio)) || (!string.IsNullOrEmpty(pFechaFinal)) || (!string.IsNullOrEmpty(pNkeyDeudor)))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.cod_reporte_venta in(select cod_reporte_venta from lic_reporte_venta ");

          if (pNumContrato != "0")
          {
            cSQL.Append(CondicionTwo);
            CondicionTwo = " and ";
            cSQL.Append(" num_contrato = @num_contrato ");
            oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
          }

          if ((!string.IsNullOrEmpty(pFechaInicio)) && (!string.IsNullOrEmpty(pFechaFinal)))
          {
            cSQL.Append(CondicionTwo);
            CondicionTwo = " and ";
            cSQL.Append(" mes_reporte in( " + pFechaInicio + " )");
            cSQL.Append(" and ano_reporte in ( " + pFechaFinal + " ) ");
          }

          if (pNkeyDeudor != "0")
          {
            cSQL.Append(CondicionTwo);
            CondicionTwo = " and ";
            cSQL.Append(" cod_reporte_venta in(select cod_reporte_venta from lic_reporte_venta where num_contrato in (select num_contrato from lic_contratos where nKey_Deudor = @nKey_Deudor)) ");
            oParam.AddParameters("@nKey_Deudor", pNkeyDeudor, TypeSQL.Numeric);
          }

          cSQL.Append(") ");
        }

        cSQL.Append(" group by a.cod_reporte_venta, a.cod_marca, a.cod_categoria, a.cod_subcategoria, a.cod_royalty, a.royalty, a.bdi, producto, a.descripcion_producto, a.cliente, a.sku ");
        cSQL.Append(" order by ano, mes ");

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
              //pCodigoDetalle = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
              cSQL = new StringBuilder();
              cSQL.Append("insert into lic_detalle_venta(cod_reporte_venta, cod_marca, cod_categoria, cod_subcategoria, producto, cod_royalty, royalty, bdi, cliente, sku, descripcion_producto, precio_uni_venta_bruta, cantidad_venta_bruta, precio_unit_descue_devol, cantidad_descue_devol) values(");
              cSQL.Append("@cod_reporte_venta, @cod_marca, @cod_categoria, @cod_subcategoria, @producto, @cod_royalty, @royalty, @bdi, @cliente, @sku, @descripcion_producto, @precio_uni_venta_bruta, @cantidad_venta_bruta, @precio_unit_descue_devol, @cantidad_descue_devol) ");
              oParam.AddParameters("@cod_reporte_venta", pCodigoReporteVenta, TypeSQL.Numeric);
              oParam.AddParameters("@cod_marca", pMarca, TypeSQL.Numeric);
              oParam.AddParameters("@cod_categoria", pCategoria, TypeSQL.Numeric);
              oParam.AddParameters("@cod_subcategoria", pSubCategoria, TypeSQL.Numeric);
              oParam.AddParameters("@producto", pProducto, TypeSQL.Varchar);
              oParam.AddParameters("@cod_royalty", pCodRoyalty, TypeSQL.Numeric);
              oParam.AddParameters("@royalty", pRoyalty, TypeSQL.Float);
              oParam.AddParameters("@bdi", pBdi, TypeSQL.Float);
              oParam.AddParameters("@cliente", pCliente, TypeSQL.Varchar);
              oParam.AddParameters("@sku", pSku, TypeSQL.Varchar);
              oParam.AddParameters("@descripcion_producto", pDescripcionProducto, TypeSQL.Varchar);
              oParam.AddParameters("@precio_uni_venta_bruta", pPrecioUnitarioVentaBruta, TypeSQL.Float);
              oParam.AddParameters("@cantidad_venta_bruta", pCantidadVentaBruta, TypeSQL.Numeric);
              oParam.AddParameters("@precio_unit_descue_devol", pPrecioUnitDescueDevol, TypeSQL.Float);
              oParam.AddParameters("@cantidad_descue_devol", pCantidadDescueDevol, TypeSQL.Numeric);
              oConn.Insert(cSQL.ToString(), oParam);

              cSQL = new StringBuilder();
              cSQL.Append("select @@IDENTITY");
              dtData = oConn.Select(cSQL.ToString());
              if (dtData != null)
                if (dtData.Rows.Count > 0)
                  pCodigoDetalle = dtData.Rows[0][0].ToString();
              dtData = null;

              break;
            case "EDITAR":
              cSQL = new StringBuilder();
              cSQL.Append("update lic_detalle_venta set ");

              if (!string.IsNullOrEmpty(pPrecioUnitarioVentaBruta))
              {
                cSQL.Append(sComa);
                cSQL.Append(" precio_uni_venta_bruta = @precio_uni_venta_bruta");
                oParam.AddParameters("@precio_uni_venta_bruta", pPrecioUnitarioVentaBruta, TypeSQL.Float);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pCantidadVentaBruta))
              {
                cSQL.Append(sComa);
                cSQL.Append(" cantidad_venta_bruta = @cantidad_venta_bruta");
                oParam.AddParameters("@cantidad_venta_bruta", pCantidadVentaBruta, TypeSQL.Numeric);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pPrecioUnitDescueDevol))
              {
                cSQL.Append(sComa);
                cSQL.Append(" precio_unit_descue_devol = @precio_unit_descue_devol");
                oParam.AddParameters("@precio_unit_descue_devol", pPrecioUnitDescueDevol, TypeSQL.Float);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pCantidadDescueDevol))
              {
                cSQL.Append(sComa);
                cSQL.Append(" cantidad_descue_devol = @cantidad_descue_devol");
                oParam.AddParameters("@cantidad_descue_devol", pCantidadDescueDevol, TypeSQL.Numeric);
                sComa = ", ";
              }

              cSQL.Append(" where codigo_detalle = @codigo_detalle ");
              oParam.AddParameters("@codigo_detalle", pCodigoDetalle, TypeSQL.Numeric);
              oConn.Update(cSQL.ToString(), oParam);

              break;
            case "ELIMINAR":
              string Condicion = " where ";
              cSQL = new StringBuilder();
              cSQL.Append("delete from lic_detalle_venta ");

              if (!string.IsNullOrEmpty(pCodigoDetalle))
              {
                cSQL.Append(Condicion);
                Condicion = " and ";
                cSQL.Append(" codigo_detalle = @codigo_detalle");
                oParam.AddParameters("@codigo_detalle", pCodigoDetalle, TypeSQL.Numeric);

              }

              if (!string.IsNullOrEmpty(pCodigoReporteVenta))
              {
                cSQL.Append(Condicion);
                Condicion = " and ";
                cSQL.Append(" cod_reporte_venta = @cod_reporte_venta");
                oParam.AddParameters("@cod_reporte_venta", pCodigoReporteVenta, TypeSQL.Numeric);

              }
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
