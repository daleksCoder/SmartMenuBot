using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenuBot.Core.Services
{
    public static class VersionInfoService
    {
        public static void GetVersionInfo(out string version, out DateTime creationDate)
        {
            version = "1.2.3";
            creationDate = new DateTime(2025, 3, 1);
        }
    }
}
