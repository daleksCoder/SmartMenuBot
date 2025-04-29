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
    public class AddTaskCommand : IBotCommand
    {
        public string CommandText => "/addtask";

        public bool CanExecute(CommandContext context)
        {
            string? messageText = context.Update.Message?.Text;
            return messageText != null && messageText.StartsWith(CommandText, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute(CommandContext context)
        {
            if (!context.IsUserAuthorized)
            {
                context.BotClient.SendMessage(context.Update.Message.Chat, $"\n\rПользователь не зарегистрирован, команда недоступна");
                return;
            }

            var userServiceInstance = context.GetService<IUserService>();
            ArgumentNullException.ThrowIfNull(userServiceInstance);

            var user = userServiceInstance.GetUser(context.Update.Message.From.Id);
            if (user == null)
            {
                context.BotClient.SendMessage(context.Update.Message.Chat, $"\n\rПользователь не найден, команда недоступна");
                return;
            }

            var taskName = context.Update.Message.Text.Replace(CommandText, "", StringComparison.OrdinalIgnoreCase).Trim();
            if (string.IsNullOrEmpty(taskName))
            {
                context.BotClient.SendMessage(context.Update.Message.Chat, $"\nНаименование задачи не может быть пустым");
                return;
            }

            var toDoServiceInstance = context.GetService<IToDoService>();
            ArgumentNullException.ThrowIfNull(toDoServiceInstance);

            var item = toDoServiceInstance.Add(user, taskName);

            string addInfo = $"Добавлена задача: \"{item.Name}\" - {item.CreatedAt} - {item.Id}\n";
            context.BotClient.SendMessage(context.Update.Message.Chat, addInfo);
        }
    }
}
