#define SPEEDUP

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Linq;

namespace WebDriverWrapper.Utils
{
    public partial class WebDriverUtils
    {
        public By CurrentSelector { get; set; }

        private IWebElement _currentElement;

        public IWebElement CurrentElement
        {
            get
            {
                Debug_ShowHighlighter(_currentElement);
                return _currentElement;
            }
            set
            {
                _currentElement = value;
            }
        }

        public IWebElement GetElement(By p_Selector, WaitType waitType = WaitType.Present)
        {
            CurrentSelector = p_Selector;

            return RepeatIfExeptionNullFalse((d) =>
           {
               //(.FindElement not used to avoid throwing NoSuchElementException during debug mode)
               var elements = GetElements(CurrentSelector);

               switch (waitType)
               {
                   case WaitType.Present: //Quickest and least restrictive
                   default:
                       //return FIRST element matching the By selector is present in the DOM
                       return CurrentElement = elements.FirstOrDefault();

                   case WaitType.Visible:
                       //return FIRST element matching the By selector and only if its visible (ignores visibility of subsequent matching elements)
                       var element = elements.FirstOrDefault();
                       return CurrentElement = element != null && IsElementVisible(CurrentElement) ? element : null;

                   case WaitType.AnyVisible:
                       //return the first of ANY element matching the By selector that is visible
                       return CurrentElement = elements.Where(e => IsElementVisible(e)).FirstOrDefault();

                   case WaitType.Clickable: //most reliable (but slowest - and needs to be clickable)

                       //return FIRST element matching the By selector is clickable (ignores clickablity of subsequent matching elements)
                       element = elements.FirstOrDefault();
                       return CurrentElement = SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element)(d);
               }
           });
        }

        public ReadOnlyCollection<IWebElement> GetElements(By p_Selector, bool p_TimeoutIfNoneFound = true)
        {
            CurrentSelector = p_Selector;
            ReadOnlyCollection<IWebElement> elements = null;

            if (p_TimeoutIfNoneFound)
                RepeatIfExeptionNullFalse(() => elements = Driver.FindElements(CurrentSelector)); //has to return a value or will timeout
            else
                RepeatIfExeption(() => elements = Driver.FindElements(CurrentSelector)); //doesnt need to return a value, will only repeat on exception

            return elements;
        }

        public ReadOnlyCollection<IWebElement> GetVisibleElements(By p_Selector)
        {
            CurrentSelector = p_Selector;

            //then return all elements currently loaded and visible
            return RepeatIfExeptionNullFalse(() => GetElements(CurrentSelector).Where(e => IsElementVisible(e)).ToList().AsReadOnly());
        }

        public IWebElement GetFirstVisbleElement(By p_Selector)
        {
            return GetVisibleElements(p_Selector).FirstOrDefault();
        }

        private SelectElement GetSelect(By p_Selector, WaitType p_WaitType = WaitType.Clickable)
        {
            CurrentSelector = p_Selector;

            return new SelectElement(GetElement(CurrentSelector, p_WaitType));
        }

        public static By FindByElementText(string p_Text, string p_ElementType = "a")
        {
            return By.XPath($"//{p_ElementType}[contains(translate(text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') ,'{p_Text.ToLower()}')]");
        }
    }
}