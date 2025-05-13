using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Services.Interfaces;
using SmartMenuBot.TelegramBot.Commands;
using SmartMenuBot.TelegramBot.Commands.Interfaces;

namespace SmartMenuBot.TelegramBot.Commands.Implementations
{
    public class CompleteTaskCommand(ITelegramBotClient botClient, IUserService userService, IToDoService toDoService) : IBotCommand
    {
        public string CommandText => "/completetask";

        public bool CanExecute(CommandContext context)
        {
            string? messageText = context.Update.Message?.Text;
            return messageText != null && messageText.StartsWith(CommandText, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute(CommandContext context)
        {
            var existingUser = userService.GetUser(context.Update.Message.From.Id);
            if (existingUser == null)
            {
                botClient.SendMessage(
                    context.Update.Message.Chat,
                    "\nДля использования этой функции необходимо авторизоваться командой /start"
                );
                return;
            }

            int taskListCount = toDoService.GetActiveByUserId(existingUser.UserId).Count;
            if (taskListCount == 0)
            {
                botClient.SendMessage(context.Update.Message.Chat, $"\nСписок задач пуст");
                return;
            }

            var input = context.Update.Message.Text.Replace(CommandText, "", StringComparison.OrdinalIgnoreCase).Trim();
            if (string.IsNullOrEmpty(input))
            {
                botClient.SendMessage(context.Update.Message.Chat, $"\nИдентификатор задачи не может быть пустым");
                return;
            }

            if (!Guid.TryParse(input, out Guid taskId) || taskId == Guid.Empty)
            {
                botClient.SendMessage(context.Update.Message.Chat, $"\nИдентификатор задачи должен быть корректным GUID");
                return;
            }

            toDoService.MarkCompleted(taskId);
        }
    }
}