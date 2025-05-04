using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenuBot.Core.Services.Domain
{
    public static class VersionInfoService
    {
        public static void GetVersionInfo(out string version, out DateTime creationDate)
        {
            version = "2.0.1";
            creationDate = new DateTime(2025, 5, 1);
        }
    }
}