using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Komodo.Core.Events
{
    [Binding]
    public class WebEvents
    {

        [BeforeScenario("web", Order = 50)]
        public static void BeforeWebScenario()
        {
            SeleniumSupport.StartTestSuite();
        }

        [AfterScenario("web", Order = 50)]
        public static void AfterWebScenario()
        {
            Alerts.AlertDismissAll(SeleniumSupport.driverCache);
            if (SeleniumSupport.ReuseWebSession)
                SeleniumSupport.StopSelenium();

            Assert.AreEqual("", ScenarioContext.Current.SeleniumErrors().ToString(), "Selenium verification errors");
        }
    }
}
