using System;
using System.Diagnostics;
using System.IO;

namespace TestResultBuilder
{
    public partial class HelperResults
    {
        public static string LogsBaseFolder { get; private set; }

        public static string ScenarioLogFolder { get; private set; }

        public string ScenarioScreenshotFolder { get; set; } = string.Empty;
        public string ErrorScreenShotLocation { get; set; } = string.Empty;

        public static string DefaultDownloadFolder { get; private set; }

        public static void CreateLogsBaseFolder(string logsBaseFolderDir)
        {
            //folder for storing log of results

            LogsBaseFolder = logsBaseFolderDir;

            if (!Directory.Exists(LogsBaseFolder))
                Directory.CreateDirectory(LogsBaseFolder);
        }

        public static void CreateDefaultDownloadFolder(string dir)
        {
            DefaultDownloadFolder = Path.Combine(dir, @"Download");

            if (!Directory.Exists(DefaultDownloadFolder))
                Directory.CreateDirectory(DefaultDownloadFolder);
        }

        public string CreateTestScenarioLogFolder(string testTitle)
        {
            testTitle = testTitle.Replace(' ', '_');

            ScenarioLogFolder = Path.Combine(LogsBaseFolder, $"{testTitle}_" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"));

            if (!Directory.Exists(ScenarioLogFolder))
                Directory.CreateDirectory(ScenarioLogFolder);

            return ScenarioLogFolder;
        }

        public string CreateTestSpecificScreenshotfolder(string scenarioLogFolder)
        {
            //Setup screenshot folder for this this scenario
            string scenarioScreenshotFolder = Path.Combine(scenarioLogFolder, @"screenshot");

            if (!Directory.Exists(scenarioScreenshotFolder))
                Directory.CreateDirectory(scenarioScreenshotFolder);

            return scenarioScreenshotFolder;
        }

        public string CreateScreenshotDirectoryAndDesiredScreenshotFilename()
        {
            try
            {
                ScenarioScreenshotFolder = CreateTestSpecificScreenshotfolder(ScenarioLogFolder);
                ErrorScreenShotLocation = Path.Combine(ScenarioScreenshotFolder, $"{Guid.NewGuid()}.png");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return ErrorScreenShotLocation;
        }

        public void GenerateTestLog()
        {
            string logFile = Path.Combine(ScenarioLogFolder, @"log.txt");

            string log =
                        $"Test name: {TestResultScenario.TestName}" +
                        Environment.NewLine +
                        $"Test parameters: {TestResultScenario.TestParameters}" +
                        Environment.NewLine +
                        $"Test group: {TestResultScenario.TestGroup}" +
                        Environment.NewLine +
                        $"Result: {TestResultScenario.Result}" +
                        Environment.NewLine +
                        $"Execution start time: {TestResultScenario.ExecutionStartTime}" +
                        Environment.NewLine +
                        $"Execution end time: {TestResultScenario.ExecutionEndTime}" +
                        Environment.NewLine +
                        $"Execution duration (secs): {TestResultScenario.ExecutionDurationSeconds}" +
                        Environment.NewLine +
                        Environment.NewLine +
                        TestResultScenario.ErrorMessage +
                        Environment.NewLine +
                        TestResultScenario.AdditionalInfo;

            File.WriteAllText(logFile, log);

            Debug.WriteLine(log);
        }
    }
}