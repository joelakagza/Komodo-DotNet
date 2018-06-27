using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using NUnit.Framework;
using Komodo.Core;

namespace Komodo.Web.Steps
{
    [Binding]
    public  class CheckboxSteps : TestSuiteStepsBase
    {

        #region checkboxes

        [Then(@"click on checkbox '(.*)'")]
        public void ThenClickOnCheckboxs(string locator)
        {
           webDriverX.FindElement(locator).Click();
        }

        #endregion

    }
}