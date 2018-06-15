using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
    public class cSysPerfiles
    {
        DBConn.SQLParameters oParam;
        DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

        private string pCodPerfil;
        public string CodPerfil { get { return pCodPerfil; } set { pCodPerfil = value; } }

        private string pNomPerfil;
        public string NomPerfil { get { return pNomPerfil; } set { pNomPerfil = value; } }

        private string pEstPerfil;
        public string EstPerfil { get { return pEstPerfil; } set { pEstPerfil = value; } }

        private string pError = string.Empty;
        public string Error { get { return pError; } set { pError = value; } }

        private DBConn oConn;

        public cSysPerfiles()
        {

        }

        public cSysPerfiles(ref DBConn oConn)
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
                cSQL.Append("select cod_perfil, nom_perfil, est_perfil ");
                cSQL.Append("from sys_perfiles ");

                if (!string.IsNullOrEmpty(pCodPerfil))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" cod_perfil = @cod_perfil");
                    oParam.AddParameters("@cod_perfil", pCodPerfil, TypeSQL.Numeric);

                }

                if (!string.IsNullOrEmpty(pEstPerfil))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" est_perfil = @est_perfil");
                    oParam.AddParameters("@est_perfil", pEstPerfil, TypeSQL.Varchar);

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
