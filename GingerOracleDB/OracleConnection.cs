
using Amdocs.Ginger.Plugin.Core;
using Amdocs.Ginger.Plugin.Core.DatabaseLib;
using Amdocs.Ginger.Plugin.Core.Reporter;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Oracle
{
    [GingerService("OracleService", "Oracle Database service")]
    public class GingerOracleConnection : IDatabase
    {
        OracleConnection mOracleConnection;
        
        private DbTransaction tran = null;

        [DatabaseParam("Protocol")]
        [Default("TCP")]
        public string Protocol { get; set; }

        // [Mandatory]
        [DatabaseParam("Host")]        
        public string Host { get; set; }

        [DatabaseParam("Port")]
        [Default("1521")]
        public int Port { get; set; }

        [DatabaseParam("Sid")]        
        public string Sid { get; set; }


        [DatabaseParam("UserId")]        
        public string UserId { get; set; }


        [DatabaseParam("Password")]
        // TODO: Add password encrypted field
        public string Password { get; set; }
        

        private IReporter mReporter;


        private string mConnectionString;
        public string ConnectionString
        {
            get
            {
                // Unless the user set the conn string we auto build it from params
                if (mConnectionString != null)
                {
                    return mConnectionString;
                }
                else
                {
                    string conn = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL={Protocol})(HOST={Host})(PORT={Port}))(CONNECT_DATA=(sid={Sid})));User Id={UserId};Password={Password};";
                    return conn;
                }
            }
            set
            {
                mConnectionString = value;
            }
        }

        
        //    string connStr = null;
        //    bool res;
        //    res = false;

        //    if (String.IsNullOrEmpty(ConnectionString) == false)
        //    {
        //        connStr = ConnectionString.Replace("{USER}", User);

        //        String deCryptValue = null; //EncryptionHandler.DecryptString(Password, ref res, false);
        //        if (res == true)
        //        { connStr = connStr.Replace("{PASS}", deCryptValue); }
        //        else
        //        { connStr = connStr.Replace("{PASS}", Password); }
        //    }
        //    else
        //    {
        //        String strConnString = TNS;

        //        connStr = "Data Source=" + TNS + ";User Id=" + User + ";";

        //        String deCryptValue = null;// EncryptionHandler.DecryptString(Password, ref res, false);

        //        if (res == true) { connStr = connStr + "Password=" + deCryptValue + ";"; }
        //        else { connStr = connStr + "Password=" + Password + ";"; }
        //    }
        //    return connStr;
        // }
        //public static string GetMissingDLLErrorDescription()
        //{
        //    string message = "Connect to the DB failed." + Environment.NewLine + "The file Oracle.ManagedDataAccess.dll is missing," + Environment.NewLine + "Please download the file, place it under the below folder, restart Ginger and retry." + Environment.NewLine + AppDomain.CurrentDomain.BaseDirectory + Environment.NewLine + "Links to download the file:" + Environment.NewLine + "https://docs.oracle.com/database/121/ODPNT/installODPmd.htm#ODPNT8149" + Environment.NewLine + "http://www.oracle.com/technetwork/topics/dotnet/downloads/odacdeploy-4242173.html";
        //    return message;
        //}

        

        public bool OpenConnection()
        {
            mOracleConnection = new OracleConnection(ConnectionString);
            mOracleConnection.Open();
            if (mOracleConnection.State == ConnectionState.Open)
            {
                return true;
            }
            else
            {
                return false;
            }

            //DbProviderFactory factory;
            //try
            //{
            //    // GetConnectionString(KeyvalParamatersList);
            //    //var DLL = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + @"Oracle.ManagedDataAccess.dll");
            //    //var class1Type = DLL.GetType("Oracle.ManagedDataAccess.Client.OracleConnection");
            //    //object[] param = new object[1];
            //    //param[0] = ConnectionString;
            //    //dynamic c = Activator.CreateInstance(class1Type, param);
            //    //conn = (DbConnection)c;

            //    conn.ConnectionString = ConnectionString;

            //    conn.Open();


            //    return true;


            //}
            //catch (Exception e)
            //{
            //    String Temp = e.Message;
            //    //if (Temp.Contains ("ORA-03111"))
            //    //if (Temp.Contains("ORA-03111") || Temp.Contains("ORA-01017"))
            //    //{
            //    //    factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            //    //    conn = factory.CreateConnection();
            //    //    conn.ConnectionString = "Provider=msdaora;" + ConnectionString;
            //    //    conn.Open();
            //    //}
            //    //else if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Oracle.ManagedDataAccess.dll"))
            //    //{

            //    //    throw new Exception(GetMissingDLLErrorDescription());
            //    //}
            //    //else
            //    //{
            //    //    throw e;
            //    //}
            //}
            //return true;
        }
        public void CloseConnection()
        {
            try
            {
                if (mOracleConnection != null)
                {
                    mOracleConnection.Close();
                }
            }
            catch (Exception e)
            {
                mReporter.ToLog(eLogLevel.ERROR, "Failed to close DB Connection", e);
                throw (e);
            }
            finally
            {
                mOracleConnection?.Dispose();
            }
        }

        public object ExecuteQuery(string query)
        {
            // TODO: using

            OracleCommand cmd = new OracleCommand()            
            {
                Connection = (OracleConnection)mOracleConnection,
                CommandText = query
            };
            cmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataTable results = new DataTable();
            da.Fill(results);
            return results;
        }
        

        public List<string> GetTablesColumns(string table)
        {
            DbDataReader reader = null;
            List<string> rc = new List<string>();
            if ((mOracleConnection == null || string.IsNullOrEmpty(table)))
            {
                return rc;
            }
            try
            {
                DbCommand command = mOracleConnection.CreateCommand();
                // Do select with zero records
                command.CommandText = "select * from " + table + " where 1 = 0";
                command.CommandType = CommandType.Text;

                reader = command.ExecuteReader();
                // Get the schema and read the cols
                DataTable schemaTable = reader.GetSchemaTable();
                foreach (DataRow row in schemaTable.Rows)
                {
                    string ColName = (string)row[0];
                    rc.Add(ColName);
                }
            }
            catch (Exception e)
            {
                mReporter.ToLog(eLogLevel.ERROR, "", e);
                //mReporter.ToUser(eUserMsgKey.DbTableError, "table columns", e.Message);
                throw (e);
            }
            finally
            {
                reader.Close();
            }
            return rc;
        }

        public List<string> GetTablesList(string Name = null)
        {            
            List<string> rc = new List<string>() { "" };
            DataTable table = mOracleConnection.GetSchema("Tables");            
            foreach (DataRow row in table.Rows)
            {                
                string tableName = (string)row[0] + "." +(string)row[1];
                rc.Add(tableName);
            }
            
            return rc;
        }

        //public string RunUpdateCommand(string updateCmd, bool commit = true)
        //{
        //    string result = "";

        //    using (DbCommand command = conn.CreateCommand())
        //    {
        //        try
        //        {
        //            if (commit)
        //            {
        //                tran = conn.BeginTransaction();
        //                // to Command object for a pending local transaction
        //                command.Connection = conn;
        //                command.Transaction = tran;
        //            }
        //            command.CommandText = updateCmd;
        //            command.CommandType = CommandType.Text;

        //            result = command.ExecuteNonQuery().ToString();
        //            if (commit)
        //            {
        //                tran.Commit();
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            tran.Rollback();
        //            mReporter.ToLog2(eLogLevel.ERROR, "Commit failed for:" + updateCmd, e);
        //            throw e;
        //        }
        //    }
        //    return result;
        //}

        

        public void InitReporter(IReporter reporter)
        {
            mReporter = reporter;
        }
    }
}
