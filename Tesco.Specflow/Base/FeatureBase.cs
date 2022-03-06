using TechTalk.SpecFlow;
using Tesco.Framework.PageInteractions;
using Tesco.Specflow.BusinessLogic;

namespace Tesco.Specflow.Base
{
    public class FeatureBase : ScenarioBase
    {
        public FeatureBase(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            SetPageInteractionObjects();
            SetBusinessLogicObjects();
        }

        public Account_Login_Page Account_Login_Page;
        public Account_Login_UpdateDetails_Page Account_Login_UpdateDetails_Page;
        public Account_Manage_Page Account_Manage_Page;
        public Account_PersonalDetails_Page Account_PersonalDetails_Page;
        public Account_PersonalDetailsName_Page Account_PersonalDetailsName_Page;
        public Account_Register_Confirm_Page Account_Register_Confirm_Page;
        public Account_Register_Page Account_Register_Page;
        public Groceries_Search_Page Groceries_Search_Page;
        public Index_Page Index_Page;
        public Manage_ChangePassword_Page Manage_ChangePassword_Page;
        public StoreLocator_Page StoreLocator_Page;

        public AccountPortal_BL AccountPortal_BL;
        public Global_BL Global_BL;
        public Search_BL Search_BL;
        public StoreLocator_BL StoreLocator_BL;

        public void SetPageInteractionObjects()
        {
            Account_Login_Page = new Account_Login_Page(Utils);
            Account_Login_UpdateDetails_Page = new Account_Login_UpdateDetails_Page(Utils);
            Account_Manage_Page = new Account_Manage_Page(Utils);
            Account_PersonalDetails_Page = new Account_PersonalDetails_Page(Utils);
            Account_PersonalDetailsName_Page = new Account_PersonalDetailsName_Page(Utils);
            Account_Register_Confirm_Page = new Account_Register_Confirm_Page(Utils);
            Account_Register_Page = new Account_Register_Page(Utils);
            Groceries_Search_Page = new Groceries_Search_Page(Utils);
            Index_Page = new Index_Page(Utils);
            Manage_ChangePassword_Page = new Manage_ChangePassword_Page(Utils);
            StoreLocator_Page = new StoreLocator_Page(Utils);
        }

        public void SetBusinessLogicObjects()
        {
            AccountPortal_BL = new AccountPortal_BL(ScenarioContext);
            Global_BL = new Global_BL(ScenarioContext);
            Search_BL = new Search_BL(ScenarioContext);
            StoreLocator_BL = new StoreLocator_BL(ScenarioContext);
        }
    }
}