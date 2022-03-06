using OpenQA.Selenium;
using System;
using Tesco.Framework.Enums;
using UsefulMethods;
using WebDriverWrapper.Utils;

namespace Tesco.Framework.PageInteractions
{
    public class Account_Register_Page : PageInteractionsBase
    {
        public Account_Register_Page(WebDriverUtils Utils) : base(Utils)
        {
        }

        private readonly By _byImgLnkTesco = By.XPath("//img[@alt='Tesco logo']/parent::a");

        private readonly By _byLnkSignIn = By.XPath("//a[@data-tracking='back link']");

        private readonly By _byTxtEmailAddress = By.XPath("//input[@id='username']");

        private readonly By _byTxtPassword = By.XPath("//input[@id='password']");

        private readonly By _byRadYesJoinClubcard =
            By.XPath("//label[@for='registration.index.form.fields.clubcard-radio-buttons.join']");

        private readonly By _byRadAlreadyClubcard =
            By.XPath("//label[@for='registration.index.form.fields.clubcard-radio-buttons.already-have']");

        private readonly By _byRadNoJoinClubcard =
            By.XPath("//label[@for='registration.index.form.fields.clubcard-radio-buttons.opt-out']");

        private readonly By _bySelTitle = By.XPath("//select[@id='title']");

        private readonly By _byTxtFirstName = By.XPath("//input[@id='first-name']");

        private readonly By _byTxtLastName = By.XPath("//input[@id='last-name']");

        private readonly By _byTxtPhoneNumber = By.XPath("//input[@id='phone-number']");

        private readonly By _byTxtPostcode = By.XPath("//input[@id='postcode']");

        private readonly By _byBtnFindAddress =
            By.XPath("//span[text()='Find address']/parent::button[@class='ui-component__button']");

        private readonly By _bySelAddress = By.XPath("//select[@name='address_select']");

        private readonly By _byTxtEnterAddressManually = By.XPath("//a[text()='Enter address manually']");

        private readonly By _byTxtAddressLine1 = By.XPath("//input[@id='address-line1']");

        private readonly By _byTxtAddressLine2 = By.XPath("//input[@id='address-line2']");

        private readonly By _byTxtAddressLine3 = By.XPath("//input[@id='address-line3']");

        private readonly By _byTxtTownOrCity = By.XPath("//input[@id='town']");

        private readonly By _byChkTescoStore =
            By.XPath("//span[@class='ui-component__icon--checkmark']/ancestor::label[@for='marketing-stores']");

        private readonly By _byChkTescoBank =
            By.XPath("//span[@class='ui-component__icon--checkmark']/ancestor::label[@for='marketing-bank']");

        private readonly By _byChkTescoMobile =
            By.XPath("//span[@class='ui-component__icon--checkmark']/ancestor::label[@for='marketing-mobile']");

        private readonly By _byBtnCreateAccount = By.XPath("//span[text()='Create account']/parent::button");

        public void EnterEmailAddress(string value, bool doClearFirst = true)
        {
            Utils.SendKeys(_byTxtEmailAddress, value, doClearFirst);
        }

        public void EnterPassword(string value, bool doClearFirst = true)
        {
            Utils.SendKeys(_byTxtPassword, value, doClearFirst);
        }

        public ClubCardStatusType SelectClubCardOption(string clubCardStatus)
        {
            var clubCardStatusEnum = new Converters().ConvertToEnum<ClubCardStatusType>(clubCardStatus);

            switch (clubCardStatusEnum)
            {
                case ClubCardStatusType.JoinToday:
                    ClickClubSaveJoin();
                    break;

                case ClubCardStatusType.AlreadyJoined:
                    ClickClubSaveAlreadyMember();
                    break;

                case ClubCardStatusType.NoJoin:
                    ClickClubSaveNoJoin();
                    break;

                default:
                    throw new Exception($"Did not recognise club card status: {clubCardStatusEnum}");
            }

            return clubCardStatusEnum;
        }

        public void ClickClubSaveJoin()
        {
            Utils.Click(_byRadYesJoinClubcard);
        }

        public void ClickClubSaveAlreadyMember()
        {
            Utils.Click(_byRadAlreadyClubcard);
        }

        public void ClickClubSaveNoJoin()
        {
            Utils.Click(_byRadNoJoinClubcard);
        }

        public void SelectNameTitle(string title)
        {
            Utils.SelectByText(_bySelTitle, title);
        }

        public void EnterFirstName(string value, bool doClearFirst = true)
        {
            Utils.SendKeys(_byTxtFirstName, value, doClearFirst);
        }

        public void EnterLastName(string value, bool doClearFirst = true)
        {
            Utils.SendKeys(_byTxtLastName, value, doClearFirst);
        }

        public void EnterPhoneNumber(string value, bool doClearFirst = true)
        {
            Utils.SendKeys(_byTxtPhoneNumber, value, doClearFirst);
        }

        public void EnterPostCode(string value, bool doClearFirst = true)
        {
            Utils.SendKeys(_byTxtPostcode, value, doClearFirst);
        }

        public void ClickFindAddress()
        {
            Utils.Click(_byBtnFindAddress);
        }

        public void SelectAddress(int index)
        {
            Utils.SelectByIndex(_bySelAddress, index);
        }

        public void SelectAddress(string value)
        {
            Utils.SelectByText(_bySelAddress, value);
        }

        public void SelectAddressRandom()
        {
            try
            {
                Utils.SelectByRandom(_bySelAddress, 1);
            }
            catch (Exception e)
            {
                if (IsSelectAddressVisible == false)
                    throw new Exception("Address list not visible. Are you using a valid UK postcode?", e);

                throw;
            }
        }

        public bool IsSelectAddressVisible => Utils.IsElementVisible(_bySelAddress);

        public void SelectMarketingCommunication(MarketingCommunicationType marketingCommunication)
        {
            //do An initial click on marketing stores to deselect this default selection.
            ClickMarketingStores();

            switch (marketingCommunication)
            {
                case MarketingCommunicationType.Store:
                    ClickMarketingStores();
                    break;

                case MarketingCommunicationType.Bank:
                    ClickMarketingBank();
                    break;

                case MarketingCommunicationType.Mobile:
                    ClickMarketingMobile();
                    break;

                case MarketingCommunicationType.Store_Bank:
                    ClickMarketingStores();
                    ClickMarketingBank();
                    break;

                case MarketingCommunicationType.Store_Mobile:
                    ClickMarketingStores();
                    ClickMarketingMobile();
                    break;

                case MarketingCommunicationType.Bank_Mobile:
                    ClickMarketingBank();
                    ClickMarketingMobile();
                    break;

                case MarketingCommunicationType.Store_Bank_Mobile:
                    ClickMarketingStores();
                    ClickMarketingBank();
                    ClickMarketingMobile();
                    break;
            }
        }

        public void ClickMarketingStores()
        {
            Utils.Click(_byChkTescoStore);
        }

        public void ClickMarketingBank()
        {
            Utils.Click(_byChkTescoBank);
        }

        public void ClickMarketingMobile()
        {
            Utils.Click(_byChkTescoMobile);
        }

        public void ClickCreateAccount()
        {
            Utils.Click(_byBtnCreateAccount);
        }
    }
}