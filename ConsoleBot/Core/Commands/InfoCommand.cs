using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Services;
using SmartMenuBot.Infrastructure.TelegramBot;

namespace SmartMenuBot.Core.Commands
{
    public class InfoCommand : IBotCommand
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
            context.BotClient.SendMessage(context.Update.Message.Chat, $"System version: {version}, created on: {creationDate:dd.MM.yyyy}");
        }
    }
}