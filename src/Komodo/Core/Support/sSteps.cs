using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;

namespace Komodo.Core.Steps
{
    static  public partial  class SeleniumStepsExtensions
    {

        public static bool DoesWebElementExist(IWebDriver driver, By selector)
        {
            try
            {
                driver.FindElement(selector);
                return true;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }

        public static bool WaitForElement(IWebDriver driver, By selector, int timeSec)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));
            for (int i = 0; i < 60; i++)
            {
                try
                {
                    driver.FindElement(selector);
                    return true;
                }
                catch (NoSuchElementException e)
                {

                }
                Thread.Sleep(1000);
            }

            return false;
        }

        public static bool WaitForElement(IWebDriver driver, IWebElement iWeb, int timeSec)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));
            for (int i = 0; i < 60; i++)
            {
                try
                {
                    if (iWeb.Displayed) { return true; }
                    //return true;
                }
                catch (NoSuchElementException e)
                {

                }
                Thread.Sleep(1000);
            }

            return false;
        }

   

        public static bool WaitForText(IWebDriver driver, string strText, int timeSec, Boolean ignoreCase)
        {
            if (timeSec == null) { timeSec = 60; }
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));
            for (int i = 0; i < timeSec; i++)
            {
                try
                {
                    if (ignoreCase)
                    {
                        if (driver.PageSource.ToLower().Contains(strText.ToLower()) == true) { return true; }
                    }
                    else
                    {
                        if (driver.PageSource.Contains(strText) == true) { return true; }
                    }
                    
                }
                catch (NoSuchElementException e)
                {
                    Console.WriteLine("Error:" + strText);
                }
                Thread.Sleep(1000);
            }

            return false;
        }

        public static bool WaitForText(IWebDriver driver, string strText, int timeSec)
        {
            return WaitForText(driver, strText, timeSec, true);
        }

        public static bool WaitForElementNotVisible(IWebDriver driver, By selector, int timeSec)
        {

            try
            {
                driver.FindElement(selector, timeSec);

            }
            catch
            {
                Assert.Fail();
            }

            return true;
        }

    }


}
