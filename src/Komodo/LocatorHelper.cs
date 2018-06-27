using CsvHelper;
using Komodo.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.Reflection;
using Komodo.Core.Domain;

namespace Komodo.Core
{
    public class LocatorHelper
    {
        public static List<Locator> LoadLocators(string filePath)
        {
            CsvHelper.Configuration.CsvConfiguration config = new CsvHelper.Configuration.CsvConfiguration();
            config.Encoding = Encoding.Default;
            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filePath = Path.Combine(path, filePath);
            var csv = new CsvReader(new StreamReader(filePath, Encoding.Default));
            return csv.GetRecords<Locator>().ToList();
        }

        internal static By GetWebDriverBy(Locator locator)
        {
            switch (locator.ByName.ToLower())
            {
                case "xpath":
                    return By.XPath(locator.ByValue);
                case "css":
                    return By.CssSelector(locator.ByValue);
                case "id":
                    return By.CssSelector(locator.ByValue);
                case "name":
                    return By.CssSelector(locator.ByValue);
                default:
                    break;
            }

            return null;
        }
    }
}
