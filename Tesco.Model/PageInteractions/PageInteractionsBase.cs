using WebDriverWrapper.Utils;

namespace Tesco.Framework.PageInteractions
{
    public abstract class PageInteractionsBase
    {
        public PageInteractionsBase(WebDriverUtils Utils)
        {
            this.Utils = Utils;
        }

        public WebDriverUtils Utils { get; }

        public static decimal AssertionTolerance => 0.04m; //allow 4p tolerance to negate any small mismatches due to rounding
    }
}