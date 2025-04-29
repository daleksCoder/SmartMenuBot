using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Services;
using SmartMenuBot.Infrastructure.TelegramBot;

namespace SmartMenuBot.Core.Commands
{
    public class StartCommand : IBotCommand
    {
        public string CommandText => "/start";

        public bool CanExecute(CommandContext context)
        {
            string? messageText = context.Update.Message?.Text;
            return messageText != null && messageText.StartsWith(CommandText, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute(CommandContext context)
        {
            var userServiceInstance = context.GetService<IUserService>();
            ArgumentNullException.ThrowIfNull(userServiceInstance);

            if (context.IsUserAuthorized)
            {
                context.BotClient.SendMessage(context.Update.Message.Chat, $"\n\rВы уже авторизовались ранее");
                return;
            }

            var user = context.Update.Message.From;
            userServiceInstance.RegisterUser(user.Id, user.Username);
            context.IsUserAuthorized = true;

            context.BotClient.SendMessage(context.Update.Message.Chat, $"Добро пожаловать в систему управления обогревом загородного дома!\n");

            var toDoServiceInstance = context.GetService<IToDoService>();
            ArgumentNullException.ThrowIfNull(toDoServiceInstance);

            toDoServiceInstance.ToDoServiceInit(context.BotClient, context.Update);
        }
    }
}
