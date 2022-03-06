using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using WebDriverWrapper.Exceptions;
using WebDriverWrapper.Properties;
using WebDriverWrapper.Utils;

namespace WebDriverWrapper
{
    //================================================
    // Ensure the webdriver is periodically updated to keep inline with latest browser otherwise unusual errors may occur
    //================================================

    public class WebDriverFactory
    {
        public WebDriverFactory()
        { }

        public WebDriverFactory(string p_DefaultDownloadDirectory)
        {
            DefaultDownloadPath = p_DefaultDownloadDirectory;
        }

        public string NodeDetails { get; set; }

        public static bool IsChromeIncognito { get; set; } = true;

        public static bool IsDeleteAllCookies { get; set; } = true;

        private string DefaultDownloadPath { get; set; }

        private readonly Stopwatch stopWatchDriverLaunch = new Stopwatch();

        private IWebDriver driverToReturn;

        public IWebDriver NewDriver(bool p_blnUseHeadless = false, bool p_SeleniumGrid = false, string p_SeleniumGridHub = null, string p_Browser = "Chrome")
        {
            const int MaxRetries = 3;

            for (int i = 0; i < MaxRetries; i++)
            {
                try
                {
                    stopWatchDriverLaunch.Start();

                    if (string.Compare(p_Browser, "chrome", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        ChromeOptions chrOptions = new ChromeOptions();

                        //Page loading strategy
                        chrOptions.PageLoadStrategy = PageLoadStrategy.Normal;
                        //Set large aspect ratio, then maximise. Fail safe for when maximise doesnt work
                        //chrOptions.AddArgument("--window-size=1920,1080"); //Set to large size initially incase start-maximized fails to work
                        chrOptions.AddArgument("--start-maximized"); //https://stackoverflow.com/a/26283818/1689770
                        chrOptions.AddUserProfilePreference("intl.accept_languages", "nl");
                        chrOptions.AddUserProfilePreference("disable-popup-blocking", "true");

                        //Only log fatal errors to the console
                        chrOptions.AddArgument("--log-level=3");

                        ////Option to use custom Download directory
                        if (DefaultDownloadPath != null)
                            chrOptions.AddUserProfilePreference("download.default_directory", DefaultDownloadPath);

                        //A set of arguements to avoid weird and wonderful error that Chrome driver sometimes throws.
                        //https://stackoverflow.com/questions/48450594/selenium-timed-out-receiving-message-from-renderer
                        chrOptions.AddArgument("--disable-gpu");                        //https://stackoverflow.com/questions/51959986/how-to-solve-selenium-chromedriver-timed-out-receiving-message-from-renderer-exc
                        chrOptions.AddArgument("--no-sandbox");                         //https://stackoverflow.com/a/50725918/1689770
                        chrOptions.AddArgument("--disable-dev-shm-usage");              //https://stackoverflow.com/a/50725918/1689770
                        chrOptions.AddArgument("enable-automation");                    //https://stackoverflow.com/a/43840128/1689770
                        chrOptions.AddArgument("--disable-infobars");                   //https://stackoverflow.com/a/43840128/1689770
                        chrOptions.AddArgument("--disable-browser-side-navigation");    //https://stackoverflow.com/a/49123152/1689770
                        chrOptions.AddArgument("--disable-setuid-sandbox");             //https://qxf2.com/blog/chrome-not-reachable-error/

                        if (IsChromeIncognito)
                            chrOptions.AddArgument("--incognito");

                        //https://stackoverflow.com/questions/42778996/is-it-possible-to-interact-or-avoid-chromedriver-webdriver-save-password-alert
                        chrOptions.AddUserProfilePreference("credentials_enable_service", false);
                        chrOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
                        chrOptions.AddUserProfilePreference("profile.default_content_SettingsWebDriver.popups", 0);
                        chrOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
                        chrOptions.AddUserProfilePreference("download.prompt_for_download", false);

                        if (p_SeleniumGrid)
                        {
                            RemoteWebDriver remoteWebDriver = new RemoteWebDriver(new Uri(p_SeleniumGridHub), chrOptions.ToCapabilities(), TimeSpan.FromHours(1));

                            string sessionID = remoteWebDriver.SessionId.ToString();
                            string hubIpandPort = p_SeleniumGridHub.Replace("/wd/hub", "");

                            //remoteWebDriver.Url = $"{hubIpandPort}/grid/api/testsession?session={sessionID}";

                            //NodeDetails = remoteWebDriver.FindElement(By.XPath("//pre")).Text;

                            driverToReturn = remoteWebDriver;
                        }
                        else
                        {
                            if (p_blnUseHeadless)
                                chrOptions.AddArguments("--headless", "--start-maximized", "--disable-gpu", "--log-level=3", "--silent");

                            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                            service.HideCommandPromptWindow = true;
                            service.SuppressInitialDiagnosticInformation = true;
                            driverToReturn = new ChromeDriver(service, chrOptions);
                        }
                    }
                    else if (string.Compare(p_Browser, "firefox", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(DriverPath, "geckodriver.exe");
                        driverToReturn = new FirefoxDriver(service);
                    }
                    else if (string.Compare(p_Browser, "ie", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        driverToReturn = new InternetExplorerDriver(DriverPath);
                    }
                    else
                    {
                        throw new WebDriverLaunchException("Browser not recognised, check test SettingsWebDriver.");
                    }


                    driverToReturn.Manage().Window.Maximize();

                    //Set to same size as the nodes running as service user to see what they see when debugging. Unfortuently when running as a service (services.msc), screen desktop size cannot be maximised
                    if (SettingsWebDriver.Default.ForceBrowserWindowSize)
                        driverToReturn.Manage().Window.Size = new Size(SettingsWebDriver.Default.ForcedWindowSizeX, SettingsWebDriver.Default.ForcedWindowSizeY);

                    if (IsDeleteAllCookies)
                        driverToReturn.Manage().Cookies.DeleteAllCookies();

                    return driverToReturn;
                }
                catch (Exception ex)
                {
                    Exception = ex;

                    int sleepTime = 80000;

                    WebDriverUtils.DoOutput($"WebDriver Launch Failure. Waiting for {sleepTime}ms...Attempt {++i} {Environment.NewLine} {Environment.NewLine} Node Details: {NodeDetails}");

                    Thread.Sleep(sleepTime);
                }
                finally
                {
                    stopWatchDriverLaunch.Stop();

                    GetLaunchTime = Math.Round(stopWatchDriverLaunch.Elapsed.TotalSeconds);

                    //Was the launch delayed?
                    const int DELAYED_THRESHOLD = 60;
                    IsDelayedLaunch = (GetLaunchTime > DELAYED_THRESHOLD);

                    //if delayed launch is detected show the Node details
                    if (IsDelayedLaunch)
                    {
                        string p_output = $"Warning the webdriver took {GetLaunchTime} seconds to launch.  {Environment.NewLine} {Environment.NewLine} Node Details: {NodeDetails}";
                        WebDriverUtils.DoOutput(p_output, ConsoleColor.Yellow);
                    }
                }
            }

            throw new WebDriverLaunchException(Exception.GetBaseException().Message);
        }

        private Exception Exception;

        public static List<string> GetCommonDriverErrors()
        {
            List<string> listCommonDriverErrors = new List<string>
            {
                "java.net.ConnectException: Connection refused: connect",
                "FORWARDING_TO_NODE_FAILED",
                "cannot forward the request Address already in use",
                "BROWSER_TIMEOUT",
                "Unable to connect to the remote server",
                "session Error forwarding",
                "cannot forward the request Connection reset",
                "A exception with a null response was thrown sending an HTTP request to the remote WebDriver server",
                "The underlying connection was closed"
            };

            return listCommonDriverErrors;
        }

        public static List<GridConfigurationissues> CheckGridConfigurationIssues()
        {
            List<GridConfigurationissues> listCommonGridConfigurationissues = new List<GridConfigurationissues>
            {
                new GridConfigurationissues("No active session",
                    "This usually occurs when using Selenium Grid and trying to get the driver to do something (i.e. click)" +
                    "and the session has already been terminated.This might occur in a situation where the node session timeout is <= browser timeout." +
                    "There are some methods, which do UI checks for issues / errors after the timeout is reached, but if the session is already closed," +
                    "then the 'No active session' exception is thrown." +
                    "To avoid this issue ensure the node session timeout > browser timeout." +
                    "For example: If browser/ driver timeout is 60 seconds, set session timeout to 65 seconds on each node configuration.")
            };

            //Add more later if need be

            return listCommonGridConfigurationissues;
        }

        public string DriverPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public bool IsDelayedLaunch { get; private set; }

        public double GetLaunchTime { get; private set; }
    }

    public class GridConfigurationissues
    {
        public GridConfigurationissues(string p_Issue, string p_Solution)
        {
            Issue = p_Issue;
            Solution = p_Solution;
        }

        public string Issue { get; set; }
        public string Solution { get; set; }

        /*
        Some limits with Grid and how we have overcome.

        1) Chrome headless mode doesn't work with selenium grid. So headless is not used.

        2) Each node is limited to 5 chrome driver (some sort of built-in setting). If the number of tasks assigned to any given node is larger than its current capacity of 5
           chrome drivers (i.e. 10 tasks assigned), then those additional tasks will queue to launch a remote driver, whilst checking for timeout.
           If the timeout is reached, the launch fails and throws an exception. However, the driver launch request is still queued on the node.
           This ultimately leads idle chrome drivers launching, which prevent active chrome drivers from launching (due to the 5 concurrent driver limit) and
           if the problem persists then the node stops altogether from launching new drivers (even when existing drivers have been cleared down). Only way to solve is to restart hub.
           The solution is to avoid this scenario and have a long wait time (set during RemoteWebDriver call to 1 hour), patiently waiting until a node is available.
           The likelihood of long wait times are small if the tester doesn't overload the nodes with too many tasks. Ensure you have enough nodes to support the parallel tasks.
           I.e. 30 tasks require min 6 nodes (each running 5 browsers).

          If overloading/queueing issues occur, then an email notification will be triggered to let the tester know of an issue.

           I've also set the node config session limit to 5 to match the chrome driver limit. Without this set, the hub will assign a given node chrome drivers to launch requests,
           which it cant fulfil if its already running 5 chrome drivers, this will lead to queueing. It's better to have those requests go to another node, where it won't queue.
           5 is a good limit anyways, which allows for some CPU headroom, reducing the risk of client throughput issues.

        3) The nodes are configured for a session timeout setting of 60 seconds. So this means if the node and hub loss communication for all this time, then the chrome driver is released.
           Which frees up space for a new driver to be launched on the node.
        */
    }
}