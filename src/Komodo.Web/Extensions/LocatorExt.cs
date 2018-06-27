using Komodo.Core.Domain;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Komodo.Core.Extensions
{
    public static class LocatorExt
    {
        public static Locator GetLocatorByKeyName(this IEnumerable<Locator> locators,string pageName, string locatorKeyName)
        {
            return locators.FirstOrDefault(t => t.Page == pageName && t.KeyName.Trim().ToLower() == locatorKeyName.Trim().ToLower())
                ?? locators.GetLocatorByKeyName(locatorKeyName);
        }

        public static Locator GetLocatorByKeyName(this IEnumerable<Locator> locators, string locatorKeyName)
        {
            return locators.FirstOrDefault(t => t.Page == "" && t.KeyName.Trim().ToLower() == locatorKeyName.Trim().ToLower());
        }

        public static By ToWebDriverBy(this Locator locator)
        {
            return LocatorHelper.GetWebDriverBy(locator);
        }

        public static By ByKeyName(this IEnumerable<Locator> locators, string pageName, string keyName)
        {
            return LocatorHelper.GetWebDriverBy(locators.GetLocatorByKeyName(pageName,keyName));
        }

        public static By ByKeyName(this IEnumerable<Locator> locators, string pageName, string keyName, string parameter)
        {
            Thread.Sleep(1500);
            var loc = locators.GetLocatorByKeyName(pageName, keyName);
            return LocatorHelper.GetWebDriverBy(new Locator
            {
                KeyName = loc.KeyName,
                ByType = loc.ByType,
                ByValue = string.Format(loc.ByValue,parameter),
                ByName = loc.ByName,
                Page = loc.Page,
                Description = loc.Description
            });
        }

        public static IWebElement Driver(this IEnumerable<Locator> locators,IWebDriver driver, string keyName)
        {
            Thread.Sleep(1500);
            return driver.FindElement(LocatorHelper.GetWebDriverBy(locators.FirstOrDefault(t => t.KeyName.ToLower().Trim() == keyName.Trim().ToLower())));
        }
    }
}
