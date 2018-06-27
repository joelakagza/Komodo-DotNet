using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Komodo.Core.Model;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;
using Newtonsoft.Json;
using OpenQA.Selenium;
using Komodo.Core.Domain;
using Komodo.Data;
using OpenQA.Selenium.Remote;
using Komodo.Core.Support;
using System.Drawing;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using Komodo.Core.Extensions;

namespace Komodo.Core.Steps
{
    [Binding]
    public static partial class KomodoTestSuite
    {
        public static Stopwatch sw;
        public static MemoryStream ms;
        public static StreamWriter streamWriter;
        public static StringBuilder sb;
        public static int stepCount;
        public static object strOut;
        public static double lastElapsedTime;

        public static IWebDriver driverCache = null;
        public static ResultsReal realResult { get; set; }
        public static ConfigModel config { get; set; }
        private static OpenQA.Selenium.Proxy DriverProxy { get; set; }
        
        static KomodoTestSuite()
        {
            sw = new Stopwatch();
            config = new ConfigModel();
        }

        public static bool ReuseWebSession
        {
            get { return ConfigurationManager.AppSettings["ReuseWebSession"] != "true"; }
        }

        public static void SetConfigFromEnvironmentVariables()
        {
            /*Example config for teamcity {"Source":"false","Environment":"systestdbmodapi"} */

            ConfigModel envConfig = new ConfigModel();

            var testConfig = Environment.GetEnvironmentVariable("testConfig") ??
                               Environment.GetEnvironmentVariable("env.testConfig");

            if (testConfig != null)
                envConfig = JsonConvert.DeserializeObject<ConfigModel>(testConfig);

            AutoMapper.Mapper.CreateMap<ConfigModel, ConfigModel>()
                .ForAllMembers(c => c.IgnoreIfSourceIsNull());

            AutoMapper.Mapper.Map<ConfigModel, ConfigModel>(envConfig, config);
        }

        public static void SetConfigFromLocalAppConfig()
        {
            try
            {
                config.SeleniumVersion = ConfigurationManager.AppSettings.Get("selenium");
                config.Build = ConfigurationManager.AppSettings.Get("build") ?? "";
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
                config.UseCloudStorage = ConfigurationManager.AppSettings.Get("useS3asStorage");
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
            sw.Start();           
            
            Console.WriteLine("-> TestAssembly: " + config.TestAssembly);
            Console.WriteLine("-> Environment: " + config.Environment);
            Console.WriteLine("-> Selenium Version: " + config.SeleniumVersion);
            Console.WriteLine("-> Version: " + config.Version);
            Console.WriteLine("-> Build: " + config.Build);
            Console.WriteLine("-> Selected browser started: " + config.Browser + " " + config.BrowserVersion + " " + config.WindowSize);
            Console.WriteLine("-> Remote Webrdiver: " + config.RemoteWebDriver);
            Console.WriteLine("-> Remote Source: " + config.Source);
            Console.WriteLine("-> Remote Webrdiver server: " + config.RemoteHub);
            Console.WriteLine("-> Platform: " + config.Platform.StrVar());
            Console.WriteLine("-> Device: " + config.DeviceName);
            Console.WriteLine("-> Device Orientation: " + config.DeviceOrientation);
            Console.WriteLine("-> Results Path: " + config.ResultsPath.StrVar());
            Console.WriteLine("-> TestId: " + config.TestId);
            
            if (driverCache == null)
            {
                if (config.RemoteWebDriver != "false")
                {
                    UseRemoteWebDriver();
                }
                else
                {
                    UseLocalWebDriver();
                }

                StartSelenium();
            }

        }

        private static void StartSelenium()
        {
            if (driverCache != null)
            {
                ScenarioContext.Current.SetSelenium(driverCache);
                Console.WriteLine("-> Selenium started");
                TryToMaximizeWindow();
                TryToSetWindowSize();
            }
        }

        private static void UseLocalWebDriver()
        {
            switch (config.Browser)
            {
                case "firefox":
                    FirefoxBinary fb = new FirefoxBinary(config.DefaultBrowser);
                    //driverCache = new FirefoxDriver(desFF);
                    break;
                case "iexplore":
                    var options = new InternetExplorerOptions();
                    options.IgnoreZoomLevel = true;
                    options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    options.EnsureCleanSession = true;
                    driverCache = new InternetExplorerDriver(config.IeServer, options);

                    break;
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments(new[]
                    {
                        "--start-maximized",
                        "allow-running-insecure-content",
                        "--test-type"
                    });
                    chromeOptions.Proxy = DriverProxy;
                    driverCache = new ChromeDriver(config.ChromeServer, chromeOptions);
                    driverCache.Navigate().GoToUrl("localhost");
                    break;
                case "phantomjs":
                    var phantomjsOptions = new PhantomJSOptions();
                    driverCache = new PhantomJSDriver(phantomjsOptions);
                    driverCache.Navigate().GoToUrl("localhost");
                    break;
                case "safari":
                    driverCache = new SafariDriver(new SafariOptions());
                    driverCache.Navigate().GoToUrl("localhost");
                    break;
                case "opera":
                    break;
                case "iphone":
                    break;
                case "ipad":
                    break;
            }
        }

        private static void UseRemoteWebDriver()
        {
            try
            {
                if (config.Browser == "android" && config.RemoteWebDriver == "true")
                {
                    DesiredCapabilities browser = DesiredCapabilities.Android();
                    browser.SetCapability(CapabilityType.Platform, "ANDROID");
                    browser.SetCapability("webdriver.mobile.server", config.RemoteHub);
                    browser.SetCapability("browserName", config.Browser);
                    browser.SetCapability(CapabilityType.BrowserName, "android");
                    driverCache = new RemoteWebDriver(new Uri(config.RemoteHub), browser,
                        TimeSpan.FromSeconds(120));
                    driverCache.Navigate().GoToUrl("localhost");
                }
                else if (config.RemoteWebDriver == "saucelabs")
                {
                    try
                    {
                        DesiredCapabilities capabillities =
                            BuildDesiredCapabilities.BuildCapabilities(config.Browser, config.Platform,
                                config.BrowserVersion);
                        capabillities.SetCapability("name", KomodoTestSuite.realResult.Scenario);
                        // ScenarioContext.Current["TestId"].ToString());
                        capabillities.SetCapability("username", config.RemoteUserName);
                        capabillities.SetCapability("accessKey", config.RemoteAccessKey);
                        capabillities.SetCapability("browserName", config.Browser);
                        capabillities.SetCapability("version", config.BrowserVersion);
                        capabillities.SetCapability("deviceName", config.DeviceName);
                        capabillities.SetCapability("deviceOrientation", config.DeviceOrientation);
                        capabillities.SetCapability("public", "private");
                        capabillities.SetCapability("deviceName", "");
                        capabillities.SetCapability(CapabilityType.Platform, config.Platform);

                        driverCache = new ScreenShotRemoteWebDriver(
                            new Uri(config.RemoteHub), capabillities);

                        if (config.Platform != "windows")
                            Thread.Sleep(40000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (config.RemoteWebDriver == "browserstack")
                {
                    DesiredCapabilities capabillities = null;
                    //browserStackAutomateClient = new BrowserStackAutomateClient(config.RemoteUserName,
                    //    config.RemoteAccessKey);

                    try
                    {
                        capabillities =
                            BuildDesiredCapabilities.BuildCapabilities(config.Browser, config.Platform,
                                config.BrowserVersion);
                        capabillities.SetCapability("name", KomodoTestSuite.realResult.Scenario);
                        capabillities.SetCapability("browserstack.user", config.RemoteUserName);
                        capabillities.SetCapability("browserstack.key", config.RemoteAccessKey);
                        capabillities.SetCapability(CapabilityType.AcceptSslCertificates, true);
                        capabillities.SetCapability("browserName", config.Browser);
                        capabillities.SetCapability("browser_version", config.BrowserVersion);
                        capabillities.SetCapability("device", config.DeviceName);
                        capabillities.SetCapability("deviceOrientation", config.DeviceOrientation);
                        capabillities.SetCapability("public", "private");
                        capabillities.SetCapability("deviceName", "");
                        capabillities.SetCapability("build", config.Build);
                        capabillities.SetCapability(CapabilityType.Platform, config.Platform);
                        capabillities.SetCapability("browserstack.local", config.RemoteLocalByPass);
                        capabillities.SetCapability("project", config.Environment);

                        ExecuteBrowserStackExe();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Set capabilities failed");
                        Console.WriteLine(ex.Message);
                    }

                  

                    try
                    {

                        driverCache = new ScreenShotRemoteWebDriver(new Uri(config.RemoteHub), capabillities);
                        config.RemoteSessionId = ((ScreenShotRemoteWebDriver)driverCache).getExecutionID();
                        var caps = ((ScreenShotRemoteWebDriver)driverCache).Capabilities;

                        //var session = browserStackAutomateClient.GetSessionAsync(config.RemoteSessionId);
                        //Console.WriteLine("-> BowserStack Session:" + session.automation_session.browser_url);

                        string[] os = { "windows xp", "windows", "any" };
                        if (!os.Contains(config.Platform.ToLower()))
                            Thread.Sleep(config.RemoteInitializeWaitTime);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Open browser failed");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(JsonConvert.SerializeObject(config));
                    }
                }
                else if (config.RemoteWebDriver == "testingBot")
                {
                    try
                    {
                        DesiredCapabilities capabillities =
                            BuildDesiredCapabilities.BuildCapabilities(config.Browser, config.Platform,
                                config.Version);

                        capabillities.SetCapability("name", ScenarioContext.Current["TestId"].ToString());
                        capabillities.SetCapability("username", config.RemoteUserName);
                        capabillities.SetCapability("accessKey", config.RemoteAccessKey);
                        capabillities.SetCapability("deviceOrientation", config.DeviceOrientation);
                        capabillities.SetCapability("deviceName", config.DeviceName);
                        capabillities.SetCapability("deviceOrientation", config.DeviceOrientation);
                        driverCache = new ScreenShotRemoteWebDriver(
                            new Uri(config.RemoteHub), capabillities);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    DesiredCapabilities capabillities =
                        BuildDesiredCapabilities.BuildCapabilities(config.Browser, config.Platform,
                            config.Version);
                    driverCache = new ScreenShotRemoteWebDriver(new Uri(config.RemoteHub), capabillities);
                }

            }
            catch (WebDriverException ex)
            {
                Console.WriteLine(ex.Message);
                //throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //throw;
            }
        }

        private static void ExecuteBrowserStackExe()
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

        private static void TryToSetWindowSize()
        {
            try
            {
                int width = Convert.ToInt32(config.WindowSize.Split('x')[0]);
                int height = Convert.ToInt32(config.WindowSize.Split('x')[1]);
                driverCache.Manage().Window.Size = new Size(width, height);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //driverCache.Manage().Window.Maximize();
            }
        }

        private static void TryToMaximizeWindow()
        {
            try
            {
                driverCache.Manage().Window.Maximize();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void StopSelenium()
        {
            Console.WriteLine("Elapsed Total Seconds:" + sw.Elapsed.TotalSeconds);
            sw.Stop();
            //if (driverCache == null)
            //    return;

            Console.WriteLine("Errors:" + ScenarioContext.Current["selenium-errors"].ToString());

            try
            {
                //HealthCheckService.InsertSession(oAllSessions, ScenarioContext.Current["TestId"].ToString(),
                 //   ScenarioContext.Current.ScenarioInfo.Title, config.Environment);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error trying to insert fildder session data" +  ex);
            }

            try
            {
                //ProxyHelper.StopFiddlerProxy();
                ScreenShot.SaveScreenShot(Common.ResultsPathDate() + "End_of_test", driverCache);
            }
            catch (EntityException entityEx)
            {
                Console.WriteLine(entityEx.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Errors:" + ScenarioContext.Current["selenium-errors"].ToString());
                //throw;
            }
            finally
            {
                //Fiddler.FiddlerApplication.Shutdown();
            }

            try
            {
                if (driverCache != null)
                    driverCache.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Selenium stop error");
            }
            finally
            {
                if (driverCache != null)
                    driverCache.Quit();
                driverCache = null;
            }

            
            Console.WriteLine("-> Selenium stopped");


        }

        public static void LogScenarioResults()
        {
            List<string> tags = new List<string>();
            string result = "Fail";

            tags.AddRange(FeatureContext.Current.FeatureInfo.Tags);
            tags.AddRange(ScenarioContext.Current.ScenarioInfo.Tags);

            PropertyInfo missingStepsInfo = ScenarioContext.Current.GetType().GetProperty("MissingSteps",
                                                        BindingFlags.Public |
                                                        BindingFlags.NonPublic |
                                                        BindingFlags.Instance);

            List<StepInstance> missingSteps = (List<StepInstance>)missingStepsInfo.GetValue(ScenarioContext.Current, null);

            if (missingSteps.Count > 0)
                result = "Pending";

            EfRepository<ResultsReal> context = new EfRepository<ResultsReal>();

            string seleniumErrors = "";
            ScenarioContext.Current.TryGetValue("SeleniumErrors", out seleniumErrors);

            if (sb.ToString() == "" || ScenarioContext.Current.TestError != null || seleniumErrors != null ||
                ScenarioContext.Current.Count == 0)
            {
                result = "Fail";
                FeatureContext.Current.Set(result, "Result");
                ScenarioContext.Current.Set(result, "Result");
            }
            else
            {
                if (result != "Pending") result = "Pass";
                ScenarioContext.Current.Set(result, "Result");
            }

            var error = (ScenarioContext.Current.TestError == null ? "" : ScenarioContext.Current.TestError.Message);

            realResult.TestId = ScenarioContext.Current.Get<string>("TestId");
            realResult.Env = Common.GetEnvString("env");
            realResult.Version = config.Build;
            realResult.Source = config.Source;
            realResult.Result = result;
            realResult.OutPut = sb.ToString();
            realResult.Tags = String.Join(", ", tags.GroupBy(t=> t).Select(t=>t.Key).ToArray());
            realResult.Date = DateTime.Now;
            realResult.UserName = Environment.GetEnvironmentVariable("USERNAME");
            realResult.config = JsonConvert.SerializeObject(config);
            realResult.Error = error;
            
            context.Insert(KomodoTestSuite.realResult);            
        }

        
    }


}
