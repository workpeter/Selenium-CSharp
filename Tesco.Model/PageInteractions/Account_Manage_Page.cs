using OpenQA.Selenium;
using WebDriverWrapper.Utils;

namespace Tesco.Framework.PageInteractions
{
    public class Account_Manage_Page : PageInteractionsBase
    {
        public Account_Manage_Page(WebDriverUtils Utils) : base(Utils)
        {
        }

        private readonly By _byLnkGroceryOrders = By.XPath("//span[text()='Grocery orders']");

        private readonly By _byLnkDirectOrders = By.XPath("//span[text()='Direct orders']");

        private readonly By _byLnkDeliverySaver = By.XPath("//span[text()='Delivery saver']");

        private readonly By _byLnkPersonalDetails = By.XPath("//span[text()='Personal details']");

        private readonly By _byLnkAddressBook = By.XPath("//span[text()='Address book']");

        private readonly By _byLnkChangePassword = By.XPath("//span[text()='Change password']");

        private readonly By _byLnkRequestYourTescoData = By.XPath("//span[text()='Request your Tesco data']");

        private readonly By _byLnkClubcardPlus = By.XPath("//span[text()='Clubcard Plus']");

        private readonly By _byLnkClubcardsOnAccount = By.XPath("//span[text()='Clubcards on account']");

        private readonly By _byLnkOrderNewClubcard = By.XPath("//span[text()='Order a new Clubcard']");

        private readonly By _byLnkStatementPreferences = By.XPath("//span[text()='Statement Preferences']");

        private readonly By _byLnkViewClubcardAccount = By.XPath("//span[text()='View Clubcard account']");

        private readonly By _byLnkMarketingCommunications = By.XPath("//span[text()='Marketing communications']");

        private readonly By _byLnkDietaryNeeds = By.XPath("//span[text()='Dietary needs']");

        public void ClickPersonalDetails()
        {
            Utils.Click(_byLnkPersonalDetails);
        }
    }
}