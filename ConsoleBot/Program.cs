using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Commands;
using SmartMenuBot.Core.Exceptions;
using SmartMenuBot.Infrastructure.TelegramBot;
using static System.Net.Mime.MediaTypeNames;

namespace SmartMenuBot
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                var commands = new List<IBotCommand>
                {
                new StartCommand(),
                new AddTaskCommand(),
                new CompleteTaskCommand(),
                new HelpCommand(),
                new RemoveTaskCommand(),
                new ShowTasksCommand(),
                new ShowAllTasksCommand(),
                new InfoCommand(),
                new ExitCommand()
                };                

                var handler = new UpdateHandler(commands);
                var botClient = new ConsoleBotClient();
                botClient.StartReceiving(handler);
            }
            catch (Exception ex)
            {
                Console.WriteLine("В приложении произошла непредвиденная ошибка:");
                Console.WriteLine($"Тип: {ex.GetType()}");
                Console.WriteLine($"Сообщение: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                if (ex.InnerException != null)
                    Console.WriteLine($"InnerException: {ex.InnerException}");
            }
        }
    }
}
