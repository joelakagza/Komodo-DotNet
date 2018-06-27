﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.2.0.0
//      SpecFlow Generator Version:2.2.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Komodo.Test.GoogleExample.BDD
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.2.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Google Search Example")]
    public partial class GoogleSearchExampleFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "GoogleSearchExample.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Google Search Example", "\tIn order to avoid silly mistakes\r\n\tAs a math idiot\r\n\tI want to be told the sum o" +
                    "f two numbers", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Google search example")]
        [NUnit.Framework.CategoryAttribute("web")]
        public virtual void GoogleSearchExample()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Google search example", new string[] {
                        "web"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("user navigates to url \'http://www.google.com\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.Then("pause 3000", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Sainsburys search example")]
        [NUnit.Framework.CategoryAttribute("web")]
        [NUnit.Framework.TestCaseAttribute("pop", null)]
        [NUnit.Framework.TestCaseAttribute("rice", null)]
        public virtual void SainsburysSearchExample(string searchTerm, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "web"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Sainsburys search example", @__tags);
#line 12
this.ScenarioSetup(scenarioInfo);
#line 13
 testRunner.Given(string.Format(@"user navigates to url 'https://www.sainsburys.co.uk/webapp/wcs/stores/servlet/SearchDisplayView?catalogId=10123&langId=44&storeId=10151&krypto=o3Ivgm1sN33M1OwzXbuRBdcKTpcK2P%2FU3ErDKwEaqjN%2BJWdQTMTl44n6GMMEgzC5bcapxMgly0Wx1U3msKRoKpyQyZPDcU3D8LYSFBit5O%2F6Ovo0m6NKly59tO7ueCGSnDPQgcikaGE5RyN86uHprlWeydzwTs0sU0FkaYrorq%2B%2B9b%2FuZzcTbEjwYi0yV4iP#langId=44&storeId=10151&catalogId=10123&categoryId=&parent_category_rn=&top_category=&pageSize=36&orderBy=RELEVANCE&searchTerm={0}&beginIndex=0&categoryFacetId1='", searchTerm), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 14
 testRunner.Then("pause 30000", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
