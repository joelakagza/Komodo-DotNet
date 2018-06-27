using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;


namespace Komodo.Core.Steps
{
    public class BuildDesiredCapabilities
    {

        public static DesiredCapabilities BuildCapabilities(string browser, string platform, string version)
        {
            DesiredCapabilities desCap = GetDesiredCapabilities(browser);
            desCap.SetCapability(CapabilityType.Platform, platform.ToUpper());
            if(!string.IsNullOrEmpty(version))
                desCap.SetCapability(CapabilityType.Version, version);
            return desCap;
        }

        public static DesiredCapabilities GetDesiredCapabilities(string capName)
        {
            switch (capName.ToLower())
            {
                case "android":
                case "linux":
                    return DesiredCapabilities.Android();
                case "chrome":
                case "googlechrome":
                case "google chrome":
                    return DesiredCapabilities.Chrome();
                case "firefox":
                    return DesiredCapabilities.Firefox();
                case "iphone":
                    return DesiredCapabilities.IPhone();
                case "ipad":
                    return DesiredCapabilities.IPad();
                case "phantomjs":
                    return DesiredCapabilities.PhantomJS();
                case "safari":
                    var d = DesiredCapabilities.Safari();
                    d.Platform = new Platform(PlatformType.Mac);
                    d.IsJavaScriptEnabled = true;
                    return d;
                case "iexplore":
                case "internetexplorer":
                case "internet explorer":
                    return DesiredCapabilities.InternetExplorer();
                case "opera":
                    return DesiredCapabilities.Opera();
                case "htmlunit":
                case "htmlunitfirefox":
                    return DesiredCapabilities.HtmlUnitWithJavaScript();
                case "htmlunitNoJavaScript":
                    return DesiredCapabilities.HtmlUnitWithJavaScript();
                default:
                    return DesiredCapabilities.Firefox();
            }

        }



    }
}
