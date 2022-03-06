using OpenQA.Selenium;
using WebDriverWrapper.Utils;

namespace Tesco.Framework.PageInteractions
{
    public class Account_PersonalDetailsName_Page : PageInteractionsBase
    {
        public Account_PersonalDetailsName_Page(WebDriverUtils Utils) : base(Utils)
        {
        }

        private readonly By _bySelTitle = By.XPath("//select[@name='title']");

        private readonly By _byTxtFirstName = By.XPath("//input[@name='first-name']");

        private readonly By _byTxtLastName = By.XPath("//input[@name='last-name']");

        private readonly By _byBtnSaveChanges = By.XPath("//span[text()='Save changes']/ancestor::button");

        public void SelectTitle(string value)
        {
            Utils.SelectByText(_bySelTitle, value);
        }

        public void EnterFirstName(string value)
        {
            Utils.SendKeys(_byTxtFirstName, value, true);
        }

        public void EnterLasttName(string value)
        {
            Utils.SendKeys(_byTxtLastName, value, true);
        }

        public void ClickSaveChanges()
        {
            Utils.Click(_byBtnSaveChanges);
        }
    }
}