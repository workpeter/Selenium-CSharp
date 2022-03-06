using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestResultBuilder
{
    public partial class HelperResults
    {
        public HelperResults()
        {
            TestResultScenario = new TestResult();
            ListStep = new List<string>();
            TestResultDBConnect = new TestResultDBConnect();
        }

        public TestResult TestResultScenario { get; set; }

        public List<string> ListStep { get; set; }

        public TestResultDBConnect TestResultDBConnect { get; set; }

        public void SetTestResultScenarioStartTime(DateTime startTime)
        {
            TestResultScenario.ExecutionStartTime = startTime;
        }

        public void SaveTestResult(string testName, string testParameters, string testGroup, string testResult, Exception testException, string additionalInfo)
        {
            TestResultScenario.TestName = testName;
            TestResultScenario.TestParameters = testParameters;
            TestResultScenario.TestGroup = testGroup;

            TestResultScenario.Result = testResult;

            if (testException != null)
                TestResultScenario.ErrorMessage = $"Error message: {testException.Message} " +
                                                  Environment.NewLine +
                                                  $"Exception type: {testException?.GetType()} " +
                                                  Environment.NewLine +
                                                  $"{testException.StackTrace}  " +
                                                  Environment.NewLine +
                                                  $"ScreenShots Location: {ErrorScreenShotLocation}  " +
                                                  Environment.NewLine;

            TestResultScenario.AdditionalInfo = $"Additional Info: {additionalInfo} " +
                                                Environment.NewLine +
                                                Environment.NewLine +
                                                $"Steps executed:" +
                                                Environment.NewLine +
                                                $"{string.Join($"{Environment.NewLine}", ListStep.ToArray())} " +
                                                Environment.NewLine;

            TestResultScenario.ExecutionEndTime = DateTime.Now;

            if (TestResultScenario.ExecutionStartTime != default)
                TestResultScenario.ExecutionDurationSeconds = (TestResultScenario.ExecutionEndTime - TestResultScenario.ExecutionStartTime).TotalSeconds;
            else
                Debug.WriteLine("Please ensure the TestResult execution start time is set in order to get execution duration.");

            AllTestResults.Add(TestResultScenario);
        }
    }
}