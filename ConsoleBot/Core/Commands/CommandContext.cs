using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Services;
using SmartMenuBot.TelegramBot;

namespace SmartMenuBot.Core.Commands
{
    public class CommandContext(Update update)
    {
        public Update Update { get; } = update;
    }
}