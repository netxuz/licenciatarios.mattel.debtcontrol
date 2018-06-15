using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;
using DebtControl.Model;

namespace DebtControl.Model
{
  public class cLogEventos
  {
    DBConn.SQLParameters oParam;
    DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

    private string pCodLog;
    public string CodLog { get { return pCodLog; } set { pCodLog = value; } }

    private string pCodCanal;
    public string CodCanal { get { return pCodCanal; } set { pCodCanal = value; } }

    private string pCodFlujo;
    public string CodFlujo { get { return pCodFlujo; } set { pCodFlujo = value; } }

    private string pNomFlujo;
    public string NomFlujo { get { return pNomFlujo; } set { pNomFlujo = value; } }

    private string pArchLog;
    public string ArchLog { get { return pArchLog; } set { pArchLog = value; } }

    private string pFchLog;
    public string FchLog { get { return pFchLog; } set { pFchLog = value; } }

    private string pFchDesdeLog;
    public string FchDesdeLog { get { return pFchDesdeLog; } set { pFchDesdeLog = value; } }

    private string pFchHastaLog;
    public string FchHastaLog { get { return pFchHastaLog; } set { pFchHastaLog = value; } }

    private string pNumContrato;
    public string NumContrato { get { return pNumContrato; } set { pNumContrato = value; } }

    private string pNoContrato;
    public string NoContrato { get { return pNoContrato; } set { pNoContrato = value; } }

    private string pNkeyDeudor;
    public string NkeyDeudor { get { return pNkeyDeudor; } set { pNkeyDeudor = value; } }

    private string pRutDeudor;
    public string RutDeudor { get { return pRutDeudor; } set { pRutDeudor = value; } }

    private string pNomDeudor;
    public string NomDeudor { get { return pNomDeudor; } set { pNomDeudor = value; } }

    private string pCodUser;
    public string CodUser { get { return pCodUser; } set { pCodUser = value; } }

    private string pRutUser;
    public string RutUser { get { return pRutUser; } set { pRutUser = value; } }

    private string pNomUser;
    public string NomUser { get { return pNomUser; } set { pNomUser = value; } }

    private string pAccionLog;
    public string AccionLog { get { return pAccionLog; } set { pAccionLog = value; } }

    private string pPeriodoLog;
    public string PeriodoLog { get { return pPeriodoLog; } set { pPeriodoLog = value; } }

    private string pObsLog;
    public string ObsLog { get { return pObsLog; } set { pObsLog = value; } }

    private string pObsErrorLog;
    public string ObsErrorLog { get { return pObsErrorLog; } set { pObsErrorLog = value; } }

    private string pIpLog;
    public string IpLog { get { return pIpLog; } set { pIpLog = value; } }

    private string pIndReporsitorioArch;
    public string IndReporsitorioArch { get { return pIndReporsitorioArch; } set { pIndReporsitorioArch = value; } }

    private string pAccion;
    public string Accion { get { return pAccion; } set { pAccion = value; } }

    private string pError = string.Empty;
    public string Error { get { return pError; } set { pError = value; } }

    private DBConn oConn;

    public cLogEventos()
    {

    }

    public cLogEventos(ref DBConn oConn)
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
        cSQL.Append("select cod_log, cod_canal, cod_flujo, nom_flujo, arch_log, fch_log, num_contrato, no_contrato, nkey_deudor, rut_deudor, nom_deudor, cod_user, rut_user, nom_user, accion_log, periodo_log, obs_log, obs_error_log, ip_log, ind_reporsitorio_arch ");
        cSQL.Append("from sys_log_eventos  ");

        if (!string.IsNullOrEmpty(pCodCanal))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_canal = @cod_canal");
          oParam.AddParameters("@cod_canal", pCodCanal, TypeSQL.Numeric);
        }

        if (!string.IsNullOrEmpty(pCodFlujo))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" cod_flujo = @cod_flujo");
          oParam.AddParameters("@cod_flujo", pCodFlujo, TypeSQL.Numeric);
        }

        if (!string.IsNullOrEmpty(pNoContrato))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" no_contrato = @no_contrato");
          oParam.AddParameters("@no_contrato", pNoContrato, TypeSQL.Varchar);
        }

        if (!string.IsNullOrEmpty(pNkeyDeudor))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" nkey_deudor = @nkey_deudor");
          oParam.AddParameters("@nkey_deudor", pNkeyDeudor, TypeSQL.Numeric);
        }

        if ((!string.IsNullOrEmpty(pFchDesdeLog)) && (!string.IsNullOrEmpty(pFchHastaLog)))
        {
          cSQL.Append(Condicion);
          Condicion = " and ";
          cSQL.Append(" fch_log >= @fchdesde_log and fch_log <= @fchhasta_log ");
          oParam.AddParameters("@fchdesde_log", pFchDesdeLog, TypeSQL.Varchar);
          oParam.AddParameters("@fchhasta_log", pFchHastaLog, TypeSQL.Varchar);

        }

        cSQL.Append(" order by fch_log ");

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

              if ((string.IsNullOrEmpty(pNomDeudor)) && (!string.IsNullOrEmpty(pNumContrato)))
              {
                cContratos oContratos = new cContratos(ref oConn);
                oContratos.NumContrato = pNumContrato;
                DataTable dt = oContratos.Get();
                if (dt != null)
                {
                  if (dt.Rows.Count > 0)
                  {
                    pNoContrato = dt.Rows[0]["no_contrato"].ToString();

                    cDeudor oDeudor = new cDeudor(ref oConn);
                    oDeudor.NKeyDeudor = dt.Rows[0]["nkey_deudor"].ToString();
                    DataTable dtdeudor = oDeudor.Get();
                    if (dtdeudor != null)
                    {
                      if (dtdeudor.Rows.Count > 0)
                      {
                        pNomDeudor = dtdeudor.Rows[0]["snombre"].ToString();
                        pNkeyDeudor = dtdeudor.Rows[0]["nkey_deudor"].ToString();
                        pRutDeudor = dtdeudor.Rows[0]["nrut"].ToString() + '-' + dtdeudor.Rows[0]["sDigitoVerificador"].ToString();
                      }
                    }
                    dtdeudor = null;
                  }
                }
                dt = null;
              }

              if (string.IsNullOrEmpty(pNkeyDeudor)) {
                pNkeyDeudor = null;
              }

              cSQL = new StringBuilder();
              cSQL.Append("insert into sys_log_eventos(cod_canal, cod_flujo, nom_flujo, arch_log, fch_log, num_contrato, no_contrato, nkey_deudor, rut_deudor, nom_deudor, cod_user, rut_user, nom_user, accion_log, periodo_log, obs_log, obs_error_log, ip_log, ind_reporsitorio_arch) values(");
              cSQL.Append("@cod_canal, @cod_flujo, @nom_flujo, @arch_log, @fch_log, @num_contrato, @no_contrato, @nkey_deudor, @rut_deudor, @nom_deudor, @cod_user, @rut_user, @nom_user, @accion_log, @periodo_log, @obs_log, @obs_error_log, @ip_log, @ind_reporsitorio_arch)");
              oParam.AddParameters("@cod_canal", pCodCanal, TypeSQL.Numeric);
              oParam.AddParameters("@cod_flujo", CodFlujo, TypeSQL.Numeric);
              oParam.AddParameters("@nom_flujo", pNomFlujo, TypeSQL.Varchar);
              oParam.AddParameters("@arch_log", pArchLog, TypeSQL.Varchar);
              oParam.AddParameters("@fch_log", DateTime.Now.ToString(), TypeSQL.DateTime);
              oParam.AddParameters("@num_contrato", pNumContrato, TypeSQL.Numeric);
              oParam.AddParameters("@no_contrato", pNoContrato, TypeSQL.Varchar);
              oParam.AddParameters("@nkey_deudor", pNkeyDeudor, TypeSQL.Numeric);
              oParam.AddParameters("@rut_deudor", pRutDeudor, TypeSQL.Varchar);
              oParam.AddParameters("@nom_deudor", pNomDeudor, TypeSQL.Varchar);
              oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);
              oParam.AddParameters("@rut_user", pRutUser, TypeSQL.Varchar);
              oParam.AddParameters("@nom_user", pNomUser, TypeSQL.Varchar);
              oParam.AddParameters("@accion_log", pAccionLog, TypeSQL.Varchar);
              oParam.AddParameters("@periodo_log", pPeriodoLog, TypeSQL.Varchar);
              oParam.AddParameters("@obs_log", pObsLog, TypeSQL.Varchar);
              oParam.AddParameters("@obs_error_log", pObsErrorLog, TypeSQL.Varchar);
              oParam.AddParameters("@ip_log", pIpLog, TypeSQL.Varchar);
              oParam.AddParameters("@ind_reporsitorio_arch", pIndReporsitorioArch, TypeSQL.Char);
              oConn.Insert(cSQL.ToString(), oParam);

              pError = oConn.Error;

              cSQL = new StringBuilder();
              cSQL.Append("select @@IDENTITY");
              dtData = oConn.Select(cSQL.ToString());
              if (dtData != null)
                if (dtData.Rows.Count > 0)
                  pCodLog = dtData.Rows[0][0].ToString();
              dtData = null;

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
