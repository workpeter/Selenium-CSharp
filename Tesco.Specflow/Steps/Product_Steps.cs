using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Tesco.Framework.Models;
using Tesco.Specflow.Base;

namespace Tesco.Specflow.Steps
{
    [Binding]
    public sealed class Product_Steps : FeatureBase
    {
        public Product_Steps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [When(@"search by product type:(.*)")]
        public void WhenSearchByProductTypeBread(string productType)
        {
            Index_Page.EnterSearchValue(productType);
            Index_Page.ClickBtnSearch();

            Assert.IsTrue(Groceries_Search_Page.SearchResultTitle.Contains(productType));
        }

        [When(@"clear any existing items in the basket")]
        public void WhenClearAnyExistingItemsInTheBasket()
        {
            int before = Groceries_Search_Page.NumItemsInBasket;

            if (before > 0)
                Groceries_Search_Page.RemoveAllItemsFromBasket();

            int after = Groceries_Search_Page.NumItemsInBasket;

            Assert.AreEqual(0, after);
        }

        [When(@"add to basket (.*) random products from the search results")]
        public void WhenAddToBasketRandomProductsFromTheSearchResults(int numItemsToBasket)
        {
            var selectedProdcts = new Dictionary<string, decimal>();

            for (int i = 0; i < numItemsToBasket; i++)
            {
                //returns product name and price for choosen product
                var productInfo = Groceries_Search_Page.ClickAddRandomItemToItemToBasket();

                selectedProdcts.Add(productInfo.Item1, productInfo.Item2);
            }

            Scenario.SelectedProducts = selectedProdcts;
        }

        [Then(@"verify the items were added to the basket with the expected price")]
        public void ThenVerifyTheItemsWereAddedToTheBasketWithTheExpectedPrice()
        {
            //Verifying the selected item/s appeared in the basket with the correct product name and price

            var ProductAndPriceFoundInBasketAtSameIndex = new List<bool>();

            //loop through the select product names and see if that name can be found anywhere in the basket
            //do the same for prices
            foreach (var selectedItem in Scenario.SelectedProducts)
            {
                int intSyncIndex = 0;

                bool isProductFound = false;
                bool isPriceFound = false;

                foreach (var item in Groceries_Search_Page.ItemsInBasket)
                {
                    //if product name is found in basket, mark as product found
                    if (selectedItem.Key == item)
                    {
                        isProductFound = true;

                        //verify too if the price exists in the basket at the same index. This is to ensure that the product with its price was added correctly in the basket
                        var prices = Groceries_Search_Page.ItemPricesInBasket.ToList();

                        if (selectedItem.Value == decimal.Parse(prices[intSyncIndex]))
                        {
                            isPriceFound = true;
                        }

                        //confirm both product and its price is found together in the basket at the same index, then break from loop
                        if (isProductFound == true && isPriceFound == true)
                        {
                            ProductAndPriceFoundInBasketAtSameIndex.Add(true);
                            break;
                        }
                    }

                    intSyncIndex++;
                }
            }

            //count how many items we matched correctly in the basket
            int matchCount = ProductAndPriceFoundInBasketAtSameIndex.Where(x => x == true).Count();

            //Assert that we found every product we selected.
            Assert.IsTrue(matchCount == Scenario.SelectedProducts.Count);
        }

        private string productSearched;
        private decimal productPriceReturned;

        [When(@"get the price for product:(.*)")]
        public void WhenGetThePriceForProduct(string product)
        {
            productSearched = product;
            productPriceReturned = Groceries_Search_Page.GetPriceForSpecificProduct(product);
        }

        [Then(@"independently verify the price is correct")]
        public void ThenIndependentlyVerifyThePriceIsCorrect()
        {
            //hook into our DB, which has an indepdent table of products and prices and verify the price captured matches what we have in our DB.

            using (var db = new ModelTesco())
            {
                decimal expectedPrice = db.Products.Where(p => p.Name == productSearched).Select(p => p.Price).FirstOrDefault();

                //Please note: if this fails, please check that our independent prices in the DB have been updated.
                //As this is a demo, its possible that the prices have since changed on the live website and I havent updated them in my demo database.
                //In a real world scenario, I would have a process in place where we ensure our independent DB is kept up to date.
                //Alternatively we could use the actual DB the application supports, but then we would need a process in place to validate that the product/price get updated correctly in the DB.
                //If we have confidence in the prices in the DB, then we simple just need to do a UI vs. DB check.
                Assert.AreEqual(expectedPrice, productPriceReturned);
            }
        }
    }
}