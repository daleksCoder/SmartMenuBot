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
    public class StartCommand(ITelegramBotClient botClient, IUserService userService, ITaskLimitsManager limitsManager) : IBotCommand
    {
        public string CommandText => "/start";

        public bool CanExecute(CommandContext context)
        {
            string? messageText = context.Update.Message?.Text;
            return messageText != null && messageText.StartsWith(CommandText, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute(CommandContext context)
        {
            var existingUser = userService.GetUser(context.Update.Message.From.Id);
            if (existingUser != null)
            {
                botClient.SendMessage(
                    context.Update.Message.Chat,
                    "\nВы уже авторизовались ранее"
                );
                return;
            }

            var newUser = context.Update.Message.From;
            userService.RegisterUser(newUser.Id, newUser.Username);

            botClient.SendMessage(
                context.Update.Message.Chat,
                "Добро пожаловать в систему управления обогревом загородного дома!\n"
            );

            limitsManager.TryInitializeFromUserInput(context.Update);
        }
    }
}