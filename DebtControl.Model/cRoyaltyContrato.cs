using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cRoyaltyContrato
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pNumContrato;
    public string NumContrato { get { return pNumContrato; } set { pNumContrato = value; } }

    private string pNoContrato;
    public string NoContrato { get { return pNoContrato; } set { pNoContrato = value; } }

    private string pCodRoyalty;
    public string CodRoyalty { get { return pCodRoyalty; } set { pCodRoyalty = value; } }

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

    private bool pAsqMarcaNull;
    public bool AsqMarcaNull { get { return pAsqMarcaNull; } set { pAsqMarcaNull = value; } }

    private bool pAsqCategoriaNull;
    public bool AsqCategoriaNull { get { return pAsqCategoriaNull; } set { pAsqCategoriaNull = value; } }

    private bool pAsqSubCategoriaNull;
    public bool AsqSubCategoriaNull { get { return pAsqSubCategoriaNull; } set { pAsqSubCategoriaNull = value; } }

    private bool pNotUSD;
    public bool NotUSD { get { return pNotUSD; } set { pNotUSD = value; } }

    private string pPorcentaje;
    public string Porcentaje { get { return pPorcentaje; } set { pPorcentaje = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cRoyaltyContrato()
    {

    }

    public cRoyaltyContrato(ref DBConn oConn)
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
        cSQL.Append("select num_contrato, no_contrato, cod_royalty, cod_marca, cod_categoria, cod_subcategoria, porcentaje ");
        cSQL.Append("from lic_royalty_contrato ");

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

        if (!string.IsNullOrEmpty(pCodRoyalty))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_royalty = @cod_royalty");
          oParam.AddParameters("@cod_royalty", pCodRoyalty, TypeSQL.Numeric);

        }

        if (pNotUSD) {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_royalty <> 3 ");
        }

        if (!string.IsNullOrEmpty(pCodMarca))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_marca = @cod_marca");
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
          cSQL.Append(" cod_categoria = @cod_categoria");
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

        if (!string.IsNullOrEmpty(pPorcentaje))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" porcentaje = @porcentaje");
          oParam.AddParameters("@porcentaje", pPorcentaje, TypeSQL.Float);

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

    public DataTable GetValData()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select 1 ");
        cSQL.Append("from lic_royalty_contrato ");

        if (!string.IsNullOrEmpty(pNumContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" num_contrato = @num_contrato");
          oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);

        }

        if (!string.IsNullOrEmpty(pMarca))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_marca in (select cod_marca from lic_marcas where descripcion = @descripcionmarca) ");
          oParam.AddParameters("@descripcionmarca", pMarca, TypeSQL.Varchar);
        }
        else if (pAsqMarcaNull)
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_marca is null ");
        }

        if (!string.IsNullOrEmpty(pCategoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_categoria in (select cod_categoria from lic_categorias where descripcion = @descripciocategoria) ");
          oParam.AddParameters("@descripciocategoria", pCategoria, TypeSQL.Varchar);
        }
        else if (pAsqCategoriaNull)
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_categoria is null ");
        }

        if (!string.IsNullOrEmpty(pSubCategoria))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_subcategoria in (select cod_subcategoria from lic_subcategoria where descripcion = @descripciosubcategoria') ");
          oParam.AddParameters("@descripciosubcategoria", pSubCategoria, TypeSQL.Varchar);
        }
        else if (pAsqSubCategoriaNull)
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

    public DataTable GetByInvoce()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select num_contrato, no_contrato, cod_royalty, cod_marca, cod_categoria, cod_subcategoria, porcentaje ");
        cSQL.Append("from lic_royalty_contrato ");

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

    public DataTable GetByExcel()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;
      string Condicion = " where ";

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select (select no_contrato from lic_contratos where num_contrato = a.num_contrato) Contrato, ");
        cSQL.Append("(select descripcion from lic_marcas where cod_marca = a.cod_marca) Marca, ");
        cSQL.Append("(select descripcion from lic_categorias where cod_categoria = a.cod_categoria ) Categoria, ");
        cSQL.Append("(select descripcion from lic_subcategoria where cod_subcategoria = a.cod_subcategoria) SubCategoria, ");
        cSQL.Append("Case When a.cod_royalty = 3 then 'USD' Else 'CLP' END as 'Moneda', ");
        cSQL.Append("(select descripcion from lic_tipo_royalty where cod_royalty = a.cod_royalty ) as 'Desc. Royalty', ");
        cSQL.Append(" porcentaje as 'Royalty', ");
        cSQL.Append(" (select porcentaje from lic_bdi_contrato where cod_marca = a.cod_marca and  cod_categoria = a.cod_categoria and (cod_subcategoria = a.cod_subcategoria or cod_subcategoria is null ) and (num_contrato = a.num_contrato or num_contrato is null) ) as 'BDI',  ");
        cSQL.Append("  '' as 'Producto', '' as 'Cliente', '' as 'SKU', '' as 'Descripcion Producto', ");
        cSQL.Append("'' as 'Precio Unitario Venta Neta', '' as 'Q Venta Neta', '' as 'Precio Unitario Devolucion / Descuento', '' as 'Q Devolucion' ");
        cSQL.Append("from lic_royalty_contrato a ");

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

  }
}
