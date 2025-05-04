using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Commands.Interfaces;
using SmartMenuBot.Core.Services.Interfaces;
using SmartMenuBot.TelegramBot;

namespace SmartMenuBot.Core.Commands.Implementations
{
    public class AddTaskCommand(ITelegramBotClient botClient, IUserService userService, IToDoService toDoService) : IBotCommand
    {
        public string CommandText => "/addtask";

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

            var taskName = context.Update.Message.Text.Replace(CommandText, "", StringComparison.OrdinalIgnoreCase).Trim();
            if (string.IsNullOrEmpty(taskName))
            {
                BotClient.SendMessage(context.Update.Message.Chat, $"\nНаименование задачи не может быть пустым");
                return;
            }

            var item = _toDoServiceInstance.Add(existingUser, taskName);

            string addInfo = $"Добавлена задача: \"{item.Name}\" - {item.CreatedAt} - {item.Id}\n";
            BotClient.SendMessage(context.Update.Message.Chat, addInfo);
        }
    }
}