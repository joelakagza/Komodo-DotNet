using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using TechTalk.SpecFlow;
using System.IO;

namespace Komodo.Core.Extensions
{
    public static class WebElementExt
    {
        public static void ClickByOffSet(this IWebElement webElement, IWebDriver sDriver, int x, int y)
        {
            Actions builder = new Actions(sDriver);
            builder.MoveToElement(webElement).MoveByOffset(x, y).Click().Build().Perform();
        }

        public static void highlightElementAndTakeScreenshot(this IWebElement webElement, IWebDriver sDriver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)sDriver;
            String oldStyle = webElement.GetAttribute("style");

            String args = "arguments[0].setAttribute('style', arguments[1]);";
            js.ExecuteScript(args, webElement,
                "border: 4px solid yellow;display:block;");

            //takeScreenshot(priority, element, selenium);

            js.ExecuteScript(args, webElement, oldStyle);
        }

        public static void GetImage(this IWebElement webElement, IWebDriver sDriver)
        {
            Screenshot ss = ((ITakesScreenshot) sDriver).GetScreenshot();
            Stream stream = new MemoryStream(ss.AsByteArray);

            Point point = webElement.Location;

            int eleWidth = webElement.Size.Width;
            int eleHeight = webElement.Size.Height;

            Size s = new Size();
            s.Width = webElement.Size.Width;
            s.Height = webElement.Size.Height;

            Bitmap bitmap = new Bitmap(stream);
            Bitmap bmp = new Bitmap(s.Width, s.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(bitmap, 0, 0,
                new Rectangle(new Point(point.X, point.Y ),
                    new Size(webElement.Size.Width, webElement.Size.Height)), GraphicsUnit.Pixel);

            g.DrawRectangle(new Pen(Color.Red, 20), new Rectangle(webElement.Location, webElement.Size));

            bmp.Save("C:/pop.png", System.Drawing.Imaging.ImageFormat.Png);
            ss.SaveAsFile("C:/original.png", System.Drawing.Imaging.ImageFormat.Png);

            Rectangle section = new Rectangle(new Point(eleWidth, eleHeight), new Size(150, 150));
        }

    }

}