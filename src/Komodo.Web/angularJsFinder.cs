using OpenQA.Selenium;
using Protractor;

namespace Komodo.Web
{
    public class NgByModel : By
    {
        public NgByModel(string locator, IWebDriver webdriverX)
        {
            FindElementMethod = context => webdriverX.FindElement(NgBy.Model(locator));
        }
    }

    public class NgByRepeaterFinder : By
    {
        public NgByRepeaterFinder(string locator, IWebDriver webdriverX)
        {
            FindElementMethod = context => webdriverX.FindElement(NgBy.Repeater(locator));
        }
    }

    public class NgByBindingFinder : By
    {
        public NgByBindingFinder(string locator, IWebDriver webdriverX)
        {
            FindElementMethod = context => webdriverX.FindElement(NgBy.Binding(locator));
        }
    }
}
