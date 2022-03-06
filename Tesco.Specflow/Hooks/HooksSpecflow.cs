using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using TechTalk.SpecFlow;
using Tesco.Specflow.Base;
using Tesco.Specflow.Helpers;
using TestResultBuilder;
using UsefulMethods;
using WebDriverWrapper.Exceptions;
using WebDriverWrapper.Utils;

namespace Tesco.Specflow.Hooks
{
    [Binding]
    public class HooksSpecflow : ScenarioBase
    {
        public HooksSpecflow(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            HelperTestSetup = new HelperTestSetup();
            HelperResults = new HelperResults();
        }

        public static Stopwatch TestRunTime { get; set; } = new Stopwatch();

        private HelperTestSetup HelperTestSetup { get; }
        private HelperResults HelperResults { get; }

        private static string LogsBaseFolder { get; set; }

        //Change the Xpath to detect whatever DOM object always appears when the website under test fails. i.e. a generic error box/message.
        //The automation will scan for this error object after every step call.
        private By GenericErrorObjectAnyPage { get; } = By.XPath(SettingsSpecflow.Default.DetectGenericErrorObjectXpath);

        //========================================
        //Actions Before and After the entire SpecFlow solution runs
        //========================================

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            //clean up processes and temp log files before test runs
            AutomationCleanup.TidyUp(SettingsSpecflow.Default.RunFullTidyUp);

            //create a folder directory to store logs/screenshots
            LogsBaseFolder = Path.Combine(@SettingsSpecflow.Default.LogsBaseFolder, DateTime.Now.Date.ToString("dd-MM-yyyy h-mm tt"));
            HelperResults.CreateLogsBaseFolder(LogsBaseFolder);
            HelperResults.CreateDefaultDownloadFolder(LogsBaseFolder);

            //setup a DB table to store our specflow test results into
            HelperResults.SetupDBResult(SettingsSpecflow.Default.ResultsConnectionString, SettingsSpecflow.Default.ResultsTableName);

            //if we want to run the test in the future, set a specific time, and the execution will hold here until that time of the day is reached, then it will continue.
            if (SettingsSpecflow.Default.IsScheduledTest)
                HelperTestSetup.DoEnable_RunTestOnSchedule();

            //sometimes a web-server which hasn't received traffic for a while will be slow on first request. So lets send that first request now, so our testing is impacted
            WebDriverUtils.WakeupServer(SettingsSpecflow.Default.TestDomain, 5);

            TestRunTime.Start();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            WriteAllTestResultsToExcelReport();
            AutomationCleanup.TidyUp(SettingsSpecflow.Default.RunFullTidyUp);
        }

        private static void WriteAllTestResultsToExcelReport()
        {
            try
            {
                //When specflow has finished running, loop through all results and output to excel template
                HelperResults.OpenExcelReportTemplate('B', 13); //template location not specified, so will use default template location found within the solution. overload method if specific template location needs to be supplied.
                HelperResults.WriteToExcelReport();
            }
            finally
            {
                try
                {
                    string excelReportSavelocation = Path.Combine(LogsBaseFolder, $"Results_{DateTime.Now.ToString("dd-MM-yyyy h-mm tt")}.xlsx");
                    HelperResults.SaveExcelReportTo(excelReportSavelocation);
                }
                finally
                {
                    HelperResults.CloseExcelReport();
                }
            }
        }

        private static readonly object LoadDistributionLock = new object();

        //========================================
        //Actions Before and After each SpecFlow Scenario
        //========================================

        [Before]
        public void BeforeTest()
        {
            //Setup the Scenario object which will contain all the dynamic scenario data. This will be shared as a ScenarioContext key
            Scenario = new Scenario
            {
                TestDomain = SettingsSpecflow.Default.TestDomain,
                DefaultDownloadDirectory = HelperResults.DefaultDownloadFolder
            };

            //Launch web driver and create web driver wrapper object (Utils)
            Utils = HelperTestSetup.LaunchWebDriver(
                SettingsSpecflow.Default.Headless,
                SettingsSpecflow.Default.UseSeleniumGrid,
                SettingsSpecflow.Default.SeleniumGridHub,
                SettingsSpecflow.Default.Browser,
                HelperResults.DefaultDownloadFolder);

            //TODO setup a DB connection for querying object data
            //DatabaseConnect = new DatabaseConnect(Settings.Default.Env);

            HelperResults.SetTestResultScenarioStartTime(DateTime.Now);

            //Helps distributes the initial load on the application, so all threads arent hitting same services at same time.
            lock (LoadDistributionLock) { Thread.Sleep(5000); }
        }

        [After]
        public void After()
        {
            try
            {
                //----- Save screenshot if error ------------------

                HelperResults.CreateTestScenarioLogFolder(ScenarioContext.ScenarioInfo.Title);

                if (ScenarioContext.TestError != null)
                {
                    string desiredScreenshotLocation = HelperResults.CreateScreenshotDirectoryAndDesiredScreenshotFilename();
                    Utils.TakeScreenshot(desiredScreenshotLocation);
                }

                //----- Save test results -------------------------

                HelperResults.SaveTestResult(ScenarioContext.ScenarioInfo.Title,
                                             string.Join(",", TestContext.CurrentContext.Test.Arguments),
                                             string.Join(",", ScenarioContext.ScenarioInfo.Tags),
                                             TestContext.CurrentContext.Result.Outcome.Status.ToString(),
                                             ScenarioContext.TestError,
                                             Scenario.AdHocLogInfo);

                //----- generate test logs -------------------------

                HelperResults.GenerateTestLog();                                //write result to file and output to debug window
                bool isWriteSuccess = HelperResults.WriteTestResultToDB();      //write result to database (if available)

                if (isWriteSuccess == false)
                    Debug.WriteLine($"Result was not written to the DB: {HelperResults.TestResultScenario.UniqueID}");

                //with this the stack trace becomes clickable, making it much easier to debug.
                if (ScenarioContext.TestError != null && TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                    throw new ModifiedException("Scenario failure", HelperResults.TestResultScenario.ErrorMessage);
            }
            finally
            {
                if (Utils.Driver != null)
                    Utils.Driver.Quit();
            }
        }

        //========================================
        //Actions Before and After each SpecFlow Step
        //========================================

        [BeforeStep]
        public void BeforeStep()
        {
            //record the step names that triggered. If the scenario failed, then we have a log of the last step executed.
            HelperResults.ListStep.Add($"-> {ScenarioContext.StepContext.StepInfo.Text}");
        }

        [AfterStep]
        public void AfterStep()
        {
            Utils.CheckPageError(GenericErrorObjectAnyPage);
        }
    }
}