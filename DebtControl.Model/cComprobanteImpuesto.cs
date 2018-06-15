using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cComprobanteImpuesto
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodComprobante;
    public string CodComprobante { get { return pCodComprobante; } set { pCodComprobante = value; } }

    private string pNumContrato;
    public string NumContrato { get { return pNumContrato; } set { pNumContrato = value; } }

    private string pNomComprobante;
    public string NomComprobante { get { return pNomComprobante; } set { pNomComprobante = value; } }

    private string pPeriodo;
    public string Periodo { get { return pPeriodo; } set { pPeriodo = value; } }

    private string pDeclaraMovimiento;
    public string DeclaraMovimiento { get { return pDeclaraMovimiento; } set { pDeclaraMovimiento = value; } }

    private string pFechaDeclaracion;
    public string FechaDeclaracion { get { return pFechaDeclaracion; } set { pFechaDeclaracion = value; } }

    private string pRepositorioArchivo;
    public string RepositorioArchivo { get { return pRepositorioArchivo; } set { pRepositorioArchivo = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cComprobanteImpuesto()
    {

    }

    public cComprobanteImpuesto(ref DBConn oConn)
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
        cSQL.Append("select cod_comprobante, num_contrato, nom_comprobante, periodo, declara_movimiento, fecha_declaracion, repositorio_archivo ");
        cSQL.Append("from lic_comprobante_impuesto ");

        if (!string.IsNullOrEmpty(pCodComprobante))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_comprobante = @cod_comprobante");
          oParam.AddParameters("@cod_comprobante", pCodComprobante, TypeSQL.Numeric);

        }

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
          cSQL.Append(" periodo = @periodo");
          oParam.AddParameters("@periodo", pPeriodo, TypeSQL.Varchar);

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
              pCodComprobante = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
              cSQL = new StringBuilder();
              cSQL.Append("insert into lic_comprobante_impuesto(cod_comprobante, num_contrato, nom_comprobante, periodo, declara_movimiento, fecha_declaracion, repositorio_archivo) values(");
              cSQL.Append("@cod_comprobante, @num_contrato, @nom_comprobante, @periodo, @declara_movimiento, @fecha_declaracion, @repositorio_archivo) ");
              oParam.AddParameters("@cod_comprobante", pCodComprobante, TypeSQL.Numeric);
              oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
              oParam.AddParameters("@nom_comprobante", pNomComprobante, TypeSQL.Varchar);
              oParam.AddParameters("@periodo", pPeriodo, TypeSQL.Varchar);
              oParam.AddParameters("@declara_movimiento", pDeclaraMovimiento, TypeSQL.Char);
              oParam.AddParameters("@fecha_declaracion", DateTime.Now.ToString("yyyy-MM-dd").ToString(), TypeSQL.DateTime);
              oParam.AddParameters("@repositorio_archivo", pRepositorioArchivo, TypeSQL.Char);
              oConn.Insert(cSQL.ToString(), oParam);

              break;
            case "ELIMINAR":
              cSQL = new StringBuilder();
              cSQL.Append("delete from lic_comprobante_impuesto where cod_comprobante = @cod_comprobante");
              oParam.AddParameters("@cod_comprobante", pCodComprobante, TypeSQL.Numeric);
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
