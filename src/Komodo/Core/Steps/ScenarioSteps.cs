using TechTalk.SpecFlow;
using Komodo.Core.Extensions;
using Komodo.Core.Support;

namespace Komodo.Core.Steps
{
    [Binding]
    public class ScenarioSteps : SeleniumStepsBase
    {
        [Then(@"create the following scenario variables")]
        public void ThenCreateTheFollowingScenarioVariables(Table table)
        {
            foreach (TableRow tr in table.Rows)
            {
                Common.SetEnvString(tr[0], tr[1].StrVar());
            }
        }

    }
}
