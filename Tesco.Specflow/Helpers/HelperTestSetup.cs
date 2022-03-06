using System;
using System.Threading;
using WebDriverWrapper;
using WebDriverWrapper.Utils;

namespace Tesco.Specflow.Helpers
{
    public class HelperTestSetup
    {
        public static void DoEnable_RunTestOnSchedule()
        {
            while (TimeSpan.Compare(DateTime.Now.TimeOfDay, SettingsSpecflow.Default.ScheduledTime) != 1)
                Thread.Sleep(TimeSpan.FromMinutes(1));
        }

        public WebDriverUtils LaunchWebDriver(bool useHeadless = false, bool seleniumGrid = false, string seleniumGridHub = null, string browser = "Chrome", string defaultDownloadFolder = null)
        {
            return new WebDriverUtils(new WebDriverFactory(defaultDownloadFolder).NewDriver(useHeadless, seleniumGrid, seleniumGridHub, browser));
        }

        public void SpecifyChromeIncognito(bool isOn)
        {
            WebDriverFactory.IsChromeIncognito = isOn;
        }
    }
}