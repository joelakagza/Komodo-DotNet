using Komodo.Core.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using Komodo.Core.Extensions;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using Komodo.Core.Domain;
using System.Collections.ObjectModel;

namespace Komodo.Core
{
    public class WebDriverX : IWebDriver
    {
        public IWebDriver _webx;
        public readonly IEnumerable<Locator> _locators;
        public List<QuestionAndAnswers> _QnAs;

        public string Url { get => _webx.Url; set => _webx.Url = value; }

        public string Title => _webx.Title;

        public string PageSource => _webx.PageSource;

        public string CurrentWindowHandle => _webx.CurrentWindowHandle;

        public ReadOnlyCollection<string> WindowHandles => _webx.WindowHandles;

        public WebDriverX(IWebDriver webdriver, IEnumerable<Locator> locators)
        {
            _webx = webdriver;
            _locators = locators;
            _QnAs = new List<QuestionAndAnswers>();
        }

        public void Focus(string keyName)
        {
            _webx.FindElement(_locators.ByKeyName(_webx.Title, keyName)).SendKeys("");
        }

        public IWebElement FindElement(string keyName)
        {
            return _webx.FindElement(_locators.ByKeyName(_webx.Title,keyName));
        }

        public IWebElement FindElement(string keyName,string parameter)
        {
            return _webx.FindElement(_locators.ByKeyName(_webx.Title, keyName, parameter));
        }

        public List<IWebElement> FindElements(string keyName)
        {
            return _webx.FindElements(_locators.ByKeyName(_webx.Title, keyName)).ToList();
        }

        public List<IWebElement> FindElements(string keyName, string parameter)
        {
            return _webx.FindElements(_locators.ByKeyName(_webx.Title, keyName, parameter)).ToList();
        }

        public List<IWebElement> FindElements(By by)
        {
            return _webx.FindElements(by).ToList();
        }

        public By GetBy(string keyName)
        {
            return _locators.ByKeyName(_webx.Title, keyName);
        }

        public By GetBy(string keyName, string parameter)
        {
            return _locators.ByKeyName(_webx.Title, keyName, parameter);
        }

        public bool DoesElementExist(By ByName)
        {
            try
            {
                return _webx.FindElement(ByName).Displayed;
            }
            catch (NoSuchElementException nse)
            {
                Console.WriteLine(nse.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;

        }


        public  bool DoesElementExist(string keyName)
        {
            try
            {
                return this.FindElement(_locators.ByKeyName(_webx.Title, keyName)).Displayed;
            }
            catch (NoSuchElementException nse)
            {
                Console.WriteLine(nse.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }


        public bool DoesElementExist(string keyName,string parameter)
        {
            try
            {
                return _webx.FindElement(_locators.ByKeyName(_webx.Title, keyName, parameter)).Displayed;
            }
            catch (NoSuchElementException nse)
            {
                Console.WriteLine(nse.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;

        }

        public bool IsElementVisible(string keyName)
        {
            try
            {
                var IsVisible = _webx.FindElement(_locators.ByKeyName(_webx.Title, keyName)).Displayed;
                return IsVisible;
            }
            catch (NoSuchElementException nse)
            {
                Console.WriteLine(nse.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool IsElementVisible(By by)
        {
            try
            {
                var IsVisible = _webx.FindElement(by).Displayed;
                return IsVisible;
            }
            catch (NoSuchElementException nse)
            {
                Console.WriteLine(nse.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public void ClickIfElementExist(string keyname)
        {
            if (_webx.DoesElementExist(_locators.ByKeyName(_webx.Title,keyname)))
                _webx.FindElement(_locators.ByKeyName(_webx.Title, keyname)).Click();
        }

        public void ClickIfElementExist(string keyname, string parameter)
        {
            if (_webx.DoesElementExist(_locators.ByKeyName(_webx.Title, keyname, parameter)))
                _webx.FindElement(_locators.ByKeyName(_webx.Title, keyname, parameter)).Click();
        }

        public void WaitForElementLoad(By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(_webx, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(by));
            }
        }

        public void WaitForElementLoad(string keyname, int timeoutInSeconds = 30)
        {
            WaitForElementLoad(GetBy(keyname), timeoutInSeconds);
        }

        public void WaitForElementLoad(string keyname, string parameter, int timeoutInSeconds)
        {
            WaitForElementLoad(GetBy(keyname, parameter), timeoutInSeconds);
        }


        #region Q n A

        public void CreateQuestionAndAnswers(IEnumerable<QuestionAndAnswers> qnas)
        {
            _QnAs = qnas.ToList();
        }

        public void AddQuestionAndAnswers(IEnumerable<QuestionAndAnswers> qnas)
        {
            _QnAs.AddRange(qnas.ToList());
        }

        public void QnASendKeys(string question)
        {
            _webx.FindElement(_locators.ByKeyName(_webx.Title, question)).SendKeys(_QnAs.GetAnswer(question));
        }

        public IWebElement Question(string question)
        {
            var qna = _QnAs.FindQuestionAndAnswer(question);
            var loc = _locators.GetLocatorByKeyName(_webx.Title, qna.Question);

            if (Regex.Match(loc.ByValue, @"{\d+}").Length == 1)
                return _webx.FindElement(
                LocatorHelper.GetWebDriverBy(new Locator
                {
                    KeyName = loc.KeyName,
                    ByType = loc.ByType,
                    ByValue = string.Format(loc.ByValue, qna.Answer),
                    ByName = loc.ByName,
                    Page = loc.Page,
                    Description = loc.Description
                }));

            return _webx.FindElement(_locators.ByKeyName(_webx.Title, qna.Question));
        }

        public void Close()
        {
            _webx.Close();
        }

        public void Quit()
        {
            _webx.Quit();
        }

        public IOptions Manage()
        {
           return _webx.Manage();
        }

        public INavigation Navigate()
        {
            return _webx.Navigate();
        }

        public ITargetLocator SwitchTo()
        {
            return _webx.SwitchTo();
        }

        public IWebElement FindElement(By by)
        {
            return _webx.FindElement(by);
        }

        ReadOnlyCollection<IWebElement> ISearchContext.FindElements(By by)
        {
            return _webx.FindElements(by);
        }

        public void Dispose()
        {
            _webx.Dispose();
        }


        #endregion
    }
}
