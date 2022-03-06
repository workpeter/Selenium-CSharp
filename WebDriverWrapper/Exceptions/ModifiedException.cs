using System;

namespace WebDriverWrapper.Exceptions
{
    public class ModifiedException : Exception
    {
        public ModifiedException(string message, string stackTrace) : base(message)
        {
            StackTrace = stackTrace;
        }

        public override string StackTrace { get; }
    }
}