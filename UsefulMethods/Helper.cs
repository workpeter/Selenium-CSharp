using System.IO;
using System.Reflection;

namespace UsefulMethods
{
    public class Helper
    {
        public Helper()
        {
        }

        public static string ExecutingDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}