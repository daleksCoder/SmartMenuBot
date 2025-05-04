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
    public class HelpCommand(ITelegramBotClient botClient) : IBotCommand
    {
        public string CommandText => "/help";

        private ITelegramBotClient BotClient { get; } = botClient;

        public bool CanExecute(CommandContext context)
        {
            string? messageText = context.Update.Message?.Text;
            return messageText != null && messageText.StartsWith(CommandText, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute(CommandContext context)
        {
            BotClient.SendMessage(context.Update.Message.Chat, $"\n{GetAvailableBotCommands()}");
        }

        private static string GetAvailableBotCommands()
        {
            string commandList =
                """                           
                Доступные команды:
                /start        * начало работы с ботом
                /help         * справка по доступным командам
                /addTask      * добавление задач в список
                /showTasks    * вывод активных задач
                /showAllTasks * вывод всех задач
                /completeTask * завершение задачи                              
                /removeTask   * удаление задачи из списка              
                /info         * информация о версии
                """;

            return commandList;
        }
    }
}