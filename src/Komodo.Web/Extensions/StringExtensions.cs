using OpenQA.Selenium;

namespace Komodo.Web.Extensions
{
    public static class StringExtensions
    {
        public static By ToByXpath(this string xpath)
        {
            return By.XPath(xpath);
        }

    }
}
