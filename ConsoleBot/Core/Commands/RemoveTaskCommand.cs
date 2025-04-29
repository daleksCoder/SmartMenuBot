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
    public class RemoveTaskCommand : IBotCommand
    {
        public string CommandText => "/removetask";

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

            var taskStrNumber = context.Update.Message.Text.Replace(CommandText, "", StringComparison.OrdinalIgnoreCase).Trim();
            if (string.IsNullOrEmpty(taskStrNumber))
            {
                context.BotClient.SendMessage(context.Update.Message.Chat, $"\nНомер задачи не может быть пустым");
                return;
            }

            if (!int.TryParse(taskStrNumber, out int taskNumber))
            {
                context.BotClient.SendMessage(context.Update.Message.Chat, $"\nНомер задачи должен быть числом в диапазоне номеров задач!");
                return;
            }

            if (taskNumber < 1 || taskNumber > taskListCount)
            {
                context.BotClient.SendMessage(context.Update.Message.Chat, $"\nНомер задачи должен быть в допустимом диапазоне номеров задач!");
                return;
            }

            var taskList = toDoServiceInstance.GetAllByUserId(user.UserId);
            var item = taskList[taskNumber-1];

            string delInfo = $"Удалено: #{taskNumber}: \"{item.Name}\" - {item.CreatedAt} - {item.Id}\n";
            toDoServiceInstance.Delete(item.Id);
            context.BotClient.SendMessage(context.Update.Message.Chat, delInfo);
        }
    }
}
