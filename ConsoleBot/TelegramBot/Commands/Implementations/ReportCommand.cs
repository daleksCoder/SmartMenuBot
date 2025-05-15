using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Services.Domain;
using SmartMenuBot.Core.Services.Interfaces;
using SmartMenuBot.TelegramBot;
using SmartMenuBot.TelegramBot.Commands;
using SmartMenuBot.TelegramBot.Commands.Interfaces;

namespace SmartMenuBot.TelegramBot.Commands.Implementations
{
    public class ReportCommand(ITelegramBotClient botClient, IUserService userService, IToDoReportService reportService) : IBotCommand
    {
        public string CommandText => "/report";

        private const string ReportFormat = "Статистика по задачам на {0:dd.MM.yyyy HH:mm:ss}. Всего: {1}; Завершенных: {2}; Активных: {3}";

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

            var stats = reportService.GetUserStats(existingUser.UserId);
            var localTime = stats.generatedAt.ToLocalTime();

            var message = string.Format(ReportFormat,
                localTime,
                stats.total,
                stats.completed,
                stats.active);

            botClient.SendMessage(context.Update.Message.Chat, message);
        }
    }
}