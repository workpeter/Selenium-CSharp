using OpenQA.Selenium;
using WebDriverWrapper.Utils;

namespace Tesco.Framework.PageInteractions
{
    public class Account_PersonalDetails_Page : PageInteractionsBase
    {
        public Account_PersonalDetails_Page(WebDriverUtils Utils) : base(Utils)
        {
        }

        private readonly By _byLblName = By.XPath("//p[@id='name-field']");

        private readonly By _byLblPhoneNumber = By.XPath("//p[@id='phone-number-field']");

        private readonly By _byLblEmailAddress = By.XPath("//p[@id='email-field']");

        private readonly By _byBtnChangeName = By.XPath("//button[@data-tracking='change name']");

        private readonly By _byBtnChangePhoneNumber = By.XPath("//button[@data-tracking='change phone number']");

        private readonly By _byBtnChangeEmail = By.XPath("//button[@data-tracking='change email']");

        public string AccountCustomerName => Utils.GetElementText(_byLblName);

        public string AccountEmailAddresss => Utils.GetElementText(_byLblEmailAddress);

        public void ClickChangeAccountName()
        {
            Utils.Click(_byBtnChangeName);
        }
    }
}