using Komodo.Core.Domain;
using Komodo.Core.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komodo.Web.Extensions
{
    public static class WebDriverExt
    {
        public static IWebElement FindElement(this IWebDriver webdriver, Locator locator)
        {
            return webdriver.FindElement(locator.ToWebDriverBy());
        }

        public static bool DoesElementExist(this IWebDriver webdriver, Locator locator)
        {
            try
            {
                var web = webdriver.FindElement(locator.ToWebDriverBy()).Displayed;
                return true;
            }
            catch (NoSuchElementException nse)
            {
                Console.WriteLine(nse.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public static bool DoesElementExist(this IWebDriver webdriver, By byLocator)
        {
            try
            {
                var web = webdriver.FindElement(byLocator).Displayed;
                return true;
            }
            catch (NoSuchElementException nse)
            {
                Console.WriteLine(nse.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public static void ClickIfElementExist(this IWebDriver webdriver, By by)
        {
            try
            {
                if (webdriver.FindElement(by).DoesElementExist())
                    webdriver.FindElement(by).Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static IWebElement FindVisibleElement(this IWebDriver driver, By by)
        {
            var elements = driver.FindElements(by);
            foreach (var element in elements.Where(e => e.Displayed))
            {
                return element;
            }
            throw new NoSuchElementException("Unable to find visible element with " + @by);
        }

        public static bool VisibleElementExists(this IWebDriver driver, By by, Int32 implicitWait = 10)
        {
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 0);
            var elements = driver.FindElements(by);
            var visibleElements = elements.Count(e => e.Displayed);
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, implicitWait);
            return visibleElements != 0;
        }
    }
}
