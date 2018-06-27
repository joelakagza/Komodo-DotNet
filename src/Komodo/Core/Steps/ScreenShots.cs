using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using Komodo.Core.Support;
using Komodo.Core.Extensions;

namespace Komodo.Core.Steps
{
    [Binding]
    public class ScreenShots : SeleniumStepsBase
    {
        #region screen shot
        [Then(@"take screenshot")]
        public void ThenTakeScreenshot()
        {
            string fileName = Guid.NewGuid().ToString().Substring(0, 5);
            ScreenShot.SaveScreenShot(fileName, sDriver);
        }

        [Then(@"take screenshot '(.*)'")]
        public void ThenTakeScreenshotXXXX(string fileName)
        {
            string browser = KomodoTestSuite.config.Browser + KomodoTestSuite.config.WindowSize + "_"
                             + KomodoTestSuite.config.Version;

            FeatureInfo featInfo = FeatureContext.Current.FeatureInfo;
            ScenarioInfo scenInfo = ScenarioContext.Current.ScenarioInfo;

            string date = String.Format("{0:yyyyMMdd}", DateTime.Now);
            string resultsPath = KomodoTestSuite.config.ResultsPath + "\\" + date + "\\" + browser + "\\" + featInfo.Title + "\\"
                                           + ScenarioContext.Current.Get<string>("TestId") + "\\";
            Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            if (fileName == "") { Guid.NewGuid().ToString().Substring(0, 5); }
            ScreenShot.SaveScreenShot(resultsPath + r.Replace(fileName.StrVar(), ""), sDriver);
        }

        [Then(@"save screenshot to '(.*)'")]
        public void ThenSaveScreenshotToXXXX(string fileName)
        {
            Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            FileInfo fi = new FileInfo(fileName.StrVar());
            if (string.IsNullOrEmpty(fi.Name)) { Guid.NewGuid().ToString().Substring(0, 5); }
            ScreenShot.SaveScreenShot(fi.DirectoryName + "\\" + r.Replace(fi.Name, ""), sDriver);
        }
        

        [Then(@"take screenshot of element '(.*)'")]
        public void ThenTakeScreenshotOfElement(string locator)
        {
            var ele = Common.GetLinkLocator(locator, sDriver);
            SaveScreenshotWithHighlightedElement(((ITakesScreenshot) sDriver).GetScreenshot(), ele, "C:/newop.png");

        }

        public void SaveScreenshotWithHighlightedElement(Screenshot screenshot, IWebElement element, string filename)
        {
            using (var image = Bitmap.FromStream(new MemoryStream(screenshot.AsByteArray)))
            {
                using (var graphics = Graphics.FromImage(image))
                {
                    graphics.DrawRectangle(new Pen(Color.Red, 5), new Rectangle(new Point(element.Location.X, element.Location.Y - 55), element.Size));
                }

                image.Save(filename);
            }

            using (var image = Bitmap.FromStream(new MemoryStream(screenshot.AsByteArray)))
            {
                Size s = new Size(element.Size.Width, element.Size.Height);
                Bitmap bmp = new Bitmap(s.Width,s.Width);
                using (var graphics = Graphics.FromImage(bmp))
                {
                    graphics.DrawImage(image,0,0, new Rectangle(new Point(element.Location.X,element.Location.Y - 55), element.Size),GraphicsUnit.Pixel);
                }
                bmp.Save("c:/popop.png",ImageFormat.Png);
            }
        }
        #endregion
    }
}
