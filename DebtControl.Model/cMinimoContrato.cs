using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cMinimoContrato
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pNkeyDeudor;
    public string NkeyDeudor { get { return pNkeyDeudor; } set { pNkeyDeudor = value; } }

    private string pNumContrato;
    public string NumContrato { get { return pNumContrato; } set { pNumContrato = value; } }

    private string pNoContrato;
    public string NoContrato { get { return pNoContrato; } set { pNoContrato = value; } }

    private string pFechaInicio;
    public string FechaInicio { get { return pFechaInicio; } set { pFechaInicio = value; } }

    private string pFechaFinal;
    public string FechaFinal { get { return pFechaFinal; } set { pFechaFinal = value; } }

    private string pPeriodo;
    public string Periodo { get { return pPeriodo; } set { pPeriodo = value; } }

    private string pCodMarca;
    public string CodMarca { get { return pCodMarca; } set { pCodMarca = value; } }

    private string pCodCategoria;
    public string CodCategoria { get { return pCodCategoria; } set { pCodCategoria = value; } }

    private string pCodSubCategoria;
    public string CodSubCategoria { get { return pCodSubCategoria; } set { pCodSubCategoria = value; } }

    private string pMinimo;
    public string Minimo { get { return pMinimo; } set { pMinimo = value; } }

    private string pAno;
    public string Ano { get { return pAno; } set { pAno = value; } }

    private string pCodFactShortFall;
    public string CodFactShortFall { get { return pCodFactShortFall; } set { pCodFactShortFall = value; } }

    private bool pbIsNullFact;
    public bool bIsNullFact { get { return pbIsNullFact; } set { pbIsNullFact = value; } }

    private bool pbOrder;
    public bool bOrder { get { return pbOrder; } set { pbOrder = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cMinimoContrato()
    {

    }

    public cMinimoContrato(ref DBConn oConn)
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
        cSQL.Append("select ");
        cSQL.Append(" (select snombre from deudor where nKey_Deudor in( select nKey_Deudor from lic_contratos where num_contrato = a.num_contrato )) licenciatario, ");
        cSQL.Append(" ( select no_contrato from lic_contratos where num_contrato = a.num_contrato ) contrato, ");
        cSQL.Append(" a.num_contrato, a.no_contrato, a.fecha_inicio, a.fecha_final, ");
        cSQL.Append(" a.cod_marca, (select descripcion from lic_marcas where cod_marca = a.cod_marca) marca, ");
        cSQL.Append(" a.cod_categoria, (select descripcion from lic_categorias where cod_categoria = a.cod_categoria) categoria, ");
        cSQL.Append(" a.cod_subcategoria, (select descripcion from lic_subcategoria where cod_subcategoria = a.cod_subcategoria) subcategoria, ");
        cSQL.Append(" a.minimo ");
        cSQL.Append("from lic_minimo_contrato a ");

        if (!string.IsNullOrEmpty(pNkeyDeudor)) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.num_contrato in(select num_contrato from lic_contratos where nKey_Deudor = @nKey_Deudor ) ");
          oParam.AddParameters("@nKey_Deudor", pNkeyDeudor, TypeSQL.Numeric);
        }

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNoContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.no_contrato = @no_contrato");
          oParam.AddParameters("@no_contrato", pNoContrato, TypeSQL.Varchar);

        }

        if (!string.IsNullOrEmpty(pFechaInicio))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.fecha_inicio = @fecha_inicio");
          oParam.AddParameters("@fecha_inicio", pFechaInicio, TypeSQL.Varchar);

        }

        if (!string.IsNullOrEmpty(pFechaFinal))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.fecha_final = @fecha_final");
          oParam.AddParameters("@fecha_final", pFechaFinal, TypeSQL.Varchar);

        }

        if (!string.IsNullOrEmpty(pPeriodo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" Year(a.fecha_final) = @periodo");
          oParam.AddParameters("@periodo", pPeriodo, TypeSQL.Varchar);

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
          cSQL.Append(" a.cod_categoria = @cod_categoria");
          oParam.AddParameters("@cod_categoria", pCodCategoria, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pCodSubCategoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.cod_subcategoria = @cod_subcategoria");
          oParam.AddParameters("@cod_subcategoria", pCodSubCategoria, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pMinimo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.minimo = @minimo");
          oParam.AddParameters("@minimo", pMinimo, TypeSQL.Float);

        }

        if (pbOrder)
          cSQL.Append(" order by licenciatario, fecha_inicio, fecha_final, marca, categoria, subcategoria ");
        else
          cSQL.Append(" order by licenciatario, marca, categoria, subcategoria ");

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

    public DataTable GetPeriodo() { 
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select convert(varchar, fecha_inicio,105) + ' - ' + convert(varchar, fecha_final,105) periodo ");
        cSQL.Append("from lic_minimo_contrato ");

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pNkeyDeudor))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato in(select num_contrato from lic_contratos where nkey_deudor = @nkey_deudor ) ");
          oParam.AddParameters("@nkey_deudor", pNkeyDeudor, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pAno)) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" year(fecha_final) > @ano");
          oParam.AddParameters("@ano", pAno, TypeSQL.Numeric);
        }

        cSQL.Append(" group by fecha_inicio, fecha_final");
        cSQL.Append(" order by periodo ");

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

    public DataTable GetMinimoForShortFall()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " and ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select ");
        cSQL.Append(" (select snombre from deudor where nKey_Deudor in(select nKey_Deudor from lic_contratos where num_contrato = a.num_contrato)) licenciatario, ");
        cSQL.Append("a.num_contrato, a.no_contrato, convert(varchar, a.fecha_inicio,105) dinicio, convert(varchar, a.fecha_final,105) dfinal ");
        cSQL.Append("from lic_minimo_contrato a ");
        cSQL.Append("where a.fecha_final <= getdate() ");

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
        }

        if (pbIsNullFact) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" a.cod_fact_short_fall is null");
        }

        cSQL.Append(" group by a.num_contrato, a.no_contrato, a.fecha_inicio, a.fecha_final ");
        cSQL.Append("order by licenciatario, a.fecha_final, a.num_contrato ");

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
      oParam = new DBConn.SQLParameters(10);
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
              cSQL.Append("update lic_minimo_contrato set");

              cSQL.Append(" cod_fact_short_fall = @cod_fact_short_fall ");
              oParam.AddParameters("@cod_fact_short_fall", pCodFactShortFall, TypeSQL.Numeric);

              cSQL.Append(" where num_contrato = @num_contrato");
              oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

              cSQL.Append(" and convert(varchar,fecha_inicio,112) = @fecha_inicio ");
              oParam.AddParameters("@fecha_inicio", pFechaInicio, TypeSQL.Varchar);

              cSQL.Append(" and convert(varchar,fecha_final,112) = @fecha_final ");
              oParam.AddParameters("@fecha_final", pFechaFinal, TypeSQL.Varchar);

              oConn.Update(cSQL.ToString(), oParam);

              break;

            case "ELIMINAFACTURA":
              cSQL = new StringBuilder();
              cSQL.Append("update lic_minimo_contrato set cod_fact_short_fall = null where cod_fact_short_fall = @cod_fact_short_fall ");
              oParam.AddParameters("@cod_fact_short_fall", pCodFactShortFall, TypeSQL.Numeric);
              oConn.Update(cSQL.ToString(), oParam);

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
