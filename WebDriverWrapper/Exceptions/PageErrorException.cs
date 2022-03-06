using System;

namespace WebDriverWrapper.Exceptions
{
    public class PageErrorException : Exception
    {
        public PageErrorException(string message) : base(message)
        {
        }
    }
}