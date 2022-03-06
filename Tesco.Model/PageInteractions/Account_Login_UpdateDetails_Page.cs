using OpenQA.Selenium;
using WebDriverWrapper.Utils;

namespace Tesco.Framework.PageInteractions
{
    public class Account_Login_UpdateDetails_Page : PageInteractionsBase
    {
        public Account_Login_UpdateDetails_Page(WebDriverUtils Utils) : base(Utils)
        {
        }

        private readonly By _byLnkRemindMeLater = By.XPath("//span[text()='Remind me later']/parent::a");

        public void isExistClickRemindMeLater()
        {
            if (Utils.IsElementVisible(_byLnkRemindMeLater))
                Utils.Click(_byLnkRemindMeLater);
        }
    }
}