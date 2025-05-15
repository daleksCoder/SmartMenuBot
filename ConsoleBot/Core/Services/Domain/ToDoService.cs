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
using SmartMenuBot.Core.DataAccess;

namespace SmartMenuBot.Core.Services.Domain
{
    public class ToDoService(ITaskLimitsManager limitsManager, IToDoRepository toDoRepository) : IToDoService
    {
        public ToDoItem Add(ToDoUser user, string name)
        {
            ArgumentNullException.ThrowIfNull(user);

            var limits = limitsManager.GetLimits();
            if (!limits.IsInitialized)
                throw new InvalidOperationException("Лимиты не установлены. Вызовите /start.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Наименование задачи не может быть пустым", nameof(name));

            if (toDoRepository.CountActive(user.UserId) >= limits.Count)
                throw new TaskCountLimitException(limits.Count);

            string trimmedName = name.Trim();
            if (trimmedName.Length > limits.Length)
                throw new TaskLengthLimitException(trimmedName.Length, limits.Length);

            if (toDoRepository.ExistsByName(user.UserId, trimmedName))
                throw new DuplicateTaskException(trimmedName);

            var newItem = new ToDoItem(user, trimmedName);
            toDoRepository.Add(newItem);
            return newItem;
        }

        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            return toDoRepository.GetAllByUserId(userId);
        }

        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            return toDoRepository.GetActiveByUserId(userId);
        }

        public void MarkCompleted(Guid id)
        {
            var item = toDoRepository.Get(id);
            item?.Complete();
        }

        public void Delete(Guid id)
        {
            toDoRepository.Delete(id);
        }

        public IReadOnlyList<ToDoItem> Find(ToDoUser user, string namePrefix) 
        {
            return toDoRepository.Find(user.UserId, (ToDoItem item) => item.Name.StartsWith(namePrefix, StringComparison.OrdinalIgnoreCase));
        }        
    }
}