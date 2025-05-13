using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Services.Interfaces;
using SmartMenuBot.TelegramBot;
using SmartMenuBot.TelegramBot.Commands;
using SmartMenuBot.TelegramBot.Commands.Interfaces;

namespace SmartMenuBot.TelegramBot.Commands.Implementations
{
    public class AddTaskCommand(ITelegramBotClient botClient, IUserService userService, IToDoService toDoService) : IBotCommand
    {
        public string CommandText => "/addtask";

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

            var taskName = context.Update.Message.Text.Replace(CommandText, "", StringComparison.OrdinalIgnoreCase).Trim();
            if (string.IsNullOrEmpty(taskName))
            {
                botClient.SendMessage(context.Update.Message.Chat, $"\nНаименование задачи не может быть пустым");
                return;
            }

            var item = toDoService.Add(existingUser, taskName);

            string addInfo = $"Добавлена задача: \"{item.Name}\" - {item.CreatedAt} - {item.Id}\n";
            botClient.SendMessage(context.Update.Message.Chat, addInfo);
        }
    }
}