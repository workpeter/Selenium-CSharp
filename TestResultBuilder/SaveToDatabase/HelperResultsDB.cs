namespace TestResultBuilder
{
    public partial class HelperResults
    {
        public static void SetupDBResult(string sqlConnectionString, string resultsTableName)
        {
            //required to store test results
            TestResultDBConnect.CreateResultsTable(sqlConnectionString, resultsTableName);
        }

        public bool WriteTestResultToDB()
        {
            return TestResultDBConnect.AddTestRunResult(TestResultScenario);
        }
    }
}