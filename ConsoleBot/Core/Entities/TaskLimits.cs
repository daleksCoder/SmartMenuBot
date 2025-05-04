using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenuBot.Core.Entities
{
    public sealed record TaskLimits(int Count, int Length)
    {
        public static TaskLimits Uninitialized { get; } = new(Count: 0, Length: 0);

        public bool IsInitialized => Count > 0 && Length > 0;
    }
}