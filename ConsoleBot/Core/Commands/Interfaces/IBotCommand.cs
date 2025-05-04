using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.TelegramBot;

namespace SmartMenuBot.Core.Commands.Interfaces
{
    public interface IBotCommand
    {
        string CommandText { get; }

        bool CanExecute(CommandContext context);

        void Execute(CommandContext context);
    }
}