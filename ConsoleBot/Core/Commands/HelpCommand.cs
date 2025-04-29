using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Infrastructure.TelegramBot;

namespace SmartMenuBot.Core.Commands
{
    public class HelpCommand : IBotCommand
    {
        public string CommandText => "/help";

        public bool CanExecute(CommandContext context)
        {
            string? messageText = context.Update.Message?.Text;
            return messageText != null && messageText.StartsWith(CommandText, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute(CommandContext context)
        {
            context.BotClient.SendMessage(context.Update.Message.Chat, $"\n\r{GetAvailableBotCommands()}");
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
                /exit         * завершение работы с ботом
                """;

            return commandList;
        }
    }
}
