using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Services;
using SmartMenuBot.Infrastructure.TelegramBot;

namespace SmartMenuBot.Core.Commands
{
    public class CommandContext(IAuthStateProvider authProvider, ITelegramBotClient botClient, Update update, List<IBotServiceProvider> services)
    {
        private readonly IAuthStateProvider _authProvider = authProvider;
        public ITelegramBotClient BotClient { get; } = botClient;
        public Update Update { get; } = update;
        public List<IBotServiceProvider> Services { get; } = services;
        public bool IsUserAuthorized
        {
            get => _authProvider.IsAuthorized;
            set => _authProvider.IsAuthorized = value;
        }

        public T? GetService<T>() where T : IBotServiceProvider
        {
            return (T?)Services.FirstOrDefault(s => s is T);
        }
    }
}
