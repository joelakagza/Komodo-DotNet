//using Komodo.Core;
//using MongoDB.Bson;
//using MongoDB.Bson.Serialization;
//using System;
//using System.Linq;
//using TechTalk.SpecFlow;

//namespace Komodo.DataTools.Steps
//{
//    [Binding]
//    public class BsonSteps : TestSuiteStepsBase
//    {
//        [Then(@"select bson element value '(.*)' from '(.*)' into '(.*)'")]
//        public void ThenSelectBsonElementValueFromInto(string path, string bson, string varName)
//        {
//            BsonDocument document = null;
//            try
//            {
//                BsonArray bsa = BsonSerializer.Deserialize<BsonArray>(bson.StrVar());
//                document = bsa.FirstOrDefault().AsBsonDocument;
//            }
//            catch (Exception ex)
//            {
//                Console.Write("this is not a bson array");
//            }
            
//            if(document == null)
//               document = BsonSerializer.Deserialize<BsonDocument>(bson.StrVar());

//            Console.WriteLine(document.GetValue(path));
//            Common.SetEnvString(varName, document.GetValue(path).ToString());
//        }

//        [Then(@"select bson element value '(.*)' from array '(.*)' into '(.*)'")]
//        public void ThenSelectBsonElementValueFromArrayInto(string path, string bson, string varName)
//        {
//            BsonArray bsa = BsonSerializer.Deserialize<BsonArray>(bson.StrVar());
//            var str = bsa.FirstOrDefault()[path];
//            Console.WriteLine(str);
//            Common.SetEnvString(varName, str.ToString());
//        }

//        [Then(@"select bson element value '(.*)' from document '(.*)' into '(.*)'")]
//        public void ThenSelectBsonElementValueFromDocumentInto(string path, string bson, string varName)
//        {
//            BsonDocument bsa = BsonSerializer.Deserialize<BsonDocument>(bson.StrVar());
//            var str = bsa.GetValue(path);
//            Console.WriteLine(str);
//            Common.SetEnvString(varName, str.ToString());
//        }
//    }
//}
