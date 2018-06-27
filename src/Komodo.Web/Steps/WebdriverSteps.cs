using Komodo.Core;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Komodo.Web.Steps
{
    [Binding]
    public class WebdriverSteps : TestSuiteStepsBase
    {
        [Then(@"verify page title equals '(.*)'")]
        public void GivenVerifyPageTitleEqualsXx(string pageTitle)
        {
            Assert.AreEqual(webDriverX.Title, pageTitle, webDriverX.Title);
        }

        [Then(@"verify page title contains '(.*)'")]
        public void ThenVerifyPageTitleContains(string pageTitle)
        {
            Assert.IsTrue(webDriverX.Title.IndexOf(pageTitle) > -1, webDriverX.Title);
        }


    }
}
