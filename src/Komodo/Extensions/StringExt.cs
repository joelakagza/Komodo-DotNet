using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Komodo.Core.Extensions
{
    public static class StringExt
    {
        public static By ToByXpath(this string xpath)
        {
            return By.XPath(xpath);
        }

        public static string StrVar(this string xpath)
        {
            return xpath;
        }

        public static decimal TextToDecimal(this string text, decimal defaultValue = 0)
        {

            decimal outVal = 0;
            try
            {
                decimal.TryParse(text, out outVal);
            }
            catch (Exception)
            {
                return defaultValue;
            }

            return outVal;
        }

        public static decimal ToDecimal(this string text, decimal defaultIfNull = 0)
        {
            try
            {
                return decimal.Parse(Regex.Replace(text, "[^0-9.]", ""));
            }
            catch (Exception)
            {
                return defaultIfNull;
            }
        }
    }
}
