using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartMenuBot.Core.Entities;

namespace SmartMenuBot.Core.DataAccess
{
    public interface IToDoRepository
    {
        void Add(ToDoItem item);

        int CountActive(Guid userId);

        void Delete(Guid id);

        bool ExistsByName(Guid userId, string name);

        ToDoItem? Get(Guid id);

        IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId);

        IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId);

        IReadOnlyList<ToDoItem> Find(Guid userId, Func<ToDoItem, bool> predicate);

        void Update(ToDoItem item);       
    }
}