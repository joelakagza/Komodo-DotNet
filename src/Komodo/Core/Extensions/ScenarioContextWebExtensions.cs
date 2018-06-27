using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Komodo.Core.Extensions
{
    public static class ScenarioContextWebExtensions
    {
        public static IWebDriver sDriver(this ScenarioContext scenarioContext)
        {
            var result = (IWebDriver)ScenarioContext.Current["selenium"];
            Assert.IsNotNull(result, "selenium is not started");
            return result;
        }

        public static bool IsSeleniumRunning(this ScenarioContext scenarioContext)
        {
            return ScenarioContext.Current["selenium"] != null;
        }

        public static void SetTestId(this ScenarioContext scenarioContext, IWebDriver selenium, string testId = null)
        {
            ScenarioContext.Current["TestId"] = testId ?? Guid.NewGuid().ToString();
        }

        public static void SetSelenium(this ScenarioContext scenarioContext, IWebDriver selenium)
        {
            ScenarioContext.Current["selenium"] = selenium;
            ScenarioContext.Current["selenium-errors"] = new StringBuilder("");
        }

        public static StringBuilder SeleniumErrors(this ScenarioContext scenarioContext)
        {
            var result = (StringBuilder)ScenarioContext.Current["selenium-errors"];
            Assert.IsNotNull(result, "selenium is not started");
            return result;
        }
    }

}
