using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Services.Domain;
using SmartMenuBot.TelegramBot;
using SmartMenuBot.TelegramBot.Commands;
using SmartMenuBot.TelegramBot.Commands.Interfaces;

namespace SmartMenuBot.TelegramBot.Commands.Implementations
{
    public class InfoCommand(ITelegramBotClient botClient) : IBotCommand
    {
        public string CommandText => "/info";

        public bool CanExecute(CommandContext context)
        {
            string? messageText = context.Update.Message?.Text;
            return messageText != null && messageText.StartsWith(CommandText, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute(CommandContext context)
        {
            VersionInfoService.GetVersionInfo(out string version, out DateTime creationDate);
            botClient.SendMessage(context.Update.Message.Chat, $"System version: {version}, created on: {creationDate:dd.MM.yyyy}");
        }
    }
}