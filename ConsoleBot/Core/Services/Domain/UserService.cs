using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMenuBot.Core.DataAccess;
using SmartMenuBot.Core.Entities;
using SmartMenuBot.Core.Services.Interfaces;

namespace SmartMenuBot.Core.Services.Domain
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public ToDoUser RegisterUser(long telegramUserId, string? telegramUserName)
        {
            var user = new ToDoUser(telegramUserId, telegramUserName);
            userRepository.Add(user);

            return user;
        }

        public ToDoUser? GetUser(long telegramUserId)
        {
            return userRepository.GetUserByTelegramUserId(telegramUserId);
        }
    }
}