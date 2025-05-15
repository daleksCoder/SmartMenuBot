using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Entities;
using SmartMenuBot.Core.Services.Interfaces;
using SmartMenuBot.TelegramBot;
using SmartMenuBot.TelegramBot.Commands;
using SmartMenuBot.TelegramBot.Commands.Interfaces;

namespace SmartMenuBot.TelegramBot.Commands.Implementations
{
    public class FindCommand(ITelegramBotClient botClient, IUserService userService, IToDoService toDoService) : IBotCommand
    {
        public string CommandText => "/find";

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

            int taskListCount = toDoService.GetAllByUserId(existingUser.UserId).Count;
            if (taskListCount == 0)
            {
                botClient.SendMessage(context.Update.Message.Chat, $"\nСписок задач пуст");
                return;
            }

            var searchQuery = context.Update.Message.Text.Replace(CommandText, "", StringComparison.OrdinalIgnoreCase).Trim();
            if (string.IsNullOrEmpty(searchQuery))
            {
                botClient.SendMessage(context.Update.Message.Chat, $"\nНе задана строка поиска");
                return;
            }

            var taskList = toDoService.Find(existingUser, searchQuery);
            ShowTasks(context.Update, taskList);
        }

        private void ShowTasks(Update update, IReadOnlyList<ToDoItem> taskList)
        {
            if (taskList.Count == 0)
            {
                botClient.SendMessage(update.Message.Chat, $"\nСовпадения не найдены");
                return;
            }

            int i = 0;
            foreach (var item in taskList)
            {
                i++;
                botClient.SendMessage(update.Message.Chat, $"Задача #{i}: \"{item.Name}\" - {item.CreatedAt} - {item.Id}\n");
            }
        }
    }
}