using OpenQA.Selenium;
using UsefulMethods;
using WebDriverWrapper.Utils;
using Tesco.Framework.Models;

namespace Tesco.Framework.PageInteractions
{
    public class Index_Page : PageInteractionsBase
    {
        public Index_Page(WebDriverUtils Utils) : base(Utils)
        {
        }

        private readonly By _byBtnPopupAcceptAllCookies = By.XPath("//span[text()='Accept all cookies']/ancestor::button");

        private readonly By _byLnkMenuSignin = By.XPath("//span[@class='link-text' and text()='Sign in']");

        private readonly By _byLnkMenuStorelocator = By.XPath("//a[@title='Store locator']");

        private readonly By _byTxtSearchPostcodeStoreLocator = By.XPath("//input[@title='Enter postcode or town']");

        private readonly By _byBtnSearchPostcodeStoreLocator = By.XPath("//span[text()='Find store']/parent::button");

        private readonly By _byLnkMenuContactUs = By.XPath("//span[@class='link-text' and text()='Contact us']");

        private readonly By _byLnkMenuHelp = By.XPath("//span[@class='link-text' and text()='Help']");

        private readonly By _byLnkMenuMyAccount = By.XPath("//span[@class='link-text' and text()='My account']");

        private readonly By _bySelService = By.XPath("//select[@serviceName='serviceName']");

        private readonly By _byTxtSearchField = By.XPath("//input[@title='Enter search terms']");

        private readonly By _byBtnSearch = By.XPath("//button[@class='search-icon-button']");

        private readonly By _byLnkSignIn = By.XPath("//a[text()='Sign in']");

        private readonly By _byLnkSignOut = By.XPath("//a[@title='Sign Out']");

        private readonly By _byLnkRegister = By.XPath("//a[text()='Register']");

        public bool IsLoggedIn => Utils.IsElementVisible(_byLnkSignOut);

        public void ClickAcceptAllCookies()
        {
            if (Utils.IsElementVisible(_byBtnPopupAcceptAllCookies))
                Utils.Click(_byBtnPopupAcceptAllCookies);
        }

        public void ClickMenuLinkSignin()
        {
            Utils.Click(_byLnkMenuSignin);
        }

        public void ClickMenuLinkStorelocator()
        {
            Utils.Click(_byLnkMenuStorelocator);
        }

        public void EnterPostcodeForStoreLocator(string searchValue, bool doClearFirst = true)
        {
            var element = Utils.GetFirstVisbleElement(_byTxtSearchPostcodeStoreLocator);

            Utils.SendKeys(element, searchValue, doClearFirst);
        }

        public void ClickFindStore()
        {
            Utils.Click(_byBtnSearchPostcodeStoreLocator);
        }

        public void ClickMenuLinkContactUs()
        {
            Utils.Click(_byLnkMenuContactUs);
        }

        public void ClickMenuLinkHelp()
        {
            Utils.Click(_byLnkMenuHelp);
        }

        public void ClickMenuLinkMyAccount()
        {
            Utils.Click(_byLnkMenuMyAccount);
        }

        public void SelectService(string serviceName)
        {
            Service service = new Converters().ConvertToEnum<Service>(serviceName);

            //todo
            var a = new Converters().ConvertToString(service);

            Utils.SelectByText(_bySelService, serviceName);
        }

        public void EnterSearchValue(string searchValue, bool doClearFirst = true)
        {
            var element = Utils.GetFirstVisbleElement(_byTxtSearchField);

            Utils.SendKeys(element, searchValue, doClearFirst);
        }

        public void ClickBtnSearch()
        {
            var element = Utils.GetFirstVisbleElement(_byBtnSearch);

            Utils.Click(element);
        }

        public void ClickLinkSignin()
        {
            Utils.Click(_byLnkSignIn);
        }

        public void ClickLinkRegister()
        {
            Utils.Click(_byLnkRegister);
        }
    }
}