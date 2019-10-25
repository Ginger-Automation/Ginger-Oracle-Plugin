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
        public static GingerOracleConnection db = new GingerOracleConnection();

        static string conn = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=host)(PORT=1521))(CONNECT_DATA=(sid=sid)));User Id=id;Password=pass;";

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ConnectionString", conn);
            db.KeyvalParamatersList = param;
            Boolean testconn = db.OpenConnection(param);

        }

        [TestMethod]
        public void OpenConnection()
        {
            //Arrange
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ConnectionString", conn);

            //Act
            Boolean testconn = db.OpenConnection(param);

            //Assert
            Assert.IsTrue(testconn);
        }

        [TestMethod]
        public void GetTableList()
        {
            //Arrange
            List<string> Tables = null;
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("ConnectionString", conn);
            Boolean testconn = db.OpenConnection(param);
            
            //Act
            Tables = db.GetTablesList();

            //Assert
            Assert.AreEqual(4, Tables.Count);
            Assert.AreEqual("authors", Tables[1]);
            Assert.AreEqual("sys_config", Tables[2]);
            Assert.AreEqual("tutorials_tbl", Tables[3]);
        }

        [TestMethod]
        public void GetTablesColumns()
        {
            //Arrange
            List<string> Columns = null;
            string tablename = "authors";

            //Act
            Columns = db.GetTablesColumns(tablename);

            //Assert
            Assert.AreEqual(4, Columns.Count);
            Assert.AreEqual("id", Columns[1]);
            Assert.AreEqual("name", Columns[2]);
            Assert.AreEqual("email", Columns[3]);
        }

        [TestMethod]
        public void RunUpdateCommand()
        {
            //Arrange
            string upadateCommand = "UPDATE authors SET email='aaa@aa.com' where id=3";
            string result = null;

            //Act
            result = db.RunUpdateCommand(upadateCommand, false);

            //Assert
            Assert.AreEqual(result, "1");
        }

        [TestMethod]
        public void GetSingleValue()
        {
            //Arrange
            string result = null;

            //Act
            result = db.GetSingleValue("authors", "name", "id=2");

            //Assert
            Assert.AreEqual(result, "Priya");
        }

        [TestMethod]
        public void DBQuery()
        {
            //Arrange
            DataTable result = null;

            //Act
            result = db.DBQuery("SELECT * FROM authors");

            //Assert
            Assert.AreEqual(result.Rows.Count, 3);
        }

        [TestMethod]
        public void GetRecordCount()
        {
            //Arrange
            int a = 0;

            //Act
            a = db.GetRecordCount("authors");

            //Assert
            Assert.AreEqual(a, 3);
        }
    }

}
