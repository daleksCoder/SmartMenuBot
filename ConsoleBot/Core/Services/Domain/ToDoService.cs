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
using SmartMenuBot.Core.Services.Interfaces;

namespace SmartMenuBot.Core.Services.Domain
{
    public class ToDoService(ITaskLimitsManager limitsManager) : IToDoService
    {
        private readonly List<ToDoItem> _items = [];

        private readonly ITaskLimitsManager _limitsManager = limitsManager;

        public ToDoItem Add(ToDoUser user, string name)
        {
            ArgumentNullException.ThrowIfNull(user);

            var limits = _limitsManager.GetLimits();
            if (!limits.IsInitialized)
                throw new InvalidOperationException("Лимиты не установлены. Вызовите /start.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Наименование задачи не может быть пустым", nameof(name));

            if (_items.Count >= limits.Count)
                throw new TaskCountLimitException(limits.Count);

            string trimmedName = name.Trim();
            if (trimmedName.Length > limits.Length)
                throw new TaskLengthLimitException(trimmedName.Length, limits.Length);

            if (_items.Any(item => item.Name.Equals(trimmedName, StringComparison.OrdinalIgnoreCase)))
                throw new DuplicateTaskException(trimmedName);

            var newItem = new ToDoItem(user, trimmedName);
            _items.Add(newItem);
            return newItem;
        }

        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            return _items.Where(item => item.User.UserId == userId).ToList().AsReadOnly();
        }

        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            return _items.Where(item => item.User.UserId == userId &&
                                     item.State == ToDoItemState.Active)
                       .ToList().AsReadOnly();
        }

        public void MarkCompleted(Guid id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.Complete();
            }
        }

        public void Delete(Guid id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _items.Remove(item);
            }
        }
    }
}