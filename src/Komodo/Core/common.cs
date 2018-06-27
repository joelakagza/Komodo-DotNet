using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using Komodo.Core.Model;
using TechTalk.SpecFlow;
using Komodo.Core.Extensions;

namespace Komodo.Core.Steps
{
    public partial class Common
    {
        public static string ResultsPathDate()
        {
            string browser = KomodoTestSuite.config.Browser + KomodoTestSuite.config.WindowSize + "_"
                             + KomodoTestSuite.config.Version;

            FeatureInfo featInfo = FeatureContext.Current.FeatureInfo;

            string date = String.Format("{0:yyyyMMdd}", DateTime.Now);

            string resultsPath = KomodoTestSuite.config.ResultsPath + "\\" + date + "\\" + browser + "\\" + featInfo.Title +
                                 "\\"
                                 + ScenarioContext.Current.Get<string>("TestId") + "\\";


            if (!Directory.Exists(resultsPath))
            {
                Directory.CreateDirectory(resultsPath);
            }
            return resultsPath;
        }


        public static string ResultsPathDateTime()
        {
            string projectPath = ResultsPathDate();
            projectPath = projectPath + "_" + String.Format("{0:hhmmss}", DateTime.Now);
            if (!Directory.Exists(projectPath))
            {
                Directory.CreateDirectory(projectPath);
            }
            return projectPath;
        }

        public static string ResultsPathFeatureScenario()
        {
            string browser = KomodoTestSuite.config.Browser + "_" +
                             KomodoTestSuite.config.Version;
            string env = KomodoTestSuite.config.Environment;

            FeatureInfo featInfo = FeatureContext.Current.FeatureInfo;
            ScenarioInfo scenInfo = ScenarioContext.Current.ScenarioInfo;

            string date = String.Format("{0:yyyyMMdd}", DateTime.Now);
            string resultsPath = KomodoTestSuite.config.ResultsPath + "\\" + date + "\\" + env + "\\" + browser + "\\"
                                 + featInfo.Title + "\\"
                                 + ScenarioContext.Current.Get<string>("TestId") + "\\";

            return resultsPath;
        }


        public static string ResultsPath()
        {
            return ConfigurationManager.AppSettings["projectPath"].ToLower() + "Results\\";
        }

        public static string GetFeatEnvStr(string scenarioVarName)
        {
            string scenarioStrVar;
            FeatureContext.Current.TryGetValue("%{" + scenarioVarName + "}", out scenarioStrVar);
            return scenarioStrVar;
        }

        public static string SetFeatEnvStr(string scenarioVarName, string scenarioStrVar)
        {
            Regex rgx = new Regex(@"\%{[a-zA-Z0-9\s-_]*}");
            FeatureContext.Current.Set<string>(scenarioStrVar.StrVar(), "%{" + scenarioVarName + "}");
            return "true";
        }

        public static void LoadAppSettingsIntoScenarioContext()
        {
            foreach (string setting in ConfigurationManager.AppSettings)
            {
                if (ConfigurationManager.AppSettings[setting] != null)
                {
                    SetEnvString(setting,(string)ConfigurationManager.AppSettings[setting]);
                }
            }
        }

        public static string GetEnvString(string scenarioVarName)
        {
            string scenarioStrVar;
            ScenarioContext.Current.TryGetValue("%{" + scenarioVarName + "}", out scenarioStrVar);
            return scenarioStrVar;
        }

        public static string SetEnvString<T>(string scenarioVarName, T value) 
        {
            Regex rgx = new Regex(@"\%{[a-zA-Z0-9\s-_]*}");
            ScenarioContext.Current.Set<T>(value, "%{" + scenarioVarName + "}");
            return "true";
        }


        public static ConfigModel SetConfigFromLocalAppConfig(ConfigModel config = null)
        {
            if(config == null) config = new ConfigModel();
            try
            {
                config.SeleniumVersion = ConfigurationManager.AppSettings.Get("selenium");
                config.Build = ConfigurationManager.AppSettings.Get("build") ?? "";
                config.Browser = ConfigurationManager.AppSettings.Get("browserName") ?? "";
                config.BrowserVersion = ConfigurationManager.AppSettings.Get("browserVersion") ?? "";
                config.Version = ConfigurationManager.AppSettings.Get("version") ?? "";
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

            return config;
        }
    }

}
