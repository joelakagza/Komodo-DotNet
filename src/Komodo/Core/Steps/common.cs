using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using System.IO;
using Komodo.Core.Extensions;
using Komodo.Core.Support;
using NUnit.Framework;

namespace Komodo.Core.Steps
{
    [Binding]
    public partial class CommonSteps : SeleniumStepsBase
    {
        [Then(@"console writeline '(.*)'")]
        public void ThenConsoleWritelineSomeVar(string strtxt)
        {
            Console.WriteLine(strtxt.StrVar());
            Console.WriteLine(strtxt.ReplaceStrVar());
            Console.WriteLine(strtxt.ItemNo());
        }

        [Given(@"we set up the following scenario context environment variables")]
        public void GivenWeSetUpTheFollowingScenarioContextEnvironmentVariables(Table table)
        {
            foreach (var envItem in table.Rows)
            {
                Common.SetEnvString(envItem[0].StrVar(), envItem[1].StrVar());
            }
        }

        [Given(@"we set the context environment to '(.*)'")]
        public void GivenWeSetTheContextEnvironmentTo(string environment)
        {

            Common.SetEnvString("env", environment);
            Common.SetEnvString("environment", environment);
            KomodoTestSuite.config.Environment = environment;

        }

        [Then(@"create a variable named '(.*)' with data '(.*)'")]
        [Then(@"create a variable named '(.*)' and set data '(.*)'")]
        [Then(@"create a variable named '(.*)' and set as '(.*)'")]
        public void ThenCreateVariableNamedXxToWith(string varName, string data)
        {
            Common.SetEnvString(varName, data.StrVar());
        }

        [Then(@"copy variable data '(.*)' to variable named '(.*)'")]
        public void ThenCopyVariableDataToVariableNamed(string var, string varName)
        {
            Common.SetEnvString(varName, var.StrVar());
        }

        [Then(@"read file '(.*)' into scenario context environment variable '(.*)'")]
        public void ThenReadFileIntoScenarioContextEnvironmentVariable(string filePath, string p1)
        {
            foreach (char c in System.IO.Path.GetInvalidPathChars())
                filePath = filePath.StrVar().Replace(c, '_');
            FileInfo fi = new FileInfo(filePath.StrVar());
            Assert.IsTrue(fi.Exists,"File does not exist");
            Common.SetEnvString(p1, File.ReadAllText(fi.FullName));
        }


        [Given(@"this feature or scenarios only work on the following browsers")]
        [Given(@"the following feature or scenarios only work on the following browsers")]
        public void GivenTheFollowingFeatureOrScenariosOnlyWorkOfTheFollowingBrowsers(Table table)
        {
            bool browserSupported = false;
            foreach (var r in table.Rows)
            {
                if (r[0].ToLower() == KomodoTestSuite.config.Browser.ToLower())
                {
                    browserSupported = true;
                }
            }

            if (!browserSupported)
                ScenarioContext.Current.Pending();
            
        }

        [Given(@"the following feature or scenarios do not work of the following browsers")]
        public void GivenTheFollowingFeatureOrScenariosDoNotWorkOfTheFollowingBrowsers(Table table)
        {
            bool browserSupported = true;
            foreach (var r in table.Rows)
            {
                if (r["browser"] == KomodoTestSuite.config.Browser)
                {
                    browserSupported = false;
                }
            }

            if (!browserSupported)
                ScenarioContext.Current.Pending();
        }

        [Then(@"add notes to scenario")]
        public void ThenAddNotesToScenario(string multilineText)
        {
            Console.Write(multilineText);
        }
        
        [Then(@"save the date time ranges between '(.*)' and '(.*)'")]
        public void ThenSaveTheDateTimeRangesBetween(string fromDateTime, string toDateTime)
        {
            string fromDate = DateTime.UtcNow.AddMinutes(-15).ToString("o");
            Common.SetEnvString(fromDateTime, fromDate);
            string toDate = DateTime.UtcNow.AddMinutes(15).ToString("o");
            Common.SetEnvString(toDateTime, toDate);
        }

        [Then(@"save the current date time into '(.*)'")]
        public void ThenSaveTheCurrentDateTimeInto(string date)
        {
            Thread.Sleep(1000);
            string dateTime = DateTime.UtcNow.ToString("o");
            Common.SetEnvString(date, dateTime);
            Thread.Sleep(1000);
        }

        [Then(@"generate (.*) in GUID Format")]
        public void ThenEnterRequestIdInGUIDFormat(string entityId)
        {
            Guid guid = Guid.NewGuid();
            string GUID = guid.ToString();
            Common.SetEnvString(entityId, GUID);
        }

        [Then(@"verify '(.*)' equals '(.*)'")]
        public void ThenVerifyStringValueEquals(string text1, string text2)
        {
            StringAssert.AreEqualIgnoringCase(text1.StrVar(), text2.StrVar());
        }
    }
}