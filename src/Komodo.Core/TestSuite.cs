using Komodo.Core.Extensions;
using Komodo.Core.Model;
using Komodo.Core.Models;
using System;

namespace Komodo.Core
{
    public class TestSuite
    {
        public static ConfigModel config { get; set; }
        public static TestResult result { get; set; }

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
        
    }
}
