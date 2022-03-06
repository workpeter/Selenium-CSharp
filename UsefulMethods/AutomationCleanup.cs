using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;

namespace UsefulMethods
{
    public static class AutomationCleanup
    {
        private const string PROCESS_CHROME = "chrome";

        private static string GetCommandLine(this Process process)
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
                using (ManagementObjectCollection objects = searcher.Get())
                {
                    return objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
                }
            }
            catch (Win32Exception ex) when ((uint)ex.ErrorCode == 0x80004005)
            {
                // Intentionally empty - no security access to the process.
                return string.Empty;
            }
            catch (InvalidOperationException)
            {
                // Intentionally empty - the process exited before getting details.
                return string.Empty;
            }
        }

        public static void TidyUp(bool DoFullTidyUp = true)
        {
            try
            {
                //  Tidy up any previous remaining things

                foreach (string processName in new[] { "iexplore", "firefox", "geckodriver", "chromedriver" })
                {
                    foreach (Process proc in Process.GetProcessesByName(processName))
                    {
                        try
                        {
                            proc.Kill();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }

                if (DoFullTidyUp == false)
                    return;

                //Thread.Sleep(1000);
                string strAppDataTempDir = Environment.ExpandEnvironmentVariables(Path.Combine("%LocalAppData%", "Temp"));
                string[] arrFilesToDelete = Directory.GetFiles(strAppDataTempDir, "tmpaddon*");
                string[] arrFoldersToDelete = Directory.GetDirectories(strAppDataTempDir, "rust_mozprofile*");

                foreach (string strFileNameToDelete in arrFilesToDelete)
                {
                    try
                    {
                        File.Delete(strFileNameToDelete);
                    }
                    catch
                    {
                        // ignored
                    }
                }

                foreach (string strFolderNameToDelete in arrFoldersToDelete)
                {
                    try
                    {
                        Directory.Delete(strFolderNameToDelete, true);
                    }
                    catch
                    {
                        // ignored
                    }
                }

                // delete all "scoped_dir" temp folders
                string tempfolder = Path.GetTempPath();
                string[] tempfiles = Directory.GetDirectories(tempfolder, "scoped_dir*", SearchOption.AllDirectories);
                foreach (string tempfile in tempfiles)
                {
                    try
                    {
                        DirectoryInfo directory = new DirectoryInfo(tempfile);
                        foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}