using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using UsefulMethods.ExtensionMethods;

namespace TestResultBuilder
{
    public class TestResultDBConnect
    {
        public static bool IsTestResultsDBExist { get; set; }

        public static bool IsTestResultsTableExist { get; set; }

        //useful for debugging/reporting
        public static Exception CapturedException { get; set; }

        public static void CreateResultsTable(string sqlConnectionString, string resultsTableName)
        {
            SqlConnectionString = sqlConnectionString;
            TableName = resultsTableName;

            //if the database doesnt exist as per the connection string, then create it.
            IsTestResultsDBExist = CheckDatabaseExists(sqlConnectionString);

            if (IsTestResultsDBExist == false)
            {
                MessageBox.Show(
                    $"The result DB does not exist. Please create the database before running the test suite if you want to save the results long-term. {Environment.NewLine}" +
                    $"Connection string: {sqlConnectionString}");

                return;
            }

            //if no table name given for the test results, then use todays date/time
            if (string.IsNullOrEmpty(resultsTableName))
            {
                IsTestResultsTableExist = CreateResultsTable_WithDateTimeStamp();
                return;
            }

            IsTestResultsTableExist = CheckExistsTableResult(resultsTableName);

            if (IsTestResultsTableExist)
            {
                DialogResult dialogResult = MessageBox.Show($"Result Table [{resultsTableName}] aleady exists.{Environment.NewLine}{Environment.NewLine}" +
                                                            $"[Yes] to Amend results in existing table.{Environment.NewLine}" +
                                                            $"[No] To save results in a randomly named table based on date timestamp.",
                                                            $"Warning",
                                                            MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    IsTestResultsTableExist = CreateResultsTable_WithDateTimeStamp();
                    return;
                }
            }

            IsTestResultsTableExist = DoCreateResultsTable(resultsTableName);

            //private inner method
            bool CreateResultsTable_WithDateTimeStamp()
            {
                TableName = $"TestRun_{DateTime.Now:dd_MM_yyyy__HH_mm}";

                return DoCreateResultsTable(TableName);
            }
        }

        public static string HostinUse { get; private set; }
        public static string SqlConnectionString { get; set; }

        public static string TableName { get; private set; }

        private static readonly object lockObj = new object();

        private const int MaxRetries = 5;

        public static bool CheckDatabaseExists(string sqlConnectionString)
        {
            var connection = new SqlConnection(sqlConnectionString);

            bool result;

            try
            {
                string sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", connection.Database);
                using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, connection))
                {
                    connection.Open();
                    object resultObj = sqlCmd.ExecuteScalar();
                    int databaseID = 0;
                    if (resultObj != null)
                    {
                        int.TryParse(resultObj.ToString(), out databaseID);
                    }
                    connection.Close();
                    result = (databaseID > 0);
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public static bool CheckExistsTableResult(string p_ResultsTableName)
        {
            lock (lockObj)
            {
                //if it doesnt exist, build table.
                string query = $@"
                                SELECT
                                  CASE WHEN EXISTS
                                  (
                                        SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{p_ResultsTableName}'
                                  )
                                  THEN 1
                                  ELSE 0
                               END
                                ";

                for (int i = 0; i < MaxRetries; i++)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(SqlConnectionString))
                        {
                            conn.Open();

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                return (int)cmd.ExecuteScalar() == 1;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(2500);

                        CapturedException = e;
                    }
                }
            }

            return false;
        }

        private static bool DoCreateResultsTable(string p_ResultsTableName)
        {
            lock (lockObj)
            {
                if (string.IsNullOrEmpty(p_ResultsTableName))
                    throw new Exception("table name provided is blank");

                //if it doesnt exist, build table.
                string query = $@"
                                BEGIN
	                                IF NOT EXISTS (SELECT * FROM sys.objects
		                                WHERE object_id = OBJECT_ID(N'[dbo].[{p_ResultsTableName}]')
		                                AND type in (N'U'))
	                                        BEGIN
		                                        CREATE TABLE {p_ResultsTableName}(
                                                    TestGroup VARCHAR(max),
                                                    UniqueID VARCHAR(max),
			                                        TestName VARCHAR(max),
										            TestParameters VARCHAR(max),
										            Result VARCHAR(max),
										            ErrorMessage VARCHAR(max),
										            AdditionalInfo VARCHAR(max),
										            ExecutionStartTime DATETIME,
                                                    ExecutionEndTime DATETIME,
                                                    ExecutionDurationSeconds Float
		                                    )
	                                 END
                                END
                                ";

                for (int i = 0; i < MaxRetries; i++)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(SqlConnectionString))
                        {
                            conn.Open();

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.ExecuteScalar();
                                return true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(2500);

                        CapturedException = e;
                    }
                }
            }

            return false;
        }

        public static List<TestResult> GetFullTestResult()
        {
            lock (lockObj)
            {
                string query = $@"select * from {TableName}";

                for (int i = 0; i < MaxRetries; i++)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(SqlConnectionString))
                        {
                            conn.Open();

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                var records = new List<TestResult>();

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var result = new TestResult
                                        {
                                            TestGroup = reader.SafeGetString(0),
                                            UniqueID = reader.SafeGetString(1),
                                            TestName = reader.SafeGetString(2),
                                            TestParameters = reader.SafeGetString(3),
                                            Result = reader.SafeGetString(4),
                                            ErrorMessage = reader.SafeGetString(5),
                                            AdditionalInfo = reader.SafeGetString(6),
                                            ExecutionStartTime = reader.SafeGetDate(7),
                                            ExecutionEndTime = reader.SafeGetDate(8),
                                            ExecutionDurationSeconds = reader.SafeGetDouble(9)
                                        };

                                        records.Add(result);
                                    }
                                }

                                return records;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(2500);

                        CapturedException = e;
                    }
                }

                return null;
            }
        }

        public bool AddTestRunResult(TestResult p_TestResult)
        {
            if (IsTestResultsDBExist == false || IsTestResultsTableExist == false)
            {
                Debug.Write("Unable to write results due to database: ");

                if (IsTestResultsDBExist == false)
                    Debug.WriteLine($"Database doesn't exist using connection string:{SqlConnectionString}");

                if (IsTestResultsDBExist == false)
                    Debug.WriteLine($"Database table doesn't exist using table name:{TableName}");

                return false;
            }

            //Make dates write safe to the SQL server.
            //The default DateTime value (0001-01-01) used by C# will throw exception in SQL server. The smallest date SQL server will except is 1753
            if (p_TestResult.ExecutionStartTime == default)
                p_TestResult.ExecutionStartTime = new DateTime(1753, 1, 1);

            if (p_TestResult.ExecutionEndTime == default)
                p_TestResult.ExecutionEndTime = new DateTime(1753, 1, 1);

            lock (lockObj)
            {
                string queryIsExist = $@"select count(1)
                            from {TableName}
                            where UniqueID = @UniqueID";

                string queryUpdate = $@"update {TableName}
                                    set
	                                Result = @Result,
	                                ErrorMessage = @ErrorMessage,
	                                AdditionalInfo = @AdditionalInfo,
	                                ExecutionStartTime = @ExecutionStartTime,
	                                ExecutionEndTime = @ExecutionEndTime,
	                                ExecutionDurationSeconds = @ExecutionDurationSeconds
                                    where UniqueID = @UniqueID";

                string queryInsert = $@"insert into {TableName}
                            values (@TestGroup,@UniqueID,@TestName,@TestParameters,@Result,@ErrorMessage,@AdditionalInfo,@ExecutionStartTime,@ExecutionEndTime,@ExecutionDurationSeconds)";

                for (int i = 0; i < MaxRetries; i++)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(SqlConnectionString))
                        {
                            conn.Open();

                            //if this is true, then we are already pointing to an existing test table
                            bool isExist;

                            //Check to see if the specific test result already exists.
                            //if it exists, update it, otherwise insert a new record
                            using (SqlCommand cmd = new SqlCommand(queryIsExist, conn))
                            {
                                //check if exists
                                AddAllParams(cmd);
                                isExist = (int)cmd.ExecuteScalar() == 1;
                            }

                            if (isExist)
                            {
                                //do update
                                using (SqlCommand cmd = new SqlCommand(queryUpdate, conn))
                                {
                                    AddAllParams(cmd);
                                    cmd.ExecuteScalar();
                                }
                            }
                            else
                                DoInsert();

                            void AddAllParams(SqlCommand cmd)
                            {
                                var sqlParam0 = new SqlParameter("TestGroup", SqlDbType.VarChar)
                                {
                                    Value = p_TestResult.TestGroup ?? string.Empty
                                };
                                var sqlParam1 = new SqlParameter("UniqueID", SqlDbType.VarChar)
                                {
                                    Value = p_TestResult.UniqueID ?? string.Empty
                                };
                                var sqlParam2 = new SqlParameter("TestName", SqlDbType.VarChar)
                                {
                                    Value = p_TestResult.TestName ?? string.Empty
                                };
                                var sqlParam3 = new SqlParameter("TestParameters", SqlDbType.VarChar)
                                {
                                    Value = p_TestResult.TestParameters ?? string.Empty
                                };
                                var sqlParam4 = new SqlParameter("Result", SqlDbType.VarChar)
                                {
                                    Value = p_TestResult.Result ?? string.Empty
                                };
                                var sqlParam5 = new SqlParameter("ErrorMessage", SqlDbType.VarChar)
                                {
                                    Value = p_TestResult.ErrorMessage.Replace("~~~", " \n") ?? string.Empty
                                };
                                var sqlParam6 = new SqlParameter("AdditionalInfo", SqlDbType.VarChar)
                                {
                                    Value = p_TestResult.AdditionalInfo ?? string.Empty
                                };
                                var sqlParam7 = new SqlParameter("ExecutionStartTime", SqlDbType.DateTime)
                                {
                                    Value = p_TestResult.ExecutionStartTime
                                };
                                var sqlParam8 = new SqlParameter("ExecutionEndTime", SqlDbType.DateTime)
                                {
                                    Value = p_TestResult.ExecutionEndTime
                                };
                                var sqlParam9 = new SqlParameter("ExecutionDurationSeconds", SqlDbType.Float)
                                {
                                    Value = p_TestResult.ExecutionDurationSeconds
                                };

                                cmd.Parameters.Add(sqlParam0);
                                cmd.Parameters.Add(sqlParam1);
                                cmd.Parameters.Add(sqlParam2);
                                cmd.Parameters.Add(sqlParam3);
                                cmd.Parameters.Add(sqlParam4);
                                cmd.Parameters.Add(sqlParam5);
                                cmd.Parameters.Add(sqlParam6);
                                cmd.Parameters.Add(sqlParam7);
                                cmd.Parameters.Add(sqlParam8);
                                cmd.Parameters.Add(sqlParam9);
                            }

                            void DoInsert()
                            {
                                using (SqlCommand cmd = new SqlCommand(queryInsert, conn))
                                {
                                    AddAllParams(cmd);
                                    cmd.ExecuteScalar();
                                }
                            }
                        }

                        return true;
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(2500);

                        CapturedException = e;
                    }
                }
            }

            return false;
        }
    }
}