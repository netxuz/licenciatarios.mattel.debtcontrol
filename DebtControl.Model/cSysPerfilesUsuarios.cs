using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DebtControl.Conn;

namespace DebtControl.Model
{
    public class cSysPerfilesUsuarios
    {
        DBConn.SQLParameters oParam;
        DBConn.DataTypeSQL TypeSQL = new DBConn.DataTypeSQL();

        private string pCodPerfil;
        public string CodPerfil { get { return pCodPerfil; } set { pCodPerfil = value; } }

        private string pCodUser;
        public string CodUser { get { return pCodUser; } set { pCodUser = value; } }

        private string pError = string.Empty;
        public string Error { get { return pError; } set { pError = value; } }

        private string pAccion;
        public string Accion { get { return pAccion; } set { pAccion = value; } }
        
        private DBConn oConn;

        public cSysPerfilesUsuarios()
        {

        }

        public cSysPerfilesUsuarios(ref DBConn oConn)
        {
            this.oConn = oConn;
        }

        public DataTable Get()
        {
            oParam = new DBConn.SQLParameters(2);
            DataTable dtData;
            StringBuilder cSQL;
            string Condicion = " where ";

            if (oConn.bIsOpen)
            {
                cSQL = new StringBuilder();
                cSQL.Append("select cod_user, cod_perfil ");
                cSQL.Append("from sys_perfiles_usuarios ");

                if (!string.IsNullOrEmpty(pCodUser))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" cod_user = @cod_user");
                    oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);

                }

                if (!string.IsNullOrEmpty(pCodPerfil))
                {
                    cSQL.Append(Condicion);
                    Condicion = " and ";
                    cSQL.Append(" cod_perfil = @cod_perfil");
                    oParam.AddParameters("@cod_perfil", pCodPerfil, TypeSQL.Numeric);

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
            oParam = new DBConn.SQLParameters(2);
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
                            cSQL.Append("insert into sys_perfiles_usuarios(cod_user, cod_perfil) values(");
                            cSQL.Append("@cod_user, @cod_perfil)");
                            oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);
                            oParam.AddParameters("@cod_perfil", pCodPerfil, TypeSQL.Numeric);
                            oConn.Insert(cSQL.ToString(), oParam);

                            break;
                        case "ELIMINAR":
                            cSQL = new StringBuilder();
                            cSQL.Append("delete from sys_perfiles_usuarios where cod_user = @cod_user");
                            oParam.AddParameters("@cod_user", pCodUser, TypeSQL.Numeric);

                            if (!string.IsNullOrEmpty(pCodPerfil))
                            {
                                cSQL.Append(" and cod_perfil = @cod_perfil");
                                oParam.AddParameters("@cod_perfil", pCodPerfil, TypeSQL.Numeric);

                            }

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
