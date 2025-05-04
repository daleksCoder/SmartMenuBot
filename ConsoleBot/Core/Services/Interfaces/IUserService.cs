using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMenuBot.Core.Entities;

namespace SmartMenuBot.Core.Services.Interfaces
{
    public interface IUserService
    {
        ToDoUser RegisterUser(long telegramUserId, string? telegramUserName);

        ToDoUser? GetUser(long telegramUserId);
    }
}