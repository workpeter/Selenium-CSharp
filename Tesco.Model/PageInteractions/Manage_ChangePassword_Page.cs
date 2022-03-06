using OpenQA.Selenium;
using WebDriverWrapper.Utils;

namespace Tesco.Framework.PageInteractions
{
    public class Manage_ChangePassword_Page : PageInteractionsBase
    {
        public Manage_ChangePassword_Page(WebDriverUtils Utils) : base(Utils)
        {
        }

        private readonly By _byCurrentPassword = By.XPath("//input[@name='currentPassword']");

        private readonly By _byNewPassword = By.XPath("//input[@name='newPassword']");

        private readonly By _byConfirmNewPassword = By.XPath("//input[@name='confirmPassword']");

        private readonly By _byBtnSaveChanges = By.XPath("//span[text()='Save changes']/parent::button");
    }
}