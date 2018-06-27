using Komodo.Core.Model;
using Komodo.Core.Steps;
using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace Komodo.Core.Events
{
    [Binding]
    public class DefaultEvent
    {
        [BeforeStep(Order = 0)]
        public void BeforeStep()
        {
            
        }

        [AfterStep(Order = 0)]
        public void AfterStep()
        {
            

        }

        [BeforeScenarioBlock(Order = 0)]
        public void BeforeScenarioBlock()
        {
            //TODO: implement logic that has to run before each scenario block (given-when-then)

        }

        [AfterScenarioBlock(Order = 0)]
        public void AfterScenarioBlock()
        {
            //TODO: implement logic that has to run after each scenario block (given-when-then)
        }

        [BeforeScenario(Order = 0)]
        public void BeforeScenario()
        {
            try
            {
                Common.SetEnvString("feature", FeatureContext.Current.FeatureInfo.Title);
                Common.SetEnvString("scenario", ScenarioContext.Current.ScenarioInfo.Title);
                Console.WriteLine("-> Feature: " + Common.GetEnvString("feature"));
                Console.WriteLine("-> Scenario: " + Common.GetEnvString("scenario"));

                KomodoTestSuite.SetConfigFromLocalAppConfig();
                Common.LoadAppSettingsIntoScenarioContext();
                KomodoTestSuite.SetConfigFromEnvironmentVariables();

                KomodoTestSuite.sb = new StringBuilder();
                KomodoTestSuite.stepCount = 0;

                ScenarioContext.Current.SetTestId(KomodoTestSuite.driverCache, Guid.NewGuid().ToString());
                KomodoTestSuite.config.TestId = (string) ScenarioContext.Current["TestId"];

                ScenarioContext.Current.Set(DateTime.Now, "Date");
                ScenarioContext.Current.Set("Pending", "Result");
                ScenarioContext.Current.Set(KomodoTestSuite.config.Environment, "%{env}");
                ScenarioContext.Current.Set(KomodoTestSuite.config.Environment, "%{environment}");
                ScenarioContext.Current.Set(NUnit.Framework.TestContext.CurrentContext.Test.FullName, "TestName");

                KomodoTestSuite.realResult = new ResultsRealVw();
                KomodoTestSuite.realResult.Env = KomodoTestSuite.config.Environment;
                KomodoTestSuite.realResult.Browser = KomodoTestSuite.config.Browser.Replace(" ", "_") +
                                                     KomodoTestSuite.config.WindowSize;
                KomodoTestSuite.realResult.Version = KomodoTestSuite.config.Version;
                KomodoTestSuite.realResult.FeatureDate = FeatureContext.Current.Get<DateTime>("Date");
                KomodoTestSuite.realResult.ScenarioDate = ScenarioContext.Current.Get<DateTime>("Date");
                KomodoTestSuite.realResult.Feature = FeatureContext.Current.FeatureInfo.Title;
                KomodoTestSuite.realResult.Scenario = ScenarioContext.Current.ScenarioInfo.Title;

                Regex rg = new Regex(@"\(.+\)");
                if (rg.IsMatch(NUnit.Framework.TestContext.CurrentContext.Test.Name.ToString()))
                {
                    string parameterValues = rg.Match(NUnit.Framework.TestContext.CurrentContext.Test.Name).Value;
                    IDictionary pop = NUnit.Framework.TestContext.CurrentContext.Test.Properties;

                    KomodoTestSuite.realResult.Scenario = ScenarioContext.Current.ScenarioInfo.Title + " ";
                    KomodoTestSuite.realResult.Scenario += parameterValues.Replace(" ", "-");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                if (ex.InnerException != null)
                    Console.Write(ex.InnerException.Message ?? "");
            }

        }

        [AfterScenario(Order = 0)]
        public void AfterScenario()
        {
            try
            {
                KomodoTestSuite.LogScenarioResults();
            }
            catch (Exception ex)
            {
               Console.WriteLine("Results for scenario not logged into db, check the db exists and connection");
               Console.WriteLine("Error:" +ex.Message);
            }
        }

        [AfterScenario(Order = 1)]
        public void AfterScenarioBrowserStack()
        {
            try
            {
                var result = (ScenarioContext.Current.TestError == null ? "completed" : "error");
                var message = ScenarioContext.Current.TestError == null ? "" : ScenarioContext.Current.TestError.ToString();
                KomodoTestSuite.browserStackAutomateClient.SetSessionStatusAsync(KomodoTestSuite.config.RemoteSessionId, result, message);
                var session =
                    KomodoTestSuite.browserStackAutomateClient.GetSessionAsync(KomodoTestSuite.config.RemoteSessionId);
                KomodoTestSuite.sb.Append("<b>" + session.automation_session.browser_url + Environment.NewLine + "</b>");
                KomodoTestSuite.sb.Append(session.automation_session.logs + Environment.NewLine);

                KomodoTestSuite.realResult.JobAssets +=
                    string.Format(@"<a href=""{0}"">{1}</a>", session.automation_session.browser_url, "Session Details") +
                    "</br>";

                KomodoTestSuite.realResult.JobAssets +=
                    string.Format(@"<a href=""{0}"">{1}</a>", session.automation_session.logs, "Logs") + "</br>";
            }
            catch (Exception ex)
            {

            }


        }



        [BeforeFeature(Order = 0)]
        public static void BeforeFeature()
        {
            
            KomodoTestSuite.stepCount = 0;
            FeatureContext.Current.Set(KomodoTestSuite.config.Environment, "env");
            FeatureContext.Current.Set(DateTime.Now, "Date");
            FeatureContext.Current.Set("Pending", "Result");
            
        }

        [AfterFeature(Order = 0)]
        public static void AfterFeature()
        {

        }

        [BeforeTestRun(Order = 0)]
        public static void BeforeTestRun()
        {
            //TODO: implement logic that has to run before the entire test run
        }

        [AfterTestRun(Order = 0)]
        public static void AfterTestRun()
        {
            //TODO: implement logic that has to run after the entire test run
        }
    }
}
