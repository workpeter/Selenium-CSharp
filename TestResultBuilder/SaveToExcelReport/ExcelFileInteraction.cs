using Microsoft.Office.Interop.Excel;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TestResultBuilder
{
    public class ExcelFileInteraction
    {
        public ExcelFileInteraction(string excelFilePath)
        {
            ExcelFilePath = excelFilePath;
        }

        public string ExcelFilePath { get; set; }

        private Application myExcelApplication;
        private Workbook myExcelWorkbook;
        private Worksheet myExcelWorkSheet;

        public void OpenWorkbook()
        {
            // create Excel App
            myExcelApplication = new Application
            {
                DisplayAlerts = false // turn off alerts
            };

            myExcelWorkbook = myExcelApplication.Workbooks._Open(ExcelFilePath, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value); // open the existing excel file
        }

        public void OpenWorkSheet(int workSheetNumber)
        {
            myExcelWorkSheet = (Worksheet)myExcelWorkbook.Worksheets[workSheetNumber];
        }

        public void WriteToCell<T>(char column, int row, T data, bool saveNowToFile)
        {
            WriteToCell(column.ToString(), row, data, saveNowToFile);
        }

        public void WriteToCell<T>(string column, int row, T data, bool saveNowToFile)
        {
            myExcelWorkSheet.Cells[row, column] = data;

            if (saveNowToFile)
                Save();
        }

        public void Save()
        {
            myExcelWorkbook.Save();
        }

        public void SaveAs(string saveAsFilepath)
        {
            myExcelWorkbook.SaveAs(saveAsFilepath, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                                  System.Reflection.Missing.Value, System.Reflection.Missing.Value, XlSaveAsAccessMode.xlNoChange,
                                                  System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                                  System.Reflection.Missing.Value, System.Reflection.Missing.Value);
        }

        public void Close()
        {
            try
            {
                myExcelWorkbook.Close(true, ExcelFilePath, System.Reflection.Missing.Value);
                myExcelApplication.Quit(); // close the excel application
            }
            finally
            {
                if (myExcelApplication != null)
                    myExcelApplication.Quit(); // close the excel application
            }
        }

        public static void CloseAnyOpenExcelWorkbooks()
        {
            //run this to avoid having issues where excel has the template already open which then makes it read only and so cannot write to it.

            try
            {
                Application excel = (Application)Marshal.GetActiveObject("Excel.Application");
                Workbooks wbs = excel.Workbooks;
                foreach (Workbook wb in wbs)
                {
                    Debug.WriteLine(wb.Name); // print the name of excel files that are open
                    wb.Save();
                    wb.Close();
                }
                excel.Quit();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error when trying to close down any open excel workboks: " + e.Message);
                //do nothing - this is not vital operation.
            }
        }
    }
}