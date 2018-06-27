using HtmlAgilityPack;
using Komodo.Core;
using Komodo.Core.Extensions;
using Komodo.Web.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Xml;
using TechTalk.SpecFlow;

namespace Komodo.Web.Steps
{
    [Binding]
    public class Assists : TestSuiteStepsBase
    {
        #region SpecFlow Assist Helpers

        [Then(@"append '(.*)' to the begin of each file in the following directory '(.*)'")]
        public void ThenAppendToTheBeginOfEachFileInTheFollowingDirectory(string appendtxt, string dir)
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + dir.StrVar(), "*.xml");
            foreach (var file in files)
            {
                string txt = File.ReadAllText(file);
                File.WriteAllText(file,appendtxt.StrVar() + txt);
            }
           
        }

        [Then(@"append '(.*)' to the begin of file '(.*)'")]
        public void ThenAppendToTheBeginOfFile(string appendtxt, string filePath)
        {
            FileInfo fi = new FileInfo(filePath.StrVar());
            string txt = File.ReadAllText(filePath.StrVar());
            File.WriteAllText(filePath.StrVar(), appendtxt.StrVar() + Environment.NewLine + txt);
        }

        [Then(@"append '(.*)' to the end of each file in the following directory '(.*)'")]
        public void ThenAppendToTheEndOfEachFileInTheFollowingDirectory(string appendtxt, string dir)
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + dir.StrVar(), "*.xml");
            foreach (var file in files)
            {
                string txt = File.ReadAllText(file);
                File.WriteAllText(file,txt + appendtxt.StrVar());
            }
        }


        [Then(@"load xmldocument '(.*)' into feature context '(.*)'")]
        public void ThenLoadXmldocumentXxIntoFeatureContextXx(string xmlPath, string name)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlPath);
            FeatureContext.Current.Set(xDoc, name);
        }

        [Then(@"load xmldocument '(.*)' into scenario context '(.*)'")]
        public void ThenLoadXmldocumentXxIntoScenarioContextXx(string xmlPath, string name)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlPath);
            ScenarioContext.Current.Set(xDoc, name);
        }

        [Then(@"create dropdownlist from loaded xmldocument '(.*)' options xpath:'(.*)' value xpath:'(.*)' into '(.*)'")]
        public void ThenCreateDropdownlistFromLoadedXmldocumentXxOptionsXpathXxValueXpathXxIntoXX(string name, string optionXpath, string valueXpath, string dropdownListName)
        {
            XmlDocument xDoc = FeatureContext.Current.Get<XmlDocument>(name);
            System.Web.UI.WebControls.DropDownList dl = new System.Web.UI.WebControls.DropDownList();
            XmlNodeList optionNodes = xDoc.SelectNodes(optionXpath);
            XmlNodeList valueNodes = xDoc.SelectNodes(valueXpath);

            Assert.IsNotNull(optionNodes, "Nothing to load into dropdownlist check xml source");

            for (int i = 0; i < optionNodes.Count; i++)
            {
                Console.WriteLine(optionNodes[i].Value + "," + valueNodes[i].Value);
                dl.Items.Add(new ListItem(optionNodes[i].Value, valueNodes[i].Value));
            }

            FeatureContext.Current.Set(dl, dropdownListName);
        }


        [Then(@"verify xpath is visible '(.*)'")]
        public void ThenVerifyXpathIsVisibleXXXX(string locator)
        {
            Assert.True(WebDriverExt.FindElement(By.XPath(locator)).Displayed);
        }

        [Then(@"verify xpath is not visible '(.*)'")]
        public void ThenVerifyXpathIsNotVisibleXXXX(string locator)
        {
            try
            {
                webDriverX.FindElement(By.XPath(locator));
                Assert.Fail();
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }          
        }

        [Then(@"verify the following xpath:'(.*)' exists")]
        public void ThenVerifyTheFollowingXpathExists(string xpath)
        {
            IWebElement webI = null;
            try
            {
                webI = webDriverX.FindElement(By.XPath(xpath.StrVar()));
            }
            finally
            {
                Assert.IsNotNull(webI, "Xpath does not exist: " + xpath.StrVar());
            }
        }

        [Then(@"verify the following xpath:'(.*)' does not exist")]
        public void ThenVerifyTheFollowingXpathDoesNotExist(string xpath)
        {
            Assert.Throws<NoSuchElementException>(() => _webDriverX.FindElement(By.XPath(xpath)), "Xpath exists: " + xpath);
        }

        [Then(@"verify the following xpaths exists")]
        public void ThenVerifyTheFollowingXpathsExists(Table table)
        {
            foreach (var item in table.Rows)
            {
                ThenVerifyTheFollowingXpathExists(item["xpath"].StrVar());
            }
        }

        [Then(@"verify the following xpaths do not exist")]
        public void ThenVerifyTheFollowingXpathsDoNotExist(TechTalk.SpecFlow.Table table)
        {
            foreach (var item in table.Rows)
            {
                ThenVerifyTheFollowingXpathDoesNotExist(item["xpath"].StrVar());
            }
        }


        [Then(@"verify the count of xpath:'(.*)' equals '(.*)'")]
        public void ThenVerifyTheCountOfXpathXxEqualsXx(string xpath, int xCount)
        {
            int actCount = 0;
            try
            {
                actCount = webDriverX.FindElements(By.XPath(xpath)).Count;
            }
            finally
            {
                Assert.AreEqual(xCount, actCount);
            }
        }

        #endregion

        #region helpers

        [Then(@"open a new window")]
        public void ThenOpenANewWindow()
        {
            IJavaScriptExecutor jscript = webDriverX as IJavaScriptExecutor; jscript.ExecuteScript("window.open()");
        }

        [Then(@"pause")]
        public void ThenPause()
        {
            ThenPause(10000);
        }

        [Then(@"pause (.*)")]
        public void ThenPause(int time)
        {
            Thread.Sleep(time);
        }

        [Then(@"display links for xpath '(.*)' with delimiter '(.*)'")]
        public void ThenDisplayLinksForXpathWithDelimiter(string xpath,string delimiter)
        {
            ICollection<IWebElement> lst = webDriverX.FindElements(By.XPath(xpath));
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(webDriverX.PageSource);

            HtmlNodeCollection nCol = doc.DocumentNode.SelectNodes(xpath);

            foreach (HtmlNode webElement in nCol)
            {
                Console.WriteLine(System.Web.HttpUtility.HtmlDecode(webElement.GetAttributeValue("href", "").ToString())
                                  + delimiter +
                                  System.Web.HttpUtility.HtmlDecode(webElement.InnerText.Replace(Environment.NewLine, "").Trim())
                                  + delimiter +
                                  System.Web.HttpUtility.HtmlDecode(webElement.XPath.Replace(Environment.NewLine, "")));
            }
        }


        [Then(@"get elements of xpath '(.*)'")]
        public void ThenGetElementsOfXpath(string xpath)
        {
           //CommonInsertUpdateLocators(xpath, webdriverX);
        }

        [Then(@"get page elements")]
        public void ThenGetPageElements()
        {
            //CommonRunInsertUpdateLocators(webdriverX);
        }


        [Then(@"get links on page")]
        public void ThenGetLinksOnPage()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(webDriverX.PageSource);
            HtmlNodeCollection aLinks = doc.DocumentNode.SelectNodes("//a");
            Console.WriteLine("|" + "host" + " | " + "path" + " | " + "query" + " | " + "url" + "|");
            foreach (HtmlNode htmlNode in aLinks)
            {
                string hostStr = string.Empty;
                string queryStr = string.Empty;
                string pathstr = string.Empty;
                string pageStr = string.Empty;
                try
                {
                    UriBuilder newUrlB = new UriBuilder(htmlNode.GetAttributeValue("href", "No href"));
                    hostStr = newUrlB.Host;
                    queryStr =  HttpUtility.HtmlDecode(newUrlB.Query);
                    pathstr = newUrlB.Path;
                    //pageStr= newUrlB.pPageName;
                }
                catch (Exception)
                {

                }

                Console.WriteLine("|" + hostStr + " | " + pathstr + " | " + queryStr + " | " + htmlNode.GetAttributeValue("href", "No href").Replace("|", "\\|") + "|");
            }
        }

        [Then(@"click on every link on page and take screen shot")]
        public void ThenClickOnEveryLinkOnPageAndTakeScreenShot()
        {
            ICollection<IWebElement> links = webDriverX.FindElements(By.XPath("//a"));
            int i = 0;
            foreach (IWebElement aLink in links)
            {
                //Console.WriteLine(aLink.Text);
                aLink.Click();
                //ScreenShot.SaveScreenShot("pic" + i.ToString(),webdriverX);
                //Browser.ClickTheBrowserBackButton(webdriverX);
            }
        }

        #endregion

        [Then(@"refresh page (.*)")]
        public void ThenRefreshPageXx(string refreshType)
        {
            ((IJavaScriptExecutor)webDriverX).
                ExecuteScript("javascript:window.location.reload();",null);
        }

        [Then(@"close browser window")]
        [Then(@"close current browser window")]
        public void ThenCloseCurrentBrowserWindow()
        {
            webDriverX.SwitchTo().Window(webDriverX.CurrentWindowHandle).Close();
        }

        [Then(@"output pagesource")]
        public void ThenOutputPagesource()
        {
            string h = webDriverX.CurrentWindowHandle;
            Console.Write(webDriverX.PageSource.ToString());
        }

        [Then(@"output pagesource html element")]
        public void ThenOutputPagesourceHtmlElement()
        {
            Console.Write(webDriverX.FindElement(By.TagName("html")).ToString());
        }

        [Then(@"hover over '(.*)'")]
        public void ThenHoverOverForAdvertisers(string locator)
        {
            Thread.Sleep(3000);
            IWebElement hoverOverItem = webDriverX.FindElement(locator);
            Actions builder = new Actions(webDriverX);
            if (ConfigurationManager.AppSettings["browserName"] == "htmlunit")
            {
                builder.MoveToElement(hoverOverItem).Perform();
            }
            else
            {
                builder.MoveToElement(hoverOverItem).MoveByOffset(2, 2).Perform();
            }
        }

        [Then(@"hover over and click '(.*)'")]
        public void ThenHoverOverAndClick(string locator)
        {
            Thread.Sleep(3000);
            IWebElement hoverOverItem = webDriverX.FindElement(locator.StrVar());
            Actions builder = new Actions(webDriverX);
            if (ConfigurationManager.AppSettings["browserName"] == "htmlunit")
            {
                builder.MoveToElement(hoverOverItem).Perform();
            }
            else
            {
                builder.MoveToElement(hoverOverItem).MoveByOffset(2, 2).Perform();
                Thread.Sleep(1000);
                builder.MoveToElement(hoverOverItem).MoveByOffset(2, 2).Click().Perform();
            }
        }


        [Then(@"switch to the new window")]
        [Then(@"switch to other window")]
        public void ThenSwitchToOtherWindow()
        {
            ICollection<string> pop = webDriverX.WindowHandles;
            int i = 0;
            foreach (string item in pop)
            {
                if (i == 1) { webDriverX.SwitchTo().Window(item); }
                i++;
            }
            Thread.Sleep(2000);
        }

        [Then(@"switch to window (.*)")]
        [Then(@"switch to other window (.*)")]
        public void ThenSwitchToOtherWindow(int index)
        {
            ICollection<string> pop = webDriverX.WindowHandles;
            int i = 0;
            foreach (string item in pop)
            {
                if (i == index) { webDriverX.SwitchTo().Window(item); }
                i++;
            }
            Thread.Sleep(2000);
        }

        [Then(@"verify count '(.*)' equals (.*)")]
        public void ThenVerifyCountAvailabilityRecordCountEquals1(string locator, int count)
        {
            ICollection<IWebElement> col = webDriverX.FindElements(locator);
            Assert.AreEqual(count, col.Count);
        }

        [Then(@"click on alert 'OK'")]
        public void ThenClickOnAlertOk()
        {
            int i = 0;
            while (i<10)
            {
                try
                {
                    //Support.Web.Alerts.OkAlert(webdriverX);
                    //string alertText = webdriverX.SwitchTo().Alert().Text;   
                    break;
                }
                catch (Exception ex)
                {
                    i++;
                    Console.WriteLine("waiting for alert...");
                    Thread.Sleep(1000);
                }
            }

            Thread.Sleep(3000);
        }

        [Then(@"zoom (.*) on the browser x(.*)")]
        [Then(@"zoom (.*) on the browser (.*)")]
        public void ThenZoomInOnTheBrowser(string innOrOut,int x)
        {
            IWebElement html = webDriverX.FindElement(By.TagName("html"));
            for (int i = 0; i < x; i++)
            {
                switch (innOrOut.ToLower())
                {
                    case "in":
                        new Actions(webDriverX).SendKeys(html, Keys.Control + Keys.Add + Keys.Null).Perform();
                        break;
                    case "out":
                        new Actions(webDriverX).SendKeys(html, Keys.Control + Keys.Subtract + Keys.Null).Perform();
                        break;
                }
            }

        }

        [Then(@"to do")]
        public void ThenToDo()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
