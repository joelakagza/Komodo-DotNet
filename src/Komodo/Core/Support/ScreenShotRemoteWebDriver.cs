using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Komodo.Core.Support
{
    public class ScreenShotRemoteWebDriver : RemoteWebDriver, ITakesScreenshot
    {
        public ScreenShotRemoteWebDriver(Uri remoteAdress, DesiredCapabilities capabilities)
            : base(remoteAdress, capabilities)
        {
        }

        public ScreenShotRemoteWebDriver(Uri remoteAdress, DesiredCapabilities capabilities, TimeSpan timeS)
            : base(remoteAdress, capabilities, timeS)
        {
        }

        public string getExecutionID()
        {
            return this.SessionId.ToString();
        } 

        /// <summary> 
        /// Gets a <see cref="Screenshot"/> object representing the image of the page on the screen. 
        /// </summary> 
        /// <returns>A <see cref="Screenshot"/> object containing the image.</returns> 
        public Screenshot GetScreenshot()
        {
            // Get the screenshot as base64. 
            Response screenshotResponse = Execute(DriverCommand.Screenshot, null);
            string base64 = screenshotResponse.Value.ToString();

            // ... and convert it. 
            return new Screenshot(base64);

        }
    }


}
