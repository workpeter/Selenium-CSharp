using OpenQA.Selenium;
using WebDriverWrapper.Utils;

namespace Tesco.Framework.PageInteractions
{
    public class StoreLocator_Page : PageInteractionsBase
    {
        public StoreLocator_Page(WebDriverUtils Utils) : base(Utils)
        {
        }

        private readonly By _byLblStoreTitle = By.XPath("//span[@class='Text Teaser-text Teaser-heading']");

        private readonly By _byLblStorePostcode = By.XPath("//span[@class='Address-field Address-postalCode']");

        private readonly By _byLblStoreDistance = By.XPath("//div[@class='Teaser-miles']");

        public string GetStoreTitle(int index)
        {
            var elements = Utils.GetElements(_byLblStoreTitle);

            return Utils.GetElementText(elements[index]);
        }

        public string GetStorePostcode(int index)
        {
            var elements = Utils.GetElements(_byLblStorePostcode);

            return Utils.GetElementText(elements[index]);
        }

        public string GetStoreDistance(int index)
        {
            var elements = Utils.GetElements(_byLblStoreDistance);

            return Utils.GetElementText(elements[index]);
        }
    }
}