using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Commands.Interfaces;
using SmartMenuBot.Core.Services.Domain;
using SmartMenuBot.TelegramBot;

namespace SmartMenuBot.Core.Commands.Implementations
{
    public class InfoCommand(ITelegramBotClient botClient) : IBotCommand
    {
        public string CommandText => "/info";

        private ITelegramBotClient BotClient { get; } = botClient;

        public bool CanExecute(CommandContext context)
        {
            string? messageText = context.Update.Message?.Text;
            return messageText != null && messageText.StartsWith(CommandText, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute(CommandContext context)
        {
            VersionInfoService.GetVersionInfo(out string version, out DateTime creationDate);
            BotClient.SendMessage(context.Update.Message.Chat, $"System version: {version}, created on: {creationDate:dd.MM.yyyy}");
        }
    }
}