//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tesco.Specflow {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class SettingsSpecflow : global::System.Configuration.ApplicationSettingsBase {
        
        private static SettingsSpecflow defaultInstance = ((SettingsSpecflow)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new SettingsSpecflow())));
        
        public static SettingsSpecflow Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("TescoRunNumber_1")]
        public string ResultsTableName {
            get {
                return ((string)(this["ResultsTableName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Headless {
            get {
                return ((bool)(this["Headless"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool UseSeleniumGrid {
            get {
                return ((bool)(this["UseSeleniumGrid"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://192.168.1.190:2222/wd/hub")]
        public string SeleniumGridHub {
            get {
                return ((string)(this["SeleniumGridHub"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("chrome")]
        public string Browser {
            get {
                return ((string)(this["Browser"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool IsScheduledTest {
            get {
                return ((bool)(this["IsScheduledTest"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("19:00:00")]
        public global::System.TimeSpan ScheduledTime {
            get {
                return ((global::System.TimeSpan)(this["ScheduledTime"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\LogsFolder")]
        public string LogsBaseFolder {
            get {
                return ((string)(this["LogsBaseFolder"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\LogsFolder\\ExcelReport")]
        public string SaveLocatioExcelReport {
            get {
                return ((string)(this["SaveLocatioExcelReport"]));
            }
            set {
                this["SaveLocatioExcelReport"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server=localhost\\SQLEXPRESS02;Database=Results;Trusted_Connection=True;")]
        public string ResultsConnectionString {
            get {
                return ((string)(this["ResultsConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://www.tesco.com")]
        public string TestDomain {
            get {
                return ((string)(this["TestDomain"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Pass@word456")]
        public string GenericAccountPassword {
            get {
                return ((string)(this["GenericAccountPassword"]));
            }
            set {
                this["GenericAccountPassword"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("//*[contains(text(),\'but something went wrong\')]")]
        public string DetectGenericErrorObjectXpath {
            get {
                return ((string)(this["DetectGenericErrorObjectXpath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool RunFullTidyUp {
            get {
                return ((bool)(this["RunFullTidyUp"]));
            }
        }
    }
}
