using System;
using System.Reflection;
using System.Web;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using DebtControl.Method;

namespace DebtControl.Conn
{
    public class DBConn
    {
        DataTable bdDataTable = null;
        DataSet bdDataSet = null;
        DbConnection bdConnection = null;
        DbCommand bdCommand = null;
        DbDataAdapter bdDataAdapter = null;
        DbParameter bdParameter = null;
        DbTransaction bdTransaction = null;
        static DbProviderFactory bdProvFact = null;
        AppSettingsReader appReader = null;

        private string sConn = string.Empty;
        private string sConnString = string.Empty;
        private string sDbProvider = string.Empty;

        private bool blnIsTransaction = false;
        public bool IsTransaction { get { return blnIsTransaction; } }

        private int iCommandTimeout = 300;
        public int CommandTimeout { get { return iCommandTimeout; } set { iCommandTimeout = value; } }

        private string sError = string.Empty;
        public string Error { get { return sError; } set { sError = value; } }

        private string pReturnQuery = string.Empty;
        public string ReturnQuery { get { return pReturnQuery; } set { pReturnQuery = value; } }

        public bool bIsOpen;

        public struct DataTypeSQL
        {
            public string Varchar { get { return "varchar"; } }
            public string Char { get { return "char"; } }
            public string Text { get { return "text"; } }
            public string Int { get { return "int"; } }
            public string Float { get { return "float"; } }
            public string Numeric { get { return "numeric"; } }
            public string DateTime { get { return "datetime"; } }
            public string nText { get { return "ntext"; } }
            public string nChar { get { return "nchar"; } }
            public string Null { get { return "null"; } }
        }

        public struct SQLParameters
        {
            private string[,] sParameters;
            private int iIndex;
            private bool bIni;

            public int Count { get { return iIndex - 1; } }
            public string[,] arrayParameters { get { return sParameters; } set { sParameters = value; } }

            public SQLParameters(int iSize) { sParameters = new string[iSize, 3]; iIndex = 0; bIni = true; }

            public void AddParameters(string sNameDataType, string sValueDataType, string sDataType)
            {
                if (iIndex == 0)
                {
                    if (!bIni) { sParameters = new string[100, 3]; }

                    sParameters[iIndex, 0] = sNameDataType;
                    sParameters[iIndex, 1] = sValueDataType;
                    sParameters[iIndex, 2] = sDataType;
                    iIndex++;
                }
                else
                {
                    sParameters[iIndex, 0] = sNameDataType;
                    sParameters[iIndex, 1] = sValueDataType;
                    sParameters[iIndex, 2] = sDataType;
                    iIndex++;
                }
            }
        }

        private DbType DBConvertORADataType(string strNombreVarSQLServer)
        {
            switch (strNombreVarSQLServer)
            {
                case "varchar":
                    return DbType.AnsiString;
                case "char":
                    return DbType.AnsiStringFixedLength;
                case "text":
                    return DbType.StringFixedLength;
                case "int":
                    return DbType.Int32;
                case "float":
                    return DbType.Double;
                case "numeric":
                    return DbType.Double;
                case "datetime":
                    return DbType.DateTime;
                case "ntext":
                    return DbType.StringFixedLength;
                case "nchar":
                    return DbType.StringFixedLength;
                case "null":
                    return DbType.Object;
                default:
                    return DbType.String;
            }
        }

        private DbType DBConvertSQLDataType(string VariableSQL)
        {
            switch (VariableSQL)
            {
                case "varchar":
                    return DbType.AnsiString;
                case "char":
                    return DbType.AnsiStringFixedLength;
                case "text":
                    return DbType.String;
                case "int":
                    return DbType.Int32;
                case "numeric":
                    return DbType.Double;
                case "float":
                    return DbType.Double;
                case "datetime":
                    return DbType.DateTime;
                case "ntext":
                    return DbType.String;
                case "nchar":
                    return DbType.StringFixedLength;
                case "null":
                    return DbType.Object;
                default:
                    return DbType.String;
            }
        }

        public DBConn()
        {
            //string sDBConnString = HttpContext.Current.Application["ConnectionString"].ToString();
            //string sDBProvider = HttpContext.Current.Application["DBProvider"].ToString();
            appReader = new System.Configuration.AppSettingsReader();
            //sConnString = appReader.GetValue("connectionString", typeof(string)).ToString();
            Web oWeb = new Web();
            //sConnString = oWeb.UnCrypt(appReader.GetValue("connectionString", typeof(string)).ToString());
            sConnString = appReader.GetValue("connectionString", typeof(string)).ToString();
            sDbProvider = appReader.GetValue("provider", typeof(string)).ToString();
            bdProvFact = DbProviderFactories.GetFactory(sDbProvider);
        }

        public bool Open()
        {
            try
            {
                bIsOpen = false;
                bdConnection = DBConn.bdProvFact.CreateConnection();
                bdConnection.ConnectionString = sConnString;

                //abre la conexión si está cerrada
                if (bdConnection.State == ConnectionState.Closed)
                {
                    bdConnection.Open();
                    bIsOpen = true;
                    return true;
                }
                else { return false; }
            }
            catch (DbException dbEx) //Consume el error y redirecciona a la página de error
            {
                sError = dbEx.ErrorCode + " " + dbEx.Message + "\t" + GetType().Name + "." + MethodBase.GetCurrentMethod().Name;
                bIsOpen = false;
                return false;
            }
        }

        public bool Close()
        {
            bool onClose = false;
            try
            {
                if (bdConnection.State == ConnectionState.Open)
                {
                    bdConnection.Close();

                    if (bdTransaction != null)
                        bdTransaction.Dispose();

                    if (bdDataTable != null)
                        bdDataTable.Dispose();

                    if (bdDataSet != null)
                        bdDataSet.Dispose();

                    if (bdDataAdapter != null)
                        bdDataAdapter.Dispose();

                    if (bdConnection != null)
                        bdConnection.Dispose();

                    onClose = true;
                }
                return onClose;
            }
            catch (DbException dbEx)
            {
                sError = dbEx.ErrorCode + " " + dbEx.Message + "\t" + GetType().Name + "." + MethodBase.GetCurrentMethod().Name;
                bIsOpen = false;
                return false;
            }
        }

        public bool BeginTransaction()
        {
            try
            {
                if (!blnIsTransaction)
                {
                    bdTransaction = bdConnection.BeginTransaction();
                    blnIsTransaction = true;
                    return blnIsTransaction;
                }
                else
                    return blnIsTransaction;
            }
            catch (Exception Ex)
            {
                sError = Ex.Message + "\t" + GetType().Name + "." + MethodBase.GetCurrentMethod().Name;
                return false;
            }
        }

        public bool Commit()
        {
            try
            {
                if (blnIsTransaction)
                {
                    bdTransaction.Commit();
                    blnIsTransaction = false;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception Exception)
            {
                sError = Exception.Message + "\t" + GetType().Name + "." + MethodBase.GetCurrentMethod().Name;
                bdTransaction.Rollback();
                blnIsTransaction = false;
                return false;
            }
        }

        public bool Rollback()
        {
            try
            {
                bdTransaction.Rollback();
                blnIsTransaction = false;
                return true;
            }
            catch (Exception Exception)
            {
                sError = Exception.Message + "\t" + GetType().Name + "." + MethodBase.GetCurrentMethod().Name;
                blnIsTransaction = false;
                return false;
            }
        }

        public DataTable Select(string sQuery)
        {
            bdDataTable = new DataTable();
            if (bdConnection.State == ConnectionState.Closed)
                return bdDataTable;

            bdCommand = bdProvFact.CreateCommand();
            bdCommand.Connection = bdConnection;

            if (blnIsTransaction)
                bdCommand.Transaction = bdTransaction;

            try
            {
                bdDataSet = new DataSet();
                if (bdDataAdapter == null)
                    bdDataAdapter = bdProvFact.CreateDataAdapter();

                bdCommand.CommandTimeout = iCommandTimeout;
                bdCommand.CommandText = sQuery;
                bdDataAdapter.SelectCommand = bdCommand;
                bdDataAdapter.Fill(bdDataSet);

                return bdDataSet.Tables[0];
            }
            catch (DbException dbEx)
            {
                sError = dbEx.ErrorCode + " " + dbEx.Message + "\t" + GetType().Name + "." + MethodBase.GetCurrentMethod().Name;
                return null;
            }
        }

        public DataTable Select(string sQuery, SQLParameters ArrayParameters)
        {
            bdDataTable = new DataTable();
            if (bdConnection.State == ConnectionState.Closed)
                return bdDataTable;

            bdCommand = bdProvFact.CreateCommand();
            bdCommand.Connection = bdConnection;

            if (blnIsTransaction)
                bdCommand.Transaction = bdTransaction;

            try
            {
                bdDataSet = new DataSet();
                if (bdDataAdapter == null)
                    bdDataAdapter = bdProvFact.CreateDataAdapter();

                for (int IntIndice = 0; IntIndice <= ArrayParameters.Count; IntIndice++)
                {
                    bdParameter = bdProvFact.CreateParameter();
                    bdParameter.ParameterName = (sDbProvider.Contains("OracleClient") ? ArrayParameters.arrayParameters[IntIndice, 0].Replace("@", ":") : ArrayParameters.arrayParameters[IntIndice, 0]);

                    if (sDbProvider.Contains("SqlClient"))
                    { bdParameter.DbType = DBConvertSQLDataType(ArrayParameters.arrayParameters[IntIndice, 2]); }
                    else
                    {
                        if (sDbProvider.Contains("OracleClient"))
                        {
                            sQuery = sQuery.Replace("@", ":");
                            bdParameter.DbType = DBConvertORADataType(ArrayParameters.arrayParameters[IntIndice, 2]);
                        }
                    }
                    if (ArrayParameters.arrayParameters[IntIndice, 1] == null || bdParameter.DbType == DbType.Object)
                        bdParameter.Value = DBNull.Value;
                    else
                        bdParameter.Value = ArrayParameters.arrayParameters[IntIndice, 1];

                    bdCommand.Parameters.Add(bdParameter);
                }

                bdCommand.CommandTimeout = iCommandTimeout;
                bdCommand.CommandText = sQuery;
                bdDataAdapter.SelectCommand = bdCommand;
                bdDataAdapter.Fill(bdDataSet);
                pReturnQuery = sQuery;
                return bdDataSet.Tables[0];
            }
            catch (DbException dbEx)
            {
                sError = dbEx.ErrorCode + " " + dbEx.Message + "\t" + GetType().Name + "." + MethodBase.GetCurrentMethod().Name;
                return null;
            }
        }

        public void Insert(string sQuery, SQLParameters ArrayParameters)
        {

            bdCommand = bdProvFact.CreateCommand();
            bdCommand.Connection = bdConnection;

            if (blnIsTransaction)
                bdCommand.Transaction = bdTransaction;

            try
            {
                for (int IntIndice = 0; IntIndice <= ArrayParameters.Count; IntIndice++)
                {
                    bdParameter = bdProvFact.CreateParameter();
                    bdParameter.ParameterName = (sDbProvider.Contains("OracleClient") ? ArrayParameters.arrayParameters[IntIndice, 0].Replace("@", ":") : ArrayParameters.arrayParameters[IntIndice, 0]);

                    if (sDbProvider.Contains("SqlClient"))
                    { bdParameter.DbType = DBConvertSQLDataType(ArrayParameters.arrayParameters[IntIndice, 2]); }
                    else
                    {
                        if (sDbProvider.Contains("OracleClient"))
                        {
                            sQuery = sQuery.Replace("@", ":");
                            bdParameter.DbType = DBConvertORADataType(ArrayParameters.arrayParameters[IntIndice, 2]);
                        }
                    }
                    pReturnQuery = ArrayParameters.arrayParameters[IntIndice, 1];
                    pReturnQuery = pReturnQuery + "<br>";
                    if (ArrayParameters.arrayParameters[IntIndice, 1] == null || bdParameter.DbType == DbType.Object)
                        bdParameter.Value = DBNull.Value;
                    else
                        bdParameter.Value = ArrayParameters.arrayParameters[IntIndice, 1];
                    bdCommand.Parameters.Add(bdParameter);
                }

                bdCommand.CommandTimeout = iCommandTimeout;
                bdCommand.CommandText = sQuery;
                //pReturnQuery = sQuery;
                bdCommand.ExecuteNonQuery();
            }
            catch (DbException dbEx)
            {
                //pReturnQuery = sQuery;
                sError = dbEx.ErrorCode + " " + dbEx.Message + "\t" + GetType().Name + "." + MethodBase.GetCurrentMethod().Name;
            }
        }

        public void Delete(string sQuery, SQLParameters ArrayParameters)
        {
            bdCommand = bdProvFact.CreateCommand();
            bdCommand.Connection = bdConnection;

            if (blnIsTransaction)
                bdCommand.Transaction = bdTransaction;

            try
            {
                for (int IntIndice = 0; IntIndice <= ArrayParameters.Count; IntIndice++)
                {
                    bdParameter = bdProvFact.CreateParameter();
                    bdParameter.ParameterName = (sDbProvider.Contains("OracleClient") ? ArrayParameters.arrayParameters[IntIndice, 0].Replace("@", ":") : ArrayParameters.arrayParameters[IntIndice, 0]);

                    if (sDbProvider.Contains("SqlClient"))
                    { bdParameter.DbType = DBConvertSQLDataType(ArrayParameters.arrayParameters[IntIndice, 2]); }
                    else
                    {
                        if (sDbProvider.Contains("OracleClient"))
                        {
                            sQuery = sQuery.Replace("@", ":");
                            bdParameter.DbType = DBConvertORADataType(ArrayParameters.arrayParameters[IntIndice, 2]);
                        }
                    }
                    if (ArrayParameters.arrayParameters[IntIndice, 1] == null || bdParameter.DbType == DbType.Object)
                        bdParameter.Value = DBNull.Value;
                    else
                        bdParameter.Value = ArrayParameters.arrayParameters[IntIndice, 1];

                    bdCommand.Parameters.Add(bdParameter);
                }

                bdCommand.CommandTimeout = iCommandTimeout;
                bdCommand.CommandText = sQuery;
                bdCommand.ExecuteNonQuery();
            }
            catch (DbException dbEx)
            {
                sError = dbEx.ErrorCode + " " + dbEx.Message + "\t" + GetType().Name + "." + MethodBase.GetCurrentMethod().Name;
            }
        }

        public void Update(string sQuery, SQLParameters ArrayParameters)
        {
            bdCommand = bdProvFact.CreateCommand();
            bdCommand.Connection = bdConnection;

            if (blnIsTransaction)
                bdCommand.Transaction = bdTransaction;

            try
            {
                for (int IntIndice = 0; IntIndice <= ArrayParameters.Count; IntIndice++)
                {
                    bdParameter = bdProvFact.CreateParameter();
                    bdParameter.ParameterName = (sDbProvider.Contains("OracleClient") ? ArrayParameters.arrayParameters[IntIndice, 0].Replace("@", ":") : ArrayParameters.arrayParameters[IntIndice, 0]);

                    if (sDbProvider.Contains("SqlClient"))
                    { bdParameter.DbType = DBConvertSQLDataType(ArrayParameters.arrayParameters[IntIndice, 2]); }
                    else
                    {
                        if (sDbProvider.Contains("OracleClient"))
                        {
                            sQuery = sQuery.Replace("@", ":");
                            bdParameter.DbType = DBConvertORADataType(ArrayParameters.arrayParameters[IntIndice, 2]);
                        }
                    }
                    if (ArrayParameters.arrayParameters[IntIndice, 1] == null || bdParameter.DbType == DbType.Object)
                        bdParameter.Value = DBNull.Value;
                    else
                        bdParameter.Value = ArrayParameters.arrayParameters[IntIndice, 1];

                    bdCommand.Parameters.Add(bdParameter);
                }

                bdCommand.CommandTimeout = iCommandTimeout;
                bdCommand.CommandText = sQuery;
                bdCommand.ExecuteNonQuery();
            }
            catch (DbException dbEx)
            {
                sError = dbEx.ErrorCode + " " + dbEx.Message + "\t" + GetType().Name + "." + MethodBase.GetCurrentMethod().Name;
            }
        }

        public string getTableCod(string pTable, string pKey, DBConn oConn)
        {
            long lngCodigo;
            DataTable dtCodigo;
            StringBuilder sSQL = new StringBuilder("SELECT 1 FROM " + pTable + " WHERE " + pKey + " = ");
            do
            {
                lngCodigo = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss").ToString());
                dtCodigo = oConn.Select(sSQL.ToString() + lngCodigo);
            } while (dtCodigo.Rows.Count > 0);
            return lngCodigo.ToString();


        }

    }
}
