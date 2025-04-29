using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Entities;
using SmartMenuBot.Core.Services;
using SmartMenuBot.Infrastructure.TelegramBot;

namespace SmartMenuBot.Core.Commands
{
    public class ShowTasksCommand : IBotCommand
    {
        public string CommandText => "/showtasks";

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

            var toDoServiceInstance = context.GetService<IToDoService>();
            ArgumentNullException.ThrowIfNull(toDoServiceInstance);

            int taskListCount = toDoServiceInstance.GetActiveCountByUserId(user.UserId);
            if (taskListCount == 0)
            {
                context.BotClient.SendMessage(context.Update.Message.Chat, $"\nСписок задач пуст");
                return;
            }

            var taskList = toDoServiceInstance.GetActiveByUserId(user.UserId);
            ShowTasks(context.BotClient, context.Update, taskList);
        }

        private static void ShowTasks(ITelegramBotClient botClient, Update update, IReadOnlyList<ToDoItem> taskList)
        {
            int i = 0;
            foreach (var item in taskList)
            {
                i++;
                botClient.SendMessage(update.Message.Chat, $"Задача #{i}: \"{item.Name}\" - {item.CreatedAt} - {item.Id}\n");
            }
        }
    }
}
