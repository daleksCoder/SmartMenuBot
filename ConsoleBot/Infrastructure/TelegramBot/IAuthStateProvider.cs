using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenuBot.Infrastructure.TelegramBot
{
    public interface IAuthStateProvider
    {
        public bool IsAuthorized { get; set; }
    }
}
