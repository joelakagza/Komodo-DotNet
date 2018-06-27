using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komodo.Core
{
    public static class JsonHelper
    {
        public static bool CompareJson(string j1, string j2, string ignoreFields = null)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            JsonReader reader1 = new JsonTextReader(new StringReader(j1));
            reader1.DateParseHandling = DateParseHandling.None;

            JsonReader reader2 = new JsonTextReader(new StringReader(j2));
            reader2.DateParseHandling = DateParseHandling.None;

            JObject jobject1 = JObject.Load(reader1);
            jobject1 = RemoveJsonElements(jobject1, ignoreFields);
            JObject jobject2 = JObject.Load(reader2);
            jobject2 = RemoveJsonElements(jobject2, ignoreFields);

            //Console.WriteLine(JToken.DeepEquals(jobject1, jobject2));

            return JToken.DeepEquals(jobject1, jobject2);
        }

        private static JObject RemoveJsonElements(JObject jObj, string elementsToRemove)
        {
            if (string.IsNullOrEmpty(elementsToRemove))
                return jObj;

            var ignoreFieldsSplit = elementsToRemove.Split('|');
            foreach (var field in ignoreFieldsSplit)
            {
                if (jObj.SelectToken(field) != null)
                    jObj.SelectToken(field).Parent.Remove();
            }

            return jObj;
        }
    }
}
