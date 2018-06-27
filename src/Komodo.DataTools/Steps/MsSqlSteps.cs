using Komodo.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.Linq;
using TechTalk.SpecFlow;

namespace Komodo.DataTools.Steps
{
    [Binding]
    public class MsSqlSteps : TestSuiteStepsBase
    {
        private readonly MySqlObject mySqlObject;

        public MsSqlSteps(MySqlObject _mySqlObject)
        {
            mySqlObject = _mySqlObject;
            if (FeatureContext.Current.ContainsKey("mySqlConn"))
                mySqlObject.MySqlConn = (MySqlConnection) FeatureContext.Current["mySqlConn"];
            if (FeatureContext.Current.ContainsKey("msSqlConn"))
                mySqlObject.MsSqlConn = (SqlConnection) FeatureContext.Current["msSqlConn"];
        }


        [Then(@"execute sql '(.*)' with the mssql connection and get value for column '(.*)'")]
        public void ThenExecuteSqlWithTheMssqlConnectionAndGetValueForColumn(string p0, string p1)
        {
            var cols = p1.Split('|').ToList();
            foreach (var s in cols)
            {
                Then(string.Format("with the mssql connection execute sql '{0}' and dump in json format to '{1}'", p0,
                    "%{actualDataDir}/%{testBucket}/".StrVar() + s + ".json".StrVar()));
                Then(string.Format("read file '{0}' into scenario context environment variable '{1}'",
                    "%{actualDataDir}/%{testBucket}/".StrVar() + s + ".json".StrVar(), s));
                var jsonPath = "Table[0]." + s;
                JObject o = JObject.Parse(Common.GetEnvString(s));
                var j = JObject.FromObject(o).ToString(Formatting.None);
                var txt = string.Format("get json value '{0}' from '{1}' into '{2}'", jsonPath, j, s);
                Then(txt);
                Console.WriteLine(s + ":" + Common.GetEnvString(s));
            }

        }


        [Then(@"execute sql file:'(.*)' with the mssql connection")]
        public void ThenWithTheMysqlConnectionExecuteSql(string filePath)
        {
            var sql = Common.ReadFileText(filePath.StrVar());
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
    }




}

