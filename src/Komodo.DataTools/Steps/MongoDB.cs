using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using TechTalk.SpecFlow.Assist;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TechTalk.SpecFlow;
using MongoDB.Bson.IO;
using Komodo.Core.Extensions;
using Komodo.Core;

namespace Komodo.DataTools.Steps
{
    [Binding]
    public class MongoDbSteps : TestSuiteStepsBase
    {
        public class queryParamList
           {
            public string field { get; set; }
            public string fieldValue { get; set; }
           }

        [Then(@"list all mongodb collections in database '(.*)'")]
        public void ThenIShouldHaveACustomerCalledJohnDoe(string databaseName)
        {
            var mon = FeatureContext.Current.Get<MongoDatabase>(databaseName.StrVar());

            var cols = mon.GetCollectionNames();
            foreach (var item in cols)
            {
                Console.WriteLine(item);
            }
        }

        [Then(@"query mongodb '(.*)' get '(.*)' by (.*) '(.*)' write into '(.*)'")]
        [Then(@"query mongodb database:'(.*)' get '(.*)' by (.*) '(.*)' write into '(.*)'")]
        [Then(@"query mongodb database:'(.*)' collection '(.*)' by (.*) '(.*)' write into '(.*)'")]
        public void ThenQueryMongodbDatabaseXxGetXxByIdXx(string databaseName, string collectionName, string field, string fieldValue, string varName)
        {
            //var mon = FeatureContext.Current.Get<MongoDatabaseBase>(databaseName.StrVar());
            //var cols = mon.GetCollection(collectionName);
            //var lst = from p in cols.AsQueryable()
            //          where p[field] == fieldValue.StrVar()
            //          select p;

            //if (lst.Count() == 1)
            //    Common.SetEnvString(varName, lst.FirstOrDefault().ToJson());
            //if (lst.Count() > 1)
            //    Common.SetEnvString(varName, lst.ToJson());
        }

        [Then(@"query mongodb database:'(.*)' collection '(.*)' by (.*) '(.*)' write into '(.*)' as bson document")]
        public void ThenQueryMongodbDatabaseXxGetXxByIdXxAsBsonDocument(string databaseName, string collectionName, string field, string fieldValue, string varName)
        {
            var mon = FeatureContext.Current.Get<MongoDatabase>(databaseName.StrVar());
            var cols = mon.GetCollection(collectionName);
            var lst = from p in cols.AsQueryable()
                      where p[field] == fieldValue.StrVar()
                      select p;

            if (lst.Count() == 1)
                Common.SetEnvString(varName, lst.FirstOrDefault().ToJson());
            if (lst.Count() > 1)
                Common.SetEnvString(varName, lst.ToJson());
        }

        [Then(@"query mongodb '(.*)' get '(.*)' by (.*) '(.*)' write as json into '(.*)'")]
        [Then(@"query mongodb database:'(.*)' get '(.*)' by (.*) '(.*)' write as json into '(.*)'")]
        public void ThenQueryMongodbDatabaseXxGetXxByIdXxAsJson(string databaseName, string collectionName, string field, string fieldValue, string varName)
        {
            var mon = FeatureContext.Current.Get<MongoDatabase>(databaseName.StrVar());
            var cols = mon.GetCollection(collectionName);
            var lst = from p in cols.AsQueryable()
                      where p[field] == fieldValue.StrVar()
                      select p;
            var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
            if (lst.Count() == 1)
                Common.SetEnvString(varName, lst.FirstOrDefault().ToJson(jsonWriterSettings));
            if (lst.Count() > 1)
                Common.SetEnvString(varName, lst.ToJson(jsonWriterSettings));
        }

        [Then(@"query mongodb '(.*)' get '(.*)' and write as json into '(.*)'")]
        public void ThenQueryMongodbGetAndWriteAsJsonInto(string databaseName, string collectionName, string varName, Table table)
        {
            var parameters = table.CreateSet<queryParamList>();
            var mon = FeatureContext.Current.Get<MongoDatabase>(databaseName.StrVar());
            var query = mon.GetCollection(collectionName).AsQueryable();
            var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };

            foreach (var p in parameters)
            {
                query = query.Where(t => t[p.field] == p.fieldValue.StrVar());
            }

            Common.SetEnvString(varName, query.FirstOrDefault().ToJson(jsonWriterSettings));
        }


        [Then(@"verify and query mongodb '(.*)' get '(.*)' by (.*) '(.*)' count equals (.*)")]
        [Then(@"verify and query mongodb '(.*)' get '(.*)' by (.*) '(.*)' that count equals (.*)")]
        public void ThenVerifyAndQueryMongodbGetBy_IdCountEqual(string databaseName, string collectionName, string field,string fieldValue, int count)
        {
            var mon = FeatureContext.Current.Get<MongoDatabase>(databaseName.StrVar());
            var cols = mon.GetCollection(collectionName);
            var lst = from p in cols.AsQueryable()
                      where p[field] == fieldValue.StrVar()
                      select p;

            Assert.AreEqual(lst.Count(), count);
        }

        [Then(@"delete the following collection in the mongodb database '(.*)'")]
        public void ThenDeleteTheFollowingCollectionInTheMongodbDatabase(string dbConn, Table table)
        {
            MongoDatabase db = FeatureContext.Current.Get<MongoDatabase>(dbConn.StrVar());
            foreach (TableRow dr in table.Rows)
            {
                if (db.GetCollection(dr[0]).Exists())
                    db.DropCollection(dr[0]);
            }
        }

        [Then(@"get mongo db collection counts for mongodb database '(.*)'")]
        public void GivenDataCoreApiMongoDbCounts(string mongodb)
        {
            MongoDatabase db = FeatureContext.Current.Get<MongoDatabase>(mongodb.StrVar());
            var cols = db.GetCollectionNames();

            foreach (var col in cols)
            {
                var propertiesCol = db.GetCollection(col);
                Console.WriteLine(col + ": " + propertiesCol.Count());
            }
        }

        [Then(@"insert into mongodb:'(.*)' collection:'(.*)' data:'(.*)'")]
        public void ThenInsertIntoMongodbCollectionData(string mongodb, string collectionName, string data)
        {
            MongoDatabase db = FeatureContext.Current.Get<MongoDatabase>(mongodb.StrVar());
            var cols = db.GetCollection(collectionName);

            try
            {
                var document = BsonSerializer.Deserialize<BsonDocument>(data.StrVar());
                cols.Insert(document);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        [Then(@"delete and clean from database '(.*)' all from collection '(.*)' where '(.*)' equals '(.*)'")]
        [Then(@"delete and clean from '(.*)' database all from collection '(.*)' where '(.*)' equals '(.*)'")]
        public void ThenDeleteAndCleanFromXxDatabaseAllFromCollectionXxWhereXxEqualsXx(string mongoDb,string collection,string bsonPath,
            string bsonValue)
        {
            var egiDashboardDbConn = FeatureContext.Current.Get<MongoDatabase>(mongoDb);
            var col = egiDashboardDbConn.GetCollection(collection);

            var colQuery = col.AsQueryable().Where(t => t[bsonPath.StrVar()] == bsonValue.StrVar());

            foreach (var mongoDoc in colQuery)
            {
                col.Remove(Query.EQ("_id", mongoDoc.GetValue("_id")));
            }
        }


        [Then(@"delete and clean from database '(.*)' all from collection '(.*)' where '(.*)' in '(.*)'")]
        public void ThenDeleteAndCleanFromDatabaseAllFromCollectionWhereIn(string mongoDb, string collection, string bsonPath,
            string bsonValue, Table table)
        {
            foreach (TableRow tr in table.Rows)
            {
                bsonValue = tr.Values.FirstOrDefault();
                ThenDeleteAndCleanFromXxDatabaseAllFromCollectionXxWhereXxEqualsXx(mongoDb, collection, bsonPath, bsonValue);
            }
        }
        

        [Then(@"delete and clean from dashboard database all collection where dashboardProperties '(.*)' equals '(.*)'")
       ]
        public void ThenDeleteAndCleanFromDashboardDatabaseAllCollectionWhereDashboardPropertiesEquals(string bsonPath,
           string bsonValue)
        {
            var egiDashboardDbConn = FeatureContext.Current.Get<MongoDatabase>("egiDashboardDb");
            var dashboardAvailabilitiesCol = egiDashboardDbConn.GetCollection("dashboardAvailabilities");
            var dashboardPropertiesCol = egiDashboardDbConn.GetCollection("dashboardProperties");
            var dashboardAggroCol = egiDashboardDbConn.GetCollection("dashboardAggro");
            var dashboardAggrov2Col = egiDashboardDbConn.GetCollection("dashboardAggro_v2");
            var dashboardInvDealCol = egiDashboardDbConn.GetCollection("dashboardInvestmentDeals");
            var dashboardOccDealCol = egiDashboardDbConn.GetCollection("dashboardOccupationalDeals");

            var propertyQuery =
                dashboardPropertiesCol.AsQueryable().Where(t => t[bsonPath.StrVar()] == bsonValue.StrVar());

            foreach (var property in propertyQuery)
            {
                //Common.SetEnvString("propertyId", property.GetValue("_id").ToString());
                dashboardPropertiesCol.Remove(Query.EQ("_id", property.GetValue("_id")));
                dashboardAvailabilitiesCol.Remove(Query.EQ("propertyId", property.GetValue("_id")));
                dashboardAggroCol.Remove(Query.EQ("propertyId", property.GetValue("_id")));
                dashboardAggrov2Col.Remove(Query.EQ("propertyId", property.GetValue("_id")));
                dashboardInvDealCol.Remove(Query.EQ("propertyId", property.GetValue("_id")));
                dashboardOccDealCol.Remove(Query.EQ("propertyId", property.GetValue("_id")));
            }
        }

        [Then(@"delete and clean company collections from insight database where dashboardCompany '(.*)' equals '(.*)'")]
        public void ThenDeleteAndCleanCompanyCollectionsFromInsightDatabaseWhereDashboardCompanyEquals(string bsonPath, string bsonValue, Table table)
        {
            foreach (TableRow tr in table.Rows)
            {
                bsonValue = tr.Values.FirstOrDefault();

                var egiDashboardDbConn = FeatureContext.Current.Get<MongoDatabase>("egiDashboardDb");

                var dashboardCompanyCol = egiDashboardDbConn.GetCollection("dashboardCompany_v1");
                var dashboardCompanyOwnedPropertyCol = egiDashboardDbConn.GetCollection("dashboardCompanyPropertiesOwned_v1");

                var companyQuery =
                    dashboardCompanyCol.AsQueryable().Where(t => t[bsonPath.StrVar()] == bsonValue.StrVar());

                foreach (var company in companyQuery)
                {
                    dashboardCompanyCol.Remove(Query.EQ("_id", company.GetValue("_id")));
                    dashboardCompanyOwnedPropertyCol.Remove(Query.EQ("companyId", company.GetValue("_id")));
                }
            }
        }

        #region mongodb system indexes

        [Then(@"verify the count of system indexes in the mongodb '(.*)' is equals (.*)")]
        public void ThenVerifyTheCountOfSystemIndexesInTheMongodbIsEquals(string mongodbName, int p1)
        {
            var mongodb = FeatureContext.Current.Get<MongoDatabase>(mongodbName);
            var colNames = mongodb.GetCollectionNames();

            var col = mongodb.GetCollection("system.indexes");
            var indexes = col.AsQueryable().Select(t => t);

            Assert.AreEqual(p1, indexes.Count(), "indexes count is not as expected");

        }

        [Then(@"query mongodb '(.*)' get system indexes and write as json into '(.*)'")]
        public void ThenQueryMongodbGetSystemIndexesAndWriteAsJsonInto(string mongodbName, string variable)
        {
            var mongodb = FeatureContext.Current.Get<MongoDatabase>(mongodbName);
            var colNames = mongodb.GetCollectionNames();

            var col = mongodb.GetCollection("system.indexes");
            var indexes = col.AsQueryable().Select(t => t);

            Common.SetEnvString(variable, indexes.ToList().ToJson());
        }


        #endregion
    }
}
