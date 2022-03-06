using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WebDriverWrapper.Utils;

namespace Tesco.Framework.PageInteractions
{
    public class Groceries_Search_Page : PageInteractionsBase
    {
        public Groceries_Search_Page(WebDriverUtils Utils) : base(Utils)
        {
        }

        private readonly By _byTxtSearch = By.XPath("//input[@id='search-input']");

        private readonly By _byBtnSearch = By.XPath("//button[text()='Search']");

        private readonly By _byLblTitleResult = By.XPath("//h1[@class='heading query']");

        //I have anchored this reference from the add button. That way we only return search results which have the add button. This is to keep the list count selectiomns in sync.
        //for example if I select the 8th item with an add button, then the 8th item with a title and price must be the same product
        //-----
        private readonly By _byBtntResultItemAdd = By.XPath("//li[contains(@class,'product-list--list')]//div[@data-auto='buybox-container']//button[contains(@class,'add')]");

        private readonly By _byLblResultItemTitle = By.XPath("//li[contains(@class,'product-list--list')]//div[@data-auto='buybox-container']//button[contains(@class,'add')]/ancestor::li[contains(@class,'product-list--list')]//a[@data-auto='product-title']//span");

        private readonly By _byLblResultItemPrice = By.XPath("//li[contains(@class,'product-list--list')]//div[@data-auto='buybox-container']//button[contains(@class,'add')]/ancestor::li[contains(@class,'product-list--list')]//div[@data-auto='buybox-container']//p[contains(@class,'StyledHeading')]");

        private readonly By _byTxtResultItemQuantity = By.XPath("//li[contains(@class,'product-list--list')]//div[@data-auto='buybox-container']//button[contains(@class,'add')]/ancestor::li[contains(@class,'product-list--list')]//div[@data-auto='buybox-container']//input[contains(@id,'quantity-controls')]");
        //-----

        //The website can return results in this v2 format too
        //-----
        private readonly By _byBtntResultItemAddV2 = By.XPath("//li[contains(@class,'product-list--list-item')]//div[@data-auto='product-controls']//button[contains(@class,'add')]");

        private readonly By _byLblResultItemTitleV2 = By.XPath("//li[contains(@class,'product-list--list-item')]//div[@data-auto='product-controls']//button[contains(@class,'add')]/ancestor::li[contains(@class,'product-list--list-item')]//a[contains(@data-auto,'product-tile')]");

        private readonly By _byLblResultItemPriceV2 = By.XPath("//li[contains(@class,'product-list--list-item')]//div[@data-auto='product-controls']//button[contains(@class,'add')]/ancestor::li[contains(@class,'product-list--list-item')]//div[@class='price-control-wrapper']//span[@data-auto='price-value']");

        private readonly By _byTxtResultItemQuantityV2 = By.XPath("//li[contains(@class,'product-list--list-item')]//div[@data-auto='product-controls']//button[contains(@class,'add')]/ancestor::li[contains(@class,'product-list--list-item')]//div[@class='inputControl-wrapper']//input");

        //-----

        private readonly By _byBtnResultReduceItemQuantity = By.XPath("//button[contains(@data-auto,'quantity-controls-remove-button')]");

        //using remove-button as an anchor then follow sibling, so the results pulled in are just the '+' buttons, otherwise it will pull in the 'Add' buttons too
        private readonly By _byBtnResultIncreaseItemQuantity = By.XPath("//button[contains(@data-auto,'quantity-controls-remove-button')]/following-sibling::button[contains(@data-auto,'quantity-controls-add-button')]");

        private readonly By _byLnkCheckout = By.XPath("//a[@class='button button-primary mini-trolley__checkout']");

        private readonly By _byLblCheckoutItemTitle = By.XPath("//div[@class='mini-tile__title-wrapper']//span");

        private readonly By _byLblCheckoutItemPrice = By.XPath("//span[@class='mini-tile__price']//span[@data-auto='price-value']");

        private readonly By _byBtnCheckoutItemRemove = By.XPath("//button[@data-auto='remove-item']");

        private readonly By _byBtnCheckoutItemReduceQuantity = By.XPath("//button[@class='button-secondary amount-adjust-button quantity-adjust mini-tile__amount-control mini-tile__amount-control--remove']");

        private readonly By _byBtnCheckoutItemIncreaseQuantity = By.XPath("//button[@class='button-secondary amount-adjust-button quantity-adjust mini-tile__amount-control mini-tile__amount-control--add']");

        private readonly By _byLinkClickAndCollect = By.XPath("//a[text()='Choose a Click+Collect slot']");

        //the website can return either V1 or V2 for these by references, so lets determine which one and use it going forward.
        private bool IsV2ElementsShowing => Utils.IsElementVisible(_byBtntResultItemAddV2);

        private By dynAddButtons => IsV2ElementsShowing ? _byBtntResultItemAddV2 : _byBtntResultItemAdd;
        private By dynTitles => IsV2ElementsShowing ? _byLblResultItemTitleV2 : _byLblResultItemTitle;
        private By dynPrices => IsV2ElementsShowing ? _byLblResultItemPriceV2 : _byLblResultItemPrice;

        private By _byimgBasketupdating = By.XPath("//div[@class='header-guide-price is-updating']");

        public string SearchResultTitle => Utils.GetElementText(_byLblTitleResult);

        public IEnumerable<string> ItemsInBasket => Utils.GetAllTexts(_byLblCheckoutItemTitle);

        public IEnumerable<string> ItemPricesInBasket => Utils.GetAllTexts(_byLblCheckoutItemPrice);

        private bool IsAppearedClickAndCollectPopup => Utils.IsElementVisible(_byLinkClickAndCollect);

        private static Random rnd = new Random();

        public Tuple<string, decimal> ClickAddItemToItemToBasket(int index, bool isHandleDeliveryNotAvailableBySelectingClickAndCollect)
        {
            //get all add buttons, titles and prices, and select specific one based on index
            var addButtons = Utils.GetElements(dynAddButtons);
            var addButton = addButtons[index];

            var titles = Utils.GetElements(dynTitles);
            string title = Utils.GetElementText(titles[index]);

            var prices = Utils.GetElements(dynPrices);
            decimal price = Utils.GetAmount(prices[index]);

            //to make sure the list of add buttons, titles and prices returned the same number of results, so that when we return the title and price, its accurate
            //in accordance with the item we added at a specific index
            ProductSyncCheck(addButtons, titles, prices);

            WaitUntilBasketFinishLoading();

            Utils.Click(addButton);

            if (IsAppearedClickAndCollectPopup)
            {
                if (isHandleDeliveryNotAvailableBySelectingClickAndCollect == false)
                    Assert.Ignore("A popup appeared stating that your address is not available for delivery, which has prevent the item from being added to the basket.");
                else
                    Assert.Ignore("Not implemented yet: Code to be written to handle the dynamic appearing click and collect popup and to select the nearest store");
            }

            return Tuple.Create(title, price);
        }

        public void WaitUntilBasketFinishLoading()
        {
            Utils.RepeatIfExeptionNullFalse(() => Utils.IsElementsPresent(_byimgBasketupdating) == false);
        }

        public Tuple<string, decimal> ClickAddRandomItemToItemToBasket()
        {
            Utils.WaitForAnyVisibleElement(dynAddButtons);

            int count = Utils.GetElements(dynAddButtons).Count;

            int randIndex = rnd.Next(count - 1);

            return ClickAddItemToItemToBasket(randIndex, false);
        }

        public decimal GetPriceForSpecificProduct(string product)
        {
            Utils.WaitForAnyVisibleElement(dynAddButtons);

            var titles = Utils.GetElements(dynTitles);
            var prices = Utils.GetElements(dynPrices);

            //to make sure the list of  titles and prices returned the same number of results, so that when we return the price, its accurate
            //in accordance with the product name we specified and found at a specific index
            ProductSyncCheck(titles, prices);

            int intSyncIndex = 0;

            //find the correct index within titles and use that same index to get the corresponding price.

            foreach (var title in titles)
            {
                var a = Utils.GetElementText(title);

                if (product == Utils.GetElementText(title))
                    return Utils.GetAmount(prices[intSyncIndex]);

                intSyncIndex++;
            }

            return 0;
        }

        private void ProductSyncCheck(params ReadOnlyCollection<IWebElement>[] elementsLst)
        {
            //this is a fail-sage check, to verify comparing lists have the same number of elements.

            var listCount = new List<int>();

            //Count the number of elements in each element list
            foreach (var elements in elementsLst)
                listCount.Add(elements.Count);

            //then make sure that the count of each element list is the same. That way checking that all the lists have the same amount of values returned
            if (listCount.Distinct().Count() != 1)
                throw new Exception("When checking the number of elements in each element list, there was a mismatch in the counts between lists");
        }

        public int NumItemsInBasket => Utils.GetElements(_byBtnCheckoutItemRemove).Count;

        public void RemoveFirstItemFromBasket()
        {
            Utils.Click(_byBtnCheckoutItemRemove);
        }

        public void RemoveItemAtIndexFromBasket(int index)
        {
            var elements = Utils.GetElements(_byBtnCheckoutItemRemove);

            Utils.Click(elements[index]);
        }

        public void RemoveAllItemsFromBasket()
        {
            foreach (var element in Utils.GetElements(_byBtnCheckoutItemRemove))
                Utils.Click(element);
        }
    }
}