#define SPEEDUP

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Globalization;
using WebDriverWrapper.Properties;

namespace WebDriverWrapper.Utils
{
    public class BaseUtils
    {
        public BaseUtils(IWebDriver p_Driver)
        {
            Driver = p_Driver;

            Wait = GetStandardWaitConfig();

            Driver.Manage().Timeouts().PageLoad = Wait.Timeout;
        }

        public WebDriverWait GetStandardWaitConfig()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(SettingsWebDriver.Default.Timeout));

            //Wait.Until wil continue to retry if any of these exceptions are thrown until the timeout is reached, and the exception will be shown as an inner exception)
            wait.IgnoreExceptionTypes(

                typeof(NoSuchElementException), // << this ignore doesnt work
                typeof(ElementNotVisibleException),
                typeof(ElementNotInteractableException),
                typeof(ElementNotSelectableException),
                typeof(StaleElementReferenceException),
                typeof(ElementClickInterceptedException),
                typeof(InvalidElementStateException));

            return wait;
        }

        public IWebDriver Driver { get; private set; }

        public WebDriverWait Wait { get; set; }

        public static CultureInfo UkCulture { get; } = new CultureInfo("en-GB");

        public string GetDomain => new Uri(Driver.Url).GetLeftPart(UriPartial.Authority);

        protected void SetWebDriverTimeout(double p_Seconds)
        {
            Wait.Timeout = TimeSpan.FromSeconds(p_Seconds);
            Driver.Manage().Timeouts().PageLoad = Wait.Timeout;
        }
    }
}