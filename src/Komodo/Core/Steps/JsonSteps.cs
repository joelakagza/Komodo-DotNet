using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Komodo.Core.Extensions;
using Komodo.Core.Support;

namespace Komodo.Core.Steps
{
    [Binding]
    public class JsonSteps : SeleniumStepsBase
    {
        [Then(@"select json element value '(.*)' from '(.*)' into '(.*)'")]
        public void ThenSelectJsonElementValueFromInto(string jsonPath, string json, string varName)
        {
            var str = json.StrVar();
            JsonReader reader = new JsonTextReader(new StringReader(json.StrVar()));
            reader.DateParseHandling = DateParseHandling.None;
            JObject o = JObject.Load(reader);

            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //JArray j2 = JsonConvert.DeserializeObject<JArray>(json.StrVar(), settings);
            //JObject o = JsonConvert.DeserializeObject<JObject>(j2[0].ToString(), settings);

            if (varName == "<null>")
                varName = null;
            string jsonPathValue = (string)o.SelectToken(jsonPath);
            Common.SetEnvString(varName, jsonPathValue);
        }

        [Then(@"select json element value '(.*)' from json array '(.*)' into '(.*)'")]
        public void ThenSelectJsonElementValueFromJsonArrayInto(string jsonPath, string json, string varName)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            JArray j2 = JsonConvert.DeserializeObject<JArray>(json.StrVar(), settings);
            JObject o = JsonConvert.DeserializeObject<JObject>(j2[0].ToString(), settings);

            if (varName == "<null>")
                varName = null;
            string jsonPathValue = (string)o.SelectToken(jsonPath);
            Common.SetEnvString(varName, jsonPathValue);
        }

        [Then(@"select value for element '(.*)' from json array '(.*)' into '(.*)' where element '(.*)' has value '(.*)'")]
        public void ThenSelectValueForElementFromJsonArrayIntoWhereElementHasValue(string jsonPath2, string json, string varName, string jsonPath1, string value1)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            JArray j2 = JsonConvert.DeserializeObject<JArray>(Common.ReadFileText(json.StrVar()), settings);

            if (varName == "<null>")
                varName = null;

            int k = 0;
            foreach (JObject i in j2)
            {
                JObject o = JsonConvert.DeserializeObject<JObject>(i.ToString(), settings);
                string jsonPathValue1 = (string)o.SelectToken(jsonPath1);
                string jsonPathValue2 = (string)o.SelectToken(jsonPath2);
                if (jsonPathValue1 == value1)
                {
                    Common.SetEnvString(varName, jsonPathValue2);
                    k++;
                    break;
                }

            }

            if (k == 0)
                Common.SetEnvString(varName, string.Empty);

        }

        [Then(@"select json element value '(.*)' from JObject '(.*)' into '(.*)'")]
        public void ThenSelectJsonElementValueFromJObjectInto(string jsonPath, string json, string varName)
        {
            JObject o = JObject.Parse(json.StrVar());
            
            if (varName == "<null>")
                varName = null;
            string jsonPathValue = (string)o.SelectToken(jsonPath);
            CommonSetEnvString(varName, jsonPathValue);
        }

        [Then(@"get json value '(.*)' from '(.*)' into '(.*)'")]
        public void ThenGetJsonValueFromJObjectInto(string jsonPath, string json, string varName)
        {
            JsonReader reader = new JsonTextReader(new StringReader(json.StrVar()));
            reader.DateParseHandling = DateParseHandling.None;
            JObject o = JObject.Load(reader);

            if (varName == "<null>")
                varName = null;
            string jsonPathValue = (string)o.SelectToken(jsonPath);
            Common.SetEnvString<string>(varName, jsonPathValue);
        }

	    [Then(@"compare and verify that '(.*)' is lying between '(.*)' and '(.*)'")]        
	    public void ThenCompareAndVerifyThatIsLyingBetweenAnd(string deletedDate,string fromDate, string toDate)
        {
            Assert.Greater(deletedDate.StrVar(), fromDate.StrVar());
            Assert.Greater(toDate.StrVar(), deletedDate.StrVar());
        }

        [Then(@"compare and verify that '(.*)' is '(.*)'")]
        public void ThenCompareAndVerifyThatIs(string deleted, string value)
        {
            StringAssert.Contains(deleted.StrVar(), value);
        }

        [Then(@"verify following json '(.*)' contains json paths and values")]
        public void ThenVerifyFollowingJsonXxContainsJsonPathsAndValues(string jsonData,Table table)
        {
            foreach (TableRow rw in table.Rows)
            {
                ThenVerifyFollowingJsonContainsJsonPathsAndValues(jsonData,rw[0], rw[1]);
            }
        }

        [Then(@"verify following json '(.*)' contains json paths and values match")]
        public void ThenVerifyFollowingJsonXxContainsJsonPathsAndValuesMatch(string jsonData, Table table)
        {
            foreach (TableRow rw in table.Rows)
            {
                ThenVerifyFollowingJsonContainsJsonPathsAndValuesEquals(jsonData, rw[0], rw[1]);
            }
        }

        [Then(@"verify following json '(.*)' contains jsonPath '(.*)' and value '(.*)'")]
        public void ThenVerifyFollowingJsonContainsJsonPathsAndValues(string jsonData,string jsonPath, string jsonValue)
        {
            JObject o = JObject.Parse(RootJson(jsonData.StrVar()));

            if (jsonPath == "<null>")
                jsonPath = null;

            string name = (string)o.SelectToken(jsonPath);
            if (string.IsNullOrWhiteSpace(name))
            {
                name = string.Empty;
            }
            StringAssert.Contains(jsonValue.StrVar(), name);
        }

        [Then(@"verify following json '(.*)' contains jsonPath '(.*)' and value equals '(.*)'")]
        public void ThenVerifyFollowingJsonContainsJsonPathsAndValuesEquals(string jsonData, string jsonPath, string jsonValue)
        {
            JObject o = JObject.Parse(RootJson(jsonData.StrVar()));

            if (jsonPath == "<null>")
                jsonPath = null;

            string name = (string)o.SelectToken(jsonPath);
            StringAssert.Contains(jsonValue.StrVar(), name);
        }

        [Then(@"compare and verify with JToken that JObject1 '(.*)' is equal to JObject2 '(.*)'")]
        public void ThenCompareAndVerifyWithJTokenThatJObjectIsEqualToJObject(string j1, string j2)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            JsonReader reader1 = new JsonTextReader(new StringReader(RootJson(j1.StrVar())));
            reader1.DateParseHandling = DateParseHandling.None;

            JsonReader reader2 = new JsonTextReader(new StringReader(RootJson(j2.StrVar())));
            reader2.DateParseHandling = DateParseHandling.None;

            JObject jobject1 = JObject.Load(reader1);
            JObject jobject2 = JObject.Load(reader2);

            Console.WriteLine(JToken.DeepEquals(jobject1, jobject2));
            Assert.IsTrue(JToken.DeepEquals(jobject1, jobject2));
            // true
        }

        [Then(@"compare and verify with JToken that JObject1 '(.*)' is equal to JObject2 '(.*)'")]
        public void ThenCompareAndVerifyWithJTokenThatJObjectIsEqualToJObjectIgnore(string j1, string j2, string ignoreFields)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            JsonReader reader1 = new JsonTextReader(new StringReader(RootJson(j1.StrVar())));
            reader1.DateParseHandling = DateParseHandling.None;

            JsonReader reader2 = new JsonTextReader(new StringReader(RootJson(j2.StrVar())));
            reader2.DateParseHandling = DateParseHandling.None;

            JObject jobject1 = JObject.Load(reader1);
            jobject1 = RemoveJsonElements(jobject1,ignoreFields);
            JObject jobject2 = JObject.Load(reader2);
            jobject2 = RemoveJsonElements(jobject2, ignoreFields);

            Console.WriteLine(JToken.DeepEquals(jobject1, jobject2));
            Assert.IsTrue(JToken.DeepEquals(jobject1, jobject2));
            // true
        }

        private JObject RemoveJsonElements(JObject jObj, string elementsToRemove)
        {
            var ignoreFieldsSplit = elementsToRemove.Split('|');
            foreach (var field in ignoreFieldsSplit)
            {
                if (jObj.SelectToken(field) != null)
                    jObj.SelectToken(field).Parent.Remove();
            }

            return jObj;
        }

        [Then(@"compare and verify with JToken that JObject1 is equal to JObject2")]
        public void ThenCompareAndVerifyWithJTokenThatJObjectIsEqualToJObjectTable(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                Console.WriteLine(row[0].StrVar() + " " + row[1].StrVar());
                ThenCompareAndVerifyWithJTokenThatJObjectIsEqualToJObject(Common.ReadFileText(row[0].StrVar()), Common.ReadFileText(row[1].StrVar()));
            }
        }

        [Then(@"compare and verify with JToken that JObject1 is equal to JObject2 ignore '(.*)'")]
        public void ThenCompareAndVerifyWithJTokenThatJObjectIsEqualToJObjectTableIgnore(string ignoreFields,
            Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                Console.WriteLine(row[0].StrVar() + " " + row[1].StrVar());
                ThenCompareAndVerifyWithJTokenThatJObjectIsEqualToJObjectIgnore(Common.ReadFileText(row[0].StrVar()),
                    Common.ReadFileText(row[1].StrVar()), ignoreFields);
            }
        }


        private string RootJson(string json)
        {
            return "{\"root\": " + json + "}";
        }

        public void ThenCompareAndVerifyWithJTokenThatJPropertyIsEqualToJproperty(string j1, string j2, string jsonValue)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            JObject jobject1 = JObject.Parse(RootJson(j1.StrVar()));
            JObject jobject2 = JObject.Parse(RootJson(j2.StrVar()));
            JToken jproperty1 = jobject1.Property("root").Value.SelectToken(jsonValue);
            JToken jproperty2 = jobject2.Property("root").Value.SelectToken(jsonValue);
            Console.WriteLine(JToken.DeepEquals(jproperty1, jproperty2));
            Assert.IsTrue(JToken.DeepEquals(jproperty1, jproperty2));
            // true
        }

        [Then(@"compare and verify with ""(.*)"" JToken that JProperty1 is equal to JProperty2")]
        public void ThenCompareAndVerifyWithJTokenThatJPropertyIsEqualToJPropertyTable(string jsonValue, Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                Console.WriteLine(row[0].StrVar() + " " + row[1].StrVar());
                ThenCompareAndVerifyWithJTokenThatJPropertyIsEqualToJproperty(Common.ReadFileText(row[0].StrVar()), Common.ReadFileText(row[1].StrVar()), jsonValue);
            }
        }

        [Then(@"for each json file in folder '(.*)' and replace the following jsonPath and with values")]
        public void ThenForEaschJsonFileInFolderAndReplaceTheFollowingJsonPathAndWithValues(string folderPath, Table table)
        {
            DirectoryInfo dInfo = new DirectoryInfo(folderPath.StrVar());
            var files = dInfo.GetFiles("*.json",SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var obj =JObject.Parse(Common.ReadFileText(file.FullName));
                foreach (TableRow tblRow in table.Rows)
                {
                    var jTok = obj.SelectToken(tblRow[0]) as JValue;
                    
                    
                    Console.WriteLine(jTok);
                    if (jTok != null)
                        jTok.Value = tblRow[1];
                }
                
                Common.SaveFile(file.FullName,obj.ToString());
            }

        }


    }
}
