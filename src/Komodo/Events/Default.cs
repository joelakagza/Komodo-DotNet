using Komodo.Core.Data;
using TechTalk.SpecFlow;

namespace Komodo.Core.Events
{
    [Binding]
    public sealed class Default
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        [BeforeScenario(Order = 0)]
        public void BeforeScenario()
        {
            TestSuite.StartTestSuite();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }
    }
}
