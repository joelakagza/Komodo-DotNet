using Komodo.Core;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace Rbi.Property.Egi.Dashboards.UITests.Steps
{
    [Binding]
    public class TestDataSteps : TestSuiteStepsBase
    {
        protected static MongoClient _client;
        protected static MongoDatabaseBase _database;

        private void GenerateTestDataIntoDatabase()
        {
            //var mon = FeatureContext.Current.Get<MongoClient>("egiDashboardDb".StrVar());
        }

        //    [Then(@"create property test data from base '(.*)' into database '(.*)'")]
        //    public void ThenCreatePropertyTestDataFromBaseIntoDatabase(string folderPath, string database, Table table)
        //    {
        //        JsonSerializerSettings settings = new JsonSerializerSettings();
        //        settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

        //        DataTable dt = table.CopyToDataTable("Table");
        //        MongoDatabaseBase db = FeatureContext.Current.Get<MongoDatabaseBase>(database.StrVar());
        //        DirectoryInfo dirInfo = new DirectoryInfo(folderPath.StrVar());
        //        var files = dirInfo.GetFiles();

        //        foreach (TableRow tr in table.Rows)
        //        {
        //            foreach (var file in files)
        //            {
        //                ICollection<string> col = tr.Keys;
        //                var mongoDbString = "";
        //                foreach (var cell in col)
        //                {
        //                    var jsonStr = File.ReadAllText(file.FullName);
        //                    Common.SetEnvString<string>(cell, tr.GetString(cell));
        //                    mongoDbString = jsonStr.StrVar();
        //                }
        //                InsertIntoMongoDb(db, file, mongoDbString);
        //            }
        //        }

        //    }

        //    private void InsertIntoMongoDb(MongoDatabase db, FileInfo file, string mongoDb)
        //    {
        //        var collection = db.GetCollection(file.Name.Replace(".json", ""));
        //        var docs = BsonSerializer.Deserialize<BsonArray>(mongoDb);

        //        foreach (var doc in docs)
        //            collection.Save(doc);
        //    }

        //
    }
}