using TechTalk.SpecFlow;
using Tesco.Framework.Enums;
using Tesco.Specflow.Base;
using UsefulMethods;

namespace Tesco.Specflow.BusinessLogic
{
    public class AccountPortal_BL : ScenarioBase
    {
        public AccountPortal_BL(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public int ConvertMarketingCommunicationFromStringToInt(string value)
        {
            return (int)new Converters().ConvertToEnum<MarketingCommunicationType>(value);
        }

        public string ConvertMarketingCommunicationFromIntToString(int value)
        {
            return new Converters().ConvertToString((MarketingCommunicationType)value);
        }

        public int ConvertClubCardStatusFromStringToInt(string value)
        {
            return (int)new Converters().ConvertToEnum<ClubCardStatusType>(value);
        }

        public string ConvertClubCardStatuFromIntToString(int value)
        {
            return new Converters().ConvertToString((ClubCardStatusType)value);
        }
    }
}