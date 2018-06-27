using Komodo.Core;
using Komodo.Core.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using TechTalk.SpecFlow;

namespace Komodo.Web.Steps
{
    [Binding]
    public class CommonWebSteps : TestSuiteStepsBase
    {
        #region Navigations links, hyperlinks , buttons

        [Then(@"navigate to '(.*)' page via clicking '(.*)'")]
        [Then(@"navigate to page '(.*)' via clicking '(.*)'")]
        public void ThenNavigateToPageVia(string page, string p0)
        {
            string[] links = p0.Split('>');
            foreach (string link in links)
            {
                webDriverX.WaitForElementLoad(link.Trim(), 60);
                webDriverX.FindElement(link.Trim()).Click();
            }
        }

        [Then(@"do multiple clicks with the following '(.*)'")]
        public void ThenDoMultipleClicks(string clicks)
        {
            var links = clicks.Split(new[] { '>' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(t => !string.IsNullOrEmpty(t.Trim())).ToList();
            foreach (string link in links)
            {
                webDriverX.WaitForElementLoad(link.Trim(), 30);
                webDriverX.FindElement(link.Trim()).Click();
            }
        }

        [Then(@"user navigates to url '(.*)'")]
        [Given(@"user navigates to url '(.*)'")]
        public void GivenUserNavigatesToUrlXxxx(string url)
        {
            webDriverX.Navigate().GoToUrl(url.StrVar());
            var ps = webDriverX.PageSource;
        }

        [Given(@"user navigates to page '(.*)'")]
        [Then(@"user navigates to page '(.*)'")]
        public void ThenUserNavigatesToPageXx(string page)
        {
            Uri newUrl1 = new Uri(webDriverX.Url);
            webDriverX.Navigate().GoToUrl(newUrl1.OriginalString);
        }

        [Given(@"user has landed on page named '(.*)'")]
        public void GivenUserHasLandedOnPageNamedXx(string page)
        {
            Console.WriteLine(webDriverX.Title);
            Assert.AreEqual(page, webDriverX.Title);
        }

        [Then(@"click on tab '(.*)'")]
        [Then(@"click on toggle '(.*)'")]
        [When(@"i click on link '(.*)'")]
        [When(@"i click on '(.*)' link")]
        [When(@"I click on '(.*)' link")]
        [When(@"i click on the '(.*)' button")]
        [Given(@"user clicks on link '(.*)'")]
        [Given(@"I click on link '(.*)'")]
        [Given(@"i click on link '(.*)'")]
        [Then(@"click on link '(.*)'")]
        [Then(@"click on the '(.*)' button")]

        public void ThenClickOnLinkXx(string link)
        {
            webDriverX.ClickIfElementExist(link.StrVar());
        }

        [Then(@"click on browser back button")]
        [Then(@"click the browser back button")]
        public void ThenClickTheBrowserBackButton()
        {
            ((IJavaScriptExecutor)webDriverX).ExecuteScript("history.back();");
        }

        [Then(@"click on item '(.*)' from the '(.*)'")]
        public void ThenClickOnItemXxFromTheXx(int selectIndex, string locator)
        {
            webDriverX.FindElements(locator).ElementAt(selectIndex - 1).Click();
        }


        #endregion

        #region verify actions

        [Then(@"verify querystring nvp contains")]
        [Then(@"verify query string contains")]
        public void ThenVerifyQueryStringContains(Table table)
        {
            Uri ur = new Uri(webDriverX.Url);
            NameValueCollection nvp = HttpUtility.ParseQueryString(ur.Query);
            foreach (TableRow dr in table.Rows)
            {
                bool pass = false;
                string name = dr["name"];
                string value = dr["value"];

                if (nvp[name] != null)
                {
                    Console.WriteLine("name: {0} Values:{1},{2}", name, nvp.GetValues(name)[0], value);
                    if (nvp.GetValues(name)[0] == value)
                    {
                        pass = true;
                    }
                }
                Assert.IsTrue(pass);
            }
        }

        [Then(@"verify page furniture in the '(.*)' section")]
        public void ThenVerifyPageFurnitureInTheSection(string p0, Table table)
        {
            var webElemet = webDriverX.FindElement(p0);

            //webElemet.FindElements()
        }

        #endregion


    }
}
