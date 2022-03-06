using OpenQA.Selenium;
using WebDriverWrapper.Utils;

namespace Tesco.Framework.PageInteractions
{
    public class Account_Login_Page : PageInteractionsBase
    {
        public Account_Login_Page(WebDriverUtils Utils) : base(Utils)
        {
        }

        private readonly By _byLnkRegisterAccount = By.XPath("//a[text()='Register for an account']");

        private readonly By _byTxtEmail = By.XPath("//input[@id='email']");

        private readonly By _byTxtPassword = By.XPath("//input[@id='password']");

        private readonly By _byLnkShowPassword = By.XPath("//span[text()='Show']");

        private readonly By _byLnkHidePassword = By.XPath("//span[text()='Hide']");

        private readonly By _byBtnSignIn = By.XPath("//button[@id='signin-button']");

        private readonly By _byLnkForgottenPassword = By.XPath("//span[contains(text(),'forgotten my password')]");

        public void EnterUsername(string value, bool doClearFirst = true)
        {
            Utils.SendKeys(_byTxtEmail, value, doClearFirst);
        }

        public void EnterPassword(string value, bool doClearFirst = true)
        {
            Utils.SendKeys(_byTxtPassword, value, doClearFirst);
        }

        public void ClickSignIn()
        {
            Utils.Click(_byBtnSignIn);
        }
    }
}