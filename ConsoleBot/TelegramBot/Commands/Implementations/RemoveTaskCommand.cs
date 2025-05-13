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
    public class RemoveTaskCommand(ITelegramBotClient botClient, IUserService userService, IToDoService toDoService) : IBotCommand
    {
        public string CommandText => "/removetask";

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

            var taskStrNumber = context.Update.Message.Text.Replace(CommandText, "", StringComparison.OrdinalIgnoreCase).Trim();
            if (string.IsNullOrEmpty(taskStrNumber))
            {
                botClient.SendMessage(context.Update.Message.Chat, $"\nНомер задачи не может быть пустым");
                return;
            }

            if (!int.TryParse(taskStrNumber, out int taskNumber))
            {
                botClient.SendMessage(context.Update.Message.Chat, $"\nНомер задачи должен быть числом в диапазоне номеров задач!");
                return;
            }

            if (taskNumber < 1 || taskNumber > taskListCount)
            {
                botClient.SendMessage(context.Update.Message.Chat, $"\nНомер задачи должен быть в допустимом диапазоне номеров задач!");
                return;
            }

            var taskList = toDoService.GetAllByUserId(existingUser.UserId);
            var item = taskList[taskNumber - 1];

            string delInfo = $"Удалено: #{taskNumber}: \"{item.Name}\" - {item.CreatedAt} - {item.Id}\n";
            toDoService.Delete(item.Id);
            botClient.SendMessage(context.Update.Message.Chat, delInfo);
        }
    }
}