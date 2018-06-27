using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Komodo.Core.Model
{

    public  class WebPageBase
    {
        public string Page { get; set; }
        public string Html { get; set; }
        public string Head { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }

    }

    public class LocatorResults
    {
        public string Env { get; set; }
        public string Host { get; set; }
        public string Name { get; set; }
        public string Xpath { get; set; }
        public string Path { get; set; }
        public string Query { get; set; }
    }

    public class WebElementAttributes
    {
        public string Text { get; set; }
        public string TagName { get; set; }
        public string Size { get; set; }
        public string Selected { get; set; }
        public string Location { get; set; }
        public string Displayed { get; set; }
        public string Enabled { get; set; }
    }

    public class TextTable
    {
        public string text { get; set; }
    }

    public class TextTableModel
    {
        public string Text { get; set; }

    }

    public class DropDownListModel
    {
        public string option { get; set; }
        public string value { get; set; }
        public int SelectedIndex { get; set; }
    }
    
    public class cookieFilePipe
    {
        // No need Converters for primitive types the library provides them.
        public string Name;
        public string Value;
        public string Domain;
        public string Expiry;
    }
    
    public class cookieFileTab
    {
        // No need Converters for primitive types the library provides them.
        public string Name;
        public string Value;
        public string Domain;
        public string Path;
        public string Expiry;
    }

    public class Capabilities
    {
        /// <summary>
        /// A User's username. eg: "sergiotapia, mrkibbles, matumbo"
        /// </summary>
        [JsonProperty("seleniumProtocol")]
        public string SeleniumProtocol { get; set; }


        /// <summary>
        /// A User's username. eg: "sergiotapia, mrkibbles, matumbo"
        /// </summary>
        [JsonProperty("platform")]
        public string Platform { get; set; }

        /// <summary>
        /// A User's username. eg: "sergiotapia, mrkibbles, matumbo"
        /// </summary>
        [JsonProperty("firefox_binary")]
        public string FirefoxBinary { get; set; }


        /// <summary>
        /// A User's username. eg: "sergiotapia, mrkibbles, matumbo"
        /// </summary>
        [JsonProperty("browserName")]
        public string BrowserName { get; set; }


        /// <summary>
        /// A User's username. eg: "sergiotapia, mrkibbles, matumbo"
        /// </summary>
        [JsonProperty("maxInstances")]
        public string MaxInstances { get; set; }


        /// <summary>
        /// A User's username. eg: "sergiotapia, mrkibbles, matumbo"
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
    }



}
