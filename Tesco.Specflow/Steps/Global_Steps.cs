using TechTalk.SpecFlow;
using Tesco.Specflow.Base;
using Tesco.Framework.Models;

namespace Tesco.Specflow.Steps
{
    [Binding]
    public sealed class Global_Steps : FeatureBase
    {
        public Global_Steps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [Given(@"navigate to the tesco homepage")]
        [When(@"navigate to the tesco homepage")]
        public void GivenNavigateToTheTescoHomepage()
        {
            Utils.Driver.Url = Scenario.TestDomain;

            Index_Page.ClickAcceptAllCookies();
        }

        [Then(@"Account data has been written to the data database")]
        public void ThenAccountDataHasBeenWrittenToTheDataDatabase()
        {
            //Ideally our automated solution would be hooked directly into the dev database supporting the tesco website, which would mean the application would be writing the data to the DB.
            //However because this is a demo solution against the live tesco website, I am using my own database, so I have to write the data explicity.
            //I am writing account data to the database, so that it can be used in future, i.e. for a login scenario.
            //I have also designed various linked tables in the DB so that when we pull the information back out, we have a rich source of information about the account and the account holder.

            using (var db = new ModelTesco())
            {
                db.Accounts.Add(Scenario.Account);
                db.SaveChanges();
            }
        }
    }
}