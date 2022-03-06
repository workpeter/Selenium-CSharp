using System;

namespace TestResultBuilder
{
    public class TestResult
    {
        public string _uniqueID;

        public string UniqueID
        {
            get
            {
                if (string.IsNullOrEmpty(_uniqueID))
                    _uniqueID = $"{TestGroup}_{TestName}_{TestParameters}";

                return _uniqueID;
            }

            set => _uniqueID = value;
        }

        public string TestGroup { get; set; } = string.Empty;

        public string TestName { get; set; } = string.Empty;

        public string TestParameters { get; set; } = string.Empty;

        public string Result { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public string AdditionalInfo { get; set; } = string.Empty;

        public DateTime ExecutionStartTime { get; set; }

        public DateTime ExecutionEndTime { get; set; }

        public double ExecutionDurationSeconds { get; set; }
    }
}