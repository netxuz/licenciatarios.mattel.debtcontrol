using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cContratos
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pNkeyCliente;
    public string NkeyCliente { get { return pNkeyCliente; } set { pNkeyCliente = value; } }

    private string pNkeyDeudor;
    public string NkeyDeudor { get { return pNkeyDeudor; } set { pNkeyDeudor = value; } }

    private string pNumContrato;
    public string NumContrato { get { return pNumContrato; } set { pNumContrato = value; } }

    private string pNoContrato;
    public string NoContrato { get { return pNoContrato; } set { pNoContrato = value; } }

    private string pTipoContrato;
    public string TipoContrato { get { return pTipoContrato; } set { pTipoContrato = value; } }

    private string pEstado;
    public string Estado { get { return pEstado; } set { pEstado = value; } }

    private string pPaymentTerms;
    public string PaymentTerms { get { return pPaymentTerms; } set { pPaymentTerms = value; } }

    private string pPropertyContrato;
    public string PropertyContrato { get { return pPropertyContrato; } set { pPropertyContrato = value; } }

    private string pTerritorioContrato;
    public string TerritorioContrato { get { return pTerritorioContrato; } set { pTerritorioContrato = value; } }

    private string pMonedaContrato;
    public string MonedaContrato { get { return pMonedaContrato; } set { pMonedaContrato = value; } }

    private string pFechInicio;
    public string FechInicio { get { return pFechInicio; } set { pFechInicio = value; } }

    private string pFechTermino;
    public string FechTermino { get { return pFechTermino; } set { pFechTermino = value; } }

    private string pFacturadoAdvance;
    public string FacturadoAdvance { get { return pFacturadoAdvance; } set { pFacturadoAdvance = value; } }

    private bool pIsNullFacturadoAdvance;
    public bool IsNullFacturadoAdvance { get { return pIsNullFacturadoAdvance; } set { pIsNullFacturadoAdvance = value; } }

    private string pAnoTermino;
    public string AnoTermino { get { return pAnoTermino; } set { pAnoTermino = value; } }

    private string pMes;
    public string Mes { get { return pMes; } set { pMes = value; } }

    private string pAno;
    public string Ano { get { return pAno; } set { pAno = value; } }

    private bool pAprobado;
    public bool Aprobado { get { return pAprobado; } set { pAprobado = value; } }

    private bool pTerminado;
    public bool Terminado { get { return pTerminado; } set { pTerminado = value; } }

    private bool pNoAprobado;
    public bool NoAprobado { get { return pNoAprobado; } set { pNoAprobado = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cContratos()
    {

    }

    public cContratos(ref DBConn oConn)
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
        cSQL.Append("select nkey_cliente, nkey_deudor, num_contrato, no_contrato, tipo_contrato, estado, payment_terms, property_contrato, territorio_contrato, moneda_contrato, fech_inicio, fech_termino, facturado_advance ");
        cSQL.Append("from lic_contratos ");

        if (!string.IsNullOrEmpty(pNkeyCliente))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nkey_cliente = @nkey_cliente");
          oParam.AddParameters("@nkey_cliente", pNkeyCliente, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNkeyDeudor))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nkey_deudor = @nkey_deudor");
          oParam.AddParameters("@nkey_deudor", pNkeyDeudor, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pFacturadoAdvance))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" facturado_advance = @facturado_advance");
          oParam.AddParameters("@facturado_advance", pFacturadoAdvance, TypeSQL.Char);

        }

        if (pAprobado) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" aprobado = 'S' ");
        }

        if (pNoAprobado) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" aprobado <> 'N' ");
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

    public DataTable GetWithDeudor()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select a.nkey_cliente, a.nkey_deudor, ");
        cSQL.Append("(select snombre from deudor where nkey_deudor = a.nkey_deudor) licenciatario, ");
        cSQL.Append("a.num_contrato, a.no_contrato, a.tipo_contrato, a.estado, a.payment_terms, a.property_contrato, a.territorio_contrato, a.moneda_contrato, a.fech_inicio, a.fech_termino, a.facturado_advance, a.aprobado, a.pdfcontrato ");
        cSQL.Append("from lic_contratos a ");

        if (!string.IsNullOrEmpty(pNkeyCliente))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.nkey_cliente = @nkey_cliente");
          oParam.AddParameters("@nkey_cliente", pNkeyCliente, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNkeyDeudor))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.nkey_deudor = @nkey_deudor");
          oParam.AddParameters("@nkey_deudor", pNkeyDeudor, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNoContrato)) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.no_contrato like '%' + @no_contrato + '%' ");
          oParam.AddParameters("@no_contrato", pNoContrato, TypeSQL.Varchar);
        }

        if (!string.IsNullOrEmpty(pFacturadoAdvance))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.facturado_advance = @facturado_advance");
          oParam.AddParameters("@facturado_advance", pFacturadoAdvance, TypeSQL.Char);

        }

        cSQL.Append(" order by licenciatario ");

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

    public DataTable GetForResumen()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " Where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select a.nkey_cliente, a.nkey_deudor, a.num_contrato, a.no_contrato, a.fech_inicio, a.fech_termino, a.facturado_advance, ");
        cSQL.Append("(select snombre from deudor where nkey_deudor = a.nkey_deudor) licenciatario ");
        cSQL.Append("from lic_contratos a ");
        //cSQL.Append("from lic_contratos a where a.aprobado = 'S' ");

        if (!string.IsNullOrEmpty(pNkeyCliente))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.nkey_cliente = @nkey_cliente");
          oParam.AddParameters("@nkey_cliente", pNkeyCliente, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNkeyDeudor))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.nkey_deudor = @nkey_deudor");
          oParam.AddParameters("@nkey_deudor", pNkeyDeudor, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pFacturadoAdvance))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.facturado_advance = @facturado_advance");
          oParam.AddParameters("@facturado_advance", pFacturadoAdvance, TypeSQL.Char);

        }

        if (!string.IsNullOrEmpty(pAnoTermino))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" Year(a.fech_termino) = @ano_termino");
          oParam.AddParameters("@ano_termino", pAnoTermino, TypeSQL.Varchar);

        }

        if (pAprobado)
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.aprobado = 'S' ");
        }

        cSQL.Append(" order by licenciatario ");

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

    public DataTable GetForInvoceAdvance()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select a.nkey_cliente, a.nkey_deudor, (select snombre from deudor where nkey_deudor = a.nkey_deudor) licenciatario, a.num_contrato, a.no_contrato, a.tipo_contrato, a.estado, a.payment_terms, a.property_contrato, a.territorio_contrato, a.moneda_contrato, year(getdate()) as 'periodo', a.fech_inicio, a.fech_termino, a.facturado_advance ");
        if (pIsNullFacturadoAdvance)
          cSQL.Append(", (select sum(valor_original) from lic_advance_contrato where num_contrato = a.num_contrato ) 'monto', fecha_full ");
        cSQL.Append("from lic_contratos a ");

        if (!string.IsNullOrEmpty(pNkeyCliente))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.nkey_cliente = @nkey_cliente");
          oParam.AddParameters("@nkey_cliente", pNkeyCliente, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNkeyDeudor))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.nkey_deudor = @nkey_deudor");
          oParam.AddParameters("@nkey_deudor", pNkeyDeudor, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (pIsNullFacturadoAdvance)
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.facturado_advance is null ");
          cSQL.Append(" and (select sum(valor_original) from lic_advance_contrato where num_contrato = a.num_contrato )  > 0 ");
        }
        else
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.facturado_advance = 'V' ");
        }

        if (pAprobado) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.aprobado = 'S' ");
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

    public DataTable GetReporteVentaNoIngresados() {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select nkey_cliente, nkey_deudor, num_contrato, no_contrato, tipo_contrato, estado, payment_terms, property_contrato, territorio_contrato, moneda_contrato, fech_inicio, fech_termino, facturado_advance ");
        cSQL.Append("from lic_contratos ");

        if (!string.IsNullOrEmpty(pAno))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" year(fech_termino) >= @ano ");
          oParam.AddParameters("@ano", pAno, TypeSQL.Numeric);

        }

        if ((!string.IsNullOrEmpty(pMes))&&(!string.IsNullOrEmpty(pAno)) )
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" and not num_contrato in(select num_contrato from lic_reporte_venta where mes_reporte = @mes and ano_reporte = @ano_reporte) ");
          oParam.AddParameters("@mes", pMes, TypeSQL.Numeric);
          oParam.AddParameters("@ano_reporte", pAno, TypeSQL.Numeric);

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
              break;
            case "EDITAR":
              cSQL = new StringBuilder();
              cSQL.Append("update lic_contratos set ");

              if (pAprobado)
                cSQL.Append(" aprobado = 'S' ");
              else if (pTerminado)
                cSQL.Append(" aprobado = 'T' ");
              else
                cSQL.Append(" facturado_advance = 'V' ");

              cSQL.Append(" where num_contrato = @num_contrato ");
              oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
              oConn.Update(cSQL.ToString(), oParam);

              break;
            case "ELIMINAADVANCE":
              cSQL = new StringBuilder();
              cSQL.Append("update lic_contratos set ");
              cSQL.Append(" facturado_advance = null ");
              cSQL.Append(" where num_contrato = @num_contrato ");
              oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
              oConn.Update(cSQL.ToString(), oParam);

              break;
            case "ELIMINAR":
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
