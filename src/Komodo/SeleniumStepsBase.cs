using OpenQA.Selenium;
using System.Collections.Generic;
using Komodo.Core.Extensions;
using Komodo.Core.Models;
using Komodo.Core.Domain;

namespace Komodo.Core
{
    public abstract class SeleniumStepsBase : TechTalk.SpecFlow.Steps
    {
        public IWebDriver webDriver
        {
            get { return TechTalk.SpecFlow.ScenarioContext.Current.webDriver(); }
        }

        public WebDriverX webDriverX
        {
            get { return TestSuite.webdriverX; }
        }

        public List<Locator> Locators
        {
            get { return TestSuite.webLocators; }
        }
    }
}
