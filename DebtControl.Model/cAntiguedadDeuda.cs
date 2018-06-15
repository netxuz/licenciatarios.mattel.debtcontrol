using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
  public class cAntiguedadDeuda
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pNKeyCliente;
    public string NKeyCliente { get { return pNKeyCliente; } set { pNKeyCliente = value; } }

    private string pNKeyDeudor;
    public string NKeyDeudor { get { return pNKeyDeudor; } set { pNKeyDeudor = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cAntiguedadDeuda()
    {

    }

    public cAntiguedadDeuda(ref DBConn oConn)
    {
      this.oConn = oConn;
    }

    public DataTable GetFinanzas()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select CodigoDeudor.ncodigodeudor, deudor.snombre,sum(AntiguedadDeudaWeb.nSaldo) as 'Total', ");
        cSQL.Append("isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = AntiguedadDeudaWeb.nKEy_Deudor and web.natraso = '0'),0) as 'total_0', ");
        cSQL.Append("isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = AntiguedadDeudaWeb.nKEy_Deudor and web.natraso = '15'),0) as 'total_15', ");
        cSQL.Append("isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = AntiguedadDeudaWeb.nKEy_Deudor and web.natraso = '30'),0) as 'total_30', ");
        cSQL.Append("isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = AntiguedadDeudaWeb.nKEy_Deudor and web.natraso = '60'),0) as 'total_60', ");
        cSQL.Append("isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = AntiguedadDeudaWeb.nKEy_Deudor and web.natraso = '90'),0) as 'total_90', ");
        cSQL.Append("isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = AntiguedadDeudaWeb.nKEy_Deudor and web.natraso = '180'),0) as 'total_180', ");
        cSQL.Append("isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = AntiguedadDeudaWeb.nKEy_Deudor and web.natraso = '360'),0) as 'total_360' ");
        cSQL.Append("from AntiguedadDeudaWeb join codigodeudor on (codigodeudor.nkey_cliente = AntiguedadDeudaWeb.nkey_cliente  and codigodeudor.nkey_deudor = AntiguedadDeudaWeb.nkey_deudor)  Join Servicio On(Servicio.nKey_Cliente = AntiguedadDeudaWeb.nKey_Cliente  And Servicio.nKEy_Deudor = AntiguedadDeudaWeb.nKEy_Deudor and servicio.nkey_analista <> 4 ) ");
        cSQL.Append("join deudor on (deudor.nkey_deudor = codigodeudor.nkey_deudor) where AntiguedadDeudaWeb.nkey_cliente = @nkeycliente ");
        cSQL.Append("group by CodigoDeudor.ncodigodeudor, deudor.snombre,AntiguedadDeudaWeb.nkey_deudor ");
        cSQL.Append("Order by sum(AntiguedadDeudaWeb.nSaldo) desc");
        oParam.AddParameters("@nkeycliente", pNKeyCliente, TypeSQL.Numeric);

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

    public DataTable Get()
    {
      oParam = new DBConn.SQLParameters(10);
      DataTable dtData;
      StringBuilder cSQL;

      if (oConn.bIsOpen)
      {
        cSQL = new StringBuilder();
        cSQL.Append("select AntiguedadDeudaWeb.stipodocumento, AntiguedadDeudaWeb.nnumerofactura, sum(AntiguedadDeudaWeb.nSaldo) as 'Total', ");
        cSQL.Append("isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = @nkeydeudor and web.natraso = '0' and web.nnumerofactura = AntiguedadDeudaWeb.nnumerofactura and Web.stipodocumento=AntiguedadDeudaWeb.stipodocumento ),0) as 'total_0', isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = @nkeydeudor and web.natraso = '15' and web.nnumerofactura = AntiguedadDeudaWeb.nnumerofactura and Web.stipodocumento=AntiguedadDeudaWeb.stipodocumento ),0) as 'total_15', ");
        cSQL.Append("isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = @nkeydeudor and web.natraso = '30' and web.nnumerofactura = AntiguedadDeudaWeb.nnumerofactura and Web.stipodocumento=AntiguedadDeudaWeb.stipodocumento ),0) as 'total_30', isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = @nkeydeudor and web.natraso = '60' and web.nnumerofactura = AntiguedadDeudaWeb.nnumerofactura and Web.stipodocumento=AntiguedadDeudaWeb.stipodocumento ),0) as 'total_60', ");
        cSQL.Append("isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = @nkeydeudor and web.natraso = '90' and web.nnumerofactura = AntiguedadDeudaWeb.nnumerofactura and Web.stipodocumento=AntiguedadDeudaWeb.stipodocumento ),0) as 'total_90', isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = @nkeydeudor and web.natraso = '180' and web.nnumerofactura = AntiguedadDeudaWeb.nnumerofactura and Web.stipodocumento=AntiguedadDeudaWeb.stipodocumento ),0) as 'total_180', ");
        cSQL.Append("isnull((select sum(web.nSaldo) from AntiguedadDeudaWeb web where Web.nkey_cliente = @nkeycliente and web.nkey_deudor = @nkeydeudor and web.natraso = '360' and web.nnumerofactura = AntiguedadDeudaWeb.nnumerofactura and Web.stipodocumento=AntiguedadDeudaWeb.stipodocumento ),0) as 'total_360' ");
        cSQL.Append("from AntiguedadDeudaWeb join codigodeudor on (codigodeudor.nkey_cliente = AntiguedadDeudaWeb.nkey_cliente and codigodeudor.nkey_deudor = AntiguedadDeudaWeb.nkey_deudor)  ");
        cSQL.Append("join deudor on (deudor.nkey_deudor = codigodeudor.nkey_deudor)  ");
        cSQL.Append("where AntiguedadDeudaWeb.nkey_cliente = @nkeycliente ");
        cSQL.Append("and AntiguedadDeudaWeb.nkey_deudor = @nkeydeudor ");
        cSQL.Append("group by AntiguedadDeudaWeb.stipodocumento,AntiguedadDeudaWeb.nnumerofactura ");
        cSQL.Append("Order by sum(AntiguedadDeudaWeb.nSaldo) desc ");
        oParam.AddParameters("@nkeycliente", pNKeyCliente, TypeSQL.Numeric);
        oParam.AddParameters("@nkeydeudor", pNKeyDeudor, TypeSQL.Numeric);

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
