using OpenQA.Selenium;
using WebDriverWrapper.Utils;

namespace Tesco.Framework.PageInteractions
{
    public class Account_Register_Confirm_Page : PageInteractionsBase
    {
        public Account_Register_Confirm_Page(WebDriverUtils Utils) : base(Utils)
        {
        }

        private readonly By _byLblRegisterConfirmMsg = By.XPath("//div[contains(text(),'registered your account')]");

        private readonly By _byLblClubCardNumber = By.XPath("//div[@class='ui-components__clubcard-number']//h5");

        private readonly By _byBtnContinueShopping = By.XPath("//a[@class='ui-component__link ui-component__button']");

        private readonly By _byBtnRegisterNowPopup = By.XPath("//span[text()='Register now']/parent::button");

        private readonly By _byBtnSendCodeNow = By.XPath("//button[@id='send-otp-verification-button']");

        public bool IsShowingConfirmedRegistered => Utils.IsElementVisible(_byLblRegisterConfirmMsg);

        public bool IsSecurityChallenged => Utils.IsElementVisible(_byBtnSendCodeNow);

        public string ClubCardNumber => Utils.GetElementText(_byLblClubCardNumber);

        public void ClickContinueShopping()
        {
            Utils.Click(_byBtnContinueShopping);
        }

        public void ConfirmRegisteration_ifApplicable()
        {
            if (Utils.IsElementVisible(_byBtnRegisterNowPopup))
                Utils.Click(_byBtnRegisterNowPopup);
        }
    }
}