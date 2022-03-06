using System;

namespace WebDriverWrapper.Exceptions
{
    public class DataInputValidationException : Exception
    {
        public DataInputValidationException(string message) : base(message)
        {
        }
    }
}