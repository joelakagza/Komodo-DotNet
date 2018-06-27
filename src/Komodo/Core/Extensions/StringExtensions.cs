using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Komodo.Core.Extensions
{
    static public class TableRowExtension
    {
        static public DataTable CopyToDataTable(this Table table,string tableName)
        {
            DataTable dt = new DataTable();
            dt.TableName = tableName;

            foreach (TableRow rw in table.Rows)
            {
                dt.Columns.Add(new DataColumn(helper.RemoveSpecialCharacters(rw[0]).Replace(" ","")));
            }

            return dt;
        }

        static public DataRow CopyToDataRow(this TableRow table,string tableName)
        {
            DataRow dr = null;
            return dr;
        }
    }

    static public class StringExtension
    {

        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }


        static public string StrVar(this string strVar)
        {
            if (!string.IsNullOrEmpty(strVar))
            {
                for (int j = 0; j < 30; j++)
                {
                    strVar = ConstantCommands(strVar);
                    strVar = EnvString(strVar);
                }

                // Replace with scenario context variable
                string scenarioStrVar;
                Regex rgx = new Regex(@"\${[a-zA-Z0-9\s-_]*}");
                string scenarioVarName = rgx.Match(strVar).Value;
                ScenarioContext.Current.TryGetValue(scenarioVarName, out scenarioStrVar);
                if (scenarioStrVar != null)
                    strVar = strVar.Replace(scenarioVarName, scenarioStrVar);
            }
            return strVar;
        }

        static public string ReplaceStrVar(this string strVar)
        {
            strVar = StrVar(strVar);
            
            // Xpath Replace
            Regex rgx = new Regex(@"\{([^}]*)\}");
            string xpathReplaceStr = rgx.Match(strVar).Value;
            if (xpathReplaceStr != "")
                strVar = strVar.Replace(xpathReplaceStr, "{0}").TrimEnd();
            return strVar;
        }

        static string EnvString(string strVar)
        {
            //string scenarioStrVar1;
            Regex rgx = new Regex(@"\%{env*}");
            string scenarioStrVar;
            string scenarioVarName = rgx.Match(strVar).Value;
            ScenarioContext.Current.TryGetValue(scenarioVarName, out scenarioStrVar);
            if (scenarioStrVar != null)
                strVar = strVar.Replace(scenarioVarName, scenarioStrVar);


            rgx = new Regex(@"\%{[a-zA-Z0-9\s-_]*}");
            scenarioVarName = rgx.Match(strVar).Value;
            string scenarioEnvVarName = rgx.Match(strVar).Value.Replace("%{", "").Replace("}", "");
            if (ConfigurationManager.AppSettings.AllKeys.Contains(scenarioEnvVarName))
            {
                //strVar = strVar.Replace(scenarioVarName, ConfigurationManager.AppSettings[scenarioEnvVarName]);
            }

            rgx = new Regex(@"\%{[a-zA-Z0-9\s-_]*}");
            MatchCollection matches = rgx.Matches(strVar);
            foreach (Match match in matches)
            {
                scenarioVarName = match.Value;
                ScenarioContext.Current.TryGetValue(scenarioVarName, out scenarioStrVar);
                if (scenarioStrVar != null)
                    strVar = strVar.Replace(scenarioVarName, scenarioStrVar);
                else if (scenarioVarName != "")
                {
                    // Lets try to get a default value
                    rgx = new Regex(@"\%{[a-zA-Z0-9\s-_]*}");
                    scenarioVarName = rgx.Match(strVar).Value;
                    ScenarioContext.Current.TryGetValue(scenarioVarName, out scenarioStrVar);
                    if (scenarioStrVar != null)
                        strVar = strVar.Replace(scenarioVarName, "");
                }
            }

            return strVar;

        }

        public static string SelectToken(this string str)
        {
            var rgx = new Regex(@"(?<=Json.File%{)(?:\\.|[^{}\\])*(?=})");
            JObject o = JObject.Parse(File.ReadAllText(rgx.Match(str).Value));
            return (string)o.SelectToken("");
        }

        static public string ItemNo(this string strVar)
        {
            Regex rgx = new Regex(@"(?<=\{)[^}]*(?=\})");
            strVar = rgx.Match(strVar).Value;
            if (strVar == "")
            strVar = "1";
            return strVar;
        }

        static string ConstantCommands(string strVar)
        {
            Regex needle = new Regex(@"\${GUID}");
            
            int i = 0;
            while (needle.IsMatch(strVar) || i > 100)
            {
                strVar = needle.Replace(strVar, Guid.NewGuid().ToString(), 1);
                i++;
            }
            strVar = strVar.Replace("${Date}", String.Format("{0:yyyyMMdd}", DateTime.Now));
            strVar = strVar.Replace("${DateTime}", String.Format("{0:yyyyMMddhhmmss}", DateTime.Now));

            return strVar;
        }


    }
}
