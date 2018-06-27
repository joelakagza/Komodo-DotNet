using Komodo.Core;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Komodo.DataTools.Steps
{
    [Binding]
    public class DbConnSteps : TestSuiteStepsBase
    {
        #region MySql

        private readonly MySqlObject mySqlObject;

        public DbConnSteps(MySqlObject _mySqlObject)
        {
            mySqlObject = _mySqlObject;
            if (FeatureContext.Current.ContainsKey("mySqlConn"))
                mySqlObject.MySqlConn = (MySqlConnection) FeatureContext.Current["mySqlConn"];
            if (FeatureContext.Current.ContainsKey("msSqlConn"))
                mySqlObject.MsSqlConn = (SqlConnection) FeatureContext.Current["msSqlConn"];
        }

        [Given(@"we create a MySql database connection")]
        public void GivenWeCreateAMySqlDatabaseConnection(Table table)
        {
            MySqlConnectionStringBuilder connString = table.CreateInstance<MySqlConnectionStringBuilder>();
            mySqlObject.MySqlConn = new MySqlConnection(connString.ToString());
        }

        [Given(@"we create a MsSql database connection")]
        public void GivenWeCreateAMsSqlDatabaseConnection(Table table)
        {
            SqlConnectionStringBuilder connString = table.CreateInstance<SqlConnectionStringBuilder>();
            mySqlObject.MsSqlConn = new SqlConnection(connString.ToString());
        }

        [Then(@"with the mssql connection execute sql '(.*)' and dump in (.*) format to '(.*)'")]
        public void ThenWithTheMssqlConnectionExecuteSqlAndDumpTo(string sqltxt,string format, string filePath)
        {
            try
            {
                var ds = dbconn.GetMsDataSet(mySqlObject.MsSqlConn, sqltxt.StrVar());
                DataTable dt = ds.Tables[0];

                foreach (char c in System.IO.Path.GetInvalidPathChars())
                    filePath = filePath.StrVar().Replace(c, '_');

                FileInfo f = new FileInfo(filePath.StrVar());
                if (!Directory.Exists(f.DirectoryName))
                    Directory.CreateDirectory(f.DirectoryName);

                Common.SaveFile(filePath.StrVar().Replace(".xml", ".json"), ds.Ds2Json());
                ds.WriteXml(filePath.StrVar().Replace(".json", ".xml"));

                mySqlObject.MsSqlConn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }

        }

        [Then(@"with the mysql connection execute sql '(.*)' and (.*) in xml format to '(.*)'")]
        public void ThenWithTheMysqlConnectionExecuteSqlAndDumpTo(string p0,string action, string p1)
        {
            try
            {
                MySqlDataAdapter sAdap = new MySqlDataAdapter();

                mySqlObject.MySqlConn.Open();
                MySqlCommand myCmd = mySqlObject.MySqlConn.CreateCommand();
                myCmd.CommandText = p0.StrVar();
                myCmd.CommandTimeout = 300;
                sAdap.SelectCommand = myCmd;

                DataSet ds = new DataSet();
                sAdap.Fill(ds);

                FileInfo f = new FileInfo(p1.StrVar());
                if (!Directory.Exists(f.DirectoryName))
                    Directory.CreateDirectory(f.DirectoryName);

                StringWriter writer = new StringWriter();
                ds.WriteXml(writer);

                switch (action.ToLower())
                {
                    case "append":
                        File.AppendAllText(p1.StrVar(), writer.ToString());
                        break;
                    case "write":
                    case "dump":
                        File.WriteAllText(p1.StrVar(), writer.ToString());
                        break;
                }

                mySqlObject.MySqlConn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }


        [Then(@"with the mysql and sql '(.*)' row count equals (.*) in dataset (.*)")]
        public void ThenWithTheMysqlAndSqlXxCountRowsInDatasetXx(string sql, int cnt, int dsIndex)
        {
            try
            {
                MySqlDataAdapter sAdap = new MySqlDataAdapter();

                mySqlObject.MySqlConn.Open();
                MySqlCommand myCmd = mySqlObject.MySqlConn.CreateCommand();
                myCmd.CommandText = sql.StrVar();
                sAdap.SelectCommand = myCmd;

                DataSet ds = new DataSet();
                sAdap.Fill(ds);

                Console.WriteLine("No of datasets: " + ds.Tables.Count);
                Console.WriteLine("No of rows in dataset " + dsIndex + " : " + ds.Tables[dsIndex].Rows.Count);

                Assert.AreEqual(cnt, ds.Tables[dsIndex].Rows.Count);

                mySqlObject.MySqlConn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }

        }

        [Then(@"execute sql '(.*)' with the mssql connection")]
        public void ThenWithTheMysqlConnectionExecuteSql(string sql)
        {
            try
            {
                SqlDataAdapter sAdap = new SqlDataAdapter();

                mySqlObject.MsSqlConn.Open();
                SqlCommand myCmd = mySqlObject.MsSqlConn.CreateCommand();
                myCmd.CommandText = sql.StrVar();
                sAdap.SelectCommand = myCmd;

                myCmd.ExecuteNonQuery();

                mySqlObject.MsSqlConn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [Then(@"execute sql '(.*)' with the mysql connection")]
        public void ThenWithTheMssqlConnectionExecuteSql(string sql)
        {
            try
            {
                MySqlDataAdapter sAdap = new MySqlDataAdapter();

                mySqlObject.MySqlConn.Open();
                MySqlCommand myCmd = mySqlObject.MySqlConn.CreateCommand();
                myCmd.CommandText = sql.StrVar();
                sAdap.SelectCommand = myCmd;

                myCmd.ExecuteNonQuery();

                mySqlObject.MySqlConn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }


        #endregion

    }




}

