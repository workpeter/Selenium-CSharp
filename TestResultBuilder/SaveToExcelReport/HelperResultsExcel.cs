using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UsefulMethods;

namespace TestResultBuilder
{
    public partial class HelperResults
    {
        static HelperResults()
        {
            //this is static so that is serves as a global list for all running parallel tests to feed into
            AllTestResults = new List<TestResult>();
        }

        //keep results from all scenarios
        public static List<TestResult> AllTestResults { get; set; }

        public static char FirstResultColumn { get; set; }

        public static int FirstResultRow { get; set; }

        public static ExcelFileInteraction Excel { get; set; }

        public static void OpenExcelReportTemplate(char firstResultColumn, int firstResultRow)
        {
            //given that path not specified, the template is assumed to be in the same solution folder as this class.
            //if a custom path to the template needs to be specified then use the the other overloaded method which takes the path directory as a parameter

            ExcelFileInteraction.CloseAnyOpenExcelWorkbooks();

            string pathToReportTemplate = Path.Combine(Helper.ExecutingDirectory, @"SaveToExcelReport", "ReportTemplate.xlsx");

            Excel = new ExcelFileInteraction(pathToReportTemplate);
            Excel.OpenWorkbook();
            Excel.OpenWorkSheet(1);

            FirstResultColumn = firstResultColumn;
            FirstResultRow = firstResultRow;
        }

        public static void OpenExcelReportTemplate(string pathToReportTemplate, char firstResultColumn, int firstResultRow)
        {
            ExcelFileInteraction.CloseAnyOpenExcelWorkbooks();

            Excel = new ExcelFileInteraction(pathToReportTemplate);
            Excel.OpenWorkbook();
            Excel.OpenWorkSheet(1);

            FirstResultColumn = firstResultColumn;
            FirstResultRow = firstResultRow;
        }

        public static void CloseExcelReport()
        {
            IsExcelExist();

            Excel.Close();
        }

        public static void WriteToExcelReport()
        {
            IsExcelExist();

            char nextFreeResultColumn = FirstResultColumn;
            int nextFreeResultRow = FirstResultRow;

            foreach (var result in AllTestResults)
            {
                Excel.WriteToCell(nextFreeResultColumn++, nextFreeResultRow, result.TestGroup, false);
                Excel.WriteToCell(nextFreeResultColumn++, nextFreeResultRow, result.TestName, false);
                Excel.WriteToCell(nextFreeResultColumn++, nextFreeResultRow, result.TestParameters, false);
                Excel.WriteToCell(nextFreeResultColumn++, nextFreeResultRow, result.Result, false);
                Excel.WriteToCell(nextFreeResultColumn++, nextFreeResultRow, result.ErrorMessage, false);
                Excel.WriteToCell(nextFreeResultColumn++, nextFreeResultRow, result.AdditionalInfo, false);
                Excel.WriteToCell(nextFreeResultColumn++, nextFreeResultRow, result.ExecutionStartTime, false);
                Excel.WriteToCell(nextFreeResultColumn++, nextFreeResultRow, result.ExecutionEndTime, false);
                Excel.WriteToCell(nextFreeResultColumn, nextFreeResultRow, result.ExecutionDurationSeconds, false);

                nextFreeResultColumn = FirstResultColumn; //reset

                nextFreeResultRow++;
            }
        }

        public static void SaveExcelReportTo(string saveLocation)
        {
            IsExcelExist();

            Excel.SaveAs(saveLocation);
        }

        public static bool IsExcelExist()
        {
            if (Excel == null)
            {
                Debug.WriteLine("Excel object not created");
                return false;
            }

            return true;
        }
    }
}