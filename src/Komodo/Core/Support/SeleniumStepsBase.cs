using Komodo.Core.Extensions;
using Komodo.Core.Model;
using Komodo.Core.Steps;
using OpenQA.Selenium;

namespace Komodo.Core.Support
{
    public abstract class SeleniumStepsBase : TechTalk.SpecFlow.Steps
    {
        public IWebDriver sDriver
        {
            get { return TechTalk.SpecFlow.ScenarioContext.Current.sDriver(); }
        }

        public ConfigModel ConfigSettings
        {
            get { return KomodoTestSuite.config; }
        }

    }
}
