using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komodo.Core.Model
{
    public class ConfigModel
    {
        public string Source { get; set; }
        public string TestId { get; set; }
        public string BuildNo { get; set; }
        public string Version { get; set; }
        public string Environment { get; set; }
        public string Browser { get; set; }
        public string BrowserVersion { get; set; }
        public string Platform { get; set; }
        public string WindowSize { get; set; }
        public string DeviceOrientation { get; set; }
        public string DeviceName { get; set; }
        public string TestAssembly { get; set; }
        public string ResultsPath { get; set; }

        public string RemoteWebDriver { get; set; }
        public string RemoteHub { get; set; }
        public string RemoteUserName { get; set; }
        public string RemoteAccessKey { get; set; }
        public string RemoteProxyHost { get; set; }
        public string RemoteProxyPort { get; set; }
        public string RemoteLocalByPass { get; set; }
        public string RemoteSessionId { get; set; }
        public int RemoteInitializeWaitTime { get; set; }

        public string UseCloudStorage { get; set; }
        public string UseFileSystemAsStorage { get; set; }

        public string UseProxyServer { get; set; }
        public string ProxyServer { get; set; }

        public string TestToolsFolder { get; set; }
        public string ChromeServer { get; set; }
        public string IeServer { get; set; }
        public string DefaultBrowser { get; set; }
        public string BrowserDriverPath { get; set; }

        



    }

}
