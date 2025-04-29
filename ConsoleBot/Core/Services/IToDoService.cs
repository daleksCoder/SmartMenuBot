using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;
using SmartMenuBot.Core.Entities;

namespace SmartMenuBot.Core.Services
{
    public interface IToDoService : IBotServiceProvider
    {
        void ToDoServiceInit(ITelegramBotClient botClient, Update update);
        IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId);
        IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId);
        int GetActiveCountByUserId(Guid userId);
        int GetAllCountByUserId(Guid userId);
        ToDoItem Add(ToDoUser user, string name);
        void MarkCompleted(Guid id);
        void Delete(Guid id);
    }
}