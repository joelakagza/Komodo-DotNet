using Komodo.Core;
using Komodo.Core.Extensions;
using OpenQA.Selenium;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Komodo.Web.Steps
{
    [Binding]
    public class DropDownListSteps : TestSuiteStepsBase
    {

        #region dropdownlist/dropdowns

        [Then(@"select item (.*) from dropdownlist '(.*)'")]
        public void ThenSelectItemXxFromDropdownlistXx(int itemNo, string locator)
        {
            IWebElement select = webDriverX.FindElement(locator);
            ICollection<IWebElement> options = select.FindElements(By.TagName("option"));
            int i = 0;
            foreach (IWebElement option in options)
            {
                if (i == itemNo)
                {
                    option.Click();
                }
                i++;
            }

        }

        [Then(@"click and select '(.*)' from dropdownlist '(.*)'")]
        [Then(@"click and select '(.*)' from '(.*)' dropdownlist")]
        public void ThenClickAndSelectXxFromDropdownlistXx(string optionValue, string locator)
        {
            IWebElement select = webDriverX.FindElement(locator);
            select.Click();
            ICollection<IWebElement> options = select.FindElements(By.TagName("option"));

            foreach (IWebElement option in options)
            {
                if (option.Text == optionValue)
                {
                    option.Click();
                    break;
                }
            }
        }

        [Then(@"select item '(.*)' from '(.*)' dropdown")]
        [Then(@"select the item (.*) from '(.*)' dropdown")]
        public void ThenSelectTheItemXxFromXxDropdown(int itemNo, string locator)
        {
            IWebElement select = webDriverX.FindElement(locator);
            ICollection<IWebElement> options = select.FindElements(By.TagName("option"));
            int i = 0;
            foreach (IWebElement option in options)
            {
                if (i == itemNo)
                {
                    option.Click();
                }
                i++;
            }
        }

        #endregion
    }

}

