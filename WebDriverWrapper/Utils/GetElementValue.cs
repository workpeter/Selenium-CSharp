#define SPEEDUP

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebDriverWrapper.Utils
{
    public partial class WebDriverUtils
    {
        public string GetElementText(By p_Selector, WaitType p_WaitType = WaitType.Present)
        {
            return RepeatIfExeptionNullFalse(() => GetElementText(GetElement(p_Selector, p_WaitType)));
        }

        public string GetElementText(IWebElement element)
        {
            return (!string.IsNullOrEmpty(element.Text)) ? element.Text : element.GetAttribute("innerText");
        }

        public string GetElementValue(By p_Selector)
        {
            return RepeatIfExeptionNullFalse(() => GetElementValue(GetElement(p_Selector)));
        }

        public string GetElementValue(IWebElement p_IWebElement)
        {
            //not going via Get incase element is disabled
            return p_IWebElement.GetAttribute("value");
        }

        public IEnumerable<string> GetAllValues(By p_Sel)
        {
            return RepeatIfExeptionNullFalse(() => GetElements(p_Sel).Select(we => we.GetAttribute("value")));
        }

        public IEnumerable<string> GetAllTexts(By p_Sel)
        {
            return RepeatIfExeptionNullFalse(() => GetElements(p_Sel).Select(we => we.Text));
        }

        public decimal GetAmount(By p_Selector)
        {
            return IsElementVisible(p_Selector) ? GetAmount(GetElement(p_Selector, WaitType.Clickable)) : 0.00m;
        }

        public decimal GetAmount(IWebElement element)
        {
            if (element == null) return 0.00m;

            string extract = string.Empty;

            foreach (char c in GetElementText(element))
            {
                if (Char.IsDigit(c) || c == '.' || c == '-')
                    extract += c;
            }

            return (extract.Length > 0) ? decimal.Parse(extract) : 0.00m;
        }

        public decimal GetAmount(string p_Amount)
        {
            if (string.IsNullOrEmpty(p_Amount)) return 0.00m;

            string extract = string.Empty;

            foreach (char c in p_Amount)
            {
                if (Char.IsDigit(c) || c == '.' || c == '-')
                    extract += c;
            }

            return (extract.Length > 0) ? decimal.Parse(extract) : 0.00m;
        }

        public decimal GetPercantage(By p_Selector)
        {
            return IsElementVisible(p_Selector) ? GetPercantage(GetElement(p_Selector, WaitType.Clickable)) : 0;
        }

        public decimal GetPercantage(IWebElement element)
        {
            decimal value = GetAmount(element);

            return value > 0 ? value / 100 : 0;
        }

        public IEnumerable<string> GetAllOptionTexts(By p_Sel, bool p_RemoveBlankValue = false)
        {
            return RepeatIfExeptionNullFalse(() =>
            {
                IEnumerable<IWebElement> availableOptions = GetSelect(p_Sel).Options;

                if (p_RemoveBlankValue)
                {
                    availableOptions = availableOptions.Where(o => !string.IsNullOrEmpty(o.GetAttribute("value")));
                }

                return availableOptions.Select(we => we.Text);
            });
        }

        public bool DoesOptionExist(By p_Sel, string p_Value)
        {
            return GetAllOptionTexts(p_Sel).Any(x => x == p_Value);
        }

        public string GetSelectedText(By p_ElementSel, WaitType p_WaitType = WaitType.Clickable)
        {
            return RepeatIfExeptionNullFalse(() => GetSelect(p_ElementSel, p_WaitType).SelectedOption.Text);
        }
    }
}