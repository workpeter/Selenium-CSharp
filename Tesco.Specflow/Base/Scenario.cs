using System.Collections.Generic;
using Tesco.Framework.Enums;
using Tesco.Framework.Models;

namespace Tesco.Specflow.Base
{
    public class Scenario
    {
        public string TestDomain { get; set; }

        public string DefaultDownloadDirectory { get; set; }

        public Account Account { get; set; }

        public UserType UserType { get; set; }

        public string AdHocLogInfo { get; set; }

        public IDictionary<string, decimal> SelectedProducts { get; set; }
    }
}