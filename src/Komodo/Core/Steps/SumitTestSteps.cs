using System;
using System.Collections.Generic;
using Komodo.Core.Model;
using TechTalk.SpecFlow;
using Komodo.Core.Support;
using Newtonsoft.Json;
using Komodo.Core.Core.Model;
using Komodo.Core.Tools;

namespace Komodo.Core.Steps
{
    [Binding]
    public class SumitTestSteps : SeleniumStepsBase
    {
        public TestModel TestModel;

        [Then(@"submit the following tests to be processed dll name:'(.*)'")]
        public void ThenSubmitTheFollowingTestsToBeProcessed(string dllName ,Table table)
        {
            List<TestModel> tests = new List<TestModel>();
            var config = Common.SetConfigFromLocalAppConfig();

            config.RemoteHub = "http://hub.browserstack.com/wd/hub/";
            config.RemoteWebDriver = "browserstack";
            config.RemoteUserName = "joelhunt";
            config.RemoteAccessKey = "";
            config.RemoteProxyHost = "";
            config.RemoteProxyPort = "3128";
            config.ResultsPath = @"c:\inetpub\Testing\Results\";
            config.TestToolsFolder = @"C:\inetpub\Testing\Tools\";
            config.TestAssembly =string.Format(@"c:\inetpub\testing\{0}\bin\{0}.dll", dllName, dllName);

            foreach (var row in table.Rows)
            {
                var newConfig = JsonConvert.DeserializeObject<ConfigModel>(JsonConvert.SerializeObject(config));
                foreach (var category in row["tags"].Split(new [] {","},StringSplitOptions.RemoveEmptyEntries))
                {
                    newConfig.Environment = row["Environment"];
                    newConfig.Browser = row["Browser"];
                    newConfig.BrowserVersion = row["BrowserVersion"];
                    newConfig.Version = row["BrowserVersion"];
                    newConfig.Platform = row["Platform"];

                    var newModel = new TestModel()
                    {
                        Category = category,
                        Type = "category",
                        ConfigModel = newConfig
                    };

                    tests.Add(newModel); 
                }
               
            }

            RunHelper runner = new RunHelper();

            
            runner.SubmitTestsAsCatagories(tests);
        }

    }
}