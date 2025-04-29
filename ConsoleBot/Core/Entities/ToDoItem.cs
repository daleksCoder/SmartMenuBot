using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenuBot.Core.Entities
{
    public enum ToDoItemState
    {
        Active,
        Completed
    }

    public class ToDoItem(ToDoUser user, string name)
    {
        public Guid Id { get; } = Guid.NewGuid();
        public ToDoUser User { get; set; } = user ?? throw new ArgumentNullException(nameof(user));
        public string Name { get; set; } = name ?? throw new ArgumentNullException(nameof(name));
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public ToDoItemState State { get; private set; } = ToDoItemState.Active;
        public DateTime? StateChangedAt { get; private set; }

        public void ChangeState(ToDoItemState newState)
        {
            if (State == newState)
                return;

            State = newState;
            StateChangedAt = DateTime.UtcNow;
        }

        public void Complete() => ChangeState(ToDoItemState.Completed);

        public void Activate() => ChangeState(ToDoItemState.Active);
    }
}
