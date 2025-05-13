using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.TelegramBot;
using SmartMenuBot.TelegramBot.Commands;
using SmartMenuBot.TelegramBot.Commands.Interfaces;

namespace SmartMenuBot.TelegramBot.Commands.Implementations
{
    public class HelpCommand(ITelegramBotClient botClient) : IBotCommand
    {
        public string CommandText => "/help";

        public bool CanExecute(CommandContext context)
        {
            string? messageText = context.Update.Message?.Text;
            return messageText != null && messageText.StartsWith(CommandText, StringComparison.OrdinalIgnoreCase);
        }

        public void Execute(CommandContext context)
        {
            botClient.SendMessage(context.Update.Message.Chat, $"\n{GetAvailableBotCommands()}");
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
                /report       * отчет по статусу задач
                /find         * фильтр задач по строке поиска
                /info         * информация о версии
                """;

            return commandList;
        }
    }
}