using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cFactura
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pNkeyDeudor;
    public string NkeyDeudor { get { return pNkeyDeudor; } set { pNkeyDeudor = value; } }

    private string pCodFactura;
    public string CodFactura { get { return pCodFactura; } set { pCodFactura = value; } }

    private string pCodReporteVenta;
    public string CodReporteVenta { get { return pCodReporteVenta; } set { pCodReporteVenta = value; } }

    private string pNumInvoice;
    public string NumInvoice { get { return pNumInvoice; } set { pNumInvoice = value; } }

    private string pNumContrato;
    public string NumContrato { get { return pNumContrato; } set { pNumContrato = value; } }

    private string pInvoceDate;
    public string InvoceDate { get { return pInvoceDate; } set { pInvoceDate = value; } }

    private string pProperty;
    public string Property { get { return pProperty; } set { pProperty = value; } }

    private string pTerritory;
    public string Territory { get { return pTerritory; } set { pTerritory = value; } }

    private string pDateInvoce;
    public string DateInvoce { get { return pDateInvoce; } set { pDateInvoce = value; } }

    private string pDueDate;
    public string DueDate { get { return pDueDate; } set { pDueDate = value; } }

    private string pTotal;
    public string Total { get { return pTotal; } set { pTotal = value; } }

    private string pPeriodo;
    public string Periodo { get { return pPeriodo; } set { pPeriodo = value; } }

    private string pPeriodoAno;
    public string PeriodoAno { get { return pPeriodoAno; } set { pPeriodoAno = value; } }

    private string pCodComprobante;
    public string CodComprobante { get { return pCodComprobante; } set { pCodComprobante = value; } }

    private string pTipoFactura;
    public string TipoFactura { get { return pTipoFactura; } set { pTipoFactura = value; } }

    private bool pisNullComprobante;
    public bool isNullComprobante { get { return pisNullComprobante; } set { pisNullComprobante = value; } }

    private string pMes;
    public string Mes { get { return pMes; } set { pMes = value; } }

    private string pAno;
    public string Ano { get { return pAno; } set { pAno = value; } }

    private string pPdfGenerado;
    public string PdfGenerado { get { return pPdfGenerado; } set { pPdfGenerado = value; } }

    private bool pisNullPdfGenerado;
    public bool isNullPdfGenerado { get { return pisNullPdfGenerado; } set { pisNullPdfGenerado = value; } }

    private string pCantQ;
    public string CantQ { get { return pCantQ; } set { pCantQ = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cFactura()
    {

    }

    public cFactura(ref DBConn oConn)
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
        cSQL.Append("select codigo_factura, num_invoice, num_contrato, property, territory, date_invoce, due_date, total, periodo, cod_comprobante, tipo_factura, pdf_generado ");
        cSQL.Append("from lic_factura ");

        if (!string.IsNullOrEmpty(pCodFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" codigo_factura = @codigo_factura");
          oParam.AddParameters("@codigo_factura", pCodFactura, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNumInvoice)) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_invoice = @num_invoice");
          oParam.AddParameters("@num_invoice", pNumInvoice, TypeSQL.Varchar);
        }

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pCodReporteVenta))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_reporte_venta = @cod_reporte_venta");
          oParam.AddParameters("@cod_reporte_venta", pCodReporteVenta, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pPeriodo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" periodo = @periodo");
          oParam.AddParameters("@periodo", pPeriodo, TypeSQL.Varchar);

        }

        if ((!string.IsNullOrEmpty(pPeriodoAno)) && (!string.IsNullOrEmpty(pTipoFactura)))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          if (pTipoFactura == "Q")
          {
            cSQL.Append(" SUBSTRING(periodo, 4, 4) in (" + pPeriodoAno + ") ");
          }
          else {
            cSQL.Append(" SUBSTRING(periodo, 9, 4) in (" + pPeriodoAno + ") ");
          }
        }

        if (!string.IsNullOrEmpty(pCodComprobante))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_comprobante = @cod_comprobante");
          oParam.AddParameters("@cod_comprobante", pCodComprobante, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pTipoFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" tipo_factura = @tipo_factura");
          oParam.AddParameters("@tipo_factura", pTipoFactura, TypeSQL.Char);

        }

        if (pisNullComprobante)
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_comprobante is null");
          cSQL.Append(" order by date_invoce asc ");

        }

        if (!string.IsNullOrEmpty(pPeriodoAno))
        {
          cSQL.Append(" order by date_invoce ");
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

    public DataTable GetByPeriodo() {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select count(*) cantidad from lic_factura ");

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
          cSQL.Append(" periodo in (").Append(pPeriodo).Append(")");

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

    public DataTable GetForMin()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select codigo_factura, num_invoice, num_contrato, property, territory, date_invoce, due_date, total, periodo, cod_comprobante, tipo_factura, pdf_generado ");
        cSQL.Append("from lic_factura ");

        if (!string.IsNullOrEmpty(pCodFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" codigo_factura = @codigo_factura");
          oParam.AddParameters("@codigo_factura", pCodFactura, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNumInvoice))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_invoice = @num_invoice");
          oParam.AddParameters("@num_invoice", pNumInvoice, TypeSQL.Varchar);
        }

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pCodReporteVenta))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_reporte_venta = @cod_reporte_venta");
          oParam.AddParameters("@cod_reporte_venta", pCodReporteVenta, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pPeriodo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" periodo = @periodo");
          oParam.AddParameters("@periodo", pPeriodo, TypeSQL.Varchar);

        }

        if ((!string.IsNullOrEmpty(pPeriodoAno)) && (!string.IsNullOrEmpty(pTipoFactura)))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          if (pTipoFactura == "Q")
          {
            cSQL.Append(" SUBSTRING(periodo, 4, 4) in (" + pPeriodoAno + ") ");
          }
          else
          {
            cSQL.Append(" SUBSTRING(periodo, 9, 4) in (" + pPeriodoAno + ") ");
          }
        }

        if (!string.IsNullOrEmpty(pCodComprobante))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_comprobante = @cod_comprobante");
          oParam.AddParameters("@cod_comprobante", pCodComprobante, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pTipoFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" tipo_factura in ('").Append(pTipoFactura).Append("')");

        }

        if (pisNullComprobante)
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_comprobante is null");
          cSQL.Append(" order by date_invoce asc ");

        }

        if (!string.IsNullOrEmpty(pPeriodoAno))
        {
          cSQL.Append(" order by date_invoce ");
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

    public DataTable GetForMinimo()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select codigo_factura, num_invoice, num_contrato, property, territory, date_invoce, due_date, total, periodo, cod_comprobante, tipo_factura, pdf_generado ");
        cSQL.Append("from lic_factura ");

        if (!string.IsNullOrEmpty(pCodFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" codigo_factura = @codigo_factura");
          oParam.AddParameters("@codigo_factura", pCodFactura, TypeSQL.Numeric);
        }

        if (!string.IsNullOrEmpty(pNumInvoice))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_invoice = @num_invoice");
          oParam.AddParameters("@num_invoice", pNumInvoice, TypeSQL.Varchar);
        }

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
        }

        if (!string.IsNullOrEmpty(pCodReporteVenta))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_reporte_venta = @cod_reporte_venta");
          oParam.AddParameters("@cod_reporte_venta", pCodReporteVenta, TypeSQL.Numeric);
        }

        if (!string.IsNullOrEmpty(pPeriodo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" periodo = @periodo");
          oParam.AddParameters("@periodo", pPeriodo, TypeSQL.Varchar);
        }

        if ((!string.IsNullOrEmpty(pPeriodoAno)) && (!string.IsNullOrEmpty(pTipoFactura)))
        {
          Condicion = " and ";
          if (pTipoFactura == "Q")
          {
            cSQL.Append(Condicion);
            cSQL.Append(" SUBSTRING(periodo, 4, 4) in (" + pPeriodoAno + ") ");
          }
          else if (pTipoFactura == "A") {
            cSQL.Append(Condicion);
            cSQL.Append(" SUBSTRING(periodo, 9, 4) in (" + pPeriodoAno + ") ");
          }
        }

        if (!string.IsNullOrEmpty(pCodComprobante))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_comprobante = @cod_comprobante");
          oParam.AddParameters("@cod_comprobante", pCodComprobante, TypeSQL.Numeric);
        }

        if (!string.IsNullOrEmpty(pTipoFactura))
        {
          Condicion = " and ";
          if (pTipoFactura == "T") {
            cSQL.Append(Condicion);
            cSQL.Append(" tipo_factura in ('Q','A')");
          }
          else{
            cSQL.Append(Condicion);
            cSQL.Append(" tipo_factura in (").Append(pTipoFactura).Append(")");
          }
        }

        if (pisNullComprobante)
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_comprobante is null");
          cSQL.Append(" order by date_invoce asc ");
        }

        if (!string.IsNullOrEmpty(pPeriodoAno))
        {
          cSQL.Append(" order by date_invoce ");
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

    public DataTable GetStatusInvoce()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select a.codigo_factura, a.num_invoice, a.num_contrato, a.property, a.territory, a.date_invoce, a.due_date, a.total, a.periodo, a.cod_comprobante, a.tipo_factura, a.pdf_generado ");
        cSQL.Append("from lic_factura a ");

        if (!string.IsNullOrEmpty(pCodFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.codigo_factura = @codigo_factura");
          oParam.AddParameters("@codigo_factura", pCodFactura, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (pisNullPdfGenerado) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.pdf_generado is null ");
        }

        cSQL.Append(" order by date_invoce desc ");

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

    public DataTable GetSaldoFechaPago() {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";
      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select factura.nNumeroFactura, factura.nkey_factura, isnull((select sum(saldo)  from vista_saldo_factura ");
        cSQL.Append("where vista_saldo_factura.ctatipofact = factura.tipo_factura and factura.nKey_Cliente = vista_saldo_factura.nkey_cliente ");
        cSQL.Append("and factura.nKey_Deudor = vista_saldo_factura.nkey_deudor and factura.NnumeroFactura= vista_saldo_factura.NnumeroFactura),0) 'saldo' ");
        cSQL.Append(",ISNULL((select MAX(dfecharecepcion) from Aplicacion, pago where pago.nKey_pago = Aplicacion.nKey_Pago ");
        cSQL.Append("and Aplicacion.nKey_Factura = factura.nKey_Factura),null) 'fecha_pago' ");
        cSQL.Append("from lic_contratos join lic_factura on (lic_factura.num_contrato = lic_contratos.num_contrato) ");
        cSQL.Append("join factura on (factura.nKey_Cliente = lic_contratos.nkey_cliente ");
        cSQL.Append("and factura.nKey_Deudor = lic_contratos.nkey_deudor ");
        cSQL.Append("and factura.nNumeroFactura = lic_factura.codigo_factura) ");

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" lic_contratos.num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pCodFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" lic_factura.codigo_factura = @codigo_factura");
          oParam.AddParameters("@codigo_factura", pCodFactura, TypeSQL.Numeric);

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

    public DataTable GetStatusInvoceFinanzas()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select (select snombre from deudor where nKey_Deudor in (select nKey_Deudor from lic_contratos where num_contrato = a.num_contrato)) licenciatario, ");
        cSQL.Append(" (select no_contrato from lic_contratos where num_contrato = a.num_contrato) no_contrato, ");
        cSQL.Append(" a.codigo_factura, a.num_invoice, a.num_contrato, a.property, a.territory, a.date_invoce, a.due_date, a.total, a.periodo, a.cod_comprobante, a.tipo_factura, a.pdf_generado ");
        cSQL.Append("from lic_factura a ");

        if (!string.IsNullOrEmpty(pNkeyDeudor)) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.num_contrato in(select num_contrato from lic_contratos where nKey_Deudor = @nKey_Deudor )");
          oParam.AddParameters("@nKey_Deudor", pNkeyDeudor, TypeSQL.Numeric);
        }

        if (!string.IsNullOrEmpty(pCodFactura)) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.codigo_factura = @codigo_factura");
          oParam.AddParameters("@codigo_factura", pCodFactura, TypeSQL.Numeric);
        }

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if ((!string.IsNullOrEmpty(pTipoFactura)) && (pTipoFactura == "S"))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.periodo like '%").Append(pPeriodo).Append("%' and a.tipo_factura = 'S' ");
        }
        else {
          if (!string.IsNullOrEmpty(pPeriodo))
          {
            cSQL.Append(Condicion);
            Condicion = " and ";
            cSQL.Append(" a.periodo = @periodo");
            oParam.AddParameters("@periodo", pPeriodo, TypeSQL.Varchar);
          }
        }

        cSQL.Append(" order by date_invoce desc ");

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

    public DataTable GetFacturasForShortFall() {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select ");
        cSQL.Append("(select snombre from deudor where nKey_Deudor in(select nKey_Deudor from lic_contratos where num_contrato = a.num_contrato)) licenciatario, ");
        cSQL.Append(" a.num_contrato, a.tipo_factura ");
        cSQL.Append("from lic_factura a ");

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if ((!string.IsNullOrEmpty(pPeriodoAno)) && (!string.IsNullOrEmpty(pTipoFactura)))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          if (pTipoFactura == "Q")
          {
            cSQL.Append(" SUBSTRING(a.periodo, 4, 4) in (" + pPeriodoAno + ") ");
          }
          else
          {
            cSQL.Append(" SUBSTRING(a.periodo, 9, 4) in (" + pPeriodoAno + ") ");
          }
        }

        if (!string.IsNullOrEmpty(pTipoFactura))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.tipo_factura = @tipo_factura");
          oParam.AddParameters("@tipo_factura", pTipoFactura, TypeSQL.Char);

        }
        cSQL.Append(" group by a.num_contrato, a.tipo_factura ");

        if (!string.IsNullOrEmpty(pPeriodoAno))
        {
          cSQL.Append(" having count(a.tipo_factura) = " + pCantQ);
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
              cSQL.Append("insert into lic_factura(num_invoice, num_contrato, property, territory, date_invoce, due_date, total, tipo_factura) values(");
              cSQL.Append("@num_invoice, @num_contrato, @property, @territory, @date_invoce, @due_date, @total, @tipo_factura) ");
              oParam.AddParameters("@num_invoice", pNumInvoice, TypeSQL.Numeric);
              oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
              oParam.AddParameters("@invoce_date", pInvoceDate, TypeSQL.DateTime);
              oParam.AddParameters("@property", pProperty, TypeSQL.Varchar);
              oParam.AddParameters("@territory", pTerritory, TypeSQL.Varchar);
              oParam.AddParameters("@date_invoce", pDateInvoce, TypeSQL.DateTime);
              oParam.AddParameters("@due_date", pDueDate, TypeSQL.DateTime);
              oParam.AddParameters("@total", pTotal, TypeSQL.Float);
              oParam.AddParameters("@tipo_factura", pTipoFactura, TypeSQL.Char);
              oConn.Insert(cSQL.ToString(), oParam);

              cSQL = new StringBuilder();
              cSQL.Append("select @@IDENTITY");
              dtData = oConn.Select(cSQL.ToString());
              if (dtData != null)
                if (dtData.Rows.Count > 0)
                  pCodFactura = dtData.Rows[0][0].ToString();
              dtData = null;

              break;

            case "EDITAR":
              cSQL = new StringBuilder();
              cSQL.Append("update lic_factura set ");

              if (!string.IsNullOrEmpty(pNumInvoice))
              {
                cSQL.Append(sComa);
                cSQL.Append(" num_invoice = @num_invoice");
                oParam.AddParameters("@num_invoice", pNumInvoice, TypeSQL.Varchar);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pTotal))
              {
                cSQL.Append(sComa);
                cSQL.Append(" total = @total");
                oParam.AddParameters("@total", pTotal, TypeSQL.Float);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pPeriodo))
              {
                cSQL.Append(sComa);
                cSQL.Append(" periodo = @periodo");
                oParam.AddParameters("@periodo", pPeriodo, TypeSQL.Varchar);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pCodComprobante))
              {
                cSQL.Append(sComa);
                cSQL.Append(" cod_comprobante = @cod_comprobante");
                oParam.AddParameters("@cod_comprobante", pCodComprobante, TypeSQL.Numeric);
                sComa = ", ";
              }
              if (!string.IsNullOrEmpty(pPdfGenerado))
              {
                cSQL.Append(sComa);
                cSQL.Append(" pdf_generado = @pdf_generado");
                oParam.AddParameters("@pdf_generado", pPdfGenerado, TypeSQL.Char);
                sComa = ", ";
              }

              cSQL.Append(" where codigo_factura = @codigo_factura ");
              oParam.AddParameters("@codigo_factura", pCodFactura, TypeSQL.Numeric);
              oConn.Update(cSQL.ToString(), oParam);

              break;
            case "ELIMINAR":
              cSQL = new StringBuilder();
              cSQL.Append("delete from lic_factura where codigo_factura = @codigo_factura");
              oParam.AddParameters("@codigo_factura", pCodFactura, TypeSQL.Numeric);
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
