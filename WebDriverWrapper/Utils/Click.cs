#define SPEEDUP

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace WebDriverWrapper.Utils
{
    public partial class WebDriverUtils : BaseUtils
    {
        public WebDriverUtils(IWebDriver p_Driver) : base(p_Driver)
        {
        }

        public void Click(By p_Selector, ClickType p_ClickType = ClickType.GetClickableElement_Click, By p_SelectorWaitFor = null)
        {
            CurrentSelector = p_Selector;

            bool hasTriggered = false;

            RepeatIfExeption(() =>
            {
                //If the click has to be repeated, switch to another realistic click type, but one which is more reliable (but a bit slower).
                if (hasTriggered)
                    p_ClickType = SwitchClickType(p_ClickType);

                try
                {
                    Actions builder;
                    IWebElement element;
                    IJavaScriptExecutor executor;

                    switch (p_ClickType)
                    {
                        case ClickType.GetElement_Click: //fastest, but least reliable
                            CurrentElement = Driver.FindElement(CurrentSelector);
                            CurrentElement.Click();
                            break;

                        default:
                        case ClickType.GetClickableElement_Click: //reliable
                            GetElement(CurrentSelector, WaitType.Clickable).Click();
                            break;

                        case ClickType.GetClickableElement_ActionClick: //workaround if the standard webdriver click doesnt work
                            builder = new Actions(Driver);
                            builder.Click(GetElement(CurrentSelector, WaitType.Clickable)).Perform();
                            break;

                        case ClickType.ScrollToElement_ActionClick: //if getting 'element has zero size' error then use this click
                            element = ScrollToElement(CurrentSelector, false);
                            builder = new Actions(Driver);
                            builder.Click().Perform();
                            break;

                        case ClickType.GetClickableElement_DoubleActionClick: //double click
                            builder = new Actions(Driver);
                            builder.DoubleClick(GetElement(CurrentSelector, WaitType.Clickable)).Perform();
                            break;

                        case ClickType.JavaScript_Click: //use this as a last resort workaround. It's an unrealstic (code based) click.

                            element = GetElement(p_Selector);
                            executor = (IJavaScriptExecutor)Driver;
                            executor.ExecuteScript("arguments[0].click();", element);
                            break;

                        case ClickType.ScrollJavaScript_Click: //use this as a last resort workaround. It's an unrealstic (code based) click.

                            element = ScrollToElement(CurrentSelector, true);
                            executor = (IJavaScriptExecutor)Driver;
                            executor.ExecuteScript("arguments[0].click();", element);
                            break;
                    }
                }
                finally
                {
                    //specify for any adhoc selectors to wait for after click
                    if (p_SelectorWaitFor != null)
                        WaitForElement(p_SelectorWaitFor);

                    hasTriggered = true;
                }
            });

            ClickType SwitchClickType(ClickType p_ClickTypeToBeSwitched)
            {
                if (p_ClickTypeToBeSwitched == ClickType.GetElement_Click)
                    return ClickType.GetClickableElement_Click;
                else if (p_ClickTypeToBeSwitched == ClickType.GetClickableElement_Click) ;
                return ClickType.ScrollToElement_ActionClick;
            }
        }

        //overloader - Useful for some methods which have to send Element directly rather than By reference.
        public void Click(IWebElement p_Element, By p_SelectorWaitFor = null)
        {
            CurrentElement = p_Element;

            RepeatIfExeption(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(CurrentElement)(Driver).Click);

            //specify for any adhoc selectors to wait for after click
            if (p_SelectorWaitFor != null)
                WaitForElement(p_SelectorWaitFor);
        }

        //Only to be used in rare situations as a workaround. Always use webdriver click where possible to simulate end user.
        public void JavaScriptClick(IWebElement p_Element, bool IsWithScroll = false)
        {
            IJavaScriptExecutor executor = (IJavaScriptExecutor)Driver;

            if (IsWithScroll)
                executor.ExecuteScript("arguments[0].scrollIntoView(true);", p_Element);

            executor.ExecuteScript("arguments[0].click();", p_Element);
        }

        public void ClickUntilVisbility(By p_SelectorClick, By p_SelectorCheck, bool p_UntilVisibilityIs, int p_DelayMilliSecBetweenClicks = 500, ClickType p_ClickType = ClickType.GetClickableElement_Click)
        {
            //p_UntilVisibilityIs == true [Click until visible]
            //p_UntilVisibilityIs == false [Click until invisible]

            //Keep clicking the p_SelectorClick until the selectorCheck is the expected visibility

            if (IsSelectorCheckExpectedVisibility())
                return;

            WaitForElement(p_SelectorClick, WaitType.Clickable);

            RepeatIfExeptionNullFalse(() =>
                  {
                      CurrentSelector = p_SelectorClick;

                      if (IsElementClickable(CurrentSelector) == false) //if the p_SelectorClick is no longer visible, then return true, so we dont attempt further click/s
                          return true;

                      Click(CurrentSelector, p_ClickType);

                      Thread.Sleep(TimeSpan.FromMilliseconds(p_DelayMilliSecBetweenClicks));

                      return IsSelectorCheckExpectedVisibility();
                  });

            bool IsSelectorCheckExpectedVisibility()
            {
                bool selectorCheckIsVisible = IsElementVisible(p_SelectorCheck);

                return p_UntilVisibilityIs ? selectorCheckIsVisible : !selectorCheckIsVisible;
            }
        }

        public void ClickEachElement(By p_ElementSel)
        {
            CurrentSelector = p_ElementSel;

            IList<IWebElement> elementList = GetElements(CurrentSelector);

            foreach (IWebElement element in elementList)
                Click(element);
        }
    }

    public enum ClickType
    {
        GetElement_Click,
        GetClickableElement_Click,
        GetClickableElement_ActionClick,
        ScrollToElement_ActionClick,
        GetClickableElement_DoubleActionClick,
        JavaScript_Click,
        ScrollJavaScript_Click,
    }
}