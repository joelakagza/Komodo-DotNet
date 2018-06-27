using Komodo.Core.Domain;
using Komodo.Core.Extensions;
using Komodo.Core.Model;
using Komodo.Core.Models;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using TechTalk.SpecFlow;

namespace Komodo.Core
{
    public static partial class TestSuiteWeb 
    {
        public static IWebDriver driverCache = null;
        public static WebDriverX webdriverX;
        
        public static List<Locator> webLocators;
        private static TimeSpan INIT_TIMEOUT_SEC = TimeSpan.FromSeconds(180); /* Change this to a more reasonable value */
        private static TimeSpan IMPLICIT_TIMEOUT_SEC = TimeSpan.FromSeconds(10); /* Change this to a more reasonable value */
        private static DesiredCapabilities capabillities;
        private static DriverOptions driverOptions;
        public static ConfigModel config { get; set; }
        public static TestResult result { get; set; }

        public static bool ReuseWebSession
        {
            get { return (bool.Parse( ConfigurationManager.AppSettings.Get("ReuseWebSession"))); }
        }

        public static void SetConfigFromLocalAppConfig()
        {
            try
            {
                config.BuildNo = ConfigurationManager.AppSettings.Get("build") ?? "";
                config.Browser = ConfigurationManager.AppSettings.Get("browserName") ?? "";
                config.Version = ConfigurationManager.AppSettings.Get("version") ?? "";
                config.BrowserVersion = ConfigurationManager.AppSettings.Get("browserVersion") ?? "";
                config.Platform = ConfigurationManager.AppSettings.Get("platform");
                config.RemoteHub = ConfigurationManager.AppSettings.Get("remoteHub");
                config.Source = ConfigurationManager.AppSettings.Get("remoteWebDriver");
                config.Environment = ConfigurationManager.AppSettings.Get("environment");
                config.DeviceName = ConfigurationManager.AppSettings.Get("deviceName");
                config.DeviceOrientation = ConfigurationManager.AppSettings.Get("deviceOrientation");
                config.RemoteUserName = ConfigurationManager.AppSettings.Get("remoteUserName");
                config.RemoteAccessKey = ConfigurationManager.AppSettings.Get("remoteAccessKey");
                config.RemoteLocalByPass = ConfigurationManager.AppSettings.Get("remoteLocalByPass") ?? "true";
                config.WindowSize = ConfigurationManager.AppSettings.Get("windowSize") ?? "";
                config.ResultsPath = ConfigurationManager.AppSettings.Get("resultsPath");
                config.RemoteWebDriver = ConfigurationManager.AppSettings.Get("remoteWebDriver");
                config.IeServer = ConfigurationManager.AppSettings.Get("ieServer");
                config.ChromeServer = ConfigurationManager.AppSettings.Get("chromeServer");
                config.UseCloudStorage = ConfigurationManager.AppSettings.Get("useCloudStorage");
                config.UseFileSystemAsStorage = ConfigurationManager.AppSettings.Get("useFileSystemAsStorage");
                config.TestToolsFolder = ConfigurationManager.AppSettings.Get("testToolsFolder");

            }
            catch (Exception ex)
            {
                Console.WriteLine("SetConfigFromLocalAppConfig:" + ex.Message);
            }
        }

        public static void StartTestSuite()
        {
            config = new ConfigModel();
            Console.WriteLine("-> TestAssembly: " + config.TestAssembly);
            Console.WriteLine("-> Environment: " + config.Environment);
            Console.WriteLine("-> Selenium Version: " + "2.0");
            Console.WriteLine("-> Version: " + config.Version);
            Console.WriteLine("-> Build: " + config.BuildNo);
            Console.WriteLine("-> Selected browser started: " + config.Browser + " " + config.BrowserVersion + " " + config.WindowSize);
            Console.WriteLine("-> Remote Webrdiver: " + config.RemoteWebDriver);
            Console.WriteLine("-> Remote Source: " + config.Source);
            Console.WriteLine("-> Remote Webrdiver server: " + config.RemoteHub);
            Console.WriteLine("-> Platform: " + config.Platform.StrVar());
            Console.WriteLine("-> Device: " + config.DeviceName);
            Console.WriteLine("-> Device Orientation: " + config.DeviceOrientation);
            Console.WriteLine("-> Results Path: " + config.ResultsPath.StrVar());
            Console.WriteLine("-> TestId: " + config.TestId);
        }

        public static void StartWeb()
        {
            capabillities = new DesiredCapabilities();
            webLocators = LocatorHelper.LoadLocators("locators/locators.csv");
            var browser = ConfigurationManager.AppSettings.Get("Browser");

            switch (browser.ToLower())
            {
                case "chrome":
                    capabillities = DesiredCapabilities.Chrome();
                    StartChromeDriver();
                    break;
                case "firefox":
                    capabillities = DesiredCapabilities.Firefox();
                    StartFireFoxDriver();
                    break;
                case "edge":
                    capabillities = DesiredCapabilities.Edge();
                    StartEdgeDriver();
                    break;
                case "htmlunit":
                    capabillities = DesiredCapabilities.HtmlUnit();
                    StartHtmlUnitDriver();
                    break;
                default:
                    break;
            }
        }

        private static void SetDesiredCapablities(string deviceOrbrowser)
        {
            switch (deviceOrbrowser.ToLower())
            {
                case "chrome":
                    capabillities = DesiredCapabilities.Chrome();
                    break;
                case "firefox":
                    capabillities = DesiredCapabilities.Firefox();
                    break;
                case "edge":
                    capabillities = DesiredCapabilities.Edge();
                    break;
                case "ipad":
                    capabillities = DesiredCapabilities.IPad();
                    break;
                case "iphone":
                    capabillities = DesiredCapabilities.IPhone();
                    break;
                case "ie":
                case "internetexplorer":
                    capabillities = DesiredCapabilities.InternetExplorer();
                    break;
                default:
                    break;
            }
        }

        private static void BrowserStack()
        {
            try
            {
                capabillities.SetCapability("name", TestSuite.result.Scenario);
                capabillities.SetCapability("browserstack.user", config.RemoteUserName);
                capabillities.SetCapability("browserstack.key", config.RemoteAccessKey);
                capabillities.SetCapability(CapabilityType.AcceptSslCertificates, true);
                capabillities.SetCapability("browserName", config.Browser);
                capabillities.SetCapability("browser_version", config.BrowserVersion);
                capabillities.SetCapability("device", config.DeviceName);
                capabillities.SetCapability("deviceOrientation", config.DeviceOrientation);
                capabillities.SetCapability("public", "private");
                capabillities.SetCapability("deviceName", "");
                capabillities.SetCapability("build", config.BuildNo);
                capabillities.SetCapability(CapabilityType.Platform, config.Platform);
                capabillities.SetCapability("browserstack.local", config.RemoteLocalByPass);
                capabillities.SetCapability("project", config.Environment);

                ExecuteBrowserStackLocal();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Set capabilities failed");
                Console.WriteLine(ex.Message);
            }

        }

        private static void ExecuteBrowserStackLocal()
        {
            try
            {
                var arguments = string.Format(" {0} {1} {2}",
                    config.RemoteAccessKey +
                    (config.RemoteLocalByPass == "false" ? "" : " -forcelocal ")
                    ,
                    string.IsNullOrEmpty(config.RemoteProxyHost)
                        ? ""
                        : " -proxyHost " + config.RemoteProxyHost
                    ,
                    string.IsNullOrEmpty(config.RemoteProxyPort)
                        ? ""
                        : " -proxyPort " + config.RemoteProxyPort);


                Console.WriteLine("-> Executing:" + config.TestToolsFolder + "BrowserStackLocal.exe" +
                                  arguments);

                if (Process.GetProcessesByName("BrowserStackLocal").Length == 0)
                {
                    var p = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = config.TestToolsFolder + "BrowserStackLocal.exe",
                            Arguments = arguments
                        }
                    }.Start();

                    Thread.Sleep(15000);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("BrowserStackLocal failed");
                Console.WriteLine(ex.Message);
                Console.WriteLine(JsonConvert.SerializeObject(config));
            }
        }

        private static void StartChromeDriver()
        {
            
            var pathToDriver = ConfigurationManager.AppSettings.Get("PathToDriver");

            // chrome driver cache setup
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments(new[] {
                        "--start-maximized",
                        "allow-running-insecure-content",
                        "--test-type"
                    });
            driverCache = new ChromeDriver(pathToDriver, chromeOptions);
            driverCache.Navigate().GoToUrl("localhost");

            webdriverX = new WebDriverX(driverCache, webLocators);
            ScenarioContext.Current.SetSelenium(driverCache);
        }

        private static void StartFireFoxDriver()
        {
            var pathToDriver = ConfigurationManager.AppSettings.Get("PathToDriver");

            // chrome driver cache setup
            var fireFoxOptions = new FirefoxOptions();
            fireFoxOptions.AddArguments(new[] {
                        "--start-maximized",
                        "allow-running-insecure-content",
                        "--test-type"
                    });
            driverCache = new FirefoxDriver();
            driverCache.Navigate().GoToUrl("localhost");
            
            webdriverX = new WebDriverX(driverCache, webLocators);
            ScenarioContext.Current.SetSelenium(driverCache);
        }

        private static void StartEdgeDriver()
        {
            var pathToDriver = ConfigurationManager.AppSettings.Get("PathToDriver");

            string serverPath = "Microsoft Web Driver";

            if (System.Environment.Is64BitOperatingSystem)
            {
                serverPath = Path.Combine(System.Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"), serverPath);
            }
            else
            {
                serverPath = Path.Combine(System.Environment.ExpandEnvironmentVariables("%ProgramFiles%"), serverPath);
            }

            EdgeOptions edgeOptions = new EdgeOptions();
            //edgeOptions.PageLoadStrategy = EdgePageLoadStrategy.None;
            driverCache = new EdgeDriver(pathToDriver, edgeOptions);
            driverCache.Navigate().GoToUrl("localhost");
            driverCache.Manage().Timeouts().ImplicitWait = (TimeSpan.FromSeconds(10));
            ScenarioContext.Current.SetSelenium(driverCache);
        }

        private static void StartHtmlUnitDriver()
        {
            var pathToDriver = ConfigurationManager.AppSettings.Get("PathToDriver");

            driverCache = new RemoteWebDriver(new Uri("localhost"), capabillities);
            driverCache.Manage().Timeouts().ImplicitWait = (TimeSpan.FromSeconds(10));
            ScenarioContext.Current.SetSelenium(driverCache);
        }

        public static void StopWeb()
        {
            if (driverCache != null)
                driverCache.Close();
        }
    }
}
