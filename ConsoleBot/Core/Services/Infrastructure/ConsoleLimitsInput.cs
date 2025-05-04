using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Entities;
using SmartMenuBot.Core.Services.Interfaces;

namespace SmartMenuBot.Core.Services.Infrastructure
{
    public class ConsoleLimitsInput(ITelegramBotClient botClient) : ILimitsInputProvider
    {
        private ITelegramBotClient BotClient { get; } = botClient;

        public TaskLimits GetLimitsFromUser(Update update)
        {
            int count = GetValidatedInput(
                "Введите максимальное количество задач (1-100): ",
                1, 100, update);

            int length = GetValidatedInput(
                "Введите максимальную длину задачи (1-100): ",
                1, 100, update);

            return new TaskLimits(count, length);
        }

        private int GetValidatedInput(string prompt, int min, int max, Update update)
        {
            while (true)
            {
                BotClient.SendMessage(update.Message.Chat, prompt);
                string? input = Console.ReadLine();

                try
                {
                    return ParseAndValidateInt(input, min, max);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static int ParseAndValidateInt(string? str, int min, int max)
        {
            if (!int.TryParse(str, out int result))
                throw new ArgumentException($"\nВведите корректное целое число в диапазоне от {min} до {max}");

            if (result < min || result > max)
                throw new ArgumentException($"\nЧисло должно быть в диапазоне от {min} до {max}");

            return result;
        }
    }
}