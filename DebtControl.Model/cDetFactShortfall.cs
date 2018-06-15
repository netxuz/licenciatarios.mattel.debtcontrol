using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cDetFactShortfall
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

    private string pMntMinGarantizado;
    public string MntMinGarantizado { get { return pMntMinGarantizado; } set { pMntMinGarantizado = value; } }

    private string pMntFactAdvance;
    public string MntFactAdvance { get { return pMntFactAdvance; } set { pMntFactAdvance = value; } }

    private string pPeriodoFactUno;
    public string PeriodoFactUno { get { return pPeriodoFactUno; } set { pPeriodoFactUno = value; } }

    private string pMntPeriodoFactUno;
    public string MntPeriodoFactUno { get { return pMntPeriodoFactUno; } set { pMntPeriodoFactUno = value; } }

    private string pPeriodoFactDos;
    public string PeriodoFactDos { get { return pPeriodoFactDos; } set { pPeriodoFactDos = value; } }

    private string pMntPeriodoFactDos;
    public string MntPeriodoFactDos { get { return pMntPeriodoFactDos; } set { pMntPeriodoFactDos = value; } }

    private string pPeriodoFactTres;
    public string PeriodoFactTres { get { return pPeriodoFactTres; } set { pPeriodoFactTres = value; } }

    private string pMntPeriodoFactTres;
    public string MntPeriodoFactTres { get { return pMntPeriodoFactTres; } set { pMntPeriodoFactTres = value; } }

    private string pPeriodoFactCuatro;
    public string PeriodoFactCuatro { get { return pPeriodoFactCuatro; } set { pPeriodoFactCuatro = value; } }

    private string pMntPeriodoFactCuatro;
    public string MntPeriodoFactCuatro { get { return pMntPeriodoFactCuatro; } set { pMntPeriodoFactCuatro = value; } }

    private string pFacturaUsd;
    public string FacturaUsd { get { return pFacturaUsd; } set { pFacturaUsd = value; } }

    private string pMntDescuento;
    public string MntDescuento { get { return pMntDescuento; } set { pMntDescuento = value; } }

    private string pFacturaUsdDf;
    public string FacturaUsdDf { get { return pFacturaUsdDf; } set { pFacturaUsdDf = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cDetFactShortfall()
    {

    }

    public cDetFactShortfall(ref DBConn oConn)
    {
      this.oConn = oConn;
    }

    public void Put() {
      DataTable dtData;
      oParam = new DBConn.SQLParameters(20);
      StringBuilder cSQL;
      string sComa = string.Empty;
      if (oConn.bIsOpen) {
        try {
          switch (pAccion) {
            case "CREAR":
              cSQL = new StringBuilder();
              cSQL.Append("insert into lic_det_fact_shortfall(codigo_factura, num_contrato, cod_marca, cod_categoria, cod_subcategoria, mnt_min_garantizado, mnt_fact_advance, periodo_fact_uno, mnt_periodo_fact_uno, periodo_fact_dos, mnt_periodo_fact_dos, periodo_fact_tres, mnt_periodo_fact_tres, periodo_fact_cuatro, mnt_periodo_fact_cuatro, factura_usd, mnt_descuento, factura_usd_df) values(");
              cSQL.Append("@codigo_factura, @num_contrato, @cod_marca, @cod_categoria, @cod_subcategoria, @mnt_min_garantizado, @mnt_fact_advance, @periodo_fact_uno, @mnt_periodo_fact_uno, @periodo_fact_dos, @mnt_periodo_fact_dos, @periodo_fact_tres, @mnt_periodo_fact_tres, @periodo_fact_cuatro, @mnt_periodo_fact_cuatro, @factura_usd, @mnt_descuento, @factura_usd_df) ");
              oParam.AddParameters("@codigo_factura", pCodigoFactura, TypeSQL.Numeric);
              oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
              oParam.AddParameters("@cod_marca", pCodMarca, TypeSQL.Numeric);
              oParam.AddParameters("@cod_categoria", pCodCategoria, TypeSQL.Numeric);
              oParam.AddParameters("@cod_subcategoria", pCodSubcategoria, TypeSQL.Numeric);
              oParam.AddParameters("@mnt_min_garantizado", pMntMinGarantizado, TypeSQL.Float);
              oParam.AddParameters("@mnt_fact_advance", pMntFactAdvance, TypeSQL.Float);
              oParam.AddParameters("@periodo_fact_uno", pPeriodoFactUno, TypeSQL.Varchar);
              oParam.AddParameters("@mnt_periodo_fact_uno", pMntPeriodoFactUno, TypeSQL.Float);
              oParam.AddParameters("@periodo_fact_dos", pPeriodoFactDos, TypeSQL.Varchar);
              oParam.AddParameters("@mnt_periodo_fact_dos", pMntPeriodoFactDos, TypeSQL.Numeric);
              oParam.AddParameters("@periodo_fact_tres", pPeriodoFactTres, TypeSQL.Varchar);
              oParam.AddParameters("@mnt_periodo_fact_tres", pMntPeriodoFactTres, TypeSQL.Float);
              oParam.AddParameters("@periodo_fact_cuatro", pPeriodoFactCuatro, TypeSQL.Varchar);
              oParam.AddParameters("@mnt_periodo_fact_cuatro", pMntPeriodoFactCuatro, TypeSQL.Float);
              oParam.AddParameters("@factura_usd", pFacturaUsd, TypeSQL.Float);
              oParam.AddParameters("@mnt_descuento", pMntDescuento, TypeSQL.Float);
              oParam.AddParameters("@factura_usd_df", pFacturaUsdDf, TypeSQL.Float);
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
              cSQL.Append("delete from lic_det_fact_shortfall where codigo_factura = @codigo_factura");
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
