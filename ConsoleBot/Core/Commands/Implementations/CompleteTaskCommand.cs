using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Commands.Interfaces;
using SmartMenuBot.Core.Services.Interfaces;

namespace SmartMenuBot.Core.Commands.Implementations
{
    public class CompleteTaskCommand(ITelegramBotClient botClient, IUserService userService, IToDoService toDoService) : IBotCommand
    {
        public string CommandText => "/completetask";

        private ITelegramBotClient BotClient { get; } = botClient;

        private readonly IUserService _userServiceInstance = userService;

        private readonly IToDoService _toDoServiceInstance = toDoService;

        public bool CanExecute(CommandContext context)
        {
            string? messageText = context.Update.Message?.Text;
            return messageText != null && messageText.StartsWith(CommandText, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute(CommandContext context)
        {
            var existingUser = _userServiceInstance.GetUser(context.Update.Message.From.Id);
            if (existingUser == null)
            {
                BotClient.SendMessage(
                    context.Update.Message.Chat,
                    "\nДля использования этой функции необходимо авторизоваться командой /start"
                );
                return;
            }

            int taskListCount = _toDoServiceInstance.GetActiveByUserId(existingUser.UserId).Count;
            if (taskListCount == 0)
            {
                BotClient.SendMessage(context.Update.Message.Chat, $"\nСписок задач пуст");
                return;
            }

            var taskStrNumber = context.Update.Message.Text.Replace(CommandText, "", StringComparison.OrdinalIgnoreCase).Trim();
            if (string.IsNullOrEmpty(taskStrNumber))
            {
                BotClient.SendMessage(context.Update.Message.Chat, $"\nНомер задачи не может быть пустым");
                return;
            }

            if (!int.TryParse(taskStrNumber, out int taskNumber))
            {
                BotClient.SendMessage(context.Update.Message.Chat, $"\nНомер задачи должен быть числом в диапазоне номеров задач!");
                return;
            }

            if (taskNumber < 1 || taskNumber > taskListCount)
            {
                BotClient.SendMessage(context.Update.Message.Chat, $"\nНомер задачи должен быть в допустимом диапазоне номеров задач!");
                return;
            }

            var taskList = _toDoServiceInstance.GetActiveByUserId(existingUser.UserId);
            var item = taskList[taskNumber - 1];

            string delInfo = $"Завершено: #{taskNumber}: \"{item.Name}\" - {item.CreatedAt} - {item.Id}\n";
            _toDoServiceInstance.MarkCompleted(item.Id);
            BotClient.SendMessage(context.Update.Message.Chat, delInfo);
        }
    }
}