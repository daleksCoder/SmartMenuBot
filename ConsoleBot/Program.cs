using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Exceptions;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.DependencyInjection;
using SmartMenuBot.Core.Commands.Interfaces;
using SmartMenuBot.Core.Commands.Implementations;
using SmartMenuBot.TelegramBot;
using SmartMenuBot.Core.Services.Interfaces;
using SmartMenuBot.Core.Services.Domain;
using SmartMenuBot.Core.Services.Managers;
using SmartMenuBot.Core.Services.Infrastructure;

namespace SmartMenuBot
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                var services = new ServiceCollection();

                services.AddSingleton<IUserService, UserService>();
                services.AddSingleton<IToDoService, ToDoService>();                

                services.AddSingleton<ILimitsInputProvider, ConsoleLimitsInput>();
                services.AddSingleton<ITaskLimitsManager  , TaskLimitsManager>();

                services.AddSingleton<IBotCommand, StartCommand>();
                services.AddSingleton<IBotCommand, HelpCommand>();
                services.AddSingleton<IBotCommand, AddTaskCommand>();
                services.AddSingleton<IBotCommand, CompleteTaskCommand>();                
                services.AddSingleton<IBotCommand, RemoveTaskCommand>();
                services.AddSingleton<IBotCommand, ShowTasksCommand>();
                services.AddSingleton<IBotCommand, ShowAllTasksCommand>();
                services.AddSingleton<IBotCommand, InfoCommand>();

                services.AddSingleton<UpdateHandler>();
                services.AddSingleton<ITelegramBotClient, ConsoleBotClient>();

                var serviceProvider = services.BuildServiceProvider();

                var handler   = serviceProvider.GetRequiredService<UpdateHandler>();
                var botClient = serviceProvider.GetRequiredService<ITelegramBotClient>();                

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