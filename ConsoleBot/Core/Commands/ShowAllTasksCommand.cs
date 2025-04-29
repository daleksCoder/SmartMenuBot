using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Entities;
using SmartMenuBot.Core.Services;

namespace SmartMenuBot.Core.Commands
{
    public class ShowAllTasksCommand : IBotCommand
    {
        public string CommandText => "/showalltasks";

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

            int taskListCount = toDoServiceInstance.GetAllCountByUserId(user.UserId);
            if (taskListCount == 0)
            {
                context.BotClient.SendMessage(context.Update.Message.Chat, $"\nСписок задач пуст");
                return;
            }

            var taskList = toDoServiceInstance.GetAllByUserId(user.UserId);
            ShowTasks(context.BotClient, context.Update, taskList);
        }

        private static void ShowTasks(ITelegramBotClient botClient, Update update, IReadOnlyList<ToDoItem> taskList)
        {
            int i = 0;
            foreach (var item in taskList)
            {
                i++;
                botClient.SendMessage(update.Message.Chat, $"Задача #{i}: {item.State} \"{item.Name}\" - {item.CreatedAt} - {item.Id}\n");
            }
        }
    }
}
