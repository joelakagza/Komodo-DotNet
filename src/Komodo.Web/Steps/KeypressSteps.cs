using System.Threading;
using OpenQA.Selenium.Interactions;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using Komodo.Core;

namespace Komodo.Web.Steps
{
    [Binding]
    public class KeyboardSteps : TestSuiteStepsBase
    {
        #region keypress

        [Then(@"user presses '(.*)' key x(.*)")]
        public void ThenPressXXXXKeyXx(string key, int x)
        {
            Thread.Sleep(3000);
            Actions pop = new Actions(webDriverX);
            for (int i = 0; i < x; i++)
            {
                switch (key.ToLower())
                {
                    case "pageup":
                    case "page up":
                        pop.SendKeys(Keys.PageUp).Perform();
                        break;
                    case "pagedown":
                    case "page down":
                        pop.SendKeys(Keys.PageDown).Perform();
                        break;
                    case "arrowdown":
                    case "arrow down":
                        pop.SendKeys(Keys.ArrowDown).Perform();
                        break;
                    case "arrowup":
                    case "arrow up":
                        pop.SendKeys(Keys.ArrowDown).Perform();
                        break;
                    case "arrow left":
                    case "arrowleft":
                        pop.SendKeys(Keys.Return).Perform();
                        break;
                    case "arrow right":
                    case "arrowright":
                        pop.SendKeys(Keys.Return).Perform();
                        break;
                    case "enter":
                    case "return":
                        pop.SendKeys(Keys.Return).Perform();
                        break;
                    default:
                        break;
                }

            }
        }

        
        [Then(@"press on key '(.*)'")]
        [Then(@"press on '(.*)' key")]
        [Then(@"user presses on key '(.*)'")]
        public void ThenUserPressesXxOnKey(string key)
        {
            ThenPressXXXXKeyXx(key, 1);
        }

        #endregion
    }
}
