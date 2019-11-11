using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oracle;
using System;
using System.Collections.Generic;
using System.Data;

namespace GingerOracleTest
{
    [TestClass]
    public class OracleTest
    {
        public static GingerOracleConnection mGingerOracleConnection;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            mGingerOracleConnection = new GingerOracleConnection();
            mGingerOracleConnection.Protocol = "TCP";
            mGingerOracleConnection.Host = "illin056";
            mGingerOracleConnection.Port = 1521;
            mGingerOracleConnection.Sid = "ATSDEVDB";
            mGingerOracleConnection.UserId = "ATS_MAIN";
            mGingerOracleConnection.Password = "ATS_MAIN";
            // mGingerOracleConnection.OpenConnection();

        }

        //[TestMethod]
        //public void OpenConnection()
        //{
        //    //Arrange
        //    Dictionary<string, string> param = new Dictionary<string, string>();
        //    param.Add("ConnectionString", conn);

        //    //Act
        //    Boolean testconn = mGingerOracleConnection.OpenConnection(param);

        //    //Assert
        //    Assert.IsTrue(testconn);
        //}

        [TestMethod]
        public void GetTableList()
        {
            ////Arrange            
            
            ////Act
            //List<string> Tables = mGingerOracleConnection.GetTablesList();

            ////Assert
            //Assert.AreEqual(4, Tables.Count);
            //Assert.AreEqual("authors", Tables[1]);
            //Assert.AreEqual("sys_config", Tables[2]);
            //Assert.AreEqual("tutorials_tbl", Tables[3]);
        }

        [TestMethod]
        public void GetTablesColumns()
        {
            //Arrange            
            //string tablename = "ALM_TRANSFORM_RQM_2.TEST";

            ////Act
            //List<string> Columns = mGingerOracleConnection.GetTableColumns(tablename);

            ////Assert
            //Assert.AreEqual(4, Columns.Count);
            //Assert.AreEqual("id", Columns[1]);
            //Assert.AreEqual("name", Columns[2]);
            //Assert.AreEqual("email", Columns[3]);
        }

        //[TestMethod]
        //public void RunUpdateCommand()
        //{
        //    //Arrange
        //    string upadateCommand = "UPDATE authors SET email='aaa@aa.com' where id=3";
        //    string result = null;

        //    //Act
        //    result = mGingerOracleConnection.RunUpdateCommand(upadateCommand, false);

        //    //Assert
        //    Assert.AreEqual(result, "1");
        //}

        

        [TestMethod]
        public void ExeucuteQuery()
        {
            ////Arrange            

            ////Act
            //DataTable result = (DataTable)mGingerOracleConnection.ExecuteQuery("SELECT TS_TEST_ID FROM ALM_TRANSFORM_RQM_2.TEST WHERE ROWNUM = 1");

            //DataTable result2 = (DataTable)mGingerOracleConnection.ExecuteQuery("select* from SVI_TEST_10_ATS.HRM_COPY_LOG where ROWNUM = 1");
            

            ////Assert
            //Assert.AreEqual(result.Rows.Count, 3);
        }

        //[TestMethod]
        //public void GetRecordCount()
        //{
        //    //Arrange
        //    int a = 0;

        //    //Act
        //    a = mGingerOracleConnection.GetRecordCount("authors");

        //    //Assert
        //    Assert.AreEqual(a, 3);
        //}
    }

}
