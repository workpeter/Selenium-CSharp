using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Data.Entity;
using System.Linq;
using TechTalk.SpecFlow;
using Tesco.Framework.Enums;
using Tesco.Framework.Models;
using Tesco.Specflow.Base;
using UsefulMethods.GenCommonData;

namespace Tesco.Specflow.Steps
{
    [Binding]
    public sealed class AccountPortal_Steps : FeatureBase
    {
        public AccountPortal_Steps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [Given(@"click the register link")]
        [When(@"click the register link")]
        public void GivenClickTheRegisterLink()
        {
            Index_Page.ClickLinkRegister();
        }

        [Given(@"generate random account details")]
        [When(@"generate random account details")]
        public void GivenGenerateRandomAccountDetails()
        {
            var dategen = new CommonDataGenerator();
            var randPerson = dategen.GenRandomUKPerson();
            var randAddress = dategen.GenRandomUKAddress();

            Scenario.Account = new Account
            {
                //Map properties from generic model dategen.Person to PersonalDetail (Tesco specific Entity framework linked class)
                PersonalDetail = new PersonalDetail
                {
                    Title = randPerson.Title,
                    FirstName = randPerson.FirstName,
                    LastName = randPerson.LastName,
                    DateOfBirth = randPerson.DateOfBirth,
                    PhoneNumber = randPerson.PhoneNumber,
                    MobileNumber = randPerson.MobileNumber,
                    EmailAddress = randPerson.EmailAddress
                },

                //Map properties from generic model dategen.Address to Address (Tesco specific Entity framework linked class)
                Address = new Address
                {
                    AddressLine1 = randAddress.AddressLine1,
                    TownOrCity = randAddress.City,
                    Postcode = randAddress.Postcode,
                    County = randAddress.County,
                    Country = randAddress.Country,
                }
            };

            Scenario.Account.Username = Scenario.Account.PersonalDetail.EmailAddress;
            Scenario.Account.Password = SettingsSpecflow.Default.GenericAccountPassword;
        }

        [Given(@"complete the registration page using random details - using clubcard status:(.*) Marketing communications:(.*)")]
        [When(@"complete the registration page using random details - using clubcard status:(.*) Marketing communications:(.*)")]
        public void GivenRegistrationPageUsingRandomDetails(string clubCardStatus, string marketingCommunication)
        {
            Account_Register_Page.EnterEmailAddress(Scenario.Account.Username);
            Account_Register_Page.EnterPassword(Scenario.Account.Password);

            Scenario.Account.ClubCardStatus = (int)Account_Register_Page.SelectClubCardOption(clubCardStatus);

            Account_Register_Page.SelectNameTitle(Scenario.Account.PersonalDetail.Title_NoFullStop);
            Account_Register_Page.EnterFirstName(Scenario.Account.PersonalDetail.FirstName);
            Account_Register_Page.EnterLastName(Scenario.Account.PersonalDetail.LastName);
            Account_Register_Page.EnterPhoneNumber(Scenario.Account.PersonalDetail.MobileNumber);
            Account_Register_Page.EnterPostCode(Scenario.Account.Address.Postcode);
            Account_Register_Page.ClickFindAddress();
            Account_Register_Page.SelectAddressRandom();

            Scenario.Account.MarketingCommunication = AccountPortal_BL.ConvertMarketingCommunicationFromStringToInt(marketingCommunication);

            Account_Register_Page.SelectMarketingCommunication((MarketingCommunicationType)Scenario.Account.MarketingCommunication);
        }

        [When(@"click to submit registration details and create an account")]
        public void WhenClickToSubmitRegistrationDetailsAndCreateAnAccount()
        {
            Account_Register_Page.ClickCreateAccount();
        }

        [When(@"the account has been created succesfully")]
        [Then(@"the account has been created succesfully")]
        public void ThenTheAccountHasBeenCreatedSuccesfully()
        {
            Account_Register_Confirm_Page.ConfirmRegisteration_ifApplicable();

            //I would ask the developers in the dev environment to give me the ability to switch this security challenge off if it was impacting data generation.
            //I could then reserve the option to turn it back on if I wanted to test it specifically
            //Given that this is just an example project against a live website, i am simple doing Assert.ignore, so that its not a pass or fail. i can consider it a skipped test.
            if (Account_Register_Confirm_Page.IsSecurityChallenged)
                Assert.Ignore("unable to complete registration process. The website has done a security challenge asking for a mobile number.");

            //confirm registration was succesful
            Assert.IsTrue(Account_Register_Confirm_Page.IsShowingConfirmedRegistered);

            //get the club card number from UI
            Scenario.Account.ClubCardNumber = Account_Register_Confirm_Page.ClubCardNumber;
        }

        [Given(@"click the login link")]
        public void GivenClickTheLoginLink()
        {
            Index_Page.ClickLinkSignin();
        }

        [Given(@"select account by:(.*) and (.*)")]
        public void GivenSelectAccountCriteria(string selectionType, string searchCriteria)
        {
            selectionType = selectionType.ToLower();
            searchCriteria = searchCriteria.ToLower();

            using (var db = new ModelTesco())
            {
                // Get all accounts
                var query = db.Accounts.Include(t => t.PersonalDetail).Include(t => t.Address);

                switch (selectionType)
                {
                    case "random":
                        {
                            Scenario.Account = query.OrderBy(_ => Guid.NewGuid()).Take(1).FirstOrDefault(); //Select random top 1 account
                            break;
                        }
                    case "email":
                        {
                            Scenario.Account = query.FirstOrDefault(a => a.Username == searchCriteria); //select account by email/username
                            break;
                        }
                    case "marketingcommunication":
                        {
                            int marketingComm = AccountPortal_BL.ConvertMarketingCommunicationFromStringToInt(searchCriteria);

                            Scenario.Account = query.Where(a => a.MarketingCommunication == marketingComm).OrderBy(_ => Guid.NewGuid()).Take(1).FirstOrDefault(); //select random account where marketing comm meets criteria
                            break;
                        }
                    case "clubcardstatus":
                        {
                            int clubCardStatus = AccountPortal_BL.ConvertClubCardStatusFromStringToInt(searchCriteria);
                            Scenario.Account = query.Where(a => a.ClubCardStatus == clubCardStatus).OrderBy(_ => Guid.NewGuid()).Take(1).FirstOrDefault(); //select random account where club card status meets criteria
                            break;
                        }
                }
            }
        }

        [When(@"submit login details username:(.*) and password:(.*)")]
        public void WhenSubmitLoginDetails(string username, string password)
        {
            Scenario.Account = new Account(username, password);

            WhenSubmitLoginDetails(true);
        }

        [When(@"submit login details and igmore one time passcode setup:(.*)")]
        public void WhenSubmitLoginDetails(bool isIgnoreOneTimePasscordSetup)
        {
            //this page refresh loop is a WORKAROUND (has built in retry limit).
            //The tesco website appears to occasionally block automation chromdriver login with a permission error, however doesnt appear to be an issue when manually testing!
            //So it specific to the automation.
            //I find refreshing the login page can resolve that issue.
            //This is something I would query with the dev team and remove (if) any environment login restrictions in the dev environment, so this workaround wouldnt be required.

            By accessDenied = By.XPath("//h1[text()='Access Denied']");

            Utils.RepeatIfExeptionNullFalse(() =>
                {
                    Utils.Driver.Navigate().Refresh();

                    Account_Login_Page.EnterUsername(Scenario.Account.Username);
                    Account_Login_Page.EnterPassword(Scenario.Account.Password);

                    Account_Login_Page.ClickSignIn();

                    if (Utils.IsElementVisible(accessDenied))
                    {
                        Utils.Driver.Navigate().Back();
                        return false;
                    }

                    return true;
                });

            //if 'Improve your log in experience and security' page shows and we want to ignore it then click 'Remind me later'
            if (isIgnoreOneTimePasscordSetup)
                Account_Login_UpdateDetails_Page.isExistClickRemindMeLater();
            else
                ScenarioContext.Current.Pending(); //i have not set this root up. For this demo, all test cases that login will ignore the onetime passcord setup processs (if it appears)
        }

        [Then(@"account is logged in")]
        public void ThenAccountIsLoggedIn()
        {
            Assert.IsTrue(Index_Page.IsLoggedIn);
        }

        [Given(@"log into a random account")]
        public void GivenLogIntoARandomAccount()
        {
            //====================================
            //This method is an example of a potentially popular high level step designed to call a common series of smaller steps.
            //====================================

            //example of being able to call other specflow binded classes and sharing existing scenario values with ScenarioContext injection
            var stepsGlobal = new Global_Steps(ScenarioContext);

            stepsGlobal.GivenNavigateToTheTescoHomepage();

            GivenClickTheLoginLink();
            GivenSelectAccountCriteria("random", "n/a");
            WhenSubmitLoginDetails(true);
            ThenAccountIsLoggedIn();
        }

        [When(@"view my account portal")]
        public void WhenViewMyAccountPortal()
        {
            Index_Page.ClickMenuLinkMyAccount();
        }

        [When(@"view my personal details page")]
        public void WhenViewMyPersonalDetailsPage()
        {
            Account_Manage_Page.ClickPersonalDetails();
        }

        [Then(@"verify the customer name is correct")]
        public void ThenVerifyTheCustomerNameIsCorrect()
        {
            //here is a good example of how entity framework is handy, because the account data it pulled back from the database is rich and complete.
            Assert.AreEqual(Scenario.Account.PersonalDetail.FullTitleName, Account_PersonalDetails_Page.AccountCustomerName);
        }

        [When(@"update the customer name to a random name")]
        public void WhenUpdateTheCustomerNameToARandomName()
        {
            using (var db = new ModelTesco())
            {
                //re-sync account with Entity framework, so that when we update it, we can also do save changes into our DB, keeping track of the change.
                Scenario.Account = db.Accounts.Include(t => t.PersonalDetail).FirstOrDefault(a => a.Username == Scenario.Account.Username);

                var person = new CommonDataGenerator().GenRandomUKPerson();

                Scenario.Account.PersonalDetail.FirstName = person.FirstName;
                Scenario.Account.PersonalDetail.LastName = person.LastName;

                Account_PersonalDetails_Page.ClickChangeAccountName();

                Account_PersonalDetailsName_Page.EnterFirstName(Scenario.Account.PersonalDetail.FirstName);
                Account_PersonalDetailsName_Page.EnterLasttName(Scenario.Account.PersonalDetail.LastName);
                Account_PersonalDetailsName_Page.ClickSaveChanges();

                db.SaveChanges();
            }
        }
    }
}