using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Entities;
using SmartMenuBot.Core.Commands.Interfaces;
using SmartMenuBot.Core.Services.Interfaces;

namespace SmartMenuBot.Core.Commands.Implementations
{
    public class ShowAllTasksCommand(ITelegramBotClient botClient, IUserService userService, IToDoService toDoService) : IBotCommand
    {
        public string CommandText => "/showalltasks";

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

            int taskListCount = _toDoServiceInstance.GetAllByUserId(existingUser.UserId).Count;
            if (taskListCount == 0)
            {
                BotClient.SendMessage(context.Update.Message.Chat, $"\nСписок задач пуст");
                return;
            }

            var taskList = _toDoServiceInstance.GetAllByUserId(existingUser.UserId);
            ShowTasks(context.Update, taskList);
        }

        private void ShowTasks(Update update, IReadOnlyList<ToDoItem> taskList)
        {
            int i = 0;
            foreach (var item in taskList)
            {
                i++;
                BotClient.SendMessage(update.Message.Chat, $"Задача #{i}: {item.State} \"{item.Name}\" - {item.CreatedAt} - {item.Id}\n");
            }
        }
    }
}