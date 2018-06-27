using System;
using TechTalk.SpecFlow;

namespace Komodo.Core.Events
{
    [Binding]
    public sealed class Web
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario("web", Order = 50)]
        public static void BeforeWebScenario()
        {
            TestSuite.StartWeb();
            TestSuite.driverCache.Manage().Timeouts().ImplicitWait = (TimeSpan.FromSeconds(10));
        }

        [AfterScenario("web", "reuse", Order = 50)]
        public static void AfterWebScenario()
        {
            if (!TestSuite.ReuseWebSession)
                TestSuite.StopWeb();
        }

    }
}
