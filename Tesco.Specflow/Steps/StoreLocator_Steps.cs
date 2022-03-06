using NUnit.Framework;
using TechTalk.SpecFlow;
using Tesco.Specflow.Base;

namespace Tesco.Specflow.Steps
{
    [Binding]
    public sealed class StoreLocator_Steps : FeatureBase
    {
        public StoreLocator_Steps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [When(@"search for store using postcode:(.*)")]
        public void WhenSearchForStoreUsingPostcodeCTQA(string postcode)
        {
            Index_Page.ClickMenuLinkStorelocator();
            Index_Page.EnterPostcodeForStoreLocator(postcode);
            Index_Page.ClickFindStore();
        }

        [Then(@"verify the top 2 stores are Store1Name:(.*) Store1Postcode:(.*) Store1Distance:(.*) Store2Name:(.*) Store2Postcode:(.*) Store2Distance:(.*)")]
        public void ThenVerifyTheTopStoresAreStoreNameBroadstairsExtraStorePostcodeCTQJStoreDistanceStoreNameRamsgateManstonSuperstoreStorePostcodeCTNTStoreDistance(string expectedStore1Name,
                                                                                                                                                                     string expectedStore1Postcode,
                                                                                                                                                                     string expectedStore1Distance,
                                                                                                                                                                     string expectedStore2Name,
                                                                                                                                                                     string expectedStore2Postcode,
                                                                                                                                                                     string expectedStore2Distance)
        {
            string actualStore1Name = StoreLocator_Page.GetStoreTitle(0);
            string actualStore1Postcode = StoreLocator_Page.GetStorePostcode(0);
            string actualStore1Distance = StoreLocator_Page.GetStoreDistance(0).Split(' ')[0];

            Assert.AreEqual(expectedStore1Name, actualStore1Name);
            Assert.AreEqual(expectedStore1Postcode, actualStore1Postcode);
            Assert.AreEqual(expectedStore1Distance, actualStore1Distance);

            //Do send store applicable check
            string expectedStore2NameLower = expectedStore2Name.ToLower();

            if (expectedStore2NameLower != "na" && expectedStore2NameLower != "n/a")
            {
                string actualStore2Name = StoreLocator_Page.GetStoreTitle(1);
                string actualStore2Postcode = StoreLocator_Page.GetStorePostcode(1);
                string actualStore2Distance = StoreLocator_Page.GetStoreDistance(1).Split(' ')[0];

                Assert.AreEqual(expectedStore2Name, actualStore2Name);
                Assert.AreEqual(expectedStore2Postcode, actualStore2Postcode);
                Assert.AreEqual(expectedStore2Distance, actualStore2Distance);
            }
        }
    }
}