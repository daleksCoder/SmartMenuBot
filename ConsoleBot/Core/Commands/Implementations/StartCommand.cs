using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Commands.Interfaces;
using SmartMenuBot.Core.Services.Interfaces;
using SmartMenuBot.TelegramBot;

namespace SmartMenuBot.Core.Commands.Implementations
{
    public class StartCommand(ITelegramBotClient botClient, IUserService userService, ITaskLimitsManager limitsManager) : IBotCommand
    {
        public string CommandText => "/start";

        private ITelegramBotClient BotClient { get; } = botClient;

        private readonly IUserService _userServiceInstance = userService;

        private readonly ITaskLimitsManager _limitsManager = limitsManager;

        public bool CanExecute(CommandContext context)
        {
            string? messageText = context.Update.Message?.Text;
            return messageText != null && messageText.StartsWith(CommandText, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute(CommandContext context)
        {
            var existingUser = _userServiceInstance.GetUser(context.Update.Message.From.Id);
            if (existingUser != null)
            {
                BotClient.SendMessage(
                    context.Update.Message.Chat,
                    "\nВы уже авторизовались ранее"
                );
                return;
            }

            var newUser = context.Update.Message.From;
            _userServiceInstance.RegisterUser(newUser.Id, newUser.Username);

            BotClient.SendMessage(
                context.Update.Message.Chat,
                "Добро пожаловать в систему управления обогревом загородного дома!\n"
            );

            _limitsManager.TryInitializeFromUserInput(context.Update);
        }
    }
}