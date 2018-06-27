
using System;
using TechTalk.SpecFlow;
using Komodo.Core.Support;

namespace Komodo.Core.Events
{
    [Binding]
    public class NonWebEvents : SeleniumStepsBase
    {
        
        #region Scenario Hooks nonweb

        [BeforeScenario("webService", Order = 100)]
        [BeforeScenario("restfulApi", Order = 100)]
        [BeforeScenario("restful", Order = 100)]
        public static void BeforeRestFulApiScenario()
        {
            try
            {
                SeleniumSupport.config.Browser = "nonWeb";
                SeleniumSupport.config.Version = "";
                SeleniumSupport.StartTestSuite();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                if (ex.InnerException != null) 
                    Console.Write(ex.InnerException.Message ?? "");
            }
        }


        [AfterScenario("webService", Order = 100)]
        [AfterScenario("restfulApi", Order = 100)]
        [AfterScenario("restful",Order = 100)]
        public static void AfterWebScenario()
        {
            //SeleniumSupport.config.Browser = "nonWeb";
            SeleniumSupport.config.Version = "";

        }

        #endregion
    }


}
