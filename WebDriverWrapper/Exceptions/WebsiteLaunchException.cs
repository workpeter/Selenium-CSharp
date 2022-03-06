using System;

namespace WebDriverWrapper.Exceptions
{
    public class WebsiteLaunchException : Exception
    {
        public WebsiteLaunchException(string message) : base(message)
        {
        }
    }
}