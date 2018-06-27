using Komodo.Core;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace Komodo.Web.Steps
{
    [Binding]
    public class CookieSteps : TestSuiteStepsBase
    {
        #region cookies

        [Then(@"delete all cookies")]
        public void ThenDeleteAllCookies()
        {
            webDriverX.Manage().Cookies.DeleteAllCookies();
        }

        [Then(@"delete the cookie '(.*)'")]
        public void ThenDeleteTheCookie(string cookie)
        {
            webDriverX.Manage().Cookies.DeleteCookie(webDriverX.Manage().Cookies.GetCookieNamed(cookie));
        }

        [Then(@"copy cookie '(.*)' to domain '(.*)' return url '(.*)'")]
        public void ThenCopyCookieToDomain(string cookie, string domain,string returnUrl)
        {
            Cookie c = webDriverX.Manage().Cookies.GetCookieNamed(cookie);
            Cookie newCook = new Cookie(c.Name,c.Value,domain,c.Path,c.Expiry);
            webDriverX.Navigate().GoToUrl(returnUrl);
            webDriverX.Manage().Cookies.AddCookie(newCook);
        }

        /// <summary>
        /// Create cookie from table 
        /// </summary>
        /// <param name="table"></param>
        [Given(@"that we create the following cookies")]
        public void GivenThatWeCreateTheFollowingCookies(Table table)
        {
            foreach (TableRow rw in table.Rows)
            {
                if (rw["host"] == "")
                    rw["host"] = new Uri(webDriverX.Url).Host;

                if (rw["path"] == "")
                    rw["path"] = "/";

                Cookie cookie = new Cookie(rw["name"],rw["content"], rw["host"], rw["path"],
                                           DateTime.Now.AddDays(7));
                
                webDriverX.Manage().Cookies.AddCookie(cookie);
            }
        }


        #endregion

    }
}
