using Komodo.Core;
using OpenQA.Selenium;
using System;
using System.Configuration;
using System.Threading;

namespace Komodo.Web
{
    public class Alerts
    {
        public static string GetAlertText(IWebDriver webdriverX)
        {
            IAlert alert = webdriverX.SwitchTo().Alert();
            // Get the text from the alert
            string alertText = alert.Text;
            // Accept the alert
            alert.Accept();

            return alertText;
        }

        public static void OkAlert(IWebDriver webdriverX)
        {
            if (ConfigurationManager.AppSettings["browserName"] != "htmlunit")
            {
                IAlert alert = webdriverX.SwitchTo().Alert();
                // Get the text from the alert.
                string alertText = alert.Text;
                // Accept the alert
                alert.Accept();
            }
        }

        public static void AlertDismiss(IWebDriver webdriverX)
        {
            IAlert alert = webdriverX.SwitchTo().Alert();
            // Get the text from the alert
            string alertText = alert.Text;
            // Accept the alert
            alert.Accept();
            Thread.Sleep(3000);
        }

        public static void AlertDismissAll(IWebDriver webdriverX)
        {
            bool alertBool = false;

            try
            {
                // Get a handle to the open alert, prompt or confirmation
                if (webdriverX.SwitchTo().Alert() != null)
                {
                    alertBool = true;
                }
            }
            catch (Exception)
            {
                //
                //Assert.Fail();
                //throw;
            }

            if (alertBool)
            {
                AlertDismiss(webdriverX);
                Console.WriteLine("An alert was found, please dismiss all alerts before ending scenario");
                //ScenarioContext.Current.Pending();
                //Assert.Fail(); 
            }
        }
    }
}
