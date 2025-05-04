using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Entities;
using SmartMenuBot.Core.Services.Interfaces;

namespace SmartMenuBot.Core.Services.Managers
{
    public class TaskLimitsManager(ILimitsInputProvider inputProvider) : ITaskLimitsManager
    {
        private TaskLimits Limits { get; set; } = TaskLimits.Uninitialized;
        private readonly ILimitsInputProvider _inputProvider = inputProvider;

        public bool TryInitializeFromUserInput(Update update)
        {
            if (Limits.IsInitialized)
                return false;

            Limits = _inputProvider.GetLimitsFromUser(update);

            return true;
        }

        public TaskLimits GetLimits() => Limits;
    }
}