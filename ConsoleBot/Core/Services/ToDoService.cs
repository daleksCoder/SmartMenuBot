using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Entities;
using SmartMenuBot.Core.Exceptions;

namespace SmartMenuBot.Core.Services
{
    public class ToDoService : IToDoService
    {
        private List<ToDoItem> Items { get; } = [];
        private int TaskCountLimit { get; set; }
        private int TaskLengthLimit { get; set; }
        public void ToDoServiceInit(ITelegramBotClient botClient, Update update)
        {
            TaskCountLimit = InitTaskCountLimit(botClient, update);
            TaskLengthLimit = InitTaskLengthLimit(botClient, update);
        }

        public ToDoItem Add(ToDoUser user, string name)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Наименование задачи не может быть пустым", nameof(name));

            if (Items.Count == TaskCountLimit)
                throw new TaskCountLimitException(TaskCountLimit);

            string trimmedName = name.Trim();
            if (trimmedName.Length > TaskLengthLimit)
                throw new TaskLengthLimitException(trimmedName.Length, TaskLengthLimit);

            if (Items.Any(item => item.Name.Equals(trimmedName, StringComparison.OrdinalIgnoreCase)))
                throw new DuplicateTaskException(trimmedName);

            var newItem = new ToDoItem(user, trimmedName);
            Items.Add(newItem);
            return newItem;
        }

        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            return Items.Where(item => item.User.UserId == userId).ToList().AsReadOnly();
        }

        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            return Items.Where(item => item.User.UserId == userId &&
                                     item.State == ToDoItemState.Active)
                       .ToList().AsReadOnly();
        }

        public int GetAllCountByUserId(Guid userId)
        {
            return Items.Where(item => item.User.UserId == userId).Count();
        }

        public int GetActiveCountByUserId(Guid userId)
        {
            return Items.Where(item => item.User.UserId == userId &&
                                     item.State == ToDoItemState.Active).Count();            
        }

        public void MarkCompleted(Guid id)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.Complete();
            }
        }

        public void Delete(Guid id)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        private static int InitTaskCountLimit(ITelegramBotClient botClient, Update update)
        {
            while (true)
            {
                botClient.SendMessage(update.Message.Chat, $"\nВведите максимально допустимое количество задач: ");
                string? input = Console.ReadLine();

                try
                {
                    return ParseAndValidateInt(input, 1, 100);
                }
                catch (ArgumentException ex)
                {
                    botClient.SendMessage(update.Message.Chat, ex.Message);
                }
            }
        }

        private static int InitTaskLengthLimit(ITelegramBotClient botClient, Update update)
        {
            while (true)
            {
                botClient.SendMessage(update.Message.Chat, $"\nВведите максимально допустимую длину задачи: ");
                string? input = Console.ReadLine();

                try
                {
                    return ParseAndValidateInt(input, 1, 100);
                }
                catch (ArgumentException ex)
                {
                    botClient.SendMessage(update.Message.Chat, ex.Message);
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
