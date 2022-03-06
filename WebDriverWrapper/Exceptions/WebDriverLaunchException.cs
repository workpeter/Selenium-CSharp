using System;

namespace WebDriverWrapper.Exceptions
{
    public class WebDriverLaunchException : Exception
    {
        public WebDriverLaunchException(string message) : base(message)
        {
        }
    }
}