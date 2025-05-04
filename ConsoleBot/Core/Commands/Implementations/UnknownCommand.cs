using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Commands.Interfaces;
using SmartMenuBot.TelegramBot;

namespace SmartMenuBot.Core.Commands.Implementations
{
    public class UnknownCommand(ITelegramBotClient botClient) : IBotCommand
    {
        public string CommandText => string.Empty;

        private ITelegramBotClient BotClient { get; } = botClient;

        public bool CanExecute(CommandContext context)
        {
            string? messageText = context.Update.Message?.Text;
            return messageText != null && messageText.StartsWith(CommandText, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute(CommandContext context)
        {
            BotClient.SendMessage(context.Update.Message.Chat, $"\nНеизвестная команда");
        }
    }
}