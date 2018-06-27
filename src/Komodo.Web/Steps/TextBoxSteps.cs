using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Support.UI;
using Komodo.Core;
using Komodo.Core.Extensions;
using NUnit.Framework;

namespace Komodo.Web.Steps
{
    [Binding]
    public class TextBoxes : TestSuiteStepsBase
    {
        #region textbox

        [Then(@"verify element '(.*)' text equals '(.*)'")]
        public void ThenVerifyElementTextEquals(string locator, string text)
        {
            Assert.AreEqual(webDriverX.FindElement(locator).Text, text);
        }


        [Then(@"clear text box '(.*)'")]
        [Then(@"clear textbox '(.*)'")]
        public void ThenClearTextboxUsernameEmailAddress(string locator)
        {
           webDriverX.FindElement(locator).Clear();
        }

        [Then(@"enter '(.*)' into '(.*)' textbox")]
        [Then(@"enter '(.*)' into the '(.*)' text box")]
        [Then(@"enter '(.*)' into the '(.*)' textbox")]
        public void ThenEnterAuctionIntoTheNewsSearchTextbox(string text, string locator)
        {
            webDriverX.WaitForElementLoad(locator);
            webDriverX.FindElement(locator).SendKeys(text.StrVar());
        }

        [Then(@"enter '(.*)' into the '(.*)' textbox thinktime (.*) seconds")]
        public void ThenEnterXxIntoTheXxTextbox(string text, string locator, TimeSpan thinkTime)
        {
            webDriverX.FindElement(locator).ClearAndSendKeys(text.StrVar(), thinkTime); 
        }

        [StepArgumentTransformation(@"thinktime (\d+) seconds?")]
        public ThinkTime InXDaysTransform(int thinkTime)
        {
            return new ThinkTime(thinkTime);
        }

        [StepArgumentTransformation]
        public ThinkTime ThinkTimeTransform(string thinkTime)
        {
            return new ThinkTime(int.Parse(thinkTime));
        }

        public class ThinkTime 
        {
            public ThinkTime(int value)
            {
                this.Value = value * 1000;
            }
            public int Value { get; private set; }
        }
        #endregion
    }
}
