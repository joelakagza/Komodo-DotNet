using Komodo.Core;
using Komodo.Core.Data;
using Komodo.Core.Extensions;
using System;
using System.Configuration;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace HomeLyfe.Harvest.Steps
{
    [Binding]
    public sealed class MoneySuperMarketSteps : TestSuiteStepsBase
    {
        EfRepository<Quote> quotesRepository;
        QuoteDetails quoteDetails;
        string UseQueue;

        public MoneySuperMarketSteps()
        {
            quotesRepository = new EfRepository<Quote>(new hlfContext());
            quoteDetails = ScenarioContextService.GetValue<QuoteDetails>("quoteData");
            UseQueue = ConfigurationManager.AppSettings.Get("UseQueue").ToLower();
        }

        [Given(@"user has navigated to url '(.*)'")]
        public void GivenUserHasNavigatedToUrl(string url)
        {
            webDriver.Navigate().GoToUrl(url);
        }

        [Then(@"enter into the part 1 section 1 of the form")]
        public void ThenEnterIntoThePartOfTheForm(Table table)
        {
            webdriverX.CreateQuestionAndAnswers(table.ToQuestionAndAnswers());
            var QnAs = webdriverX._QnAs;

            if (UseQueue != "true")
                quoteDetails = table.CreateInstance<QuoteDetails>();

            webdriverX.FindElement("What's your house number or name?").SendKeys(quoteDetails.WhatsYourHouseNumberOrName);
            webdriverX.FindElement("What's your postcode?").SendKeys(quoteDetails.WhatsYourPostcode);
            webdriverX.Focus("What's your house number or name?");

            quoteDetails.FullAddress = webdriverX.FindElement("What's the address that you'd like to insure?").GetTextFromDDL(1);

            if (webdriverX.DoesElementExist("What's the address that you'd like to insure?"))
                webdriverX.FindElement("What's the address that you'd like to insure?").SelectByIndex(1);

            webdriverX.FindElement("Do you own or rent your home?", QnAs.GetAnswer("Do you own or rent your home?")).Click();
            webdriverX.FindElement("What would you like to insure?", QnAs.GetAnswer("What would you like to insure?")).Click();
            
            webdriverX.FindElement("What is your day of birth?").SendKeys(quoteDetails.WhatIsYourDateOfBirth.Split('/')[0]);
            webdriverX.FindElement("What is your month of birth?").SendKeys(quoteDetails.WhatIsYourDateOfBirth.Split('/')[1]);
            webdriverX.FindElement("What is your year of birth?").SendKeys(quoteDetails.WhatIsYourDateOfBirth.Split('/')[2]);

            webdriverX.FindElement("Would you like to insure any laptops or bikes?", QnAs.GetAnswer("Would you like to insure any laptops or bikes?")).Click();

            webdriverX.FindElement("Would you like to insure any items worth over £1000 each?", QnAs.GetAnswer("Would you like to insure any items worth over £1000 each?") ).Click();
            webdriverX.FindElement("How much would it cost to replace the entire contents of your home?").SendKeys(QnAs.GetAnswer("How much would it cost to replace the entire contents of your home?"));
            webdriverX.FindElement("Would you like to insure any personal possessions you take out with you?", table.Rows[8][1]).Click();
        }

        [Then(@"click on the part 1 continue button")]
        public void ThenClickOnThePart1ContinueButton()
        {
            webdriverX.FindElement("part 1 continue").Click();
        }

        [Then(@"enter into the part 2 section 1 of the form")]
        public void ThenEnterIntoThePartSectionOfTheForm(Table table)
        {
            webdriverX.CreateQuestionAndAnswers(table.ToQuestionAndAnswers());
            var QnAs = webdriverX._QnAs;

            if (UseQueue != "true")
                quoteDetails = table.CreateInstance<QuoteDetails>();

            bool PleaseCheckTheFollowingDetailsAreCorrect = webDriver.FindElement("//h3//span[contains(.,'Please check the following details are correct.')]".ToByXpath()).Displayed;

            if (!PleaseCheckTheFollowingDetailsAreCorrect)
            {
                webdriverX.FindElement("Which type of lock do you have on your main door?", table.Rows[0][1]).Click();
                webdriverX.FindElement("Does your home have patio doors?", table.Rows[1][1]).Click();
                webdriverX.FindElement("Are there any other doors that lead outside?", table.Rows[2][1]).Click();
                webdriverX.FindElement("Do your windows have key-operated locks on them?", table.Rows[3][1]).Click();
                webdriverX.FindElement("Do you have a working burglar alarm?", table.Rows[4][1]).Click();
                webdriverX.FindElement("Do you have a fixed and lockable safe?", table.Rows[5][1]).Click();
                webdriverX.FindElement("How many working smoke alarms do you have?", table.Rows[6][1]).Click();
                webdriverX.FindElement("Is there a local neighbourhood watch?", table.Rows[7][1]).Click();
                webdriverX.FindElement("What kind of home do you live in?", quoteDetails.WhatKindOfHomeDoYouLiveIn).Click();
                webdriverX.FindElement("What kind of house is it?", quoteDetails.WhatKindOfHouseIsIt).Click();        
                webdriverX.FindElement("What percentage of your roof is flat?", table.Rows[11][1]).Click();
                webdriverX.FindElement("Bedrooms").ClearAndSendKeys(quoteDetails.Bedrooms);
                webdriverX.FindElement("Reception rooms").ClearAndSendKeys(table.Rows[13][1]);
                webdriverX.FindElement("Bathrooms").ClearAndSendKeys(table.Rows[14][1]);
                webdriverX.FindElement("Other rooms").ClearAndSendKeys(table.Rows[15][1]);
            }

            if (webdriverX.IsElementVisible(webdriverX.GetBy("Are the statements above correct?", "Yes")))
                webdriverX.FindElement("Are the statements above correct?", "Yes").Click();

            if (webdriverX.IsElementVisible(webdriverX.GetBy("Roughly when was your home built?")))
                webdriverX.FindElement("Roughly when was your home built?").SendKeys(table.Rows[10][1]);

            webdriverX.FindElement("What's the current market value of your property?").SendKeys(table.Rows[16][1]);
            webdriverX.FindElement("How much would it cost to rebuild your home today?").ClearAndSendKeys(quoteDetails.HowMuchWouldItCostToRebuildYourHomeToday);

            if (!PleaseCheckTheFollowingDetailsAreCorrect)
            {
                webdriverX.FindElement("Are there any trees within 5 metres of your home?", table.Rows[18][1]).Click();
            }

            webdriverX.FindElement("Did you buy your home?", table.Rows[19][1]).Click();
            webdriverX.FindElement("When did you buy your home? Month").ClickAndSelectByText(table.Rows[20][1].Split('/')[0]);
            webdriverX.FindElement("When did you buy your home? Year").ClickAndSelectByText(table.Rows[20][1].Split('/')[1]);
            webdriverX.FindElement("Who lives in your home?", table.Rows[21][1]).Click();
            webdriverX.FindElement("Do you have a mortgage?", table.Rows[22][1]).Click();
            webdriverX.FindElement("How many adults live there?").ClickAndSelectByText(quoteDetails.HowManyAdultsLiveThere);
            webdriverX.FindElement("And how many children?", table.Rows[24][1]).ClickAndSelectByText(quoteDetails.AndHowManyChildren);
            webdriverX.FindElement("In general, when are there people at home?", table.Rows[25][1]).Click();
            webdriverX.FindElement("Is your home ever left empty for more than 30 days in a row?", table.Rows[26][1]).Click();
            webdriverX.FindElement("Does anyone living in your home smoke?", table.Rows[27][1]).Click();
            webdriverX.FindElement("Is your place ever used for business?", table.Rows[28][1]).Click();

            webdriverX.ClickIfElementExist("Have you had any incidents at home, or made any home insurance claims in the last 5 years?", table.Rows[29][1]);

            webdriverX.ClickIfElementExist("Have you made any home insurance claims or suffered any losses in the last 5 years", table.Rows[30][1]);
        }

        [Then(@"click on the part 2 continue button")]
        public void ThenClickOnThePart2ContinueButton()
        {
            webdriverX.FindElement("part 2 continue").Click();
        }

        [Then(@"enter into the part 3 section 1 of the form")]
        public void ThenEnterIntoThePart3Section1OfTheForm(Table table)
        {
            webdriverX.CreateQuestionAndAnswers(table.ToQuestionAndAnswers());
            var QnAs = webdriverX._QnAs;

            if (UseQueue != "true")
                quoteDetails = table.CreateInstance<QuoteDetails>();

            webdriverX.Question("What's your first name?").SendKeys(quoteDetails.WhatsYourFirstName);
            webdriverX.FindElement("And your last name?").SendKeys(quoteDetails.AndYourLastName);
            webdriverX.FindElement("What's your email address?").SendKeys(quoteDetails.WhatsYourEmailAddress);

            webdriverX.Focus("And your last name?");

            if (webdriverX.DoesElementExist("Please confirm your email address"))
                webdriverX.FindElement("Please confirm your email address").SendKeys(quoteDetails.WhatsYourEmailAddress);

            webdriverX.FindElement("Please choose a password").Focus().SendKeys(quoteDetails.PleaseChooseAPassword);

            if (webdriverX.DoesElementExist("Please re-enter your password"))
                webdriverX.FindElement("Please re-enter your password").SendKeys(quoteDetails.PleaseChooseAPassword);

            webDriver.ClickIfElementExist("//*[@id='policyHolder.password']//button[contains(.,'Sign in')]".ToByXpath());

            webDriver.ClickIfElementExist("//input[@type='checkbox'][@class='checkbox__input ng-valid ng-dirty ng-valid-parse ng-touched ng-not-empty']".ToByXpath());
            webDriver.ClickIfElementExist("//input[@type='checkbox'][@class='checkbox__input ng-pristine ng-untouched ng-valid ng-not-empty']".ToByXpath());
            webdriverX.FindElement("What is your gender?",quoteDetails.WhatIsYourGender).Click();
            webdriverX.FindElement("What is your relationship status?", table.Rows[7][1]).Click();
            webdriverX.FindElement("What do you do?", table.Rows[8][1]).Click();
            webdriverX.FindElement("What job do you do?", table.Rows[9][1]).ClearAndSendKeys(table.Rows[9][1]);
            webdriverX.FindElement("Which industry do you work in?").ClearAndSendKeys(table.Rows[10][1]);
            webdriverX.FindElement("Do you do any other work?",table.Rows[11][1]).Click();
            webdriverX.FindElement("Have you lived in the UK since you were born?", table.Rows[12][1]).Click();
            webdriverX.FindElement("Would you like this to be a joint policy?", table.Rows[13][1]).Click();
            webdriverX.FindElement("When would you like the policy to start?").ClickAndSelectByIndex(int.Parse(table.Rows[14][1]));
            webdriverX.FindElement("How do you normally pay for your home insurance?", table.Rows[15][1]).Click();
            webdriverX.FindElement("How many years of no claims discount do you have for buildings insurance?").ClickAndSelectByText(table.Rows[16][1]);
            webdriverX.FindElement("How many years of no claims discount do you have for contents insurance?").ClickAndSelectByText(table.Rows[17][1]);

            if (webdriverX.DoesElementExist("Would you like the two insurance companies with the best prices to contact you to discuss your quote?", table.Rows[18][1]))
                webdriverX.FindElement("Would you like the two insurance companies with the best prices to contact you to discuss your quote?", table.Rows[18][1]).Click();

            if (webdriverX.DoesElementExist("Would you like us to remember you on this device for 13 months?", table.Rows[11][1]))
                webdriverX.FindElement("Would you like us to remember you on this device for 13 months?", table.Rows[11][1]).Click();
        }

        [Then(@"click on link '(.*)'")]
        public void ThenClickOnLink(string keyname)
        {
            webdriverX.ClickIfElementExist(keyname);
        }

        [Then(@"wait for '(.*)' element for '(.*)'")]
        public void ThenWaitForElementTimeOf(string keyname, int timeSec)
        {
            webdriverX.WaitForElementLoad(keyname, timeSec);
        }

        [Then(@"dump the search results to database")]
        public void ThenDumpTheSearchResultsToDatabase()
        {
            var QnAs = webdriverX._QnAs;
            var date = DateTime.Now;
            var results = webdriverX.FindElements("//li[contains(@class,'result-table__row')]".ToByXpath());
            for (int i = 1; i < results.Count(); i++)
            {
                string idAsString = i.ToString();
                try
                {
                    var quote = new Quote();
                    quote.BatchId = ScenarioContextService.GetValue<QuoteDetails>("quoteData").BatchId;
                    quote.Date = date;
                    quote.InsurerName = webdriverX.FindElement("Insurer Name", idAsString).GetAttribute("alt");
                    quote.AnnualPrice = webdriverX.FindElement("Annual Price", idAsString).CleanToDecimal();
                    quote.VoluntaryExcessBuildings = webdriverX.FindElement("Voluntary Excess Buildings", idAsString).CleanToDecimal();
                    quote.VoluntaryExcessContents = webdriverX.FindElement("Voluntary Excess Contents", idAsString).CleanToDecimal();
                    quote.CompulsoryExcessBuildings = webdriverX.FindElement("Compulsory Excess Buildings", idAsString).CleanToDecimal();
                    quote.CompulsoryExcessContents = webdriverX.FindElement("Compulsory Excess Contents", idAsString).CleanToDecimal();
                    quote.TotalExcessBuildings = 0;
                    quote.TotalExcessContents = 0;
                    quote.BuildingsCover = webdriverX.FindElement("Building Cover", idAsString).Text.Replace("Unlimited", "999999999").Replace(" m", "000000").Replace(" K", "000").CleanToDecimal();
                    quote.ContentsCover = webdriverX.FindElement("Contents Cover", idAsString).Text.Replace("Unlimited", "999999999").Replace(" k", "000").CleanToDecimal(); ;

                    quote.LegalExpenses = -1;
                    if (webdriverX.DoesElementExist("Legal Expenses", idAsString))
                        quote.LegalExpenses = webdriverX.FindElement("Legal Expenses", idAsString).CleanToDecimal();

                    quote.HomeEmergency = -1;
                    if (webdriverX.DoesElementExist("Home Emergency", idAsString))
                        quote.HomeEmergency = webdriverX.FindElement("Home Emergency", idAsString).CleanToDecimal();

                    quote.AboutInsurer = webdriverX.FindElement("About Insurer", idAsString).GetAttribute("innerHTML") ?? "";
                    quote.UnderWriter = webdriverX.FindElement("UnderWriter", idAsString).GetAttribute("innerHTML") ?? "";
                    quote.PostCode = ScenarioContextService.GetValue<QuoteDetails>("quoteData").WhatsYourPostcode;
                    quote.HouseNumber = quoteDetails.WhatsYourHouseNumberOrName;
                    quote.FullAddress = quoteDetails.FullAddress;
                    quote.DOB = DateTime.Parse(quoteDetails.WhatIsYourDateOfBirth);
                    quote.NumAdults = int.Parse(quoteDetails.HowManyAdultsLiveThere);
                    quote.NumChildren = int.Parse(quoteDetails.AndHowManyChildren);
                    quote.PropertyType = quoteDetails.WhatKindOfHomeDoYouLiveIn;
                    quote.ContentSumInsured = quoteDetails.HowMuchWouldItCostToReplaceTheEntireContentsOfYourHome;
                    quote.BuildingSumInsured = quoteDetails.HowMuchWouldItCostToRebuildYourHomeToday;             
                    quote.QnA = string.Join(Environment.NewLine, QnAs.Select(t => new { qa = (t.Question + ": " + t.Answer) }).Select(t => t.qa));
                    quotesRepository.Insert(quote);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}


