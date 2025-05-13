using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot.Types;
using System.Xml.Linq;
using SmartMenuBot.Core.DataAccess;
using SmartMenuBot.Core.Entities;
using SmartMenuBot.Core.Exceptions;

namespace SmartMenuBot.Infrastructure.DataAccess
{
    public class InMemoryToDoRepository : IToDoRepository
    {
        private readonly List<ToDoItem> _items = [];

        public void Add(ToDoItem item)
        {
            _items.Add(item);
        }

        public int CountActive(Guid userId)
        {
            return _items.Where(item => item.User.UserId == userId &&
                                     item.State == ToDoItemState.Active)
                       .ToList().Count;
        }

        public void Delete(Guid id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        public bool ExistsByName(Guid userId, string name)
        {
            return _items.Any(item => item.User.UserId == userId &&
                                     item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public ToDoItem? Get(Guid id)
        {
            return _items.FirstOrDefault(item => item.Id == id);
        }

        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            return _items.Where(item => item.User.UserId == userId &&
                                     item.State == ToDoItemState.Active)
                       .ToList().AsReadOnly();
        }

        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            return _items.Where(item => item.User.UserId == userId).ToList().AsReadOnly();
        }

        public IReadOnlyList<ToDoItem> Find(Guid userId, Func<ToDoItem, bool> predicate) 
        {
            return _items.Where(item => item.User.UserId == userId &&
                                     predicate(item)).ToList().AsReadOnly();
        }

        public void Update(ToDoItem item)
        {
            throw new NotImplementedException();
        }
    }
}