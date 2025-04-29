using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMenuBot.Core.Entities;

namespace SmartMenuBot.Core.Services
{
    public class UserService : IUserService
    {
        private readonly List<ToDoUser> _users = [];

        public ToDoUser RegisterUser(long telegramUserId, string? telegramUserName)
        {
            var user = new ToDoUser(telegramUserId, telegramUserName);
            _users.Add(user);
            return user;
        }

        public ToDoUser? GetUser(long telegramUserId)
        {
            return _users.FirstOrDefault(u => u.TelegramUserId == telegramUserId);
        }
    }
}
