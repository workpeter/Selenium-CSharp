#define SPEEDUP

using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace WebDriverWrapper.Utils
{
    public partial class WebDriverUtils
    {
        public void WaitForElement(By p_Selector, WaitType waitType = WaitType.Clickable)
        {
            CurrentSelector = p_Selector;

            switch (waitType)
            {
                case WaitType.Present:
                    RepeatIfExeptionNullFalse((d) => IsElementsPresent(p_Selector)); //wait until ANY element matching the By selector is present in the DOM
                    break;

                case WaitType.Visible:
                    RepeatIfExeptionNullFalse((d) => IsElementVisible(p_Selector)); //wait until the FIRST element matching the By selector is visible (ignores visibility of subsequent matching elements)
                    break;

                case WaitType.AnyVisible:
                    RepeatIfExeptionNullFalse((d) => IsAnyElementVisible(p_Selector)); //wait until ANY element matching the By selector is visible
                    break;

                case WaitType.Clickable:  //most reliable (but slowest - and needs to be clickable)
                default:
                    RepeatIfExeptionNullFalse((d) => IsElementClickable(p_Selector)); //wait until the FIRST element matching the By selector is clickable (ignores clickablity of subsequent matching elements)
                    break;
            }
        }

        public void WaitForAnyVisibleElement(By p_Selector)
        {
            //If ANY of the elements matching the selector is visible, then IsAnyElementVisible will be true.
            //We wait until that condition is true
            RepeatIfExeptionNullFalse((d) => IsAnyElementVisible(p_Selector));
        }

        public void WaitForAnyVisibleElement(List<By> p_Selectors)
        {
            //Can pass 1 or more different By selectors as a List.
            //For each By selector, if ANY of the elements matching that selector is visible, then IsAnyElementVisible will be true.
            //We wait until that condition is true
            RepeatIfExeptionNullFalse((d) => IsAnyElementVisible(p_Selectors));
        }

        public void WaitForAllElementsNotVisible(By p_Selector)
        {
            CurrentSelector = p_Selector;

            //We wait until All elements matching the By selector are invisible
            RepeatIfExeptionNullFalse((d) => GetElements(CurrentSelector, false).Where(e => IsElementVisible(e)).Any() == false);
        }

        public bool IsElementsPresent(By p_Selector)
        {
            //Is ANY element matching the By selector present in the DOM

            CurrentSelector = p_Selector;

            bool elementPresent = false;

            RepeatIfExeption(() =>
            {
                CurrentElement = GetElements(CurrentSelector, false).FirstOrDefault();
                elementPresent = CurrentElement != null;
            });

            return elementPresent;
        }

        public bool IsElementVisible(By p_Selector)
        {
            //Is the FIRST element matching the By selector visible (ignores visibility of subsequent matching elements)

            return IsElementsPresent(p_Selector) && IsElementVisible(CurrentElement);
        }

        public bool IsElementVisible(IWebElement p_Element)
        {
            //This is the same as .Displayed but safer to use because it wont throw an exception if the element goes stale.
            return SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(new List<IWebElement>() { p_Element }.AsReadOnly())(Driver) != null;
        }

        public bool IsElementClickable(By p_Selector)
        {
            //Is the FIRST element matching the By selector clickable (ignores clickability of subsequent matching elements)
            return IsElementsPresent(p_Selector) && SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(CurrentElement)(Driver) != null;
        }

        public bool IsElementClickable(IWebElement p_Element)
        {
            CurrentElement = p_Element;

            //Is the FIRST element matching the By selector clickable (ignores clickability of subsequent matching elements)
            return SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(CurrentElement)(Driver) != null;
        }

        public bool IsAnyElementVisible(By p_Selector)
        {
            CurrentSelector = p_Selector;

            bool anyElementVisible = false;

            RepeatIfExeption(() => anyElementVisible = GetElements(CurrentSelector, false).Where(e => IsElementVisible(e)).Any()); //is ANY element matching the current By selector displayed

            return anyElementVisible;
        }

        public bool IsAnyElementVisible(List<By> p_Selectors)
        {
            bool anyElementVisible = false;

            RepeatIfExeption(() =>
            {
                //can loop through different By selectors
                foreach (By selector in p_Selectors)
                {
                    CurrentSelector = selector;

                    anyElementVisible = IsAnyElementVisible(CurrentSelector);

                    if (anyElementVisible)
                        break;
                }
            });

            return anyElementVisible;
        }

        public bool IsElementStale(IWebElement element)
        {
            CurrentElement = element;

            return SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(CurrentElement)(Driver);
        }
    }

    public enum WaitType
    {
        Present,
        Visible,
        AnyVisible,
        Clickable
    }
}