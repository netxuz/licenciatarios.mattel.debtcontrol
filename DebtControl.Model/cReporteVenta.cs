using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cReporteVenta
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodigoReporteVenta;
    public string CodigoReporteVenta { get { return pCodigoReporteVenta; } set { pCodigoReporteVenta = value; } }

    private string pNumContrato;
    public string NumContrato { get { return pNumContrato; } set { pNumContrato = value; } }

    private string pNoContrato;
    public string NoContrato { get { return pNoContrato; } set { pNoContrato = value; } }

    private string pTotalVenta;
    public string TotalVenta { get { return pTotalVenta; } set { pTotalVenta = value; } }

    private string pMesReporte;
    public string MesReporte { get { return pMesReporte; } set { pMesReporte = value; } }

    private string pInMesReporte;
    public string InMesReporte { get { return pInMesReporte; } set { pInMesReporte = value; } }

    private string pAnoReporte;
    public string AnoReporte { get { return pAnoReporte; } set { pAnoReporte = value; } }

    private string pFechaReporte;
    public string FechaReporte { get { return pFechaReporte; } set { pFechaReporte = value; } }

    private string pFacturado;
    public string Facturado { get { return pFacturado; } set { pFacturado = value; } }

    private string pPeriodo;
    public string Periodo { get { return pPeriodo; } set { pPeriodo = value; } }

    private string pEstReporte;
    public string EstReporte { get { return pEstReporte; } set { pEstReporte = value; } }

    private string pCodigoFactura;
    public string CodigoFactura { get { return pCodigoFactura; } set { pCodigoFactura = value; } }

    private string pDeclaraMovimiento;
    public string DeclaraMovimiento { get { return pDeclaraMovimiento; } set { pDeclaraMovimiento = value; } }

    private string pArchivoReporte;
    public string ArchivoReporte { get { return pArchivoReporte; } set { pArchivoReporte = value; } }

    private string pRepositorioArchivo;
    public string RepositorioArchivo { get { return pRepositorioArchivo; } set { pRepositorioArchivo = value; } }

    private bool pOrderMes;
    public bool OrderMes { get { return pOrderMes; } set { pOrderMes = value; } }

    private bool pOrderDesc;
    public bool OrderDesc { get { return pOrderDesc; } set { pOrderDesc = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cReporteVenta()
    {

    }

    public cReporteVenta(ref DBConn oConn)
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
        cSQL.Append("select cod_reporte_venta, num_contrato, no_contrato, total_venta, mes_reporte, ano_reporte, fecha_reporte, facturado, periodo_q, est_reporte, codigo_factura, declara_movimiento, archivo_reporte, repositorio_archivo ");
        cSQL.Append("from lic_reporte_venta ");

        if (!string.IsNullOrEmpty(pCodigoReporteVenta))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_reporte_venta = @cod_reporte_venta");
          oParam.AddParameters("@cod_reporte_venta", pCodigoReporteVenta, TypeSQL.Numeric);

        }

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

        if (!string.IsNullOrEmpty(pMesReporte))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" mes_reporte = @mes_reporte");
          oParam.AddParameters("@mes_reporte", pMesReporte, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pAnoReporte))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" ano_reporte = @ano_reporte");
          oParam.AddParameters("@ano_reporte", pAnoReporte, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pFacturado))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" facturado = @facturado");
          oParam.AddParameters("@facturado", pFacturado, TypeSQL.Char);

        }

        if (!string.IsNullOrEmpty(pPeriodo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" periodo_q = @periodo_q");
          oParam.AddParameters("@periodo_q", pPeriodo, TypeSQL.Char);

        }

        if (!string.IsNullOrEmpty(pEstReporte))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" est_reporte = @est_reporte");
          oParam.AddParameters("@est_reporte", pEstReporte, TypeSQL.Char);

        }

        if (!string.IsNullOrEmpty(pCodigoFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" codigo_factura = @codigo_factura");
          oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);

        }

        if (pOrderDesc)
        {
          cSQL.Append(" order by convert(varchar, ano_reporte) + right('00' + convert(varchar,mes_reporte),2) + '01' desc  ");
        }

        if (pOrderMes) {
          cSQL.Append(" order by ano_reporte, mes_reporte ");
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

    public DataTable GetResumenContrato()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select cod_reporte_venta, num_contrato, no_contrato, total_venta, mes_reporte, ano_reporte, fecha_reporte, facturado, periodo_q, est_reporte, codigo_factura, declara_movimiento ");
        cSQL.Append("from lic_reporte_venta where not declara_movimiento is null ");

        if (!string.IsNullOrEmpty(pCodigoReporteVenta))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_reporte_venta = @cod_reporte_venta");
          oParam.AddParameters("@cod_reporte_venta", pCodigoReporteVenta, TypeSQL.Numeric);

        }

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

        if (!string.IsNullOrEmpty(pInMesReporte))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" mes_reporte in(").Append(pInMesReporte).Append(")");

        }

        if (!string.IsNullOrEmpty(AnoReporte))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" ano_reporte = @ano_reporte");
          oParam.AddParameters("@ano_reporte", AnoReporte, TypeSQL.Varchar);

        }

        if (!string.IsNullOrEmpty(pFacturado))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" facturado = @facturado");
          oParam.AddParameters("@facturado", pFacturado, TypeSQL.Char);

        }

        if (!string.IsNullOrEmpty(pEstReporte))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" est_reporte = @est_reporte");
          oParam.AddParameters("@est_reporte", pEstReporte, TypeSQL.Char);

        }

        if (!string.IsNullOrEmpty(pCodigoFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" codigo_factura = @codigo_factura");
          oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);

        }

        if (pOrderDesc)
        {
          cSQL.Append(" order by convert(varchar, ano_reporte) + convert(varchar,right('00' + mes_reporte,2)) + '01' desc ");
        }

        if (pOrderMes)
        {
          cSQL.Append(" order by mes_reporte asc ");
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

    public DataTable GettingForInvoice()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select (select sNombre from deudor where nKey_Deudor in(select  b.nKey_Deudor from lic_contratos b where b.num_contrato = a.num_contrato) ) as nombre, ");
        cSQL.Append(" (select  no_contrato from lic_contratos where num_contrato = a.num_contrato) as nom_contrato, a.num_contrato, a.periodo_q, a.ano_reporte, max(a.fecha_reporte) 'fech_prefacturacion' from lic_reporte_venta a ");
        cSQL.Append(" where a.facturado = 'N' ");

        if (!string.IsNullOrEmpty(pEstReporte))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.est_reporte = @est_reporte ");
          oParam.AddParameters("@est_reporte", pEstReporte, TypeSQL.Char);
        }

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.num_contrato = @num_contrato ");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        cSQL.Append("group by a.num_contrato, a.periodo_q, a.ano_reporte having count(a.num_contrato) = 3 order by nombre, a.periodo_q, a.ano_reporte  ");

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

    public DataTable GetForInvoice() { 
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select periodo_q, ano_reporte from lic_reporte_venta  ");
        cSQL.Append("where facturado = 'N' and est_reporte = 'C' ");

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        cSQL.Append(" group by periodo_q, ano_reporte having count(num_contrato) = 3 order by periodo_q, ano_reporte ");

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

    public DataTable GetReportForInvoice()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select * from lic_reporte_venta   ");
        cSQL.Append("where facturado = 'N' and est_reporte = 'C' ");

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pPeriodo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" periodo_q = @periodo_q");
          oParam.AddParameters("@periodo_q", pPeriodo, TypeSQL.Varchar);

        }

        if (!string.IsNullOrEmpty(pAnoReporte))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" ano_reporte = @ano_reporte");
          oParam.AddParameters("@ano_reporte", pAnoReporte, TypeSQL.Numeric);

        }

        cSQL.Append(" order by mes_reporte asc ");

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
              pCodigoReporteVenta = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
              cSQL = new StringBuilder();
              cSQL.Append("insert into lic_reporte_venta(cod_reporte_venta, num_contrato, no_contrato, total_venta, mes_reporte, ano_reporte, fecha_reporte, facturado, periodo_q, est_reporte, codigo_factura, declara_movimiento, archivo_reporte, repositorio_archivo) values(");
              cSQL.Append("@cod_reporte_venta, @num_contrato, @no_contrato, @total_venta, @mes_reporte, @ano_reporte, @fecha_reporte, @facturado, @periodo_q, @est_reporte, @codigo_factura, @declara_movimiento, @archivo_reporte, @repositorio_archivo)");
              oParam.AddParameters("@cod_reporte_venta", pCodigoReporteVenta, TypeSQL.Numeric);
              oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
              oParam.AddParameters("@no_contrato", pNoContrato, TypeSQL.Varchar);
              oParam.AddParameters("@total_venta", pTotalVenta, TypeSQL.Numeric);
              oParam.AddParameters("@mes_reporte", pMesReporte, TypeSQL.Numeric);
              oParam.AddParameters("@ano_reporte", pAnoReporte, TypeSQL.Numeric);
              oParam.AddParameters("@fecha_reporte", pFechaReporte, TypeSQL.DateTime);
              oParam.AddParameters("@facturado", pFacturado, TypeSQL.Char);
              oParam.AddParameters("@periodo_q", pPeriodo, TypeSQL.Char);
              oParam.AddParameters("@est_reporte", pEstReporte, TypeSQL.Char);
              oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);
              oParam.AddParameters("@declara_movimiento", pDeclaraMovimiento, TypeSQL.Char);
              oParam.AddParameters("@archivo_reporte", pArchivoReporte, TypeSQL.Varchar);
              oParam.AddParameters("@repositorio_archivo", pRepositorioArchivo, TypeSQL.Char);
              oConn.Insert(cSQL.ToString(), oParam);

              break;
            case "EDITAR":
              cSQL = new StringBuilder();
              cSQL.Append("update lic_reporte_venta set ");

              if (!string.IsNullOrEmpty(pTotalVenta))
              {
                cSQL.Append(sComa);
                cSQL.Append(" total_venta = @total_venta");
                oParam.AddParameters("@total_venta", pTotalVenta, TypeSQL.Varchar);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pFechaReporte))
              {
                cSQL.Append(sComa);
                cSQL.Append(" fecha_reporte = @fecha_reporte");
                oParam.AddParameters("@fecha_reporte", pFechaReporte, TypeSQL.DateTime);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pFacturado))
              {
                cSQL.Append(sComa);
                cSQL.Append(" facturado = @facturado");
                oParam.AddParameters("@facturado", pFacturado, TypeSQL.Char);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pPeriodo))
              {
                cSQL.Append(sComa);
                cSQL.Append(" periodo_q = @periodo_q");
                oParam.AddParameters("@periodo_q", pPeriodo, TypeSQL.Char);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pEstReporte))
              {
                cSQL.Append(sComa);
                cSQL.Append(" est_reporte = @est_reporte");
                oParam.AddParameters("@est_reporte", pEstReporte, TypeSQL.Char);
                sComa = ", ";
              }

              if (!string.IsNullOrEmpty(pCodigoFactura))
              {
                cSQL.Append(sComa);
                cSQL.Append(" codigo_factura = @codigo_factura");
                oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);
                sComa = ", ";
              }

              if (!string.IsNullOrEmpty(pDeclaraMovimiento))
              {
                cSQL.Append(sComa);
                cSQL.Append(" declara_movimiento = @declara_movimiento");
                oParam.AddParameters("@declara_movimiento", pDeclaraMovimiento, TypeSQL.Char);
                sComa = ", ";
              }

              if (!string.IsNullOrEmpty(pArchivoReporte))
              {
                cSQL.Append(sComa);
                cSQL.Append(" archivo_reporte = @archivo_reporte");
                oParam.AddParameters("@archivo_reporte", pArchivoReporte, TypeSQL.Varchar);
                sComa = ", ";
              }

              if (!string.IsNullOrEmpty(pRepositorioArchivo))
              {
                cSQL.Append(sComa);
                cSQL.Append(" repositorio_archivo = @repositorio_archivo");
                oParam.AddParameters("@repositorio_archivo", pRepositorioArchivo, TypeSQL.Char);
                sComa = ", ";
              }

              cSQL.Append(" where cod_reporte_venta = @cod_reporte_venta ");
              oParam.AddParameters("@cod_reporte_venta", pCodigoReporteVenta, TypeSQL.Numeric);
              oConn.Update(cSQL.ToString(), oParam);

              break;
            case "ELIMINAFACTURA":
              cSQL = new StringBuilder();
              cSQL.Append("update lic_reporte_venta set facturado = 'N', est_reporte = 'P', codigo_factura = null  where codigo_factura = @codigo_factura ");
              oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);
              oConn.Update(cSQL.ToString(), oParam);

              break;
            case "ELIMINAR":
              cSQL = new StringBuilder();
              cSQL.Append("delete from lic_reporte_venta where cod_reporte_venta = @cod_reporte_venta");
              oParam.AddParameters("@cod_reporte_venta", pCodigoReporteVenta, TypeSQL.Numeric);
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
