using System;
using System.Configuration;
using System.IO;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using TechTalk.SpecFlow;
using System.Drawing.Imaging;
using Komodo.Core.Support;

namespace Komodo.Core.Steps
{
    public class ScreenShot
    {
        /// <summary.
        /// Saves a screenshot of the current error page
        /// </summary>
        public static void SaveScreenShot(string fileName, IWebDriver sDriver)
        {
            try
            {
                //Get the screenshot
                Type driverType = sDriver.GetType();
                string fileExt = ".jpg";
                Screenshot screenshot = null;

                if (KomodoTestSuite.config.Browser== "htmlunit")
                    fileExt = ".html";

                if (KomodoTestSuite.config.RemoteWebDriver != "true")
                {
                    switch (KomodoTestSuite.config.Browser.ToLower())
                    {
                        case "firefox":
                            FirefoxDriver ff = (FirefoxDriver)sDriver;
                            screenshot = ff.GetScreenshot();
                            break;
                        case "iexplore":
                            InternetExplorerDriver ie = (InternetExplorerDriver)sDriver;
                            screenshot = ie.GetScreenshot();
                            break;
                        case "chrome":
                            ChromeDriver chr = (ChromeDriver)sDriver;
                            screenshot = chr.GetScreenshot();
                            break;
                        case "safari":
                            SafariDriver safD = (SafariDriver)sDriver;
                            screenshot = safD.GetScreenshot();
                            break;
                        case "android":
                            //AndroidDriver an = (AndroidDriver)sDriver;
                            //screenshot = an.GetScreenshot();
                            break;
                        case "htmlunit":
                            fileExt = ".html";
                            break;
                        default:
                            break;
                    }
                }

                if (KomodoTestSuite.config.RemoteWebDriver == "true" && fileExt != ".html")
                {
                    screenshot = ((ScreenShotRemoteWebDriver)sDriver).GetScreenshot();
                }

                //Build up on our filename
                StringBuilder filename = new StringBuilder(fileName);
                filename.Append("-");
                filename.Append(String.Format("{0:yyyyMMdd}", DateTime.Now));//DateTime.Now.ToShortDateString().Replace("/", "_"));
                filename.Append(fileExt);

                FileInfo fileInfo = new FileInfo(filename.ToString());
                if (!Directory.Exists(fileInfo.FullName))
                    Directory.CreateDirectory(fileInfo.DirectoryName);

                //save the image
                if (fileExt == ".jpg")
                {
                    Common.SaveScreenShot(filename.ToString(),screenshot);
                }
                else 
                {
                    Common.SaveFile(fileName.ToString(),sDriver.PageSource);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Error: Unable to take screenshot, at the moment this will not work with remotewebdriver");
                //   throw;
            }
        }

    }
}
