using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Net;
using System.Net.Mail;

namespace DebtControl.Method
{
  public class ResumenContrato
  {
    private string pLicenciatario;
    public string Licenciatario { get { return pLicenciatario; } set { pLicenciatario = value; } }

    private string pNoContrato;
    public string NoContrato { get { return pNoContrato; } set { pNoContrato = value; } }

    private string pInicio;
    public string Inicio { get { return pInicio; } set { pInicio = value; } }

    private string pFinal;
    public string Final { get { return pFinal; } set { pFinal = value; } }

    private string pPorVencer;
    public string PorVencer { get { return pPorVencer; } set { pPorVencer = value; } }

    private string pMesFechaUno;
    public string MesFechaUno { get { return pMesFechaUno; } set { pMesFechaUno = value; } }

    private string pMesFechaDos;
    public string MesFechaDos { get { return pMesFechaDos; } set { pMesFechaDos = value; } }

    private string pMesFechaTres;
    public string MesFechaTres { get { return pMesFechaTres; } set { pMesFechaTres = value; } }

    private string pNumInvoce;
    public string NumInvoce { get { return pNumInvoce; } set { pNumInvoce = value; } }

    private string pFechFactura;
    public string FechFactura { get { return pFechFactura; } set { pFechFactura = value; } }

    private string pFechComprobante;
    public string FechComprobante { get { return pFechComprobante; } set { pFechComprobante = value; } }

    private DataTable table = new DataTable("ResumenContrato");

    public ResumenContrato()
    {

    }

    public void getMakeTable()
    {
      table.Columns.Add(new DataColumn("Licenciatario", typeof(string)));
      table.Columns.Add(new DataColumn("Contrato", typeof(string)));
      table.Columns.Add(new DataColumn("Inicio", typeof(string)));
      table.Columns.Add(new DataColumn("Termino", typeof(string)));
      table.Columns.Add(new DataColumn("Por vencer", typeof(string)));
      table.Columns.Add(new DataColumn("mesuno", typeof(string)));
      table.Columns.Add(new DataColumn("mesdos", typeof(string)));
      table.Columns.Add(new DataColumn("mestres", typeof(string)));
      table.Columns.Add(new DataColumn("Numero Factura", typeof(string)));
      table.Columns.Add(new DataColumn("Fecha Factura", typeof(string)));
      table.Columns.Add(new DataColumn("Fecha Comprobante", typeof(string)));

    }

    public void AddRow()
    {
      table.Rows.Add(pLicenciatario, pNoContrato, pInicio, pFinal, pPorVencer, pMesFechaUno, pMesFechaDos, pMesFechaTres, pNumInvoce, pFechFactura, pFechComprobante);
    }

    public DataTable Get()
    {
      return table;
    }

  }

  public class MinimoGarantizado
  {
    private bool pViewClient;
    public bool ViewClient { get { return pViewClient; } set { pViewClient = value; } }

    private string pLicenciatario;
    public string Licenciatario { get { return pLicenciatario; } set { pLicenciatario = value; } }

    private string pNoContrato;
    public string NoContrato { get { return pNoContrato; } set { pNoContrato = value; } }

    private string pMarca;
    public string Marca { get { return pMarca; } set { pMarca = value; } }

    private string pCategoria;
    public string Categoria { get { return pCategoria; } set { pCategoria = value; } }

    private string pSubCategoria;
    public string SubCategoria { get { return pSubCategoria; } set { pSubCategoria = value; } }

    private string pMinimoUsd;
    public string MinimoUsd { get { return pMinimoUsd; } set { pMinimoUsd = value; } }

    private string pAdvance;
    public string Advance { get { return pAdvance; } set { pAdvance = value; } }

    private string pPeriodo;
    public string Periodo { get { return pPeriodo; } set { pPeriodo = value; } }

    private string pPeriodoUno;
    public string PeriodoUno { get { return pPeriodoUno; } set { pPeriodoUno = value; } }

    private string pPeriodoDos;
    public string PeriodoDos { get { return pPeriodoDos; } set { pPeriodoDos = value; } }

    private string pPeriodoTres;
    public string PeriodoTres { get { return pPeriodoTres; } set { pPeriodoTres = value; } }

    private string pPeriodoCuatro;
    public string PeriodoCuatro { get { return pPeriodoCuatro; } set { pPeriodoCuatro = value; } }

    private string pSaldo;
    public string Saldo { get { return pSaldo; } set { pSaldo = value; } }

    private DataTable table = new DataTable("MinimoGarantizado");
    private int lngTrimestres = 0;

    public MinimoGarantizado()
    {

    }

    public void getMakeTable()
    {
      Web oWeb = new Web();
      DateTime fecha_inicial = DateTime.Parse(pPeriodo.Substring(0, 10));
      DateTime fecha_final = DateTime.Parse(pPeriodo.Substring(13));

      int lngCantMes = Math.Abs((fecha_final.Month - fecha_inicial.Month) + 12 * (fecha_final.Year - fecha_inicial.Year)) + 1;
      lngTrimestres = lngCantMes / 3;

      if (pViewClient)
      {
        table.Columns.Add(new DataColumn("Licenciatario", typeof(string)));
        table.Columns.Add(new DataColumn("Contrato", typeof(string)));
      }
      table.Columns.Add(new DataColumn("Marca", typeof(string)));
      table.Columns.Add(new DataColumn("Categoría", typeof(string)));
      table.Columns.Add(new DataColumn("Subcategoría", typeof(string)));
      table.Columns.Add(new DataColumn("Mínimo Garantizado USD", typeof(double)));
      table.Columns.Add(new DataColumn("Factura Advance", typeof(double)));

      if (lngTrimestres > 0)
      {
        DateTime dMonthForQ = fecha_inicial;
        for (int i = 0; i < lngTrimestres; i++)
        {
          dMonthForQ = dMonthForQ.AddMonths(2);
          table.Columns.Add(new DataColumn("Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString(), typeof(double)));
          dMonthForQ = dMonthForQ.AddMonths(1);
        }
      }
      else
      {
        DateTime iMonth = fecha_inicial.AddMonths(2);
        table.Columns.Add(new DataColumn("Factura " + oWeb.getPeriodoByNumMonth(iMonth.Month) + "/" + iMonth.Year.ToString(), typeof(double)));
      }
      /*table.Columns.Add(new DataColumn("Factura Q1/" + Periodo, typeof(double)));
      table.Columns.Add(new DataColumn("Factura Q2/" + Periodo, typeof(double)));
      table.Columns.Add(new DataColumn("Factura Q3/" + Periodo, typeof(double)));
      table.Columns.Add(new DataColumn("Factura Q4/" + Periodo, typeof(double)));*/
      table.Columns.Add(new DataColumn("Saldo", typeof(double)));

    }

    public void AddRow()
    {
      if (pViewClient)
      {
        if (lngTrimestres == 1)
          table.Rows.Add(pLicenciatario, pNoContrato, pMarca, pCategoria, pSubCategoria, pMinimoUsd, pAdvance, pPeriodoUno, pSaldo);
        else if (lngTrimestres == 2)
          table.Rows.Add(pLicenciatario, pNoContrato, pMarca, pCategoria, pSubCategoria, pMinimoUsd, pAdvance, pPeriodoUno, pPeriodoDos, pSaldo);
        else if (lngTrimestres == 3)
          table.Rows.Add(pLicenciatario, pNoContrato, pMarca, pCategoria, pSubCategoria, pMinimoUsd, pAdvance, pPeriodoUno, pPeriodoDos, pPeriodoTres, pSaldo);
        else if (lngTrimestres == 4)
          table.Rows.Add(pLicenciatario, pNoContrato, pMarca, pCategoria, pSubCategoria, pMinimoUsd, pAdvance, pPeriodoUno, pPeriodoDos, pPeriodoTres, pPeriodoCuatro, pSaldo);
      }
      else
      {
        if (lngTrimestres == 1)
          table.Rows.Add(pMarca, pCategoria, pSubCategoria, pMinimoUsd, pAdvance, pPeriodoUno, pSaldo);
        else if (lngTrimestres == 2)
          table.Rows.Add(pMarca, pCategoria, pSubCategoria, pMinimoUsd, pAdvance, pPeriodoUno, pPeriodoDos, pSaldo);
        else if (lngTrimestres == 3)
          table.Rows.Add(pMarca, pCategoria, pSubCategoria, pMinimoUsd, pAdvance, pPeriodoUno, pPeriodoDos, pPeriodoTres, pSaldo);
        else if (lngTrimestres == 4)
          table.Rows.Add(pMarca, pCategoria, pSubCategoria, pMinimoUsd, pAdvance, pPeriodoUno, pPeriodoDos, pPeriodoTres, pPeriodoCuatro, pSaldo);
      }
    }

    public DataTable Get()
    {
      return table;
    }

  }

  public class FacturaShortFall
  {
    private bool pViewClient;
    public bool ViewClient { get { return pViewClient; } set { pViewClient = value; } }

    private string pLicenciatario;
    public string Licenciatario { get { return pLicenciatario; } set { pLicenciatario = value; } }

    private string pNoContrato;
    public string NoContrato { get { return pNoContrato; } set { pNoContrato = value; } }

    private string pMarca;
    public string Marca { get { return pMarca; } set { pMarca = value; } }

    private string pCategoria;
    public string Categoria { get { return pCategoria; } set { pCategoria = value; } }

    private string pSubCategoria;
    public string SubCategoria { get { return pSubCategoria; } set { pSubCategoria = value; } }

    private string pMntMinimoGarantizado;
    public string MntMinimoGarantizado { get { return pMntMinimoGarantizado; } set { pMntMinimoGarantizado = value; } }

    private string pPeriodo;
    public string Periodo { get { return pPeriodo; } set { pPeriodo = value; } }

    private string pMntFactAdvance;
    public string MntFactAdvance { get { return pMntFactAdvance; } set { pMntFactAdvance = value; } }

    private string pMntPeriodoFactUno;
    public string MntPeriodoFactUno { get { return pMntPeriodoFactUno; } set { pMntPeriodoFactUno = value; } }

    private string pMntPeriodoFactDos;
    public string MntPeriodoFactDos { get { return pMntPeriodoFactDos; } set { pMntPeriodoFactDos = value; } }

    private string pMntPeriodoFactTres;
    public string MntPeriodoFactTres { get { return pMntPeriodoFactTres; } set { pMntPeriodoFactTres = value; } }

    private string pMntPeriodoFactCuatro;
    public string MntPeriodoFactCuatro { get { return pMntPeriodoFactCuatro; } set { pMntPeriodoFactCuatro = value; } }

    private string pMntPeriodoFactCinco;
    public string MntPeriodoFactCinco { get { return pMntPeriodoFactCinco; } set { pMntPeriodoFactCinco = value; } }

    private string pFacturaUsd;
    public string FacturaUsd { get { return pFacturaUsd; } set { pFacturaUsd = value; } }

    private string pDescuento;

    public string Descuento { get { return pDescuento; } set { pDescuento = value; } }

    private string pFacturaUsdCorregida;
    public string FacturaUsdCorregida { get { return pFacturaUsdCorregida; } set { pFacturaUsdCorregida = value; } }

    private DataTable table = new DataTable("FacturaShortFall");
    private int lngTrimestres = 0;

    public FacturaShortFall()
    {

    }

    public void getMakeTable()
    {
      Web oWeb = new Web();
      DateTime fecha_inicial = DateTime.Parse(pPeriodo.Substring(0, 10));
      DateTime fecha_final = DateTime.Parse(pPeriodo.Substring(13));

      int lngCantMes = Math.Abs((fecha_final.Month - fecha_inicial.Month) + 12 * (fecha_final.Year - fecha_inicial.Year)) + 1;
      lngTrimestres = lngCantMes / 3;

      if (pViewClient)
      {
        table.Columns.Add(new DataColumn("Licenciatario", typeof(string)));
        table.Columns.Add(new DataColumn("Contrato", typeof(string)));
      }
      table.Columns.Add(new DataColumn("Marca", typeof(string)));
      table.Columns.Add(new DataColumn("Categoría", typeof(string)));
      table.Columns.Add(new DataColumn("Subcategoría", typeof(string)));
      table.Columns.Add(new DataColumn("Mínimo Garantizado USD", typeof(double)));
      table.Columns.Add(new DataColumn("Periodo", typeof(string)));
      table.Columns.Add(new DataColumn("Factura Advance", typeof(double)));

      if (lngTrimestres > 0)
      {
        DateTime dMonthForQ = fecha_inicial;
        for (int i = 0; i < lngTrimestres; i++)
        {
          dMonthForQ = dMonthForQ.AddMonths(2);
          table.Columns.Add(new DataColumn("Factura " + oWeb.getPeriodoByNumMonth(dMonthForQ.Month) + "/" + dMonthForQ.Year.ToString(), typeof(double)));
          dMonthForQ = dMonthForQ.AddMonths(1);
        }
      }
      else
      {
        DateTime iMonth = fecha_inicial.AddMonths(2);
        table.Columns.Add(new DataColumn("Factura " + oWeb.getPeriodoByNumMonth(iMonth.Month) + "/" + iMonth.Year.ToString(), typeof(double)));
      }

      table.Columns.Add(new DataColumn("Factura USD", typeof(double)));
      table.Columns.Add(new DataColumn("Factura Prueba", typeof(double)));
      table.Columns.Add(new DataColumn("Descuento", typeof(double)));
      table.Columns.Add(new DataColumn("Factura USD Corregida", typeof(double)));

    }

    public void AddRow()
    {
      if (pViewClient)
      {
        if (lngTrimestres == 1)
          table.Rows.Add(pLicenciatario, pNoContrato, pMarca, pCategoria, pSubCategoria, pMntMinimoGarantizado, pPeriodo, pMntFactAdvance, pMntPeriodoFactUno, pFacturaUsd, pMntPeriodoFactCinco, pDescuento, pFacturaUsdCorregida);
        else if (lngTrimestres == 2)
          table.Rows.Add(pLicenciatario, pNoContrato, pMarca, pCategoria, pSubCategoria, pMntMinimoGarantizado, pPeriodo, pMntFactAdvance, pMntPeriodoFactUno, pMntPeriodoFactDos, pFacturaUsd, pMntPeriodoFactCinco, pDescuento, pFacturaUsdCorregida);
        else if (lngTrimestres == 3)
          table.Rows.Add(pLicenciatario, pNoContrato, pMarca, pCategoria, pSubCategoria, pMntMinimoGarantizado, pPeriodo, pMntFactAdvance, pMntPeriodoFactUno, pMntPeriodoFactDos, pMntPeriodoFactTres, pFacturaUsd, pMntPeriodoFactCinco, pDescuento, pFacturaUsdCorregida);
        else if (lngTrimestres == 4)
          table.Rows.Add(pLicenciatario, pNoContrato, pMarca, pCategoria, pSubCategoria, pMntMinimoGarantizado, pPeriodo, pMntFactAdvance, pMntPeriodoFactUno, pMntPeriodoFactDos, pMntPeriodoFactTres, pMntPeriodoFactCuatro, pFacturaUsd, pMntPeriodoFactCinco, pDescuento, pFacturaUsdCorregida);
      }
      else
      {
        if (lngTrimestres == 1)
          table.Rows.Add(pMarca, pCategoria, pSubCategoria, pMntMinimoGarantizado, pPeriodo, pMntFactAdvance, pMntPeriodoFactUno, pFacturaUsd, pMntPeriodoFactCinco, pDescuento, pFacturaUsdCorregida);
        else if (lngTrimestres == 2)
          table.Rows.Add(pMarca, pCategoria, pSubCategoria, pMntMinimoGarantizado, pPeriodo, pMntFactAdvance, pMntPeriodoFactUno, pMntPeriodoFactDos, pFacturaUsd, pMntPeriodoFactCinco, pDescuento, pFacturaUsdCorregida);
        else if (lngTrimestres == 3)
          table.Rows.Add(pMarca, pCategoria, pSubCategoria, pMntMinimoGarantizado, pPeriodo, pMntFactAdvance, pMntPeriodoFactUno, pMntPeriodoFactDos, pMntPeriodoFactTres, pFacturaUsd, pMntPeriodoFactCinco, pDescuento, pFacturaUsdCorregida);
        else if (lngTrimestres == 4)
          table.Rows.Add(pMarca, pCategoria, pSubCategoria, pMntMinimoGarantizado, pPeriodo, pMntFactAdvance, pMntPeriodoFactUno, pMntPeriodoFactDos, pMntPeriodoFactTres, pMntPeriodoFactCuatro, pFacturaUsd, pMntPeriodoFactCinco, pDescuento, pFacturaUsdCorregida);
      }
    }

    public DataTable Get()
    {
      return table;
    }
  }

  public class viewFacturaShortFall
  {
    private string pLicenciatario;
    public string Licenciatario { get { return pLicenciatario; } set { pLicenciatario = value; } }

    private string pNumContrato;
    public string NumContrato { get { return pNumContrato; } set { pNumContrato = value; } }

    private string pNoContrato;
    public string NoContrato { get { return pNoContrato; } set { pNoContrato = value; } }

    private string pInicio;
    public string Inicio { get { return pInicio; } set { pInicio = value; } }

    private string pFinal;
    public string Final { get { return pFinal; } set { pFinal = value; } }

    private DataTable table = new DataTable("viewFacturaShortFall");

    public viewFacturaShortFall()
    {

    }

    public void getMakeTable() {
      table.Columns.Add(new DataColumn("licenciatario", typeof(string)));
      table.Columns.Add(new DataColumn("num_contrato", typeof(string)));
      table.Columns.Add(new DataColumn("contrato", typeof(string)));
      table.Columns.Add(new DataColumn("dinicio", typeof(string)));
      table.Columns.Add(new DataColumn("dfinal", typeof(string)));
    }

    public void AddRow()
    {
      table.Rows.Add(pLicenciatario, pNumContrato, pNoContrato, pInicio, pFinal);
    }

    public DataTable Get()
    {
      return table;
    }

  }


  public class FacturaAdvance
  {

    private string pCodMarca;
    public string CodMarca { get { return pCodMarca; } set { pCodMarca = value; } }

    private string pMarca;
    public string Marca { get { return pMarca; } set { pMarca = value; } }

    private string pCodCategoria;
    public string CodCategoria { get { return pCodCategoria; } set { pCodCategoria = value; } }

    private string pCategoria;
    public string Categoria { get { return pCategoria; } set { pCategoria = value; } }

    private string pCodSubCategoria;
    public string CodSubCategoria { get { return pCodSubCategoria; } set { pCodSubCategoria = value; } }

    private string pSubCategoria;
    public string SubCategoria { get { return pSubCategoria; } set { pSubCategoria = value; } }

    private string pAdvanceUsd;
    public string AdvanceUsd { get { return pAdvanceUsd; } set { pAdvanceUsd = value; } }

    private DataTable table = new DataTable("FacturaAdvance");

    public FacturaAdvance()
    {

    }

    public void getMakeTable()
    {

      table.Columns.Add(new DataColumn("CodMarca", typeof(string)));
      table.Columns.Add(new DataColumn("Marca", typeof(string)));
      table.Columns.Add(new DataColumn("CodCategoria", typeof(string)));
      table.Columns.Add(new DataColumn("Categoria", typeof(string)));
      table.Columns.Add(new DataColumn("CodSubCategoria", typeof(string)));
      table.Columns.Add(new DataColumn("SubCategoria", typeof(string)));
      table.Columns.Add(new DataColumn("Advance USD", typeof(double)));

    }

    public void AddRow()
    {

      table.Rows.Add(pCodMarca, pMarca, pCodCategoria, pCategoria, pCodSubCategoria, pSubCategoria, pAdvanceUsd);

    }

    public DataTable Get()
    {
      return table;
    }

  }

  public class detFactura
  {

    private string pMesNomUno;
    public string MesNomUno { get { return pMesNomUno; } set { pMesNomUno = value; } }

    private string pMesNomDos;
    public string MesNomDos { get { return pMesNomDos; } set { pMesNomDos = value; } }

    private string pMesNomTres;
    public string MesNomTres { get { return pMesNomTres; } set { pMesNomTres = value; } }

    private string pMarca;
    public string Marca { get { return pMarca; } set { pMarca = value; } }

    private string pCategoria;
    public string Categoria { get { return pCategoria; } set { pCategoria = value; } }

    private string pSubCategoria;
    public string SubCategoria { get { return pSubCategoria; } set { pSubCategoria = value; } }

    private string pMesUno;
    public string MesUno { get { return pMesUno; } set { pMesUno = value; } }

    private string pMesDos;
    public string MesDos { get { return pMesDos; } set { pMesDos = value; } }

    private string pMesTres;
    public string MesTres { get { return pMesTres; } set { pMesTres = value; } }

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

    private string pMontoDbiUsd;
    public string MontoDbiUsd { get { return pMontoDbiUsd; } set { pMontoDbiUsd = value; } }

    private string pSaldoAdvanceUsd;
    public string SaldoAdvanceUsd { get { return pSaldoAdvanceUsd; } set { pSaldoAdvanceUsd = value; } }

    private string pSaldo;
    public string Saldo { get { return pSaldo; } set { pSaldo = value; } }

    private string pFacturaUsd;
    public string FacturaUsd { get { return pFacturaUsd; } set { pFacturaUsd = value; } }

    private DataTable table = new DataTable("detFactura");

    public detFactura()
    {

    }

    public void getMakeTable()
    {

      table.Columns.Add(new DataColumn("Marca", typeof(string)));
      table.Columns.Add(new DataColumn("Categoría", typeof(string)));
      table.Columns.Add(new DataColumn("Subcategoría", typeof(string)));
      table.Columns.Add(new DataColumn(pMesNomUno, typeof(double)));
      table.Columns.Add(new DataColumn(pMesNomDos, typeof(double)));
      table.Columns.Add(new DataColumn(pMesNomTres, typeof(double)));
      table.Columns.Add(new DataColumn("CodRoyalty", typeof(double)));
      table.Columns.Add(new DataColumn("Royalty (%)", typeof(double)));
      table.Columns.Add(new DataColumn("BDI (%)", typeof(double)));
      table.Columns.Add(new DataColumn("Periodo", typeof(string)));
      table.Columns.Add(new DataColumn("Monto Royalty USD", typeof(double)));
      table.Columns.Add(new DataColumn("Monto BDI USD", typeof(double)));
      table.Columns.Add(new DataColumn("Saldo Advance USD", typeof(double)));
      table.Columns.Add(new DataColumn("Saldo", typeof(double)));
      table.Columns.Add(new DataColumn("Factura USD", typeof(double)));

    }

    public void AddRow()
    {

      table.Rows.Add(pMarca, pCategoria, pSubCategoria, pMesUno, pMesDos, pMesTres, pCodRoyalty, pRoyalty, pBdi, pPeriodo, pMontoRoyaltyUsd, pMontoDbiUsd, pSaldoAdvanceUsd, pSaldo, pFacturaUsd);

    }

    public DataTable Get()
    {
      return table;
    }

  }

  public class Usuario
  {
    private string pCodUsuario = string.Empty;
    public string CodUsuario { get { return pCodUsuario; } set { pCodUsuario = value; } }

    private string pNKeyUsuario = string.Empty;
    public string NKeyUsuario { get { return pNKeyUsuario; } set { pNKeyUsuario = value; } }

    private string pNombres = string.Empty;
    public string Nombres { get { return pNombres; } set { pNombres = value; } }

    private string pPais = string.Empty;
    public string Pais { get { return pPais; } set { pPais = value; } }

    private string pTipo = string.Empty;
    public string Tipo { get { return pTipo; } set { pTipo = value; } }

    private string pEmail = string.Empty;
    public string Email { get { return pEmail; } set { pEmail = value; } }

    private string pFono = string.Empty;
    public string Fono { get { return pFono; } set { pFono = value; } }

    private string pImagen = string.Empty;
    public string Imagen { get { return pImagen; } set { pImagen = value; } }

    private string pVistaMenu = string.Empty;
    public string VistaMenu { get { return pVistaMenu; } set { pVistaMenu = value; } }

    private string pCodTipoUsuario = string.Empty;
    public string CodTipoUsuario { get { return pCodTipoUsuario; } set { pCodTipoUsuario = value; } }

    private string pRutUsuario = string.Empty;
    public string RutUsuario { get { return pRutUsuario; } set { pRutUsuario = value; } }

    private string pNKeyDeudor = string.Empty;
    public string NKeyDeudor { get { return pNKeyDeudor; } set { pNKeyDeudor = value; } }

    private string pRutLicenciatario = string.Empty;
    public string RutLicenciatario { get { return pRutLicenciatario; } set { pRutLicenciatario = value; } }

    private string pLicenciatario = string.Empty;
    public string Licenciatario { get { return pLicenciatario; } set { pLicenciatario = value; } }

    public Usuario()
    {
    }

  }

  public class Web
  {
    public string getMes(int numeroMes)
    {
      try
      {
        DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
        string nombreMes = formatoFecha.GetMonthName(numeroMes);
        return nombreMes;
      }
      catch
      {
        return "Desconocido";
      }
    }

    public void getMesesDdlist(DropDownList oDdlist, int i)
    {
      oDdlist.Items.Add(new ListItem("<< Selecciona Mes >>", ""));
      switch (i)
      {
        case 1:
        case 2:
        case 3:
          oDdlist.Items.Add(new ListItem(getMes(1), "1"));
          oDdlist.Items.Add(new ListItem(getMes(2), "2"));
          oDdlist.Items.Add(new ListItem(getMes(3), "3"));
          break;
        case 4:
        case 5:
        case 6:
          oDdlist.Items.Add(new ListItem(getMes(4), "4"));
          oDdlist.Items.Add(new ListItem(getMes(5), "5"));
          oDdlist.Items.Add(new ListItem(getMes(6), "6"));
          break;
        case 7:
        case 8:
        case 9:
          oDdlist.Items.Add(new ListItem(getMes(7), "7"));
          oDdlist.Items.Add(new ListItem(getMes(8), "8"));
          oDdlist.Items.Add(new ListItem(getMes(9), "9"));
          break;
        case 10:
        case 11:
        case 12:
          oDdlist.Items.Add(new ListItem(getMes(10), "10"));
          oDdlist.Items.Add(new ListItem(getMes(11), "11"));
          oDdlist.Items.Add(new ListItem(getMes(12), "12"));
          break;
      }
    }

    public string getPeriodoVenta(string sMes)
    {
      string sPeriodo = string.Empty;
      switch (sMes)
      {
        case "ENERO":
        case "FEBRERO":
        case "MARZO":
          sPeriodo = "Q1";
          break;
        case "ABRIL":
        case "MAYO":
        case "JUNIO":
          sPeriodo = "Q2";
          break;
        case "JULIO":
        case "AGOSTO":
        case "SEPTIEMBRE":
          sPeriodo = "Q3";
          break;
        case "OCTUBRE":
        case "NOVIEMBRE":
        case "DICIEMBRE":
          sPeriodo = "Q4";
          break;
      }
      return sPeriodo;
    }

    public string getPeriodoByNumMonth(int sMes)
    {
      string sPeriodo = string.Empty;
      switch (sMes)
      {
        case 1:
        case 2:
        case 3:
          sPeriodo = "Q1";
          break;
        case 4:
        case 5:
        case 6:
          sPeriodo = "Q2";
          break;
        case 7:
        case 8:
        case 9:
          sPeriodo = "Q3";
          break;
        case 10:
        case 11:
        case 12:
          sPeriodo = "Q4";
          break;
      }
      return sPeriodo;
    }

    public string getPeriodoMesesQ(string sQ)
    {
      string sPeriodo = "";

      switch (sQ)
      {
        case "Q1":
          sPeriodo = "1,2,3";
          break;
        case "Q2":
          sPeriodo = "4,5,6";
          break;
        case "Q3":
          sPeriodo = "7,8,9";
          break;
        case "Q4":
          sPeriodo = "10,11,12";
          break;
      }
      return sPeriodo;
    }

    public Usuario GetObjUsuario()
    {
      Usuario oIsUsuario;
      if ((HttpContext.Current.Session["USUARIO"] != null) && (!string.IsNullOrEmpty(HttpContext.Current.Session["USUARIO"].ToString())))
      {
        oIsUsuario = (Usuario)HttpContext.Current.Session["USUARIO"];
      }
      else
      {
        oIsUsuario = new Usuario();
      }
      return oIsUsuario;

    }

    public Usuario GetObjAdmUsuario()
    {
      Usuario oIsUsuario;
      if ((HttpContext.Current.Session["Administrador"] != null) && (!string.IsNullOrEmpty(HttpContext.Current.Session["Administrador"].ToString())))
      {
        oIsUsuario = (Usuario)HttpContext.Current.Session["Administrador"];
      }
      else
      {
        oIsUsuario = new Usuario();
      }
      return oIsUsuario;

    }

    public string GetIpUsuario()
    {
      string sIpUsuario = ((HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) && (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString())) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString() : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString());
      sIpUsuario = ((!string.IsNullOrEmpty(sIpUsuario) && (sIpUsuario != "::1")) ? sIpUsuario : string.Empty);

      return sIpUsuario;
    }

    public void ValidaSession()
    {
      if ((HttpContext.Current.Session["USUARIO"] == null) || (HttpContext.Current.Session["Administrador"] == null) || (string.IsNullOrEmpty(HttpContext.Current.Session["USUARIO"].ToString())) || (string.IsNullOrEmpty(HttpContext.Current.Session["USUARIO"].ToString())))
      {
        HttpContext.Current.Response.Redirect("redirection.htm");
      }
    }

    public string GetDatRefreshPage()
    {
      AppSettingsReader appReader = new AppSettingsReader();
      return appReader.GetValue("setuprefreshpage", typeof(string)).ToString();
    }

    public string GetData(string sData)
    {
      string sRetorno = String.Empty;

      try
      {
        if (HttpContext.Current.Request.Form.Count != 0)
        {
          sRetorno = Convert.ToString(HttpContext.Current.Request.Form[sData]);
        }
        else if (HttpContext.Current.Request.QueryString.Count != 0)
        {
          sRetorno = Convert.ToString(HttpContext.Current.Request.QueryString[sData]);
        }
        return sRetorno;
      }
      catch { return sRetorno; }
    }

    public string GetSession(string sData)
    {
      string sRetorno = string.Empty;
      if (HttpContext.Current.Session[sData] != null)
        sRetorno = HttpContext.Current.Session[sData].ToString();

      return sRetorno;
    }

    public bool ValidaMail(string sMail)
    {
      bool bSuccess = false;
      Regex r = new Regex("^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
      //Regex r = new Regex("/^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,4})+$/", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
      bSuccess = r.Match(sMail).Success;

      return bSuccess;
    }

    public DataTable DeserializarTbl(string cPath, string cFile)
    {
      if (!string.IsNullOrEmpty(cPath))
      {

        StringBuilder oFolder = new StringBuilder();
        oFolder.Append(cPath);
        oFolder.Append(@"\binary\");
        if (File.Exists(oFolder.ToString() + cFile))
        {
          IFormatter oBinFormat = new BinaryFormatter();
          Stream oFileStream = new FileStream(oFolder.ToString() + cFile, FileMode.Open, FileAccess.Read, FileShare.Read);
          DataTable oData = (DataTable)oBinFormat.Deserialize(oFileStream);
          oFileStream.Close();
          return oData;
        }
        return new DataTable();
      }
      return new DataTable();
    }

    public byte[] GetImageBytes(Stream stream)
    {
      byte[] buffer;

      using (Bitmap image = ResizeImage(stream))
      {
        using (MemoryStream ms = new MemoryStream())
        {
          image.Save(ms, ImageFormat.Png);

          //return the current position in the stream at the beginning
          ms.Position = 0;

          buffer = new byte[ms.Length];
          ms.Read(buffer, 0, (int)ms.Length);
          return buffer;
        }
      }
    }

    public byte[] GetImageBytes(Stream stream, int height, int width)
    {
      byte[] buffer;

      using (Bitmap image = ResizeImage(stream, height, width))
      {
        using (MemoryStream ms = new MemoryStream())
        {
          image.Save(ms, ImageFormat.Png);

          //return the current position in the stream at the beginning
          ms.Position = 0;

          buffer = new byte[ms.Length];
          ms.Read(buffer, 0, (int)ms.Length);
          return buffer;
        }
      }
    }

    public Bitmap ResizeImage(Stream stream)
    {
      System.Drawing.Image originalImage = Bitmap.FromStream(stream);

      Bitmap scaledImage = new Bitmap(originalImage.Width, originalImage.Height);

      using (Graphics g = Graphics.FromImage(scaledImage))
      {
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);

        return scaledImage;
      }

    }

    public Bitmap ResizeImage(Stream stream, int height, int width)
    {

      System.Drawing.Image originalImage = Bitmap.FromStream(stream);

      //int height = 300;
      //int width = 300;

      double ratio = Math.Min(originalImage.Width, originalImage.Height) / (double)Math.Max(originalImage.Width, originalImage.Height);

      if (originalImage.Width > originalImage.Height)
      {
        height = Convert.ToInt32(height * ratio);
      }
      else
      {
        width = Convert.ToInt32(width * ratio);
      }

      Bitmap scaledImage = new Bitmap(width, height);

      using (Graphics g = Graphics.FromImage(scaledImage))
      {
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.DrawImage(originalImage, 0, 0, width, height);

        return scaledImage;
      }

    }

    #region Encriptar
    /// <summary>
    /// Método para encriptar un texto plano usando el algoritmo (Rijndael).
    /// Este es el mas simple posible, muchos de los datos necesarios los
    /// definimos como constantes.
    /// </summary>
    /// <param name="textoQueEncriptaremos">texto a encriptar</param>
    /// <returns>Texto encriptado</returns>
    public string Crypt(string textoQueEncriptaremos)
    {
      return Crypt(textoQueEncriptaremos, "icommunity75dc@avz10", "s@lAvz", "MD5", 1, "@1B2c3D4e5F6g7H8", 128);
    }
    /// <summary>
    /// Método para encriptar un texto plano usando el algoritmo (Rijndael)
    /// </summary>
    /// <returns>Texto encriptado</returns>
    public string Crypt(string textoQueEncriptaremos, string passBase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
    {
      byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
      byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
      byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoQueEncriptaremos);
      PasswordDeriveBytes password = new PasswordDeriveBytes(passBase, saltValueBytes, hashAlgorithm, passwordIterations);
      byte[] keyBytes = password.GetBytes(keySize / 8);
      RijndaelManaged symmetricKey = new RijndaelManaged()
      {
        Mode = CipherMode.CBC
      };
      ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
      cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
      cryptoStream.FlushFinalBlock();
      byte[] cipherTextBytes = memoryStream.ToArray();
      memoryStream.Close();
      cryptoStream.Close();
      string cipherText = Convert.ToBase64String(cipherTextBytes);
      return cipherText;
    }
    #endregion

    #region Desencriptar
    /// <summary>
    /// Método para desencriptar un texto encriptado.
    /// </summary>
    /// <returns>Texto desencriptado</returns>
    public string UnCrypt(string textoEncriptado)
    {
      return UnCrypt(textoEncriptado, "icommunity75dc@avz10", "s@lAvz", "MD5", 1, "@1B2c3D4e5F6g7H8", 128);
    }
    /// <summary>
    /// Método para desencriptar un texto encriptado (Rijndael)
    /// </summary>
    /// <returns>Texto desencriptado</returns>
    public string UnCrypt(string textoEncriptado, string passBase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
    {
      byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
      byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
      byte[] cipherTextBytes = Convert.FromBase64String(textoEncriptado);
      PasswordDeriveBytes password = new PasswordDeriveBytes(passBase, saltValueBytes, hashAlgorithm, passwordIterations);
      byte[] keyBytes = password.GetBytes(keySize / 8);
      RijndaelManaged symmetricKey = new RijndaelManaged()
      {
        Mode = CipherMode.CBC
      };
      ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
      MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
      CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
      byte[] plainTextBytes = new byte[cipherTextBytes.Length];
      int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
      memoryStream.Close();
      cryptoStream.Close();
      string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
      return plainText;
    }
    #endregion

    public string getColorArrowCalidad(double dValorActual, double dValueAnterior)
    {

      if (dValorActual < dValueAnterior)
      {
        return "images/flechas/flecharojaabajo.jpg";
      }
      else if (dValorActual > dValueAnterior)
      {
        return "images/flechas/flechaverdearriba.jpg";
      }
      else
      {
        return "images/flechas/flechaverdederecha.jpg";
      }
    }

    public string getColorArrowImpecabilidad(double dValorActual, double dValueAnterior)
    {
      if (dValorActual < dValueAnterior)
      {
        return "images/flechas/flechaverdeabajo.jpg";
      }
      else if (dValorActual > dValueAnterior)
      {
        return "images/flechas/flecharojaarriba.jpg";
      }
      else
      {
        return "images/flechas/flechaverdederecha.jpg";
      }
    }

    public string getColor(string sColor)
    {
      string sDato = string.Empty;
      switch (sColor)
      {
        case "V":
          sDato = "tbDatVerde";
          break;
        case "A":
          sDato = "tbDatAmarilla";
          break;
        case "R":
          sDato = "tbDatRojo";
          break;
      }
      return sDato;
    }

    public string getColorNumAvance(double dValor, DataTable dtValores)
    {
      if (dValor <= double.Parse(dtValores.Rows[0]["valor_bajo"].ToString()))
      {
        return getColor(dtValores.Rows[0]["colorbajo"].ToString());
      }
      else if ((dValor > double.Parse(dtValores.Rows[0]["valor_bajo"].ToString())) && (dValor < double.Parse(dtValores.Rows[0]["valor_alto"].ToString())))
      {
        return getColor(dtValores.Rows[0]["colormedio"].ToString());
      }
      else
      {
        return getColor(dtValores.Rows[0]["coloralto"].ToString());
      }
    }

    public string fUnCrypt(string txtInp)
    {
      string txtOut = string.Empty;
      char strCaracter;
      int lngCodigo = 0;
      int lnPos = 1;
      int Acum = 0;
      char cDato = ' ';

      if (txtInp.Length > 0)
      {
        //Acum = Asc(Mid(txtInp, 1, 1)) / 2;
        cDato = Convert.ToChar(txtInp.Substring(0, 1));
        Acum = ((int)cDato) / 2;
        do
        {
          //lngCodigo = Asc(Mid(txtInp, lnPos, 1)) - Acum
          lngCodigo = (int)(Convert.ToChar(txtInp.Substring(lnPos, 1))) - Acum;
          if (lngCodigo < 1)
          {
            lngCodigo = lngCodigo + 255;
          }
          strCaracter = (char)lngCodigo;
          txtOut = txtOut + strCaracter.ToString();
          //Acum = (Asc(Mid(txtInp, lnPos, 1)) - Acum) + Acum;
          Acum = (int)(Convert.ToChar(txtInp.Substring(lnPos, 1))) + Acum;
          lnPos++;
        } while (lnPos <= txtInp.Length);
      }

      return txtOut;
    }
  }

  public class Emailing
  {
    private string pFrom;
    public string From { get { return pFrom; } set { pFrom = value; } }

    private string pFromName;
    public string FromName { get { return pFromName; } set { pFromName = value; } }

    private string pAddress;
    public string Address { get { return pAddress; } set { pAddress = value; } }

    private string pSubject;
    public string Subject { get { return pSubject; } set { pSubject = value; } }

    private string pError;
    public string Error { get { return pError; } set { pError = value; } }

    private StringBuilder pBody;
    public StringBuilder Body { get { return pBody; } set { pBody = value; } }

    public bool EmailSend()
    {
      bool bSend = false;
      MailMessage oMail = new MailMessage();
      oMail.IsBodyHtml = true;
      oMail.From = new MailAddress(pFrom, pFromName);
      oMail.To.Add(pAddress);
      oMail.Subject = pSubject;
      oMail.Body = pBody.ToString();
      try
      {
        SmtpClient oStmpClient = new SmtpClient();
        oStmpClient.Host = HttpContext.Current.Application["SmtpServer"].ToString();
        oStmpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        oStmpClient.UseDefaultCredentials = true;
        if ((HttpContext.Current.Application["PortSmtpServer"] != null) && (!string.IsNullOrEmpty(HttpContext.Current.Application["PortSmtpServer"].ToString())))
          oStmpClient.Port = int.Parse(HttpContext.Current.Application["PortSmtpServer"].ToString());
        if ((HttpContext.Current.Application["UserSmtp"] != null) && (!string.IsNullOrEmpty(HttpContext.Current.Application["UserSmtp"].ToString())))
          oStmpClient.Credentials = new NetworkCredential(HttpContext.Current.Application["UserSmtp"].ToString(), HttpContext.Current.Application["PwdSmtp"].ToString());
        oStmpClient.Send(oMail);
        bSend = true;
      }
      catch (Exception e)
      {
        Error = e.Message;
      }
      return bSend;
    }
  }

  public class EmailingTest
  {
    private string pHost;
    public string Host { get { return pHost; } set { pHost = value; } }

    private string pPort;
    public string Port { get { return pPort; } set { pPort = value; } }

    private string pUserSmtp;
    public string UserSmtp { get { return pUserSmtp; } set { pUserSmtp = value; } }

    private string pPwdSmtp;
    public string PwdSmtp { get { return pPwdSmtp; } set { pPwdSmtp = value; } }

    private string pFrom;
    public string From { get { return pFrom; } set { pFrom = value; } }

    private string pFromName;
    public string FromName { get { return pFromName; } set { pFromName = value; } }

    private string pAddress;
    public string Address { get { return pAddress; } set { pAddress = value; } }

    private string pSubject;
    public string Subject { get { return pSubject; } set { pSubject = value; } }

    private string pError;
    public string Error { get { return pError; } set { pError = value; } }

    private StringBuilder pBody;
    public StringBuilder Body { get { return pBody; } set { pBody = value; } }

    public bool EmailSend()
    {
      bool bSend = false;
      MailMessage oMail = new MailMessage();
      oMail.IsBodyHtml = true;
      oMail.From = new MailAddress(pFrom, pFromName);
      oMail.To.Add(pAddress);
      oMail.Subject = pSubject;
      oMail.Body = pBody.ToString();
      try
      {
        SmtpClient oStmpClient = new SmtpClient();
        oStmpClient.Host = pHost;
        oStmpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        oStmpClient.UseDefaultCredentials = true;
        if (!string.IsNullOrEmpty(pPort))
          oStmpClient.Port = int.Parse(pPort);
        if (!string.IsNullOrEmpty(pUserSmtp))
          oStmpClient.Credentials = new NetworkCredential(pUserSmtp, PwdSmtp);
        oStmpClient.Send(oMail);
        bSend = true;
      }
      catch (Exception e)
      {
        Error = e.Message;
      }
      return bSend;
    }
  }
}
