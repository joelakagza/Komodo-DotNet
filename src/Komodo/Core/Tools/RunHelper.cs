using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using System.Reflection;
using NUnit.Framework.Internal;
using TechTalk.SpecFlow.Tracing;
using NUnit.Framework.Internal.Filters;
using NUnit.Framework.Interfaces;
using NUnit.Engine;
using Komodo.Core.Model;
using Komodo.Core.Core.Model;
using System.Xml;

namespace Komodo.Core.Tools
{
    public class RunHelper
    {
        KomodoTestQueue testQueue;
        public int retries { get; set; }
        public string[] envFilter { get; set; }
        public string[] browserFilter { get; set; }
        public string[] catFilter { get; set; }
        public string[] testNameFilter { get; set; }
        public string runnerConfigPath { get; set; }      

        public RunHelper()
        {
          testQueue =  new KomodoTestQueue();
        }

        public RunHelper(string runnerConfigPath)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap { ExeConfigFilename = runnerConfigPath };
            Configuration config1 = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            AppSettingsSection app = config1.AppSettings;

            envFilter = app.Settings["envFilter"].Value.Split(',');
            browserFilter = app.Settings["browserFilter"].Value.Split(',');
            catFilter = app.Settings["catFilter"].Value.Split(',');
            testNameFilter = app.Settings["testNameFilter"].Value.Split(',');

            //scheduler = new Scheduler();
        }

        public void RunTests()
        {
            ITestEngine engine = TestEngineActivator.CreateInstance();
            TestSuite mTestSuite = null;
            ITestEventListener listner = null;
            //EventListener li = new NullListener();

            Console.WriteLine("Created amazonSqsClient");
            Console.WriteLine("App Config path:" + AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            var message = testQueue.Get();
            
            if (message == null)
                return;

            // if count greater than 3 then delete message
            if (int.Parse(message.Attributes["ApproximateReceiveCount"]) > 3)
                testQueue.DeleteMessage(queue.QueueUrl, message.ReceiptHandle);


            var testmodel = JsonConvert.DeserializeObject<TestModel>(message.Body);
            Environment.SetEnvironmentVariable("env.testConfig", JsonConvert.SerializeObject(testmodel.ConfigModel));

            // if test assembly does not exists delete message
            if(File.Exists(testmodel.ConfigModel.TestAssembly))
                testQueue.DeleteMessage(message);

         TestPackage  package = TestPackage(testmodel.ConfigModel.TestAssembly);
            AppConfig.Change(package.FullName + ".config");

            TestPackage package = new TestPackage(testmodel.ConfigModel.TestAssembly);
            mTestSuite = new TestSuiteBuilder().Build(package);

            amazonSqsClient.DeleteMessage(queue.QueueUrl, message.ReceiptHandle);

            Console.WriteLine("-> starting tests ....");
            Console.WriteLine("-> Tests:" + testmodel.ConfigModel.TestAssembly);
            Console.WriteLine("-> Category:" + testmodel.Category);

            ITestFilter testFilter = new CategoryFilter(testmodel.Category);
            ITestRunner runner = engine.GetRunner(package);
            XmlNode result = runner.Run(, testFilter);
            TestResult result = mTestSuite.Run(li, testFilter);

            Thread.Sleep(5000);
        }

        public void SubmitTestsAsCatagories(IEnumerable<string> categories, ConfigModel configModel , string configFileArgs,
            string packageFilePath = null)
        {
            List<TestModel> tests = new List<TestModel>();

            foreach (var category in categories)
            {
                tests.Add(new TestModel
                {
                    Category = category,
                    Type = "category",
                    ConfigModel = ConfigFileArgsToConfigModel(configModel,configFileArgs)
                });
            }

            SubmitTestsAsCatagories(tests);
        }

        public void SubmitTestsAsCatagories(List<TestModel> tests)
        {

            testQueue.CreateIfNotExists();

            foreach (var test in tests)
                testQueue.Insert(JsonConvert.SerializeObject(test));
        }

        public void SubmitJobAsTestModel(string testModelAsString)
        {
            AmazonSQSClient amazonSqsClient = new AmazonSQSClient(ConfigurationManager.AppSettings["accessKeyID"],ConfigurationManager.AppSettings["secretAccessKeyID"], RegionEndpoint.EUWest1);

            var queue = amazonSqsClient.GetQueueUrl("EgAutomatedTests");

            var mod = JsonConvert.DeserializeObject<TestModel>(testModelAsString);

            amazonSqsClient.SendMessage(queue.QueueUrl, JsonConvert.SerializeObject(mod));
        }

   
        public void CreateQueueIfNotExists()
        {
            AmazonSQSClient amazonSqsClient = new AmazonSQSClient(ConfigurationManager.AppSettings["accessKeyID"], ConfigurationManager.AppSettings["secretAccessKeyID"],RegionEndpoint.EUWest1);
            amazonSqsClient.CreateQueue("EgAutomatedTests");
        }

        private ConfigModel ConfigFileArgsToConfigModel(ConfigModel configModel,string configFileArgs)
        {
            string[] configFileArguments = configFileArgs.Split(',');

            PropertyInfo[] props = typeof(ConfigModel).GetProperties();
            foreach (var configFileArg in configFileArguments)
            {
                try
                {
                    string key = configFileArg.Split('|')[0];
                    string value = configFileArg.Split('|')[1];

                    foreach (PropertyInfo prop in props)
                    {
                        if (prop.Name.ToLower() == key.ToLower())
                            prop.SetValue(configModel, value);                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return configModel;

        }

        public static ConfigModel GetConfig(string configJson)
        {
            return JsonConvert.DeserializeObject<ConfigModel>(configJson);
        }

        private static ExeConfigurationFileMap GetExeConfigurationFileMap(TestPackage package)
        {
            FileInfo fileInfo = new FileInfo(package.FullName);
            return new ExeConfigurationFileMap { ExeConfigFilename = fileInfo.FullName + ".config" };
        }

 
        }
   
    }
}

