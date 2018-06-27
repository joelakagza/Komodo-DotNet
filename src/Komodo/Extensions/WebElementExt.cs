using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Komodo.Core.Extensions
{
    public static class WebElementExt
    { 
        public static IWebElement Focus(this IWebElement element, int wait = 1)
        {
            var loc = element.Location;
            element.SendKeys("");
            Thread.Sleep(wait * 1000);
            return element;
        }
        
        public static void EnterText(this IWebElement element, string value)
        {
            element.SendKeys(value);
        }

        public static string RegExReplaceText(this IWebElement element, string regexExpression = "[^0-9a-zA-Z]+", string defaultIfNull = null)
        {
            try
            {
                return Regex.Replace(element.Text, regexExpression, "") ?? defaultIfNull;
            }
            catch (Exception)
            {
                return defaultIfNull;
            }
        }

        public static void SelectDropDown(this IWebElement element, string value)
        {
            new SelectElement(element).SelectByText(value);
        }

        public static bool DoesElementExist(this IWebElement element)
        {
            try
            {
                var web = element.Displayed;
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

        public static void ClickIfElementExist(this IWebElement element)
        {
            if (element.DoesElementExist())
                element.Click();
        }

        public static string GetText(this IWebElement element)
        {
            return element.GetAttribute("value");
        }

        public static string GetTextFromSelect(this IWebElement element, int index)
        {
            return new SelectElement(element).Options.Skip(index).FirstOrDefault().Text;
        }

        public static void ClickAndSelectByText(this IWebElement element, string txtValue)
        {
            element.Click();
            new SelectElement(element).SelectByText(txtValue);
        }

        public static void ClickAndSelectByIndex(this IWebElement element, int txtValue)
        {
            element.Click();
            new SelectElement(element).SelectByIndex(txtValue);
        }

        public static void SelectByIndex(this IWebElement element, int txtValue)
        {
            new SelectElement(element).SelectByIndex(txtValue);
        }

        public static void ClearAndSendKeys(this IWebElement element, string txtValue)
        {
            ClearAndSendKeys(element, txtValue, 0);
        }

        public static void ClearAndSendKeys(this IWebElement element, string txtValue, TimeSpan thinkTimeSec)
        {
            ClearAndSendKeys(element, txtValue, thinkTimeSec.Seconds);
        }

        public static void ClearAndSendKeys(this IWebElement element, string txtValue, int thinkTimeSec)
        {
            foreach (var sChar in txtValue.ToCharArray())
            {
                element.Clear();
                element.SendKeys(sChar.ToString());
                Thread.Sleep(thinkTimeSec);
            }
        }
    }
}
