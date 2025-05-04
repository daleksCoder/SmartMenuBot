using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot.Types;
using SmartMenuBot.Core.Entities;

namespace SmartMenuBot.Core.Services.Interfaces
{
    public interface ITaskLimitsManager
    {
        bool TryInitializeFromUserInput(Update update);

        TaskLimits GetLimits();
    }
}